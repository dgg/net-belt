using System.ComponentModel;
using System.Runtime.CompilerServices;

using Dumpify;

using Net.Belt.Eventing;

namespace Net.Belt.Tests.Eventing;

[TestFixture, NUnit.Framework.Category("showcase")]
public class EventingShowcase
{
	class EventDataArgs : EventArgs
	{
		public string S { get; set; }
		public int I { get; set; }
	}
	
	// no need to inherit from EventArgs anymore
	record EventData(string S, int I);
	
	class WithEvents
	{
		public event EventHandler<EventDataArgs>? ClassicEvent;
		public event EventHandler<EventData>? ModernEvent;
		// encapsulate the creation of the args
		protected virtual void OnTheEvent(string s, int i) =>
			ClassicEvent?.Invoke(this, new EventDataArgs { S = s, I = i });

		public void FiringClassic() => OnTheEvent("s", 1);
		public void FiringModern() => ModernEvent?.Invoke(this, new EventData("SS", 2));
		public void RaisingClassic() => ClassicEvent.Raise(this, new EventDataArgs { S = "S", I = 2 });
	}
	
	[Test]
	public void Classic()
	{
		var withEvents = new WithEvents();
		// nothing happens as there are no subscriptions
		withEvents.FiringClassic(); 

		withEvents.ClassicEvent += (_, e) =>
		{
			e.Dump("event fired");
		};
		// event argument will be printed
		withEvents.FiringClassic();
		withEvents.RaisingClassic();
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
				field = value;
				OnPropertyChanged(nameof(Barebones));
			}
		}
		
		public string Extended
		{
			get;
			set
			{
				this.Notify(PropertyChanging, field, value);
				field = value;
				this.Notify(PropertyChanged, field, value);
			}
		}

		public string Raising
		{
			get;
			set
			{
				PropertyChanging.Raise(this, nameof(Raising));
				field = value;
				PropertyChanged.Raise(this);
			}
		}

		protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
		{
			if (EqualityComparer<T>.Default.Equals(field, value)) return false;
			field = value;
			OnPropertyChanged(propertyName);
			return true;
		}
	}

	[Test]
	public void Observations()
	{
		var chatty = new Chatty();
		chatty.PropertyChanging += (_, e) => e.Dump("propertyChanging");
		chatty.PropertyChanged += (_, e) => e.Dump("propertyChanged");

		//chatty.Barebones = 42;
		//chatty.Extended = "42";
		chatty.Raising = "lol";

		using (chatty.Observing((_, e) => e.Dump("observing")))
		{
			//chatty.Extended = "hola";
		}
	}
}