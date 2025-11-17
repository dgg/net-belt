namespace Net.Belt.Patterns.Pipeline;

/// <summary>
/// Defines a step in a pipeline that can be executed and rolled back.
/// </summary>
/// <typeparam name="TContext">The type of context passed through the pipeline.</typeparam>
public interface IPipelineStep<in TContext>
{
	/// <summary>
	/// Executes the pipeline step with the provided context.
	/// </summary>
	/// <param name="context">The context passed through the pipeline.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	Task ExecuteAsync(TContext context, CancellationToken cancellationToken);

	/// <summary>
	/// Rolls back the pipeline step with the provided context.
	/// </summary>
	/// <param name="context">The context passed through the pipeline.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	Task RollbackAsync(TContext context, CancellationToken cancellationToken);

	/// <summary>
	/// Gets the error key associated with this pipeline step.
	/// </summary>
	string? ErrorKey { get; }
}