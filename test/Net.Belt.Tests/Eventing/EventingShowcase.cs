using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

using Dumpify;

using Net.Belt.Eventing;
// ReSharper disable UnusedMember.Local, UnusedAutoPropertyAccessor.Local, NotAccessedPositionalProperty.Local

namespace Net.Belt.Tests.Eventing;

[TestFixture, NUnit.Framework.Category("showcase")]
public class EventingShowcase
{
	class DataEventArgs : EventArgs
	{
		public required string S { get; set; }
		public int I { get; set; }
	}

	// no need to inherit from EventArgs anymore
	record DataArgs(string S, int I);

	event EventHandler<DataEventArgs>? ClassicEvent;
	event EventHandler<DataArgs>? ModernEvent;

	// encapsulate the firing of the event and the event args creation with a method
	protected virtual void OnClassicEvent(string s, int i) =>
		ClassicEvent.Raise(this, new DataEventArgs { S = s, I = i });

	// not so useful anymore with nullable conditional operator
	protected virtual void OnModernEvent(string s, int i) =>
		ModernEvent?.Invoke(this, new DataArgs(s, i));

	[Test]
	public void Classic()
	{
		OnClassicEvent("classic", 1);
		OnModernEvent("modern", 1);
		// nothing happens as there are no subscriptions

		// listen and one will hear
		ClassicEvent += (_, e) => e.Dump("classic event fired");
		ModernEvent += (_, e) => e.Dump("modern event fired");

		// event argument will be printed
		OnClassicEvent("classic", 2);
		ClassicEvent.Raise(this, new DataEventArgs { S = "classic", I = 3 });
		OnModernEvent("modern", 2);
		// no .Raise() for non event args
		ModernEvent?.Invoke(this, new DataArgs("modern", 3));
	}

	event EventHandler<ValueEventArgs<decimal>>? Single;
	private event EventHandler<MultiArgs<string, int>>? Multi;

	private static void _printInfo<T>(object? sender, T e) => new { sender, e }.Dump("event info");

	[Test]
	public void ValueArgs()
	{
		// single-value (event args) constructor created
		Single += _printInfo;
		Single.Raise(this, new ValueEventArgs<decimal>(1.1m));

		// multi-value (not event args) factory created
		Multi += _printInfo;
		Multi.Invoke(this, Args.Value("foo", -1));
	}

	private event EventHandler<MutableArgs<decimal>>? Mutable;

	[Test]
	public void MutableArgs()
	{
		Mutable += (sender, e) =>
		{
			e.Value = decimal.MinusOne;
			_printInfo(sender, e);
		};
		// mutated value (-1) will be printed instead of the initial value (33)
		Mutable.Invoke(this, Args.Mutable(33m));
	}

	event EventHandler<CancelArgs>? Cancelable;
	event EventHandler<ValueCancelArgs<int>>? ValueCancelable;

	[Test]
	public void CancelableArgs()
	{
		// cancel args
		Cancelable += (sender, e) =>
		{
			_printInfo(sender, e);
			e.Cancel();
		};
		Cancelable += _printInfo;

		// first handler prints the non-canceled args, second handler prints the canceled args
		Cancelable.Invoke(this, new CancelArgs());

		// cancel args if negative
		ValueCancelable += (sender, e) =>
		{
			_printInfo(sender, e);
			if (e.Value < 0) e.Cancel();
		};
		ValueCancelable += _printInfo;
		// first handler prints the non-canceled args, second handler prints the canceled args as the value is negative
		ValueCancelable?.Invoke(this, Args.Cancel(-1));
	}

	[Test]
	[SuppressMessage("ReSharper", "UnusedVariable")]
	public void Interfaces()
	{
		IValueArgs<string> value = Args.Value("single");
		IMultiArgs<string, bool> multi = Args.Value("multi", true);
		IMutableArgs<decimal> mutable = Args.Mutable(.2m);
		var cancel = Args.Cancel(-7);
		IValueArgs<int> val = cancel;
		ICancelArgs iCancel = cancel;
	}

	class Chatty : INotifyPropertyChanging, INotifyPropertyChanged
	{
		public event PropertyChangingEventHandler? PropertyChanging;
		public event PropertyChangedEventHandler? PropertyChanged;

		public int Barebones
		{
			get;
			set
			{
				// no help
				PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(Barebones)));
				field = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Barebones)));
			}
		}
		
		public string? Raising
		{
			get;
			set
			{
				// quite a bit of help
				PropertyChanging.Raise(this);
				field = value;
				PropertyChanged.Raise(this);
			}
		}

		public string? Extended
		{
			get;
			set
			{
				string? old = field;
				// add values to the event (and ability to cancel)
				bool cancel = this.Notify(PropertyChanging, old, value);
				if (!cancel)
				{
					field = value;
					this.Notify(PropertyChanged, old, value);	
				}
			}
		}
	}

	[Test]
	public void Observations()
	{
		var chatty = new Chatty();
		chatty.PropertyChanging += _printInfo;
		chatty.PropertyChanged += _printInfo;

		// print just property name in the changing and changed events
		chatty.Barebones = 42;
		
		// print just property name in the changing and changed events
		chatty.Raising = "lol";
		
		// prints old and new values
		chatty.Extended = "value_1";
		chatty.PropertyChanging += (_, e) =>
		{
			// nasty downcasting
			var cancelable = (ICancelArgs)e;
			cancelable.Cancel();
		};
		// prevents changed event to be raised
		chatty.Extended = "value_2";

		using (chatty.Observing((_, e) => e.Dump("observing")))
		{
			chatty.Extended = "value_3";
		}
	}

	private event ChainedEventHandler<ChainedArgs>? Chained; 
	[Test]
	public void BreakChainOfHandling()
	{
		Chained += (_, e) => e.Dump("unhandled");
		Chained += (_, e) =>
		{
			e.Handled = true;
			e.Dump("handled");
		};
		Chained += (_, e) => e.Dump("not executed");

		// prints args unhandled and handled (third handler is not executed
		bool wasHandled = Chained.Raise(this, new ChainedArgs());
		wasHandled.Dump("handled?");
	}

	private event ChainedEventHandler<MutableArgs<int>, int>? Abortable;
	[Test]
	public void AbortChainOfHandling()
	{
		Abortable += (_, e) =>
		{
			e.Dump("first");
			return e.Value = 1;
		};
		Abortable += (_, e) =>
		{
			e.Dump("second");
			return e.Value = 2;
		};
		Abortable += (_, e) =>
		{
			e.Dump("third");
			return e.Value = 3;
		};

		// prints arg (0 and 1) for first and second handlers
		bool wasAborted = Abortable.RaiseUntil(this, new MutableArgs<int>(0), _divisibleByTwo);
		wasAborted.Dump("aborted?");
		bool _divisibleByTwo(int i) => i % 2 == 0;
	}
}