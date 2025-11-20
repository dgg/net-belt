namespace Net.Belt.Patterns.ChainOfResponsibility;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IResponsibleLink<T> : IChainable<IResponsibleLink<T>>
{
	/// <summary>
	/// 
	/// </summary>
	/// <param name="context"></param>
	/// <returns></returns>
	Handled<T> Handle(T context);
}

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="U"></typeparam>
public interface IResponsibleLink<T, U> : IChainable<IResponsibleLink<T, U>>
{
	/// <summary>
	/// 
	/// </summary>
	/// <param name="context"></param>
	/// <returns></returns>
	Handled<U> Handle(T context);
}