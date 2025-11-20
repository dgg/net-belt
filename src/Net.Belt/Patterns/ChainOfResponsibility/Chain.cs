namespace Net.Belt.Patterns.ChainOfResponsibility;

/// <summary>
/// 
/// </summary>
public static class Chain
{
	/// <summary>
	/// 
	/// </summary>
	/// <param name="links"></param>
	/// <returns></returns>
	/// <exception cref="ArgumentException"></exception>
	public static IResponsibleLink<T> OfResponsibility<T>(params IResponsibleLink<T>[] links) => chain(links);

	/// <summary>
	/// 
	/// </summary>
	/// <param name="links"></param>
	/// <returns></returns>
	/// <exception cref="ArgumentException"></exception>
	public static IResponsibleLink<T, U> OfResponsibility<T, U>(params IResponsibleLink<T, U>[] links) => chain(links);

	/// <summary>
	///
	/// </summary>
	/// <param name="links"></param>
	/// <returns></returns>
	/// <exception cref="ArgumentException"></exception>
	public static IResponsibleAsyncLink<T> OfAsyncResponsibility<T>(params IResponsibleAsyncLink<T>[] links) =>
		chain(links);

	/// <summary>
	///
	/// </summary>
	/// <param name="links"></param>
	/// <returns></returns>
	/// <exception cref="ArgumentException"></exception>
	public static IResponsibleAsyncLink<T, U> OfAsyncResponsibility<T, U>(params IResponsibleAsyncLink<T, U>[] links) =>
		chain(links);

	private static TChainable chain<TChainable>(TChainable[] links) where TChainable : IChainable<TChainable>
	{
		if (links.Length == 0)
		{
			throw new ArgumentException("Cannot create a chain without links", nameof(links));
		}

		for (int i = 0; i < links.Length - 1; i++)
		{
			links[i].Chain(links[i + 1]);
		}

		return links[0];
	}
}