namespace Net.Belt.Eventing;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
/// <param name="sender">The source of the event.</param>
/// <param name="e">An object that contains the chained event data.</param>
public delegate void ChainedEventHandler<in T>(object sender, T e) where T : IChainedArgs;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="K"></typeparam>
/// <param name="sender">The source of the event.</param>
/// <param name="e">An object that contains the mutable event data.</param>
public delegate K ChainedEventHandler<in T, out K>(object sender, T e) where T : IMutableArgs<K>;