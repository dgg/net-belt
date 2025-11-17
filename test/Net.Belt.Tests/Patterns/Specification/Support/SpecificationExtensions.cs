using NUnit.Framework.Constraints;

namespace Net.Belt.Tests.Patterns.Specification.Support;

internal class Iz: Is
{
	public static SpecificationConstraint<T> SatisfiedBy<T>(T value) => new(value, true);
}

internal static class CustomExtensions
{
	public static SpecificationConstraint<T> SatisfiedBy<T>(this ConstraintExpression expression, T expected)
	{
		var constraint = new SpecificationConstraint<T>(expected, true);
		expression.Append(constraint);
		return constraint;
	}
}