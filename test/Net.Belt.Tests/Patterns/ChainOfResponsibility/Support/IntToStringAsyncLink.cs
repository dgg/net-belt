using System.Globalization;

using Net.Belt.Patterns.ChainOfResponsibility;

namespace Net.Belt.Tests.Patterns.ChainOfResponsibility.Support;

internal class IntToStringAsyncLink(int contextToHandle) : ResponsibleAsyncLinkBase<int, string>
{
	protected override ValueTask<bool> CanHandle(int context, CancellationToken cancellationToken) =>
		ValueTask.FromResult(context == contextToHandle);

	protected override Task<string> DoHandle(int context, CancellationToken cancellationToken) =>
		Task.FromResult(context.ToString(CultureInfo.InvariantCulture));
}