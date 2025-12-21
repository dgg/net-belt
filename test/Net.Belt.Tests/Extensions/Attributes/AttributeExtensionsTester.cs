using System.Diagnostics.CodeAnalysis;

using Net.Belt.Extensions.Attributes;
using Net.Belt.Tests.Extensions.Attributes.Support;

namespace Net.Belt.Tests.Extensions.Attributes;

[TestFixture]
[SuppressMessage("ReSharper", "RedundantArgumentDefaultValue")]
public class AttributeExtensionsTester
{
	#region HasAttribute

	[Test]
	public void HasAttribute_InstanceDecorated_True() =>
		Assert.That(this.HasAttribute<TestFixtureAttribute>(), Is.True);

	[Test]
	public void HasAttribute_InstanceNotDecorated_False() =>
		Assert.That(this.HasAttribute<DescriptionAttribute>(), Is.False);

	[Test]
	public void HasAttribute_ParentDecoratedNoInherit_False()
	{
		var inheritor = new ParentDecoratedWithCategoryAndDescription();
		Assert.That(inheritor.HasAttribute<CategoryAttribute>(), Is.False);
		Assert.That(inheritor.HasAttribute<CategoryAttribute>(false), Is.False);
	}

	[Test]
	public void HasAttribute_ParentDecoratedInherit_True()
	{
		var inheritor = new ParentDecoratedWithCategoryAndDescription();
		Assert.That(inheritor.HasAttribute<CategoryAttribute>(true), Is.True);
	}

	[Test]
	public void HasAttribute_ParentDecoratedWithNonInheritable_False()
	{
		var inheritor = new ParentDecoratedWithCategoryAndDescription();
		Assert.That(inheritor.HasAttribute<DescriptionAttribute>(true), Is.False);
	}

	[Test]
	public void HasAttributeOnInstace_ParentNotDecorated_False()
	{
		var inheritor = new ParentDecoratedWithCategoryAndDescription();
		Assert.That(inheritor.HasAttribute<TestAttribute>(true), Is.False);
	}

	#endregion

	#region GetAttribute

	[Test]
	public void GetAttribute_Decorated_Instance() =>
		Assert.That(this.GetAttribute<TestFixtureAttribute>(), Is.InstanceOf<TestFixtureAttribute>());

	[Test]
	public void GetAttribute_NotDecorated_Exception() =>
		Assert.That(() => this.GetAttribute<DescriptionAttribute>(), Throws.InvalidOperationException);

	[Test]
	public void GetAttribute_ParentDecoratedNoInherit_Exception()
	{
		var inheritor = new ParentDecoratedWithCategoryAndDescription();
		Assert.That(() => inheritor.GetAttribute<CategoryAttribute>(), Throws.InvalidOperationException);
		Assert.That(() => inheritor.GetAttribute<CategoryAttribute>(false), Throws.InvalidOperationException);
	}

	[Test]
	public void GetAttribute_ParentDecoratedInherit_Instance()
	{
		var inheritor = new ParentDecoratedWithCategoryAndDescription();
		Assert.That(inheritor.GetAttribute<CategoryAttribute>(true), Is.InstanceOf<CategoryAttribute>()
			.With.Property("Name").EqualTo("cat"));
	}

	[Test]
	public void GetAttribute_ParentDecoratedWithNonInheritable_Exception()
	{
		var inheritor = new ParentDecoratedWithCategoryAndDescription();
		Assert.That(() => inheritor.GetAttribute<DescriptionAttribute>(true), Throws.InvalidOperationException);
	}

	[Test]
	public void GetAttribute_ParentNotDecorated_Exception()
	{
		var inheritor = new ParentDecoratedWithCategoryAndDescription();
		Assert.That(() => inheritor.GetAttribute<TestAttribute>(true), Throws.InvalidOperationException);
	}

	#endregion

	#region GetAttribute

	[Test]
	public void TryGetAttribute_Decorated_True()
	{
		Assert.That(this.TryGetAttribute<TestFixtureAttribute>(out var attr), Is.True);
		Assert.That(attr, Is.InstanceOf<TestFixtureAttribute>());
		Assert.That(typeof(AttributeExtensionsTester).TryGetAttribute<TestFixtureAttribute>(out attr), Is.True);
	}

	[Test]
	public void TryGetAttribute_NotDecorated_False()
	{
		Assert.That(this.TryGetAttribute<DescriptionAttribute>(out var attr), Is.False);
		Assert.That(attr, Is.Null);
		Assert.That(typeof(AttributeExtensionsTester).TryGetAttribute(out attr), Is.False);
	}

	[Test]
	public void TryGetAttribute_ParentDecoratedNoInherit_False()
	{
		var inheritor = new ParentDecoratedWithCategoryAndDescription();
		Assert.That(inheritor.TryGetAttribute<CategoryAttribute>(out _), Is.False);
		Assert.That(inheritor.TryGetAttribute<CategoryAttribute>(out _, false), Is.False);
		Assert.That(typeof(ParentDecoratedWithCategoryAndDescription).TryGetAttribute<CategoryAttribute>(out _), Is.False);
	}

	[Test]
	public void TryGetAttribute_ParentDecoratedInherit_True()
	{
		var inheritor = new ParentDecoratedWithCategoryAndDescription();
		Assert.That(inheritor.TryGetAttribute<CategoryAttribute>(out var attr, true), Is.True);
		Assert.That(attr, Is.InstanceOf<CategoryAttribute>()
			.With.Property("Name").EqualTo("cat"));
		Assert.That(typeof(ParentDecoratedWithCategoryAndDescription).TryGetAttribute(out attr, true),
			Is.True);
	}

	[Test]
	public void TryGetAttribute_ParentDecoratedWithNonInheritable_False()
	{
		var inheritor = new ParentDecoratedWithCategoryAndDescription();
		Assert.That(inheritor.TryGetAttribute<DescriptionAttribute>(out _, true), Is.False);
		Assert.That(
			typeof(ParentDecoratedWithCategoryAndDescription).TryGetAttribute<DescriptionAttribute>(out _, true),
			Is.False);
	}

	[Test]
	public void TryGetAttribute_ParentNotDecorated_False()
	{
		var inheritor = new ParentDecoratedWithCategoryAndDescription();
		Assert.That(inheritor.TryGetAttribute<TestAttribute>(out _, true), Is.False);
		Assert.That(typeof(ParentDecoratedWithCategoryAndDescription).TryGetAttribute<TestAttribute>(out _, true),
			Is.False);
	}

	#endregion
	
	#region GetAttributes

	[Test]
	public void GetAttributes_Decorated_Instance() =>
		Assert.That(this.GetAttributes<TestFixtureAttribute>(), Has.Length.EqualTo(1)
			.With.All.InstanceOf<TestFixtureAttribute>());
	
	[Test]
	public void GetAttributes_NotDecorated_Empty() =>
		Assert.That(this.GetAttributes<CategoryAttribute>(), Is.Empty);
	
	[Test]
	public void GetAttributes_MultipleDecorated_Instances()
	{
		var multi = new DecoratedMultipleTimes();
		Assert.That(multi.GetAttributes<MultiAttribute>(), Has.Length.EqualTo(3).And
			.Some.Matches<MultiAttribute>(a => a.Positional.Equals("a")).And
			.Some.Matches<MultiAttribute>(a => a.Positional.Equals("b")).And
			.Some.Matches<MultiAttribute>(a => a.Positional.Equals("c")), 
			"order is not guaranteed"
		);
	}

	#endregion
}