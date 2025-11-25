namespace Net.Belt.Patterns.Visitor;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TBase"></typeparam>
public interface IVisitor<in TBase>
{
	/// <summary>
	/// 
	/// </summary>
	/// <param name="visitable"></param>
	/// <typeparam name="TSub"></typeparam>
	void Visit<TSub>(TSub visitable) where TSub : TBase;
}