using FluffyFox.Commands;
using FluffyFox.Helpers;
using FluffyFox.Repositories;
using FluffyFox.Services;

namespace FluffyFox.ViewModels
{
    public class LocationViewModel : ViewModelBase
	{
		private UserSession UserSession { get; set; }
		private INavigationService Navigation { get; set; }
		public IUserRepository UserRepository { get; set; }
		
		private readonly Dictionary<string, PingUtility> _pingUtilities = new();
		
		public bool IsEnabledVpn { get; set; }
		public int GermanyPing { get; private set; }
		public int FrancePing { get; private set; }
		public int CanadaPing { get; private set; }
		public int PolandPing { get; private set; }
		
		public RelayCommand NavigateToHomeCommand { get; }
		public RelayCommand NavigateToHomePolandCommand { get; private set; }
		public RelayCommand NavigateToHomeGermanyCommand { get; private set; }
		public RelayCommand NavigateToHomeCanadaCommand { get; private set; }
		public RelayCommand NavigateToHomeFranceCommand { get; private set; }

		public LocationViewModel(INavigationService navigationService, UserSession userSession, IUserRepository userRepository)
		{
			Navigation = navigationService;
			UserSession = userSession;
			UserRepository = userRepository;
			
			InitializePingUtility(PingUtility.Germany, ping => GermanyPing = ping);
			InitializePingUtility(PingUtility.France, ping => FrancePing = ping);
			InitializePingUtility(PingUtility.Canada, ping => CanadaPing = ping);
			InitializePingUtility(PingUtility.Poland, ping => PolandPing = ping);
			
			CheckUserTariff();
			
			NavigateToHomeCommand = new RelayCommand(o => { Navigation.NavigateTo<HomeViewModel>(); }, o => true);
			
			NavigateToHomePolandCommand = new RelayCommand(o =>
			{
				UserSession.Region = "PL";
				Navigation.NavigateTo<HomeViewModel>();
			}, o => true);
			
			NavigateToHomeGermanyCommand = new RelayCommand(o =>
			{
				UserSession.Region = "DE";
				Navigation.NavigateTo<HomeViewModel>();
			}, o => true);
			
			NavigateToHomeCanadaCommand = new RelayCommand(o =>
			{
				UserSession.Region = "CA";
				Navigation.NavigateTo<HomeViewModel>();
			}, o => true);
			
			NavigateToHomeFranceCommand = new RelayCommand(o =>
			{
				UserSession.Region = "FR";
				Navigation.NavigateTo<HomeViewModel>();
			}, o => true);
		}

		private void CheckUserTariff()
		{
			var user = UserSession.CurrentUser;
			IsEnabledVpn = user.TariffId != 20;
		}
		
		private void InitializePingUtility(string host, Action<int> setPingValue)
		{
			var pingUtility = new PingUtility(host);
			pingUtility.PingUpdated += (_, ping) => setPingValue(ping);
			_pingUtilities.Add(host, pingUtility);
			pingUtility.Start();
		}
	}
}
