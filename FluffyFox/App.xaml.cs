using FluffyFox.Services;
using FluffyFox.Stores;
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

		services.AddSingleton<NavigationStore>();
		services.AddSingleton<MainViewModel>();

		services.AddSingleton(CreateAuthorizeNavigationService);

		services.AddTransient(s => new AuthorizeViewModel(CreateLoginNavigationService(s), CreateHomeNavigationService(s)));
		services.AddTransient(s => new LoginKeyViewModel(CreateAuthorizeNavigationService(s), CreateRecoverKeyNavigationService(s)));
		services.AddTransient(s => new RecoveryKeyViewModel(CreateLoginNavigationService(s)));
		services.AddTransient(s => new HomeViewModel(CreatePremiumNavigationService(s), CreateSettingsNavigationService(s)));
		services.AddTransient(s => new PremiumViewModel(CreatePreviousNavigationService(s)));
		services.AddTransient(s => new SettingsViewModel(CreateHomeNavigationService(s), CreatePremiumNavigationService(s), CreateUserSettingsNavigationService(s), CreateAuthorizeNavigationService(s)));
		services.AddTransient(s => new UserSettingsViewModel(CreateSettingsNavigationService(s)));

		services.AddSingleton(s => new MainView()
		{
			DataContext = s.GetRequiredService<MainViewModel>()
		});

		_serviceProvider = services.BuildServiceProvider();
	}

	protected override void OnStartup(StartupEventArgs e)
	{
		var initialNavigationService = _serviceProvider.GetRequiredService<INavigationService>();
		initialNavigationService.Navigate();

		MainWindow = _serviceProvider.GetRequiredService<MainView>();
		MainWindow.Show();

		base.OnStartup(e);
	}

	private static INavigationService CreateAuthorizeNavigationService(IServiceProvider serviceProvider)
	{
		return new NavigationService<AuthorizeViewModel>(
			serviceProvider.GetRequiredService<NavigationStore>(),
			serviceProvider.GetRequiredService<AuthorizeViewModel>);
	}

	private static INavigationService CreateLoginNavigationService(IServiceProvider serviceProvider)
	{
		return new NavigationService<LoginKeyViewModel>(
			serviceProvider.GetRequiredService<NavigationStore>(),
			serviceProvider.GetRequiredService<LoginKeyViewModel>);
	}

	private static INavigationService CreateRecoverKeyNavigationService(IServiceProvider serviceProvider)
	{
		return new NavigationService<RecoveryKeyViewModel>(
			serviceProvider.GetRequiredService<NavigationStore>(),
			serviceProvider.GetRequiredService<RecoveryKeyViewModel>);
	}

	private static INavigationService CreateHomeNavigationService(IServiceProvider serviceProvider)
	{
		return new NavigationService<HomeViewModel>(
			serviceProvider.GetRequiredService<NavigationStore>(),
			serviceProvider.GetRequiredService<HomeViewModel>);
	}

	private static INavigationService CreatePremiumNavigationService(IServiceProvider serviceProvider)
	{
		return new NavigationService<PremiumViewModel>(
			serviceProvider.GetRequiredService<NavigationStore>(),
			serviceProvider.GetRequiredService<PremiumViewModel>);
	}

	private static INavigationService CreateSettingsNavigationService(IServiceProvider serviceProvider)
	{
		return new NavigationService<SettingsViewModel>(
			serviceProvider.GetRequiredService<NavigationStore>(),
			serviceProvider.GetRequiredService<SettingsViewModel>);
	}

	private static INavigationService CreateUserSettingsNavigationService(IServiceProvider serviceProvider)
	{
		return new NavigationService<UserSettingsViewModel>(
			serviceProvider.GetRequiredService<NavigationStore>(),
			serviceProvider.GetRequiredService<UserSettingsViewModel>);
	}

	private static INavigationService CreatePreviousNavigationService(IServiceProvider serviceProvider)
	{
		// Получаем сервис навигации
		var navigationStore = serviceProvider.GetRequiredService<NavigationStore>();

		// Получаем текущую ViewModel
		var currentViewModel = navigationStore.CurrentViewModel;

		if (currentViewModel is SettingsViewModel)
		{
			return CreateSettingsNavigationService(serviceProvider);
		}

		// По умолчанию возвращаем сервис навигации на HomeViewModel
		return CreateHomeNavigationService(serviceProvider);
	}

}