using System.Reflection;
using Net.Belt.Reflection;
using Net.Belt.Tests.Reflection.Support;


namespace Net.Belt.Tests.Reflection;

[TestFixture]
public class CacheTester
{
	#region InheritedSingleAttribute

	[Test]
	public void ForValue_InheritedSingleBase_AttrInSelf()
	{
		InheritedSingleAttribute? attr = Cache<InheritedSingleAttribute>.For<InheritedSingleBase>.Value;
		
		Assert.That(attr, Is.InstanceOf<InheritedSingleAttribute>()
			.With.Property(nameof(InheritedSingleAttribute.Subject)).EqualTo("base"));
	}
	
	[Test]
	public void ForValue_InheritedSingleDerived_AttrInSelf()
	{
		InheritedSingleAttribute? attr = Cache<InheritedSingleAttribute>.For<InheritedSingleDerived>.Value;
		
		Assert.That(attr, Is.InstanceOf<InheritedSingleAttribute>()
			.With.Property(nameof(InheritedSingleAttribute.Subject)).EqualTo("derived"));
	}
	
	[Test]
	public void ForValue_InheritedSingleNotDecorated_AttrInBase()
	{
		InheritedSingleAttribute? attr = Cache<InheritedSingleAttribute>.For<InheritedSingleNotDecorated>.Value;
		
		Assert.That(attr, Is.InstanceOf<InheritedSingleAttribute>()
			.With.Property(nameof(InheritedSingleAttribute.Subject)).EqualTo("base"));
	}
	
	[Test]
	public void ForExactValue_InheritedSingleBase_AttrInSelf()
	{
		InheritedSingleAttribute? attr = Cache<InheritedSingleAttribute>.ForExact<InheritedSingleBase>.Value;
		
		Assert.That(attr, Is.InstanceOf<InheritedSingleAttribute>()
			.With.Property(nameof(InheritedSingleAttribute.Subject)).EqualTo("base"));
	}
	
	[Test]
	public void ForExactValue_InheritedSingleDerived_AttrInSelf()
	{
		InheritedSingleAttribute? attr = Cache<InheritedSingleAttribute>.ForExact<InheritedSingleDerived>.Value;
		
		Assert.That(attr, Is.InstanceOf<InheritedSingleAttribute>()
			.With.Property(nameof(InheritedSingleAttribute.Subject)).EqualTo("derived"));
	}
	
	[Test]
	public void ForExactValue_InheritedSingleNotDecorated_AttrNotFound()
	{
		InheritedSingleAttribute? attr = Cache<InheritedSingleAttribute>.ForExact<InheritedSingleNotDecorated>.Value;
		
		Assert.That(attr, Is.Null);
	}
	
	#endregion
	
	#region NotInheritedSingleAttribute
	
	[Test]
	public void ForValue_NotInheritedSingleBase_AttrInSelf()
	{
		NotInheritedSingleAttribute? attr = Cache<NotInheritedSingleAttribute>.For<NotInheritedSingleBase>.Value;
		
		Assert.That(attr, Is.InstanceOf<NotInheritedSingleAttribute>()
			.With.Property(nameof(NotInheritedSingleAttribute.Subject)).EqualTo("base"));
	}
	
	[Test]
	public void ForValue_NotInheritedSingleDerived_AttrInSelf()
	{
		NotInheritedSingleAttribute? attr = Cache<NotInheritedSingleAttribute>.For<NotInheritedSingleDerived>.Value;
		
		Assert.That(attr, Is.InstanceOf<NotInheritedSingleAttribute>()
			.With.Property(nameof(NotInheritedSingleAttribute.Subject)).EqualTo("derived"));
	}
	
	[Test]
	public void ForValue_NotInheritedSingleNotDecorated_AttrNotFound()
	{
		NotInheritedSingleAttribute? attr = Cache<NotInheritedSingleAttribute>.For<NotInheritedSingleNotDecorated>.Value;
		
		Assert.That(attr, Is.Null,
			"since Inherited = false, the attribute is not matched, despite inherit: true.");
	}
	
	[Test]
	public void ForExactValue_NotInheritedSingleBase_AttrInSelf()
	{
		NotInheritedSingleAttribute? attr = Cache<NotInheritedSingleAttribute>.ForExact<NotInheritedSingleBase>.Value;
		
		Assert.That(attr, Is.InstanceOf<NotInheritedSingleAttribute>()
			.With.Property(nameof(NotInheritedSingleAttribute.Subject)).EqualTo("base"));
	}
	
