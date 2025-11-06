using System.Linq.Expressions;

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

/// <summary>
/// Provides functionality to construct and retrieve instances of <see cref="ExpressionSpecification{T}"/>.
/// The created specifications enable the utilization of LINQ-based expressions
/// to evaluate whether entities of a specific type meet certain criteria.
/// These specifications can further support logical operations and chaining
/// when combined with other specifications.
/// </summary>
public static class ExpressionSpecification
{
	/// <summary>
	/// Creates a new instance of <see cref="ExpressionSpecification{T}"/> using the provided LINQ expression.
	/// This method enables the construction of specifications that encapsulate a predicate,
	/// allowing for the evaluation of whether entities of type <typeparamref name="T"/>
	/// satisfy the given criteria.
	/// </summary>
	/// <typeparam name="T">The type of the entities to be evaluated by the specification.</typeparam>
	/// <param name="expression">A LINQ expression that defines the condition or rule for the specification.
	/// This expression determines if a given entity satisfies the specification criteria.</param>
	/// <returns>An instance of <see cref="ExpressionSpecification{T}"/> initialized with the provided expression.</returns>
	public static ExpressionSpecification<T> For<T>(Expression<Func<T, bool>> expression) => new(expression);
}