namespace Net.Belt.Eventing;

/// <summary>
/// Represents a method that handles a chained event.
/// </summary>
/// <typeparam name="T">The type of the event data, which must implement <see cref="IChainedArgs"/>.</typeparam>
/// <param name="sender">The source of the event.</param>
/// <param name="e">An object that contains the chained event data.</param>
public delegate void ChainedEventHandler<in T>(object sender, T e) where T : IChainedArgs;

/// <summary>
/// Represents a method that handles a chained event with a mutable return value.
/// </summary>
/// <typeparam name="T">The type of the event data, which must implement <see cref="IMutableArgs{K}"/>.</typeparam>
/// <typeparam name="K">The type of the mutable return value.</typeparam>
/// <param name="sender">The source of the event.</param>
/// <param name="e">An object that contains the mutable event data.</param>
public delegate K ChainedEventHandler<in T, out K>(object sender, T e) where T : IMutableArgs<K>;