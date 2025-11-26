namespace Net.Belt.Eventing;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
public interface INewValueArgs<out T>
{
	/// <summary>
	/// 
	/// </summary>
	T NewValue { get; }
}

/// <inheritdoc cref="INewValueArgs{T}" />
public class ValueChangingArgs<T>(T value, T newValue) : ValueCancelArgs<T>(value), INewValueArgs<T>
{
	/// <inheritdoc />
	public T NewValue { get; } = newValue;
}