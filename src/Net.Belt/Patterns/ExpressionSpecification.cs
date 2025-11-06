using System.Linq.Expressions;

namespace Net.Belt.Patterns;

/// <summary>
/// Represents a specification for instances of <typeparamref name="T"/> that is defined by a LINQ
/// expression.
/// The specification can be evaluated either as an expression tree or as a compiled delegate,
/// and can be combined with other <see cref="ExpressionSpecification{T}"/> instances using
/// logical operators while preserving an expression-tree representation suitable for LINQ providers.
/// </summary>
/// <typeparam name="T">The type of the entity the specification applies to.</typeparam>
public class ExpressionSpecification<T>(Expression<Func<T, bool>> expression) : Specification<T>
{
	/// <summary>
	/// Gets the original expression that defines this specification.
	/// This expression is retained so that the specification can be combined with others as an
	/// expression tree and can be translated by LINQ providers that understand expression trees.
	/// </summary>
	/// <value>The expression tree representing the specification predicate.</value>
	public Expression<Func<T, bool>> Expression { get => expression; }

	private Func<T, bool>? _predicate;

	/// <summary>
	/// Gets a compiled delegate (<c>Func&lt;T, bool&gt;</c>) for the specification.
	/// The delegate is compiled lazily on first access and cached for subsequent evaluations.
	/// Use this property to evaluate the specification in-memory (for example, against objects
	/// in collections) rather than as an expression tree.
	/// </summary>
	/// <remarks>
	/// Calling this property causes <see cref="Expression"/> to be compiled. If you intend to
	/// remain in expression-tree form for provider translation (e.g., IQueryable), prefer using
	/// the <see cref="Expression"/> property instead of repeatedly calling this delegate.
	/// </remarks>
	public Func<T, bool> Function { get { return _predicate ??= expression.Compile(); } }

	/// <summary>
	/// Determines whether the specified <paramref name="entity"/> satisfies the specification.
	/// This override evaluates the compiled delegate returned by the <see cref="Function"/> property.
	/// </summary>
	/// <param name="entity">The entity to test against the specification.</param>
	/// <returns><c>true</c> if the entity satisfies the predicate; otherwise <c>false</c>.</returns>
	public override bool IsSatisfiedBy(T entity) => Function(entity);

	#region operators

	#region conversion

	/// <summary>
	/// Implicitly converts an <see cref="ExpressionSpecification{T}"/> to a <c>Func&lt;T, bool&gt;</c>.
	/// The returned delegate is the compiled predicate (equivalent to the <see cref="Function"/> property).
	/// This conversion compiles the expression if it has not already been compiled.
	/// </summary>
	/// <param name="specification">The specification to convert.</param>
	/// <returns>A compiled delegate representing the specification predicate.</returns>
	public static implicit operator Func<T, bool>(ExpressionSpecification<T> specification) => specification.Function;

	/// <summary>
	/// Implicitly converts an <see cref="ExpressionSpecification{T}"/> to its underlying
	/// expression (<c>Expression&lt;Func&lt;T, bool&gt;&gt;</c>).
	/// This conversion returns the original, uncompiled expression tree retained by the specification.
	/// </summary>
	/// <param name="specification">The specification to convert.</param>
	/// <returns>The expression tree that defines the specification.</returns>
	public static implicit operator Expression<Func<T, bool>>(ExpressionSpecification<T> specification) =>
		specification.Expression;

	/// <summary>
	/// Implicitly converts an <see cref="ExpressionSpecification{T}"/> to a <c>Predicate&lt;T&gt;</c>.
	/// The returned predicate invokes the compiled delegate, so conversion triggers compilation if needed.
	/// </summary>
	/// <param name="spec">The specification to convert.</param>
	/// <returns>A <see cref="Predicate{T}"/> that invokes the specification's compiled function.</returns>
	public static implicit operator Predicate<T>(ExpressionSpecification<T> spec) => t => spec.Function(t);

	#endregion

	/// <summary>
	/// Returns a new <see cref="ExpressionSpecification{T}"/> that represents the logical negation
	/// of this specification. The negation is performed at the expression-tree level, producing a new
	/// expression equivalent to applying a logical NOT to the original expression's body.
	/// </summary>
	/// <param name="spec">The specification to negate.</param>
	/// <returns>A new specification representing the negated predicate.</returns>
	/// <remarks>
	/// The resulting specification keeps the expression-tree form, which makes it suitable for
	/// further composition or for translation by LINQ providers. Providers that do not support
	/// certain expression shapes should be considered when composing complex expressions.
	/// </remarks>
	public static ExpressionSpecification<T> operator !(ExpressionSpecification<T> spec)
	{
		var newExpression =
			System.Linq.Expressions.Expression.MakeUnary(ExpressionType.Not, spec.Expression.Body, typeof(bool));

		return new ExpressionSpecification<T>(toLambda(newExpression, spec.Expression.Parameters));
	}

