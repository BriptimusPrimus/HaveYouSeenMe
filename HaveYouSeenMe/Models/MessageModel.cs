using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HaveYouSeenMe.Utilities;

namespace HaveYouSeenMe.Models
{
    public class MessageModel : ITransformableViewModel
    {
        [Display(Name = "Message ID Number:")]
        public int MessageID { get; set; }

        [Required(ErrorMessage = "Please type your name")]
        [StringLength(150, ErrorMessage = "You can only add up to 150 characters")]
        [FullName(ErrorMessage = "Please type your full name")]
        public string From { get; set; }

        [Required(ErrorMessage = "Please type your email address ")]
        [StringLength(150, ErrorMessage = "You can only add up to 150 characters")]
        [EmailAddress(ErrorMessage = "We don't recognize this as a valid email address")]
        public string Email { get; set; }

        [StringLength(150, ErrorMessage = "You can only add up to 150 characters")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Please type your message")]
        [StringLength(1500, ErrorMessage = "You can only add up to 1500 characters")]
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string Message { get; set; }

        [Required(ErrorMessage = "Could not find pet owner")]
        public string PetName { get; set; }

        public string To { get; set; }

        public int UserID { get; set; }

        [Display(Name = "Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        [DataType(DataType.Date)]
        public DateTime MessageDate { get; set; }

        [Display(Name = "From")]
        public string Sender 
        {
            get 
            {
                return this.From + "(" + this.Email + ")";
            } 
        }

        public bool GetValues(object obj)
        {
            var message = obj as Message;

            //not expected type
            if (message == null)
            {
                return false;
            }

            //obtain values
            this.MessageID = message.MessageID;
            this.From = message.From;
            this.Email = message.Email;
            this.Subject = message.Subject;
            this.Message = message.Message1;
            this.To = message.UserProfile.Email;
            this.UserID = message.UserId;
            this.MessageDate = message.MessageDate;

            return true;
        }

        public Message ToDataModel()
        {
            //transform to original data object 
            return new Message
            {
                MessageID = this.MessageID,
                UserId = this.UserID,
                From = this.From,
                Email = this.Email,
                Subject = this.Subject,
                Message1 = this.Message
            };
        }
    }
}