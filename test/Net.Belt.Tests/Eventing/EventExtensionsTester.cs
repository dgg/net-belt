using Net.Belt.Eventing;
using Net.Belt.Tests.Eventing.Support;

#pragma warning disable CS0067 // Event is never used

namespace Net.Belt.Tests.Eventing;

[TestFixture]
public class EventExtensionsTester
{
	#region subject

	private EventHandler<ValueEventArgs<int>>? _complexEvent;

	private readonly Lock _padlock = new();

	public event EventHandler<ValueEventArgs<int>>? ComplexEvent
	{
		add
		{
			lock (_padlock)
			{
				_complexEvent += value;
			}
		}
		remove
		{
			lock (_padlock)
			{
				_complexEvent -= value;
			}
		}
	}

	private EventHandler? _simpleEvent;

	public event EventHandler? SimpleEvent
	{
		add
		{
			lock (_padlock)
			{
				_simpleEvent += value;
			}
		}
		remove
		{
			lock (_padlock)
			{
				_simpleEvent -= value;
			}
		}
	}

	public event EventHandler<ValueEventArgs<string>>? NoAccessorEvent;

	private static event ChainedEventHandler<ChainedArgs>? Chained;

	internal class EventChainSubject
	{
		internal event ChainedEventHandler<ChainedArgs>? Event;
		internal bool OnChainedEvent(ChainedArgs args) => Event.Raise(this, args);
	}

	private static event ChainedEventHandler<MutableArgs<string>, string>? Abortable;

	#endregion

	#region Raise

	[Test]
	public void Raise_NoSubscribers_NoException()
	{
		_simpleEvent = null;
		Assert.That(() => _simpleEvent.Raise(this, EventArgs.Empty), Throws.Nothing);
	}

	[Test]
	public void Raise_EventsFired_EmptyEventArgs()
	{
		int numberOfEventsFired = 0;

		SimpleEvent += (_, _) => numberOfEventsFired++;
		SimpleEvent += (_, _) => numberOfEventsFired++;

		_simpleEvent.Raise(this, EventArgs.Empty);

		Assert.That(numberOfEventsFired, Is.EqualTo(2));
	}

	[Test]
	public void Raise_EventsFired_NonEmptyEventArgs()
	{
		int eventResult = 0;
		ComplexEvent += (_, e) => eventResult = e.Value;

		_complexEvent.Raise(this, new ValueEventArgs<int>(2));

		Assert.That(eventResult, Is.EqualTo(2));
	}

	[Test]
	public void Raise_NoAccessorEvent_EventFired()
	{
		string eventValue = string.Empty;
		NoAccessorEvent += (_, e) => eventValue = e.Value;

		NoAccessorEvent.Raise(this, new ValueEventArgs<string>("asd"));

		Assert.That(eventValue, Is.EqualTo("asd"));
	}

	[Test]
	public void RaiseExtension_NoSubscribers_NoException()
	{
		_simpleEvent = null;
		Assert.That(() => _simpleEvent.Raise(this, EventArgs.Empty), Throws.Nothing);
	}

	[Test]
	public void Raise_PropertyChangeExtension_EventFired()
	{
		var raiser = new NotifySubject();
		string changed = string.Empty;
		raiser.PropertyChanged += (_, e) => { changed = e.PropertyName!; };

		// act
		raiser.D = 3m;

		Assert.That(changed, Is.EqualTo("D"));
	}

	#endregion

	#region Notify

	#region property changing

	[Test]
	public void Notify_Changing_NoValues()
	{
		var subject = new NotifySubject();
		string propertyChangingName = string.Empty;
		subject.PropertyChanging += (_, args) => propertyChangingName = args.PropertyName!;

		// act
		subject.S = "2";
		Assert.That(propertyChangingName, Is.EqualTo("S"));

		// act
		subject.I = 2;
		Assert.That(propertyChangingName, Is.EqualTo("I"));
	}

	[Test]
	public void Notify_Changing_Values()
	{
		var subject = new NotifySubject { I = 1 };

		string propertyChangingName = string.Empty;
		int oldValue = 0, newValue = 0;
		subject.PropertyChanging += (_, args) =>
		{
			// nasty casting due to inflexibility of generics
			var extended = (PropertyValueChangingEventArgs<int>)args;
			propertyChangingName = extended.PropertyName!;
			oldValue = extended.OldValue;
			newValue = extended.NewValue;
		};

		// act
		subject.I = 2;

		Assert.That(propertyChangingName, Is.EqualTo("I"));
		Assert.That(oldValue, Is.EqualTo(1));
		Assert.That(newValue, Is.EqualTo(2));
	}

	[Test]
	public void Notify_HowToCancel_OnProperlyImplementedProperties()
	{
		var subject = new NotifySubject { I = 1, F = 2.0f };

		subject.PropertyChanging += (_, args) =>
		{
			if (args is ICancelArgs cancellable) cancellable.Cancel();
		};

		// act
		subject.I = 2;
		Assert.That(subject.I, Is.EqualTo(2),
			"'I' does does not obey cancellation rules");

		// act
		subject.F = 3.0f;
		Assert.That(subject.F, Is.EqualTo(2.0f),
			"'F' does obey cancellation rules and always cancelled, so the new value will never be set");
	}

	#endregion

	#region property changed

	[Test]
	public void Notify_Changed_NoValues()
	{
		var subject = new NotifySubject();
		string propertyChangedName = string.Empty;
		subject.PropertyChanged += (_, args) => propertyChangedName = args.PropertyName!;

		// act
		subject.S = "2";
		Assert.That(propertyChangedName, Is.EqualTo("S"));

		// act
		subject.I = 2;
		Assert.That(propertyChangedName, Is.EqualTo("I"));
	}

