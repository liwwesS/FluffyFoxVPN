using FluffyFox.Helpers;

namespace FluffyFox.Commands
{
	public class CloseCommand : BaseCommand
	{
		public override async void Execute(object? parameter)
		{
			System.Windows.Application.Current.Shutdown();
			await VpnManager.Disconnect();
		}
	}
}
