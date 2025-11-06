namespace Net.Belt.Tests.Patterns.Support;

internal class ComplexType(bool enabled, string foo, int bar)
{
	public string Foo { get; } = foo;
	public int Bar { get; } = bar;
	public bool Enabled { get; } = enabled;
}