	[Test]
	public void Notify_Changed_Values()
	{
		var subject = new NotifySubject { I = 1 };

		string propertyChangedName = string.Empty;
		int oldValue = 0, newValue = 0;
		subject.PropertyChanged += (_, args) =>
		{
			// nasty casting due to inflexibility of generics
			var extended = (PropertyValueChangedEventArgs<int>)args;
			propertyChangedName = extended.PropertyName!;
			oldValue = extended.OldValue;
			newValue = extended.NewValue;
		};

		// act
		subject.I = 2;

		Assert.That(propertyChangedName, Is.EqualTo("I"));
		Assert.That(oldValue, Is.EqualTo(1));
		Assert.That(newValue, Is.EqualTo(2));
	}

	#endregion

	#endregion

	#region observations

	[Test]
	public void Observed_PropertyChanged_Changes()
	{
		var subject = new NotifySubject();
		string propertyChangedName = string.Empty;
		using (subject.Observed((_, e) => propertyChangedName = e.PropertyName!))
		{
			subject.S = "2";
		}

		// this happens after the handler has been unregistered
		subject.I = 2;

		Assert.That(propertyChangedName, Is.EqualTo("S"), "only the prop inside the 'using' scope");
	}

	[Test]
	public void Observing_PropertyChanging_Changes()
	{
		var subject = new NotifySubject();
		string propertyChangingName = string.Empty;
		using (subject.Observing((_, e) => propertyChangingName = e.PropertyName!))
		{
			subject.S = "2";
		}

		// this happens after the handler has been unregistered
		subject.I = 2;

		Assert.That(propertyChangingName, Is.EqualTo("S"), "only the prop inside the 'using' scope");
	}

	#endregion

	#region chains

	[Test]
	public void Raise_OwnEvent_HandledBySecond_NoFurtherCallbackExecuted()
	{
		string lastHandler = string.Empty;
		int numberOfHandlersExecuted = 0;

		Chained += (_, _) =>
		{
			numberOfHandlersExecuted++;
			lastHandler = "one";
		};
		Chained += (_, e) =>
		{
			numberOfHandlersExecuted++;
			lastHandler = "two";
			e.Handled = true;
		};
		Chained += (_, _) =>
		{
			numberOfHandlersExecuted++;
			lastHandler = "three";
		};

		var args = new ChainedArgs();

		// act
		bool result = Chained.Raise(this, args);

		Assert.That(result, Is.True);
		Assert.That(args.Handled, Is.True);
		Assert.That(numberOfHandlersExecuted, Is.EqualTo(2));
		Assert.That(lastHandler, Is.EqualTo("two"));
	}

	[Test]
	public void Raise_ExternalClassEvent_HandledBySecond_NoFurtherCallbackExecuted()
	{
		int numberOfHandlersExecuted = 0;

		var subject = new EventChainSubject();
		subject.Event += (_, _) => numberOfHandlersExecuted++;
		subject.Event += (_, e) =>
		{
			numberOfHandlersExecuted++;
			e.Handled = true;
		};
		subject.Event += (_, _) => numberOfHandlersExecuted++;

		var args = new ChainedArgs();
		
		// act
		bool result = subject.OnChainedEvent(args);

		Assert.That(result, Is.True);
		Assert.That(args.Handled, Is.True);
		Assert.That(numberOfHandlersExecuted, Is.EqualTo(2));
	}

	[Test]
	public void Raise_EmptyRegistratinList_NoHandling()
	{
		var subject = new EventChainSubject();
		var args = new ChainedArgs();
		
		// act
		bool result = subject.OnChainedEvent(args);

		Assert.That(result, Is.False);
		Assert.That(args.Handled, Is.False);
	}

	[Test]
	public void RaiseUntil_AbortableChain_HandledWhenValueIs2_NoFurtherCallbacksExecuted()
	{
		int numberOfHandlersExecuted = 0;
		Abortable += (_, e) =>
		{
			numberOfHandlersExecuted++;
			return e.Value = "1";
		};
		Abortable += (_, e) =>
		{
			numberOfHandlersExecuted++;
			return e.Value = "2";
		};
		Abortable += (_, e) =>
		{
			numberOfHandlersExecuted++;
			return e.Value = "3";
		};

		var args = new MutableArgs<string>(string.Empty);

		bool result = Abortable.RaiseUntil(this, args, e => e.Equals("2", StringComparison.OrdinalIgnoreCase));

		Assert.That(result, Is.True);
		Assert.That(args.Value, Is.EqualTo("2"));
		Assert.That(numberOfHandlersExecuted, Is.EqualTo(2));
	}

	[Test]
	public void RaiseUntil_AbortableChain_NotHandled_AllCallbacksExecuted()
	{
		int numberOfHandlersExecuted = 0;
		Abortable += (_, e) =>
		{
			numberOfHandlersExecuted++;
			return e.Value = "1";
		};
		Abortable += (_, e) =>
		{
			numberOfHandlersExecuted++;
			return e.Value = "2";
		};
		Abortable += (_, e) =>
		{
			numberOfHandlersExecuted++;
			return e.Value = "3";
		};

		var args = new MutableArgs<string>(string.Empty);

		bool result = Abortable.RaiseUntil(this, args, e => e.Equals("5", StringComparison.OrdinalIgnoreCase));

		Assert.That(result, Is.False);
		Assert.That(args.Value, Is.EqualTo("3"), "value of the last callback");
		Assert.That(numberOfHandlersExecuted, Is.EqualTo(3));
	}

	#endregion
}