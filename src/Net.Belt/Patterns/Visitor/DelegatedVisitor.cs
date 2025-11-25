namespace Net.Belt.Patterns.Visitor;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TBase"></typeparam>
public class DelegatedVisitor<TBase> : IVisitor<TBase>
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TSub"></typeparam>
	public delegate void VisitDelegate<in TSub>(TSub u) where TSub : TBase;
	
	readonly Dictionary<RuntimeTypeHandle, VisitDelegate<TBase>> _delegates = new();

	/// <summary>
	/// 
	/// </summary>
	/// <param name="visitor"></param>
	/// <typeparam name="TSub"></typeparam>
	/// <returns></returns>
	public DelegatedVisitor<TBase> AddVisitor<TSub>(VisitDelegate<TSub> visitor) where TSub : TBase
	{
		_delegates.Add(typeof(TSub).TypeHandle, _adapted);
		return this;
		void _adapted(TBase b) => visitor((TSub)b!);
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="visitable"></param>
	/// <typeparam name="TSub"></typeparam>
	public void Visit<TSub>(TSub visitable) where TSub : TBase
	{
		RuntimeTypeHandle handle = typeof (TSub).TypeHandle;
		if (_delegates.TryGetValue(handle, out VisitDelegate<TBase>? found))
		{
			found(visitable);
		}
	}
}