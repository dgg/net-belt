namespace Net.Belt.Patterns.ChainOfResponsibility;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class ResponsibleAsyncLinkBase<T> : ChainableBase<IResponsibleAsyncLink<T>>, IResponsibleAsyncLink<T>
{
	/// <summary>
	/// 
	/// </summary>
	/// <param name="context"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	protected abstract ValueTask<bool> CanHandle(T context, CancellationToken cancellationToken);

	/// <summary>
	/// 
	/// </summary>
	/// <param name="context"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	protected abstract Task<T> DoHandle(T context, CancellationToken cancellationToken);

	/// <summary>
	/// 
	/// </summary>
	/// <param name="context"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public async Task<Handled<T>> Handle(T context, CancellationToken cancellationToken = default)
	{
		return await CanHandle(context, cancellationToken)
			// handled by this link
			? new Handled<T>(await DoHandle(context, cancellationToken))
			// handled by next (or next.next, ...) or unhandled (default(Handled<T>)) if last
			: await (Next?.Handle(context, cancellationToken) ?? Task.FromResult(default(Handled<T>)));
	}
}

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="U"></typeparam>
public abstract class ResponsibleAsyncLinkBase<T, U> : ChainableBase<IResponsibleAsyncLink<T, U>>, IResponsibleAsyncLink<T, U>
{
	/// <summary>
	/// 
	/// </summary>
	/// <param name="context"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	protected abstract ValueTask<bool> CanHandle(T context, CancellationToken cancellationToken);

	/// <summary>
	/// 
	/// </summary>
	/// <param name="context"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	protected abstract Task<U> DoHandle(T context, CancellationToken cancellationToken);

	/// <summary>
	/// 
	/// </summary>
	/// <param name="context"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public async Task<Handled<U>> Handle(T context, CancellationToken cancellationToken = default)
	{
		return await CanHandle(context, cancellationToken)
			// handled by this link
			? new Handled<U>(await DoHandle(context, cancellationToken))
			// handled by next (or next.next, ...) or unhandled (default(Handled<T>)) if last
			: await (Next?.Handle(context, cancellationToken) ?? Task.FromResult(default(Handled<U>)));
	}
}
	