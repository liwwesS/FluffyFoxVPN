using FluffyFox.Commands;
using FluffyFox.Helpers;
using FluffyFox.Services;

namespace FluffyFox.ViewModels
{
    public class AuthorizeViewModel : ViewModelBase
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
				OnPropertyChanged(nameof(UserSession));
			}
		}

		private IUserRepository _userRepository;
		public IUserRepository UserRepository
		{
			get => _userRepository;
			set
			{
				_userRepository = value;
				OnPropertyChanged(nameof(UserSession));
			}
		}

		public RelayCommand NavigateToLoginCommand { get; set; }
		public RelayCommand NavigateToHomeCommand { get; set; }

		public AuthorizeViewModel(INavigationService navigationService, UserSession userSession, IUserRepository userRepository)
		{
			Navigation = navigationService;
			UserSession = userSession;
			UserRepository = userRepository;

			NavigateToLoginCommand = new RelayCommand(o => { Navigation.NavigateTo<LoginKeyViewModel>(); }, o => true);
			NavigateToHomeCommand = new RelayCommand(Execute, o => true);
		}

		private async void Execute(object o)
		{
			await NavigateAndAddUserAsync();
		}

		private async Task NavigateAndAddUserAsync()
		{
			var key = await UserRepository.GenerateUniqueKeyAsync();
			var isUnique = await UserRepository.IsKeyUniqueAsync(key);

			if (isUnique)
			{
				await UserRepository.AddUserAsync(key);

				UserSession.CurrentUser = await UserRepository.GetUserByKeyAsync(key);
				Navigation.NavigateTo<HomeViewModel>();
			}
		}
	}
}
