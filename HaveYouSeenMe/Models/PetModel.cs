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
        [Required(ErrorMessage = "Please type your pet name")]
        [StringLength(100, ErrorMessage = "You can only add up to 100 characters")]
        public string PetName { get; set; }

        [Display(Name = "Years:")]
        [Range(0, int.MaxValue, ErrorMessage = "The value must be 0 or greater than 0")]
        public int? PetAgeYears { get; set; }

        [Display(Name = "Months:")]
        [Range(0, int.MaxValue, ErrorMessage = "The value must be 0 or greater than 0")]
        public int? PetAgeMonths { get; set; }

        [Display(Name = "Status ID:")]
        public int StatusID { get; set; }

        [Display(Name = "Last Seen On:")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public System.DateTime? LastSeenOn { get; set; }

        [Display(Name = "Last Seen At:", Prompt = "testing a prompt")]
        public string LastSeenWhere { get; set; }

        [Display(Name = "Notes:")]
        [StringLength(400, ErrorMessage = "You can only add up to 400 characters")]
        [DataType(DataType.MultilineText)]
        public string Notes { get; set; }
        
        public int UserId { get; set; }

        [Display(Name = "Status:")]
        public string StatusDescription { get; set; }

        [Display(Name = "Owner:")]
        public string OwnerUserName { get; set; }

        [Display(Name = "Age(Years-Months):")]
        public string PetAge 
        {
            get 
            {
                return this.PetAgeYears + " - " + this.PetAgeMonths;
            } 
        }

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

        public Pet ToDataModel()
        {
            //transform to original data object 
            return new Pet 
            {
                PetID = this.PetID,
                PetName = this.PetName,
                PetAgeYears = this.PetAgeYears,
                PetAgeMonths = this.PetAgeMonths,
                StatusID = this.StatusID,
                LastSeenOn = this.LastSeenOn,
                LastSeenWhere = this.LastSeenWhere,
                Notes = this.Notes,
                UserId = this.UserId        
            };            
        }

    }
}