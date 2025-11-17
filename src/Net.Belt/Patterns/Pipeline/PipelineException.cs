namespace Net.Belt.Patterns.Pipeline;

/// <summary>
/// Represents an exception that occurs during pipeline execution.
/// </summary>
public class PipelineException : Exception
{
	/// <summary>
	/// Initializes a new instance of the <see cref="PipelineException"/> class.
	/// </summary>
	public PipelineException() { }
	
	/// <summary>
	/// Initializes a new instance of the <see cref="PipelineException"/> class with a specified error message.
	/// </summary>
	/// <param name="message">The error message that explains the reason for the exception.</param>
	public PipelineException(string message) : base(message) { }
	
	/// <summary>
	/// Initializes a new instance of the <see cref="PipelineException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
	/// </summary>
	/// <param name="message">The error message that explains the reason for the exception.</param>
	/// <param name="inner">The exception that is the cause of the current exception.</param>
	public PipelineException(string message, Exception inner) : base(message, inner) { }
	
	private readonly List<StepRollbackException> _rollbackExceptions = new();
	
	/// <summary>
	/// Initializes a new instance of the <see cref="PipelineException"/> class with the step index and inner exception.
	/// </summary>
	/// <param name="stepIndex">The index of the step where the exception occurred.</param>
	/// <param name="inner">The exception that is the cause of the current exception.</param>
	public PipelineException(int stepIndex, Exception inner) : base($"Pipeline exception in step [{stepIndex}]", inner)
	{
		StepIndex = stepIndex;
	}

	/// <summary>
	/// Gets the index of the step where the exception occurred.
	/// </summary>
	public int StepIndex { get; init; }
	
	/// <summary>
	/// Gets a read-only list of rollback exceptions that occurred during pipeline rollback.
	/// </summary>
	public IReadOnlyList<StepRollbackException> RollbackExceptions => _rollbackExceptions.AsReadOnly();

	/// <summary>
	/// Adds a rollback exception to the collection.
	/// </summary>
	/// <param name="rollbackException">The rollback exception to add.</param>
	/// <returns>The current <see cref="PipelineException"/> instance.</returns>
	public PipelineException AddRollbackException(StepRollbackException rollbackException)
	{
		_rollbackExceptions.Add(rollbackException);
		return this;
	}

	/// <summary>
	/// Adds a rollback exception for the specified step index.
	/// </summary>
	/// <param name="stepIndex">The index of the step where the rollback exception occurred.</param>
	/// <param name="rollbackException">The rollback exception to add.</param>
	/// <returns>The current <see cref="PipelineException"/> instance.</returns>
	public PipelineException AddRollbackException(int stepIndex, Exception rollbackException) =>
		AddRollbackException(new StepRollbackException(stepIndex, rollbackException));
}