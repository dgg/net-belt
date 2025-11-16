using System.Diagnostics.CodeAnalysis;

using Net.Belt.Tests.Support;

namespace Net.Belt.Tests;

[TestFixture]
[SuppressMessage("ReSharper", "RedundantArgumentDefaultValue")]
public class EnumerationTester
{
	#region checking

	[Test]
	public void IsEnum_EnumType_True() =>
		Assert.That(Enumeration.IsEnum<ActionTargets>(), Is.True);

	[Test]
	public void IsEnum_NotEnumType_False() =>
		Assert.That(Enumeration.IsEnum<int>(), Is.False);

	[Test]
	public void AssertEnum_EnumType_NoException() =>
		Assert.That(Enumeration.AssertEnum<ActionTargets>, Throws.Nothing);

	[Test]
	public void AssertEnum_NotEnumType_Exception() =>
		Assert.That(Enumeration.AssertEnum<int>, Throws.InstanceOf<ArgumentException>().With.Message.Contains("Int32"));

	#endregion

	#region definition

	[Test]
	public void IsDefined_DefinedEnumValue_True()
	{
		var defined = StringComparison.Ordinal;
		Assert.That(Enumeration.IsDefined(defined), Is.True);
	}

	[Test]
	public void IsDefined_UndefinedEnumValue_False()
	{
		var undefined = (StringComparison)100;
		Assert.That(Enumeration.IsDefined(undefined), Is.False);
	}

	[Test]
	public void IsDefined_DefinedExplicitNumericValue_True()
	{
		byte bValue = 1;
		Assert.That(Enumeration.IsDefined<ByteEnum, byte>(bValue), Is.True);
		sbyte sbValue = 1;
		Assert.That(Enumeration.IsDefined<SByteEnum, sbyte>(sbValue), Is.True);
		short sValue = 1;
		Assert.That(Enumeration.IsDefined<ShortEnum, short>(sValue), Is.True);
		ushort usValue = 1;
		Assert.That(Enumeration.IsDefined<UShortEnum, ushort>(usValue), Is.True);
		Assert.That(Enumeration.IsDefined<IntEnum, int>(1), Is.True);
		Assert.That(Enumeration.IsDefined<UIntEnum, uint>(1U), Is.True);
		Assert.That(Enumeration.IsDefined<LongEnum, long>(1L), Is.True);
		Assert.That(Enumeration.IsDefined<ULongEnum, ulong>(1UL), Is.True);
	}

	[Test]
	public void IsDefined_DefinedImplicitNumericValue_True()
	{
		byte bValue = 1;
		Assert.That(Enumeration.IsDefined<ByteEnum>(bValue), Is.True);
		sbyte sbValue = 1;
		Assert.That(Enumeration.IsDefined<SByteEnum>(sbValue), Is.True);
		short sValue = 1;
		Assert.That(Enumeration.IsDefined<ShortEnum>(sValue), Is.True);
		ushort usValue = 1;
		Assert.That(Enumeration.IsDefined<UShortEnum>(usValue), Is.True);
		Assert.That(Enumeration.IsDefined<IntEnum>(1), Is.True);
		Assert.That(Enumeration.IsDefined<UIntEnum>(1U), Is.True);
		Assert.That(Enumeration.IsDefined<LongEnum>(1L), Is.True);
		Assert.That(Enumeration.IsDefined<ULongEnum>(1UL), Is.True);
	}

	[Test]
	public void IsDefined_UndefinedExplicitNumericValue_False()
	{
		Assert.That(Enumeration.IsDefined<ByteEnum, byte>(100), Is.False);
		Assert.That(Enumeration.IsDefined<SByteEnum, sbyte>(100), Is.False);
		Assert.That(Enumeration.IsDefined<ShortEnum, short>(100), Is.False);
		Assert.That(Enumeration.IsDefined<UShortEnum, ushort>(100), Is.False);
		Assert.That(Enumeration.IsDefined<IntEnum, int>(100), Is.False);
		Assert.That(Enumeration.IsDefined<UIntEnum, uint>(100u), Is.False);
		Assert.That(Enumeration.IsDefined<LongEnum, long>(100L), Is.False);
		Assert.That(Enumeration.IsDefined<ULongEnum, ulong>(100UL), Is.False);
	}

