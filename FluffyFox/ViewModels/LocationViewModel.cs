using FluffyFox.Commands;
using FluffyFox.Helpers;
using FluffyFox.Repositories;
using FluffyFox.Services;
using System.Collections.ObjectModel;
using System.Windows;

namespace FluffyFox.ViewModels
{
    public class LocationViewModel : VpnViewModel
	{
        private readonly IUserRepository _userRepository;

        public ObservableCollection<RegionViewModel> FavouriteRegions { get; set; } = [];

        public RelayCommand NavigateToHomeCommand { get; }
        public RelayCommand NavigateToHomeRegionCommand { get; private set; }
        public RelayCommand ToggleFavouriteCommand { get; }

        public Visibility FavouriteRegionsVisibility { get; set; }
        public Visibility FreeRegionsVisibility { get; set; }
        public Visibility PremiumRegionsVisibility { get; set; }

        public bool IsEnabledVpn { get; set; }

        public LocationViewModel(INavigationService navigationService, UserSession userSession, IUserRepository userRepository)
            : base(navigationService, userSession, userRepository)
        {
            _userRepository = userRepository;

            CheckUserTariff();
            LoadFavouriteRegions();
            UpdateRegionsVisibility();

            NavigateToHomeCommand = new RelayCommand(o => Navigation.NavigateTo<HomeViewModel>(), o => true);
            NavigateToHomeRegionCommand = new RelayCommand(NavigateToHomeRegion, o => true);
            ToggleFavouriteCommand = new RelayCommand(ToggleFavourite);
        }

        private void LoadFavouriteRegions()
        {
            var user = UserSession.CurrentUser;
            var favouriteRegionCodes = user.FavouriteRegions?.Split(',', StringSplitOptions.RemoveEmptyEntries);

            if (favouriteRegionCodes != null)
            {
                foreach (var regionCode in favouriteRegionCodes)
                {
                    var region = Regions.FirstOrDefault(r => r.Code == regionCode);
                    if (region != null)
                    {
                        region.IsFavourite = true;
                        FavouriteRegions.Add(region);

                        if (region.IsPremium)
                        {
                            PremiumRegions.Remove(region);
                        }
                        else
                        {
                            FreeRegions.Remove(region);
                        }
                    }
                }
            }
        }

        private void UpdateRegionsVisibility()
        {
            FavouriteRegionsVisibility = FavouriteRegions.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
            PremiumRegionsVisibility = PremiumRegions.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
            FreeRegionsVisibility = FreeRegions.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        private void NavigateToHomeRegion(object parameter)
        {
			if (parameter is RegionViewModel region)
			{
                UserSession.Region = region.Code;
                Navigation.NavigateTo<HomeViewModel>();
            } 
        }

        private void CheckUserTariff()
		{
			var user = UserSession.CurrentUser;
			IsEnabledVpn = user.TariffId != 20;
		}

        private void ToggleFavourite(object parameter)
        {
			if (parameter is RegionViewModel region)
			{
                var user = UserSession.CurrentUser;
                var favouriteRegionCodes = user.FavouriteRegions?.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList() ?? [];

                if (favouriteRegionCodes.Contains(region.Code))
                {
                    favouriteRegionCodes.Remove(region.Code);
                    user.FavouriteRegions = string.Join(',', favouriteRegionCodes);
                    FavouriteRegions.Remove(region);

                    if (region.IsPremium)
                    {
                        PremiumRegions.Add(region);
                    }
                    else
                    {
                        FreeRegions.Add(region);
                    }

                    region.IsFavourite = false;
                }
                else
                {
                    favouriteRegionCodes.Add(region.Code);
                    user.FavouriteRegions = string.Join(',', favouriteRegionCodes);
                    FavouriteRegions.Add(region);
                    region.IsFavourite = true;

                    if (region.IsPremium)
                    {
                        PremiumRegions.Remove(region);
                    }
                    else
                    {
                        FreeRegions.Remove(region);
                    }
                }

                _userRepository.UpdateUserAsync(user);
                UpdateRegionsVisibility();
            }
        }
    }
}
