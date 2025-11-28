namespace Net.Belt.Collections;

/// <summary>
/// Represents an entry in a sequence with additional information about its position.
/// </summary>
/// <param name="Index">The zero-based index of the entry in the sequence.</param>
/// <param name="Value">The actual value of the entry.</param>
/// <param name="IsFirst"><c>true</c> if this is the first entry in the sequence; otherwise, <c>false</c>.</param>
/// <param name="IsLast"><c>true</c> if this is the last entry in the sequence; otherwise, <c>false</c>.</param>
/// <typeparam name="T">The type of the value.</typeparam>
public readonly record struct SmartEntry<T>(uint Index, T Value, bool IsFirst, bool IsLast);