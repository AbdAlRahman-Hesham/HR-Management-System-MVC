using System.ComponentModel.DataAnnotations;

namespace MvcProject1.PL.ViewModels.AuthViewModel
{
    public class SignUpViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password),ErrorMessage ="Must be like password")]
        public string ConfirmPassword { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required]
        public bool IsAgree { get; set; }
        


    }
}
