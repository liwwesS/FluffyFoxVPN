using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Threading;

namespace FluffyFox.Commands
{
	public class CopyTextCommand : BaseCommand
	{
		public override void Execute(object? parameter)
		{
			if (parameter is string text)
			{
				Clipboard.SetText(text);
				ShowTooltip();
			}
		}

		private static Popup? FindPopup(DependencyObject parent)
		{
			if (parent == null) return null;

			var count = VisualTreeHelper.GetChildrenCount(parent);
			for (var i = 0; i < count; i++)
			{
				var child = VisualTreeHelper.GetChild(parent, i);
				if (child is Popup popup)
				{
					return popup;
				}
				else
				{
					var result = FindPopup(child);
					if (result != null) return result;
				}
			}

			return null;
		}

		private static void ShowTooltip()
		{
			var mainWindow = Application.Current.MainWindow;
			if (mainWindow != null)
			{
				var popup = FindPopup(mainWindow);
				if (popup != null)
				{
					var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1.2) }; // Желаемое время показа
					timer.Tick += (sender, args) =>
					{
						popup.IsOpen = false;
						timer.Stop();
					};
					popup.IsOpen = true;
					timer.Start();
				}
			}
		}
	}
}
