using MvcProject1.DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace MvcProject1.PL.ViewModels
{
    public class DepartmentViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = $"{nameof(Name)} is requird")]
        public string Name { get; set; }


        [Required(ErrorMessage = $"{nameof(Code)} is requird")]
        public string Code { get; set; }

        [Display(Name = "Date Of Creation")]
        public DateTime DateOfCreation { get; set; }
        public ICollection<Employee>? Employees { get; set; }
            = new HashSet<Employee> { };
    }
}
