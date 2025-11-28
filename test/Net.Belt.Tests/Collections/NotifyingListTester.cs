using Net.Belt.Collections;
using Net.Belt.Eventing;
using Net.Belt.Tests.Collections.Support;

using NSubstitute;

namespace Net.Belt.Tests.Collections;

[TestFixture]
public class NotifyingListTester
{
	#region Insert

	[Test]
	public void Insert_EventsRaised()
	{
		var insertingHandler = Event.Handler<int, ValueIndexCancelArgs<int>>();
		var insertedHandler = Event.Handler<int, ValueIndexArgs<int>>();

		var subject = new NotifyingList<int>();
		subject.Inserting += insertingHandler;
		subject.Inserted += insertedHandler;

		subject.Insert(0, 3);
		
		insertingHandler.CalledOnce();
		insertedHandler.CalledOnce();
	}
	
	[Test]
	public void Insert_EventsRaisedWithCorrectArgumentsAndItemAdded()
	{
		int index = 0, item = 3;

		var subject = new NotifyingList<int>();
		var inserting = Event.Handler<int, ValueIndexCancelArgs<int>>();
		var inserted = Event.Handler<int, ValueIndexArgs<int>>();
		
		subject.Inserting += inserting;
		subject.Inserted += inserted;

		subject.Insert(index, item);
		
		inserting.CalledWith(index, item, false );
		inserted.CalledWith(index, item);
		Assert.That(subject.Count, Is.EqualTo(1));
		Assert.That(subject[index], Is.EqualTo(item));
	}

	[Test]
	public void Insert_CanCancelInsertion()
	{
		var inserted = Event.Handler<int, ValueIndexArgs<int>>();
		
		var subject = new NotifyingList<int>();
		subject.Inserting += Event.Cancelling<int, ValueIndexCancelArgs<int>>();
		subject.Inserted += inserted;
		
		subject.Insert(0, 3);
		
		inserted.NotCalled();
	}

	#endregion
	
	#region RemoveAt

	[Test]
	public void RemoveAt_OutOfBounds_Exception()
	{
		var subject = new NotifyingList<int> { 3 };

		Assert.That(() => subject.RemoveAt(2), Throws.InstanceOf<ArgumentOutOfRangeException>());
	}

	[Test]
	public void RemoveAt_EventsRaised()
	{
		var removing = Event.Handler<int, ValueIndexCancelArgs<int>>();
		var removed = Event.Handler<int, ValueIndexArgs<int>>();
		
		var subject = new NotifyingList<int> { 3 };
		subject.Removing += removing;
		subject.Removed += removed;

		subject.RemoveAt(0);

		removing.CalledOnce();
		removed.CalledOnce();
	}

	[Test]
	public void RemoveAt_EventsRaisedWithCorrectArgumentsAndItemRemoved()
	{
		int item = 3;
			
		var subject = new NotifyingList<int> { item };
		var removing = Event.Handler<int, ValueIndexCancelArgs<int>>();
		var removed = Event.Handler<int, ValueIndexArgs<int>>();
		subject.Removing += removing;
		subject.Removed += removed;
		
		subject.RemoveAt(0);

		Assert.That(subject.Count, Is.Zero);
		removing.CalledWith(0, item, false);
		removed.CalledWith(0, item);
	}

	[Test]
	public void RemoveAt_CanCancelDeletion()
	{
		int item = 3;
		var removed = Event.Handler<int, ValueIndexArgs<int>>();
		var subject = new NotifyingList<int> { item };
		subject.Removing += Event.Cancelling<int, ValueIndexCancelArgs<int>>();
		subject.Removed += removed;

		subject.RemoveAt(0);
		
		removed.NotCalled();
		Assert.That(subject.Count, Is.EqualTo(1));
		Assert.That(subject[0], Is.EqualTo(item));
	}

	#endregion
	
	#region Setter

	[Test]
	public void Set_OutOfBounds_Exception()
	{
		var subject = new NotifyingList<int> { 1 };

		Assert.That(() => subject[2] = 3, Throws.InstanceOf<ArgumentOutOfRangeException>());
	}

	[Test]
	public void Set_EventsRaised()
	{
		var setting = Event.Handler<int, ValueIndexChangingArgs<int>>();
		var set = Event.Handler<int, ValueIndexChangedArgs<int>>();
		
		var subject = new NotifyingList<int>();
		subject.Setting += setting;
		subject.Set += set;

		subject.Add(1);
		subject[0] = 3;

		setting.CalledOnce();
		set.CalledOnce();
	}

	[Test]
	public void Set_EventsRaisedWithCorrectArgumentsAndItemSet()
	{
		int index = 0, newValue = 3, previousValue = 1;

		var setting = Event.Handler<int, ValueIndexChangingArgs<int>>();
		var set = Event.Handler<int, ValueIndexChangedArgs<int>>();
		var subject = new NotifyingList<int> { previousValue };

		subject.Setting += setting;
		subject.Set += set;
		
		subject[index] = newValue;

		Assert.That(subject.Count, Is.EqualTo(1));
		Assert.That(subject[index], Is.EqualTo(newValue));
		setting.CalledWith(index, previousValue, newValue, false);
		set.CalledWith(index, previousValue, newValue);
	}

