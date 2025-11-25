namespace Net.Belt.Patterns.Visitor;

/// <summary>
/// Represents an element that can be "visited" by a <see cref="IVisitor{TBase}"/>.
/// </summary>
/// <remarks>"Visit" in this context means to perform a (potentially different) operation on a <typeparamref name="TBase"/> subtype.</remarks>
/// <typeparam name="TBase">The base type of the elements that can be visited.</typeparam>
public interface IVisitable<out TBase>
{
	/// <summary>
	/// Accepts a <paramref name="visitor"/> to process this instance.
	/// </summary>
	/// <param name="visitor">The visitor that will process this instance.</param>
	void Accept(IVisitor<TBase> visitor);
}