	[Test]
	public void IsDefined_UndefinedImplicitNumericValue_False()
	{
		byte bValue = 100;
		Assert.That(Enumeration.IsDefined<ByteEnum>(bValue), Is.False);
		sbyte sbValue = 100;
		Assert.That(Enumeration.IsDefined<SByteEnum>(sbValue), Is.False);
		short sValue = 100;
		Assert.That(Enumeration.IsDefined<ShortEnum>(sValue), Is.False);
		ushort usValue = 100;
		Assert.That(Enumeration.IsDefined<UShortEnum>(usValue), Is.False);
		Assert.That(Enumeration.IsDefined<IntEnum>(100), Is.False);
		Assert.That(Enumeration.IsDefined<UIntEnum>(100u), Is.False);
		Assert.That(Enumeration.IsDefined<LongEnum>(100L), Is.False);
		Assert.That(Enumeration.IsDefined<ULongEnum>(100UL), Is.False);
	}

	[Test]
	public void IsDefined_ImplicitUnderlyingValueMismatch_Exception() =>
		Assert.That(() => Enumeration.IsDefined<ByteEnum>(100L), Throws.ArgumentException);

	[Test]
	public void IsDefined_ExplicitUnderlyingValueMismatch_Exception() =>
		Assert.That(() => Enumeration.IsDefined<ByteEnum, long>(100L), Throws.ArgumentException);

	[Test]
	public void IsDefined_DefinedName_True() =>
		Assert.That(Enumeration.IsDefined<StringComparison>(nameof(StringComparison.Ordinal)), Is.True);

	[Test]
	public void IsDefined_UndefinedName_False() =>
		Assert.That(Enumeration.IsDefined<StringComparison>("undefined"), Is.False);

	[Test]
	public void IsDefined_DefinedWrongCasingName_False()
	{
		string wrongCasing = "ordinal";
		Assert.That(Enumeration.IsDefined<StringComparison>(wrongCasing), Is.False);
		Assert.That(Enumeration.IsDefined<StringComparison>(wrongCasing, false), Is.False);
	}

	[Test]
	public void IsDefined_DefinedWrongCasingNameWhenIgnoredCasing_True() =>
		Assert.That(Enumeration.IsDefined<StringComparison>("ordinal", true), Is.True);

	#endregion

	#region AssertDefined

	[Test]
	public void AssertDefined_DefinedEnumValue_NoException() =>
		Assert.That(() => Enumeration.AssertDefined(StringComparison.Ordinal), Throws.Nothing);

	[Test]
	public void AssertDefined_UndefinedEnumValue_Exception()
	{
		var undefined = (StringComparison)100;
		Assert.That(() => Enumeration.AssertDefined(undefined), Throws.ArgumentException
			.With.Message.Contain("100")
			.And.With.Message.Contain("StringComparison"));
	}

	[Test]
	public void AssertDefined_DefinedImplicitNumericValue_NoException()
	{
		byte bValue = 1;
		Assert.That(() => Enumeration.AssertDefined<ByteEnum>(bValue), Throws.Nothing);
		sbyte sbValue = 1;
		Assert.That(() => Enumeration.AssertDefined<SByteEnum>(sbValue), Throws.Nothing);
		short sValue = 1;
		Assert.That(() => Enumeration.AssertDefined<ShortEnum>(sValue), Throws.Nothing);
		ushort usValue = 1;
		Assert.That(() => Enumeration.AssertDefined<UShortEnum>(usValue), Throws.Nothing);
		Assert.That(() => Enumeration.AssertDefined<IntEnum>(1), Throws.Nothing);
		Assert.That(() => Enumeration.AssertDefined<UIntEnum>(1U), Throws.Nothing);
		Assert.That(() => Enumeration.AssertDefined<LongEnum>(1L), Throws.Nothing);
		Assert.That(() => Enumeration.AssertDefined<ULongEnum>(1UL), Throws.Nothing);
	}

