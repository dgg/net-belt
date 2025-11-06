using Net.Belt.Patterns;

namespace Net.Belt.Tests.Patterns.Support;

internal class MoreThan10 : Specification<int>
{
	public override bool IsSatisfiedBy(int item) => item > 10;
}

internal class LessThan5 : Specification<int>
{
	public override bool IsSatisfiedBy(int item) => item < 5;
}

internal class LessThan10 : Specification<int>
{
	public override bool IsSatisfiedBy(int item) => item < 10;
}

internal class MoreThan5 : Specification<int>
{
	public override bool IsSatisfiedBy(int item) => item > 5;
}

internal class LengthBetween5And10 : Specification<string>
{
	public override bool IsSatisfiedBy(string item) => item.Length is >= 5 and <= 10;
}

internal class Foo_LengthOf2 : Specification<ComplexType>
{
	public override bool IsSatisfiedBy(ComplexType item) => item.Foo.Length == 2;
}

internal class Bar_Even : Specification<ComplexType>
{
	public override bool IsSatisfiedBy(ComplexType item) => item.Bar % 2 != 0;
}

internal class ComplexType_Enabled : Specification<ComplexType>
{
	public override bool IsSatisfiedBy(ComplexType item) => item.Enabled;
}