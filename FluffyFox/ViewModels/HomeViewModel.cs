using System.Collections.ObjectModel;
using System.Net;
using System.Windows;
using System.Windows.Input;
using FluffyFox.Commands;
using FluffyFox.Helpers;
using FluffyFox.Repositories;
using FluffyFox.Services;

namespace FluffyFox.ViewModels
{
    public class HomeViewModel : ViewModelBase
	{
		private readonly Dictionary<string, PingUtility> _pingUtilities = new();
        public ObservableCollection<RegionViewModel> Regions { get; set; } = new();


        private readonly Dictionary<string, int> _regionPings = new();
        private readonly Dictionary<string, Visibility> _regionVisibilities = new();

		public Visibility IPGridVisibility { get; private set; }

		public bool IsCheckedVpn { get; set; }
		public string IP { get; set; }

		private UserSession UserSession { get; set; }
		private INavigationService Navigation { get; set; }
		public IUserRepository UserRepository { get; set; }

		public RelayCommand NavigateToPremiumCommand { get; }
		public RelayCommand NavigateToSettingsCommand { get; }
		public RelayCommand NavigateToLocationCommand { get; }
		
		public ICommand ConnectToVpnCommand { get; }

		public HomeViewModel(INavigationService navigationService, UserSession userSession, IUserRepository userRepository)
		{
			Navigation = navigationService;
			UserSession = userSession;
			UserRepository = userRepository;

			IsCheckedVpn = UserSession.IsVpnConnect;

            InitializePingUtilities();

            NavigateToPremiumCommand = new RelayCommand(o => { Navigation.NavigateTo<PremiumViewModel>(); }, o => true);
			NavigateToSettingsCommand = new RelayCommand(o => { Navigation.NavigateTo<SettingsViewModel>(); }, o => true);
			NavigateToLocationCommand = new RelayCommand(o => { Navigation.NavigateTo<LocationViewModel>(); }, o => true);

			UpdateGridVisibility();
			CheckIP();

			IP = GetIP();

			ConnectToVpnCommand = new RelayCommand(async o => await OnConnectToVpn());
		}

		private void UpdateGridVisibility()
		{
            foreach (var region in Regions)
            {
                region.Visibility = region.Code == UserSession.Region ? Visibility.Visible : Visibility.Collapsed;
            }
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

        private void InitializePingUtilities()
        {
            var regionData = new[]
            {
                new { Name = "Польша", Icon = "PolandIcon", Code = "PL"},
                new { Name = "Германия", Icon = "GermanyIcon", Code = "DE" },
                new { Name = "Франция", Icon = "FranceIcon", Code = "FR" },
                new { Name = "Канада", Icon = "CanadaIcon", Code = "CA" }
            };

            foreach (var data in regionData)
            {
                var region = new RegionViewModel { Name = data.Name, Icon = data.Icon, Code = data.Code, Visibility = Visibility.Collapsed };
                Regions.Add(region);
                var host = PingUtility.GetHostByRegion(data.Code);
                InitializePingUtility(host, ping => region.Ping = ping);
            }
        }

        private void InitializePingUtility(string host, Action<int> setPingValue)
		{
			var pingUtility = new PingUtility(host);
			pingUtility.PingUpdated += (_, ping) => setPingValue(ping);
			_pingUtilities.Add(host, pingUtility);
			pingUtility.Start();
		}

        private async Task OnConnectToVpn()
		{
            try
            {
                var region = UserSession.Region;
                const string userName = PingUtility.UsernameVpnBook;
                const string userPassword = PingUtility.PasswordVpnBook;

                var host = PingUtility.GetHostByRegion(region);
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
