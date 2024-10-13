using System.ComponentModel.DataAnnotations;

namespace BookManagement.Models
{
    public class SignUpUserViewModel
    {
        [Required,
        MinLength(5, ErrorMessage = "Minimum 5 characters required in username."),
        MaxLength(15, ErrorMessage = "Username cannot exceed 15 characters.")]
        public string UserName { get; set; }
        [Required,
        MinLength(5, ErrorMessage = "Minimum 5 characters required in password."),
        MaxLength(15, ErrorMessage = "password cannot exceed 15 characters.")]
        public string Password { get; set; }
        [Required,
        Compare("Password", ErrorMessage = "Password must match confirm password.")]
        public string ConfirmPassword { get; set; }
    }
}
