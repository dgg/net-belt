namespace Net.Belt.Eventing;

/// <summary>
/// Provides a set of static methods for creating various types of event argument objects.
/// </summary>
/// <remarks>This class simplifies the instantiation of common event-args types by leveraging type inference.</remarks>
public static class Args
{
	/// <summary>
	/// Creates a new instance of <see cref="MultiArgs{T, U}"/> with two values.
	/// </summary>
	/// <param name="value">The first value.</param>
	/// <param name="value2">The second value.</param>
	/// <typeparam name="T">The type of the first value.</typeparam>
	/// <typeparam name="U">The type of the second value.</typeparam>
	/// <returns>A new <see cref="MultiArgs{T, U}"/> instance containing the specified values.</returns>
	public static MultiArgs<T, U> Value<T, U>(T value, U value2) => new(value, value2);

	/// <summary>
	/// Creates a new instance of <see cref="ValueArgs{T}"/> with a single value.
	/// </summary>
	/// <param name="value">The value.</param>
	/// <typeparam name="T">The type of the value.</typeparam>
	/// <returns>A new <see cref="ValueArgs{T}"/> instance containing the specified value.</returns>
	public static ValueArgs<T> Value<T>(T value) => new(value);

	/// <summary>
	/// Creates a new instance of <see cref="MutableArgs{T}"/> with a mutable value.
	/// </summary>
	/// <param name="value">The initial value.</param>
	/// <typeparam name="T">The type of the mutable value.</typeparam>
	/// <returns>A new <see cref="MutableArgs{T}"/> instance containing the specified value.</returns>
	public static MutableArgs<T> Mutable<T>(T value) => new(value);

	/// <summary>
	/// Creates a new instance of <see cref="PropertyValueChangedEventArgs{T}"/> for a property that has changed.
	/// </summary>
	/// <param name="propertyName">The name of the property that changed.</param>
	/// <param name="oldValue">The old value of the property.</param>
	/// <param name="newValue">The new value of the property.</param>
	/// <typeparam name="T">The type of the property value.</typeparam>
	/// <returns>A new <see cref="PropertyValueChangedEventArgs{T}"/> instance.</returns>
	public static PropertyValueChangedEventArgs<T> Changed<T>(string propertyName, T oldValue, T newValue) =>
		new(propertyName, oldValue, newValue);

	/// <summary>
	/// Creates a new instance of <see cref="ValueIndexChangedArgs{T}"/> for a value at a specific index that has changed.
	/// </summary>
	/// <param name="index">The index of the value that changed.</param>
	/// <param name="oldValue">The old value at the specified index.</param>
	/// <param name="newValue">The new value at the specified index.</param>
	/// <typeparam name="T">The type of the value.</typeparam>
	/// <returns>A new <see cref="ValueIndexChangedArgs{T}"/> instance.</returns>
	public static ValueIndexChangedArgs<T> Changed<T>(int index, T oldValue, T newValue) =>
		new(index, oldValue, newValue);

	/// <summary>
	/// Creates a new instance of <see cref="PropertyValueChangingEventArgs{T}"/> for a property that is changing.
	/// </summary>
	/// <param name="propertyName">The name of the property that is changing.</param>
	/// <param name="oldValue">The old value of the property.</param>
	/// <param name="newValue">The new value of the property.</param>
	/// <typeparam name="T">The type of the property value.</typeparam>
	/// <returns>A new <see cref="PropertyValueChangingEventArgs{T}"/> instance.</returns>
	public static PropertyValueChangingEventArgs<T> Changing<T>(string propertyName, T oldValue, T newValue) =>
		new(propertyName, oldValue, newValue);

	/// <summary>
	/// Creates a new instance of <see cref="ValueIndexChangingArgs{T}"/> for a value at a specific index that is changing.
	/// </summary>
	/// <param name="index">The index of the value that is changing.</param>
	/// <param name="oldValue">The old value at the specified index.</param>
	/// <param name="newValue">The new value at the specified index.</param>
	/// <typeparam name="T">The type of the value.</typeparam>
	/// <returns>A new <see cref="ValueIndexChangingArgs{T}"/> instance.</returns>
	public static ValueIndexChangingArgs<T> Changing<T>(int index, T oldValue, T newValue) =>
		new(index, oldValue, newValue);
	
	/// <summary>
	/// Creates a new instance of <see cref="ValueCancelArgs{T}"/> with a value that can be cancelled.
	/// </summary>
	/// <param name="value">The value.</param>
	/// <typeparam name="T">The type of the value.</typeparam>
	/// <returns>A new <see cref="ValueCancelArgs{T}"/> instance containing the specified value.</returns>
	public static ValueCancelArgs<T> Cancel<T>(T value) => new(value);

	/// <summary>
	/// Creates a new instance of <see cref="ValueIndexCancelArgs{T}"/> with a value at a specific index that can be cancelled.
	/// </summary>
	/// <param name="index">The index of the value.</param>
	/// <param name="value">The value.</param>
	/// <typeparam name="T">The type of the value.</typeparam>
	/// <returns>A new <see cref="ValueIndexCancelArgs{T}"/> instance containing the specified index and value.</returns>
	public static ValueIndexCancelArgs<T> Cancel<T>(int index, T value) => new(index, value);

	/// <summary>
	/// Creates a new instance of <see cref="ValueIndexArgs{T}"/> with a value at a specific index.
	/// </summary>
	/// <param name="index">The index of the value.</param>
	/// <param name="value">The value.</param>
	/// <typeparam name="T">The type of the value.</typeparam>
	/// <returns>A new <see cref="ValueIndexArgs{T}"/> instance containing the specified index and value.</returns>
	public static ValueIndexArgs<T> Index<T>(int index, T value) => new(index, value);
}