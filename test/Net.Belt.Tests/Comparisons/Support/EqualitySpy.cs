using System.Diagnostics.CodeAnalysis;

using Net.Belt.Comparisons;

namespace Net.Belt.Tests.Comparisons.Support;

[SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
[SuppressMessage("ReSharper", "BaseObjectGetHashCodeCallInGetHashCode")]
[SuppressMessage("ReSharper", "BaseObjectEqualsIsObjectEquals")]
internal class EqualitySpy : IEquatable<EqualitySpy>
{
	public bool GetHashCodeCalled { get; private set; }
	public bool EqualsCalled { get; private set; }

	public Func<T, T, bool> GetEquals<T>(bool result) =>
		(x, y) =>
		{
			EqualsCalled = true;
			return result;
		};

	public Comparison<T> GetComparison<T>(int result) =>
		(x, y) =>
		{
			EqualsCalled = true;
			return result;
		};

	public Func<T, int> GetHashCode<T>(int result) =>
		x =>
		{
			GetHashCodeCalled = true;
			return result;
		};

	public IComparer<T> GetComparer<T>(int result) => new ComparisonComparer<T>(GetComparison<T>(result));

	public int SelectorCallCount { get; private set; }
	public Func<T, int> GetSelector<T>() =>
		x =>
		{
			SelectorCallCount++;
			return 42;
		};

	public override int GetHashCode()
	{
		GetHashCodeCalled = true;
		return base.GetHashCode();
	}

	public bool Equals(EqualitySpy? other)
	{
		EqualsCalled = true;
		other!.EqualsCalled = true;
		return base.Equals(other);
	}

	public override bool Equals(object? obj)
	{
		EqualsCalled = true;
		return base.Equals(obj);
	}

	public T FakeASelector<T>(T selectorValue) where T : class
	{
		SelectorCallCount++;
		return selectorValue;
	}
}