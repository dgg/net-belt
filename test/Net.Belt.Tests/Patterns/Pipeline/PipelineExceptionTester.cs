using Net.Belt.Patterns.Pipeline;

namespace Net.Belt.Tests.Patterns.Pipelining;

[TestFixture]
public class PipelineExceptionTester
{
	[Test]
	public void Ctor_IndexAndMessage()
	{
		var subject = new PipelineException(1, new Exception("from second step"));

		Assert.That(subject.Message, Does.Contain("[1]"));
		Assert.That(subject.StepIndex, Is.EqualTo(1));
	}
	
	[Test]
	public void Ctor_InnerSet()
	{
		var inner = new Exception("from second step");
		var subject = new PipelineException(1, inner);

		Assert.That(subject.InnerException, Is.SameAs(inner));
	}
	
	[Test]
	public void AddRollback_ExceptionsInThirdAndFirstRollbackSteps_ExceptionsAddedInReverseOrder()
	{
		var stepException = new Exception("from fifth step");
		var subject = new PipelineException(4, stepException);
        
		// rollback index 3
		// failure index 2
		var index2 = new Exception("idx 2");
		subject.AddRollbackException(2, index2);
		// rollback index 1
		// failure index 0
		var index0 = new Exception("idx 0");
		subject.AddRollbackException(new StepRollbackException(0, index0));

		Assert.That(subject.InnerException, Is.SameAs(stepException));
        
		Assert.Multiple(() =>
		{
			Assert.That(subject.RollbackExceptions, Has.Count.EqualTo(2));
            
			Assert.That(subject.RollbackExceptions[0].StepIndex, Is.EqualTo(2));
			Assert.That(subject.RollbackExceptions[0], Has.InnerException.SameAs(index2));
            
			Assert.That(subject.RollbackExceptions[1].StepIndex, Is.EqualTo(0));
			Assert.That(subject.RollbackExceptions[1], Has.InnerException.SameAs(index0));
		});
	}
}