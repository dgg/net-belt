namespace Net.Belt.Eventing;

/// <summary>
/// Represents arguments for an event that includes an old value.
/// </summary>
/// <typeparam name="T">The type of the old value.</typeparam>
public interface IOldValueArgs<out T>
{
	/// <summary>
	/// Gets the old value.
	/// </summary>
	T OldValue { get; }
}

/// <summary>
/// Provides data for the <see cref="ValueChangedArgs{T}"/> event.
/// </summary>
/// <param name="Value">The current value.</param>
/// <param name="OldValue">The old value before the change.</param>
/// <typeparam name="T">The type of the value.</typeparam>
public record ValueChangedArgs<T>(T Value, T OldValue) : ValueArgs<T>(Value), IOldValueArgs<T>;