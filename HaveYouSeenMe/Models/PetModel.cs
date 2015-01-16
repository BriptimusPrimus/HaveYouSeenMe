using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using HaveYouSeenMe.Utilities;

namespace HaveYouSeenMe.Models
{
    public class PetModel : ITransformableViewModel
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

        public bool GetValues(Object obj) 
        {
            var pet = obj as Pet;
            
            //not expected type
            if (pet == null)
            {
                return false;
            }

            //obtain values 
            this.PetID = pet.PetID;
            this.PetName = pet.PetName;
            this.PetAgeYears = pet.PetAgeYears;
            this.PetAgeMonths = pet.PetAgeMonths;
            this.StatusID = pet.StatusID;
            this.LastSeenOn = pet.LastSeenOn;
            this.LastSeenWhere = pet.LastSeenWhere;
            this.Notes = pet.Notes;
            this.UserId = pet.UserId;

            return true;
        }

    }
}