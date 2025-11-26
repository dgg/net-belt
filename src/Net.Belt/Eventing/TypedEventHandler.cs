namespace Net.Belt.Eventing;

/// <summary>
/// Represents the method that will handle an event when the event provides data.
/// </summary>
/// <typeparam name="TEventArgs"></typeparam>
/// <typeparam name="TSender"></typeparam>
/// <param name="sender">The source of the event.</param>
/// <param name="e">An object that contains the event data.</param>
public delegate void EventHandler<in TEventArgs, in TSender>(TSender sender, TEventArgs e) where TEventArgs : EventArgs;