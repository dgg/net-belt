namespace Net.Belt.Eventing;

/// <summary>
/// Represents arguments for events that include an index.
/// </summary>
public interface IIndexArgs
{
	/// <summary>
	/// Gets the index associated with the event.
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
/// Represents arguments for a cancellable event that includes an index and a value.
/// </summary>
/// <param name="index">The index associated with the event.</param>
/// <param name="value">The value associated with the event.</param>
/// <typeparam name="T">The type of the value.</typeparam>
public class ValueIndexCancelArgs<T>(int index, T value) : CancelArgs, IValueArgs<T>, IIndexArgs
{
	/// <inheritdoc />
	public T Value => value;

	/// <inheritdoc />
	public int Index => index;
}