namespace Net.Belt.Patterns.Pipeline;

/// <summary>
/// Exception thrown when a step in the pipeline needs to rollback.
/// </summary>
public class StepRollbackException : Exception
{
	/// <summary>
	/// Initializes a new instance of the <see cref="StepRollbackException"/> class.
	/// </summary>
	public StepRollbackException() { }
	
	/// <summary>
	/// Initializes a new instance of the <see cref="StepRollbackException"/> class with a specified error message.
	/// </summary>
	/// <param name="message">The message that describes the error.</param>
	public StepRollbackException(string message) : base(message) { }
	
	/// <summary>
	/// Initializes a new instance of the <see cref="StepRollbackException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
	/// </summary>
	/// <param name="message">The message that describes the error.</param>
	/// <param name="inner">The exception that is the cause of the current exception.</param>
	public StepRollbackException(string message, Exception inner) : base(message, inner) { }
	
	/// <summary>
	/// Initializes a new instance of the <see cref="StepRollbackException"/> class with a step index and inner exception.
	/// </summary>
	/// <param name="stepIndex">The index of the step where rollback occurred.</param>
	/// <param name="inner">The exception that is the cause of the current exception.</param>
	public StepRollbackException(int stepIndex, Exception inner) : base($"Rollback exception in step [{stepIndex}]", inner)
	{
		StepIndex = stepIndex;
	}
	
	/// <summary>
	/// Gets the index of the step where the rollback occurred.
	/// </summary>
	public int StepIndex { get; init; }
}