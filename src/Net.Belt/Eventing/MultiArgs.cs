namespace Net.Belt.Eventing;

/// <summary>
/// Represents event arguments that carry two values.
/// </summary>
/// <typeparam name="T">The type of the first value.</typeparam>
/// <typeparam name="U">The type of the second value.</typeparam>
public interface IMultiArgs<out T, out U> : IValueArgs<T>
{
	/// <summary>
	/// Gets the second value.
	/// </summary>
	U Value2 { get; }
}

/// <inheritdoc cref="IMultiArgs{T,U}" />
public record MultiArgs<T, U>(T Value, U Value2) : ValueArgs<T>(Value), IMultiArgs<T, U>;