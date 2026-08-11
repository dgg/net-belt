/*using System.Reflection;

using Net.Belt.Reflection;
using Net.Belt.Tests.Reflection.Support;

namespace Net.Belt.Tests.Reflection;

[TestFixture]
public class AttributeCacheTester
{
	#region InheritedSingleAttribute
	
	[Test]
	public void SingleAttribute_InheritedSingleBase_AttrInSelf()
	{
		InheritedSingleAttribute? attr = AttributeCache<InheritedSingleBase>
			.Single<InheritedSingleAttribute>
			.Attribute;
		
		Assert.That(attr, Is.InstanceOf<InheritedSingleAttribute>()
			.With.Property(nameof(InheritedSingleAttribute.Subject)).EqualTo("base"));
	}
	
	[Test]
	public void SingleAttribute_InheritedSingleDerived_AttrInSelf()
	{
		InheritedSingleAttribute? attr = AttributeCache<InheritedSingleDerived>
			.Single<InheritedSingleAttribute>
			.Attribute;
		
		Assert.That(attr, Is.InstanceOf<InheritedSingleAttribute>()
			.With.Property(nameof(InheritedSingleAttribute.Subject)).EqualTo("derived"));
	}
	
	[Test]
	public void SingleInherited_InheritedSingleBase_AttrInSelf()
	{
		InheritedSingleAttribute? attr = AttributeCache<InheritedSingleBase>
			.Single<InheritedSingleAttribute>
			.Inherited;
		
		Assert.That(attr, Is.InstanceOf<InheritedSingleAttribute>()
			.With.Property(nameof(InheritedSingleAttribute.Subject)).EqualTo("base"));
	}
	
	[Test]
	public void SingleInherited_InheritedSingleDerived_AttrInSelf()
	{
		InheritedSingleAttribute? attr = AttributeCache<InheritedSingleDerived>
			.Single<InheritedSingleAttribute>
			.Inherited;
		
		Assert.That(attr, Is.InstanceOf<InheritedSingleAttribute>()
			.With.Property(nameof(InheritedSingleAttribute.Subject)).EqualTo("derived"));
	}
	
	[Test]
	public void MultiAttributes_InheritedSingleBase_AttrInSelf()
	{
		InheritedSingleAttribute[] attr = AttributeCache<InheritedSingleBase>
			.Multi<InheritedSingleAttribute>
			.Attributes;
		
		Assert.That(attr, Has.Length.EqualTo(1));
		Assert.That(attr[0], Is.InstanceOf<InheritedSingleAttribute>()
			.With.Property(nameof(InheritedSingleAttribute.Subject)).EqualTo("base"));
	}
	
	[Test]
	public void MultiAttributes_InheritedSingleDerived_AttrInSelf()
	{
		InheritedSingleAttribute[] attr = AttributeCache<InheritedSingleDerived>
			.Multi<InheritedSingleAttribute>
			.Attributes;
		
		Assert.That(attr, Has.Length.EqualTo(1));
		Assert.That(attr[0], Is.InstanceOf<InheritedSingleAttribute>()
			.With.Property(nameof(InheritedSingleAttribute.Subject)).EqualTo("derived"));
	}
	
	[Test]
	public void MultiInherited_InheritedSingleBase_AttrInSelf()
	{
		InheritedSingleAttribute[] attr = AttributeCache<InheritedSingleBase>
			.Multi<InheritedSingleAttribute>
			.Inherited;
		
		Assert.That(attr, Has.Length.EqualTo(1));
		Assert.That(attr[0], Is.InstanceOf<InheritedSingleAttribute>()
			.With.Property(nameof(InheritedSingleAttribute.Subject)).EqualTo("base"));
	}
	
	[Test]
	public void MultiInherited_InheritedSingleDerived_AttrInSelf()
	{
		InheritedSingleAttribute[] attr = AttributeCache<InheritedSingleDerived>
			.Multi<InheritedSingleAttribute>
			.Inherited;
		
		Assert.That(attr, Has.Length.EqualTo(1));
		Assert.That(attr[0], Is.InstanceOf<InheritedSingleAttribute>()
			.With.Property(nameof(InheritedSingleAttribute.Subject)).EqualTo("derived"));
	}
	
	#endregion
	
	#region NotInheritedSingleAttribute
	
	[Test]
	public void SingleAttribute_NotInheritedSingleBase_AttrInSelf()
	{
		NotInheritedSingleAttribute? attr = AttributeCache<NotInheritedSingleBase>
			.Single<NotInheritedSingleAttribute>
			.Attribute;
		
		Assert.That(attr, Is.InstanceOf<NotInheritedSingleAttribute>()
			.With.Property(nameof(NotInheritedSingleAttribute.Subject)).EqualTo("base"));
	}
	
	[Test]
	public void SingleAttribute_NotInheritedSingleNotDecorated_AttrNotFound()
	{
		NotInheritedSingleAttribute? attr = AttributeCache<NotInheritedSingleNotDecorated>
			.Single<NotInheritedSingleAttribute>
			.Attribute;
		
		Assert.That(attr, Is.Null);
	}
	
	[Test]
	public void SingleAttribute_NotInheritedSingleDerived_AttrInSelf()
	{
		NotInheritedSingleAttribute? attr = AttributeCache<NotInheritedSingleDerived>
			.Single<NotInheritedSingleAttribute>
			.Attribute;
		
		Assert.That(attr, Is.InstanceOf<NotInheritedSingleAttribute>()
			.With.Property(nameof(NotInheritedSingleAttribute.Subject)).EqualTo("derived"));
	}
	
	[Test]
	public void SingleInherited_NotInheritedSingleBase_AttrInSelf()
	{
		NotInheritedSingleAttribute? attr = AttributeCache<NotInheritedSingleBase>
			.Single<NotInheritedSingleAttribute>
			.Inherited;
		
		Assert.That(attr, Is.InstanceOf<NotInheritedSingleAttribute>()
			.With.Property(nameof(NotInheritedSingleAttribute.Subject)).EqualTo("base"));
	}
	
	[Test]
	public void SingleInherited_NotInheritedSingleNotDecorated_AttrNotFound()
	{
		NotInheritedSingleAttribute? attr = AttributeCache<NotInheritedSingleNotDecorated>
			.Single<NotInheritedSingleAttribute>
			.Inherited;
		
		Assert.That(attr, Is.Null);
	}
	
	[Test]
	public void SingleInherited_NotInheritedSingleDerived_AttrInSelf()
	{
		NotInheritedSingleAttribute? attr = AttributeCache<NotInheritedSingleDerived>
			.Single<NotInheritedSingleAttribute>
			.Inherited;
		
		Assert.That(attr, Is.InstanceOf<NotInheritedSingleAttribute>()
			.With.Property(nameof(NotInheritedSingleAttribute.Subject)).EqualTo("derived"));
	}
	
	[Test]
	public void SingleAttribute_NotInheritedSingleBase_AttrInSelf()
	{
		NotInheritedSingleAttribute? attr = AttributeCache<NotInheritedSingleBase>
			.Single<NotInheritedSingleAttribute>
			.Attribute;
		
		Assert.That(attr, Is.InstanceOf<NotInheritedSingleAttribute>()
			.With.Property(nameof(NotInheritedSingleAttribute.Subject)).EqualTo("base"));
	}
	
	[Test]
	public void SingleAttribute_NotInheritedSingleNotDecorated_AttrNotFound()
	{
		NotInheritedSingleAttribute? attr = AttributeCache<NotInheritedSingleNotDecorated>
			.Single<NotInheritedSingleAttribute>
			.Attribute;
		
		Assert.That(attr, Is.Null);
	}
	
	[Test]
	public void SingleAttribute_NotInheritedSingleDerived_AttrInSelf()
	{
		NotInheritedSingleAttribute? attr = AttributeCache<NotInheritedSingleDerived>
			.Single<NotInheritedSingleAttribute>
			.Attribute;
		
		Assert.That(attr, Is.InstanceOf<NotInheritedSingleAttribute>()
			.With.Property(nameof(NotInheritedSingleAttribute.Subject)).EqualTo("derived"));
	}
	
	[Test]
	public void MultiInherited_NotInheritedSingleBase_AttrInSelf()
	{
		NotInheritedSingleAttribute[] attr = AttributeCache<NotInheritedSingleBase>
			.Multi<NotInheritedSingleAttribute>
			.Inherited;
		
		Assert.That(attr, Is.InstanceOf<NotInheritedSingleAttribute>()
			.With.Property(nameof(NotInheritedSingleAttribute.Subject)).EqualTo("base"));
	}
	
	[Test]
	public void MultiInherited_NotInheritedSingleNotDecorated_AttrNotFound()
	{
		NotInheritedSingleAttribute[] attr = AttributeCache<NotInheritedSingleNotDecorated>
			.Multi<NotInheritedSingleAttribute>
			.Inherited;
		
		Assert.That(attr, Is.Null);
	}
	
	[Test]
	public void MultiInherited_NotInheritedSingleDerived_AttrInSelf()
	{
		NotInheritedSingleAttribute[] attr = AttributeCache<NotInheritedSingleDerived>
			.Multi<NotInheritedSingleAttribute>
			.Inherited;
		
		Assert.That(attr, Is.InstanceOf<NotInheritedSingleAttribute>()
			.With.Property(nameof(NotInheritedSingleAttribute.Subject)).EqualTo("derived"));
	}
	
	#endregion
	
	#region NotInheritedMultiAttribute
	
	[Test]
	public void Attribute_NotInheritedMultiBase_AttrInSelf()
	{
		NotInheritedMultiAttribute? attr = AttributeCache<NotInheritedMultiBase>
			.Single<NotInheritedMultiAttribute>
			.Attribute;
		
		Assert.That(attr, Is.InstanceOf<NotInheritedMultiAttribute>()
			.With.Property(nameof(NotInheritedMultiAttribute.Subject)).EqualTo("base"));
	}
	
	[Test]
	public void Attribute_NotInheritedMultiDerived_Exception()
	{
		Assert.That(() => AttributeCache<NotInheritedMultiDerived>
			.Single<NotInheritedMultiAttribute>
			.Attribute, Throws.InstanceOf<TypeInitializationException>()
			.With.InnerException.InstanceOf<AmbiguousMatchException>());
	}
	
	[Test]
	public void Attribute_NotInheritedMulti_Exception()
	{
		Assert.That(()=> AttributeCache<NotInheritedMulti>
			.Single<NotInheritedMultiAttribute>
			.Attribute, Throws.InstanceOf<TypeInitializationException>()
			.With.InnerException.InstanceOf<AmbiguousMatchException>());
	}
	
	[Test]
	public void Inherited_NotInheritedMultiBase_AttrInSelf()
	{
		NotInheritedMultiAttribute? attr = AttributeCache<NotInheritedMultiBase>
			.Single<NotInheritedMultiAttribute>
			.Inherited;
		
		Assert.That(attr, Is.InstanceOf<NotInheritedMultiAttribute>()
			.With.Property(nameof(NotInheritedMultiAttribute.Subject)).EqualTo("base"));
	}
	
	[Test]
	public void Inherited_NotInheritedMultiDerived_Exception()
	{
		Assert.That(() => AttributeCache<NotInheritedMultiDerived>
			.Single<NotInheritedMultiAttribute>
			.Inherited, Throws.InstanceOf<TypeInitializationException>()
			.With.InnerException.InstanceOf<AmbiguousMatchException>());
	}
	
	[Test]
	public void Inherited_NotInheritedMulti_Exception()
	{
		Assert.That(()=> AttributeCache<NotInheritedMulti>
			.Single<NotInheritedMultiAttribute>
			.Inherited, Throws.Nothing);
	}
	
	#endregion
}
*/