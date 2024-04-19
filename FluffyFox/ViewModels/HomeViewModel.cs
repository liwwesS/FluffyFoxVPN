using FluffyFox.Commands;
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
				OnPropertyChanged(nameof(UserSession));
			}
		}

		private INavigationService _navigation;
		public INavigationService Navigation
		{
			get => _navigation;
			set
			{
				_navigation = value;
				OnPropertyChanged(nameof(Navigation));
			}
		}

		private IUserRepository _userRepository;
		public IUserRepository UserRepository
		{
			get => _userRepository;
			set
			{
				_userRepository = value;
				OnPropertyChanged(nameof(UserRepository));
			}
		}

		public RelayCommand NavigateToPremiumCommand { get; }
		public RelayCommand NavigateToSettingsCommand { get; }
		public RelayCommand NavigateToLocationCommand { get; }

		public HomeViewModel(INavigationService navigationService, UserSession userSession, IUserRepository userRepository)
		{
			Navigation = navigationService;
			UserSession = userSession;
			UserRepository = userRepository;

			NavigateToPremiumCommand = new RelayCommand(o => { Navigation.NavigateTo<PremiumViewModel>(); }, o => true);
			NavigateToSettingsCommand = new RelayCommand(o => { Navigation.NavigateTo<SettingsViewModel>(); }, o => true);
			NavigateToLocationCommand = new RelayCommand(o => { Navigation.NavigateTo<LocationViewModel>(); }, o => true);
		}
	}
}
