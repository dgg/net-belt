using System.Globalization;

using Net.Belt.Patterns.ChainOfResponsibility;

namespace Net.Belt.Tests.Patterns.ChainOfResponsibility.Support;

internal class MultiLink(TimeProvider time) : IChainOfResponsibilityLink<int, string>, IChainOfResponsibilityLink<string, int>, IChainOfResponsibilityLink<Exception>
{
	private readonly int _intToHandle;
	public MultiLink(int contextToHandle, TimeProvider time): this(time)
	{
		_intToHandle = contextToHandle;
	}

	public bool CanHandle(int context) => context.Equals(_intToHandle);

	public string DoHandle(int context) => context.ToString(CultureInfo.InvariantCulture);

	private int _handled;
	public bool CanHandle(string context) => int.TryParse(context, NumberStyles.Integer, CultureInfo.InvariantCulture, out _handled);

	public int DoHandle(string context) => _handled;

	public bool CanHandle(Exception context) => true;

	public void DoHandle(Exception context) => context.Data.Add("handled", time.GetUtcNow());
}