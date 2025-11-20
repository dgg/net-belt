namespace Net.Belt.Patterns.ChainOfResponsibility;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IChainable<T>
{
	/// <summary>
	/// 
	/// </summary>
	T? Next { get; }

	/// <summary>
	/// 
	/// </summary>
	/// <param name="next"></param>
	/// <returns></returns>
	T Chain(T next);
}

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class ChainableBase<T> : IChainable<T>
{
	/// <summary>
	/// 
	/// </summary>
	public T? Next { get; private set; }

	/// <summary>
	/// 
	/// </summary>
	/// <param name="next"></param>
	/// <returns></returns>
	/// <exception cref="NotImplementedException"></exception>
	public T Chain(T next) => Next = next;
}