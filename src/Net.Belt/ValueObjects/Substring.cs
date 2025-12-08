namespace Net.Belt.ValueObjects;

/// <summary>
/// Represents a substring value object.
/// </summary>
public readonly struct Substring
{
	/// <summary>
	/// Initializes a new instance of the <see cref="Substring"/> struct with the specified value.
	/// </summary>
	/// <param name="value">The string value of the substring.</param>
	public Substring(string value)
	{
		Value = value;
		HasValue = true;
	}

	/// <summary>
	/// Gets the string value of the substring.
	/// </summary>
	public string Value => field ?? string.Empty;

	/// <summary>
	/// Gets a value indicating whether this <see cref="Substring"/> instance has a value.
	/// </summary>
	/// <value><c>true</c> if this instance has a value; otherwise, <c>false</c>.</value>
	public bool HasValue { get; }
	/// <summary>
	/// Gets an empty <see cref="Substring"/> instance.
	/// </summary>
	public static Substring Empty { get; } = new();

	/// <inheritdoc />
	public override string ToString() => Value;
}