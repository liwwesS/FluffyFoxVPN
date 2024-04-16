using FluffyFox.Commands;
using FluffyFox.Services;
using System.Diagnostics;
using System.Windows.Input;

namespace FluffyFox.ViewModels
{
	public class PremiumViewModel : BaseViewModel
	{
		public ICommand NavigateBackCommand { get; }
		public ICommand OpenLinkCommand { get; }

		public PremiumViewModel(INavigationService previousNavigationService)
		{
			NavigateBackCommand = new NavigateCommand(previousNavigationService);
			OpenLinkCommand = new RelayCommand(OnOpenLinkCommand);
		}

		private void OnOpenLinkCommand(object parameter)
		{
			Process.Start(new ProcessStartInfo("cmd", $"/c start {parameter}"));
		}
	}
}
