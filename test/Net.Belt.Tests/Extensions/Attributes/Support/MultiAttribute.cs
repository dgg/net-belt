namespace Net.Belt.Tests.Extensions.Attributes.Support;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
internal sealed class MultiAttribute(string positional) : Attribute
{
	public string Positional { get; } = positional;
}