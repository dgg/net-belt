namespace Net.Belt.Eventing;

/// <summary>
/// Represents event arguments that carry one value.
/// </summary>
/// <typeparam name="T">The type of the value.</typeparam>
public interface IValueArgs<out T>
{
	/// <summary>
	/// Gets the value.
	/// </summary>
	T Value { get; }
}

/// <inheritdoc />
public record ValueArgs<T>(T Value) : IValueArgs<T>;

/// <inheritdoc cref="IValueArgs{T}" />
public class ValueEventArgs<T>(T value) : EventArgs, IValueArgs<T>
{
	/// <inheritdoc />
	public T Value => value;
}