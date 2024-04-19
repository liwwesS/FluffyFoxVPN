using FluffyFox.Commands;
using FluffyFox.Helpers;
using FluffyFox.Services;
using System.Windows;

namespace FluffyFox.ViewModels
{
    public class LoginKeyViewModel : ViewModelBase
    {
		private string _enteredKey;
		public string EnteredKey
		{
			get => _enteredKey;
			set
			{
				_enteredKey = value;
				OnPropertyChanged(nameof(EnteredKey));
			}
		}

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

		public RelayCommand NavigateToAuthorizeCommand { get; }
		public RelayCommand NavigateToRecoveryCommand { get; }
		public RelayCommand NavigateToHomeCommand { get; }

        public LoginKeyViewModel(INavigationService navigationService, UserSession userSession, IUserRepository userRepository)
        {
			Navigation = navigationService;
			UserSession = userSession;
			UserRepository = userRepository;

			NavigateToAuthorizeCommand = new RelayCommand(o => { Navigation.NavigateTo<AuthorizeViewModel>(); }, o => true);
			NavigateToRecoveryCommand = new RelayCommand(o => { Navigation.NavigateTo<RecoveryKeyViewModel>(); }, o => true);
			NavigateToHomeCommand = new RelayCommand(Execute, o => true);
		}

        private async void Execute(object o)
        {
	        await NavigateAndCheckUserAsync();
        }

        private async Task NavigateAndCheckUserAsync()
		{
			if (string.IsNullOrWhiteSpace(EnteredKey))
			{
				MessageBox.Show("Пожалуйста, введите ключ", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			var isKeyValid = await _userRepository.IsKeyValidAsync(EnteredKey);

			if (isKeyValid)
			{
				UserSession.CurrentUser = await _userRepository.GetUserByKeyAsync(EnteredKey);
				Navigation.NavigateTo<HomeViewModel>();
			}
			else
			{
				MessageBox.Show("Введенный ключ недействителен. Пожалуйста, попробуйте снова.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
	}
}
