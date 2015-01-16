using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HaveYouSeenMe.Models
{
    public class PetModel
    {

        public int PetID { get; set; }
        
        public string PetName { get; set; }
        
        public int? PetAgeYears { get; set; }
        
        public int? PetAgeMonths { get; set; }
        
        public int StatusID { get; set; }
        
        public System.DateTime? LastSeenOn { get; set; }
        
        public string LastSeenWhere { get; set; }
        
        public string Notes { get; set; }
        
        public int UserId { get; set; }
    }
}