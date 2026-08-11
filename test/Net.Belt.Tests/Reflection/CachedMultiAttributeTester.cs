using System.Collections.Immutable;
using Net.Belt.Tests.Reflection.Support;

namespace Net.Belt.Tests.Reflection;

[TestFixture]
public class CachedMultiAttributeTester
{
	#region NotInheritedMultiAttribute

	[Test]
	public void CacheValues_NotInheritedMultiBase_SingleInSelf()
	{
		ImmutableArray<NotInheritedMultiAttribute> attrs = NotInheritedMultiAttribute.Cache<NotInheritedMultiBase>.Values;

		Assert.That(attrs, Has.Length.EqualTo(1));
		Assert.That(attrs[0], Is.InstanceOf<NotInheritedMultiAttribute>()
			.With.Property(nameof(NotInheritedMultiAttribute.Subject)).EqualTo("base"));
	}

	[Test]
	public void CacheFirst_NotInheritedMultiBase_AttrInSelf()
	{
		NotInheritedMultiAttribute? attr = NotInheritedMultiAttribute.Cache<NotInheritedMultiBase>.First;

		Assert.That(attr, Is.InstanceOf<NotInheritedMultiAttribute>()
			.With.Property(nameof(NotInheritedMultiAttribute.Subject)).EqualTo("base"));
	}

	[Test]
	public void CacheValues_NotInheritedMultiDerived_SingleInSelf()
	{
		// Inherited = false, so inherit: true only sees the single attribute on the derived type itself.
		ImmutableArray<NotInheritedMultiAttribute> attrs = NotInheritedMultiAttribute.Cache<NotInheritedMultiDerived>.Values;

		Assert.That(attrs, Has.Length.EqualTo(1));
		Assert.That(attrs[0], Is.InstanceOf<NotInheritedMultiAttribute>()
			.With.Property(nameof(NotInheritedMultiAttribute.Subject)).EqualTo("derived"));
	}

	[Test]
	public void CacheFirst_NotInheritedMultiDerived_AttrInSelf()
	{
		NotInheritedMultiAttribute? attr = NotInheritedMultiAttribute.Cache<NotInheritedMultiDerived>.First;

		Assert.That(attr, Is.InstanceOf<NotInheritedMultiAttribute>()
			.With.Property(nameof(NotInheritedMultiAttribute.Subject)).EqualTo("derived"));
	}

	[Test]
	public void CacheValues_NotInheritedMultiNotDecorated_Empty()
	{
		// Inherited = false, so the base attribute is not matched, despite inherit: true.
		ImmutableArray<NotInheritedMultiAttribute> attrs = NotInheritedMultiAttribute.Cache<NotInheritedMultiNotDecorated>.Values;

		Assert.That(attrs, Is.Empty);
	}

	[Test]
	public void CacheFirst_NotInheritedMultiNotDecorated_Null()
	{
		NotInheritedMultiAttribute? attr = NotInheritedMultiAttribute.Cache<NotInheritedMultiNotDecorated>.First;

		Assert.That(attr, Is.Null);
	}

	[Test]
	public void CacheValues_NotInheritedMulti_AllInSelf()
	{
		ImmutableArray<NotInheritedMultiAttribute> attrs = NotInheritedMultiAttribute.Cache<NotInheritedMulti>.Values;

		Assert.That(attrs, Has.Length.EqualTo(2));
		Assert.That(attrs[0], Is.InstanceOf<NotInheritedMultiAttribute>()
			.With.Property(nameof(NotInheritedMultiAttribute.Subject)).EqualTo("one"));
		Assert.That(attrs[1], Is.InstanceOf<NotInheritedMultiAttribute>()
			.With.Property(nameof(NotInheritedMultiAttribute.Subject)).EqualTo("two"));
	}

	[Test]
	public void CacheFirst_NotInheritedMulti_FirstInSelf()
	{
		NotInheritedMultiAttribute? attr = NotInheritedMultiAttribute.Cache<NotInheritedMulti>.First;

		Assert.That(attr, Is.InstanceOf<NotInheritedMultiAttribute>()
			.With.Property(nameof(NotInheritedMultiAttribute.Subject)).EqualTo("one"));
	}

