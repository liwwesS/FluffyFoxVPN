using FluffyFox.Commands;
using FluffyFox.Services;

namespace FluffyFox.ViewModels
{
    public class LocationViewModel : ViewModelBase
	{
		public INavigationService Navigation { get; set; }

		public RelayCommand NavigateToHomeCommand { get; }

		public LocationViewModel(INavigationService navigationService)
		{
			Navigation = navigationService;
			NavigateToHomeCommand = new RelayCommand(o => { Navigation.NavigateTo<HomeViewModel>(); }, o => true);
		}
	}
}
