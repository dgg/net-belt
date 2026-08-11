using Net.Belt.Tests.Reflection.Support;

namespace Net.Belt.Tests.Reflection;

[TestFixture]
public class CachedAttributeTester
{
	#region InheritedSingleAttribute

	[Test]
	public void CacheValue_InheritedSingleBase_AttrInSelf()
	{
		InheritedSingleAttribute? attr = InheritedSingleAttribute.Cache<InheritedSingleBase>.Value;

		Assert.That(attr, Is.InstanceOf<InheritedSingleAttribute>()
			.With.Property(nameof(InheritedSingleAttribute.Subject)).EqualTo("base"));
	}

	[Test]
	public void CacheValue_InheritedSingleDerived_AttrInSelf()
	{
		InheritedSingleAttribute? attr = InheritedSingleAttribute.Cache<InheritedSingleDerived>.Value;

		Assert.That(attr, Is.InstanceOf<InheritedSingleAttribute>()
			.With.Property(nameof(InheritedSingleAttribute.Subject)).EqualTo("derived"));
	}

	[Test]
	public void CacheValue_InheritedSingleNotDecorated_AttrInBase()
	{
		InheritedSingleAttribute? attr = InheritedSingleAttribute.Cache<InheritedSingleNotDecorated>.Value;

		Assert.That(attr, Is.InstanceOf<InheritedSingleAttribute>()
			.With.Property(nameof(InheritedSingleAttribute.Subject)).EqualTo("base"));
	}

	[Test]
	public void ExactCacheValue_InheritedSingleBase_AttrInSelf()
	{
		InheritedSingleAttribute? attr = InheritedSingleAttribute.ExactCache<InheritedSingleBase>.Value;

		Assert.That(attr, Is.InstanceOf<InheritedSingleAttribute>()
			.With.Property(nameof(InheritedSingleAttribute.Subject)).EqualTo("base"));
	}

	[Test]
	public void ExactCacheValue_InheritedSingleDerived_AttrInSelf()
	{
		InheritedSingleAttribute? attr = InheritedSingleAttribute.ExactCache<InheritedSingleDerived>.Value;

		Assert.That(attr, Is.InstanceOf<InheritedSingleAttribute>()
			.With.Property(nameof(InheritedSingleAttribute.Subject)).EqualTo("derived"));
	}

	[Test]
	public void ExactCacheValue_InheritedSingleNotDecorated_AttrNotFound()
	{
		InheritedSingleAttribute? attr = InheritedSingleAttribute.ExactCache<InheritedSingleNotDecorated>.Value;

		Assert.That(attr, Is.Null);
	}

	#endregion

	#region NotInheritedSingleAttribute

	[Test]
	public void CacheValue_NotInheritedSingleBase_AttrInSelf()
	{
		NotInheritedSingleAttribute? attr = NotInheritedSingleAttribute.Cache<NotInheritedSingleBase>.Value;

		Assert.That(attr, Is.InstanceOf<NotInheritedSingleAttribute>()
			.With.Property(nameof(NotInheritedSingleAttribute.Subject)).EqualTo("base"));
	}

	[Test]
	public void CacheValue_NotInheritedSingleDerived_AttrInSelf()
	{
		NotInheritedSingleAttribute? attr = NotInheritedSingleAttribute.Cache<NotInheritedSingleDerived>.Value;

		Assert.That(attr, Is.InstanceOf<NotInheritedSingleAttribute>()
			.With.Property(nameof(NotInheritedSingleAttribute.Subject)).EqualTo("derived"));
	}

	[Test]
	public void CacheValue_NotInheritedSingleNotDecorated_AttrNotFound()
	{
		// Since Inherited = false, the attribute is not matched, despite inherit: true.
		NotInheritedSingleAttribute? attr = NotInheritedSingleAttribute.Cache<NotInheritedSingleNotDecorated>.Value;

		Assert.That(attr, Is.Null);
	}

	[Test]
	public void ExactCacheValue_NotInheritedSingleBase_AttrInSelf()
	{
		NotInheritedSingleAttribute? attr = NotInheritedSingleAttribute.ExactCache<NotInheritedSingleBase>.Value;

		Assert.That(attr, Is.InstanceOf<NotInheritedSingleAttribute>()
			.With.Property(nameof(NotInheritedSingleAttribute.Subject)).EqualTo("base"));
	}

	[Test]
	public void ExactCacheValue_NotInheritedSingleDerived_AttrInSelf()
	{
		NotInheritedSingleAttribute? attr = NotInheritedSingleAttribute.ExactCache<NotInheritedSingleDerived>.Value;

		Assert.That(attr, Is.InstanceOf<NotInheritedSingleAttribute>()
			.With.Property(nameof(NotInheritedSingleAttribute.Subject)).EqualTo("derived"));
	}

	[Test]
	public void ExactCacheValue_NotInheritedSingleNotDecorated_AttrNotFound()
	{
		NotInheritedSingleAttribute? attr = NotInheritedSingleAttribute.ExactCache<NotInheritedSingleNotDecorated>.Value;

		Assert.That(attr, Is.Null);
	}

	#endregion

	[Test]
	public void Value_NotDecorated_AttrNotFound()
	{
		Assert.That(typeof(NotDecorated).CustomAttributes, Is.Empty, "sanity check");

		Assert.Multiple(() =>
		{
			Assert.That(InheritedSingleAttribute.Cache<NotDecorated>.Value, Is.Null);
			Assert.That(InheritedSingleAttribute.ExactCache<NotDecorated>.Value, Is.Null);
			Assert.That(NotInheritedSingleAttribute.Cache<NotDecorated>.Value, Is.Null);
			Assert.That(NotInheritedSingleAttribute.ExactCache<NotDecorated>.Value, Is.Null);
		});
	}
}
