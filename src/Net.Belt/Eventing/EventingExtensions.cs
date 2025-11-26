using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Net.Belt.Eventing;

/// <summary>
/// 
/// </summary>
public static class EventingExtensions
{
	/// <summary>
	/// 
	/// </summary>
	/// <param name="handler"></param>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	public static void Raise(this EventHandler? handler, object sender, EventArgs e) => handler?.Invoke(sender, e);

	/// <summary>
	/// 
	/// </summary>
	/// <param name="handler"></param>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	/// <typeparam name="TEventArgs"></typeparam>
	public static void Raise<TEventArgs>(this EventHandler<TEventArgs>? handler,
		object sender, TEventArgs e) where TEventArgs : EventArgs =>
		handler?.Invoke(sender, e);

	/// <summary>
	/// 
	/// </summary>
	/// <param name="handler"></param>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	/// <typeparam name="TEventArgs"></typeparam>
	/// <typeparam name="TSender"></typeparam>
	public static void Raise<TEventArgs, TSender>(this EventHandler<TEventArgs>? handler,
		TSender sender, TEventArgs e) where TEventArgs : EventArgs
	{
		handler?.Invoke(sender, e);
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="handler"></param>
	/// <param name="sender"></param>
	/// <param name="propertyName"></param>
	public static void Raise(this PropertyChangedEventHandler? handler, object sender,
		[CallerMemberName] string? propertyName = null) =>
		handler?.Invoke(sender, new PropertyChangedEventArgs(propertyName));

	/// <summary>
	/// 
	/// </summary>
	/// <param name="handler"></param>
	/// <param name="sender"></param>
	/// <param name="propertyName"></param>
	public static void Raise(this PropertyChangingEventHandler? handler, object sender,
		[CallerMemberName] string? propertyName = null) =>
		handler?.Invoke(sender, new PropertyChangingEventArgs(propertyName));
	
	/// <summary>
	/// 
	/// </summary>
	/// <param name="notifier"></param>
	/// <param name="handler"></param>
	/// <param name="oldValue"></param>
	/// <param name="newValue"></param>
	/// <param name="propertyName"></param>
	/// <typeparam name="TProp"></typeparam>
	/// <returns></returns>
	public static bool Notify<TProp>(this INotifyPropertyChanging notifier, PropertyChangingEventHandler? handler,
		TProp oldValue, TProp newValue,
		[CallerMemberName] string? propertyName = null)
	{
		if (handler == null) return false;
		var args = new PropertyValueChangingEventArgs<TProp>(propertyName!, oldValue, newValue);
		handler.Invoke(notifier, args);
		return args.IsCancelled;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="notifier"></param>
	/// <param name="handler"></param>
	/// <param name="oldValue"></param>
	/// <param name="newValue"></param>
	/// <param name="propertyName"></param>
	/// <typeparam name="TProp"></typeparam>
	public static void Notify<TProp>(this INotifyPropertyChanged notifier, PropertyChangedEventHandler? handler,
		TProp oldValue,
		TProp newValue, [CallerMemberName] string? propertyName = null)
	{
		handler?.Invoke(notifier, new PropertyValueChangedEventArgs<TProp>(propertyName!, oldValue, newValue));
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="notify"></param>
	/// <param name="handler"></param>
	/// <returns></returns>
	public static IDisposable Observed(this INotifyPropertyChanged notify, PropertyChangedEventHandler handler)
	{
		notify.PropertyChanged += handler;

		return new DisposableAction(() => notify.PropertyChanged -= handler);
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="notify"></param>
	/// <param name="handler"></param>
	/// <returns></returns>
	public static IDisposable Observing(this INotifyPropertyChanging notify, PropertyChangingEventHandler handler)
	{
		notify.PropertyChanging += handler;

		return new DisposableAction(() => notify.PropertyChanging -= handler);
	}
	
	/// <summary>
	/// Fires each event in the invocation list in the order in which
	/// the events were added until an event handler sets the <see cref="IChainedArgs.Handled"/>
	/// property to <c>true</c>.
	/// </summary>
	/// <remarks>Any exception that the event throws must be caught by the caller.</remarks>
	/// <param name="delegates">The multicast delegate</param>
	/// <param name="sender">The source of the event.</param>
	/// <param name="arg">An object that contains the chained event data</param>
	/// <typeparam name="T">Subtype of <see cref="ChainedArgs"/>.</typeparam>
	/// <returns><c>true</c> if an event sink handled the event, <c>false</c> otherwise.</returns>
	public static bool Raise<T>(this ChainedEventHandler<T>? delegates, object sender, T arg) where T : ChainedArgs
	{
		bool handled = false;
		// Assuming the multicast delegate is not null...
		if (delegates != null)
		{
			// Call the methods until one of them handles the event
			// or all the methods in the delegate list are processed.
			Delegate[] invocationList = delegates.GetInvocationList();

			for (int i = 0; i < invocationList.Length && !handled; i++)
			{
				((ChainedEventHandler<T>)invocationList[i])(sender, arg);
				handled = arg.Handled;
			}
		}
		// Return a flag indicating whether an event sink handled
		// the event.
		return handled;
	}
	
	/// <summary>
	/// 
	/// </summary>
	/// <param name="delegates"></param>
	/// <param name="sender"></param>
	/// <param name="args"></param>
	/// <param name="predicate"></param>
	/// <typeparam name="T"></typeparam>
	/// <typeparam name="K"></typeparam>
	/// <returns></returns>
	public static bool RaiseUntil<T, K>(this ChainedEventHandler<T, K>? delegates, object sender, T args, Func<K, bool> predicate) where T : IMutableArgs<K>
	{
		bool handled = false;

		if (delegates != null)
		{
			Delegate[] invocationList = delegates.GetInvocationList();
			for (int i = 0; i < invocationList.Length && !handled; i++)
			{
				K result = ((ChainedEventHandler<T, K>)invocationList[i])(sender, args);
				handled = predicate(result);
			}
		}
		return handled;
	}
}