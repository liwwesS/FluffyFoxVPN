using FluffyFox.Helpers;
using FluffyFox.Repositories;
using FluffyFox.Services;
using System.Collections.ObjectModel;

namespace FluffyFox.ViewModels
{
    public abstract class VpnViewModel : ViewModelBase
    {
        private readonly Dictionary<string, PingUtility> _pingUtilities = [];

        protected UserSession UserSession { get; private set; }
        protected INavigationService Navigation { get; private set; }
        protected IUserRepository UserRepository { get; private set; }

        public ObservableCollection<RegionViewModel> Regions { get; set; } = [];
        public ObservableCollection<RegionViewModel> FreeRegions { get; set; } = [];
        public ObservableCollection<RegionViewModel> PremiumRegions { get; set; } = [];
        
        protected VpnViewModel(INavigationService navigationService, UserSession userSession, IUserRepository userRepository)
        {
            Navigation = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            UserSession = userSession ?? throw new ArgumentNullException(nameof(userSession));
            UserRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));

            InitializeRegions();
            InitializePingUtilities();
        }

        private void InitializeRegions()
        {
            var regionData = new[]
            {
                new { Name = "Польша", Icon = "PolandIcon", Code = "PL", IsPremium = false },
                new { Name = "Германия", Icon = "GermanyIcon", Code = "DE", IsPremium = true },
                new { Name = "Франция", Icon = "FranceIcon", Code = "FR", IsPremium = true },
                new { Name = "Канада", Icon = "CanadaIcon", Code = "CA", IsPremium = true }
            };

            foreach (var data in regionData)
            {
                var region = new RegionViewModel { Name = data.Name, Icon = data.Icon, Code = data.Code, IsPremium = data.IsPremium };
                Regions.Add(region);

                if (data.IsPremium)
                {
                    PremiumRegions.Add(region);
                }
                else
                {
                    FreeRegions.Add(region);
                }
            }
        }

        private void InitializePingUtilities()
        {
            foreach (var region in Regions)
            {
                InitializePingUtility(region.Code, ping => region.Ping = ping);
            }
        }

        private void InitializePingUtility(string regionCode, Action<int> setPingValue)
        {
            var host = PingUtility.GetHostByRegion(regionCode) ?? throw new ArgumentException($"Host not found for region: {regionCode}");
            var pingUtility = new PingUtility(host);
            pingUtility.PingUpdated += (_, ping) => setPingValue(ping);
            _pingUtilities[regionCode] = pingUtility;
            pingUtility.Start();
        }
    }
}
