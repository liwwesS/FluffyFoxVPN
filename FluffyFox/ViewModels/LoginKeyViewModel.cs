using FluffyFox.Commands;
using FluffyFox.Helpers;
using FluffyFox.Services;
using System.Windows;
using FluffyFox.Repositories;
using FluffyFox.Library;

namespace FluffyFox.ViewModels
{
    public class LoginKeyViewModel : ViewModelBase
    {
		public string EnteredKey { get; set; }

		public UserSession UserSession { get; set; }

		public INavigationService Navigation { get; set; }

		public IUserRepository UserRepository { get; set; }

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

			var key = KeyFormat.RemoveSpaces(EnteredKey);
			var isKeyValid = await UserRepository.IsKeyValidAsync(key);
			
			if (isKeyValid)
			{
				UserSession.CurrentUser = await UserRepository.GetUserByKeyAsync(key);
				Navigation.NavigateTo<HomeViewModel>();
			}
			else
			{
				MessageBox.Show("Введенный ключ недействителен.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
	}
}
