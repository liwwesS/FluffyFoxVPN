using FluffyFox.Commands;
using FluffyFox.Services;
using System.Windows.Input;

namespace FluffyFox.ViewModels
{
    public class RecoveryKeyViewModel : BaseViewModel
    {
		public ICommand NavigateLoginCommand { get; }

		public RecoveryKeyViewModel(INavigationService loginNavigationService)
		{
			NavigateLoginCommand = new NavigateCommand(loginNavigationService);
		}
	}
}