	[Test]
	public void AssertDefined_DefinedExplicitNumericValue_NoException()
	{
		Assert.That(() => Enumeration.AssertDefined<ByteEnum, byte>(1), Throws.Nothing);
		Assert.That(() => Enumeration.AssertDefined<SByteEnum, sbyte>(1), Throws.Nothing);
		Assert.That(() => Enumeration.AssertDefined<ShortEnum, short>(1), Throws.Nothing);
		Assert.That(() => Enumeration.AssertDefined<UShortEnum, ushort>(1), Throws.Nothing);
		Assert.That(() => Enumeration.AssertDefined<IntEnum, int>(1), Throws.Nothing);
		Assert.That(() => Enumeration.AssertDefined<UIntEnum, uint>(1U), Throws.Nothing);
		Assert.That(() => Enumeration.AssertDefined<LongEnum, long>(1L), Throws.Nothing);
		Assert.That(() => Enumeration.AssertDefined<ULongEnum, ulong>(1UL), Throws.Nothing);
	}

	[Test]
	public void AssertDefined_UndefinedImplicitNumericValue_Exception()
	{
		byte bValue = 100;
		Assert.That(() => Enumeration.AssertDefined<ByteEnum>(bValue),
			Throws.ArgumentException);
		sbyte sbValue = 100;
		Assert.That(() => Enumeration.AssertDefined<SByteEnum>(sbValue),
			Throws.ArgumentException);
		short sValue = 100;
		Assert.That(() => Enumeration.AssertDefined<ShortEnum>(sValue),
			Throws.ArgumentException);
		ushort usValue = 100;
		Assert.That(() => Enumeration.AssertDefined<UShortEnum>(usValue),
			Throws.ArgumentException);
		Assert.That(() => Enumeration.AssertDefined<IntEnum>(100),
			Throws.ArgumentException);
		Assert.That(() => Enumeration.AssertDefined<UIntEnum>(100U),
			Throws.ArgumentException);
		Assert.That(() => Enumeration.AssertDefined<LongEnum>(100L),
			Throws.ArgumentException);
		Assert.That(() => Enumeration.AssertDefined<ULongEnum>(100UL),
			Throws.ArgumentException);
	}
	
	[Test]
	public void AssertDefined_UndefinedExplicitNumericValue_Exception()
	{
		Assert.That(() => Enumeration.AssertDefined<ByteEnum, byte>(100),
			Throws.ArgumentException);
		Assert.That(() => Enumeration.AssertDefined<SByteEnum, sbyte>(100),
			Throws.ArgumentException);
		Assert.That(() => Enumeration.AssertDefined<ShortEnum, short>(100),
			Throws.ArgumentException);
		Assert.That(() => Enumeration.AssertDefined<UShortEnum, ushort>(100),
			Throws.ArgumentException);
		Assert.That(() => Enumeration.AssertDefined<IntEnum, int>(100),
			Throws.ArgumentException);
		Assert.That(() => Enumeration.AssertDefined<UIntEnum, uint>(100U),
			Throws.ArgumentException);
		Assert.That(() => Enumeration.AssertDefined<LongEnum, long>(100L),
			Throws.ArgumentException);
		Assert.That(() => Enumeration.AssertDefined<ULongEnum, ulong>(100UL),
			Throws.ArgumentException);
	}

	[Test]
	public void AssertDefined_UnderlyingValueMismatch_Exception() =>
		Assert.That(() => Enumeration.AssertDefined<ByteEnum>(100L), Throws.ArgumentException);

	[Test]
	public void AssertDefined_DefinedName_NoException() => Assert.That(
		() => Enumeration.AssertDefined<StringComparison>(nameof(StringComparison.Ordinal)), Throws.Nothing);

	[Test]
	public void AssertDefined_UndefinedName_Exception() =>
		Assert.That(() => Enumeration.AssertDefined<StringComparison>("undefined"),
			Throws.ArgumentException);

	[Test]
	public void AssertDefined_DefinedWrongCasingName_Exception()
	{
		string wrongCasing = "ordinal";
		Assert.That(() => Enumeration.AssertDefined<StringComparison>(wrongCasing),
			Throws.ArgumentException);
		Assert.That(() => Enumeration.AssertDefined<StringComparison>(wrongCasing, false),
			Throws.ArgumentException);
	}

	[Test]
	public void AssertDefined_DefinedWrongCasingNameWhenIgnoredCasing_NoException() =>
		Assert.That(() => Enumeration.AssertDefined<StringComparison>("ordinal", true), Throws.Nothing);

	#endregion
}