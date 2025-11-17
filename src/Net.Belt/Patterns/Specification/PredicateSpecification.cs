namespace Net.Belt.Patterns.Specification;

/// <summary>
/// Represents a concrete implementation of the Specification design pattern that evaluates
/// whether an object satisfies a specific predicate. This class provides functionality to define
/// and combine business rules in a composable way, leveraging logical operators and predicate-based logic.
/// </summary>
/// <typeparam name="T">The type of objects to evaluate against the specification.</typeparam>
public class PredicateSpecification<T>(Predicate<T> predicate) : Specification<T>
{
	/// <summary>
	/// Determines whether the specification is satisfied by the given item.
	/// </summary>
	/// <param name="item">The item to evaluate against the specification.</param>
	/// <returns>True if the item satisfies the specification; otherwise, false.</returns>
	public override bool IsSatisfiedBy(T item) => predicate(item);

	/// <summary>
	/// Gets the underlying predicate function used to evaluate the specification.
	/// This property provides direct access to the predicate logic, enabling
	/// flexibility in evaluating or reusing the predicate outside of the specification context.
	/// </summary>
	public Predicate<T> Predicate => predicate;

	/// <summary>
	/// Provides a function representation of the underlying predicate logic.
	/// This property allows the predicate to be utilized as a delegate, offering
	/// compatibility with functional programming constructs and seamless integration
	/// with methods expecting a Func delegate.
	/// </summary>
	public Func<T, bool> Function => new(t => predicate(t));

	#region operators

	/// <summary>
	/// Combines two predicate specifications using a logical AND operation.
	/// </summary>
	/// <param name="left">The left-hand operand of the operator.</param>
	/// <param name="right">The right-hand operand of the operator.</param>
	/// <returns>A new predicate specification that is satisfied if both the left and right specifications are satisfied.</returns>
	public static PredicateSpecification<T> operator
		&(PredicateSpecification<T> left, PredicateSpecification<T> right) => new(t => left.IsSatisfiedBy(t) && right.IsSatisfiedBy(t));

	/// <summary>
	/// Defines the logical OR operator for combining two PredicateSpecification instances.
	/// </summary>
	/// <param name="left">The left PredicateSpecification operand.</param>
	/// <param name="right">The right PredicateSpecification operand.</param>
	/// <returns>A new PredicateSpecification instance combining the logical evaluation of
	/// both operands using the OR operator.</returns>
	public static PredicateSpecification<T> operator
		|(PredicateSpecification<T> left, PredicateSpecification<T> right) => new(t => left.IsSatisfiedBy(t) || right.IsSatisfiedBy(t));
	
	/// <summary>
	/// Defines the logical NOT operator for negating a PredicateSpecification instance.
	/// </summary>
	/// <param name="specification">The PredicateSpecification to negate.</param>
	/// <returns>A new PredicateSpecification instance that represents the logical negation of the input specification.</returns>
	public static PredicateSpecification<T> operator !(PredicateSpecification<T> specification) => new(t => !specification.IsSatisfiedBy(t));
	
	/// <summary>
	/// Required operator overload for enabling short-circuit evaluation in conjunction with other logical operators.
	/// This operator always returns false as the actual evaluation is handled by IsSatisfiedBy.
	/// </summary>
	/// <param name="specification">The specification to evaluate.</param>
	/// <returns>Always returns false.</returns>
	public static bool operator true(PredicateSpecification<T> specification) => false;

	/// <summary>
	/// Required operator overload for enabling short-circuit evaluation in conjunction with other logical operators.
	/// This operator always returns false as the actual evaluation is handled by IsSatisfiedBy.
	/// </summary>
	/// <param name="specification">The specification to evaluate.</param>
	/// <returns>Always returns false.</returns>
	public static bool operator false(PredicateSpecification<T> specification) => false;

	/// <summary>
	/// Defines an implicit conversion from PredicateSpecification to Predicate delegate.
	/// </summary>
	/// <param name="spec">The specification to convert.</param>
	/// <returns>The underlying predicate of the specification.</returns>
	public static implicit operator Predicate<T>(PredicateSpecification<T> spec) => spec.Predicate;

	/// <summary>
	/// Defines an implicit conversion from PredicateSpecification to Func&lt;T, bool&gt; delegate.
	/// </summary>
	/// <param name="spec">The specification to convert.</param>
	/// <returns>The underlying predicate exposed as a Func delegate.</returns>
	public static implicit operator Func<T, bool>(PredicateSpecification<T> spec) => spec.Function;

	#endregion
}