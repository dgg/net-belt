using Net.Belt.Patterns.ChainOfResponsibility;

namespace Net.Belt.Tests.Patterns.ChainOfResponsibility.Support;

internal class MaybeToUpper(string prefix) : ResponsibleLinkBase<string>
{
	protected override bool CanHandle(string context) => context.StartsWith(prefix, StringComparison.InvariantCulture);

	protected override string DoHandle(string context) => context.ToUpperInvariant();
}