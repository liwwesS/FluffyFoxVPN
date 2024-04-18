using FluffyFox.Core;

namespace FluffyFox.Services
{
	public interface INavigationService
	{
		ViewModelBase CurrentView { get; }
		void NavigateTo<T>() where T : ViewModelBase;
	}
}
