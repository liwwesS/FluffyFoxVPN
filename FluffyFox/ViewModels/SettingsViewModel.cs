using FluffyFox.Commands;
using FluffyFox.Helpers;
using FluffyFox.Services;
using System.Diagnostics;
using System.Windows.Input;

namespace FluffyFox.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
		private bool _isDialogOpen;
		public bool IsDialogOpen
		{
			get => _isDialogOpen;
			set 
			{
				OnPropertyChanged(ref _isDialogOpen, value);
				OnPropertyChanged(nameof(BlurEffect));
			} 
		}

		private System.Windows.Controls.Primitives.Popup _copyPopup;
		public System.Windows.Controls.Primitives.Popup CopyPopup
		{
			get { return _copyPopup; }
			set
			{
				_copyPopup = value;
				OnPropertyChanged(nameof(CopyPopup));
			}
		}

		private System.Windows.Media.Effects.Effect _blurEffect;
		public System.Windows.Media.Effects.Effect BlurEffect
		{
			get
			{
				if (IsDialogOpen)
				{
					_blurEffect = new System.Windows.Media.Effects.BlurEffect { Radius = 15 };
				}
				else
				{
					_blurEffect = null;
				}

				return _blurEffect;
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

		private string _key;
		public string Key
		{
			get => _key;
			set
			{

				_key = value;
				OnPropertyChanged(nameof(Key));

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

		private void OnOpenLinkCommand(object parameter)
		{
			Process.Start(new ProcessStartInfo("cmd", $"/c start {parameter}"));
		}
	}
}
