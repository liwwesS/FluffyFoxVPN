using FluffyFox.Commands;
using FluffyFox.Stores;
using System.Windows.Input;

namespace FluffyFox.ViewModels
{
	public class MainViewModel : BaseViewModel
	{
		private readonly NavigationStore _navigationStore;

		public BaseViewModel CurrentViewModel => _navigationStore.CurrentViewModel;

		public ICommand DragMoveCommand { get; }
		public ICommand CloseCommand { get; }
		public ICommand MinimizeCommand { get; }

		public MainViewModel(NavigationStore navigationStore)
		{
			DragMoveCommand = new DragMoveCommand();
			CloseCommand = new CloseCommand();
			MinimizeCommand = new MinimizeCommand();

			_navigationStore = navigationStore;
			_navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
		}

		private void OnCurrentViewModelChanged()
		{
			OnPropertyChanged(nameof(CurrentViewModel));
		}
	}
}
