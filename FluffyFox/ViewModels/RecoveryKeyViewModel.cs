using System.Net;
using System.Net.Mail;
using System.Windows;
using System.Windows.Input;
using FluffyFox.Commands;
using FluffyFox.Helpers;
using FluffyFox.Library;
using FluffyFox.Repositories;
using FluffyFox.Services;

namespace FluffyFox.ViewModels
{
    public class RecoveryKeyViewModel : ViewModelBase
    {
	    public UserSession UserSession { get; set; }

	    public INavigationService Navigation { get; set; }

	    public IUserRepository UserRepository { get; set; }
	    
		public string Email { get; set; }

		public RelayCommand NavigateToLoginCommand { get; }

		public ICommand SendKeyCommand { get; }

		public RecoveryKeyViewModel(INavigationService navigationService, UserSession userSession, IUserRepository userRepository)
		{
			Navigation = navigationService;
			UserSession = userSession;
			UserRepository = userRepository;

			NavigateToLoginCommand = new RelayCommand(o => { Navigation.NavigateTo<LoginKeyViewModel>(); }, o => true);
			SendKeyCommand = new RelayCommand(OnSendEmailCommand);
		}

		private async void OnSendEmailCommand(object parameter)
		{

			UserSession.CurrentUser = await UserRepository.GetUserByEmailAsync(Email);
			var user = UserSession.CurrentUser;

			if (user == null) return;
			var userKey = user.Key;
			var formattedKey = KeyFormat.FormatString(userKey);
				
			SendEmail(formattedKey);
		}

		private void SendEmail(string formattedKey)
		{
			var fromAddress = new MailAddress("fluffyfoxnet@gmail.com", "FluffyFox.net");
			var toAddress = new MailAddress(Email, Email);
			var message = new MailMessage(fromAddress, toAddress);
			
			message.Body = "Hello, " + 
			               Email + 
			               Environment.NewLine + 
			               Environment.NewLine + 
			               "Your key: " +
			               formattedKey +
			               Environment.NewLine + 
			               Environment.NewLine +
			               "Sincerely, your FluffyFox administration.";
			message.Subject = "Access key recovery";

			SmtpClient smtpClient = new()
			{
				Host = "smtp.gmail.com",
				Port = 587,
				EnableSsl = true,
				DeliveryMethod = SmtpDeliveryMethod.Network,
				UseDefaultCredentials = false,
				Credentials = new NetworkCredential("fluffyfoxnet@gmail.com", "judv ctih ktby ovwm")
			};

			smtpClient.Send(message);

			MessageBox.Show("Ключ успешно отправлен на почту!");
		}
	}
}
