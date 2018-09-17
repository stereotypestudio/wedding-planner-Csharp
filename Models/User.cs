using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace LoginReg.Models {
    public class User {
        
        public int UserId {get; set;}

        [Required]
        [MinLength(2)]
        public string Firstname {get; set;}

        [Required]
        [MinLength(2)]
        public string Lastname {get; set;}

        [Required]
        public string Password {get; set;}

        [Required]
        [EmailAddress]
        public string Email {get; set;}

        public List<Rsvp> RsvpLists { get; set; }

        public User() {
            RsvpLists = new List<Rsvp>();
        }
    }
}