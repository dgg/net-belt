using Net.Belt.Patterns.ChainOfResponsibility;

namespace Net.Belt.Tests.Patterns.ChainOfResponsibility.Support;

internal class AsyncLink<T> : ResponsibleAsyncLinkBase<T>
{
	protected override ValueTask<bool> CanHandle(T context, CancellationToken cancellationToken) =>
		ValueTask.FromResult(true);

	protected override Task<T> DoHandle(T context, CancellationToken cancellationToken) => Task.FromResult(context);
}