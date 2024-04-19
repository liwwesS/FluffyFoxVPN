using FluffyFox.Commands;
using FluffyFox.Services;

namespace FluffyFox.ViewModels
{
    public class UserSettingsViewModel : ViewModelBase
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

		public RelayCommand NavigateToSettingsCommand { get; }

		public UserSettingsViewModel(INavigationService navigationService)
		{
			Navigation = navigationService;
			NavigateToSettingsCommand = new RelayCommand(o => { Navigation.NavigateTo<SettingsViewModel>(); }, o => true);
		}
	}
}
