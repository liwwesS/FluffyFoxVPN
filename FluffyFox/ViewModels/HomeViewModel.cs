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

		public Visibility PolandGridVisibility { get; private set; } = Visibility.Collapsed;
		public Visibility GermanyGridVisibility { get; private set; } = Visibility.Collapsed;
		public Visibility CanadaGridVisibility { get; private set; } = Visibility.Collapsed;
		public Visibility FranceGridVisibility { get; private set; } = Visibility.Collapsed;
		public Visibility IPGridVisibility { get; private set; }

		public bool IsCheckedVpn { get; set; }

		public int GermanyPing { get; private set; }
		public int FrancePing { get; private set; }
		public int CanadaPing { get; private set; }
		public int PolandPing { get; private set; }

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

			InitializePingUtility(PingUtility.Germany, ping => GermanyPing = ping);
			InitializePingUtility(PingUtility.France, ping => FrancePing = ping);
			InitializePingUtility(PingUtility.Canada, ping => CanadaPing = ping);
			InitializePingUtility(PingUtility.Poland, ping => PolandPing = ping);

			NavigateToPremiumCommand = new RelayCommand(o => 
			{

				Navigation.NavigateTo<PremiumViewModel>();
			}, o => true);

			NavigateToSettingsCommand = new RelayCommand(o => 
			{

				Navigation.NavigateTo<SettingsViewModel>();
			}, o => true);

			NavigateToLocationCommand = new RelayCommand(o => 
			{

				Navigation.NavigateTo<LocationViewModel>();
			}, o => true);

			UpdateGridVisibility();
			CheckIP();
			IP = GetIP();

			ConnectToVpnCommand = new RelayCommand(OnConnectToVpn);
		}

		private void UpdateGridVisibility()
		{
			PolandGridVisibility = Visibility.Collapsed;
			GermanyGridVisibility = Visibility.Collapsed;
			CanadaGridVisibility = Visibility.Collapsed;
			FranceGridVisibility = Visibility.Collapsed;
			
			switch (UserSession.Region)
			{
				case "PL":
					PolandGridVisibility = Visibility.Visible;
					break;
				case "DE":
					GermanyGridVisibility = Visibility.Visible;
					break;
				case "CA":
					CanadaGridVisibility = Visibility.Visible;
					break;
				case "FR":
					FranceGridVisibility = Visibility.Visible;
					break;
				default:
					PolandGridVisibility = Visibility.Visible;
					break;
			}
		}

		private void CheckIP()
		{
			if (IsCheckedVpn)
			{
				IPGridVisibility = Visibility.Visible;
			}
			else
			{
				IPGridVisibility = Visibility.Collapsed;
			}
		}

		private static string GetIP()
		{
			string host = System.Net.Dns.GetHostName();
			string ip = System.Net.Dns.GetHostByName(host).AddressList[0].ToString();

			return ip;
		}

		private void InitializePingUtility(string host, Action<int> setPingValue)
		{
			var pingUtility = new PingUtility(host);
			pingUtility.PingUpdated += (_, ping) => setPingValue(ping);
			_pingUtilities.Add(host, pingUtility);
			pingUtility.Start();
		}
		
		private async void OnConnectToVpn(object parameter)
		{
			var region = UserSession.Region;
			const string userName = PingUtility.UsernameVpnBook;
			const string userPassword = PingUtility.PasswordVpnBook;

			switch (region)
			{
				case "PL":
					region = PingUtility.Poland;
					break;
				case "DE":
					region = PingUtility.Germany;
					break;
				case "CA":
					region = PingUtility.Canada;
					break;
				case "FR":
					region = PingUtility.France;
					break;
				default:
					region = PingUtility.Poland;
					break;
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
	}
}
