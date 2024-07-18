using System.Net;
using System.Windows;
using FluffyFox.Commands;
using FluffyFox.Helpers;
using FluffyFox.Repositories;
using FluffyFox.Services;

namespace FluffyFox.ViewModels
{
    public class HomeViewModel : VpnViewModel
	{
        public Visibility IPGridVisibility { get; private set; }
        public RegionViewModel SelectedRegion { get; set; }

        public RelayCommand NavigateToPremiumCommand { get; }
        public RelayCommand NavigateToSettingsCommand { get; }
        public RelayCommand NavigateToLocationCommand { get; }
        public RelayCommand ConnectToVpnCommand { get; }

        public bool IsCheckedVpn { get; set; }
        public string IP { get; set; }

        public HomeViewModel(INavigationService navigationService, UserSession userSession, IUserRepository userRepository)
            : base(navigationService, userSession, userRepository)
        {
            IsCheckedVpn = UserSession.IsVpnConnect;

            NavigateToPremiumCommand = new RelayCommand(o => Navigation.NavigateTo<PremiumViewModel>(), o => true);
            NavigateToSettingsCommand = new RelayCommand(o => Navigation.NavigateTo<SettingsViewModel>(), o => true);
            NavigateToLocationCommand = new RelayCommand(o => Navigation.NavigateTo<LocationViewModel>(), o => true);

            CheckIP();
            SetSelectedRegion();

            IP = GetIP();

            ConnectToVpnCommand = new RelayCommand(async o => await OnConnectToVpn());
        }

        private void CheckIP()
		{
            IPGridVisibility = IsCheckedVpn ? Visibility.Visible : Visibility.Collapsed;
        }

		private static string GetIP()
		{
            var host = Dns.GetHostName();
            var ip = Dns.GetHostEntry(host).AddressList.FirstOrDefault()?.ToString();

            return ip ?? "Unknown";
        }

        private void SetSelectedRegion()
        {
            var selectedRegionCode = UserSession.Region;
            SelectedRegion = Regions.FirstOrDefault(r => r.Code == selectedRegionCode);

            SelectedRegion ??= Regions.FirstOrDefault(r => r.Code == "PL");
        }

        private async Task OnConnectToVpn()
		{
            try
            {
                var region = UserSession.Region;
                const string userName = PingUtility.UsernameVpnBook;
                const string userPassword = PingUtility.PasswordVpnBook;

                var host = PingUtility.GetHostByRegion(region);

                if (string.IsNullOrEmpty(host))
                {
                    throw new ArgumentException("Host cannot be null or empty.");
                }

                if (IsCheckedVpn)
                {
                    UserSession.IsVpnConnect = true;
                    await VpnManager.Connect(region, userName, userPassword);
                    IPGridVisibility = Visibility.Visible;
                    IP = GetIP();
                }
                else
                {
                    UserSession.IsVpnConnect = false;
                    IPGridVisibility = Visibility.Collapsed;
                    await VpnManager.Disconnect();
                }
            }
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
            }

        }
	}
}
