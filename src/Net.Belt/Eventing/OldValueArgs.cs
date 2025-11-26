namespace Net.Belt.Eventing;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IOldValueArgs<out T>
{
	/// <summary>
	/// 
	/// </summary>
	T OldValue { get; }
}

/// <summary>
/// 
/// </summary>
/// <param name="Value"></param>
/// <param name="OldValue"></param>
/// <typeparam name="T"></typeparam>
public record ValueChangedArgs<T>(T Value, T OldValue) : ValueArgs<T>(Value), IOldValueArgs<T>;