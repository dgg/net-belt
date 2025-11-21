namespace Net.Belt.Patterns.ChainOfResponsibility;

/// <summary>
/// Represents a link in a chain of responsibility that handles a context of type <typeparamref name="T"/>
/// and produces a result of the same type.
/// </summary>
/// <typeparam name="T">The type of the context being handled.</typeparam>
public interface IResponsibleLink<T> : IChainable<IResponsibleLink<T>>
{
	/// <summary>
	/// Handles the given context.
	/// </summary>
	/// <param name="context">The context to handle.</param>
	/// <returns>A <see cref="Handled{T}"/> indicating whether the context was handled and the resulting context.</returns>
	Handled<T> Handle(T context);
}

/// <summary>
/// Represents a link in a chain of responsibility that handles a context of type <typeparamref name="T"/>
/// and produces a result of type <typeparamref name="U"/>.
/// </summary>
/// <typeparam name="T">The type of the input context being handled.</typeparam>
/// <typeparam name="U">The type of the output context produced after handling.</typeparam>
public interface IResponsibleLink<T, U> : IChainable<IResponsibleLink<T, U>>
{
	/// <summary>
	/// Handles the given context.
	/// </summary>
	/// <param name="context">The context to handle.</param>
	/// <returns>A <see cref="Handled{U}"/> indicating whether the context was handled and the resulting context.</returns>
	Handled<U> Handle(T context);
}