using Net.Belt.Patterns.Pipeline;
using Net.Belt.Tests.Patterns.Pipelining.Support;

namespace Net.Belt.Tests.Patterns.Pipelining;

[TestFixture]
public class PipelineRunnerTester
{
	#region construction

	[Test]
	public void ParamsCtor_NoArgs_EmptySteps()
	{
		var subject = new PipelineRunner<AContext>();

		Assert.That(subject.Steps, Is.Empty);
	}

	[Test]
	public void ParamsCtor_SomeArgs_NotEmpty()
	{
		var subject = new PipelineRunner<AContext>(new OkStep());
		Assert.That(subject.Steps, Is.Not.Empty);
	}

	[Test]
	public void IEnumerableCtor_Empty_EmptySteps()
	{
		var subject = new PipelineRunner<AContext>(Enumerable.Empty<IPipelineStep<AContext>>());

		Assert.That(subject.Steps, Is.Empty);
	}

	[Test]
	public void IEnumerableCtor_NonEmpty_EmptySteps()
	{
		IEnumerable<IPipelineStep<AContext>> steps = [new OkStep()];
		var subject = new PipelineRunner<AContext>(steps);

		Assert.That(subject.Steps, Is.Not.Empty);
	}

	#endregion

	#region execution

	[Test]
	public async Task Execute_OkSteps_AllExecuted()
	{
		var index0 = new OkStep();
		var index1 = new OkStep();
		var index2 = new OkStep();

		var subject = new PipelineRunner<AContext>(index0, index1, index2);

		var context = new AContext("prop");
		await subject.ExecuteAsync(context, CancellationToken.None);

		Assert.Multiple(() =>
		{
			Assert.That(index0, Has.Property(nameof(OkStep.Executed)).True.And
				.Property(nameof(OkStep.Rollbacked)).False);

			Assert.That(index1, Has.Property(nameof(OkStep.Executed)).True.And
				.Property(nameof(OkStep.Rollbacked)).False);

			Assert.That(index2, Has.Property(nameof(OkStep.Executed)).True.And
				.Property(nameof(OkStep.Rollbacked)).False);
		});
	}

	[Test]
	public void Execute_IntermediateFailure_SomeNotExecutedAndPreviousRollbacked()
	{
		var index0 = new OkStep();
		var index1 = new OkStep();
		var index2 = new CannotExecuteStep("three");
		var index3 = new OkStep();

		var subject = new PipelineRunner<AContext>(index0, index1, index2, index3);

		var context = new AContext("prop");

		var exception =
			Assert.ThrowsAsync<PipelineException>(() => subject.ExecuteAsync(context, CancellationToken.None));

		Assert.Multiple(() =>
		{
			Assert.That(exception.StepIndex, Is.EqualTo(2));
			Assert.That(exception.Message, Does.Contain("[2]"));
			Assert.That(exception.InnerException, Has.Message.EqualTo("three"));
			Assert.That(exception.RollbackExceptions, Is.Empty);
		});

		Assert.Multiple(() =>
		{
			// index 2 and previous executed and rollbacked
			Assert.That(index0, Has.Property(nameof(OkStep.Executed)).True.And
				.Property(nameof(OkStep.Rollbacked)).True);

			Assert.That(index1, Has.Property(nameof(OkStep.Executed)).True.And
				.Property(nameof(OkStep.Rollbacked)).True);

			Assert.That(index2, Has.Property(nameof(OkStep.Executed)).True.And
				.Property(nameof(OkStep.Rollbacked)).True);

			// we did not get to index 3
			Assert.That(index3, Has.Property(nameof(OkStep.Executed)).False.And
				.Property(nameof(OkStep.Rollbacked)).False);
		});
	}

	[Test]
	public void Execute_IntermediateFailureWithRollbackProblems_SomeNotExecutedAndPreviousRollbacked()
	{
		var index0 = new OkStep();
		var index1 = new CannotRollbackStep("two");
		var index2 = new CannotRollbackStep("three");
		var index3 = new CannotExecuteStep("four");
		var index4 = new OkStep();

		var subject = new PipelineRunner<AContext>(index0, index1, index2, index3, index4);

		var context = new AContext("prop");

		var exception =
			Assert.ThrowsAsync<PipelineException>(() => subject.ExecuteAsync(context, CancellationToken.None));

		Assert.Multiple(() =>
		{
			Assert.That(exception.StepIndex, Is.EqualTo(3));
			Assert.That(exception.Message, Does.Contain("[3]"));
			Assert.That(exception.InnerException, Has.Message.EqualTo("four"));
			// two steps had problems rollbacking
			Assert.That(exception.RollbackExceptions, Has.Count.EqualTo(2));

			// the first having problems is step[2]
			Assert.That(exception.RollbackExceptions[0].StepIndex, Is.EqualTo(2));
			Assert.That(exception.RollbackExceptions[0].InnerException, Has.Message.EqualTo("three"));

			// the last having problems is step[1]
			Assert.That(exception.RollbackExceptions[1].StepIndex, Is.EqualTo(1));
			Assert.That(exception.RollbackExceptions[1].InnerException, Has.Message.EqualTo("two"));
		});

		Assert.Multiple(() =>
		{
			// index 3 and previous executed and rollbacked (some successfully)
			Assert.That(index0, Has.Property(nameof(OkStep.Executed)).True.And
				.Property(nameof(OkStep.Rollbacked)).True);

			Assert.That(index1, Has.Property(nameof(OkStep.Executed)).True.And
				.Property(nameof(OkStep.Rollbacked)).True);

			Assert.That(index2, Has.Property(nameof(OkStep.Executed)).True.And
				.Property(nameof(OkStep.Rollbacked)).True);

			Assert.That(index3, Has.Property(nameof(OkStep.Executed)).True.And
				.Property(nameof(OkStep.Rollbacked)).True);

			// we did not get to index 4
			Assert.That(index4, Has.Property(nameof(OkStep.Executed)).False.And
				.Property(nameof(OkStep.Rollbacked)).False);
		});
	}

	#endregion
}