	/// <summary>
	/// Returns a new <see cref="ExpressionSpecification{T}"/> representing the logical AND (short-circuiting)
	/// of two specifications. The resulting expression preserves the left-hand parameters and invokes the
	/// right-hand expression with those parameters to form a combined expression using <c>AndAlso</c>.
	/// </summary>
	/// <param name="leftSide">The left operand specification.</param>
	/// <param name="rightSide">The right operand specification.</param>
	/// <returns>A new specification representing the conjunction of the two predicates.</returns>
	/// <remarks>
	/// The implementation uses <see cref="M:Expression.Invoke(Expression,IEnumerable{.Expression})"/> to incorporate
	/// the right-hand expression into the left-hand parameter set. Some LINQ providers do not support
	/// <c>Invoke</c>, so the composed expression might not be translatable by all providers.
	/// </remarks>
	public static ExpressionSpecification<T> operator &(ExpressionSpecification<T> leftSide,
		ExpressionSpecification<T> rightSide)
	{
		var newExpression = mergeIntoBinary(rightSide.Expression, leftSide.Expression,
			ExpressionType.AndAlso);

		return new ExpressionSpecification<T>(toLambda(newExpression, leftSide.Expression.Parameters));
	}

	/// <summary>
	/// Returns a new <see cref="ExpressionSpecification{T}"/> representing the logical OR (short-circuiting)
	/// of two specifications. The resulting expression preserves the left-hand parameters and invokes the
	/// right-hand expression with those parameters to form a combined expression using <c>OrElse</c>.
	/// </summary>
	/// <param name="leftSide">The left operand specification.</param>
	/// <param name="rightSide">The right operand specification.</param>
	/// <returns>A new specification representing the disjunction of the two predicates.</returns>
	/// <remarks>
	/// The implementation uses <see cref="M:Expression.Invoke(Expression,IEnumerable{.Expression})"/> to incorporate
	/// the right-hand expression into the left-hand parameter set. Some LINQ providers do not support
	/// <c>Invoke</c>, so the composed expression might not be translatable by all providers.
	/// </remarks>
	public static ExpressionSpecification<T> operator |(ExpressionSpecification<T> leftSide,
		ExpressionSpecification<T> rightSide)
	{
		var newExpression = mergeIntoBinary(rightSide.Expression, leftSide.Expression,
			ExpressionType.OrElse);

		return new ExpressionSpecification<T>(toLambda(newExpression, leftSide.Expression.Parameters));
	}


	/// <summary>
	/// Determines whether the specified <paramref name="specification"/> represents a condition that is always true.
	/// </summary>
	/// <param name="specification">The specification to evaluate for truthiness.</param>
	/// <returns><c>true</c> if the specification evaluates to a condition that is always true; otherwise, <c>false</c>.</returns>
	public static bool operator true(ExpressionSpecification<T> specification) => false;

	/// <summary>
	/// Determines the logical false value for the specified <paramref name="specification"/> instance.
	/// This operator is utilized in certain logical expressions to evaluate the specification as a
	/// boolean 'false' when necessary.
	/// </summary>
	/// <param name="specification">The specification to evaluate as false.</param>
	/// <returns>Always returns <c>false</c>.</returns>
	public static bool operator false(ExpressionSpecification<T> specification) => false;

	#endregion

	#region operator-based methods

	/// <summary>
	/// Returns a new specification that is the logical negation of the current instance.
	/// This is a convenience wrapper around the unary <c>!</c> operator overload.
	/// </summary>
	/// <returns>A new <see cref="ExpressionSpecification{T}"/> representing the negated predicate.</returns>
	public new ExpressionSpecification<T> Not() => !(this);

	/// <summary>
	/// Returns a new specification that is the logical conjunction of this specification with the supplied <paramref name="right"/> specification.
	/// This is a convenience wrapper around the binary <c>&amp;</c> operator overload.
	/// </summary>
	/// <param name="right">The specification to combine with via logical AND.</param>
	/// <returns>A new <see cref="ExpressionSpecification{T}"/> representing the combined predicate.</returns>
	public ExpressionSpecification<T> And(ExpressionSpecification<T> right) => this & right;

	/// <summary>
	/// Returns a new specification that is the logical disjunction of this specification with the supplied <paramref name="right"/> specification.
	/// This is a convenience wrapper around the binary <c>|</c> operator overload.
	/// </summary>
	/// <param name="right">The specification to combine with via logical OR.</param>
	/// <returns>A new <see cref="ExpressionSpecification{T}"/> representing the combined predicate.</returns>
	public ExpressionSpecification<T> Or(ExpressionSpecification<T> right) => this | right;

	#endregion

	private static Expression<Func<T, bool>> toLambda(Expression expression,
		IEnumerable<ParameterExpression> parameters) =>
		System.Linq.Expressions.Expression.Lambda<Func<T, bool>>(expression, parameters);

	private static BinaryExpression mergeIntoBinary(Expression<Func<T, bool>> right, Expression<Func<T, bool>> left,
		ExpressionType type)
	{
		InvocationExpression rightInvoke = System.Linq.Expressions.Expression.Invoke(right, left.Parameters);

		BinaryExpression mergedExpression = System.Linq.Expressions.Expression.MakeBinary(type, left.Body, rightInvoke);

		return mergedExpression;
	}
}