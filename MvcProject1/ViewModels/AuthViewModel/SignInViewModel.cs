using System.ComponentModel.DataAnnotations;

namespace MvcProject1.PL.ViewModels.AuthViewModel
{
    public class SignInViewModel
    {


        [Required]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}
