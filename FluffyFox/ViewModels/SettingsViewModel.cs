using FluffyFox.Commands;
using FluffyFox.Core;
using FluffyFox.Services;
using System.Diagnostics;
using System.Windows.Input;

namespace FluffyFox.ViewModels
{
    public class SettingsViewModel : ViewModelBase
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

		public RelayCommand NavigateToHomeCommand { get; }
		public RelayCommand NavigateToPremiumCommand { get; }
		public RelayCommand NavigateToUserSettingsCommand { get; }
		public RelayCommand NavigateToAuthorizeCommand { get; }

		public ICommand OpenLinkCommand { get; }
		public ICommand CopyCommand { get; }

		public SettingsViewModel(INavigationService navigationService)
		{
			Navigation = navigationService;

			NavigateToHomeCommand = new RelayCommand(o => { Navigation.NavigateTo<HomeViewModel>(); }, o => true);
			NavigateToPremiumCommand = new RelayCommand(o => { Navigation.NavigateTo<PremiumViewModel>(); }, o => true);
			NavigateToUserSettingsCommand = new RelayCommand(o => { Navigation.NavigateTo<UserSettingsViewModel>(); }, o => true);
			NavigateToAuthorizeCommand = new RelayCommand(o => { Navigation.NavigateTo<AuthorizeViewModel>(); }, o => true);

			OpenLinkCommand = new RelayCommand(OnOpenLinkCommand);
			CopyCommand = new CopyTextCommand();
		}

		private void OnOpenLinkCommand(object parameter)
		{
			Process.Start(new ProcessStartInfo("cmd", $"/c start {parameter}"));
		}
	}
}
