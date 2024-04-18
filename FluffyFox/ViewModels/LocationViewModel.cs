using FluffyFox.Core;
using FluffyFox.Services;

namespace FluffyFox.ViewModels
{
    public class LocationViewModel : ViewModelBase
	{
		private INavigationService _navigation;
		public INavigationService Navigation
		{
			get => _navigation;
			set
			{
				_navigation = value;
				OnPropertyChanged();
			}
		}

		public RelayCommand NavigateToHomeCommand { get; }

		public LocationViewModel(INavigationService navigationService)
		{
			Navigation = navigationService;
			NavigateToHomeCommand = new RelayCommand(o => { Navigation.NavigateTo<HomeViewModel>(); }, o => true);
		}
	}
}