	[Test]
	public void ExactCacheValues_NotInheritedMultiBase_SingleInSelf()
	{
		ImmutableArray<NotInheritedMultiAttribute> attrs = NotInheritedMultiAttribute.ExactCache<NotInheritedMultiBase>.Values;

		Assert.That(attrs, Has.Length.EqualTo(1));
		Assert.That(attrs[0], Is.InstanceOf<NotInheritedMultiAttribute>()
			.With.Property(nameof(NotInheritedMultiAttribute.Subject)).EqualTo("base"));
	}

	[Test]
	public void ExactCacheFirst_NotInheritedMultiBase_AttrInSelf()
	{
		NotInheritedMultiAttribute? attr = NotInheritedMultiAttribute.ExactCache<NotInheritedMultiBase>.First;

		Assert.That(attr, Is.InstanceOf<NotInheritedMultiAttribute>()
			.With.Property(nameof(NotInheritedMultiAttribute.Subject)).EqualTo("base"));
	}

	[Test]
	public void ExactCacheValues_NotInheritedMultiDerived_SingleInSelf()
	{
		ImmutableArray<NotInheritedMultiAttribute> attrs = NotInheritedMultiAttribute.ExactCache<NotInheritedMultiDerived>.Values;

		Assert.That(attrs, Has.Length.EqualTo(1));
		Assert.That(attrs[0], Is.InstanceOf<NotInheritedMultiAttribute>()
			.With.Property(nameof(NotInheritedMultiAttribute.Subject)).EqualTo("derived"));
	}

	[Test]
	public void ExactCacheFirst_NotInheritedMultiDerived_AttrInSelf()
	{
		NotInheritedMultiAttribute? attr = NotInheritedMultiAttribute.ExactCache<NotInheritedMultiDerived>.First;

		Assert.That(attr, Is.InstanceOf<NotInheritedMultiAttribute>()
			.With.Property(nameof(NotInheritedMultiAttribute.Subject)).EqualTo("derived"));
	}

	[Test]
	public void ExactCacheValues_NotInheritedMultiNotDecorated_Empty()
	{
		ImmutableArray<NotInheritedMultiAttribute> attrs = NotInheritedMultiAttribute.ExactCache<NotInheritedMultiNotDecorated>.Values;

		Assert.That(attrs, Is.Empty);
	}

	[Test]
	public void ExactCacheFirst_NotInheritedMultiNotDecorated_Null()
	{
		NotInheritedMultiAttribute? attr = NotInheritedMultiAttribute.ExactCache<NotInheritedMultiNotDecorated>.First;

		Assert.That(attr, Is.Null);
	}

	[Test]
	public void ExactCacheValues_NotInheritedMulti_AllInSelf()
	{
		ImmutableArray<NotInheritedMultiAttribute> attrs = NotInheritedMultiAttribute.ExactCache<NotInheritedMulti>.Values;

		Assert.That(attrs, Has.Length.EqualTo(2));
		Assert.That(attrs[0], Is.InstanceOf<NotInheritedMultiAttribute>()
			.With.Property(nameof(NotInheritedMultiAttribute.Subject)).EqualTo("one"));
		Assert.That(attrs[1], Is.InstanceOf<NotInheritedMultiAttribute>()
			.With.Property(nameof(NotInheritedMultiAttribute.Subject)).EqualTo("two"));
	}

	[Test]
	public void ExactCacheFirst_NotInheritedMulti_FirstInSelf()
	{
		NotInheritedMultiAttribute? attr = NotInheritedMultiAttribute.ExactCache<NotInheritedMulti>.First;

		Assert.That(attr, Is.InstanceOf<NotInheritedMultiAttribute>()
			.With.Property(nameof(NotInheritedMultiAttribute.Subject)).EqualTo("one"));
	}

	#endregion

	#region InheritedMultiAttribute

	[Test]
	public void CacheValues_InheritedMultiBase_SingleInSelf()
	{
		ImmutableArray<InheritedMultiAttribute> attrs = InheritedMultiAttribute.Cache<InheritedMultiBase>.Values;

		Assert.That(attrs, Has.Length.EqualTo(1));
		Assert.That(attrs[0], Is.InstanceOf<InheritedMultiAttribute>()
			.With.Property(nameof(InheritedMultiAttribute.Subject)).EqualTo("base"));
	}

