using System.ComponentModel.DataAnnotations;

namespace MvcProject1.PL.ViewModels.AuthViewModel
{
    public class ForgotPasswordViewModel
    {


        [Required]
        [EmailAddress]
        public string Email { get; set; }

    }
}
