using FluffyFox.Commands;
using FluffyFox.Services;

namespace FluffyFox.ViewModels
{
    public class RecoveryKeyViewModel : ViewModelBase
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

		public RelayCommand NavigateToLoginCommand { get; }

		public RecoveryKeyViewModel(INavigationService navigationService)
		{
			Navigation = navigationService;

			NavigateToLoginCommand = new RelayCommand(o => { Navigation.NavigateTo<LoginKeyViewModel>(); }, o => true);
		}
	}
}
