using Net.Belt.ValueObjects;

using NUnit.Framework.Constraints;

namespace Net.Belt.Tests.ValueObjects.Support;

internal class Haz : Has
{
	public static Constraint Components(ushort years, byte months, byte days) =>
		Property(nameof(Age.Years)).EqualTo(years).And
			.Property(nameof(Age.Months)).EqualTo(months).And
			.Property(nameof(Age.Days)).EqualTo(days);

	public static Constraint Span(TimeSpan value) =>
		Property(nameof(Age.Span)).EqualTo(value);
}

internal partial class Iz : Is
{
	public static Constraint Bounded(DateOnly advent, DateOnly terminus) => Has
		.Property(nameof(Age.Advent)).EqualTo(advent).And
		.Property(nameof(Age.Terminus)).EqualTo(terminus);
}