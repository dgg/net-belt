using Net.Belt.Patterns.ChainOfResponsibility;

namespace Net.Belt.Tests.Patterns.ChainOfResponsibility.Support;

internal class RevealingLink(Func<string, bool> canHandle, Func<string, string> doHandle) : ResponsibleLinkBase<string>
{
	protected override bool CanHandle(string context) => canHandle(context);

	protected override string DoHandle(string context) => doHandle(context);
}