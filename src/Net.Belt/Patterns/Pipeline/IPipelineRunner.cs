namespace Net.Belt.Patterns.Pipeline;

/// <summary>
/// Defines a contract for executing a pipeline of steps against a context.
/// </summary>
/// <typeparam name="TContext">The type of context passed through the pipeline.</typeparam>
public interface IPipelineRunner<in TContext>
{
	/// <summary>
	/// Gets the collection of pipeline steps to be executed.
	/// </summary>
	IReadOnlyCollection<IPipelineStep<TContext>> Steps { get; }

	/// <summary>
	/// Executes the pipeline steps against the provided context.
	/// </summary>
	/// <param name="context">The context to pass through the pipeline.</param>
	/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	Task ExecuteAsync(TContext context, CancellationToken cancellationToken);
}