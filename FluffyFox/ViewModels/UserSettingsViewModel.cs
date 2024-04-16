using FluffyFox.Commands;
using FluffyFox.Services;
using System.Windows.Input;

namespace FluffyFox.ViewModels
{
    public class UserSettingsViewModel : BaseViewModel
    {
		public ICommand NavigateSettingsCommand { get; }

		public UserSettingsViewModel(INavigationService settingsNavigationService)
		{
			NavigateSettingsCommand = new NavigateCommand(settingsNavigationService);
		}
	}
}
