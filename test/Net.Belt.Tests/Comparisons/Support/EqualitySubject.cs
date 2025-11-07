namespace Net.Belt.Tests.Comparisons.Support;

internal class EqualitySubject
{
	public EqualitySubject()  { }

	public EqualitySubject(string? s, int i, decimal d)
	{
		S = s;
		I = i;
		D = d;
	}

	public int I { get; init; }
	public string? S { get; init; }
	public decimal D { get; init;  }

	public override string ToString() => "[" + S + " " + I + " " + D + "]";

	public override int GetHashCode()
	{
		unchecked
		{
			var hashCode = I;
			hashCode = (hashCode * 397) ^ D.GetHashCode();
			hashCode = (hashCode * 397) ^ (S != null ? S.GetHashCode() : 0);
			return hashCode;
		}
	}
}