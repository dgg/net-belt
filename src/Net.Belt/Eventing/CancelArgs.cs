namespace Net.Belt.Eventing;

/// <summary>
/// 
/// </summary>
public interface ICancelArgs
{
	/// <summary>
	/// 
	/// </summary>
	bool IsCancelled { get; }
	/// <summary>
	/// 
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

/// <summary>
/// 
/// </summary>
/// <param name="value"></param>
/// <typeparam name="T"></typeparam>
public class ValueCancelArgs<T>(T value) : CancelArgs, IValueArgs<T>
{
	/// <inheritdoc />
	public T Value { get; } = value;
}