	[Test]
	public void ForExactValue_NotInheritedSingleDerived_AttrInSelf()
	{
		NotInheritedSingleAttribute? attr = Cache<NotInheritedSingleAttribute>.ForExact<NotInheritedSingleDerived>.Value;
		
		Assert.That(attr, Is.InstanceOf<NotInheritedSingleAttribute>()
			.With.Property(nameof(NotInheritedSingleAttribute.Subject)).EqualTo("derived"));
	}
	
	[Test]
	public void ForExactValue_NotInheritedSingleNotDecorated_AttrNotFound()
	{
		NotInheritedSingleAttribute? attr = Cache<NotInheritedSingleAttribute>.ForExact<NotInheritedSingleNotDecorated>.Value;
		
		Assert.That(attr, Is.Null);
	}
	
	#endregion
	
	#region NotInheritedMultiAttribute
	
	[Test]
	public void ForValue_NotInheritedMultiBase_AttrInSelf()
	{
		// works because only one attribute matched
		NotInheritedMultiAttribute? attr = Cache<NotInheritedMultiAttribute>.For<NotInheritedMultiBase>.Value;
		
		Assert.That(attr, Is.InstanceOf<NotInheritedMultiAttribute>()
			.With.Property(nameof(NotInheritedMultiAttribute.Subject)).EqualTo("base"));
	}
	
	[Test]
	public void ForValue_NotInheritedMultiDerived_AttrInSelf()
	{
		// Inherited = false, so inherit: true only sees the single attribute on the derived type itself.
		NotInheritedMultiAttribute? attr = Cache<NotInheritedMultiAttribute>.For<NotInheritedMultiDerived>.Value;
		
		Assert.That(attr, Is.InstanceOf<NotInheritedMultiAttribute>()
			.With.Property(nameof(NotInheritedMultiAttribute.Subject)).EqualTo("derived"));
	}
	
	[Test]
	public void ForValue_NotInheritedMultiNotDecorated_AttrNotFound()
	{
		// Inherited = false, so the base attribute is not matched, despite inherit: true.
		NotInheritedMultiAttribute? attr = Cache<NotInheritedMultiAttribute>.For<NotInheritedMultiNotDecorated>.Value;
		
		Assert.That(attr, Is.Null);
	}
	
	[Test]
	public void ForValue_NotInheritedMulti_ThrowsBecauseAmbiguous()
	{
		// Multiple attributes on same type cause GetCustomAttribute to throw AmbiguousMatchException,
		// which surfaces wrapped in TypeInitializationException through the static field initializer.
		Assert.That(() => Cache<NotInheritedMultiAttribute>.For<NotInheritedMulti>.Value,
			Throws.InstanceOf<TypeInitializationException>()
				.With.InnerException.InstanceOf<AmbiguousMatchException>());
	}
	
	[Test]
	public void ForExactValue_NotInheritedMultiBase_AttrInSelf()
	{
		NotInheritedMultiAttribute? attr = Cache<NotInheritedMultiAttribute>.ForExact<NotInheritedMultiBase>.Value;
		
		Assert.That(attr, Is.InstanceOf<NotInheritedMultiAttribute>()
			.With.Property(nameof(NotInheritedMultiAttribute.Subject)).EqualTo("base"));
	}
	
	[Test]
	public void ForExactValue_NotInheritedMultiDerived_AttrInSelf()
	{
		NotInheritedMultiAttribute? attr = Cache<NotInheritedMultiAttribute>.ForExact<NotInheritedMultiDerived>.Value;
		
		Assert.That(attr, Is.InstanceOf<NotInheritedMultiAttribute>()
			.With.Property(nameof(NotInheritedMultiAttribute.Subject)).EqualTo("derived"));
	}
	
	[Test]
	public void ForExactValue_NotInheritedMultiNotDecorated_AttrNotFound()
	{
		NotInheritedMultiAttribute? attr = Cache<NotInheritedMultiAttribute>.ForExact<NotInheritedMultiNotDecorated>.Value;
		
		Assert.That(attr, Is.Null);
	}
	
	[Test]
	public void ForExactValue_NotInheritedMulti_ThrowsBecauseAmbiguous()
	{
		Assert.That(() => Cache<NotInheritedMultiAttribute>.ForExact<NotInheritedMulti>.Value,
			Throws.InstanceOf<TypeInitializationException>()
				.With.InnerException.InstanceOf<AmbiguousMatchException>());
	}
	
	#endregion
	
	#region InheritedMultiAttribute
	
