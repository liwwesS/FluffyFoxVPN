using FluffyFox.Commands;
using FluffyFox.Helpers;
using FluffyFox.Services;
using System.Net;
using System.Net.Mail;
using System.Windows;
using System.Windows.Input;
using FluffyFox.Repositories;

namespace FluffyFox.ViewModels
{
    public class UserSettingsViewModel : ViewModelBase
    {
		private UserSession _userSession;
		public UserSession UserSession
		{
			get => _userSession;
			set
			{
				_userSession = value;
				OnPropertyChanged();
			}
		}

		private string _email;
		public string Email
		{
			get => _email;
			set
			{
				_email = value;
				OnPropertyChanged();
			}
		}

		private INavigationService _navigation;
		public INavigationService Navigation
		{
			get => _navigation;
			set
			{
				_navigation = value;
				OnPropertyChanged();
			}
		}

		private IUserRepository _userRepository;
		public IUserRepository UserRepository
		{
			get => _userRepository;
			set
			{
				_userRepository = value;
				OnPropertyChanged();
			}
		}

		public RelayCommand NavigateToSettingsCommand { get; }
		public ICommand SendEmailCommand { get; }

		public UserSettingsViewModel(INavigationService navigationService, UserSession userSession, IUserRepository userRepository)
		{
			Navigation = navigationService;
			UserSession = userSession;
			UserRepository = userRepository;

			Email = UserSession.CurrentUser.Email;

			NavigateToSettingsCommand = new RelayCommand(o => { Navigation.NavigateTo<SettingsViewModel>(); }, o => true);
		}

		public void OnSendEmailCommand(object parameter)
		{
			MailAddress fromAdress = new MailAddress("fluffyfoxnet@gmail.com", "FluffyFox.net");
			MailAddress toAdress = new MailAddress(Email, Email);
			MailMessage message = new MailMessage(fromAdress, toAdress);

			message.Body = "Добрый день, " + 
				Email + 
				Environment.NewLine + 
				Environment.NewLine + 
				"" + 
				Environment.NewLine + 
				Environment.NewLine +
						 "С уважением администрация медицинского центра";
			message.Subject = "Медицинский центр";

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
