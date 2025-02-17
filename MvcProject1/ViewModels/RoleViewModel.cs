using System.ComponentModel.DataAnnotations;

namespace MvcProject1.PL.ViewModels
{
    public class RoleViewModel
    {
        public string? Id { get; set; }
        [Required]
        [Display(Name = "Role Name")]
        public string Name { get; set; }
        public RoleViewModel()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
