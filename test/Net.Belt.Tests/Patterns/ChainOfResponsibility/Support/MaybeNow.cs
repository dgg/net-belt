using Net.Belt.Patterns.ChainOfResponsibility;

namespace Net.Belt.Tests.Patterns.ChainOfResponsibility.Support;

internal class MaybeNow(TimeProvider time) : ResponsibleLinkBase<string, DateTimeOffset?>
{
	protected override bool CanHandle(string context) => context.Equals("now", StringComparison.Ordinal);
	
	protected override DateTimeOffset? DoHandle(string _) => time.GetUtcNow();
}