using System.Diagnostics.CodeAnalysis;

using Net.Belt.Extensions.Anonymous;

namespace Net.Belt.Tests.Extensions.Anonymous;

[TestFixture]
[SuppressMessage("ReSharper", "PreferConcreteValueOverDefault")]
public class AnonymousExtensionsTester
{
	private static object returnAnonymous(string p1, int p2) => new { P1 = p1, P2 = p2 };

	#region Cast

	[Test]
	public void Cast_OnObjects_CanStrongTypeSubsequentUses()
	{
		var example = new { P1 = default(string), P2 = default(int) };

		var typed = returnAnonymous("P1", 2).Cast(example);

		Assert.That(typed.P1, Is.EqualTo("P1"));
		Assert.That(typed.P2, Is.EqualTo(2));
	}
	
	[Test]
	public void Cast_OnCollections_CanStrongTypeSubsequentUses()
	{
		var example = new { P1 = default(string), P2 = default(int) };
		var enumerable = new[] { returnAnonymous("p1_1", 1), returnAnonymous("p1_2", 2) };

		var typed = enumerable.Select(a => a.Cast(example)).ToArray();

		Assert.That(typed, Has.Length.EqualTo(2).And
			.All.InstanceOf(example.GetType()));
		Assert.Multiple(() =>
		{
			Assert.That(typed[0].P1, Is.EqualTo("p1_1"));
			Assert.That(typed[0].P2, Is.EqualTo(1));
			Assert.That(typed[1].P1, Is.EqualTo("p1_2"));
			Assert.That(typed[1].P2, Is.EqualTo(2));
		});
	}

	#endregion
	
	#region AsTuples

	[Test]
	public void AsTuples_TuplesWithPropertyNamesAndValues()
	{
		var a = new { P1 = "1", P2 = 2 };

		Assert.That(a.AsTuples(), Is.EquivalentTo([
			new Tuple<string, object>("P1", "1"),
			new Tuple<string, object>("P2", 2)
		]));
	}

	[Test]
	public void AsTuples_NullProperties_NotSkipped()
	{
		var a = new { P1 = "1", P2 = (string?)null, P3 = 0 };

		Assert.That(a.AsTuples(), Is.EquivalentTo([
			new Tuple<string, object?>("P1", "1"),
			new Tuple<string, object?>("P2", null),
			new Tuple<string, object?>("P3", 0)
		]));
	}

	[Test]
	public void AsTuples_DefaultValueType_NoValueSkipped()
	{
		var a = new { P1 = 1, P2 = default(int), P3 = 3 };

		Assert.That(a.AsTuples(), Is.EquivalentTo([
			new Tuple<string, object>("P1", 1), 
			new Tuple<string, object>("P2", 0), 
			new Tuple<string, object>("P3", 3)
		]));
	}

	#endregion
	
	#region AsValueTuples

	[Test]
	public void AsValueTuples_TuplesWithPropertyNamesAndValues()
	{
		var a = new { P1 = "1", P2 = 2 };

		Assert.That(a.AsValueTuples(), Is.EquivalentTo([
			new ValueTuple<string, object>("P1", "1"),
			new ValueTuple<string, object>("P2", 2)
		]));
	}

	[Test]
	public void AsValueTuples_NullProperties_NotSkipped()
	{
		var a = new { P1 = "1", P2 = (string?)null, P3 = 0 };

		Assert.That(a.AsValueTuples(), Is.EquivalentTo([
			new ValueTuple<string, object?>("P1", "1"),
			new ValueTuple<string, object?>("P2", null),
			new ValueTuple<string, object?>("P3", 0)
		]));
	}

	[Test]
	public void AsValueTuples_DefaultValueType_NoValueSkipped()
	{
		var a = new { P1 = 1, P2 = default(int), P3 = 3 };

		Assert.That(a.AsValueTuples(), Is.EquivalentTo([
			new ValueTuple<string, object>("P1", 1), 
			new ValueTuple<string, object>("P2", 0), 
			new ValueTuple<string, object>("P3", 3)
		]));
	}

