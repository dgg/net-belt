namespace Net.Belt.Eventing;

/// <summary>
/// 
/// </summary>
public interface IIndexArgs
{
	/// <summary>
	/// 
	/// </summary>
	int Index { get; }
}

/// <inheritdoc cref="IIndexArgs" />
public record ValueIndexArgs<T>(int Index, T Value) : ValueArgs<T>(Value), IIndexArgs;

/// <inheritdoc cref="ValueChangingArgs{T}"/>
public class ValueIndexChangingArgs<T>(int index, T value, T newValue) : ValueChangingArgs<T>(value, newValue), IIndexArgs
{
	/// <inheritdoc />
	public int Index => index;
}

/// <inheritdoc cref="ValueChangedArgs{T}"/>
public record ValueIndexChangedArgs<T>(int Index, T Value, T OldValue) : ValueChangedArgs<T>(Value, OldValue), IIndexArgs;

/// <summary>
/// 
/// </summary>
/// <param name="index"></param>
/// <param name="value"></param>
/// <typeparam name="T"></typeparam>
public class ValueIndexCancelArgs<T>(int index, T value) : CancelArgs, IValueArgs<T>, IIndexArgs
{
	/// <inheritdoc />
	public T Value => value;

	/// <inheritdoc />
	public int Index => index;
}