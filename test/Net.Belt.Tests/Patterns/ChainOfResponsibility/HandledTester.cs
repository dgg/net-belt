using Net.Belt.Patterns.ChainOfResponsibility;

namespace Net.Belt.Tests.Patterns.ChainOfResponsibility;

[TestFixture]
public class HandledTester
{
	[Test]
	public void Handled_Struct_DefaultWhenUnhandled()
	{
		Handled<int> handled = new(2);
		Assert.That(handled.IsHandled, Is.True);
		Assert.That(handled.Context, Is.EqualTo(2));
		
		Handled<int> notHandled = default;
		Assert.That(notHandled.IsHandled, Is.False);
		Assert.That(notHandled.Context, Is.EqualTo(0));
		
		notHandled = new();
		Assert.That(notHandled.IsHandled, Is.False);
		Assert.That(notHandled.Context, Is.EqualTo(0));
		
		notHandled = Handled<int>.Not;
		Assert.That(notHandled.IsHandled, Is.False);
		Assert.That(notHandled.Context, Is.EqualTo(0));
	}

	[Test]
	public void Handled_NullableStruct_NullWhenUnhandled()
	{
		Handled<int?> handled = new(2);
		Assert.That(handled.IsHandled, Is.True);
		Assert.That(handled.Context, Is.EqualTo(2));
		
		Handled<int?> notHandled = default;
		Assert.That(notHandled.IsHandled, Is.False);
		Assert.That(notHandled.Context, Is.Null);
		
		notHandled = new();
		Assert.That(notHandled.IsHandled, Is.False);
		Assert.That(notHandled.Context, Is.Null);
		
		notHandled = Handled<int?>.Not;
		Assert.That(notHandled.IsHandled, Is.False);
		Assert.That(notHandled.Context, Is.Null);
	}
	
	[Test]
	public void Handled_NonNullableReference_NullWhenUnhandled()
	{
		var context = new object();
		
		Handled<object> handled = new(context);
		Assert.That(handled.IsHandled, Is.True);
		Assert.That(handled.Context, Is.SameAs(context));
		
		Handled<object> notHandled = default;
		Assert.That(notHandled.IsHandled, Is.False);
		Assert.That(notHandled.Context, Is.Null);
		
		notHandled = new();
		Assert.That(notHandled.IsHandled, Is.False);
		Assert.That(notHandled.Context, Is.Null);
		
		notHandled = Handled<object>.Not;
		Assert.That(notHandled.IsHandled, Is.False);
		Assert.That(notHandled.Context, Is.Null);
	}

	[Test]
	public void Handled_NullableReference_NullWhenUnhandled()
	{
		object? context = new object();
		
		Handled<object?> handled = new(context);
		Assert.That(handled.IsHandled, Is.True);
		Assert.That(handled.Context, Is.SameAs(context));
		
		Handled<object?> notHandled = default;
		Assert.That(notHandled.IsHandled, Is.False);
		Assert.That(notHandled.Context, Is.Null);
		
		notHandled = new();
		Assert.That(notHandled.IsHandled, Is.False);
		Assert.That(notHandled.Context, Is.Null);
		
		notHandled = Handled<object?>.Not;
		Assert.That(notHandled.IsHandled, Is.False);
		Assert.That(notHandled.Context, Is.Null);
	}
}