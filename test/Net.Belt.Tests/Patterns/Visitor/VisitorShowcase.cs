using Dumpify;

using Net.Belt.Patterns.Visitor;

namespace Net.Belt.Tests.Patterns.Visitor;

[TestFixture, Category("showcase"), Explicit("noisy")]
public class VisitorShowcase
{
	abstract class Employee
	{
		public byte DaysOfVacation { get; set; }
		public abstract void Accept(IEmployeeVisitor visitor);
	}

	class Clerk : Employee
	{
		public Clerk() => DaysOfVacation = 14;
		public override void Accept(IEmployeeVisitor visitor) => visitor.Visit(this);
	}

	class Director : Employee
	{
		public Director() => DaysOfVacation = 16;
		public override void Accept(IEmployeeVisitor visitor) => visitor.Visit(this);
	}

	class President : Employee
	{
		public President() => DaysOfVacation = 21;
		public override void Accept(IEmployeeVisitor visitor) => visitor.Visit(this);
	}

	interface IEmployeeVisitor
	{
		void Visit(Clerk clerk);
		void Visit(Director director);
		void Visit(President president);
	}

	class VacationConfigurator : IEmployeeVisitor
	{
		public void Visit(Clerk visitable) => visitable.DaysOfVacation += 2;

		public void Visit(Director visitable) => visitable.DaysOfVacation += 4;

		public void Visit(President visitable) => visitable.DaysOfVacation += 20;
	}

	[Test]
	public void Classic()
	{
		var visitor = new VacationConfigurator();
		Employee clerk = new Clerk().Dump("initial"),
			director = new Director().Dump("initial"),
			president = new President().Dump("initial");

		clerk.Accept(visitor);
		clerk.Dump("after visitor configuration");

		director.Accept(visitor);
		director.Dump("after visitor configuration");

		president.Accept(visitor);
		president.Dump("after visitor configuration");
	}

	abstract class Staff : IVisitable<Staff>
	{
		public byte DaysOfVacation { get; set; }
		public abstract void Accept(IVisitor<Staff> visitor);
	}

	class Teller : Staff
	{
		public Teller() => DaysOfVacation = 14;

		public override void Accept(IVisitor<Staff> visitor) => visitor.Visit(this);
	}

	class Manager : Staff
	{
		public Manager() => DaysOfVacation = 16;
		public override void Accept(IVisitor<Staff> visitor) => visitor.Visit(this);
	}

	class Executive : Staff
	{
		public Executive() => DaysOfVacation = 18;
		public override void Accept(IVisitor<Staff> visitor) => visitor.Visit(this);
	}

	[Test]
	public void SimplerWithDelegated()
	{
		// no need for IVisitor or its implementation => use delegates
		
		var visitor = new DelegatedVisitor<Staff>()
			.AddVisitor<Executive>(p => p.DaysOfVacation += 20)
			.AddVisitor<Manager>(p => p.DaysOfVacation += 4)
			.AddVisitor<Teller>(p => p.DaysOfVacation += 2);
		
		Staff teller = new Teller(),
			manager = new Manager(),
			executive = new Executive();
		
		teller.Accept(visitor);
		teller
			.Dump("after visitor configuration");
		manager.Accept(visitor);
		manager
			.Dump("after visitor configuration");
		executive.Accept(visitor);
		executive
			.Dump("after visitor configuration");
		
		var another = new DelegatedVisitor<Staff>()
			.AddVisitor<Executive>(p => p.DaysOfVacation += 20)
			.AddVisitor<Manager>(p => p.DaysOfVacation = 1)
			.AddVisitor<Teller>(p => p.DaysOfVacation = 0);
		
		executive.Accept(another);
		executive
			.Dump("after another configuration");
		manager.Accept(another);
		manager
			.Dump("after another configuration");
		teller.Accept(another);
		teller
			.Dump("after another configuration");
	}
}