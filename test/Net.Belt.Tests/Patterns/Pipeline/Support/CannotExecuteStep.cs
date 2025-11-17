namespace Net.Belt.Tests.Patterns.Pipelining.Support;

internal class CannotExecuteStep(string Message) : ExposingStep
{
	public override Task ExecuteAsync(AContext context, CancellationToken cancellationToken)
	{
		Executed = true;
		throw new Exception(Message);
	}

	public override Task RollbackAsync(AContext context, CancellationToken cancellationToken)
	{
		Rollbacked = true;
		return Task.CompletedTask;
	}
}