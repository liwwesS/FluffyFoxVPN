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
	    private UserSession UserSession { get; set; }

	    private INavigationService Navigation { get; set; }

	    private IUserRepository UserRepository { get; set; }
	    
		public string Email { get; set; }
		public string ErrorMessage { get; set; }
		public string OkMessage { get; set; }
		
		public Visibility ErrorMessageVisibility { get; private set; } = Visibility.Collapsed;
		public Visibility OkMessageVisibility { get; private set; } = Visibility.Collapsed;

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
			ErrorMessageVisibility = Visibility.Collapsed;
			OkMessageVisibility = Visibility.Collapsed;

			var currentUser = await UserRepository.GetUserByEmailAsync(Email);

			if (currentUser == null)
			{
				if (string.IsNullOrWhiteSpace(Email))
				{
					ErrorMessage = "Поле ввода электронной почты не должно быть пустым.";
					ErrorMessageVisibility = Visibility.Visible;
					return;
				}

				if (!IsValidEmail(Email))
				{
					ErrorMessage = "Некорректный формат электронной почты.";
					ErrorMessageVisibility = Visibility.Visible;
					return;
				}

				ErrorMessage = "Пользователя с таким адресом электронной почты не существует.";
				ErrorMessageVisibility = Visibility.Visible;
				return;
			}
	
			var userKey = currentUser.Key;
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

			OkMessage = "Ключ успешно отправлен на почту!";
			OkMessageVisibility = Visibility.Visible;
		}

		private static bool IsValidEmail(string email)
		{
			return System.Text.RegularExpressions.Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
		}
	}
}
