using System.Collections.Immutable;

using Net.Belt.Reflection;
using Net.Belt.Tests.Reflection.Support;


namespace Net.Belt.Tests.Reflection;

[TestFixture]
public class MultiCacheTester
{
	#region InheritedSingleAttribute

	[Test]
	public void ForValues_InheritedSingleBase_SingleInSelf()
	{
		ImmutableArray<InheritedSingleAttribute> attrs = MultiCache<InheritedSingleAttribute>.For<InheritedSingleBase>.Values;

		Assert.That(attrs, Has.Length.EqualTo(1));
		Assert.That(attrs[0], Is.InstanceOf<InheritedSingleAttribute>()
			.With.Property(nameof(InheritedSingleAttribute.Subject)).EqualTo("base"));
	}

	[Test]
	public void ForFirst_InheritedSingleBase_AttrInSelf()
	{
		InheritedSingleAttribute? attr = MultiCache<InheritedSingleAttribute>.For<InheritedSingleBase>.First;

		Assert.That(attr, Is.InstanceOf<InheritedSingleAttribute>()
			.With.Property(nameof(InheritedSingleAttribute.Subject)).EqualTo("base"));
	}

	[Test]
	public void ForValues_InheritedSingleDerived_SingleInSelf()
	{
		// AllowMultiple = false, so the derived attribute shadows the inherited base one.
		ImmutableArray<InheritedSingleAttribute> attrs = MultiCache<InheritedSingleAttribute>.For<InheritedSingleDerived>.Values;

		Assert.That(attrs, Has.Length.EqualTo(1));
		Assert.That(attrs[0], Is.InstanceOf<InheritedSingleAttribute>()
			.With.Property(nameof(InheritedSingleAttribute.Subject)).EqualTo("derived"));
	}

	[Test]
	public void ForFirst_InheritedSingleDerived_AttrInSelf()
	{
		InheritedSingleAttribute? attr = MultiCache<InheritedSingleAttribute>.For<InheritedSingleDerived>.First;

		Assert.That(attr, Is.InstanceOf<InheritedSingleAttribute>()
			.With.Property(nameof(InheritedSingleAttribute.Subject)).EqualTo("derived"));
	}

	[Test]
	public void ForValues_InheritedSingleNotDecorated_BaseInherited()
	{
		// Inherited = true, so the base attribute is inherited by the undecorated derived type.
		ImmutableArray<InheritedSingleAttribute> attrs = MultiCache<InheritedSingleAttribute>.For<InheritedSingleNotDecorated>.Values;

		Assert.That(attrs, Has.Length.EqualTo(1));
		Assert.That(attrs[0], Is.InstanceOf<InheritedSingleAttribute>()
			.With.Property(nameof(InheritedSingleAttribute.Subject)).EqualTo("base"));
	}

	[Test]
	public void ForFirst_InheritedSingleNotDecorated_AttrInBase()
	{
		InheritedSingleAttribute? attr = MultiCache<InheritedSingleAttribute>.For<InheritedSingleNotDecorated>.First;

		Assert.That(attr, Is.InstanceOf<InheritedSingleAttribute>()
			.With.Property(nameof(InheritedSingleAttribute.Subject)).EqualTo("base"));
	}

	[Test]
	public void ForExactValues_InheritedSingleBase_SingleInSelf()
	{
		ImmutableArray<InheritedSingleAttribute> attrs = MultiCache<InheritedSingleAttribute>.ForExact<InheritedSingleBase>.Values;

		Assert.That(attrs, Has.Length.EqualTo(1));
		Assert.That(attrs[0], Is.InstanceOf<InheritedSingleAttribute>()
			.With.Property(nameof(InheritedSingleAttribute.Subject)).EqualTo("base"));
	}

	[Test]
	public void ForExactFirst_InheritedSingleBase_AttrInSelf()
	{
		InheritedSingleAttribute? attr = MultiCache<InheritedSingleAttribute>.ForExact<InheritedSingleBase>.First;

		Assert.That(attr, Is.InstanceOf<InheritedSingleAttribute>()
			.With.Property(nameof(InheritedSingleAttribute.Subject)).EqualTo("base"));
	}

