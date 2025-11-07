using Net.Belt.Comparisons;

namespace Net.Belt.Tests.Comparisons.Support;

internal class Property2ChainableComparer(Direction sortDirection = default)
	: ChainableComparer<ComparisonSubject>(sortDirection)
{
	protected override int DoCompare(ComparisonSubject x, ComparisonSubject y) => x.Property2.CompareTo(y.Property2);
}