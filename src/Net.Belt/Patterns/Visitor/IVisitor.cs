namespace Net.Belt.Patterns.Visitor;

/// <summary>
/// Represents a visitor that can operate on elements implementing <see cref="IVisitable{TBase}"/>.
/// </summary>
/// <typeparam name="TBase">The base type of the elements that this visitor can visit.</typeparam>
public interface IVisitor<in TBase>
{
	/// <summary>
	/// Visits a specific <paramref name="visitable"/> instance.
	/// </summary>
	/// <param name="visitable">The instance to visit.</param>
	/// <typeparam name="TSub">The concrete type of the visitable instance.</typeparam>
	void Visit<TSub>(TSub visitable) where TSub : TBase, IVisitable<TBase>;
}