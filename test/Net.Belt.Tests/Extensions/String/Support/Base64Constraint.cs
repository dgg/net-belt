using System.Buffers.Text;

using NUnit.Framework.Constraints;

namespace Net.Belt.Tests.Extensions.String.Support;

internal class Base64Constraint : Constraint
{
	public override string Description => "a valid Base64 string";
	
	public override ConstraintResult ApplyTo<TActual>(TActual actual)
	{
		if (actual is not string str)
		{
			return new ConstraintResult(this, actual, isSuccess: false);
		}
		
		bool isValid = Base64.IsValid(str);
		return new ConstraintResult(this, actual, isValid);
	}
}

internal partial class Iz : Is
{
	public static Base64Constraint Base64() => new();
}