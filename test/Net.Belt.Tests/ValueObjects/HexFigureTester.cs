using System.Diagnostics.CodeAnalysis;

using Net.Belt.ValueObjects;

using NUnit.Framework.Constraints;

using Iz = Net.Belt.Tests.ValueObjects.Support.Iz;

namespace Net.Belt.Tests.ValueObjects;

[TestFixture]
public class HexFigureTester
{
	#region construction

	[TestCase(0, '0')]
	[TestCase(1, '1')]
	[TestCase(9, '9')]
	[TestCase(10, 'A')]
	[TestCase(15, 'F')]
	public void Ctor_ValidNumerics_SetsProps(byte numeric, char expectedCharacter)
	{
		// Act
		var hexFigure = new HexFigure(numeric);

		// Assert
		Assert.That(hexFigure, Iz.Hex(numeric, expectedCharacter));
	}

	[Test]
	public void Ctor_BigNumeric_Exception()
	{
		// Arrange
		byte biggie = 16;

		// Act & Assert
		Assert.That(() => new HexFigure(biggie), Throws.InstanceOf<ArgumentOutOfRangeException>()
			.With.Message.Contains("[0..15]"));
	}

	[TestCase('0', 0, '0')]
	[TestCase('9', 9, '9')]
	[TestCase('A', 10, 'A', Description = "accepts uppercase")]
	[TestCase('F', 15, 'F', Description = "accepts uppercase")]
	[TestCase('a', 10, 'A', Description = "accepts lowercase")]
	[TestCase('f', 15, 'F', Description = "accepts lowercase")]
	public void Ctor_ValidCharacters_SetsProps(char character, byte expectedNumeric, char expectedCharacter)
	{
		// Act
		var hexFigure = new HexFigure(character);

		// Assert
		Assert.That(hexFigure, Iz.Hex(expectedNumeric, expectedCharacter));
	}

	[Test]
	public void Ctor_WithInvalidCharacter_ThrowsArgumentOutOfRangeException()
	{
		// Arrange
		char notHex = 'G';

		// Act & Assert
		Assert.That(() => new HexFigure(notHex), Throws.InstanceOf<ArgumentOutOfRangeException>()
			.With.Message.Contains("['0'..'9']").And
			.Message.Contains("['A'..'F']").And
			.Message.Contains("['a'..'f']").And
			.Message.Contains("∪"));
	}

	[Test]
	public void DefaultCtor_BuildsZeroDefault()
	{
		var zero = new HexFigure();
		Assert.That(zero, Iz.Hex(0, '0'));

		HexFigure @default = default;
		Assert.That(@default, Iz.Hex(0, '0'));

		Assert.That(zero, Is.EqualTo(HexFigure.Zero));
		Assert.That(@default, Is.EqualTo(HexFigure.Zero));
	}

	#endregion

	#region shortcuts

	[Test]
	public void MinMax_Spec()
	{
		Assert.That(HexFigure.Min, Iz.Hex(0, '0'));
		Assert.That(HexFigure.Max, Iz.Hex(15, 'F'));
		Assert.That(HexFigure.Min, Is.EqualTo(HexFigure.Zero));
	}

	#endregion

	#region representation

	[Test]
	public void ToString_ReturnsUppercaseCharacterRepresentation()
	{
		Assert.That(new HexFigure(1).ToString(), Is.EqualTo("1"));
		Assert.That(new HexFigure(11).ToString(), Is.EqualTo("B"));
	}

	#endregion

	#region tuple-like features

	[Test]
	public void Deconstruct_Props()
	{
		(byte numeric, char character) = new HexFigure(12);

		Assert.That(numeric, Is.EqualTo(12));
		Assert.That(character, Is.EqualTo('C'));
	}

	[Test]
	public void Value_NamedTuple()
	{
		var namedTuple = new HexFigure(5).Value;

		Assert.That(namedTuple.numeric, Is.EqualTo(5));
		Assert.That(namedTuple.character, Is.EqualTo('5'));
	}

	[Test]
	public void Value_CanBeDeconstructedAsWell()
	{
		var (_, c) = new HexFigure(14).Value;
		Assert.That(c, Is.EqualTo('E'));
	}
	#endregion

	#region equality

