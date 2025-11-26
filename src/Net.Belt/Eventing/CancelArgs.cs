namespace Net.Belt.Eventing;

/// <summary>
/// Represents arguments for an event that can be canceled.
/// </summary>
public interface ICancelArgs
{
	/// <summary>
	/// Gets a value indicating whether the event has been canceled.
	/// </summary>
	/// <value><c>true</c> if the event is canceled; otherwise, <c>false</c>.</value>
	bool IsCancelled { get; }
	/// <summary>
	/// Cancels the event.
	/// </summary>
	void Cancel();
}

/// <inheritdoc />
public class CancelArgs : ICancelArgs
{
	/// <inheritdoc />
	public bool IsCancelled { get; private set; }

	/// <inheritdoc />
	public void Cancel() => IsCancelled = true;
}

/// <inheritdoc cref="CancelArgs" />
/// <param name="value">The value.</param>
public class ValueCancelArgs<T>(T value) : CancelArgs, IValueArgs<T>
{
	/// <inheritdoc />
	public T Value { get; } = value;
}