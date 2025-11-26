namespace Net.Belt.Eventing;

/// <summary>
/// 
/// </summary>
public interface IChainedArgs
{
	/// <summary>
	/// 
	/// </summary>
	bool Handled { get; set; }
}

/// <inheritdoc />
public class ChainedArgs : IChainedArgs
{
	/// <inheritdoc />
	public bool Handled { get; set; }
}