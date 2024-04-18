using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities
{
	[Table("tarifs")]
	public class Tarrifs
	{
		[Key]
		[Column("id")]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[Column("question")]
		public string? Question { get; set; }

		[Column("cost")]
		public int? Cost { get; set; }

		[Column("mounthh")]
		[Required]
		public int CountMonth { get; set; }

		[Column("created_at")]
		public DateTime? DateOfCreate { get; set; }

	}
}
