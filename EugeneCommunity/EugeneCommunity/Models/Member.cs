using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EugeneCommunity.Models
{
    public class Member : IdentityUser
    {
        /* IdentityUser already has these properties
         * Email, EmailConfirmed, Id, PasswordHash, PhoneNumber, PhoneNumberConfirmed, UserName
         * Some other properties which offer navigation to more things are Claims and Roles.
         * */

        [StringLength(20, ErrorMessage = "{0} is only allowed to be at most 20 characters.")]
        [Display(Name="First Name")]
        public string FName { get; set; }

        [StringLength(41, ErrorMessage = "{0} must be less than 42 characters.")]
        public string State { get; set; }

        // TODO: use this annotation for the RegisterViewModel
        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(16, MinimumLength = 6, ErrorMessage = "{0} must be between 6 and 16 characters.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}