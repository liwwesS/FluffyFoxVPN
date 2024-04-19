using DataAccess.Entities;
using FluffyFox.Library;
using System.Collections.ObjectModel;

namespace DataAccess.Models
{
	public class UsersModel : ObservableObject
	{
		public int Id { get; set; }

		public int RoleId { get; set; }

		private string _key;
		public string Key
		{
			get => _key;
			set => OnPropertyChanged(ref _key, value);
		}

		private DateTime _dateOfRegistration;
		public DateTime DateOfRegistration
		{
			get => _dateOfRegistration;
			set => OnPropertyChanged(ref _dateOfRegistration, value);
		}

		private string? _token;
		public string? Token
		{
			get => _token;
			set => OnPropertyChanged(ref _token, value);
		}

		private ObservableCollection<TariffsModel>? _tariff;
		public ObservableCollection<TariffsModel>? Tariff 
		{
			get => _tariff;
			set => OnPropertyChanged(ref _tariff, value);
		}

		private string? _email;
		public string? Email
		{
			get => _email;
			set => OnPropertyChanged(ref _email, value);
		}

		public static UsersModel ToUserModelMap(Users user)
		{
			return new UsersModel()
			{
				Id = user.Id,
				RoleId = user.RoleId,
				Key = user.Key,
				DateOfRegistration = user.DateOfRegistration,
				Token = user.Token ?? null,
				Tariff = user.Tariff != null ? new ObservableCollection<TariffsModel>(user.Tariff.ToList().Select(TariffsModel.ToTariffModelMap)) : [],
				Email = user.Email ?? null
			};
		}
		
		public static Users ToUserMap(UsersModel user)
		{
			return new Users()
			{
				Id = user.Id,
				RoleId = user.RoleId,
				Key = user.Key,
				DateOfRegistration = user.DateOfRegistration,
				Token = user.Token ?? null,
				Tariff = user.Tariff != null ? new ObservableCollection<Tarrifs>(user.Tariff.ToList().Select(TariffsModel.ToTariffMap)) : [],
				Email = user.Email ?? null
			};
		}
	}
}
