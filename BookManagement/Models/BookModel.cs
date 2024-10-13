using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookManagement.Models
{
	public class BookModel
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[Required, MaxLength(20)]
		public string Title { get; set; }

		[Required, MaxLength(200)]
		public string Description { get; set; }

		[Required, MaxLength(20)]
		public string Author { get; set; }

		[Required, StringLength(13, MinimumLength = 13, ErrorMessage = "ISBN must be 13 characters long.")]
		public string ISBN { get; set; }

		[Required, EmailAddress]
		public string author_email { get; set; }

		public int price { get; set; }
	}
}
