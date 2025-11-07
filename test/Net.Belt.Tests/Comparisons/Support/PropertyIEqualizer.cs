using Net.Belt.Comparisons;

namespace Net.Belt.Tests.Comparisons.Support;

internal class PropertyIEqualizer : ChainableEqualizer<EqualitySubject>
{
	protected override bool DoEquals(EqualitySubject x, EqualitySubject y) => x.I.Equals(y.I);

	protected override int DoGetHashCode(EqualitySubject obj) => obj.I.GetHashCode();
}