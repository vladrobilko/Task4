using System.ComponentModel.DataAnnotations;

namespace Task4.Models
{
    public class LoginUserViewModel
    {
        [Required(ErrorMessage = "Please enter your email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter a password.")]
        public string Password { get; set; }
    }
}
