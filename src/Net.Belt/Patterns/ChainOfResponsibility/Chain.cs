namespace Net.Belt.Patterns.ChainOfResponsibility;

/// <summary>
/// Provides static factory methods for creating chains of responsibility of different links.
/// </summary>
public static class Chain
{
	/// <summary>
	/// Creates a chain of responsibility from a sequence of <see cref="IResponsibleLink{T}"/> instances.
	/// </summary>
	/// <typeparam name="T">The type of the context being handled by the links.</typeparam>
	/// <param name="links">An array of <see cref="IResponsibleLink{T}"/> instances to form the chain.</param>
	/// <returns>The first link in the created chain, which can be used to initiate the responsibility flow.</returns>
	/// <exception cref="ArgumentException">Thrown if no links are provided to create the chain.</exception>
	public static IResponsibleLink<T> OfResponsibility<T>(params IResponsibleLink<T>[] links) => chain(links);

	/// <summary>
	/// Creates a chain of responsibility from a sequence of <see cref="IResponsibleLink{T, U}"/> instances.
	/// </summary>
	/// <typeparam name="T">The type of the input context being handled by the links.</typeparam>
	/// <typeparam name="U">The type of the output result produced by the links.</typeparam>
	/// <param name="links">An array of <see cref="IResponsibleLink{T, U}"/> instances to form the chain.</param>
	/// <returns>The first link in the created chain, which can be used to initiate the responsibility flow.</returns>
	/// <exception cref="ArgumentException">Thrown if no links are provided to create the chain.</exception>
	public static IResponsibleLink<T, U> OfResponsibility<T, U>(params IResponsibleLink<T, U>[] links) => chain(links);

	/// <summary>
	/// Creates an chain of responsibility from a sequence of <see cref="IResponsibleAsyncLink{T}"/> instances.
	/// </summary>
	/// <typeparam name="T">The type of the context being handled by the links.</typeparam>
	/// <param name="links">An array of <see cref="IResponsibleAsyncLink{T}"/> instances to form the chain.</param>
	/// <returns>The first link in the created chain, which can be used to initiate the responsibility flow.</returns>
	/// <exception cref="ArgumentException">Thrown if no links are provided to create the chain.</exception>
	public static IResponsibleAsyncLink<T> OfAsyncResponsibility<T>(params IResponsibleAsyncLink<T>[] links) =>
		chain(links);

	/// <summary>
	/// Creates a chain of responsibility from a sequence of <see cref="IResponsibleAsyncLink{T, U}"/> instances.
	/// </summary>
	/// <typeparam name="T">The type of the input context being handled by the links.</typeparam>
	/// <typeparam name="U">The type of the output result produced by the asynchronous links.</typeparam>
	/// <param name="links">An array of <see cref="IResponsibleAsyncLink{T, U}"/> instances to form the chain.</param>
	/// <returns>The first link in the created chain, which can be used to initiate the responsibility flow.</returns>
	/// <exception cref="ArgumentException">Thrown if no links are provided to create the chain.</exception>
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