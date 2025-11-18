namespace Net.Belt.Patterns.ChainOfResponsibility;

/// <summary>
/// 
/// </summary>
public static class Chain
{
	private class EmptyLink<T> : ChainOfResponsibilityLink<T>
	{
		public override bool CanHandle(T context) => false;

		protected override void DoHandle(T context) { }
	}

	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <returns></returns>
	public static ChainOfResponsibilityLink<T> OfResponsibility<T>() => new EmptyLink<T>();

	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <typeparam name="TResult"></typeparam>
	/// <returns></returns>
	public static ChainOfResponsibilityLink<T, TResult> OfResponsibility<T, TResult>() => new EmptyLink<T, TResult>();

	private class EmptyLink<T, TResult> : ChainOfResponsibilityLink<T, TResult>
	{
		public override bool CanHandle(T context) => false;

		protected override TResult DoHandle(T context) => default!;
	}
}