	[Test]
	public void ForExactValues_InheritedSingleDerived_SingleInSelf()
	{
		ImmutableArray<InheritedSingleAttribute> attrs = MultiCache<InheritedSingleAttribute>.ForExact<InheritedSingleDerived>.Values;

		Assert.That(attrs, Has.Length.EqualTo(1));
		Assert.That(attrs[0], Is.InstanceOf<InheritedSingleAttribute>()
			.With.Property(nameof(InheritedSingleAttribute.Subject)).EqualTo("derived"));
	}

	[Test]
	public void ForExactFirst_InheritedSingleDerived_AttrInSelf()
	{
		InheritedSingleAttribute? attr = MultiCache<InheritedSingleAttribute>.ForExact<InheritedSingleDerived>.First;

		Assert.That(attr, Is.InstanceOf<InheritedSingleAttribute>()
			.With.Property(nameof(InheritedSingleAttribute.Subject)).EqualTo("derived"));
	}

	[Test]
	public void ForExactValues_InheritedSingleNotDecorated_Empty()
	{
		ImmutableArray<InheritedSingleAttribute> attrs = MultiCache<InheritedSingleAttribute>.ForExact<InheritedSingleNotDecorated>.Values;

		Assert.That(attrs, Is.Empty);
	}

	[Test]
	public void ForExactFirst_InheritedSingleNotDecorated_Null()
	{
		InheritedSingleAttribute? attr = MultiCache<InheritedSingleAttribute>.ForExact<InheritedSingleNotDecorated>.First;

		Assert.That(attr, Is.Null);
	}

	#endregion

	#region NotInheritedSingleAttribute

	[Test]
	public void ForValues_NotInheritedSingleBase_SingleInSelf()
	{
		ImmutableArray<NotInheritedSingleAttribute> attrs = MultiCache<NotInheritedSingleAttribute>.For<NotInheritedSingleBase>.Values;

		Assert.That(attrs, Has.Length.EqualTo(1));
		Assert.That(attrs[0], Is.InstanceOf<NotInheritedSingleAttribute>()
			.With.Property(nameof(NotInheritedSingleAttribute.Subject)).EqualTo("base"));
	}

	[Test]
	public void ForFirst_NotInheritedSingleBase_AttrInSelf()
	{
		NotInheritedSingleAttribute? attr = MultiCache<NotInheritedSingleAttribute>.For<NotInheritedSingleBase>.First;

		Assert.That(attr, Is.InstanceOf<NotInheritedSingleAttribute>()
			.With.Property(nameof(NotInheritedSingleAttribute.Subject)).EqualTo("base"));
	}

	[Test]
	public void ForValues_NotInheritedSingleDerived_SingleInSelf()
	{
		ImmutableArray<NotInheritedSingleAttribute> attrs = MultiCache<NotInheritedSingleAttribute>.For<NotInheritedSingleDerived>.Values;

		Assert.That(attrs, Has.Length.EqualTo(1));
		Assert.That(attrs[0], Is.InstanceOf<NotInheritedSingleAttribute>()
			.With.Property(nameof(NotInheritedSingleAttribute.Subject)).EqualTo("derived"));
	}

	[Test]
	public void ForFirst_NotInheritedSingleDerived_AttrInSelf()
	{
		NotInheritedSingleAttribute? attr = MultiCache<NotInheritedSingleAttribute>.For<NotInheritedSingleDerived>.First;

		Assert.That(attr, Is.InstanceOf<NotInheritedSingleAttribute>()
			.With.Property(nameof(NotInheritedSingleAttribute.Subject)).EqualTo("derived"));
	}

	[Test]
	public void ForValues_NotInheritedSingleNotDecorated_Empty()
	{
		// Inherited = false, so the base attribute is not matched, despite inherit: true.
		ImmutableArray<NotInheritedSingleAttribute> attrs = MultiCache<NotInheritedSingleAttribute>.For<NotInheritedSingleNotDecorated>.Values;

		Assert.That(attrs, Is.Empty);
	}

	[Test]
	public void ForFirst_NotInheritedSingleNotDecorated_Null()
	{
		NotInheritedSingleAttribute? attr = MultiCache<NotInheritedSingleAttribute>.For<NotInheritedSingleNotDecorated>.First;

		Assert.That(attr, Is.Null);
	}

