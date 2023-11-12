using System.ComponentModel.DataAnnotations;

namespace Demo.PL.Models
{
    public class ForgetPasswordViewModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "InValid Email")]
        public string Email { get; set; }
    }
}
