using Net.Belt.ValueObjects;

using NUnit.Framework.Constraints;

namespace Net.Belt.Tests.ValueObjects.Support;

internal partial class Iz : Is
{
	public static Constraint Hex(byte expectedNumeric, char expectedCharacter) => Has
		.Property(nameof(HexFigure.Numeric)).EqualTo(expectedNumeric).And
		.Property(nameof(HexFigure.Character)).EqualTo(expectedCharacter);
}
