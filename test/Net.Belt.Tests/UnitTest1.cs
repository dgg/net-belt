namespace Net.Belt.Tests;

public class Tests
{
	[SetUp]
	public void Setup()
	{
	}

	[Test]
	public void Test1()
	{
		Assert.Fail("boom");
	}

	[Test]
	public void Test2()
	{
		var subject = new Class1(3);
		string five = subject.MultiMethod(2);

		Assert.That(five, Is.EqualTo("5"));
	}
}
