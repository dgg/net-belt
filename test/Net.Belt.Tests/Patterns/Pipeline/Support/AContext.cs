namespace Net.Belt.Tests.Patterns.Pipelining.Support;

internal record AContext(string ARequestProp)
{
	public int? AContextProp { get; set; }
}