using System.ComponentModel.DataAnnotations;

namespace Demo.PL.Models
{
    public class DepartmentViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
