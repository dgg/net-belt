using Net.Belt.Patterns;

using NUnit.Framework.Constraints;

namespace Net.Belt.Tests.Patterns.Support;

internal class SpecificationConstraint<T>(T value, bool satisfied) : Constraint
{
	private readonly List<T> _values = [value];

	public override ConstraintResult ApplyTo<TActual>(TActual actual)
	{
		ConstraintResult result = new ConstraintResult(this, actual, true);

		var equal = new EqualConstraint(satisfied);
		ISpecification<T> spec = (ISpecification<T>)actual!;
		foreach (var value in _values)
		{
			result = new SpecificationResult(equal, equal.ApplyTo(spec.IsSatisfiedBy(value)));
			if (!result.IsSuccess)
			{
				break;
			}
		}
		
		return result;
	}

	public override string Description => "specification satisfied";

	public new SpecificationConstraint<T> Or(T value)
	{
		_values.Add(value);
		return this;
	}

	public new SpecificationConstraint<T> And(T value)
	{
		_values.Add(value);
		return this;
	}

	class SpecificationResult(IConstraint constraint, ConstraintResult result)
		: ConstraintResult(constraint, result.ActualValue, result.IsSuccess);
}