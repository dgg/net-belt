using System.Numerics;

using Net.Belt.Extensions.Object;

namespace Net.Belt.Tests.Extensions.Object;

[TestFixture]
public class ObjectExtensionsTester
{
	[Test]
	[TestCase(true, true), TestCase(false, false)]
	public void ConvertToBool_Bool_Spec(bool b, bool expected) =>
		Assert.That(b.ConvertToBool(), Is.EqualTo(expected));

	[Test]
	[TestCase('1'), TestCase('t'), TestCase('T'), TestCase((char)1)]
	public void ConvertToBool_Char_True(char c) =>
		Assert.That(c.ConvertToBool(), Is.True);
	
	[Test]
	[TestCase('0'), TestCase('f'), TestCase('F'), TestCase(char.MinValue)]
	public void ConvertToBool_Char_False(char c) =>
		Assert.That(c.ConvertToBool(), Is.False);
	
	[Test]
	[TestCase('a'), TestCase('2'), TestCase(' '), TestCase(char.MaxValue)]
	public void ConvertToBool_Char_Null(char c) =>
		Assert.That(c.ConvertToBool(), Is.Null);
	
	[Test]
	[TestCase("1"), TestCase("t"), TestCase("T"), TestCase("true"), TestCase("TRUE"), TestCase("True")]
	public void ConvertToBool_String_True(string s) =>
		Assert.That(s.ConvertToBool(), Is.True);
	
	[Test]
	[TestCase("0"), TestCase("f"), TestCase("F"), TestCase("false"), TestCase("FALSE"), TestCase("False")]
	public void ConvertToBool_String_False(string s) =>
		Assert.That(s.ConvertToBool(), Is.False);
	
	[Test]
	[TestCase("a"), TestCase("2"), TestCase(" "), TestCase("")]
	public void ConvertToBool_String_Null(string s) =>
		Assert.That(s.ConvertToBool(), Is.Null);
	
	private static readonly TestCaseData[] _zeros =
	[
		// integrals
		new TestCaseData<byte>(0), new TestCaseData<sbyte>(0),
		new TestCaseData<short>(0), new TestCaseData<ushort>(0),
		new TestCaseData<int>(0), new TestCaseData<uint>(0),
		new TestCaseData<long>(0), new TestCaseData<ulong>(0),
		new TestCaseData<IntPtr>(0), new TestCaseData<UIntPtr>(0),
		new TestCaseData<Int128>(Int128.Zero), new TestCaseData<UInt128>(UInt128.Zero),
		new TestCaseData<BigInteger>(BigInteger.Zero),
		// reals
		new TestCaseData<float>(.0F), new TestCaseData<double>(.0d),
		new TestCaseData<decimal>(decimal.Zero), new TestCaseData<Half>(Half.Zero)
	];
	[Test, TestCaseSource(nameof(_zeros))]
	public void ConvertToBool_NumericZero_Zero<T>(T numericZero) where T : INumber<T> =>
		Assert.That(ObjectExtensions.ConvertToBool(numericZero), Is.False);
	[Test, TestCaseSource(nameof(_zeros))]
	public void ConvertToBool_BoxedZero_False(object boxedOnes) =>
		Assert.That(boxedOnes.ConvertToBool(), Is.False);
	
	private static readonly TestCaseData[] _ones =
	[
		// integrals
		new TestCaseData<byte>(1), new TestCaseData<sbyte>(1),
		new TestCaseData<short>(1), new TestCaseData<ushort>(1),
		new TestCaseData<int>(1), new TestCaseData<uint>(1),
		new TestCaseData<long>(1L), new TestCaseData<ulong>(1UL),
		new TestCaseData<IntPtr>(1), new TestCaseData<UIntPtr>(1),
		new TestCaseData<Int128>(Int128.One), new TestCaseData<UInt128>(UInt128.One),
		new TestCaseData<BigInteger>(BigInteger.One),
		// reals
		new TestCaseData<float>(1.0F), new TestCaseData<double>(1.0d),
		new TestCaseData<decimal>(decimal.One), new TestCaseData<Half>(Half.One)
	];
	[Test, TestCaseSource(nameof(_ones))]
	public void ConvertToBool_NumericOne_True<T>(T numericOne) where T : INumber<T> =>
		Assert.That(ObjectExtensions.ConvertToBool(numericOne), Is.True);
	
	[Test, TestCaseSource(nameof(_ones))]
	public void ConvertToBool_BoxedOne_False(object boxedOne) =>
		Assert.That(boxedOne.ConvertToBool(), Is.True);
	
	private static readonly TestCaseData[] _notBool =
	[
		// integrals
		new TestCaseData<byte>(byte.MaxValue), new TestCaseData<sbyte>(-1),
		new TestCaseData<short>(-2), new TestCaseData<ushort>(2),
		new TestCaseData<int>(-1), new TestCaseData<uint>(2),
		new TestCaseData<long>(-1), new TestCaseData<ulong>(ulong.MaxValue),
		new TestCaseData<IntPtr>(-1), new TestCaseData<UIntPtr>(2),
		new TestCaseData<Int128>(Int128.NegativeOne), new TestCaseData<UInt128>(33),
		new TestCaseData<BigInteger>(42),
		// reals
		new TestCaseData<float>(float.Pi), new TestCaseData<double>(double.Tau),
		new TestCaseData<decimal>(decimal.MaxValue), new TestCaseData<Half>(Half.E),
		// non-numerics
		new TestCaseData<Exception>(new Exception()),
		new TestCaseData<DateTime>(DateTime.MinValue)
	];
	[Test, TestCaseSource(nameof(_notBool))]
	public void ConvertToBool_NotBool_False(object boxed) =>
		Assert.That(boxed.ConvertToBool(), Is.Null);
}