using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;

namespace FluffyFox.Views
{
	public partial class HomeView : UserControl
	{
		public HomeView()
		{
			InitializeComponent();
		}

		private void ToggleButton_Checked(object sender, System.Windows.RoutedEventArgs e)
		{
			if (sender is ToggleButton toggleButton)
			{
				var storyboard = FindResource("MyAnimation") as Storyboard;
				storyboard.Stop(grayFoxIcon);
				storyboard.Begin(grayFoxIcon, true);

			}
		}

		private void ToggleButton_Unchecked(object sender, System.Windows.RoutedEventArgs e)
		{
			if (sender is ToggleButton toggleButton)
			{
				var storyboard = FindResource("MyAnimation") as Storyboard;
				storyboard.Stop(grayFoxIcon);
			}
		}
	}
}