	[Test]
	public void ForValue_InheritedMultiBase_AttrInSelf()
	{
		InheritedMultiAttribute? attr = Cache<InheritedMultiAttribute>.For<InheritedMultiBase>.Value;
		
		Assert.That(attr, Is.InstanceOf<InheritedMultiAttribute>()
			.With.Property(nameof(InheritedMultiAttribute.Subject)).EqualTo("base"));
	}
	
	[Test]
	public void ForValue_InheritedMultiDerived_ThrowsBecauseAmbiguous()
	{
		// Inherited = true and inherit: true see both the base and the derived attribute, making the call ambiguous.
		Assert.That(() => Cache<InheritedMultiAttribute>.For<InheritedMultiDerived>.Value,
			Throws.InstanceOf<TypeInitializationException>()
				.With.InnerException.InstanceOf<AmbiguousMatchException>());
	}
	
	[Test]
	public void ForValue_InheritedMultiNotDecorated_AttrInBase()
	{
		// Inherited = true, so the base attribute is inherited by the undecorated derived type.
		InheritedMultiAttribute? attr = Cache<InheritedMultiAttribute>.For<InheritedMultiNotDecorated>.Value;
		
		Assert.That(attr, Is.InstanceOf<InheritedMultiAttribute>()
			.With.Property(nameof(InheritedMultiAttribute.Subject)).EqualTo("base"));
	}
	
	[Test]
	public void ForValue_InheritedMulti_ThrowsBecauseAmbiguous()
	{
		Assert.That(() => Cache<InheritedMultiAttribute>.For<InheritedMulti>.Value,
			Throws.InstanceOf<TypeInitializationException>()
				.With.InnerException.InstanceOf<AmbiguousMatchException>());
	}
	
	[Test]
	public void ForExactValue_InheritedMultiBase_AttrInSelf()
	{
		InheritedMultiAttribute? attr = Cache<InheritedMultiAttribute>.ForExact<InheritedMultiBase>.Value;
		
		Assert.That(attr, Is.InstanceOf<InheritedMultiAttribute>()
			.With.Property(nameof(InheritedMultiAttribute.Subject)).EqualTo("base"));
	}
	
	[Test]
	public void ForExactValue_InheritedMultiDerived_AttrInSelf()
	{
		InheritedMultiAttribute? attr = Cache<InheritedMultiAttribute>.ForExact<InheritedMultiDerived>.Value;
		
		Assert.That(attr, Is.InstanceOf<InheritedMultiAttribute>()
			.With.Property(nameof(InheritedMultiAttribute.Subject)).EqualTo("derived"));
	}
	
	[Test]
	public void ForExactValue_InheritedMultiNotDecorated_AttrNotFound()
	{
		InheritedMultiAttribute? attr = Cache<InheritedMultiAttribute>.ForExact<InheritedMultiNotDecorated>.Value;
		
		Assert.That(attr, Is.Null);
	}
	
	[Test]
	public void ForExactValue_InheritedMulti_ThrowsBecauseAmbiguous()
	{
		Assert.That(() => Cache<InheritedMultiAttribute>.ForExact<InheritedMulti>.Value,
			Throws.InstanceOf<TypeInitializationException>()
				.With.InnerException.InstanceOf<AmbiguousMatchException>());
	}
	
	#endregion
	
	[Test]
	public void Value_NotDecorated_AttrNotFound()
	{
		Assert.That(typeof(NotDecorated).CustomAttributes, Is.Empty, "sanity check");

		Assert.Multiple(() =>
		{
			Assert.That(Cache<InheritedSingleAttribute>.For<NotDecorated>.Value, Is.Null);
			Assert.That(Cache<InheritedSingleAttribute>.ForExact<NotDecorated>.Value, Is.Null);
			Assert.That(Cache<NotInheritedSingleAttribute>.For<NotDecorated>.Value, Is.Null);
			Assert.That(Cache<NotInheritedSingleAttribute>.ForExact<NotDecorated>.Value, Is.Null);
			Assert.That(Cache<NotInheritedMultiAttribute>.For<NotDecorated>.Value, Is.Null);
			Assert.That(Cache<NotInheritedMultiAttribute>.ForExact<NotDecorated>.Value, Is.Null);
			Assert.That(Cache<InheritedMultiAttribute>.For<NotDecorated>.Value, Is.Null);
			Assert.That(Cache<InheritedMultiAttribute>.ForExact<NotDecorated>.Value, Is.Null);
		});
	}
}