	[Test]
	public void ForExactValues_NotInheritedSingleBase_SingleInSelf()
	{
		ImmutableArray<NotInheritedSingleAttribute> attrs = MultiCache<NotInheritedSingleAttribute>.ForExact<NotInheritedSingleBase>.Values;

		Assert.That(attrs, Has.Length.EqualTo(1));
		Assert.That(attrs[0], Is.InstanceOf<NotInheritedSingleAttribute>()
			.With.Property(nameof(NotInheritedSingleAttribute.Subject)).EqualTo("base"));
	}

	[Test]
	public void ForExactFirst_NotInheritedSingleBase_AttrInSelf()
	{
		NotInheritedSingleAttribute? attr = MultiCache<NotInheritedSingleAttribute>.ForExact<NotInheritedSingleBase>.First;

		Assert.That(attr, Is.InstanceOf<NotInheritedSingleAttribute>()
			.With.Property(nameof(NotInheritedSingleAttribute.Subject)).EqualTo("base"));
	}

	[Test]
	public void ForExactValues_NotInheritedSingleDerived_SingleInSelf()
	{
		ImmutableArray<NotInheritedSingleAttribute> attrs = MultiCache<NotInheritedSingleAttribute>.ForExact<NotInheritedSingleDerived>.Values;

		Assert.That(attrs, Has.Length.EqualTo(1));
		Assert.That(attrs[0], Is.InstanceOf<NotInheritedSingleAttribute>()
			.With.Property(nameof(NotInheritedSingleAttribute.Subject)).EqualTo("derived"));
	}

	[Test]
	public void ForExactFirst_NotInheritedSingleDerived_AttrInSelf()
	{
		NotInheritedSingleAttribute? attr = MultiCache<NotInheritedSingleAttribute>.ForExact<NotInheritedSingleDerived>.First;

		Assert.That(attr, Is.InstanceOf<NotInheritedSingleAttribute>()
			.With.Property(nameof(NotInheritedSingleAttribute.Subject)).EqualTo("derived"));
	}

	[Test]
	public void ForExactValues_NotInheritedSingleNotDecorated_Empty()
	{
		ImmutableArray<NotInheritedSingleAttribute> attrs = MultiCache<NotInheritedSingleAttribute>.ForExact<NotInheritedSingleNotDecorated>.Values;

		Assert.That(attrs, Is.Empty);
	}

	[Test]
	public void ForExactFirst_NotInheritedSingleNotDecorated_Null()
	{
		NotInheritedSingleAttribute? attr = MultiCache<NotInheritedSingleAttribute>.ForExact<NotInheritedSingleNotDecorated>.First;

		Assert.That(attr, Is.Null);
	}

	#endregion

	#region NotInheritedMultiAttribute

	[Test]
	public void ForValues_NotInheritedMultiBase_SingleInSelf()
	{
		ImmutableArray<NotInheritedMultiAttribute> attrs = MultiCache<NotInheritedMultiAttribute>.For<NotInheritedMultiBase>.Values;

		Assert.That(attrs, Has.Length.EqualTo(1));
		Assert.That(attrs[0], Is.InstanceOf<NotInheritedMultiAttribute>()
			.With.Property(nameof(NotInheritedMultiAttribute.Subject)).EqualTo("base"));
	}

	[Test]
	public void ForFirst_NotInheritedMultiBase_AttrInSelf()
	{
		NotInheritedMultiAttribute? attr = MultiCache<NotInheritedMultiAttribute>.For<NotInheritedMultiBase>.First;

		Assert.That(attr, Is.InstanceOf<NotInheritedMultiAttribute>()
			.With.Property(nameof(NotInheritedMultiAttribute.Subject)).EqualTo("base"));
	}

	[Test]
	public void ForValues_NotInheritedMultiDerived_SingleInSelf()
	{
		// Inherited = false, so inherit: true only sees the single attribute on the derived type itself.
		ImmutableArray<NotInheritedMultiAttribute> attrs = MultiCache<NotInheritedMultiAttribute>.For<NotInheritedMultiDerived>.Values;

		Assert.That(attrs, Has.Length.EqualTo(1));
		Assert.That(attrs[0], Is.InstanceOf<NotInheritedMultiAttribute>()
			.With.Property(nameof(NotInheritedMultiAttribute.Subject)).EqualTo("derived"));
	}

