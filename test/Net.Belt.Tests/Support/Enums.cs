using Desc = System.ComponentModel.DescriptionAttribute;

namespace Net.Belt.Tests.Support;

internal enum MyTargets
{
	Default,
	Suite
}

internal enum ByteEnum : byte { One, Two }
internal enum SByteEnum : sbyte { One, Two }
internal enum ShortEnum : short { One, Two }
internal enum UShortEnum : ushort { One, Two }
internal enum IntEnum : int { One, Two }
internal enum UIntEnum : uint { One, Two }
internal enum LongEnum : long { One, Two }
internal enum ULongEnum : ulong { One, Two }

internal enum MaxEnum : ulong { Max = ulong.MaxValue }

internal enum Attributed
{
	Zero = 0,
	[Desc("Sub-Zero")]
	SubZero = -1,
}

[Flags]
internal enum NoZeroFlags : byte
{
	One = 1,
	Two = 2,
	Three = 4,
	Four = 8
}

[Flags]
internal enum ZeroFlags : byte
{
	Zero = 0,
	One = 1,
	Two = 2,
	Three = 4,
	Four = 8
}