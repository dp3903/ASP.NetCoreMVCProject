using System.ComponentModel.DataAnnotations;

namespace BookManagement.Models
{
    public class UserViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
