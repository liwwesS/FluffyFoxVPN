using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;

namespace FluffyFox.Commands
{
	public class CopyTextCommand : BaseCommand
	{
		private readonly Popup _popup;

		public CopyTextCommand(Popup popup)
		{
			_popup = popup;
		}

		public override void Execute(object? parameter)
		{
			if (parameter is string text)
			{
				Clipboard.SetText(text);
				ShowTooltip();
			}
		}

		private void ShowTooltip()
		{
			_popup.IsOpen = true;

			var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1.2) }; // Желаемое время показа
			timer.Tick += (sender, args) =>
			{
				_popup.IsOpen = false;
				timer.Stop();
			};
			timer.Start();
		}
	}
}
