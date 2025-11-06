namespace Net.Belt.Patterns;

/// <summary>
/// Represents the entry point for creating predicate-based specifications.
/// This class provides a factory method to initialize a <see cref="PredicateSpecification{T}"/>
/// with a given predicate, enabling the use of the Specification design pattern
/// for evaluating whether an object satisfies specific business rules.
/// </summary>
public static class PredicateSpecification
{
	/// <summary>
	/// Creates a new instance of <see cref="PredicateSpecification{T}"/> using the provided predicate.
	/// This method serves as a factory for initializing specifications with custom business rules
	/// defined as predicates, enabling the Specification pattern for flexible rule evaluation.
	/// </summary>
	/// <typeparam name="T">The type of the objects to be evaluated by the specification.</typeparam>
	/// <param name="predicate">A predicate function that defines the rule for the specification.
	/// The predicate evaluates whether the given object satisfies the condition.</param>
	/// <returns>A new instance of <see cref="PredicateSpecification{T}"/> initialized with the provided predicate.</returns>
	public static PredicateSpecification<T> For<T>(Predicate<T> predicate) => new(predicate);
}