	[Test]
	public void CacheFirst_InheritedMultiBase_AttrInSelf()
	{
		InheritedMultiAttribute? attr = InheritedMultiAttribute.Cache<InheritedMultiBase>.First;

		Assert.That(attr, Is.InstanceOf<InheritedMultiAttribute>()
			.With.Property(nameof(InheritedMultiAttribute.Subject)).EqualTo("base"));
	}

	[Test]
	public void CacheValues_InheritedMultiDerived_DerivedThenBase()
	{
		// Inherited = true and inherit: true: own attributes come first, then the inherited base attribute.
		ImmutableArray<InheritedMultiAttribute> attrs = InheritedMultiAttribute.Cache<InheritedMultiDerived>.Values;

		Assert.That(attrs, Has.Length.EqualTo(2));
		Assert.That(attrs[0], Is.InstanceOf<InheritedMultiAttribute>()
			.With.Property(nameof(InheritedMultiAttribute.Subject)).EqualTo("derived"));
		Assert.That(attrs[1], Is.InstanceOf<InheritedMultiAttribute>()
			.With.Property(nameof(InheritedMultiAttribute.Subject)).EqualTo("base"));
	}

	[Test]
	public void CacheFirst_InheritedMultiDerived_AttrInSelf()
	{
		InheritedMultiAttribute? attr = InheritedMultiAttribute.Cache<InheritedMultiDerived>.First;

		Assert.That(attr, Is.InstanceOf<InheritedMultiAttribute>()
			.With.Property(nameof(InheritedMultiAttribute.Subject)).EqualTo("derived"));
	}

	[Test]
	public void CacheValues_InheritedMultiNotDecorated_BaseInherited()
	{
		// Inherited = true, so the base attribute is inherited by the undecorated derived type.
		ImmutableArray<InheritedMultiAttribute> attrs = InheritedMultiAttribute.Cache<InheritedMultiNotDecorated>.Values;

		Assert.That(attrs, Has.Length.EqualTo(1));
		Assert.That(attrs[0], Is.InstanceOf<InheritedMultiAttribute>()
			.With.Property(nameof(InheritedMultiAttribute.Subject)).EqualTo("base"));
	}

	[Test]
	public void CacheFirst_InheritedMultiNotDecorated_AttrInBase()
	{
		InheritedMultiAttribute? attr = InheritedMultiAttribute.Cache<InheritedMultiNotDecorated>.First;

		Assert.That(attr, Is.InstanceOf<InheritedMultiAttribute>()
			.With.Property(nameof(InheritedMultiAttribute.Subject)).EqualTo("base"));
	}

	[Test]
	public void CacheValues_InheritedMulti_AllInSelf()
	{
		ImmutableArray<InheritedMultiAttribute> attrs = InheritedMultiAttribute.Cache<InheritedMulti>.Values;

		Assert.That(attrs, Has.Length.EqualTo(2));
		Assert.That(attrs[0], Is.InstanceOf<InheritedMultiAttribute>()
			.With.Property(nameof(InheritedMultiAttribute.Subject)).EqualTo("one"));
		Assert.That(attrs[1], Is.InstanceOf<InheritedMultiAttribute>()
			.With.Property(nameof(InheritedMultiAttribute.Subject)).EqualTo("two"));
	}

	[Test]
	public void CacheFirst_InheritedMulti_FirstInSelf()
	{
		InheritedMultiAttribute? attr = InheritedMultiAttribute.Cache<InheritedMulti>.First;

		Assert.That(attr, Is.InstanceOf<InheritedMultiAttribute>()
			.With.Property(nameof(InheritedMultiAttribute.Subject)).EqualTo("one"));
	}

	[Test]
	public void ExactCacheValues_InheritedMultiBase_SingleInSelf()
	{
		ImmutableArray<InheritedMultiAttribute> attrs = InheritedMultiAttribute.ExactCache<InheritedMultiBase>.Values;

		Assert.That(attrs, Has.Length.EqualTo(1));
		Assert.That(attrs[0], Is.InstanceOf<InheritedMultiAttribute>()
			.With.Property(nameof(InheritedMultiAttribute.Subject)).EqualTo("base"));
	}

