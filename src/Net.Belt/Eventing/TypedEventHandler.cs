namespace Net.Belt.Eventing;

/// <summary>
/// Represents the method that will handle an event when the event provides data.
/// </summary>
/// <typeparam name="TArgs">The type of the event data.</typeparam>
/// <typeparam name="TSender">The type of source of the event.</typeparam>
/// <param name="sender">The source of the event.</param>
/// <param name="e">An object that contains the event data.</param>
public delegate void EventHandler<in TArgs, in TSender>(TSender sender, TArgs e);