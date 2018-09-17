using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace LoginReg.Models {
    public class Wedding { 

        public int WeddingId { get; set; }

        public int CreatorId { get; set; }

        public string Wedder1 { get; set; }
        public string Wedder2 { get; set; }

        public DateTime WeddingDate { get; set; }
        public string Address { get; set; }

        public List<Rsvp> RsvpList { get; set;}

        public Wedding() {
            RsvpList = new List<Rsvp>();
        }
    } 
}