namespace Net.Belt.Eventing;

internal class DisposableAction(Action onDispose) : IDisposable
{
	public void Dispose() => onDispose();
}