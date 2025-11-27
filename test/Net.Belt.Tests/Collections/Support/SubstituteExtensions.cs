using Net.Belt.Collections;
using Net.Belt.Eventing;

using NSubstitute;

namespace Net.Belt.Tests.Collections.Support;

internal static class Event
{
	public static Belt.Eventing.EventHandler<TArgs, NotifyingList<T>> Handler<T, TArgs>() => Substitute
		.For<Belt.Eventing.EventHandler<TArgs, NotifyingList<T>>>();
	
	public static Belt.Eventing.EventHandler<TArgs, NotifyingList<T>> Cancelling<T, TArgs>() where TArgs : ICancelArgs
	{
		var substitute =  Substitute
			.For<Belt.Eventing.EventHandler<TArgs, NotifyingList<T>>>();
		substitute
			.WhenForAnyArgs(s => s.Invoke(null!, default!))
			.Do(ci => ci.ArgAt<TArgs>(1).Cancel());
		return substitute;
	}
}

internal static class SubstituteExtensions
{
	public static void CalledOnce<T, TArgs>(this Net.Belt.Eventing.EventHandler<TArgs, NotifyingList<T>> handler) =>
		handler.ReceivedWithAnyArgs().Invoke(null!, default!);
	
	public static void NotCalled<T, TArgs>(this Net.Belt.Eventing.EventHandler<TArgs, NotifyingList<T>> handler) =>
		handler.DidNotReceiveWithAnyArgs().Invoke(null!, default!);
	
	public static void CalledWith<T>(this Net.Belt.Eventing.EventHandler<ValueIndexCancelArgs<T>, NotifyingList<T>> handler, int index, T item, bool cancelled)
	{
		handler.Received().Invoke(Arg.Any<NotifyingList<T>>(), Arg.Is<ValueIndexCancelArgs<T>>(a => 
			a.IsCancelled.Equals(cancelled) &&
			a.Index.Equals(index) &&
			a.Value!.Equals(item)));
	}
	
	public static void CalledWith<T>(this Net.Belt.Eventing.EventHandler<ValueIndexChangingArgs<T>, NotifyingList<T>> handler, int index, T oldValue, T newValue, bool cancelled)
	{
		handler.Received().Invoke(Arg.Any<NotifyingList<T>>(), Arg.Is<ValueIndexChangingArgs<T>>(a => 
			a.IsCancelled.Equals(cancelled) &&
			a.Index.Equals(index) &&
			a.Value!.Equals(oldValue) &&
			a.NewValue!.Equals(newValue)));
	}
	
	public static void CalledWith<T>(this Net.Belt.Eventing.EventHandler<ValueIndexChangedArgs<T>, NotifyingList<T>> handler, int index, T oldValue, T newValue)
	{
		handler.Received().Invoke(Arg.Any<NotifyingList<T>>(), Arg.Is<ValueIndexChangedArgs<T>>(a => 
			a.Index.Equals(index) &&
			a.Value!.Equals(newValue) &&
			a.OldValue!.Equals(oldValue)));
	}
	
	public static void CalledWith<T>(this Net.Belt.Eventing.EventHandler<ValueIndexArgs<T>, NotifyingList<T>> handler, int index, T item)
	{
		var expected = new ValueIndexArgs<T>(index, item);
		handler.Received().Invoke(Arg.Any<NotifyingList<T>>(), Arg.Is<ValueIndexArgs<T>>(expected));
	}
	
	public static void CalledWith<T>(this Net.Belt.Eventing.EventHandler<CancelArgs, NotifyingList<T>> handler, bool cancelled)
	{
		handler.Received().Invoke(Arg.Any<NotifyingList<T>>(), Arg.Is<CancelArgs>(a => 
			a.IsCancelled.Equals(cancelled)));
	}
}