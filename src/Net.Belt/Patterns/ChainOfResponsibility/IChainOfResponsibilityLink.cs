namespace Net.Belt.Patterns.ChainOfResponsibility;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IChainOfResponsibilityLink<in T>
{
	/// <summary>
	/// 
	/// </summary>
	/// <param name="context"></param>
	/// <returns></returns>
	bool CanHandle(T context);
	/// <summary>
	/// 
	/// </summary>
	/// <param name="context"></param>
	void DoHandle(T context);
}

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TResult"></typeparam>
public interface IChainOfResponsibilityLink<in T, out TResult>
{
	/// <summary>
	/// 
	/// </summary>
	/// <param name="context"></param>
	/// <returns></returns>
	bool CanHandle(T context);
	/// <summary>
	/// 
	/// </summary>
	/// <param name="context"></param>
	/// <returns></returns>
	TResult DoHandle(T context);
}