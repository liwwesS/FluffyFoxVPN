using FluffyFox.Commands;
using FluffyFox.Services;
using System.Windows.Input;

namespace FluffyFox.ViewModels
{
	public class AuthorizeViewModel : BaseViewModel
	{
		public ICommand NavigateLoginCommand { get; }
		public ICommand NavigateHomeCommand { get; }

		public AuthorizeViewModel(INavigationService loginNavigationService, INavigationService homeNavigationService) 
		{
			NavigateLoginCommand = new NavigateCommand(loginNavigationService);
			NavigateHomeCommand = new NavigateCommand(homeNavigationService);
		}
	}
}