	[Test]
	public void Set_CanCancelSetting()
	{
		var set = Event.Handler<int, ValueIndexChangedArgs<int>>();
		var subject = new NotifyingList<int>();
		subject.Insert(0, 1);
		subject.Setting += Event.Cancelling<int, ValueIndexChangingArgs<int>>();
		subject.Set += set;

		subject[0] = 3;

		Assert.That(subject[0], Is.EqualTo(1));
		set.NotCalled();
	}

	#endregion
	
	#region Add

	[Test]
	public void Add_EventsRaised()
	{
		var adding = Event.Handler<int, ValueIndexCancelArgs<int>>();
		var added = Event.Handler<int, ValueIndexArgs<int>>();

		var subject = new NotifyingList<int>();
		subject.Inserting += adding;
		subject.Inserted += added;

		subject.Add(3);
		adding.CalledOnce();
		added.CalledOnce();
	}

	[Test]
	public void Add_EventsRaisedWithCorrectArgumentsAndItemAdded()
	{
		int index = 0, item = 3;

		var adding = Event.Handler<int, ValueIndexCancelArgs<int>>();
		var added = Event.Handler<int, ValueIndexArgs<int>>();
		var subject = new NotifyingList<int>();
		subject.Inserting += adding;
		subject.Inserted += added;

		subject.Add(item);
		
		Assert.That(subject.Count, Is.EqualTo(1));
		Assert.That(subject[index], Is.EqualTo(item));
		adding.CalledWith(index, item, false);
		added.CalledWith(index, item);
	}

	[Test]
	public void Add_CanCancelAddition()
	{
		var added = Event.Handler<int, ValueIndexArgs<int>>();
		var subject = new NotifyingList<int>();
		subject.Inserting += Event.Cancelling<int, ValueIndexCancelArgs<int>>();
		subject.Inserted += added;

		subject.Add(3);

		Assert.That(subject, Is.Empty);
		added.NotCalled();
	}

	#endregion
	
	#region Clear

	[Test]
	public void Clear_EventsRaised()
	{
		var clearing = Event.Handler<int, CancelArgs>();
		var cleared = Event.Handler<int, EventArgs>();

		var subject = new NotifyingList<int>();
		subject.Clearing += clearing;
		subject.Cleared += cleared;

		subject.Clear();
		
		clearing.CalledOnce();
		cleared.CalledOnce();
	}

	[Test]
	public void Clear_EventsRaisedWithCorrectArgumentsAndItemsRemoved()
	{
		var clearing = Event.Handler<int, CancelArgs>();
		var subject = new NotifyingList<int>();
		subject.Clearing += clearing;
		subject.Cleared += Event.Handler<int, EventArgs>();

		subject.Clear();
		Assert.That(subject, Is.Empty);
		clearing.CalledWith(false);
	}

	[Test]
	public void Clear_CanCancelClearance()
	{
		var cleared = Event.Handler<int, EventArgs>();
		var subject = new NotifyingList<int>{ 4 };
		subject.Clearing += Event.Cancelling<int, CancelArgs>();
		subject.Cleared += cleared;

		subject.Clear();
		
		Assert.That(subject, Is.Not.Empty);
		cleared.NotCalled();
	}

	#endregion

	#region Remove

	[Test]
	public void RemoveAt_OutOfBounds_ReturnsFalse()
	{
		var subject = new NotifyingList<int> { 3 };

		Assert.That(subject.Remove(2), Is.False);
	}

	[Test]
	public void Remove_EventsRaised()
	{
		var removing = Event.Handler<int, ValueIndexCancelArgs<int>>();
		var removed = Event.Handler<int, ValueIndexArgs<int>>();

		var subject = new NotifyingList<int> { 3 };
		subject.Removing += removing;
		subject.Removed += removed;

		Assert.That(subject.Remove(3), Is.True);

		removing.CalledOnce();
		removed.CalledOnce();
	}

	[Test]
	public void Remove_EventsRaisedWithCorrectArgumentsAndItemRemoved()
	{
		int index = 0, item = 3;

		var removing = Event.Handler<int, ValueIndexCancelArgs<int>>();
		var removed = Event.Handler<int, ValueIndexArgs<int>>();
		
		var subject = new NotifyingList<int> { 3 };

		subject.Removing += removing;
		subject.Removed += removed;

		subject.Remove(3);
		
		Assert.That(subject, Is.Empty);
		removing.CalledWith(index, item, false);
		removed.CalledWith(index, item);
	}

	[Test]
	public void Remove_CanCancelDeletion()
	{
		var removed = Event.Handler<int, ValueIndexArgs<int>>();

		var subject = new NotifyingList<int> { 3 };
		subject.Removing += Event.Cancelling<int, ValueIndexCancelArgs<int>>();
		subject.Removed += removed;

		Assert.That(subject.Remove(3), Is.False);
		
		Assert.That(subject[0], Is.EqualTo(3));
		removed.NotCalled();
	}

	#endregion
}