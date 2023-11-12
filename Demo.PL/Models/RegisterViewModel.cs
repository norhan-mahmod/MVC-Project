﻿using System.ComponentModel.DataAnnotations;

namespace Demo.PL.Models
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "InValid Email")]
        public string Email { get; set; }
        [Required]
        [StringLength(6 , MinimumLength =  6)]
        public string Password { get; set; }
        [Required]
        [Compare(nameof(Password) , ErrorMessage = "Password MisMatch")]
        public string ConfirmPassword { get; set; }
        [Required]
        public bool IsAgree { get; set; }
    }
}
