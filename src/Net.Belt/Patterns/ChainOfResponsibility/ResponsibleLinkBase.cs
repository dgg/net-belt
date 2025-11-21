namespace Net.Belt.Patterns.ChainOfResponsibility;

/// <summary>
/// Base class for a responsible link in a chain of responsibility pattern.
/// A responsible link processes a context and can decide whether to handle it or pass it to the next link.
/// </summary>
/// <typeparam name="T">The type of the context being handled.</typeparam>
public abstract class ResponsibleLinkBase<T> : ChainableBase<IResponsibleLink<T>>, IResponsibleLink<T>
{
	/// <summary>
	/// Determines if this link can handle the given input context.
	/// </summary>
	/// <param name="context">The input context to evaluate.</param>
	/// <returns><c>true</c> if this link can handle the context; otherwise, <c>false</c>.</returns>
	protected abstract bool CanHandle(T context);

	/// <summary>
	/// Performs the actual handling of the context.
	/// </summary>
	/// <remarks>Only called when <see cref="CanHandle"/> returns <c>true</c>.</remarks>
	/// <param name="context">The context to handle.</param>
	/// <returns>The handled context.</returns>
	protected abstract T DoHandle(T context);

	/// <summary>
	/// Handles the given context. If this link can handle the context, it processes it; otherwise, it passes the context
	/// to the next link in the chain.
	/// </summary>
	/// <param name="context">The context to handle.</param>
	/// <returns>A <see cref="Handled{T}"/> instance containing the result if handled, or a default (unhandled) instance
	/// if not handled by this link or any subsequent link.</returns>
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
/// Base class for a responsible link in a chain of responsibility pattern with different input and output types.
/// A responsible link processes a context and can decide whether to handle it or pass it to the next link.
/// </summary>
/// <typeparam name="T">The type of the input context being handled.</typeparam>
/// <typeparam name="U">The type of the output result after handling the context.</typeparam>
public abstract class ResponsibleLinkBase<T, U> : ChainableBase<IResponsibleLink<T, U>>, IResponsibleLink<T, U>
{
	/// <summary>
	/// Determines if this link can handle the given context.
	/// </summary>
	/// <param name="context">The context to evaluate.</param>
	/// <returns><c>true</c> if this link can handle the context; otherwise, <c>false</c>.</returns>
	protected abstract bool CanHandle(T context);

	/// <summary>
	/// Performs the actual handling of the input context and produces an output.
	/// </summary>
	/// <remarks>Only called when <see cref="CanHandle"/> returns <c>true</c>.</remarks>
	/// <param name="context">The input context to handle.</param>
	/// <returns>The output result after handling the context.</returns>
	protected abstract U DoHandle(T context);

	/// <summary>
	/// Handles the given input context. If this link can handle the context, it processes it and returns an output;
	/// otherwise, it passes the context to the next link in the chain.
	/// </summary>
	/// <param name="context">The input context to handle.</param>
	/// <returns>A <see cref="Handled{U}"/> instance containing the result if handled, or a default (unhandled) instance
	/// if not handled by this link or any subsequent link.</returns>
	public Handled<U> Handle(T context)
	{
		return CanHandle(context)
			// handled by this link
			? new Handled<U>(DoHandle(context))
			// handled by next (or next.next, ...) or unhandled (default(Handled<T>)) if last
			: Next?.Handle(context) ?? default;
	}
}