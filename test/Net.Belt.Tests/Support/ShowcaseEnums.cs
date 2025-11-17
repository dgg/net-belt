using System.Reflection;

namespace Net.Belt.Tests.Support;

internal enum MyEnum : byte
{
	One = 1,
	[Obfuscation]
	Three = 3
}

[Flags]
internal enum FlagEnum : byte
{
	Zero = 0,
	One = 1,
	Four = 4
}