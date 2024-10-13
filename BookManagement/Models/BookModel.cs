using System.ComponentModel.DataAnnotations;

namespace BookManagement.Models
{
    public class BookModel
    {
        public int Id { get; set; }
        [Required, MaxLength(20)]
        public string Title { get; set; }
        [Required, MaxLength(200)]
        public string Description { get; set; }
        [Required, MaxLength(20)]
        public string Author { get; set; }
        [Required,Length(13,13,ErrorMessage = "ISBN must be 13 characters long.")]
        public long ISBN { get; set; }
        [Required, EmailAddress]
        public string author_email { get; set; }
        public int price { get; set; }
    }
}