	#endregion
	
	#region AsKeyValuePairs

		[Test]
		public void AsPairs_PairsWithPropertyNamesAndValues()
		{
			var a = new { P1 = "1", P2 = 2 };

			Assert.That(a.AsPairs(), Is.EquivalentTo([
				new KeyValuePair<string, object>("P1", "1"),
				new KeyValuePair<string, object>("P2", 2)
			]));
		}

		[Test]
		public void AsPairs_NullProperties_NotSkipped()
		{
			var a = new { P1 = "1", P2 = (string?)null, P3 = 0 };

			Assert.That(a.AsPairs(), Is.EquivalentTo([
				new KeyValuePair<string, object?>("P1", "1"),
				new KeyValuePair<string, object?>("P2", null),
				new KeyValuePair<string, object?>("P3", 0)
			]));
		}

		[Test]
		public void AsPairs_DefaultValueType_NoValueSkipped()
		{
			var a = new { P1 = 1, P2 = default(int), P3 = 3 };

			Assert.That(a.AsPairs(), Is.EquivalentTo([
				new KeyValuePair<string, object>("P1", 1), 
				new KeyValuePair<string, object>("P2", 0), 
				new KeyValuePair<string, object>("P3", 3)
			]));
		}

		#endregion

		#region AsDictionary

		[Test]
		public void AsDictionary_PairsWithPropertyNamesAndValues()
		{
			var a = new { P1 = "1", P2 = 2 };

			Assert.That(a.AsDictionary(), Is.EquivalentTo([
				new KeyValuePair<string, object>("P1", "1"),
				new KeyValuePair<string, object>("P2", 2)
			]));
		}

		[Test]
		public void AsDictionarys_NullProperties_NotSkipped()
		{
			var a = new { P1 = "1", P2 = (string?)null, P3 = 0 };

			Assert.That(a.AsDictionary(), Is.EquivalentTo([
				new KeyValuePair<string, object?>("P1", "1"),
				new KeyValuePair<string, object?>("P2", null),
				new KeyValuePair<string, object?>("P3", 0)
			]));
		}

		[Test]
		public void AsDictionary_DefaultValueType_NoValueSkipped()
		{
			var a = new { P1 = 1, P2 = default(int), P3 = 3 };

			Assert.That(a.AsDictionary(), Is.EquivalentTo([
				new KeyValuePair<string, object>("P1", 1), 
				new KeyValuePair<string, object>("P2", 0), 
				new KeyValuePair<string, object>("P3", 3)
			]));
		}

		#endregion

		#region AsAnonymous

		[Test]
		public void AsAnonymous_AnonymousAsPerPrototype()
		{
			var d = new Dictionary<string, object?>
			{
				{ "P1", "1" },
				{ "P2", 2 }
			};

			var prototype = new { P1 = default(string), P2 = default(int) };
			Assert.That(d.AsAnonymous(prototype), Is.EqualTo(new { P1 = "1", P2 = 2 }));
		}

		[Test]
		public void AsAnonymous_NullElementsNotSkipped()
		{
			var d = new Dictionary<string, object?>
			{
				{ "P1", "1" },
				{ "P2", null },
				{ "P3", 0 }
			};

			var prototype = new {P1 = default(string?), P2 = default(string?), P3 = default(int)};
			Assert.That(d.AsAnonymous(prototype), Is.EqualTo(new { P1 = "1", P2 = (string?)null, P3 = 0 }));
		}
		
		[Test]
		public void AsAnonymous_NullWeirdness()
		{
			var d = new Dictionary<string, object?>
			{
				{ "P1", "1" },
				{ "P2", null },
				{ "P3", 0 }
			};

			var prototype = new {P1 = default(string?), P2 = default(string?), P3 = default(int)};
			Assert.That(d.AsAnonymous(prototype), Is.EqualTo(new { P1 = "1", P2 = (string?)null, P3 = 0 }));
		}

		#endregion
}