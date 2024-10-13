using System.ComponentModel.DataAnnotations;

namespace BookManagement.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        [Required]
        [MinLength(5, ErrorMessage = "Minimum 5 characters required in username.")]
        [MaxLength(15, ErrorMessage = "Username cannot exceed 15 characters.")]
        public string UserName { get; set; }

        [Required]
        [MinLength(5, ErrorMessage = "Minimum 5 characters required in password.")]
        [MaxLength(15, ErrorMessage = "password cannot exceed 15 characters.")]
        public string Password { get; set; }
        public Roles Role { get; set; }
    }
}
