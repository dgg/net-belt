namespace Net.Belt.Eventing;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="U"></typeparam>
public interface IMultiArgs<out T, out U> : IValueArgs<T>
{
	/// <summary>
	/// 
	/// </summary>
	U Value2 { get; }
}

/// <inheritdoc cref="IMultiArgs{T,U}" />
public record MultiArgs<T, U>(T Value, U Value2) : ValueArgs<T>(Value), IMultiArgs<T, U>;