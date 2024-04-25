using FluffyFox.Commands;
using FluffyFox.Services;
using System.Diagnostics;
using System.Windows.Input;

namespace FluffyFox.ViewModels
{
    public class PremiumViewModel : ViewModelBase
	{
		public INavigationService Navigation { get; set; }

		public RelayCommand NavigateToHomeCommand { get; }
		public ICommand OpenLinkCommand { get; }

		public PremiumViewModel(INavigationService navigationService)
		{
			Navigation = navigationService;

			NavigateToHomeCommand = new RelayCommand(o => { Navigation.NavigateTo<HomeViewModel>(); }, o => true);
			OpenLinkCommand = new RelayCommand(OnOpenLinkCommand);
		}

		private static void OnOpenLinkCommand(object parameter)
		{
			Process.Start(new ProcessStartInfo("cmd", $"/c start {parameter}"));
		}
	}
}
