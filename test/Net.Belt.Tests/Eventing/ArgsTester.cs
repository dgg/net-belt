using System.ComponentModel;

using Net.Belt.Eventing;

namespace Net.Belt.Tests.Eventing;

[TestFixture]
public class ArgsTester
{
	[Test]
	public void Ctor_ValueArgs_EqualToFactoryCreated()
	{
		IValueArgs<int> args = new ValueArgs<int>(1);
		Assert.That(args, Is.EqualTo(Args.Value(1)));
	}

	[Test]
	public void Ctor_MultiArgs_EqualToFactoryCreated()
	{
		IMultiArgs<int, bool> args = new MultiArgs<int, bool>(1, true);
		Assert.That(args, Is.EqualTo(Args.Value(1, true)));
	}

	[Test]
	public void Ctor_MutableArgs_EqualToFactoryCreated()
	{
		IMutableArgs<decimal> args = new MutableArgs<decimal>(1m);
		args.Value = 2m;
		Assert.That(args, Is.EqualTo(Args.Mutable(2m)).UsingPropertiesComparer(),
			"using props comparer as it is not a record");
	}

	[Test]
	public void Ctor_ValueChangedArgs_EqualToFactoryCreated()
	{
		PropertyChangedEventArgs args = new PropertyValueChangedEventArgs<int>("Prop", 2, 3);
		Assert.That(args, Is.EqualTo(Args.Changed("Prop", 2, 3)).UsingPropertiesComparer());
	}

	[Test]
	public void Ctor_ValueChangingArgs_EqualToFactoryCreated()
	{
		PropertyChangingEventArgs args = new PropertyValueChangingEventArgs<int>("Prop", 2, 3);
		Assert.That(args, Is.EqualTo(Args.Changing("Prop", 2, 3)).UsingPropertiesComparer());
	}

	[Test]
	public void Ctor_IndexChangedArgs_EqualToFactoryCreated()
	{
		var args = new ValueIndexChangedArgs<string>(0, "new", "old");
		Assert.That(args, Is.EqualTo(Args.Changed(0, "old", "new")));
	}

	[Test]
	public void Ctor_IndexChangingArgs_EqualToFactoryCreated()
	{
		var args = new ValueIndexChangingArgs<string>(0, "old", "new");
		Assert.That(args, Is.EqualTo(Args.Changing(0, "old", "new")).UsingPropertiesComparer());
	}
	
	[Test]
	public void Ctor_IndexCancelArgs_EqualToFactoryCreated()
	{
		ICancelArgs args = new ValueIndexCancelArgs<char>(2, 'b');
		Assert.That(args, Is.EqualTo(Args.Cancel(2, 'b')).UsingPropertiesComparer());
	}
	
	[Test]
	public void Ctor_IndexArgs_EqualToFactoryCreated()
	{
		IIndexArgs args = new ValueIndexArgs<char>(2, 'b');
		Assert.That(args, Is.EqualTo(Args.Index(2, 'b')));
	}
}