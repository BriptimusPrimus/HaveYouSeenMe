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
        [Display(Name = "Pet ID Number:")]
        public int PetID { get; set; }

        [Display(Name = "Name:")]
        public string PetName { get; set; }

        [Display(Name = "Years:")]
        public int? PetAgeYears { get; set; }

        [Display(Name = "Months:")]
        public int? PetAgeMonths { get; set; }

        [Display(Name = "Status ID:")]
        public int StatusID { get; set; }

        [Display(Name = "Last Seen On:")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = false)]
        public System.DateTime? LastSeenOn { get; set; }

        [Display(Name = "Last Seen At:", Prompt = "shalalalala")]
        public string LastSeenWhere { get; set; }

        [Display(Name = "Notes:")]
        public string Notes { get; set; }
        
        public int UserId { get; set; }

        [Display(Name = "Status:")]
        public string StatusDescription { get; set; }

        [Display(Name = "Owner:")]
        public string OwnerUserName { get; set; }

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
            this.StatusDescription = pet.Status.Description;
            this.OwnerUserName = pet.UserProfile.UserName;

            return true;
        }

    }
}