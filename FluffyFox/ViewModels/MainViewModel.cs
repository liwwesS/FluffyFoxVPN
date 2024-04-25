using FluffyFox.Commands;
using FluffyFox.Services;
using System.Windows.Input;

namespace FluffyFox.ViewModels
{
    public class MainViewModel : ViewModelBase
	{
		public INavigationService Navigation { get; set; }

		public ICommand DragMoveCommand { get; }
		public ICommand CloseCommand { get; }
		public ICommand MinimizeCommand { get; }

		public MainViewModel(INavigationService navigationService)
		{
			DragMoveCommand = new DragMoveCommand();
			CloseCommand = new CloseCommand();
			MinimizeCommand = new MinimizeCommand();

			Navigation = navigationService;
			Navigation.NavigateTo<AuthorizeViewModel>();
		}
	}
}
