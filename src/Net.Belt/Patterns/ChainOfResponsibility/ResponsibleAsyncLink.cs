namespace Net.Belt.Patterns.ChainOfResponsibility;

/// <summary>
/// Represents an asynchronous link in a chain of responsibility that can handle a context of type <typeparamref name="T"/>
/// and produce a result of type <typeparamref name="U"/>.
/// </summary>
/// <param name="canHandle">A function that determines if the link can handle the given context.</param>
/// <param name="doHandle">A function that performs the handling of the context and returns a result.</param>
/// <typeparam name="T">The type of the context being handled.</typeparam>
/// <typeparam name="U">The type of the result produced by handling the context.</typeparam>
public class ResponsibleAsyncLink<T, U>(
	Func<T, CancellationToken, ValueTask<bool>> canHandle,
	Func<T, CancellationToken, Task<U>> doHandle) : ResponsibleAsyncLinkBase<T, U>
{
	/// <summary>
	/// Determines if this link can handle the specified context asynchronously.
	/// </summary>
	/// <param name="context">The context to check.</param>
	/// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
	/// <returns><see langword="true"/> if the link can handle the context; otherwise, <see langword="false"/>.</returns>
	protected override ValueTask<bool> CanHandle(T context, CancellationToken cancellationToken) =>
		canHandle(context, cancellationToken);

	/// <summary>
	/// Handles the specified context asynchronously and produces a result.
	/// </summary>
	/// <param name="context">The context to handle.</param>
	/// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
	/// <returns>A <see cref="Task{U}"/> representing the asynchronous operation, containing the result of handling the context.</returns>
	protected override Task<U> DoHandle(T context, CancellationToken cancellationToken) =>
		doHandle(context, cancellationToken);
}

/// <summary>
/// Represents an asynchronous link in a chain of responsibility that can handle a context of type <typeparamref name="T"/>
/// and return the modified context.
/// </summary>
/// <param name="canHandle">A function that determines if the link can handle the given context.</param>
/// <param name="doHandle">A function that performs the handling of the context and returns the modified context.</param>
/// <typeparam name="T">The type of the context being handled.</typeparam>
public class ResponsibleAsyncLink<T>(
	Func<T, CancellationToken, ValueTask<bool>> canHandle,
	Func<T, CancellationToken, Task<T>> doHandle) : ResponsibleAsyncLinkBase<T>
{
	/// <summary>
	/// Determines if this link can handle the specified context asynchronously.
	/// </summary>
	/// <param name="context">The context to check.</param>
	/// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
	/// <returns><see langword="true"/> if the link can handle the context; otherwise, <see langword="false"/>.</returns>
	protected override ValueTask<bool> CanHandle(T context, CancellationToken cancellationToken) =>
		canHandle(context, cancellationToken);

	/// <summary>
	/// Handles the specified context asynchronously and returns the modified context.
	/// </summary>
	/// <param name="context">The context to handle.</param>
	/// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
	/// <returns>A <see cref="Task{T}"/> representing the asynchronous operation, containing the modified context.</returns>
	protected override Task<T> DoHandle(T context, CancellationToken cancellationToken) =>
		doHandle(context, cancellationToken);
}