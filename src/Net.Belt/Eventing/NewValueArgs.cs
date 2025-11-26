namespace Net.Belt.Eventing;

/// <summary>
/// Represents arguments for an event that includes a new value.
/// </summary>
/// <typeparam name="T">The type of the new value.</typeparam>
public interface INewValueArgs<out T>
{
	/// <summary>
	/// Gets the new value.
	/// </summary>
	T NewValue { get; }
}

/// <inheritdoc cref="INewValueArgs{T}" />
/// <param name="oldValue">The old value.</param>
/// <param name="newValue">The new value.</param>
public class ValueChangingArgs<T>(T oldValue, T newValue) : ValueCancelArgs<T>(oldValue), INewValueArgs<T>
{
	/// <inheritdoc />
	public T NewValue { get; } = newValue;
}