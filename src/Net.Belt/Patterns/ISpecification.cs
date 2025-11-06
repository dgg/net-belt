namespace Net.Belt.Patterns;

/// <summary>
/// Defines a contract for constructing specifications that encapsulate business rules
/// or criteria to be evaluated against a given object of type <typeparamref name="T"/>.
/// The specification pattern is used to encapsulate logic for querying objects,
/// providing a clean and reusable way to compose complex predicates or filters.
/// </summary>
/// <typeparam name="T">The type of object that the specification applies to.</typeparam>
public interface ISpecification<T>
{
	/// <summary>
	/// Evaluates whether the given object of type <typeparamref name="T"/> satisfies the criteria
	/// defined within the specification.
	/// </summary>
	/// <param name="item">The object of type <typeparamref name="T"/> to evaluate against the specification.</param>
	/// <returns>
	/// A boolean value indicating whether the specified object satisfies the criteria
	/// encapsulated by the specification.
	/// </returns>
	bool IsSatisfiedBy(T item);

	/// <summary>
	/// Combines the current specification with another specification using a logical "AND" operation,
	/// creating a new specification that is satisfied only if both the current and the given specifications are satisfied.
	/// </summary>
	/// <param name="other">The other specification to combine with the current specification.</param>
	/// <returns>
	/// A new specification that represents the logical "AND" combination of the current specification
	/// and the provided specification.
	/// </returns>
	ISpecification<T> And(ISpecification<T> other);

	/// <summary>
	/// Creates a new specification that represents the logical negation of the current specification.
	/// </summary>
	/// <returns>
	/// A new specification that is satisfied if and only if the current specification is not satisfied.
	/// </returns>
	ISpecification<T> Not();

	/// <summary>
	/// Combines the current specification with another specification using a logical "OR" operation,
	/// creating a new specification that is satisfied if either the current or the given specification is satisfied.
	/// </summary>
	/// <param name="other">The other specification to combine with the current specification.</param>
	/// <returns>
	/// A new specification that represents the logical "OR" combination of the current specification
	/// and the provided specification.
	/// </returns>
	ISpecification<T> Or(ISpecification<T> other);
}