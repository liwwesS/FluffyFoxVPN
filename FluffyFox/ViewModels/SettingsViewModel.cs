using FluffyFox.Commands;
using FluffyFox.Helpers;
using FluffyFox.Services;
using System.Diagnostics;
using System.Windows.Input;
using FluffyFox.Repositories;

namespace FluffyFox.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
		public bool IsDialogOpen { get; set; }
		
		public System.Windows.Media.Effects.Effect BlurEffect
		{
			get
			{
				if (IsDialogOpen)
				{
					return new System.Windows.Media.Effects.BlurEffect { Radius = 15 };
				}
				else
				{
					return null;
				}
			}
		}
		
		public System.Windows.Controls.Primitives.Popup CopyPopup { get; set; }
		
		public string Key { get; set; }
		
		public UserSession UserSession { get; set; }

		public INavigationService Navigation { get; set; }

		public IUserRepository UserRepository { get; set; }

		public RelayCommand NavigateToHomeCommand { get; }
		public RelayCommand NavigateToPremiumCommand { get; }
		public RelayCommand NavigateToUserSettingsCommand { get; }
		public RelayCommand NavigateToAuthorizeCommand { get; }

		public ICommand OpenLinkCommand { get; }
		public ICommand CopyCommand { get; }

		public ICommand OpenDialogCommand { get; }
		public ICommand CloseDialogCommand { get; }

		public SettingsViewModel(INavigationService navigationService, UserSession userSession, IUserRepository userRepository)
		{
			Navigation = navigationService;
			UserSession = userSession;
			UserRepository = userRepository;

			Key = userSession.CurrentUser.Key;
			CopyPopup = new System.Windows.Controls.Primitives.Popup();

			NavigateToHomeCommand = new RelayCommand(o => { Navigation.NavigateTo<HomeViewModel>(); }, o => true);
			NavigateToPremiumCommand = new RelayCommand(o => { Navigation.NavigateTo<PremiumViewModel>(); }, o => true);
			NavigateToUserSettingsCommand = new RelayCommand(o => { Navigation.NavigateTo<UserSettingsViewModel>(); }, o => true);
			NavigateToAuthorizeCommand = new RelayCommand(o => { Navigation.NavigateTo<AuthorizeViewModel>(); }, o => true);

			OpenLinkCommand = new RelayCommand(OnOpenLinkCommand);
			CopyCommand = new CopyTextCommand(CopyPopup);

			OpenDialogCommand = new RelayCommand(OnOpenDialogCommand);
			CloseDialogCommand = new RelayCommand(OnCloseDialogCommand);
		}

		private void OnOpenDialogCommand(object parameter)
		{
			IsDialogOpen = true;
		}
		private void OnCloseDialogCommand(object parameter)
		{
			IsDialogOpen = false;
		}

		private static void OnOpenLinkCommand(object parameter)
		{
			Process.Start(new ProcessStartInfo("cmd", $"/c start {parameter}"));
		}
	}
}
