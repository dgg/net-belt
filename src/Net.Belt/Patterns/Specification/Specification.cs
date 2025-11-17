namespace Net.Belt.Patterns.Specification;

/// <summary>
/// Represents a base class for specifications in the specification design pattern.
/// Provides functionality to compose specifications and evaluate whether objects satisfy the criteria
/// encapsulated by a specification. This class is intended to be inherited by concrete implementations
/// that define specific business rules or conditions.
/// </summary>
/// <typeparam name="T">The type of objects that this specification applies to.</typeparam>
public abstract class Specification<T> : ISpecification<T>
{
	/// <summary>
	/// Evaluates whether the specified object satisfies the criteria defined by the specification.
	/// </summary>
	/// <param name="item">The object to evaluate against the specification.</param>
	/// <returns>
	/// Returns <c>true</c> if the specified object meets the criteria of the specification; otherwise, <c>false</c>.
	/// </returns>
	public abstract bool IsSatisfiedBy(T item);

	/// <summary>
	/// Combines the current specification with another specification using a logical AND operation.
	/// </summary>
	/// <param name="other">The specification to combine with the current specification.</param>
	/// <returns>
	/// A new specification that is satisfied only if both the current specification
	/// and the provided specification are satisfied by the evaluated object.
	/// </returns>
	public virtual ISpecification<T> And(ISpecification<T> other) => new AndSpecification(this, other);

	/// <summary>
	/// Creates a new specification that represents the negation of the current specification.
	/// </summary>
	/// <returns>
	/// Returns a new specification that evaluates to <c>true</c> for objects that do not satisfy
	/// the criteria defined by the current specification, and <c>false</c> for objects that do.
	/// </returns>
	public virtual ISpecification<T> Not() => new NotSpecification(this);

	/// <summary>
	/// Combines the current specification with another specification using a logical OR operation.
	/// </summary>
	/// <param name="other">The specification to combine with the current specification.</param>
	/// <returns>
	/// A new specification that evaluates to <c>true</c> if either the current specification
	/// or the provided specification is satisfied.
	/// </returns>
	public virtual ISpecification<T> Or(ISpecification<T> other) => new OrSpecification(this, other);

	private class AndSpecification(ISpecification<T> leftSide, ISpecification<T> rightSide) : Specification<T>
	{
		public override bool IsSatisfiedBy(T item) => leftSide.IsSatisfiedBy(item) && rightSide.IsSatisfiedBy(item);
	}

	private class NotSpecification(ISpecification<T> specification) : Specification<T>
	{
		public override bool IsSatisfiedBy(T item) => !specification.IsSatisfiedBy(item);
	}

	private class OrSpecification(ISpecification<T> leftSide, ISpecification<T> rightSide) : Specification<T>
	{
		public override bool IsSatisfiedBy(T item) => leftSide.IsSatisfiedBy(item) || rightSide.IsSatisfiedBy(item);
	}
}