using System.Numerics;

using NUnit.Framework.Constraints;

using Net.Belt.Extensions.Numerics;

namespace Net.Belt.Tests.Extensions.Numerics;

[TestFixture]
public class NumericsExtensionsTester
{
	private static readonly TestCaseData[] _integral =
	[
		new TestCaseData<byte>(1), new TestCaseData<sbyte>(-1),
		new TestCaseData<short>(-1), new TestCaseData<ushort>(1),
		new TestCaseData<int>(-1), new TestCaseData<uint>(1U),
		new TestCaseData<long>(-1L), new TestCaseData<ulong>(1UL),
		new TestCaseData<char>('1'),
		new TestCaseData<IntPtr>(-1), new TestCaseData<UIntPtr>(1),
		new TestCaseData<Int128>(Int128.NegativeOne), new TestCaseData<UInt128>(UInt128.One),
		new TestCaseData<BigInteger>(BigInteger.MinusOne)
	];

	private static readonly TestCaseData[] _real =
	[
		new TestCaseData<float>(1.0f),
		new TestCaseData<double>(-1.0d),
		new TestCaseData<decimal>(decimal.MinusOne),
		new TestCaseData<Half>(Half.NegativeOne)
	];

	[Test, TestCaseSource(nameof(_integral))]
	public void IsIntegral_Integral_True<T>(T integral) =>
		Assert.That(integral.IsIntegral(), Is.True);

	[Test, TestCaseSource(nameof(_integral))]
	public void IsIntegral_BoxedIntegral_False(object boxedIntegral) =>
		Assert.That(boxedIntegral.IsIntegral(), Is.False);

	[Test, TestCaseSource(nameof(_integral))]
	public void IsBoxedIntegral_Integral_True(object boxedIntegral) =>
		Assert.That(boxedIntegral.IsBoxedIntegral(), Is.True);

	[Test, TestCaseSource(nameof(_real))]
	public void IsIntegral_Real_False<TReal>(TReal real) =>
		Assert.That(real.IsIntegral(), Is.False);
	
	[Test, TestCaseSource(nameof(_real))]
	public void IsIntegral_BoxedReal_False(object boxedReal) =>
		Assert.That(boxedReal.IsIntegral(), Is.False);

	[Test, TestCaseSource(nameof(_real))]
	public void IsBoxedIntegral_BoxedReal_False(object boxedReal) =>
		Assert.That(boxedReal.IsBoxedIntegral(), Is.False);

	[Test, TestCaseSource(nameof(_real))]
	public void IsReal_Real_True<TReal>(TReal real) =>
		Assert.That(real.IsReal(), Is.True);
	
	[Test, TestCaseSource(nameof(_real))]
	public void IsReal_BoxedReal_False(object boxedReal) =>
		Assert.That(boxedReal.IsReal(), Is.False);

	[Test, TestCaseSource(nameof(_real))]
	public void IsBoxedReal_BoxedReal_True(object boxedReal) =>
		Assert.That(boxedReal.IsBoxedReal(), Is.True);

	[Test, TestCaseSource(nameof(_integral))]
	public void IsReal_Integral_False<TIntegral>(TIntegral integral) =>
		Assert.That(integral.IsReal(), Is.False);
	
	[Test, TestCaseSource(nameof(_integral))]
	public void IsReal_BoxedIntegral_False(object boxedIntegral) =>
		Assert.That(boxedIntegral.IsReal(), Is.False);

	[Test, TestCaseSource(nameof(_integral))]
	public void IsBoxedReal_BoxedReal_False(object boxedIntegral) =>
		Assert.That(boxedIntegral.IsBoxedReal(), Is.False);

	private static readonly TestCaseData[] _signed =
	[
		new TestCaseData<sbyte>(-1), new TestCaseData<short>(-1),
		new TestCaseData<int>(-1), new TestCaseData<long>(-1L),
		new TestCaseData<IntPtr>(-1), new TestCaseData<Int128>(Int128.NegativeOne),
		new TestCaseData<BigInteger>(BigInteger.MinusOne),
		new TestCaseData<decimal>(decimal.MinusOne), new TestCaseData<Half>(Half.NegativeOne),
		new TestCaseData<float>(-1.0f), new TestCaseData<double>(-1.0d)
	];
	
	private static readonly TestCaseData[] _unsigned =
	[
		new TestCaseData<byte>(1), new TestCaseData<ushort>(1),
		new TestCaseData<uint>(1u), new TestCaseData<ulong>(1UL),
		new TestCaseData<UIntPtr>(1), new TestCaseData<UInt128>(UInt128.One),
		new TestCaseData<char>('1')
	];

	[Test, TestCaseSource(nameof(_signed))]
	public void IsSigned_Signed_True<TSigned>(TSigned signed) =>
		Assert.That(signed.IsSigned(), Is.True);
	
	[Test, TestCaseSource(nameof(_signed))]
	public void IsSigned_BoxedSigned_False(object signed) =>
		Assert.That(signed.IsSigned(), Is.False);
	
	[Test, TestCaseSource(nameof(_signed))]
	public void IsBoxedSigned_BoxedSigned_True(object signed) =>
		Assert.That(signed.IsBoxedSigned(), Is.True);

	[Test, TestCaseSource(nameof(_unsigned))]
	public void IsSigned_Unsigned_False<TSigned>(TSigned signed) =>
		Assert.That(signed.IsSigned(), Is.False);
	
	[Test, TestCaseSource(nameof(_unsigned))]
	public void IsSigned_BoxedUnsigned_False(object unsigned) =>
		Assert.That(unsigned.IsSigned(), Is.False);
	
	[Test, TestCaseSource(nameof(_unsigned))]
	public void IsBoxedSigned_BoxedUnsigned_False(object unsigned) =>
		Assert.That(unsigned.IsBoxedSigned(), Is.False);

	[Test, TestCaseSource(nameof(_unsigned))]
	public void IsUnsigned_Unsigned_True<TUnsigned>(TUnsigned unsigned) =>
		Assert.That(unsigned.IsUnsigned(), Is.True);
	
	[Test, TestCaseSource(nameof(_unsigned))]
	public void IsUnsigned_BoxedUnsigned_False(object unsigned) =>
		Assert.That(unsigned.IsUnsigned(), Is.False);
	
	[Test, TestCaseSource(nameof(_unsigned))]
	public void IsBoxedUnsigned_BoxedUnsigned_True(object unsigned) =>
		Assert.That(unsigned.IsBoxedUnsigned(), Is.True);
	
	[Test, TestCaseSource(nameof(_signed))]
	public void IsUnsigned_Signed_False<TSigned>(TSigned signed) =>
		Assert.That(signed.IsUnsigned(), Is.False);
	
	[Test, TestCaseSource(nameof(_signed))]
	public void IsUnsigned_BoxedSigned_False(object signed) =>
		Assert.That(signed.IsUnsigned(), Is.False);
	
	[Test, TestCaseSource(nameof(_signed))]
	public void IsBoxedUnsigned_BoxedSigned_False(object signed) =>
		Assert.That(signed.IsBoxedUnsigned(), Is.False);
}