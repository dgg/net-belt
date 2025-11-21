using System.Diagnostics.CodeAnalysis;

namespace Net.Belt.Patterns.ChainOfResponsibility;

/// <summary>
/// Represents the result of a chain of responsibility link processing, indicating whether the context was handled.
/// </summary>
/// <typeparam name="T">The type of the context being handled.</typeparam>
public struct Handled<T>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="Handled{T}"/> struct, indicating that the context was handled.
	/// </summary>
	/// <param name="context">The context that was handled.</param>
	internal Handled(T context)
	{
		IsHandled = true;
		Context = context;
	}
	/// <summary>
	/// Gets the context that was handled.
	/// </summary>
	public T? Context { get; }
	
	/// <summary>
	/// Gets a value indicating whether the context was handled by a link in the chain.
	/// </summary>
	[MemberNotNullWhen(true, nameof(Context))]
	public bool IsHandled { get; }

	/// <summary>
	/// Gets a <see cref="Handled{T}"/> instance indicating that the context was not handled.
	/// </summary>
	public static Handled<T> Not => default;
}