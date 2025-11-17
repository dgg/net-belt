namespace Net.Belt.Tests.Patterns.Pipelining.Support;

internal class CannotRollbackStep(string Message) : ExposingStep
{
	public override Task ExecuteAsync(AContext context, CancellationToken cancellationToken)
	{
		Executed = true;
		return Task.CompletedTask;
	}

	public override Task RollbackAsync(AContext context, CancellationToken cancellationToken)
	{
		Rollbacked = true;
		throw new Exception(Message);
	}
}