	[Test]
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Assertion", "NUnit2009")]
	public void Equality_SameValue_Equals()
	{
		HexFigure ten = new(10), anotherTen = new('A');

		Assert.That(ten.Equals(ten), Is.True);
		Assert.That(ten.GetHashCode(), Is.EqualTo(ten.GetHashCode()));
		Assert.That(ten.Equals(anotherTen), Is.True);
		Assert.That(ten.GetHashCode(), Is.EqualTo(anotherTen.GetHashCode()));
		Assert.That(ten == anotherTen, Is.True);
		Assert.That(ten != anotherTen, Is.False);
	}

	[Test]
	public void Equality_DifferentValue_NotEquals()
	{
		HexFigure one = new(1), two = new('2');

		Assert.That(one.Equals(two), Is.False);
		Assert.That(one.GetHashCode(), Is.Not.EqualTo(two.GetHashCode()));
		Assert.That(one == two, Is.False);
		Assert.That(one != two, Is.True);
	}

	[Test]
	public void Equals_Null_False() =>
		Assert.That(new HexFigure(0).Equals(null), Is.False);

	[Test]
	[SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
	public void Equals_NotAnHexFigure_False()
	{
		HexFigure ten = new(10);
		object str = "A";
		byte number = 10;

		Assert.That(ten.Equals(str), Is.False);
		Assert.That(ten.Equals(number), Is.False);
	}

	#endregion

	#region comparison

	private static readonly HexFigure _one = new(1);
	private static readonly HexFigure _ten = new(10);
	private static readonly HexFigure _a = new('a');
	private static readonly TestCaseData<HexFigure, HexFigure, Constraint>[] _compareToHexFigure =
	[
		testCase(_ten, _ten, Is.Zero),
		testCase(_ten, _a, Is.Zero),
		testCase(_one, _ten, Is.LessThan(0)),
		testCase(_ten, _one, Is.GreaterThan(0))
	];

	private static TestCaseData<HexFigure, HexFigure, Constraint> testCase(HexFigure left, HexFigure right, Constraint comparisonConstraint) =>
		new(left, right, comparisonConstraint);

	private static TestCaseData<HexFigure, object?, Constraint> testCase(HexFigure left, object? right, Constraint comparisonConstraint) =>
		new(left, right, comparisonConstraint);

	[Test, TestCaseSource(nameof(_compareToHexFigure))]
	public void CompareTo_HexFigure_Spec(HexFigure left, HexFigure right, Constraint comparisonConstraint) =>
		Assert.That(left.CompareTo(right), comparisonConstraint);

	[Test, TestCaseSource(nameof(_compareToHexFigure))]
	public void CompareTo_BoxedHexFigure_Spec(HexFigure left, object? right, Constraint comparisonConstraint) =>
		Assert.That(left.CompareTo(right), comparisonConstraint);

	[Test]
	public void CompareTo_BoxedNull_GreaterThanZero() =>
		Assert.That(() => HexFigure.Zero.CompareTo(null), Is.GreaterThan(0));

	[Test]
	public void CompareTo_BoxedUnsupported_Exception() =>
		Assert.That(() => HexFigure.Zero.CompareTo("0"), Throws.ArgumentException
			.With.Message.Contains(nameof(HexFigure)));

	private static readonly TestCaseData<HexFigure, HexFigure, Constraint>[] _greaterThan =
	[
		testCase(_ten, _ten, Is.False),
		testCase(_ten, _a, Is.False),
		testCase(_one, _ten, Is.False),
		testCase(_ten, _one, Is.True)
	];

	[Test, TestCaseSource(nameof(_greaterThan))]
	public void GreaterThan_HexFigure_ComparesNumerically(HexFigure left, HexFigure right, Constraint comparisonConstraint) =>
		Assert.That(left > right, comparisonConstraint);

	private static readonly TestCaseData<HexFigure, HexFigure, Constraint>[] _greaterThanOrEqual =
	[
		testCase(_ten, _ten, Is.True),
		testCase(_ten, _a, Is.True),
		testCase(_one, _ten, Is.False),
		testCase(_ten, _one, Is.True)
	];

	[Test, TestCaseSource(nameof(_greaterThanOrEqual))]
	public void GreaterThanOrEqual_HexFigure_ComparesNumerically(HexFigure left, HexFigure right, Constraint comparisonConstraint) =>
		Assert.That(left >= right, comparisonConstraint);

	private static readonly TestCaseData<HexFigure, HexFigure, Constraint>[] _lessThan =
	[
		testCase(_ten, _ten, Is.False),
		testCase(_ten, _a, Is.False),
		testCase(_one, _ten, Is.True),
		testCase(_ten, _one, Is.False)
	];

	[Test, TestCaseSource(nameof(_lessThan))]
	public void LessThan_HexFigure_ComparesNumerically(HexFigure left, HexFigure right, Constraint comparisonConstraint) =>
		Assert.That(left < right, comparisonConstraint);

	private static readonly TestCaseData<HexFigure, HexFigure, Constraint>[] _lessThanOrEqual =
	[
		testCase(_ten, _ten, Is.True),
		testCase(_ten, _a, Is.True),
		testCase(_one, _ten, Is.True),
		testCase(_ten, _one, Is.False)
	];

	[Test, TestCaseSource(nameof(_lessThanOrEqual))]
	public void LessThanOrEqual_HexFigure_ComparesNumerically(HexFigure left, HexFigure right, Constraint comparisonConstraint) =>
		Assert.That(left <= right, comparisonConstraint);

	#endregion

	#region parsing

	private static readonly TestCaseData<string, byte>[] _validInputs =
	[
		testCase("0", 0),
		testCase("9", 9),
		testCase("09", 9),
		testCase("10", 10),
		testCase("011", 11),
		testCase("A", 10),
		testCase("F", 15),
		testCase("a", 10),
		testCase("f", 15),
		
	];

	private static readonly TestCaseData[] _invalidInputs =
	[
		new TestCaseData("G").SetDescription("not hex"),
		new TestCaseData("hola").SetDescription("very not hex"),
		new TestCaseData("").SetDescription("empty"),
		new TestCaseData("16").SetDescription("numeric value out of range"),
		new TestCaseData("FF").SetDescription("multi-character hex string out of range")
	];
	private static TestCaseData<string, byte> testCase(string input, byte expectedNumeric) =>
		new(input, expectedNumeric);

	[TestCaseSource(nameof(_validInputs))]
	public void Parse_ValidString_ReturnsHexFigure(string input, byte expectedNumeric) =>
		Assert.That(HexFigure.Parse(input, null), Is.EqualTo(new HexFigure(expectedNumeric)));

	[Test]
	public void Parse_Null_Exception() =>
		Assert.That(() => HexFigure.Parse(null!, null), Throws.ArgumentNullException);

	[TestCaseSource(nameof(_invalidInputs))]
	public void Parse_InvalidString_Exception(string s) =>
		Assert.That(() => HexFigure.Parse(s, null), Throws.InstanceOf<FormatException>());

	[TestCaseSource(nameof(_validInputs))]
	public void Parse_ValidSpan_ReturnsHexFigure(string s, byte expectedNumeric) =>
		Assert.That(HexFigure.Parse(s.AsSpan(), null), Is.EqualTo(new HexFigure(expectedNumeric)));

	[TestCaseSource(nameof(_invalidInputs))]
	public void Parse_Span_InvalidInput_ThrowsException(string s) =>
		Assert.That(() => HexFigure.Parse(s.AsSpan(), null), Throws.InstanceOf<FormatException>());

	[TestCaseSource(nameof(_validInputs))]
	public void TryParse_ValidString_TrueOutHexFigure(string input, byte expectedNumeric)
	{
		Assert.That(HexFigure.TryParse(input, null, out var hex), Is.True);
		Assert.That(hex, Is.EqualTo(new HexFigure(expectedNumeric)));
	}

	[TestCaseSource(nameof(_invalidInputs))]
	public void TryParse_InvalidString_FalseDefault(string input)
	{
		Assert.That(HexFigure.TryParse(input, null, out var hex), Is.False);
		Assert.That(hex, Is.EqualTo(HexFigure.Zero));
	}

	[TestCaseSource(nameof(_validInputs))]
	public void TryParse_ValidSpan_TrueOutHexFigure(string input, byte expectedNumeric)
	{
		Assert.That(HexFigure.TryParse(input.AsSpan(), null, out var hex), Is.True);
		Assert.That(hex, Is.EqualTo(new HexFigure(expectedNumeric)));
	}

	[TestCaseSource(nameof(_invalidInputs))]
	public void TryParse_InvalidSpan_FalseDefault(string input)
	{
		Assert.That(HexFigure.TryParse(input.AsSpan(), null, out var hex), Is.False);
		Assert.That(hex, Is.EqualTo(HexFigure.Zero));
	}

	#endregion

}