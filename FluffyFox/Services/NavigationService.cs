using FluffyFox.Core;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FluffyFox.Services
{
	public class NavigationService : INotifyPropertyChanged, INavigationService
	{
		public Func<Type, ViewModelBase> _viewModelFactory { get; }
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
			ViewModelBase viewModel = _viewModelFactory.Invoke(typeof(TViewModel));
			CurrentView = viewModel;
		}

		public event PropertyChangedEventHandler? PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
