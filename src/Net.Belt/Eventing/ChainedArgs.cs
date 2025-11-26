namespace Net.Belt.Eventing;

/// <summary>
/// Represents arguments for an event that can be chained, allowing subsequent handlers to know if the event has already been handled.
/// </summary>
public interface IChainedArgs
{
	/// <summary>
	/// Gets or sets a value indicating whether the event has been handled by a previous handler in the chain.
	/// </summary>
	/// <value><c>true</c> if the event has been handled; otherwise, <c>false</c>.</value>
	bool Handled { get; set; }
}

/// <inheritdoc />
public class ChainedArgs : IChainedArgs
{
	/// <inheritdoc />
	public bool Handled { get; set; }
}