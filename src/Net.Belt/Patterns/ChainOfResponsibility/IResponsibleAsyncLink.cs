namespace Net.Belt.Patterns.ChainOfResponsibility;

/// <summary>
///
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IResponsibleAsyncLink<T> : IChainable<IResponsibleAsyncLink<T>>
{
	/// <summary>
	///
	/// </summary>
	/// <param name="context"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<Handled<T>> Handle(T context, CancellationToken cancellationToken = default);
}

/// <summary>
///
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="U"></typeparam>
public interface IResponsibleAsyncLink<T, U> : IChainable<IResponsibleAsyncLink<T, U>>
{
	/// <summary>
	///
	/// </summary>
	/// <param name="context"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<Handled<U>> Handle(T context, CancellationToken cancellationToken = default);
}