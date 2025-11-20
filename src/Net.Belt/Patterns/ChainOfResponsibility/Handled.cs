using System.Diagnostics.CodeAnalysis;

namespace Net.Belt.Patterns.ChainOfResponsibility;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
public struct Handled<T>
{
	/// <summary>
	/// 
	/// </summary>
	/// <param name="context"></param>
	internal Handled(T context)
	{
		IsHandled = true;
		Context = context;
	}
	/// <summary>
	/// 
	/// </summary>
	public T? Context { get; }
	
	/// <summary>
	/// 
	/// </summary>
	[MemberNotNullWhen(true, nameof(Context))]
	public bool IsHandled { get; }

	/// <summary>
	/// 
	/// </summary>
	public static Handled<T> Not => default;
}