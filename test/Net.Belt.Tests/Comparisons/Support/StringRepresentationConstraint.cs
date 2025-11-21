using System.Collections;

using NUnit.Framework.Constraints;
using NUnit.Framework.Internal;

namespace Net.Belt.Tests.Comparisons.Support;

internal partial class StringRepresentationConstraint(string representation) : Constraint
{
	private readonly EqualConstraint _inner = new(representation);
	
	private static string represent(IEnumerable collection) => string.Join(", ", collection.Cast<object>().Select(o => o.ToString()));

	public override ConstraintResult ApplyTo<TActual>(TActual actual)
	{
		string current = represent((IEnumerable)actual!);
		var result = _inner.ApplyTo(current);
		return new RepresentationResult(_inner, result);
	}

	public override string Description
	{
		get
		{
			MessageWriter writer = new TextMessageWriter();
			representedAs(writer);
			writer.WriteValue(representation);
			return writer.ToString();
		}
	}
	
	private static void representedAs(MessageWriter writer)
	{
		writer.Write("Something representable as ");
	}
	
	class RepresentationResult(IConstraint constraint, ConstraintResult result)
		: ConstraintResult(constraint, result.ActualValue, result.IsSuccess)
	{
		public override void WriteActualValueTo(MessageWriter writer)
		{
			representedAs(writer);
			result.WriteActualValueTo(writer);
		}
	}
}

internal class Doez : Does
{
	public static StringRepresentationConstraint RepresentAs(string representation) => new(representation);
}