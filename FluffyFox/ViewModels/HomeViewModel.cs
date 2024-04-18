using FluffyFox.Core;
using FluffyFox.Helpers;
using FluffyFox.Services;

namespace FluffyFox.ViewModels
{
    public class HomeViewModel : ViewModelBase
	{
		private UserSession _userSession;
		public UserSession UserSession
		{
			get => _userSession;
			set
			{
				_userSession = value;
				OnPropertyChanged();
			}
		}

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

		private readonly IUserRepository _userRepository;

		public RelayCommand NavigateToPremiumCommand { get; }
		public RelayCommand NavigateToSettingsCommand { get; }
		public RelayCommand NavigateToLocationCommand { get; }

		public HomeViewModel(INavigationService navigationService, UserSession userSession, IUserRepository userRepository)
		{
			Navigation = navigationService;
			UserSession = userSession;
			_userRepository = userRepository;

			NavigateToPremiumCommand = new RelayCommand(o => { Navigation.NavigateTo<PremiumViewModel>(); }, o => true);
			NavigateToSettingsCommand = new RelayCommand(o => { Navigation.NavigateTo<SettingsViewModel>(); }, o => true);
			NavigateToLocationCommand = new RelayCommand(o => { Navigation.NavigateTo<LocationViewModel>(); }, o => true);
		}
	}
}
