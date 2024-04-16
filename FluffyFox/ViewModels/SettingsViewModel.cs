using FluffyFox.Commands;
using FluffyFox.Services;
using System.Diagnostics;
using System.Windows.Input;

namespace FluffyFox.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
		public ICommand NavigateHomeCommand { get; }
		public ICommand NavigatePremiumCommand { get; }
		public ICommand NavigateUserSettingsCommand { get; }
		public ICommand NavigateAuthorizeCommand { get; }
		public ICommand OpenLinkCommand { get; }
		public ICommand CopyCommand { get; }

		public SettingsViewModel(INavigationService homeNavigationService, 
								 INavigationService premiumNavigationService, 
								 INavigationService userNavigationService, 
								 INavigationService authorizeNavigationService)
		{
			NavigateHomeCommand = new NavigateCommand(homeNavigationService);
			NavigatePremiumCommand = new NavigateCommand(premiumNavigationService);
			NavigateUserSettingsCommand = new NavigateCommand(userNavigationService);
			NavigateAuthorizeCommand = new NavigateCommand(authorizeNavigationService);
			OpenLinkCommand = new RelayCommand(OnOpenLinkCommand);
			CopyCommand = new CopyTextCommand();
		}

		private void OnOpenLinkCommand(object parameter)
		{
			Process.Start(new ProcessStartInfo("cmd", $"/c start {parameter}"));
		}
	}
}
