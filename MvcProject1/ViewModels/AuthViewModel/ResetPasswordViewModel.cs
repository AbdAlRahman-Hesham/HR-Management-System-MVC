using System.ComponentModel.DataAnnotations;

namespace MvcProject1.PL.ViewModels.AuthViewModel
{
    public class ResetPasswordViewModel
    {

        [Required]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password),ErrorMessage ="Must be like password")]
        public string ConfirmPassword { get; set; }

    }
}
