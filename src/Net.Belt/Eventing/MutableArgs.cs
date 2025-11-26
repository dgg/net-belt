namespace Net.Belt.Eventing;

/// <summary>
/// Represents arguments for an event where the value can be mutated.
/// </summary>
/// <typeparam name="T">The type of the mutable value.</typeparam>
public interface IMutableArgs<T>
{
	/// <summary>
	/// Gets or sets the value.
	/// </summary>
	T Value { get; set; }
}

/// <inheritdoc />
/// <param name="value">The initial value.</param>
public class MutableArgs<T>(T value) : IMutableArgs<T>
{
	/// <inheritdoc />
	public T Value { get; set; } = value;
}