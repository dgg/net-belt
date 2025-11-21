namespace Net.Belt.Patterns.ChainOfResponsibility;

/// <summary>
/// Base class for an asynchronous responsible link in a chain of responsibility pattern.
/// This link can decide whether to handle a request or pass it to the next link.
/// </summary>
/// <typeparam name="T">The type of the context that flows through the chain.</typeparam>
public abstract class ResponsibleAsyncLinkBase<T> : ChainableBase<IResponsibleAsyncLink<T>>, IResponsibleAsyncLink<T>
{
	/// <summary>
	/// Determines whether this link can handle the specified context asynchronously.
	/// </summary>
	/// <param name="context">The context to handle.</param>
	/// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
	/// <returns><c>true</c> if this link can handle the context; otherwise, <c>false</c>.</returns>
	protected abstract ValueTask<bool> CanHandle(T context, CancellationToken cancellationToken);

	/// <summary>
	/// Handles the specified context asynchronously.
	/// </summary>
	/// <param name="context">The context to handle.</param>
	/// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
	/// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation, with the handled context as the result.</returns>
	protected abstract Task<T> DoHandle(T context, CancellationToken cancellationToken);

	/// <summary>
	/// Handles the specified context asynchronously. If this link can handle the context, it processes it;
	/// otherwise, it passes the context to the next link in the chain.
	/// </summary>
	/// <param name="context">The context to handle.</param>
	/// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
	/// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation, containing a <see cref="Handled{T}"/> instance.
	/// The <see cref="Handled{T}"/> will contain the processed context if handled by this or a subsequent link,
	/// or be unhandled if no link in the chain could process it.</returns>
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
/// Base class for an asynchronous responsible link in a chain of responsibility pattern,
/// where the input context type <typeparamref name="T"/> is different from the output type <typeparamref name="U"/>.
/// This link can decide whether to handle a request or pass it to the next link.
/// </summary>
/// <typeparam name="T">The type of the context that flows into the link.</typeparam>
/// <typeparam name="U">The type of the context that flows out of the link.</typeparam>
public abstract class ResponsibleAsyncLinkBase<T, U> : ChainableBase<IResponsibleAsyncLink<T, U>>, IResponsibleAsyncLink<T, U>
{
	/// <summary>
	/// Determines whether this link can handle the specified context asynchronously.
	/// </summary>
	/// <param name="context">The context to handle.</param>
	/// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
	/// <returns><c>true</c> if this link can handle the context; otherwise, <c>false</c>.</returns>
	protected abstract ValueTask<bool> CanHandle(T context, CancellationToken cancellationToken);

	/// <summary>
	/// Handles the specified context asynchronously and returns a result of type <typeparamref name="U"/>.
	/// </summary>
	/// <param name="context">The context to handle.</param>
	/// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
	/// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation, with the handled result of type <typeparamref name="U"/>.</returns>
	protected abstract Task<U> DoHandle(T context, CancellationToken cancellationToken);

	/// <summary>
	/// Handles the specified context asynchronously. If this link can handle the context, it processes it;
	/// otherwise, it passes the context to the next link in the chain.
	/// </summary>
	/// <param name="context">The context to handle.</param>
	/// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
	/// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation, containing a <see cref="Handled{U}"/> instance.
	/// The <see cref="Handled{U}"/> will contain the processed result of type <typeparamref name="U"/> if handled by this or a subsequent link,
	/// or be unhandled if no link in the chain could process it.</returns>
	public async Task<Handled<U>> Handle(T context, CancellationToken cancellationToken = default)
	{
		return await CanHandle(context, cancellationToken)
			// handled by this link
			? new Handled<U>(await DoHandle(context, cancellationToken))
			// handled by next (or next.next, ...) or unhandled (default(Handled<T>)) if last
			: await (Next?.Handle(context, cancellationToken) ?? Task.FromResult(default(Handled<U>)));
	}
}
	