using FluffyFox.Library;
using FluffyFox.ViewModels;

namespace FluffyFox.Services
{
    public class NavigationService : ObservableObject, INavigationService
	{
		private Func<Type, ViewModelBase> _viewModelFactory { get; }

		private ViewModelBase _currentView;
		public ViewModelBase CurrentView
		{
			get => _currentView;
			private set 
			{
				_currentView = value;
				OnPropertyChanged();
			}
		}

		public NavigationService(Func<Type, ViewModelBase> viewModelFactory)
        {
			_viewModelFactory = viewModelFactory;
		}

        public void NavigateTo<TViewModel>() where TViewModel : ViewModelBase
		{
			var viewModel = _viewModelFactory.Invoke(typeof(TViewModel));
			CurrentView = viewModel;
		}
	}
}