	[Test]
	public void ForFirst_NotInheritedMultiDerived_AttrInSelf()
	{
		NotInheritedMultiAttribute? attr = MultiCache<NotInheritedMultiAttribute>.For<NotInheritedMultiDerived>.First;

		Assert.That(attr, Is.InstanceOf<NotInheritedMultiAttribute>()
			.With.Property(nameof(NotInheritedMultiAttribute.Subject)).EqualTo("derived"));
	}

	[Test]
	public void ForValues_NotInheritedMultiNotDecorated_Empty()
	{
		// Inherited = false, so the base attribute is not matched, despite inherit: true.
		ImmutableArray<NotInheritedMultiAttribute> attrs = MultiCache<NotInheritedMultiAttribute>.For<NotInheritedMultiNotDecorated>.Values;

		Assert.That(attrs, Is.Empty);
	}

	[Test]
	public void ForFirst_NotInheritedMultiNotDecorated_Null()
	{
		NotInheritedMultiAttribute? attr = MultiCache<NotInheritedMultiAttribute>.For<NotInheritedMultiNotDecorated>.First;

		Assert.That(attr, Is.Null);
	}

	[Test]
	public void ForValues_NotInheritedMulti_AllInSelf()
	{
		ImmutableArray<NotInheritedMultiAttribute> attrs = MultiCache<NotInheritedMultiAttribute>.For<NotInheritedMulti>.Values;

		Assert.That(attrs, Has.Length.EqualTo(2));
		Assert.That(attrs[0], Is.InstanceOf<NotInheritedMultiAttribute>()
			.With.Property(nameof(NotInheritedMultiAttribute.Subject)).EqualTo("one"));
		Assert.That(attrs[1], Is.InstanceOf<NotInheritedMultiAttribute>()
			.With.Property(nameof(NotInheritedMultiAttribute.Subject)).EqualTo("two"));
	}

	[Test]
	public void ForFirst_NotInheritedMulti_FirstInSelf()
	{
		NotInheritedMultiAttribute? attr = MultiCache<NotInheritedMultiAttribute>.For<NotInheritedMulti>.First;

		Assert.That(attr, Is.InstanceOf<NotInheritedMultiAttribute>()
			.With.Property(nameof(NotInheritedMultiAttribute.Subject)).EqualTo("one"));
	}

	[Test]
	public void ForExactValues_NotInheritedMultiBase_SingleInSelf()
	{
		ImmutableArray<NotInheritedMultiAttribute> attrs = MultiCache<NotInheritedMultiAttribute>.ForExact<NotInheritedMultiBase>.Values;

		Assert.That(attrs, Has.Length.EqualTo(1));
		Assert.That(attrs[0], Is.InstanceOf<NotInheritedMultiAttribute>()
			.With.Property(nameof(NotInheritedMultiAttribute.Subject)).EqualTo("base"));
	}

	[Test]
	public void ForExactFirst_NotInheritedMultiBase_AttrInSelf()
	{
		NotInheritedMultiAttribute? attr = MultiCache<NotInheritedMultiAttribute>.ForExact<NotInheritedMultiBase>.First;

		Assert.That(attr, Is.InstanceOf<NotInheritedMultiAttribute>()
			.With.Property(nameof(NotInheritedMultiAttribute.Subject)).EqualTo("base"));
	}

	[Test]
	public void ForExactValues_NotInheritedMultiDerived_SingleInSelf()
	{
		ImmutableArray<NotInheritedMultiAttribute> attrs = MultiCache<NotInheritedMultiAttribute>.ForExact<NotInheritedMultiDerived>.Values;

		Assert.That(attrs, Has.Length.EqualTo(1));
		Assert.That(attrs[0], Is.InstanceOf<NotInheritedMultiAttribute>()
			.With.Property(nameof(NotInheritedMultiAttribute.Subject)).EqualTo("derived"));
	}

