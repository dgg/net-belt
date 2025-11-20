namespace Net.Belt.Patterns.ChainOfResponsibility;

/// <summary>
/// 
/// </summary>
public abstract class ResponsibleLinkBase<T> : ChainableBase<IResponsibleLink<T>>, IResponsibleLink<T>
{
	/// <summary>
	/// 
	/// </summary>
	/// <param name="context"></param>
	/// <returns></returns>
	protected abstract bool CanHandle(T context);

	/// <summary>
	/// 
	/// </summary>
	/// <param name="context"></param>
	/// <returns></returns>
	protected abstract T DoHandle(T context);

	/// <summary>
	/// 
	/// </summary>
	/// <param name="context"></param>
	/// <returns></returns>
	public Handled<T> Handle(T context)
	{
		return CanHandle(context)
			// handled by this link
			? new Handled<T>(DoHandle(context))
			// handled by next (or next.next, ...) or unhandled (default(Handled<T>)) if last
			: Next?.Handle(context) ?? default;
	}
}

/// <summary>
/// 
/// </summary>
public abstract class ResponsibleLinkBase<T, U> : ChainableBase<IResponsibleLink<T, U>>, IResponsibleLink<T, U>
{
	/// <summary>
	/// 
	/// </summary>
	/// <param name="context"></param>
	/// <returns></returns>
	protected abstract bool CanHandle(T context);

	/// <summary>
	/// 
	/// </summary>
	/// <param name="context"></param>
	/// <returns></returns>
	protected abstract U DoHandle(T context);

	/// <summary>
	/// 
	/// </summary>
	/// <param name="context"></param>
	/// <returns></returns>
	public Handled<U> Handle(T context)
	{
		return CanHandle(context)
			// handled by this link
			? new Handled<U>(DoHandle(context))
			// handled by next (or next.next, ...) or unhandled (default(Handled<T>)) if last
			: Next?.Handle(context) ?? default;
	}
}