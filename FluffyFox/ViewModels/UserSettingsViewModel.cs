using System.Net;
using System.Net.Mail;
using System.Windows.Input;
using FluffyFox.Commands;
using FluffyFox.Helpers;
using FluffyFox.Library;
using FluffyFox.Services;
using FluffyFox.Repositories;
using System.Windows;

namespace FluffyFox.ViewModels
{
	public class UserSettingsViewModel : ViewModelBase
    {
	    private UserSession UserSession { get; set; }

	    private INavigationService Navigation { get; set; }

	    private IUserRepository UserRepository { get; set; }
	    
	    public string Email { get; set; }
	    private string Key { get; set; }
		public string ErrorMessage { get; set; }
		public string OkMessage { get; set; }
	    public bool IsEditable { get; set; }

		public Visibility ErrorMessageVisibility { get; private set; } = Visibility.Collapsed;
		public Visibility OkMessageVisibility { get; private set; } = Visibility.Collapsed;

		public RelayCommand NavigateToSettingsCommand { get; }
		public ICommand SendKeyCommand { get; }

		public UserSettingsViewModel(INavigationService navigationService, UserSession userSession, IUserRepository userRepository)
		{
			Navigation = navigationService;
			UserSession = userSession;
			UserRepository = userRepository;

			Email = UserSession.CurrentUser.Email;
			Key = UserSession.CurrentUser.Key;
			
			IsEditable = string.IsNullOrEmpty(Email);

			SendKeyCommand = new RelayCommand(OnSendEmailCommand);
			NavigateToSettingsCommand = new RelayCommand(o => { Navigation.NavigateTo<SettingsViewModel>(); }, o => true);
		}
		
		private async void OnSendEmailCommand(object parameter)
		{
			ErrorMessageVisibility = Visibility.Collapsed;
			OkMessageVisibility = Visibility.Collapsed;

			var user = await UserRepository.GetUserByKeyAsync(Key);
			var isEmailUnique = await UserRepository.IsEmailUniqueAsync(Email);

			var userKey = user.Key;
			var formattedKey = KeyFormat.FormatString(userKey);

			if (!isEmailUnique)
			{
				if (!string.IsNullOrWhiteSpace(Email))
				{
					ErrorMessage = "Пользователь с таким адресом электронной почты уже зарегистрирован.";
					ErrorMessageVisibility = Visibility.Visible;
					return;
				}
				
				ErrorMessage = "Поле ввода электронной почты не должно быть пустым.";
				ErrorMessageVisibility = Visibility.Visible;
				return;
			}

			if (!IsValidEmail(Email))
			{
				ErrorMessage = "Некорректный формат электронной почты.";
				ErrorMessageVisibility= Visibility.Visible;
				return;
			}

			user.Email = Email;
			await UserRepository.UpdateUserAsync(user);

			UserSession.CurrentUser = await UserRepository.GetUserByKeyAsync(Key);
			
			SendEmail(formattedKey);
		}

		private void SendEmail(string formattedKey)
		{
			var fromAddress = new MailAddress("fluffyfoxnet@gmail.com", "FluffyFox.net");
			var toAddress = new MailAddress(Email, Email);
			var message = new MailMessage(fromAddress, toAddress)
			{
				Body = "Hello, " +
						   Email +
						   Environment.NewLine +
						   "Thank you for registration" +
						   Environment.NewLine +
						   "Your key: " +
						   formattedKey +
						   Environment.NewLine +
						   Environment.NewLine +
						   "Sincerely, your FluffyFox administration.",
				Subject = "Registration"
			};

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
