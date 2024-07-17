using FluffyFox.Commands;
using FluffyFox.Helpers;
using FluffyFox.Repositories;
using FluffyFox.Services;
using System.Collections.ObjectModel;

namespace FluffyFox.ViewModels
{
    public class LocationViewModel : ViewModelBase
	{
		private UserSession UserSession { get; set; }
		private INavigationService Navigation { get; set; }
		public IUserRepository UserRepository { get; set; }

        private readonly Dictionary<string, PingUtility> _pingUtilities = [];
        private readonly Dictionary<string, int> _regionPings = [];

        public bool IsEnabledVpn { get; set; }
		public int GermanyPing { get; private set; }
		public int FrancePing { get; private set; }
		public int CanadaPing { get; private set; }
		public int PolandPing { get; private set; }

        public ObservableCollection<Region> AllRegions { get; set; }
        public ObservableCollection<Region> FavouriteRegions { get; set; }

        public RelayCommand NavigateToHomeCommand { get; }
		public RelayCommand NavigateToHomePolandCommand { get; private set; }
		public RelayCommand NavigateToHomeGermanyCommand { get; private set; }
		public RelayCommand NavigateToHomeCanadaCommand { get; private set; }
		public RelayCommand NavigateToHomeFranceCommand { get; private set; }
        public RelayCommand NavigateToHomeRegionCommand { get; private set; }
        public RelayCommand ToggleFavouriteCommand { get; }

        public LocationViewModel(INavigationService navigationService, UserSession userSession, IUserRepository userRepository)
		{
			Navigation = navigationService;
			UserSession = userSession;
			UserRepository = userRepository;

            AllRegions =
            [
                new Region { ShortName = "PL", IsFavourite = false },
				new Region { ShortName = "DE", IsFavourite = false },
				new Region { ShortName = "FR", IsFavourite = false },
				new Region { ShortName = "CA", IsFavourite = false }
			];

            FavouriteRegions = [];

            InitializePingUtilities();
            CheckUserTariff();
			
			NavigateToHomeCommand = new RelayCommand(o => { Navigation.NavigateTo<HomeViewModel>(); }, o => true);
            NavigateToHomeRegionCommand = new RelayCommand(NavigateToHomeRegion, o => true);
            ToggleFavouriteCommand = new RelayCommand(ToggleFavourite);
        }

        private void NavigateToHomeRegion(object parameter)
        {
			if (parameter is Region region)
			{
                UserSession.Region = region.ShortName;
                Navigation.NavigateTo<HomeViewModel>();
            } 
        }

        private void CheckUserTariff()
		{
			var user = UserSession.CurrentUser;
			IsEnabledVpn = user.TariffId != 20;
		}

        private void InitializePingUtilities()
        {
            foreach (var region in AllRegions)
            {
                InitializePingUtility(region.ShortName, ping => _regionPings[region.ShortName] = ping);
            }
        }

        private void InitializePingUtility(string region, Action<int> setPingValue)
		{
            var host = PingUtility.GetHostByRegion(region);
            var pingUtility = new PingUtility(host);
			pingUtility.PingUpdated += (_, ping) => setPingValue(ping);
            _pingUtilities[region] = pingUtility;
            pingUtility.Start();
		}

        private void ToggleFavourite(object parameter)
        {
			if (parameter is Region region)
			{
                region.IsFavourite = !region.IsFavourite;

                if (region.IsFavourite)
                {
                    FavouriteRegions.Add(region);
                }
                else
                {
                    FavouriteRegions.Remove(region);
                }
            }
        }
    }
}
