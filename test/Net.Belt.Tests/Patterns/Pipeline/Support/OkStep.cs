namespace Net.Belt.Tests.Patterns.Pipelining.Support;

internal class OkStep : ExposingStep
{
	public override Task ExecuteAsync(AContext context, CancellationToken cancellationToken)
	{
		Executed = true;
		return Task.CompletedTask;
	}

	public override Task RollbackAsync(AContext context, CancellationToken cancellationToken)
	{
		Rollbacked = true;
		return Task.CompletedTask;
	}
}