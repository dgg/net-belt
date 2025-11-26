using System.ComponentModel;

namespace Net.Belt.Eventing;

/// <inheritdoc cref="PropertyChangingEventArgs" />
public class PropertyValueChangingEventArgs<T>(string propertyName, T oldValue, T newValue)
	: PropertyChangingEventArgs(propertyName), IOldValueArgs<T>, INewValueArgs<T>, ICancelArgs
{
	/// <inheritdoc />
	public T OldValue { get; } = oldValue;

	/// <inheritdoc />
	public T NewValue { get; } = newValue;

	/// <inheritdoc />
	public bool IsCancelled { get; private set; }

	/// <inheritdoc />
	public void Cancel() => IsCancelled = true;
}