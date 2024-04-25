using FluffyFox.Commands;
using FluffyFox.Helpers;
using FluffyFox.Repositories;
using FluffyFox.Services;

namespace FluffyFox.ViewModels
{
    public class AuthorizeViewModel : ViewModelBase
	{
		public UserSession UserSession { get; set; }

		public INavigationService Navigation { get; set; }

		public IUserRepository UserRepository { get; set; }

		public RelayCommand NavigateToLoginCommand { get; }
		public RelayCommand NavigateToHomeCommand { get; }

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
