﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;

namespace DataAccess.Entities
{
	public class Users
	{
		[Key]
		[Column("id")]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[Column("key")]
		[MaxLength(16)]
		[Required]
		public string Key { get; set; }

		[Column("role_id")]
		[Required]
		public int RoleId { get; set; }

		[Column("date_of_registation")]
		[Required]
		public DateTime DateOfRegistration { get; set; }

		[Column("token")]
		public string? Token { get; set; }

		[Column("tarif")]
		[ForeignKey("TariffId")]
		public int? TariffId { get; set; }
		public ObservableCollection<Tarrifs>? Tariff { get; set; } = [];

		[Column("email")]
		[MaxLength(126)]
		public string? Email { get; set; }

		[Column("favourite_regions")]
		[MaxLength(2)]
        public string? FavouriteRegions { get; set; }
    }
}
