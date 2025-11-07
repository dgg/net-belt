using System.Diagnostics.CodeAnalysis;

namespace Net.Belt.Comparisons;

/// <summary>
/// Represents an abstract base class for creating chainable equality comparers.
/// This class allows combining multiple equality comparison logic in a sequential manner.
/// </summary>
/// <typeparam name="T">The type of objects to compare.</typeparam>
public abstract class ChainableEqualizer<T> : IEqualityComparer<T>
{
	/// <summary>
	/// Determines whether the specified objects are equal.
	/// This method is expected to be implemented by subclasses to define custom equality logic.
	/// </summary>
	/// <param name="x">The first object to compare.</param>
	/// <param name="y">The second object to compare.</param>
	/// <returns>True if the specified objects are equal; otherwise, false.</returns>
	protected abstract bool DoEquals(T x, T y);

	/// <summary>
	/// Generates a hash code for the specified object.
	/// This method is expected to be implemented by subclasses to define custom hash code generation logic.
	/// </summary>
	/// <param name="obj">The object for which to generate a hash code.</param>
	/// <returns>An integer that represents the hash code of the specified object.</returns>
	protected abstract int DoGetHashCode(T obj);

	/// <inheritdoc />
	public bool Equals(T? x, T? y)
	{
		bool? shortCircuit = handleNulls(x, y);
		if (shortCircuit.HasValue) return shortCircuit.Value;

		// they are not null, so we can compare them
		bool result = DoEquals(x!, y!);
		if (needsToEvaluateNext(result))
		{
			result = _nextEqualizer.Equals(x, y);
		}

		return result;
	}

	private static bool? handleNulls(T? x, T? y)
	{
		bool? shortCircuit = null;
		if (!typeof(T).IsValueType)
		{
			if (x == null)
			{
				shortCircuit = (y == null);
			}
			else if (y == null)
			{
				shortCircuit = false;
			}

		}
		return shortCircuit;
	}

	/// <inheritdoc />
	public int GetHashCode(T x)
	{
		int result = DoGetHashCode(x);
		return result;
	}

	[MemberNotNullWhen(true, nameof(_nextEqualizer))]
	private bool needsToEvaluateNext(bool ret)
	{
		return ret && _nextEqualizer != null;
	}

	private ChainableEqualizer<T>? _nextEqualizer;
	private void chain(ChainableEqualizer<T> lastEqualizer)
	{
		if (_nextEqualizer == null)
		{
			_nextEqualizer = lastEqualizer;
		}
		else
		{
			_nextEqualizer.chain(lastEqualizer);
		}
	}

	private ChainableEqualizer<T>? _lastEqualizer;

	/// <summary>
	/// Chains the current equalizer with the provided equalizer to evaluate equality and hash codes sequentially.
	/// </summary>
	/// <param name="equalizer">The equalizer to chain with the current equalizer instance.</param>
	/// <returns>The current instance of the chainable equalizer, allowing further chaining operations.</returns>
	public ChainableEqualizer<T> Then(ChainableEqualizer<T> equalizer)
	{
		if (_nextEqualizer == null)
		{
			_nextEqualizer = equalizer;
		}
		else
		{
			_lastEqualizer?.chain(equalizer);
		}
		_lastEqualizer = equalizer;
		return this;
	}
}