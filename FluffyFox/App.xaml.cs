using DataAccess;
using FluffyFox.Core;
using FluffyFox.Helpers;
using FluffyFox.Services;
using FluffyFox.ViewModels;
using FluffyFox.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace FluffyFox;

public partial class App : Application
{
	private readonly IServiceProvider _serviceProvider;

	public App()
	{
		IServiceCollection services = new ServiceCollection();

		services.AddSingleton(provider => new MainView
		{
			DataContext = provider.GetRequiredService<MainViewModel>()
		});

		services.AddDbContext<FluffyDbContext>();
	
		services.AddSingleton<MainViewModel>();
		services.AddSingleton<AuthorizeViewModel>();
		services.AddSingleton<LoginKeyViewModel>();
		services.AddSingleton<RecoveryKeyViewModel>();
		services.AddSingleton<HomeViewModel>();
		services.AddSingleton<LocationViewModel>();
		services.AddSingleton<PremiumViewModel>();
		services.AddSingleton<SettingsViewModel>();
		services.AddSingleton<UserSettingsViewModel>();

		services.AddSingleton<Func<Type, ViewModelBase>>(serviceProvider => viewModelType => (ViewModelBase)serviceProvider.GetRequiredService(viewModelType));
		services.AddSingleton<INavigationService, NavigationService>();

		services.AddSingleton<UserSession>();
		services.AddSingleton<IUserRepository, UserRepository>();

		_serviceProvider = services.BuildServiceProvider();
	}

	protected override void OnStartup(StartupEventArgs e)
	{
		MainWindow = _serviceProvider.GetRequiredService<MainView>();
		MainWindow.Show();

		base.OnStartup(e);
	}
}