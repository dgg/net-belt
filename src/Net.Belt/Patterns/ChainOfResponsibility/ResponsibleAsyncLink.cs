namespace Net.Belt.Patterns.ChainOfResponsibility;

/// <summary>
/// 
/// </summary>
/// <param name="canHandle"></param>
/// <param name="doHandle"></param>
/// <typeparam name="T"></typeparam>
/// <typeparam name="U"></typeparam>
public class ResponsibleAsyncLink<T, U>(
	Func<T, CancellationToken, ValueTask<bool>> canHandle,
	Func<T, CancellationToken, Task<U>> doHandle) : ResponsibleAsyncLinkBase<T, U>
{
	/// <summary>
	///
	/// </summary>
	/// <param name="context"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	protected override ValueTask<bool> CanHandle(T context, CancellationToken cancellationToken) =>
		canHandle(context, cancellationToken);

	/// <summary>
	/// 
	/// </summary>
	/// <param name="context"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	protected override Task<U> DoHandle(T context, CancellationToken cancellationToken) =>
		doHandle(context, cancellationToken);
}

/// <summary>
/// 
/// </summary>
/// <param name="canHandle"></param>
/// <param name="doHandle"></param>
/// <typeparam name="T"></typeparam>
public class ResponsibleAsyncLink<T>(
	Func<T, CancellationToken, ValueTask<bool>> canHandle,
	Func<T, CancellationToken, Task<T>> doHandle) : ResponsibleAsyncLinkBase<T>
{
	/// <summary>
	///
	/// </summary>
	/// <param name="context"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	protected override ValueTask<bool> CanHandle(T context, CancellationToken cancellationToken) =>
		canHandle(context, cancellationToken);

	/// <summary>
	/// 
	/// </summary>
	/// <param name="context"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	protected override Task<T> DoHandle(T context, CancellationToken cancellationToken) =>
		doHandle(context, cancellationToken);
}