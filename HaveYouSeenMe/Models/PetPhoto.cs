//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HaveYouSeenMe.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public partial class PetPhoto
    {
        [Key]
        public int PhotoID { get; set; }
        public int PetID { get; set; }
        public string Photo { get; set; }
        public string Notes { get; set; }
    
        public virtual Pet Pet { get; set; }
    }
}
