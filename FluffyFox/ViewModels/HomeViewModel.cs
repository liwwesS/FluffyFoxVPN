using FluffyFox.Commands;
using FluffyFox.Services;
using System.Windows.Input;

namespace FluffyFox.ViewModels
{
	public class HomeViewModel : BaseViewModel
	{
		public ICommand NavigatePremiumCommand { get; }
		public ICommand NavigateSettingsCommand { get; }

		public HomeViewModel(INavigationService premiumNavigationService, INavigationService settingsNavigationService)
		{
			NavigatePremiumCommand = new NavigateCommand(premiumNavigationService);
			NavigateSettingsCommand = new NavigateCommand(settingsNavigationService);
		}
	}
}
