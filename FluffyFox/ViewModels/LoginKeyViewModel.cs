using FluffyFox.Commands;
using FluffyFox.Services;
using System.Windows.Input;

namespace FluffyFox.ViewModels
{
    public class LoginKeyViewModel : BaseViewModel
    {
		public ICommand NavigateHomeCommand { get; }
		public ICommand NavigateRecoveryCommand { get; }

        public LoginKeyViewModel(INavigationService homeNavigationService, INavigationService recoveryNavigationService)
        {
			NavigateHomeCommand = new NavigateCommand(homeNavigationService);
			NavigateRecoveryCommand = new NavigateCommand(recoveryNavigationService);
		}
    }
}
