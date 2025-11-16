using System.ComponentModel;
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

	#region IsDefined

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

	[Test]
	public void GetNames_EnumType_EnumerationOfStrings() =>
		Assert.That(Enumeration.GetNames<StringComparison>(), Is.EquivalentTo([
			nameof(StringComparison.Ordinal),
			nameof(StringComparison.OrdinalIgnoreCase),
			nameof(StringComparison.CurrentCulture),
			nameof(StringComparison.CurrentCultureIgnoreCase),
			nameof(StringComparison.InvariantCulture),
			nameof(StringComparison.InvariantCultureIgnoreCase)
		]));


	#region GetName

	[Test]
	public void GetName_DefinedEnumValue_Name() => Assert.That(Enumeration.GetName(StringComparison.Ordinal),
		Is.EqualTo(nameof(StringComparison.Ordinal)));

	[Test]
	public void GetName_UndefinedEnumValue_Exception() =>
		Assert.That(() => Enumeration.GetName((StringComparison)100), Throws.ArgumentException);

	[Test]
	public void GetName_DefinedImplicitNumericValue_Name()
	{
		byte bValue = 1;
		Assert.That(Enumeration.GetName<ByteEnum>(bValue), Is.EqualTo("Two"));
		sbyte sbValue = 1;
		Assert.That(Enumeration.GetName<SByteEnum>(sbValue), Is.EqualTo("Two"));
		short sValue = 1;
		Assert.That(Enumeration.GetName<ShortEnum>(sValue), Is.EqualTo("Two"));
		ushort usValue = 1;
		Assert.That(Enumeration.GetName<UShortEnum>(usValue), Is.EqualTo("Two"));
		Assert.That(Enumeration.GetName<IntEnum>(1), Is.EqualTo("Two"));
		Assert.That(Enumeration.GetName<UIntEnum>(1U), Is.EqualTo("Two"));
		Assert.That(Enumeration.GetName<LongEnum>(1L), Is.EqualTo("Two"));
		Assert.That(Enumeration.GetName<ULongEnum>(1UL), Is.EqualTo("Two"));
	}

	[Test]
	public void GetName_DefinedExplicitNumericValue_Name()
	{
		Assert.That(Enumeration.GetName<ByteEnum, byte>(1), Is.EqualTo("Two"));
		Assert.That(Enumeration.GetName<SByteEnum, sbyte>(1), Is.EqualTo("Two"));
		Assert.That(Enumeration.GetName<ShortEnum, short>(1), Is.EqualTo("Two"));
		Assert.That(Enumeration.GetName<UShortEnum, ushort>(1), Is.EqualTo("Two"));
		Assert.That(Enumeration.GetName<IntEnum, int>(1), Is.EqualTo("Two"));
		Assert.That(Enumeration.GetName<UIntEnum, uint>(1U), Is.EqualTo("Two"));
		Assert.That(Enumeration.GetName<LongEnum, long>(1L), Is.EqualTo("Two"));
		Assert.That(Enumeration.GetName<ULongEnum, ulong>(1UL), Is.EqualTo("Two"));
	}

	[Test]
	public void GetName_UndefinedImplicitNumericValue_Exception()
	{
		byte bValue = 100;
		Assert.That(() => Enumeration.GetName<ByteEnum>(bValue), Throws.ArgumentException);
		sbyte sbValue = 100;
		Assert.That(() => Enumeration.GetName<SByteEnum>(sbValue), Throws.ArgumentException);
		short sValue = 100;
		Assert.That(() => Enumeration.GetName<ShortEnum>(sValue), Throws.ArgumentException);
		ushort usValue = 100;
		Assert.That(() => Enumeration.GetName<UShortEnum>(usValue), Throws.ArgumentException);
		Assert.That(() => Enumeration.GetName<IntEnum>(100), Throws.ArgumentException);
		Assert.That(() => Enumeration.GetName<UIntEnum>(100U), Throws.ArgumentException);
		Assert.That(() => Enumeration.GetName<LongEnum>(100L), Throws.ArgumentException);
		Assert.That(() => Enumeration.GetName<ULongEnum>(100UL), Throws.ArgumentException);
	}

	[Test]
	public void GetName_UndefinedExplicitNumericValue_Exception()
	{
		Assert.That(() => Enumeration.GetName<ByteEnum, byte>(100), Throws.ArgumentException);
		Assert.That(() => Enumeration.GetName<SByteEnum, sbyte>(100), Throws.ArgumentException);
		Assert.That(() => Enumeration.GetName<ShortEnum, short>(100), Throws.ArgumentException);
		Assert.That(() => Enumeration.GetName<UShortEnum, ushort>(100), Throws.ArgumentException);
		Assert.That(() => Enumeration.GetName<IntEnum, int>(100), Throws.ArgumentException);
		Assert.That(() => Enumeration.GetName<UIntEnum, uint>(100U), Throws.ArgumentException);
		Assert.That(() => Enumeration.GetName<LongEnum, long>(100L), Throws.ArgumentException);
		Assert.That(() => Enumeration.GetName<ULongEnum, ulong>(100UL), Throws.ArgumentException);
	}

	#endregion

	#region TryGetName

	[Test]
	public void TryGetName_DefinedEnumValue_Name()
	{
		Assert.That(Enumeration.TryGetName(StringComparison.Ordinal, out string? name), Is.True);
		Assert.That(name, Is.Not.Null.And.EqualTo(nameof(StringComparison.Ordinal)));
	}

	[Test]
	public void TryGetName_UndefinedEnumValue_False()
	{
		StringComparison undefined = (StringComparison)100;
		Assert.That(Enumeration.TryGetName(undefined, out string? name), Is.False);
		Assert.That(name, Is.Null);
	}

	[Test]
	public void TryGetName_DefinedImplicitNumericValue_True()
	{
		byte bValue = 1;
		Assert.That(Enumeration.TryGetName<ByteEnum>(bValue, out string? name), Is.True);
		Assert.That(name, Is.EqualTo("Two"));
		sbyte sbValue = 1;
		Assert.That(Enumeration.TryGetName<SByteEnum>(sbValue, out name), Is.True);
		Assert.That(name, Is.EqualTo("Two"));
		short sValue = 1;
		Assert.That(Enumeration.TryGetName<ShortEnum>(sValue, out name), Is.True);
		Assert.That(name, Is.EqualTo("Two"));
		ushort usValue = 1;
		Assert.That(Enumeration.TryGetName<UShortEnum>(usValue, out name), Is.True);
		Assert.That(name, Is.EqualTo("Two"));
		Assert.That(Enumeration.TryGetName<IntEnum>(1, out name), Is.True);
		Assert.That(name, Is.EqualTo("Two"));
		Assert.That(Enumeration.TryGetName<UIntEnum>(1U, out name), Is.True);
		Assert.That(name, Is.EqualTo("Two"));
		Assert.That(Enumeration.TryGetName<LongEnum>(1L, out name), Is.True);
		Assert.That(name, Is.EqualTo("Two"));
		Assert.That(Enumeration.TryGetName<ULongEnum>(1UL, out name), Is.True);
		Assert.That(name, Is.EqualTo("Two"));
	}

	[Test]
	public void TryGetName_DefinedExplicitNumericValue_True()
	{
		Assert.That(Enumeration.TryGetName<ByteEnum, byte>(1, out string? name), Is.True);
		Assert.That(name, Is.EqualTo("Two"));
		Assert.That(Enumeration.TryGetName<SByteEnum, sbyte>(1, out name), Is.True);
		Assert.That(name, Is.EqualTo("Two"));
		Assert.That(Enumeration.TryGetName<ShortEnum, short>(1, out name), Is.True);
		Assert.That(name, Is.EqualTo("Two"));
		Assert.That(Enumeration.TryGetName<UShortEnum, ushort>(1, out name), Is.True);
		Assert.That(name, Is.EqualTo("Two"));
		Assert.That(Enumeration.TryGetName<IntEnum, int>(1, out name), Is.True);
		Assert.That(name, Is.EqualTo("Two"));
		Assert.That(Enumeration.TryGetName<UIntEnum, uint>(1U, out name), Is.True);
		Assert.That(name, Is.EqualTo("Two"));
		Assert.That(Enumeration.TryGetName<LongEnum, long>(1L, out name), Is.True);
		Assert.That(name, Is.EqualTo("Two"));
		Assert.That(Enumeration.TryGetName<ULongEnum, ulong>(1UL, out name), Is.True);
		Assert.That(name, Is.EqualTo("Two"));
	}

	[Test]
	public void TryGetName_UndefinedImplicitNumericValue_False()
	{
		byte bValue = 100;
		Assert.That(() => Enumeration.TryGetName<ByteEnum>(bValue, out _), Is.False);
		sbyte sbValue = 100;
		Assert.That(() => Enumeration.TryGetName<SByteEnum>(sbValue, out _), Is.False);
		short sValue = 100;
		Assert.That(() => Enumeration.TryGetName<ShortEnum>(sValue, out _), Is.False);
		ushort usValue = 100;
		Assert.That(() => Enumeration.TryGetName<UShortEnum>(usValue, out _), Is.False);
		Assert.That(() => Enumeration.TryGetName<IntEnum>(100, out _), Is.False);
		Assert.That(() => Enumeration.TryGetName<UIntEnum>(100U, out _), Is.False);
		Assert.That(() => Enumeration.TryGetName<LongEnum>(100L, out _), Is.False);
		Assert.That(() => Enumeration.TryGetName<ULongEnum>(100UL, out _), Is.False);
	}

	[Test]
	public void TryGetName_UndefinedExplicitNumericValue_False()
	{
		Assert.That(() => Enumeration.TryGetName<ByteEnum, byte>(100, out _), Is.False);
		Assert.That(() => Enumeration.TryGetName<SByteEnum, sbyte>(100, out _), Is.False);
		Assert.That(() => Enumeration.TryGetName<ShortEnum, short>(100, out _), Is.False);
		Assert.That(() => Enumeration.TryGetName<UShortEnum, ushort>(10, out _), Is.False);
		Assert.That(() => Enumeration.TryGetName<IntEnum, int>(100, out _), Is.False);
		Assert.That(() => Enumeration.TryGetName<UIntEnum, uint>(100U, out _), Is.False);
		Assert.That(() => Enumeration.TryGetName<LongEnum, long>(100L, out _), Is.False);
		Assert.That(() => Enumeration.TryGetName<ULongEnum, ulong>(100UL, out _), Is.False);
	}

	#endregion

	[Test]
	public void GetUnderlyingType_CorrectType()
	{
		Assert.That(Enumeration.GetUnderlyingType<ByteEnum>(), Is.EqualTo(typeof(byte)));
		Assert.That(Enumeration.GetUnderlyingType<SByteEnum>(), Is.EqualTo(typeof(sbyte)));
		Assert.That(Enumeration.GetUnderlyingType<ShortEnum>(), Is.EqualTo(typeof(short)));
		Assert.That(Enumeration.GetUnderlyingType<UShortEnum>(), Is.EqualTo(typeof(ushort)));
		Assert.That(Enumeration.GetUnderlyingType<IntEnum>(), Is.EqualTo(typeof(int)));
		Assert.That(Enumeration.GetUnderlyingType<UIntEnum>(), Is.EqualTo(typeof(uint)));
		Assert.That(Enumeration.GetUnderlyingType<LongEnum>(), Is.EqualTo(typeof(long)));
		Assert.That(Enumeration.GetUnderlyingType<ULongEnum>(), Is.EqualTo(typeof(ulong)));
	}

	[Test]
	public void GetValues_GetsAllTypedValues() =>
		Assert.That(Enumeration.GetValues<StringSplitOptions>(), Is.EquivalentTo([
			StringSplitOptions.None, StringSplitOptions.RemoveEmptyEntries, StringSplitOptions.TrimEntries
		]));

	#region GetValue

	[Test]
	public void GetValue_DefinedUnderlying_NumericValue()
	{
		Assert.That(Enumeration.GetValue<ByteEnum, byte>(ByteEnum.Two), Is.InstanceOf<byte>().And.EqualTo(1));
		Assert.That(Enumeration.GetValue<SByteEnum, sbyte>(SByteEnum.Two), Is.InstanceOf<sbyte>().And.EqualTo(1));
		Assert.That(Enumeration.GetValue<ShortEnum, short>(ShortEnum.Two), Is.InstanceOf<short>().And.EqualTo(1));
		Assert.That(Enumeration.GetValue<UShortEnum, ushort>(UShortEnum.Two), Is.InstanceOf<ushort>().And.EqualTo(1));
		Assert.That(Enumeration.GetValue<IntEnum, int>(IntEnum.Two), Is.InstanceOf<int>().And.EqualTo(1));
		Assert.That(Enumeration.GetValue<UIntEnum, uint>(UIntEnum.Two), Is.InstanceOf<uint>().And.EqualTo(1));
		Assert.That(Enumeration.GetValue<LongEnum, long>(LongEnum.Two), Is.InstanceOf<long>().And.EqualTo(1));
		Assert.That(Enumeration.GetValue<ULongEnum, ulong>(ULongEnum.Two), Is.InstanceOf<ulong>().And.EqualTo(1));
	}

	[Test]
	public void GetValue_NotOverflowingNotUnderlying_NumericValue()
	{
		decimal notOverflowing = Enumeration.GetValue<LongEnum, decimal>(LongEnum.Two);
		Assert.That(notOverflowing, Is.InstanceOf<decimal>().And.EqualTo(1m));
	}

	[Test]
	public void GetValue_OverflowingNotUnderlying_Exception() =>
		Assert.That(() => Enumeration.GetValue<MaxEnum, byte>(MaxEnum.Max), Throws.InstanceOf<OverflowException>());

	[Test]
	public void GetValue_Undefined_Exception() =>
		Assert.That(() => Enumeration.GetValue<StringComparison, int>((StringComparison)100), Throws.ArgumentException);

	#endregion

	#region TryGetValue

	[Test]
	public void TryGetValue_DefinedUnderlying_True()
	{
		Assert.That(Enumeration.TryGetValue(ByteEnum.Two, out byte? bValue), Is.True);
		Assert.That(bValue, Is.InstanceOf<byte>().And.EqualTo(1));
		Assert.That(Enumeration.TryGetValue(SByteEnum.Two, out sbyte? sbValue), Is.True);
		Assert.That(sbValue, Is.InstanceOf<sbyte>().And.EqualTo(1));
		Assert.That(Enumeration.TryGetValue(ShortEnum.Two, out short? sValue), Is.True);
		Assert.That(sValue, Is.InstanceOf<short>().And.EqualTo(1));
		Assert.That(Enumeration.TryGetValue(UShortEnum.Two, out ushort? usValue), Is.True);
		Assert.That(usValue, Is.InstanceOf<ushort>().And.EqualTo(1));
		Assert.That(Enumeration.TryGetValue(IntEnum.Two, out int? iValue), Is.True);
		Assert.That(iValue, Is.InstanceOf<int>().And.EqualTo(1));
		Assert.That(Enumeration.TryGetValue(UIntEnum.Two, out uint? uiValue), Is.True);
		Assert.That(uiValue, Is.InstanceOf<uint>().And.EqualTo(1));
		Assert.That(Enumeration.TryGetValue(LongEnum.Two, out long? lValue), Is.True);
		Assert.That(lValue, Is.InstanceOf<long>().And.EqualTo(1));
		Assert.That(Enumeration.TryGetValue(ULongEnum.Two, out ulong? ulValue), Is.True);
		Assert.That(ulValue, Is.InstanceOf<ulong>().And.EqualTo(1));
	}

	[Test]
	public void TryGetValue_NotOverflowingNotUnderlying_True()
	{
		Assert.That(Enumeration.TryGetValue(LongEnum.Two, out decimal? notUnderlying), Is.True);
		Assert.That(notUnderlying, Is.EqualTo(1m));
	}

	[Test]
	public void TryGetValue_OverflowingNotUnderlying_False() =>
		Assert.That(Enumeration.TryGetValue(MaxEnum.Max, out byte? _), Is.False);

	[Test]
	public void TryGetValue_Undefined_False()
	{
		Assert.That(Enumeration.TryGetValue((StringComparison)100, out int? value), Is.False);
		Assert.That(value, Is.Null);
	}

	#endregion

	#region Cast

	[Test]
	public void Cast_DefinedUnderlyingValue_EnumValue()
	{
		Assert.That(Enumeration.Cast<ByteEnum>((byte)1), Is.EqualTo(ByteEnum.Two));
		Assert.That(Enumeration.Cast<SByteEnum>((sbyte)1), Is.EqualTo(SByteEnum.Two));
		Assert.That(Enumeration.Cast<ShortEnum>((short)1), Is.EqualTo(ShortEnum.Two));
		Assert.That(Enumeration.Cast<UShortEnum>((ushort)1), Is.EqualTo(UShortEnum.Two));
		Assert.That(Enumeration.Cast<IntEnum>(1), Is.EqualTo(IntEnum.Two));
		Assert.That(Enumeration.Cast<UIntEnum>(1u), Is.EqualTo(UIntEnum.Two));
		Assert.That(Enumeration.Cast<LongEnum>(1L), Is.EqualTo(LongEnum.Two));
		Assert.That(Enumeration.Cast<ULongEnum>(1UL), Is.EqualTo(ULongEnum.Two));
	}

	[Test]
	public void Cast_NotUnderLying_Exception()
	{
		Assert.That(() => Enumeration.Cast<ByteEnum>(1L), Throws.ArgumentException);
		Assert.That(() => Enumeration.Cast<ByteEnum>(100L), Throws.ArgumentException);
	}

	[Test]
	public void Cast_UndefinedValue_Exception()
	{
		Assert.That(() => Enumeration.Cast<StringComparison>(100), Throws.ArgumentException);
	}

	[Test]
	public void Cast_Vs_ToObject()
	{
		// everything flows when values defined
		Assert.That(Enumeration.Cast<StringComparison>(4), Is.EqualTo(StringComparison.Ordinal));
		Assert.That(Enum.ToObject(typeof(StringComparison), 4), Is.EqualTo(StringComparison.Ordinal));

		// but .ToObject() "forwards" undefined values...
		Assert.That(() => Enumeration.Cast<StringComparison>(100), Throws.ArgumentException);
		Assert.That(Enum.ToObject(typeof(StringComparison), 100), Is.EqualTo((StringComparison)100));

		// ... and does not care about the underlying type
		Assert.That(() => Enumeration.Cast<StringComparison>(4L), Throws.ArgumentException);
		Assert.That(Enum.ToObject(typeof(StringComparison), 4L), Is.EqualTo(StringComparison.Ordinal));
	}

	#endregion

	#region TryCast

	[Test]
	public void TryCast_DefinedUnderlyingValue_True()
	{
		Assert.That(Enumeration.TryCast((byte)1, out ByteEnum? bValue), Is.True);
		Assert.That(bValue, Is.EqualTo(ByteEnum.Two));
		Assert.That(Enumeration.TryCast((sbyte)1, out SByteEnum? sbValue), Is.True);
		Assert.That(sbValue, Is.EqualTo(SByteEnum.Two));
		Assert.That(Enumeration.TryCast((short)1, out ShortEnum? sValue), Is.True);
		Assert.That(sValue, Is.EqualTo(ShortEnum.Two));
		Assert.That(Enumeration.TryCast((ushort)1, out UShortEnum? usValue), Is.True);
		Assert.That(usValue, Is.EqualTo(UShortEnum.Two));
		Assert.That(Enumeration.TryCast(1, out IntEnum? iValue), Is.True);
		Assert.That(iValue, Is.EqualTo(IntEnum.Two));
		Assert.That(Enumeration.TryCast(1u, out UIntEnum? uiValue), Is.True);
		Assert.That(uiValue, Is.EqualTo(UIntEnum.Two));
		Assert.That(Enumeration.TryCast(1L, out LongEnum? lValue), Is.True);
		Assert.That(lValue, Is.EqualTo(LongEnum.Two));
		Assert.That(Enumeration.TryCast(1UL, out ULongEnum? ulValue), Is.True);
		Assert.That(ulValue, Is.EqualTo(ULongEnum.Two));
	}

	[Test]
	public void TryCast_NotUnderLying_Exception()
	{
		Assert.That(() => Enumeration.TryCast(1L, out ByteEnum? _), Throws.ArgumentException);
		Assert.That(() => Enumeration.TryCast(100L, out ByteEnum? _), Throws.ArgumentException);
	}

	[Test]
	public void TryCast_UndefinedValue_False() =>
		Assert.That(Enumeration.TryCast(100, out StringComparison? _), Is.False);

	#endregion

	#region Parse

	[Test]
	public void Parse_DefinedValue_Value()
	{
		Assert.That(Enumeration.Parse<ActionTargets>("Test"), Is.EqualTo(ActionTargets.Test));
		Assert.That(Enumeration.Parse<ActionTargets>("TEst", true), Is.EqualTo(ActionTargets.Test));
	}

	[Test]
	public void Parse_UndefinedValue_Exception()
	{
		Assert.That(() => Enumeration.Parse<ActionTargets>("nonExisting"), Throws.ArgumentException);
		Assert.That(() => Enumeration.Parse<ActionTargets>("TEsT"), Throws.ArgumentException);
		Assert.That(() => Enumeration.Parse<ActionTargets>("SuiTE", false), Throws.ArgumentException);
	}

	[Test]
	public void Parse_DefinedNumericValue_Value() =>
		Assert.That(Enumeration.Parse<StringComparison>("4"), Is.EqualTo(StringComparison.Ordinal));

	[Test]
	public void Parse_Empty_Exception() =>
		Assert.That(() => Enumeration.Parse<ActionTargets>(string.Empty),
			Throws.ArgumentException);

	[Test]
	public void Parse_UndefinedNumericValue_Exception()
	{
		Assert.That(() => Enumeration.Parse<ActionTargets>("100"), Throws.ArgumentException);
	}

	#endregion

	#region TryParse

	[Test]
	public void TryParse_DefinedValue_True()
	{
		Assert.That(Enumeration.TryParse("Suite", out ActionTargets? parsed), Is.True);
		Assert.That(parsed, Is.EqualTo(ActionTargets.Suite));

		Assert.That(Enumeration.TryParse("SUiTe", true, out parsed), Is.True);
		Assert.That(parsed, Is.EqualTo(ActionTargets.Suite));
	}

	[Test]
	public void TryParse_UndefinedValue_False()
	{
		Assert.That(() => Enumeration.TryParse("nonExisting", out ActionTargets? _), Is.False);
		Assert.That(() => Enumeration.TryParse("SUiTe", out ActionTargets? _), Is.False);
		Assert.That(() => Enumeration.TryParse("SUiTe", false, out ActionTargets? _), Is.False);
	}

	[Test]
	public void TryParse_DefinedNumericValue_True()
	{
		Assert.That(Enumeration.TryParse("4", out StringComparison? parsed), Is.True);
		Assert.That(parsed, Is.EqualTo(StringComparison.Ordinal));
	}

	[Test]
	public void TryParse_UndefinedNumericValue_False()
	{
		Assert.That(Enumeration.TryParse("100", out ActionTargets? parsed), Is.False);
		Assert.That(parsed, Is.Null);
	}

	[Test]
	public void TryParse_Empty_False()
	{
		Assert.That(Enumeration.TryParse(string.Empty, out ActionTargets? _), Is.False);
	}

	#endregion
	
	#region Omit

	[Test]
	public void Invert_Empty_OriginalValues()
	{
		var values = Enumeration.GetValues<StringSplitOptions>();
		Assert.That(Enumeration.Omit<StringSplitOptions>(), Is.EquivalentTo(values));
		Assert.That(Enumeration.Omit(Enumerable.Empty<StringSplitOptions>()), Is.EquivalentTo(values));
	}

	[Test]
	public void Invert_AllValues_Empty() => Assert.That(Enumeration.Omit(Enumeration.GetValues<StringSplitOptions>()), Is.Empty);

	[Test]
	public void Invert_SomeValues_RemainingValues()
	{
		StringSplitOptions[] omitted = [StringSplitOptions.RemoveEmptyEntries, StringSplitOptions.TrimEntries];
		Assert.That(Enumeration.Omit(StringSplitOptions.None), Is.EquivalentTo(omitted));
		
		IEnumerable<StringSplitOptions> toRemove = new List<StringSplitOptions> { StringSplitOptions.None };
		Assert.That(Enumeration.Omit(toRemove), Is.EquivalentTo(omitted));
	}

	#endregion
}