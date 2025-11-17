using Net.Belt.Patterns.Pipeline;

namespace Net.Belt.Tests.Patterns.Pipelining.Support;

internal abstract class ExposingStep : IPipelineStep<AContext>
{
	public bool Executed { get; protected set; }
	public bool Rollbacked { get; protected set; }
	public abstract Task ExecuteAsync(AContext context, CancellationToken cancellationToken);
	public abstract Task RollbackAsync(AContext context, CancellationToken cancellationToken);
	public string? ErrorKey { get; }
}