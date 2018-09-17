using System;
using System.ComponentModel.DataAnnotations;
namespace LoginReg.Models {
    public class TestUser {
        
        [Required]
        [MinLength(2)]
        public string Firstname {get; set;}

        [Required]
        [MinLength(2)]
        public string Lastname {get; set;}

        [Required]
        [Compare("PasswordConfirm", ErrorMessage = "Passwords must match")]
        public string Password {get; set;}

        [Required]
        public string PasswordConfirm {get; set;}

        [Required]
        [EmailAddress]
        public string Email {get; set;}


    }
}