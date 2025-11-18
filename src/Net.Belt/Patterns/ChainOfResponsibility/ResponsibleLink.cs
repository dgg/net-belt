namespace Net.Belt.Patterns.ChainOfResponsibility;

/// <summary>
/// 
/// </summary>
/// <param name="link"></param>
/// <typeparam name="T"></typeparam>
public class ResponsibleLink<T>(IChainOfResponsibilityLink<T> link) : ChainOfResponsibilityLink<T>
{
	/// <summary>
	/// 
	/// </summary>
	/// <param name="context"></param>
	/// <returns></returns>
	public override bool CanHandle(T context) => link.CanHandle(context);

	/// <summary>
	/// 
	/// </summary>
	/// <param name="context"></param>
	protected override void DoHandle(T context) => link.DoHandle(context);
}

/// <summary>
/// 
/// </summary>
/// <param name="link"></param>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TResult"></typeparam>
public class ResponsibleLink<T, TResult>(IChainOfResponsibilityLink<T, TResult> link)
	: ChainOfResponsibilityLink<T, TResult>
{
	/// <summary>
	/// 
	/// </summary>
	/// <param name="context"></param>
	/// <returns></returns>
	public override bool CanHandle(T context) => link.CanHandle(context);

	/// <summary>
	/// 
	/// </summary>
	/// <param name="context"></param>
	/// <returns></returns>
	protected override TResult DoHandle(T context) => link.DoHandle(context);
}