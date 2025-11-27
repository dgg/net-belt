using System.Collections;

using Net.Belt.Eventing;

namespace Net.Belt.Collections;

/// <summary>
/// Represents a strongly typed list of objects that can be accessed by index. Provides methods to search, sort, and manipulate lists.
/// </summary>
/// <remarks>Additional events are provided whenever the contents of the list are modified.</remarks>
/// <param name="list">The list that is wrapped</param>
/// <typeparam name="T"> The type of elements in the list.</typeparam>
public class NotifyingList<T>(IList<T> list) : IList<T>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="NotifyingList{T}"/> class that is empty and has the specified initial capacity.
	/// </summary>
	/// <param name="capacity"></param>
	public NotifyingList(int capacity) : this(new List<T>(capacity)) { }

	/// <summary>
	/// Initializes a new instance of the <see cref="NotifyingList{T}"/> class that contains elements copied from the
	/// specified collection and has sufficient capacity to accommodate the number of elements copied.
	/// </summary>
	/// <param name="collection">The collection whose elements are copied to the new list.</param>
	public NotifyingList(IEnumerable<T> collection) : this(new List<T>(collection)) { }

	/// <summary>
	/// Initializes a new instance of the <see cref="NotifyingList{T}"/> class that is empty and has the default initial capacity.
	/// </summary>
	public NotifyingList() : this(new List<T>()) { }

	#region events

	/// <summary>
	/// Occurs before an item is inserted into the list.
	/// </summary>
	/// <remarks>
	/// Handlers can cancel the insertion by setting <see cref="CancelArgs.IsCancelled"/> to <c>true</c>.
	/// </remarks>
	public event Eventing.EventHandler<ValueIndexCancelArgs<T>, NotifyingList<T>>? Inserting;

	private bool onInserting(int index, T item)
	{
		// if there are no handlers, insertion is not prevented
		if (Inserting == null) return true;

		var args = Args.Cancel(index, item);
		Inserting(this, args);
		return !args.IsCancelled;
	}

	/// <summary>
	/// Occurs after an item has been inserted into the list.
	/// </summary>
	public event Eventing.EventHandler<ValueIndexArgs<T>, NotifyingList<T>>? Inserted;

	/// <summary>
	/// Occurs before an item at a specified index is set to a new value.
	/// </summary>
	/// <remarks>
	/// Handlers can cancel the setting operation by setting <see cref="CancelArgs.IsCancelled"/> to <c>true</c>.
	/// </remarks>
	public event Eventing.EventHandler<ValueIndexChangingArgs<T>, NotifyingList<T>>? Setting;

	private bool onSetting(int index, T oldValue, T newValue)
	{
		// if there are no handlers, setting is not prevented
		if (Setting == null) return true;

		var args = Args.Changing(index, oldValue, newValue);
		Setting(this, args);
		return !args.IsCancelled;
	}

	/// <summary>
	/// Occurs after an item at a specified index has been set to a new value.
	/// </summary>
	public event Eventing.EventHandler<ValueIndexChangedArgs<T>, NotifyingList<T>>? Set;

	/// <summary>
	/// Occurs before an item is removed from the list.
	/// </summary>
	/// <remarks>
	/// Handlers can cancel the removal by setting <see cref="CancelArgs.IsCancelled"/> to <c>true</c>.
	/// </remarks>
	public event Eventing.EventHandler<ValueIndexCancelArgs<T>, NotifyingList<T>>? Removing;

	private bool onRemoving(int index, T item)
	{
		// if there are no handlers, removal is not prevented
		if (Removing == null) return true;

		var args = Args.Cancel(index, item);
		Removing(this, args);
		return !args.IsCancelled;
	}

	/// <summary>
	/// Occurs after an item has been removed from the list.
	/// </summary>
	public event Eventing.EventHandler<ValueIndexArgs<T>, NotifyingList<T>>? Removed;

	/// <summary>
	/// Occurs before the list is cleared.
	/// </summary>
	/// <remarks>
	/// Handlers can cancel the clearing operation by setting <see cref="CancelArgs.IsCancelled"/> to <c>true</c>.
	/// </remarks>
	public event Eventing.EventHandler<CancelArgs, NotifyingList<T>>? Clearing;

	private bool onClearing()
	{
		// if there are no handlers, clearing is not prevented
		if (Clearing == null) return true;

		var args = new CancelArgs();
		Clearing(this, args);
		return !args.IsCancelled;
	}

	/// <summary>
	/// Occurs after the list has been cleared.
	/// </summary>
	public event Eventing.EventHandler<EventArgs, NotifyingList<T>>? Cleared;

	#endregion

	#region IList<T> members

	/// <inheritdoc />>
	public IEnumerator<T> GetEnumerator() => list.GetEnumerator();

	/// <inheritdoc />>
	IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)list).GetEnumerator();

	/// <inheritdoc />>
	public void Add(T item) => Insert(Count, item);

	/// <inheritdoc />>
	public void Clear()
	{
		if (onClearing())
		{
			list.Clear();
			Cleared?.Invoke(this, EventArgs.Empty);
		}
	}

	/// <inheritdoc />>
	public bool Contains(T item) => list.Contains(item);

	/// <inheritdoc />>
	public void CopyTo(T[] array, int arrayIndex) => list.CopyTo(array, arrayIndex);

	/// <inheritdoc />>
	public bool Remove(T item)
	{
		bool result = false;
		int index = list.IndexOf(item);
		if (index != -1)
		{
			if (onRemoving(index, item))
			{
				list.RemoveAt(index);
				result = true;
				Removed?.Invoke(this, Args.Index(index, item));
			}
		}

		return result;
	}

	/// <inheritdoc />>
	public int Count => list.Count;

	/// <inheritdoc />>
	public bool IsReadOnly => list.IsReadOnly;

	/// <inheritdoc />>
	public int IndexOf(T item) => list.IndexOf(item);

	/// <inheritdoc />>
	public void Insert(int index, T item)
	{
		if (onInserting(index, item))
		{
			list.Insert(index, item);
			Inserted?.Invoke(this, Args.Index(index, item));
		}
	}

	/// <inheritdoc />>
	public void RemoveAt(int index)
	{
		T item = list[index];
		if (onRemoving(index, item))
		{
			list.RemoveAt(index);
			Removed?.Invoke(this, Args.Index(index, item));
		}
	}

	/// <inheritdoc />>
	public T this[int index]
	{
		get => list[index];
		set
		{
			T oldValue = list[index];
			if (onSetting(index, oldValue, value))
			{
				list[index] = value;
				Set?.Invoke(this, Args.Changed(index, oldValue, value));
			}
		}
	}

	#endregion
}