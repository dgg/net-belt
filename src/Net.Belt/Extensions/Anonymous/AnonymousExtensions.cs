using System.Reflection;

namespace Net.Belt.Extensions.Anonymous;

/// <summary>
/// Extension methods for enhancing the functionality of anonymous types.
/// </summary>
public static class AnonymousExtensions
{
	/// <summary>
	/// Provides extension methods for working with anonymous objects.
	/// </summary>
	/// <param name="anonymousObject">Anonymous object to extend.</param>
	extension<T>(T anonymousObject) where T : class
	{
		private IEnumerable<TResult> transform<TResult>(Func<string, object?, TResult> transformer) =>
			anonymousObject.GetType().GetProperties()
				.Select(p => transformer(p.Name, p.GetValue(anonymousObject)));

		/// <summary>
		/// Converts the given anonymous object to an enumerable of value tuples,
		/// where each tuple contains a property name and its corresponding value.
		/// </summary>
		/// <returns>A sequence of value tuples, with each tuple containing
		/// a property's name as the first element and its value as the second element.</returns>
		public IEnumerable<(string Name, object? Value)> AsValueTuples() => anonymousObject.transform(ValueTuple.Create);

		/// <summary>
		/// Converts the given anonymous object to an enumerable of tuples,
		/// where each tuple contains a property name and its corresponding value.
		/// </summary>
		/// <returns>A sequence of tuples, with each tuple containing
		/// a property's name as the first element and its value as the second element.</returns>
		public IEnumerable<Tuple<string, object?>> AsTuples() => anonymousObject.transform(Tuple.Create);

		/// <summary>
		/// Converts the given anonymous object to an enumerable of key-value pairs,
		/// where each key is a property name and the corresponding value is the property's value.
		/// </summary>
		/// <returns>A sequence of key-value pairs, with each key representing a property's name
		/// and each value representing the property's corresponding value.</returns>
		public IEnumerable<KeyValuePair<string, object?>> AsPairs() => anonymousObject.transform(KeyValuePair.Create);

		/// <summary>
		/// Converts the given anonymous object to a dictionary where property names are the keys and property values are the values.
		/// </summary>
		/// <returns>A dictionary with property names as keys and their corresponding values as values.</returns>
		public IDictionary<string, object?> AsDictionary() => anonymousObject.AsPairs()
			.ToDictionary(p => p.Key, p => p.Value);
	}

	/// <summary>
	/// Provides extension methods for working with anonymous objects.
	/// </summary>
	/// <param name="anonymousObject">Anonymous object to infer.</param>
	extension(object anonymousObject)
	{
		/// <summary>
		/// Casts the given anonymous object to the specified type.
		/// </summary>
		/// <typeparam name="T">The target type to cast the anonymous object to.</typeparam>
		/// <param name="prototype">A parameter of the target type, used as an example for type inference.</param>
		/// <returns>The anonymous object casted to the specified type T.</returns>
		public T Cast<T>(T prototype) where T : class => (T)anonymousObject;
	}

	/// <summary>
	/// Provides extension methods for working with anonymous object creation
	/// and transformation from a dictionary of property values.
	/// </summary>
	/// <param name="dictionary">Dictionary of property values.</param>
	/// <typeparam name="T">The type of the anonymous object to create.</typeparam>
	extension<T>(IDictionary<string, object?> dictionary) where T : notnull
	{
		/// <summary>
		/// Creates an instance of an anonymous object by using the specified prototype and a dictionary of property values.
		/// </summary>
		/// <param name="prototype">An instance of the anonymous object that serves as a prototype for property and constructor mapping.</param>
		/// <returns>
		/// A new instance of the anonymous object type, with its properties initialized using values from the provided dictionary.
		/// </returns>
		public T AsAnonymous(T prototype)
		{
			// get the sole constructor
			var ctor = prototype.GetType().GetConstructors().Single();
			IEnumerable<object?> args = ctor.GetParameters()
				.Select(pi => new { pi, val = dictionary[pi.Name!] })
				.Select(a => a.val != null && a.pi.ParameterType.GetTypeInfo().IsInstanceOfType(a.val) ? a.val : null);
			return (T)ctor.Invoke(args.ToArray());
		}
	}
}