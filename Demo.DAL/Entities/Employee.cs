using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Entities
{
    public class Employee : BaseEntity
    {
        [StringLength(50, MinimumLength = 10)]
        public string Name { get; set; }
        [EmailAddress]
        public string Address { get; set; }
        public decimal Salary { get; set; }
        public bool IsActive { get; set; }
        public DateTime HireDate { get; set; }
        public string ImageUrl { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
    }
}
