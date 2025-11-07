using Net.Belt.Comparisons;

namespace Net.Belt.Tests.Comparisons.Support;

internal class PropertySEqualizer : ChainableEqualizer<EqualitySubject>
{
	protected override bool DoEquals(EqualitySubject x, EqualitySubject y) => x.S!.Equals(y.S);

	protected override int DoGetHashCode(EqualitySubject obj) => obj.S!.GetHashCode();
}