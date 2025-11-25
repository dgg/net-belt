namespace Net.Belt.Patterns.Visitor;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TBase"></typeparam>
public interface IVisitable<out TBase>
{
	/// <summary>
	/// 
	/// </summary>
	/// <param name="visitor"></param>
	void Accept(IVisitor<TBase> visitor);
}