namespace Net.Belt.Patterns.ChainOfResponsibility;

/// <summary>
/// Represents a link in a chain of responsibility that can handle a context of type <typeparamref name="T"/>.
/// This link uses delegated functions for determining if it can handle a context and for handling it.
/// </summary>
/// <param name="canHandle">A function that determines if this link can handle the given context. Returns <c>true</c> if it can handle, <c>false</c> otherwise.</param>
/// <param name="doHandle">A function that processes the context if this link can handle it.</param>
/// <typeparam name="T">The type of the context being handled.</typeparam>
public class ResponsibleLink<T>(Func<T, bool> canHandle, Func<T, T> doHandle) : ResponsibleLinkBase<T>
{
	/// <summary>
	/// Determines if this link can handle the specified <paramref name="context"/>.
	/// </summary>
	/// <param name="context">The context to evaluate.</param>
	/// <returns><c>true</c> if this link can handle the context; otherwise, <c>false</c>.</returns>
	protected override bool CanHandle(T context) => canHandle(context);

	/// <summary>
	/// Handles the specified <paramref name="context"/> by applying the delegated handling logic.
	/// </summary>
	/// <param name="context">The context to handle.</param>
	/// <returns>The processed context.</returns>
	protected override T DoHandle(T context) => doHandle(context);
}

/// <summary>
/// Represents a link in a chain of responsibility that can handle a context of type <typeparamref name="T"/>
/// and produce a result of type <typeparamref name="U"/>.
/// This link uses delegated functions for determining if it can handle a context and for handling it.
/// </summary>
/// <param name="canHandle">A function that determines if this link can handle the given context. Returns <c>true</c> if it can handle, <c>false</c> otherwise.</param>
/// <param name="doHandle">A function that processes the context if this link can handle it, returning a result of type <typeparamref name="U"/>.</param>
/// <typeparam name="T">The type of the context being handled.</typeparam>
/// <typeparam name="U">The type of the result produced by handling the context.</typeparam>
public class ResponsibleLink<T, U>(Func<T, bool> canHandle, Func<T, U> doHandle) : ResponsibleLinkBase<T, U>
{
	/// <summary>
	/// Determines if this link can handle the specified <paramref name="context"/>.
	/// </summary>
	/// <param name="context">The context to evaluate.</param>
	/// <returns><c>true</c> if this link can handle the context; otherwise, <c>false</c>.</returns>
	protected override bool CanHandle(T context) => canHandle(context);

	/// <summary>
	/// Handles the specified <paramref name="context"/> by applying the delegated handling logic.
	/// </summary>
	/// <param name="context">The context to handle.</param>
	/// <returns>The processed context, which is of type <typeparamref name="U"/>.</returns>
	protected override U DoHandle(T context) => doHandle(context);
}
