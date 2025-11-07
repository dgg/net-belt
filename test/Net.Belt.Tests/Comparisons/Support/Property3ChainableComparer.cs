using Net.Belt.Comparisons;

namespace Net.Belt.Tests.Comparisons.Support;

internal class Property3ChainableComparer(Direction sortDirection = default) : ChainableComparer<ComparisonSubject>(sortDirection)
{
	protected override int DoCompare(ComparisonSubject x, ComparisonSubject y) => x.Property3.CompareTo(y.Property3);
}