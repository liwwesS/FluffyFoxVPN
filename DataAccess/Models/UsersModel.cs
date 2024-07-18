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

		public int? TariffId { get; set; }
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

        private string? _favouriteRegions;
        public string? FavouriteRegions
        {
            get => _favouriteRegions;
            set => OnPropertyChanged(ref _favouriteRegions, value);
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
				TariffId = user.TariffId,
				Email = user.Email ?? null,
				FavouriteRegions = user.FavouriteRegions ?? null
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
				TariffId = user.TariffId,
				Email = user.Email ?? null,
				FavouriteRegions = user.FavouriteRegions ?? null
			};
		}
	}
}
