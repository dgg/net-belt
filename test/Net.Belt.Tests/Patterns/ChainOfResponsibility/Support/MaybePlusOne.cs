using Net.Belt.Patterns.ChainOfResponsibility;

namespace Net.Belt.Tests.Patterns.ChainOfResponsibility.Support;

internal class MaybePlusOne(int lowerBound) : ResponsibleLinkBase<int?>
{
	protected override bool CanHandle(int? context) => context >= lowerBound;

	protected override int? DoHandle(int? context) => context + 1;
}