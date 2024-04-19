using FluffyFox.ViewModels;

namespace FluffyFox.Services
{
    public interface INavigationService
	{
		ViewModelBase CurrentView { get; }
		void NavigateTo<TViewModel>() where TViewModel : ViewModelBase;
	}
}