	[Test]
	public void ForExactFirst_NotInheritedMultiDerived_AttrInSelf()
	{
		NotInheritedMultiAttribute? attr = MultiCache<NotInheritedMultiAttribute>.ForExact<NotInheritedMultiDerived>.First;

		Assert.That(attr, Is.InstanceOf<NotInheritedMultiAttribute>()
			.With.Property(nameof(NotInheritedMultiAttribute.Subject)).EqualTo("derived"));
	}

	[Test]
	public void ForExactValues_NotInheritedMultiNotDecorated_Empty()
	{
		ImmutableArray<NotInheritedMultiAttribute> attrs = MultiCache<NotInheritedMultiAttribute>.ForExact<NotInheritedMultiNotDecorated>.Values;

		Assert.That(attrs, Is.Empty);
	}

	[Test]
	public void ForExactFirst_NotInheritedMultiNotDecorated_Null()
	{
		NotInheritedMultiAttribute? attr = MultiCache<NotInheritedMultiAttribute>.ForExact<NotInheritedMultiNotDecorated>.First;

		Assert.That(attr, Is.Null);
	}

	[Test]
	public void ForExactValues_NotInheritedMulti_AllInSelf()
	{
		ImmutableArray<NotInheritedMultiAttribute> attrs = MultiCache<NotInheritedMultiAttribute>.ForExact<NotInheritedMulti>.Values;

		Assert.That(attrs, Has.Length.EqualTo(2));
		Assert.That(attrs[0], Is.InstanceOf<NotInheritedMultiAttribute>()
			.With.Property(nameof(NotInheritedMultiAttribute.Subject)).EqualTo("one"));
		Assert.That(attrs[1], Is.InstanceOf<NotInheritedMultiAttribute>()
			.With.Property(nameof(NotInheritedMultiAttribute.Subject)).EqualTo("two"));
	}

	[Test]
	public void ForExactFirst_NotInheritedMulti_FirstInSelf()
	{
		NotInheritedMultiAttribute? attr = MultiCache<NotInheritedMultiAttribute>.ForExact<NotInheritedMulti>.First;

		Assert.That(attr, Is.InstanceOf<NotInheritedMultiAttribute>()
			.With.Property(nameof(NotInheritedMultiAttribute.Subject)).EqualTo("one"));
	}

	#endregion

	#region InheritedMultiAttribute

	[Test]
	public void ForValues_InheritedMultiBase_SingleInSelf()
	{
		ImmutableArray<InheritedMultiAttribute> attrs = MultiCache<InheritedMultiAttribute>.For<InheritedMultiBase>.Values;

		Assert.That(attrs, Has.Length.EqualTo(1));
		Assert.That(attrs[0], Is.InstanceOf<InheritedMultiAttribute>()
			.With.Property(nameof(InheritedMultiAttribute.Subject)).EqualTo("base"));
	}

	[Test]
	public void ForFirst_InheritedMultiBase_AttrInSelf()
	{
		InheritedMultiAttribute? attr = MultiCache<InheritedMultiAttribute>.For<InheritedMultiBase>.First;

		Assert.That(attr, Is.InstanceOf<InheritedMultiAttribute>()
			.With.Property(nameof(InheritedMultiAttribute.Subject)).EqualTo("base"));
	}

	[Test]
	public void ForValues_InheritedMultiDerived_DerivedThenBase()
	{
		// Inherited = true and inherit: true: own attributes come first, then the inherited base attribute.
		ImmutableArray<InheritedMultiAttribute> attrs = MultiCache<InheritedMultiAttribute>.For<InheritedMultiDerived>.Values;

		Assert.That(attrs, Has.Length.EqualTo(2));
		Assert.That(attrs[0], Is.InstanceOf<InheritedMultiAttribute>()
			.With.Property(nameof(InheritedMultiAttribute.Subject)).EqualTo("derived"));
		Assert.That(attrs[1], Is.InstanceOf<InheritedMultiAttribute>()
			.With.Property(nameof(InheritedMultiAttribute.Subject)).EqualTo("base"));
	}

	[Test]
	public void ForFirst_InheritedMultiDerived_AttrInSelf()
	{
		InheritedMultiAttribute? attr = MultiCache<InheritedMultiAttribute>.For<InheritedMultiDerived>.First;

		Assert.That(attr, Is.InstanceOf<InheritedMultiAttribute>()
			.With.Property(nameof(InheritedMultiAttribute.Subject)).EqualTo("derived"));
	}

