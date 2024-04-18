using FluffyFox.Core;
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

		public RelayCommand NavigateToLoginCommand { get; set; }
		public RelayCommand NavigateToHomeCommand { get; set; }

		public AuthorizeViewModel(INavigationService navigationService, UserSession userSession, IUserRepository userRepository)
		{
			Navigation = navigationService;
			UserSession = userSession;
			_userRepository = userRepository;

			NavigateToLoginCommand = new RelayCommand(o => { Navigation.NavigateTo<LoginKeyViewModel>(); }, o => true);
			NavigateToHomeCommand = new RelayCommand(Execute, o => true);
		}

		private async void Execute(object o)
		{
			await NavigateAndAddUserAsync();
		}

		private async Task NavigateAndAddUserAsync()
		{
			var key = await _userRepository.GenerateUniqueKeyAsync();
			var isUnique = await _userRepository.IsKeyUniqueAsync(key);

			if (isUnique)
			{
				await _userRepository.AddUserAsync(key);

				UserSession.CurrentUser = await _userRepository.GetUserByKeyAsync(key);
				Navigation.NavigateTo<HomeViewModel>();
			}
		}
	}
}
