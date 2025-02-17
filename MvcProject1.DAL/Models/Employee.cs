using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcProject1.DAL.Models
{
    public class Employee : ModelBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public DateTime HireDate { get; set; } 
        public DateTime CreationDate { get; set; } = DateTime.Now;

        public decimal Salary { get; set; }
        public int? Age { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? ImgUrl { get; set; }
        public string Address { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool IsActive { get; set; }

        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }

        



    }
}