	[Test]
	public void ForValues_InheritedMultiNotDecorated_BaseInherited()
	{
		// Inherited = true, so the base attribute is inherited by the undecorated derived type.
		ImmutableArray<InheritedMultiAttribute> attrs = MultiCache<InheritedMultiAttribute>.For<InheritedMultiNotDecorated>.Values;

		Assert.That(attrs, Has.Length.EqualTo(1));
		Assert.That(attrs[0], Is.InstanceOf<InheritedMultiAttribute>()
			.With.Property(nameof(InheritedMultiAttribute.Subject)).EqualTo("base"));
	}

	[Test]
	public void ForFirst_InheritedMultiNotDecorated_AttrInBase()
	{
		InheritedMultiAttribute? attr = MultiCache<InheritedMultiAttribute>.For<InheritedMultiNotDecorated>.First;

		Assert.That(attr, Is.InstanceOf<InheritedMultiAttribute>()
			.With.Property(nameof(InheritedMultiAttribute.Subject)).EqualTo("base"));
	}

	[Test]
	public void ForValues_InheritedMulti_AllInSelf()
	{
		ImmutableArray<InheritedMultiAttribute> attrs = MultiCache<InheritedMultiAttribute>.For<InheritedMulti>.Values;

		Assert.That(attrs, Has.Length.EqualTo(2));
		Assert.That(attrs[0], Is.InstanceOf<InheritedMultiAttribute>()
			.With.Property(nameof(InheritedMultiAttribute.Subject)).EqualTo("one"));
		Assert.That(attrs[1], Is.InstanceOf<InheritedMultiAttribute>()
			.With.Property(nameof(InheritedMultiAttribute.Subject)).EqualTo("two"));
	}

	[Test]
	public void ForFirst_InheritedMulti_FirstInSelf()
	{
		InheritedMultiAttribute? attr = MultiCache<InheritedMultiAttribute>.For<InheritedMulti>.First;

		Assert.That(attr, Is.InstanceOf<InheritedMultiAttribute>()
			.With.Property(nameof(InheritedMultiAttribute.Subject)).EqualTo("one"));
	}

	[Test]
	public void ForExactValues_InheritedMultiBase_SingleInSelf()
	{
		ImmutableArray<InheritedMultiAttribute> attrs = MultiCache<InheritedMultiAttribute>.ForExact<InheritedMultiBase>.Values;

		Assert.That(attrs, Has.Length.EqualTo(1));
		Assert.That(attrs[0], Is.InstanceOf<InheritedMultiAttribute>()
			.With.Property(nameof(InheritedMultiAttribute.Subject)).EqualTo("base"));
	}

	[Test]
	public void ForExactFirst_InheritedMultiBase_AttrInSelf()
	{
		InheritedMultiAttribute? attr = MultiCache<InheritedMultiAttribute>.ForExact<InheritedMultiBase>.First;

		Assert.That(attr, Is.InstanceOf<InheritedMultiAttribute>()
			.With.Property(nameof(InheritedMultiAttribute.Subject)).EqualTo("base"));
	}

	[Test]
	public void ForExactValues_InheritedMultiDerived_SingleInSelf()
	{
		ImmutableArray<InheritedMultiAttribute> attrs = MultiCache<InheritedMultiAttribute>.ForExact<InheritedMultiDerived>.Values;

		Assert.That(attrs, Has.Length.EqualTo(1));
		Assert.That(attrs[0], Is.InstanceOf<InheritedMultiAttribute>()
			.With.Property(nameof(InheritedMultiAttribute.Subject)).EqualTo("derived"));
	}

	[Test]
	public void ForExactFirst_InheritedMultiDerived_AttrInSelf()
	{
		InheritedMultiAttribute? attr = MultiCache<InheritedMultiAttribute>.ForExact<InheritedMultiDerived>.First;

		Assert.That(attr, Is.InstanceOf<InheritedMultiAttribute>()
			.With.Property(nameof(InheritedMultiAttribute.Subject)).EqualTo("derived"));
	}

