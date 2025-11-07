using Net.Belt.Comparisons;
using Net.Belt.Tests.Comparisons.Support;

namespace Net.Belt.Tests.Comparisons;

[TestFixture]
public class HasherTester
{
	[Test]
	public void DefaultHasher_NotNull_InvokedGetHashCodeOnObject()
	{
		var spy = new EqualitySpy();

		Hasher.Default(spy);

		Assert.That(spy.GetHashCodeCalled, Is.True);
	}

	[Test]
	public void ZeroHasher_ReturnsZero()
	{
		var spy = new EqualitySpy();

		Assert.That(Hasher.Zero(spy), Is.EqualTo(0));

		Assert.That(spy.GetHashCodeCalled, Is.False);
	}

	[Test]
	public void CanonicalHasher_SameValue_AsResharperImplementation()
	{
		var baseline = new EqualitySubject { I = 243, D = 298.75m, S = "dGG" };

		int actual = Hasher.Canonical(baseline.I, baseline.D, baseline.S);

		Assert.That(actual, Is.EqualTo(baseline.GetHashCode()));
	}
	
	[Test]
	public void FluentHasher_FluentInterfaceOverHashCode()
	{
		var baseline = new EqualitySubject { I = 243, D = 298.75m, S = "dGG" };

		int fluent = Hasher.Fluent(baseline.I)
			.Hashing(baseline.D)
			.Hashing(baseline.S)
			.ToHashCode();
		
		Assert.That(fluent, Is.Not.EqualTo(0));
	}

	[Test]
	public void CanonicalHasher_NoValues_Zero()
	{
		Assert.That(Hasher.Canonical<string?>(null), Is.EqualTo(0));
	}
	
	[Test]
	public void DefaultHasher_NoValues_NotZero()
	{
		Assert.That(Hasher.Default<string?>(null), Is.Not.EqualTo(0));
	}

	[Test]
	public void CanonicalHasher_OrderOfArguments_Matters()
	{
		int oneTwoThree = Hasher.Canonical(1, 2, 3),
			twoThreeOne = Hasher.Canonical(2, 3, 1);

		Assert.That(oneTwoThree, Is.Not.EqualTo(twoThreeOne));
	}
	
	[Test]
	public void DefaultHasher_OrderOfArguments_Matters()
	{
		int oneTwoThree = Hasher.Default(1, 2, 3),
			twoThreeOne = Hasher.Default(2, 3, 1);

		Assert.That(oneTwoThree, Is.Not.EqualTo(twoThreeOne));
	}
}