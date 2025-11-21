namespace Net.Belt.Patterns.ChainOfResponsibility;

/// <summary>
/// Defines a contract for objects that can be chained together, forming a sequence where each object
/// can pass control or data to the next in line. This is a fundamental building block for patterns
/// like Chain of Responsibility.
/// </summary>
/// <remarks>This abstraction allows same <see cref="Chain.OfResponsibility{T}"/> implementation no matter which type of link.</remarks>
/// <typeparam name="T">The type of the chainable object itself.</typeparam>
public interface IChainable<T>
{
	/// <summary>
	/// Gets the next item in the chain.
	/// </summary>
	/// <value>The next item of type <typeparamref name="T"/> in the chain, or <see langword="null"/> if this is the last item.</value>
	T? Next { get; }

	/// <summary>
	/// Fluently chains the current object to a specified next object.
	/// </summary>
	/// <param name="next">The next object of type <typeparamref name="T"/> to chain to.</param>
	/// <returns>The <paramref name="next"/> object, allowing for fluent chaining.</returns>
	T Chain(T next);
}

/// <summary>
/// Provides a base implementation for the <see cref="IChainable{T}"/> interface,
/// offering a concrete way to manage the next item in a chain.
/// </summary>
/// <typeparam name="T">The type of the chainable object itself.</typeparam>
public abstract class ChainableBase<T> : IChainable<T>
{
	/// <summary>
	/// Gets the next item in the chain.
	/// </summary>
	/// <value>The next item of type <typeparamref name="T"/> in the chain, or <see langword="null"/> if this is the last item.</value>
	public T? Next { get; private set; }

	/// <summary>
	/// Fluently chains the current object to a specified next object.
	/// </summary>
	/// <param name="next">The next object of type <typeparamref name="T"/> to chain to.</param>
	/// <returns>The <paramref name="next"/> object, allowing for fluent chaining.</returns>
	public T Chain(T next) => Next = next;
}