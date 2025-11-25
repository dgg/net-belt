namespace Net.Belt.Patterns.Visitor;

/// <summary>
/// Provides a flexible implementation of the visitor pattern that allows
/// registration of visit methods for different concrete types using delegates.
/// </summary>
/// <typeparam name="TBase">The base type of the visitable objects this visitor can handle.
/// Must implement <see cref="IVisitable{TBase}"/>.</typeparam>
public class DelegatedVisitor<TBase> : IVisitor<TBase> where TBase : IVisitable<TBase>
{
	/// <summary>
	/// Represents a delegate for a visit method that can handle a specific subtype of <typeparamref name="TBase"/>.
	/// </summary>
	/// <param name="u">The visitable object of type <typeparamref name="TSub"/> to be visited.</param>
	/// <typeparam name="TSub">The concrete subtype of that this delegate can visit.</typeparam>
	public delegate void VisitDelegate<in TSub>(TSub u) where TSub : TBase;
	
	readonly Dictionary<RuntimeTypeHandle, VisitDelegate<TBase>> _visitors = new();

	/// <summary>
	/// Registers a visit method for a specific subtype <typeparamref name="TSub"/>.
	/// </summary>
	/// <param name="visitor">The delegate representing the visit method for <typeparamref name="TSub"/>.</param>
	/// <typeparam name="TSub">The concrete subtype of <typeparamref name="TBase"/> that this visitor will handle.
	/// Must implement <see cref="IVisitable{TBase}"/>.</typeparam>
	/// <returns>The current <see cref="DelegatedVisitor{TBase}"/> instance, allowing for fluent chaining.</returns>
	public DelegatedVisitor<TBase> AddVisitor<TSub>(VisitDelegate<TSub> visitor) where TSub : TBase, IVisitable<TBase>
	{
		_visitors.Add(typeof(TSub).TypeHandle, _adaptConcreteVisitor);
		return this;
		void _adaptConcreteVisitor(TBase b) => visitor((TSub)b);
	}

	/// <summary>
	/// Visits a specific visitable object, dispatching to the appropriate registered visit method.
	/// </summary>
	/// <param name="visitable">The object to be visited.</param>
	/// <typeparam name="TSub">The concrete type of the visitable object.
	/// Must be a subtype of <typeparamref name="TBase"/> and implement <see cref="IVisitable{TBase}"/>.</typeparam>
	public void Visit<TSub>(TSub visitable) where TSub : TBase, IVisitable<TBase>
	{
		RuntimeTypeHandle handle = typeof (TSub).TypeHandle;
		if (_visitors.TryGetValue(handle, out VisitDelegate<TBase>? visit))
		{
			visit(visitable);
		}
	}
}