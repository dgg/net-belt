using System.Numerics;
using System.Text;

namespace Net.Belt.ValueObjects;

/// <summary>
/// Represents the duration between two dates, calculated in years, months, and days.
/// </summary>
public readonly struct Age : IEquatable<Age>, IComparable<Age>, IComparisonOperators<Age, Age, bool>, IComparable,
	IComparable<TimeSpan>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="Age"/> struct.
	/// </summary>
	/// <param name="advent">The starting date.</param>
	/// <param name="terminus">The ending date.</param>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when the terminus is earlier than the advent.</exception>
	public Age(DateOnly advent, DateOnly terminus)
	{
		if (terminus < advent)
		{
			throw new ArgumentOutOfRangeException(nameof(advent), "Terminus cannot be earlier than advent.");
		}

		Advent = advent;
		Terminus = terminus;

		DateTime start = advent.ToDateTime(TimeOnly.MinValue),
			end = terminus.ToDateTime(TimeOnly.MinValue);
		Span = TimeSpan.FromTicks(end.Ticks - start.Ticks);

		ushort years = (ushort)(terminus.Year - advent.Year);
		DateOnly adjustableAdvent = advent.AddYears(years);

		if (adjustableAdvent > terminus)
		{
			years--;
			adjustableAdvent = advent.AddYears(years);
		}

		int months = 0;
		while (adjustableAdvent.AddMonths(1) <= terminus)
		{
			months++;
			adjustableAdvent = adjustableAdvent.AddMonths(1);
		}

		int days = terminus.DayNumber - adjustableAdvent.DayNumber;

		Years = years;
		Months = Convert.ToByte(months);
		Days = Convert.ToByte(days);

		Age self = this;
		_toString = new Lazy<string>(() =>
		{
			StringBuilder sb = new();
			return sb
				.Append(nameof(Age))
				.Append(" { ")
				.Append(self.printMembers())
				.Append(" }")
				.ToString();
		});
	}

	/// <summary>
	/// Gets the start date of the age calculation.
	/// </summary>
	public DateOnly Advent { get; }

	/// <summary>
	/// Gets the end date of the age calculation.
	/// </summary>
	public DateOnly Terminus { get; }

	/// <summary>
	/// Gets the total duration between <see cref="Terminus"/> and <see cref="Advent"/> as a <see cref="TimeSpan"/>.
	/// </summary>
	public TimeSpan Span { get; }

	/// <summary>
	/// Gets the number of full years between <see cref="Terminus"/> and <see cref="Advent"/>.
	/// </summary>
	public ushort Years { get; }

	/// <summary>
	/// Gets the number of full months between <see cref="Terminus"/> and <see cref="Advent"/>.
	/// </summary>
	public byte Months { get; }

	/// <summary>
	/// Gets the number of days between <see cref="Terminus"/> and <see cref="Advent"/>.
	/// </summary>
	public byte Days { get; }

	/// <summary>
	/// Represents an empty <see cref="Age"/> instance.
	/// </summary>
	public static readonly Age Empty = new();

	/// <summary>
	/// Gets a value indicating whether this <see cref="Age"/> instance is empty.
	/// </summary>
	/// <remarks>An age is considered empty if both <see cref="Advent"/> and <see cref="Terminus"/> are default (<see cref="DateOnly.MinValue"/>).</remarks>
	public bool IsEmpty => Advent.Equals(DateOnly.MinValue) && Terminus.Equals(DateOnly.MinValue);

	/// <inheritdoc />
	public bool Equals(Age other) => Advent.Equals(other.Advent) && Terminus.Equals(other.Terminus);

	/// <inheritdoc />
	public override bool Equals(object? obj) => obj is Age other && Equals(other);

	/// <inheritdoc />
	public override int GetHashCode() => HashCode.Combine(Advent, Terminus);

	/// <inheritdoc />
	public static bool operator ==(Age left, Age right) => left.Equals(right);

	/// <inheritdoc />
	public static bool operator !=(Age left, Age right) => !left.Equals(right);

	/// <inheritdoc />
	public int CompareTo(object? obj)
	{
		return obj switch
		{
			Age other => CompareTo(other),
			TimeSpan span => CompareTo(span),
			_ => throw new ArgumentException($"Object must be of type '{nameof(Age)}' or '{nameof(TimeSpan)}'.",
				nameof(obj))
		};
	}

	/// <inheritdoc />
	public int CompareTo(Age other) => Span.CompareTo(other.Span);

	/// <inheritdoc />
	public static bool operator <(Age left, Age right) => left.CompareTo(right) < 0;

	/// <inheritdoc />
	public static bool operator >(Age left, Age right) => left.CompareTo(right) > 0;

	/// <inheritdoc />
	public static bool operator <=(Age left, Age right) => left.CompareTo(right) <= 0;

	/// <inheritdoc />
	public static bool operator >=(Age left, Age right) => left.CompareTo(right) >= 0;

	/// <inheritdoc />
	public int CompareTo(TimeSpan other) => Span.CompareTo(other);

	private readonly Lazy<string> _toString;

	private string printMembers() =>
		$"{nameof(Advent)} = {Advent:O}, {nameof(Terminus)} = {Terminus:O}, {nameof(Years)} = {Years:N0}, {nameof(Months)} = {Months:N0}, {nameof(Days)} = {Days:N0}";

	/// <inheritdoc />
	/// <remarks>Mimics records' <see cref="object.ToString"/> behavior.</remarks>
	public override string ToString() => _toString.Value;
	
	/// <summary>
	/// Creates a new <see cref="Age"/> instance from a starting date to the current UTC date provided by a <see cref="TimeProvider"/>.
	/// </summary>
	/// <param name="advent">The starting date.</param>
	/// <param name="timeProvider">The time provider to get the current date from.</param>
	/// <returns>A new <see cref="Age"/> instance.</returns>
	public static Age From(DateOnly advent, TimeProvider timeProvider)
	{
		DateOnly terminus = DateOnly.FromDateTime(timeProvider.GetUtcNow().DateTime);
		return new Age(advent, terminus);
	}
}