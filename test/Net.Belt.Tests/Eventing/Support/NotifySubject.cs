using System.ComponentModel;

using Net.Belt.Eventing;

namespace Net.Belt.Tests.Eventing.Support;

internal class NotifySubject : INotifyPropertyChanged, INotifyPropertyChanging
{
	public event PropertyChangedEventHandler? PropertyChanged;
	public event PropertyChangingEventHandler? PropertyChanging;

	public string? S
	{
		get;
		set
		{
			this.Notify(PropertyChanging, field, value);
			field = value;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(S)));
		}
	}

	public int I
	{
		get;
		set
		{
			int old = field;
			this.Notify(PropertyChanging, old, value);
			field = value;
			this.Notify(PropertyChanged, old, value);
		}
	}

	public decimal D
	{
		get;
		set
		{
			PropertyChanging.Raise(this);
			field = value;
			PropertyChanged.Raise(this);
		}
	}

	public float F
	{
		get;
		set
		{
			float old = field;
			bool cancelled = this.Notify(PropertyChanging, old, value);
			if (!cancelled)
			{
				field = value;
				this.Notify(PropertyChanged, old, value);
			}
		}
	}
}