namespace Net.Belt.Patterns.ChainOfResponsibility;

/// <summary>
/// 
/// </summary>
/// <param name="canHandle"></param>
/// <param name="doHandle"></param>
/// <typeparam name="T"></typeparam>
public class ResponsibleLink<T>(Func<T, bool> canHandle, Func<T, T> doHandle) : ResponsibleLinkBase<T>
{
	/// <summary>
	/// 
	/// </summary>
	/// <param name="context"></param>
	/// <returns></returns>
	protected override bool CanHandle(T context) => canHandle(context);

	/// <summary>
	/// 
	/// </summary>
	/// <param name="context"></param>
	/// <returns></returns>
	protected override T DoHandle(T context) => doHandle(context);
}

/// <summary>
/// 
/// </summary>
/// <param name="canHandle"></param>
/// <param name="doHandle"></param>
/// <typeparam name="T"></typeparam>
/// <typeparam name="U"></typeparam>
public class ResponsibleLink<T, U>(Func<T, bool> canHandle, Func<T, U> doHandle) : ResponsibleLinkBase<T, U>
{
	/// <summary>
	/// 
	/// </summary>
	/// <param name="context"></param>
	/// <returns></returns>
	protected override bool CanHandle(T context) => canHandle(context);

	/// <summary>
	/// 
	/// </summary>
	/// <param name="context"></param>
	/// <returns></returns>
	protected override U DoHandle(T context) => doHandle(context);
}
