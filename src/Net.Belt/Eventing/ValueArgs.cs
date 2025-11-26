namespace Net.Belt.Eventing;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IValueArgs<out T>
{
	/// <summary>
	/// 
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