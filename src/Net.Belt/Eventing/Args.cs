namespace Net.Belt.Eventing;

/// <summary>
/// 
/// </summary>
public static class Args
{
	/// <summary>
	/// 
	/// </summary>
	/// <param name="value"></param>
	/// <param name="value2"></param>
	/// <typeparam name="T"></typeparam>
	/// <typeparam name="U"></typeparam>
	/// <returns></returns>
	public static MultiArgs<T, U> Value<T, U>(T value, U value2) => new(value, value2);

	/// <summary>
	/// 
	/// </summary>
	/// <param name="value"></param>
	/// <typeparam name="T"></typeparam>
	/// <returns></returns>
	public static ValueArgs<T> Value<T>(T value) => new(value);

	/// <summary>
	/// 
	/// </summary>
	/// <param name="value"></param>
	/// <typeparam name="T"></typeparam>
	/// <returns></returns>
	public static MutableArgs<T> Mutable<T>(T value) => new(value);

	/// <summary>
	/// 
	/// </summary>
	/// <param name="propertyName"></param>
	/// <param name="oldValue"></param>
	/// <param name="newValue"></param>
	/// <typeparam name="T"></typeparam>
	/// <returns></returns>
	public static PropertyValueChangedEventArgs<T> Changed<T>(string propertyName, T oldValue, T newValue) =>
		new(propertyName, oldValue, newValue);

	/// <summary>
	/// 
	/// </summary>
	/// <param name="index"></param>
	/// <param name="oldValue"></param>
	/// <param name="newValue"></param>
	/// <typeparam name="T"></typeparam>
	/// <returns></returns>
	public static ValueIndexChangedArgs<T> Changed<T>(int index, T oldValue, T newValue) =>
		new(index, oldValue, newValue);

	/// <summary>
	/// 
	/// </summary>
	/// <param name="propertyName"></param>
	/// <param name="oldValue"></param>
	/// <param name="newValue"></param>
	/// <typeparam name="T"></typeparam>
	/// <returns></returns>
	public static PropertyValueChangingEventArgs<T> Changing<T>(string propertyName, T oldValue, T newValue) =>
		new(propertyName, oldValue, newValue);

	/// <summary>
	/// 
	/// </summary>
	/// <param name="index"></param>
	/// <param name="oldValue"></param>
	/// <param name="newValue"></param>
	/// <typeparam name="T"></typeparam>
	/// <returns></returns>
	public static ValueIndexChangingArgs<T> Changing<T>(int index, T oldValue, T newValue) =>
		new(index, oldValue, newValue);
	
	/// <summary>
	/// 
	/// </summary>
	/// <param name="value"></param>
	/// <typeparam name="T"></typeparam>
	/// <returns></returns>
	public static ValueCancelArgs<T> Cancel<T>(T value) => new(value);

	/// <summary>
	/// 
	/// </summary>
	/// <param name="index"></param>
	/// <param name="value"></param>
	/// <typeparam name="T"></typeparam>
	/// <returns></returns>
	public static ValueIndexCancelArgs<T> Cancel<T>(int index, T value) => new(index, value);

	/// <summary>
	/// 
	/// </summary>
	/// <param name="index"></param>
	/// <param name="value"></param>
	/// <typeparam name="T"></typeparam>
	/// <returns></returns>
	public static ValueIndexArgs<T> Index<T>(int index, T value) => new(index, value);
}