	[Test]
	public void ForExactValues_InheritedMultiNotDecorated_Empty()
	{
		ImmutableArray<InheritedMultiAttribute> attrs = MultiCache<InheritedMultiAttribute>.ForExact<InheritedMultiNotDecorated>.Values;

		Assert.That(attrs, Is.Empty);
	}

	[Test]
	public void ForExactFirst_InheritedMultiNotDecorated_Null()
	{
		InheritedMultiAttribute? attr = MultiCache<InheritedMultiAttribute>.ForExact<InheritedMultiNotDecorated>.First;

		Assert.That(attr, Is.Null);
	}

	[Test]
	public void ForExactValues_InheritedMulti_AllInSelf()
	{
		ImmutableArray<InheritedMultiAttribute> attrs = MultiCache<InheritedMultiAttribute>.ForExact<InheritedMulti>.Values;

		Assert.That(attrs, Has.Length.EqualTo(2));
		Assert.That(attrs[0], Is.InstanceOf<InheritedMultiAttribute>()
			.With.Property(nameof(InheritedMultiAttribute.Subject)).EqualTo("one"));
		Assert.That(attrs[1], Is.InstanceOf<InheritedMultiAttribute>()
			.With.Property(nameof(InheritedMultiAttribute.Subject)).EqualTo("two"));
	}

	[Test]
	public void ForExactFirst_InheritedMulti_FirstInSelf()
	{
		InheritedMultiAttribute? attr = MultiCache<InheritedMultiAttribute>.ForExact<InheritedMulti>.First;

		Assert.That(attr, Is.InstanceOf<InheritedMultiAttribute>()
			.With.Property(nameof(InheritedMultiAttribute.Subject)).EqualTo("one"));
	}

	#endregion

	[Test]
	public void Values_NotDecorated_EmptyForAllAttributes()
	{
		Assert.That(typeof(NotDecorated).CustomAttributes, Is.Empty, "sanity check");

		Assert.Multiple(() =>
		{
			Assert.That(MultiCache<InheritedSingleAttribute>.For<NotDecorated>.Values, Is.Empty);
			Assert.That(MultiCache<InheritedSingleAttribute>.ForExact<NotDecorated>.Values, Is.Empty);
			Assert.That(MultiCache<NotInheritedSingleAttribute>.For<NotDecorated>.Values, Is.Empty);
			Assert.That(MultiCache<NotInheritedSingleAttribute>.ForExact<NotDecorated>.Values, Is.Empty);
			Assert.That(MultiCache<NotInheritedMultiAttribute>.For<NotDecorated>.Values, Is.Empty);
			Assert.That(MultiCache<NotInheritedMultiAttribute>.ForExact<NotDecorated>.Values, Is.Empty);
			Assert.That(MultiCache<InheritedMultiAttribute>.For<NotDecorated>.Values, Is.Empty);
			Assert.That(MultiCache<InheritedMultiAttribute>.ForExact<NotDecorated>.Values, Is.Empty);
		});
	}

	[Test]
	public void First_NotDecorated_NullForAllAttributes()
	{
		Assert.That(typeof(NotDecorated).CustomAttributes, Is.Empty, "sanity check");

		Assert.Multiple(() =>
		{
			Assert.That(MultiCache<InheritedSingleAttribute>.For<NotDecorated>.First, Is.Null);
			Assert.That(MultiCache<InheritedSingleAttribute>.ForExact<NotDecorated>.First, Is.Null);
			Assert.That(MultiCache<NotInheritedSingleAttribute>.For<NotDecorated>.First, Is.Null);
			Assert.That(MultiCache<NotInheritedSingleAttribute>.ForExact<NotDecorated>.First, Is.Null);
			Assert.That(MultiCache<NotInheritedMultiAttribute>.For<NotDecorated>.First, Is.Null);
			Assert.That(MultiCache<NotInheritedMultiAttribute>.ForExact<NotDecorated>.First, Is.Null);
			Assert.That(MultiCache<InheritedMultiAttribute>.For<NotDecorated>.First, Is.Null);
			Assert.That(MultiCache<InheritedMultiAttribute>.ForExact<NotDecorated>.First, Is.Null);
		});
	}
}
