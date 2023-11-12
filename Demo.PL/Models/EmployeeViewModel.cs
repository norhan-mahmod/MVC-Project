using System.ComponentModel.DataAnnotations;

namespace Demo.PL.Models
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        [StringLength(50, MinimumLength = 10)]
        public string Name { get; set; }
        [EmailAddress]
        public string Address { get; set; }
        public decimal Salary { get; set; }
        public bool IsActive { get; set; }
        public DateTime HireDate { get; set; }
        public IFormFile Image { get; set; }
        public string? ImageUrl { get; set; }
        public int DepartmentId { get; set; }
    }
}
