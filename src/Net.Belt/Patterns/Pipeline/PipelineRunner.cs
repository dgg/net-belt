namespace Net.Belt.Patterns.Pipeline;

/// <summary>
/// Executes a pipeline of steps in sequence, with rollback support on failure.
/// </summary>
/// <typeparam name="TContext">The type of context passed through the pipeline.</typeparam>
public class PipelineRunner<TContext> : IPipelineRunner<TContext>
{
    /// <summary>
    /// Gets the collection of pipeline steps to be executed.
    /// </summary>
    public IReadOnlyCollection<IPipelineStep<TContext>> Steps { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PipelineRunner{TContext}"/> class with an enumerable of pipeline steps.
    /// </summary>
    /// <param name="steps">An enumerable of pipeline steps to execute in sequence.</param>
    public PipelineRunner(IEnumerable<IPipelineStep<TContext>> steps) : this(steps.ToArray()) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="PipelineRunner{TContext}"/> class with an array of pipeline steps.
    /// </summary>
    /// <param name="steps">An array of pipeline steps to execute in sequence.</param>
    public PipelineRunner(params IPipelineStep<TContext>[] steps)
    {
        Steps = steps.AsReadOnly();
    }

    /// <summary>
    /// Executes the pipeline steps against the provided context, with rollback support on failure.
    /// </summary>
    /// <param name="context">The context to pass through the pipeline.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="PipelineException">Thrown when a pipeline step fails and rollback is necessary.</exception>
    public async Task ExecuteAsync(TContext context, CancellationToken cancellationToken)
    {
        var executedSteps = new Stack<IPipelineStep<TContext>>();
        
        int index = 0;
        foreach (var step in Steps)
        {
            try
            {
                await step.ExecuteAsync(context, cancellationToken);
            }
            catch (Exception stepException)
            {
                executedSteps.Push(step);
                var pipelineException = new PipelineException(index, stepException);

                await rollbackAsync(executedSteps, context, pipelineException, cancellationToken);

                throw pipelineException;
            }
            index++;
            executedSteps.Push(step);
        }
    }

    private static async Task rollbackAsync(
        Stack<IPipelineStep<TContext>> executedSteps, TContext context,
        PipelineException pipelineException, CancellationToken cancellationToken)
    {
        while (executedSteps.Count > 0)
        {
            var step = executedSteps.Pop();

            try
            {
                await step.RollbackAsync(context, cancellationToken);
            }
            catch (Exception rollbackException)
            {
                int index = executedSteps.Count;
                pipelineException.AddRollbackException(index, rollbackException);
            }
        }
    }
}