﻿using System.Windows;
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

		public Visibility RussiaGridVisibility { get; private set; } = Visibility.Collapsed;
		public Visibility GermanyGridVisibility { get; private set; } = Visibility.Collapsed;
		public Visibility CanadaGridVisibility { get; private set; } = Visibility.Collapsed;
		public Visibility FranceGridVisibility { get; private set; } = Visibility.Collapsed;

		public bool IsVpnConnected { get; set; } = false;

		public int GermanyPing { get; private set; }
		public int FrancePing { get; private set; }
		public int CanadaPing { get; private set; }
		public int RussiaPing { get; private set; }
		
		public UserSession UserSession { get; set; }
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
			
			InitializePingUtility(PingUtility.Germany, ping => GermanyPing = ping);
			InitializePingUtility(PingUtility.France, ping => FrancePing = ping);
			InitializePingUtility(PingUtility.Canada, ping => CanadaPing = ping);
			InitializePingUtility(PingUtility.Russia, ping => RussiaPing = ping);

			NavigateToPremiumCommand = new RelayCommand(o => { Navigation.NavigateTo<PremiumViewModel>(); }, o => true);
			NavigateToSettingsCommand = new RelayCommand(o => { Navigation.NavigateTo<SettingsViewModel>(); }, o => true);
			NavigateToLocationCommand = new RelayCommand(o => { Navigation.NavigateTo<LocationViewModel>(); }, o => true);

			UpdateGridVisibility();
			ConnectToVpnCommand = new RelayCommand(OnConnectToVpn);
		}

		private void UpdateGridVisibility()
		{
			RussiaGridVisibility = Visibility.Collapsed;
			GermanyGridVisibility = Visibility.Collapsed;
			CanadaGridVisibility = Visibility.Collapsed;
			FranceGridVisibility = Visibility.Collapsed;
			
			switch (UserSession.Region)
			{
				case "RUS":
					RussiaGridVisibility = Visibility.Visible;
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
					RussiaGridVisibility = Visibility.Visible;
					break;
			}
		}
		
		private void InitializePingUtility(string host, Action<int> setPingValue)
		{
			var pingUtility = new PingUtility(host);
			pingUtility.PingUpdated += (_, ping) => setPingValue(ping);
			_pingUtilities.Add(host, pingUtility);
			pingUtility.Start();
		}
		
		private void OnConnectToVpn(object parameter)
		{
			var vpnManager = new VpnManager();
			string region = UserSession.Region;
			string userName;
			string userPassword;

			switch (region)
			{
				case "RUS":
					region = PingUtility.Russia;
					userName = PingUtility.UsernameVpn4You;
					userPassword = PingUtility.PasswordVpn4You;
					break;
				case "DE":
					region = PingUtility.Germany;
					userName = PingUtility.UsernameVpnBook;
					userPassword = PingUtility.PasswordVpnBook;
					break;
				case "CA":
					region = PingUtility.Canada;
					userName = PingUtility.UsernameVpnBook;
					userPassword = PingUtility.PasswordVpnBook;
					break;
				case "FR":
					region = PingUtility.France;
					userName = PingUtility.UsernameVpnBook;
					userPassword = PingUtility.PasswordVpnBook;
					break;
				default:
					region = PingUtility.Russia;
					userName = PingUtility.UsernameVpn4You;
					userPassword = PingUtility.PasswordVpn4You;
					break;
			}

			if (IsVpnConnected)
			{
				vpnManager.Disconnect();
				IsVpnConnected = false;
			}
			else
			{
				vpnManager.Connect(region, userName, userPassword);
				IsVpnConnected = true;
			}
		}
	}
}
