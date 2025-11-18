namespace Net.Belt.Patterns.ChainOfResponsibility;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class ChainOfResponsibilityLink<T>
{
	/// <summary>
	/// 
	/// </summary>
	public ChainOfResponsibilityLink<T>? Next { get; private set; }

	/// <summary>
	/// 
	/// </summary>
	/// <param name="context"></param>
	public void Handle(T context)
	{
		if (CanHandle(context))
		{
			DoHandle(context);
		}
		else
		{
			Next?.Handle(context);
		}
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="context"></param>
	/// <returns></returns>
	public bool TryHandle(T context)
	{
		if (CanHandle(context))
		{
			DoHandle(context);
			return true;
		}
		return Next?.TryHandle(context) ?? false;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="context"></param>
	/// <returns></returns>
	public abstract bool CanHandle(T context);
	/// <summary>
	/// 
	/// </summary>
	/// <param name="context"></param>
	protected abstract void DoHandle(T context);

	/// <summary>
	/// 
	/// </summary>
	/// <param name="lastHandler"></param>
	/// <returns></returns>
	public ChainOfResponsibilityLink<T> Chain(IChainOfResponsibilityLink<T> lastHandler) => Chain(new ResponsibleLink<T>(lastHandler));

	private ChainOfResponsibilityLink<T>? _lastLink;
	/// <summary>
	/// 
	/// </summary>
	/// <param name="lastHandler"></param>
	/// <returns></returns>
	public ChainOfResponsibilityLink<T> Chain(ChainOfResponsibilityLink<T> lastHandler)
	{
		if (Next == null)
		{
			Next = lastHandler;
		}
		else
		{
			_lastLink?.Chain(lastHandler);
		}
		_lastLink = lastHandler;
		return this;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="handlers"></param>
	/// <returns></returns>
	public ChainOfResponsibilityLink<T>? Chain(params IChainOfResponsibilityLink<T>[] handlers) => Chain(handlers.AsEnumerable());

	/// <summary>
	/// 
	/// </summary>
	/// <param name="handlers"></param>
	/// <returns></returns>
	public ChainOfResponsibilityLink<T>? Chain(params ChainOfResponsibilityLink<T>[] handlers) => Chain(handlers.AsEnumerable());

	/// <summary>
	/// 
	/// </summary>
	/// <param name="handlers"></param>
	/// <returns></returns>
	public ChainOfResponsibilityLink<T>? Chain(IEnumerable<IChainOfResponsibilityLink<T>> handlers) => Chain(handlers.Select(h => new ResponsibleLink<T>(h)));

	/// <summary>
	/// 
	/// </summary>
	/// <param name="handlers"></param>
	/// <returns></returns>
	public ChainOfResponsibilityLink<T>? Chain(IEnumerable<ChainOfResponsibilityLink<T>> handlers)
	{
		var first = default(ChainOfResponsibilityLink<T>);
		foreach (var link in handlers)
		{
			first = Chain(link);
		}
		return first;
	}
}

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TResult"></typeparam>
public abstract class ChainOfResponsibilityLink<T, TResult>
	{
		/// <summary>
		/// 
		/// </summary>
		public ChainOfResponsibilityLink<T, TResult>? Next { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public TResult Handle(T context)
		{
			TResult result = default!;
			if (CanHandle(context))
			{
				result = DoHandle(context);
			}
			else
			{
				if (Next != null)
				{
					result = Next.Handle(context);
				}
			}
			return result;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <param name="result"></param>
		/// <returns></returns>
		public bool TryHandle(T context, out TResult result)
		{
			result = default!;
			bool handled = false;
			if (CanHandle(context))
			{
				result = DoHandle(context);
				handled = true;
			}
			else
			{
				if (Next != null)
				{
					handled = Next.TryHandle(context, out result);
				}
			}
			return handled;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public abstract bool CanHandle(T context);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		protected abstract TResult DoHandle(T context);

		private ChainOfResponsibilityLink<T, TResult>? _lastLink;
		/// <summary>
		/// 
		/// </summary>
		/// <param name="lastHandler"></param>
		/// <returns></returns>
		public ChainOfResponsibilityLink<T, TResult> Chain(ChainOfResponsibilityLink<T, TResult> lastHandler)
		{

			if (Next == null)
			{
				Next = lastHandler;
			}
			else
			{
				_lastLink?.Chain(lastHandler);
			}
			_lastLink = lastHandler;
			return this;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="lastHandler"></param>
		/// <returns></returns>
		public ChainOfResponsibilityLink<T, TResult>? Chain(IChainOfResponsibilityLink<T, TResult> lastHandler) => Chain(new ResponsibleLink<T, TResult>(lastHandler));

		/// <summary>
		/// 
		/// </summary>
		/// <param name="handlers"></param>
		/// <returns></returns>
		public ChainOfResponsibilityLink<T, TResult>? Chain(params ChainOfResponsibilityLink<T, TResult>[] handlers) => Chain((IEnumerable<ChainOfResponsibilityLink<T, TResult>>)handlers);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="handlers"></param>
		/// <returns></returns>
		public ChainOfResponsibilityLink<T, TResult>? Chain(params IChainOfResponsibilityLink<T, TResult>[] handlers) => Chain((IEnumerable<IChainOfResponsibilityLink<T, TResult>>)handlers);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="handlers"></param>
		/// <returns></returns>
		public ChainOfResponsibilityLink<T, TResult>? Chain(IEnumerable<IChainOfResponsibilityLink<T, TResult>> handlers) => Chain(handlers.Select(h => new ResponsibleLink<T, TResult>(h)));

		/// <summary>
		/// 
		/// </summary>
		/// <param name="handlers"></param>
		/// <returns></returns>
		public ChainOfResponsibilityLink<T, TResult>? Chain(IEnumerable<ChainOfResponsibilityLink<T, TResult>> handlers)
		{
			var first = default(ChainOfResponsibilityLink<T, TResult>);
			foreach (var link in handlers)
			{
				first = Chain(link);
			}
			return first;
		}
	}