namespace Net.Belt.Patterns.ChainOfResponsibility;

/// <summary>
/// Represents an asynchronous link in a chain of responsibility that handles a context of type <typeparamref name="T"/>
/// and produces a result of the same type.
/// </summary>
/// <typeparam name="T">The type of the context being handled.</typeparam>
public interface IResponsibleAsyncLink<T> : IChainable<IResponsibleAsyncLink<T>>
{
	/// <summary>
	/// Asynchronously handles the given context.
	/// </summary>
	/// <param name="context">The context to handle.</param>
	/// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
	/// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation, containing a <see cref="Handled{T}"/>
	/// indicating whether the context was handled and the resulting context.</returns>
	Task<Handled<T>> Handle(T context, CancellationToken cancellationToken = default);
}

/// <summary>
/// Represents an asynchronous link in a chain of responsibility that handles a context of type <typeparamref name="T"/>
/// and produces a result of type <typeparamref name="U"/>.
/// </summary>
/// <typeparam name="T">The type of the input context being handled.</typeparam>
/// <typeparam name="U">The type of the output context produced after handling.</typeparam>
public interface IResponsibleAsyncLink<T, U> : IChainable<IResponsibleAsyncLink<T, U>>
{
	/// <summary>
	/// Asynchronously handles the given context.
	/// </summary>
	/// <param name="context">The context to handle.</param>
	/// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
	/// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation, containing a <see cref="Handled{U}"/>
	/// indicating whether the context was handled and the resulting context.</returns>
	Task<Handled<U>> Handle(T context, CancellationToken cancellationToken = default);
}