	[Test]
	public void ExactCacheFirst_InheritedMultiBase_AttrInSelf()
	{
		InheritedMultiAttribute? attr = InheritedMultiAttribute.ExactCache<InheritedMultiBase>.First;

		Assert.That(attr, Is.InstanceOf<InheritedMultiAttribute>()
			.With.Property(nameof(InheritedMultiAttribute.Subject)).EqualTo("base"));
	}

	[Test]
	public void ExactCacheValues_InheritedMultiDerived_SingleInSelf()
	{
		ImmutableArray<InheritedMultiAttribute> attrs = InheritedMultiAttribute.ExactCache<InheritedMultiDerived>.Values;

		Assert.That(attrs, Has.Length.EqualTo(1));
		Assert.That(attrs[0], Is.InstanceOf<InheritedMultiAttribute>()
			.With.Property(nameof(InheritedMultiAttribute.Subject)).EqualTo("derived"));
	}

	[Test]
	public void ExactCacheFirst_InheritedMultiDerived_AttrInSelf()
	{
		InheritedMultiAttribute? attr = InheritedMultiAttribute.ExactCache<InheritedMultiDerived>.First;

		Assert.That(attr, Is.InstanceOf<InheritedMultiAttribute>()
			.With.Property(nameof(InheritedMultiAttribute.Subject)).EqualTo("derived"));
	}

	[Test]
	public void ExactCacheValues_InheritedMultiNotDecorated_Empty()
	{
		ImmutableArray<InheritedMultiAttribute> attrs = InheritedMultiAttribute.ExactCache<InheritedMultiNotDecorated>.Values;

		Assert.That(attrs, Is.Empty);
	}

	[Test]
	public void ExactCacheFirst_InheritedMultiNotDecorated_Null()
	{
		InheritedMultiAttribute? attr = InheritedMultiAttribute.ExactCache<InheritedMultiNotDecorated>.First;

		Assert.That(attr, Is.Null);
	}

	[Test]
	public void ExactCacheValues_InheritedMulti_AllInSelf()
	{
		ImmutableArray<InheritedMultiAttribute> attrs = InheritedMultiAttribute.ExactCache<InheritedMulti>.Values;

		Assert.That(attrs, Has.Length.EqualTo(2));
		Assert.That(attrs[0], Is.InstanceOf<InheritedMultiAttribute>()
			.With.Property(nameof(InheritedMultiAttribute.Subject)).EqualTo("one"));
		Assert.That(attrs[1], Is.InstanceOf<InheritedMultiAttribute>()
			.With.Property(nameof(InheritedMultiAttribute.Subject)).EqualTo("two"));
	}

	[Test]
	public void ExactCacheFirst_InheritedMulti_FirstInSelf()
	{
		InheritedMultiAttribute? attr = InheritedMultiAttribute.ExactCache<InheritedMulti>.First;

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
			Assert.That(NotInheritedMultiAttribute.Cache<NotDecorated>.Values, Is.Empty);
			Assert.That(NotInheritedMultiAttribute.ExactCache<NotDecorated>.Values, Is.Empty);
			Assert.That(InheritedMultiAttribute.Cache<NotDecorated>.Values, Is.Empty);
			Assert.That(InheritedMultiAttribute.ExactCache<NotDecorated>.Values, Is.Empty);
		});
	}

	[Test]
	public void First_NotDecorated_NullForAllAttributes()
	{
		Assert.That(typeof(NotDecorated).CustomAttributes, Is.Empty, "sanity check");

		Assert.Multiple(() =>
		{
			Assert.That(NotInheritedMultiAttribute.Cache<NotDecorated>.First, Is.Null);
			Assert.That(NotInheritedMultiAttribute.ExactCache<NotDecorated>.First, Is.Null);
			Assert.That(InheritedMultiAttribute.Cache<NotDecorated>.First, Is.Null);
			Assert.That(InheritedMultiAttribute.ExactCache<NotDecorated>.First, Is.Null);
		});
	}
}
