using System.ComponentModel;

namespace Net.Belt.Eventing;

/// <inheritdoc cref="PropertyChangedEventArgs" />
public class PropertyValueChangedEventArgs<T>(string propertyName, T oldValue, T newValue)
	: PropertyChangedEventArgs(propertyName), IOldValueArgs<T>, INewValueArgs<T>
{
	/// <inheritdoc />
	public T OldValue { get; } = oldValue;
	/// <inheritdoc />
	public T NewValue { get; } = newValue;
}