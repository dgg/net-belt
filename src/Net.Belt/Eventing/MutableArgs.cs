namespace Net.Belt.Eventing;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IMutableArgs<T>
{
	/// <summary>
	/// 
	/// </summary>
	T Value { get; set; }
}

/// <inheritdoc />
public class MutableArgs<T>(T value) : IMutableArgs<T>
{
	/// <inheritdoc />
	public T Value { get; set; } = value;
}