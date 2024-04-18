using System.ComponentModel.DataAnnotations.Schema;
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
		public ObservableCollection<Tarrifs>? Tariff { get; set; } = new();

		[Column("email")]
		[MaxLength(126)]
		public string? Email { get; set; }
	}
}
