using MvcProject1.DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace MvcProject1.PL.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string? ImgUrl { get; set; }
        public IFormFile? Image { get; set; }
        public DateTime HireDate { get; set; }

        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }
        [Range(18, 60, ErrorMessage = "Age must be in range 18-60")]
        public int Age { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        [RegularExpression(@"^[0-9]{1,3}-[a-zA-Z]{1,10}-[a-zA-Z]{2,10}-[a-zA-Z]{5,10}$",
            ErrorMessage = "Address must be like 123-street-city-country")]
        public string Address { get; set; }
        public bool IsActive { get; set; }

        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }
    }

}
