using NUnit.Framework.Constraints;

namespace Net.Belt.Tests.ValueObjects.Support;

internal class Throwz : Throws
{
	public static Constraint BoundException<T>(T upper, string upperBoundRepresentation) =>
		InstanceOf<ArgumentOutOfRangeException>().With.Property(nameof(ArgumentOutOfRangeException.ActualValue)).EqualTo(upper).And
			.Message.Contains(upperBoundRepresentation);
}