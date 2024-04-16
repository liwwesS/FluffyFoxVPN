using FluffyFox.Stores;
using FluffyFox.ViewModels;

namespace FluffyFox.Services
{
    public class NavigationService<TViewModel> : INavigationService where TViewModel : BaseViewModel
	{
		private readonly NavigationStore _navigationStore;
		private readonly Func<TViewModel> _createViewModel;

		public NavigationService(NavigationStore navigationStore, Func<TViewModel> createViewModel)
		{
			_navigationStore = navigationStore;
			_createViewModel = createViewModel;
		}

		public void Navigate()
        {
			_navigationStore.CurrentViewModel = _createViewModel();
		}
    }
}
