using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HaveYouSeenMe.Models
{
    public class FullNameAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var nameComponents = value.ToString().Trim().Split(' ');
            return nameComponents.Length >= 2;
        }
    }
}