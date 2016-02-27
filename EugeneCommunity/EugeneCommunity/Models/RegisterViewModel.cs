using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EugeneCommunity.Models
{
    public class RegisterViewModel
    {
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        [Required(ErrorMessage = "{0} is required.")]        
        public string Email { get; set; }

        [Display(Name = "User Name")]
        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(16, MinimumLength = 2, ErrorMessage = "{0} must be between 2 and 16 characters.")]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "{0} is required.")]        
        [StringLength(16, MinimumLength = 6, ErrorMessage = "{0} must be between 6 and 16 characters.")]
        public string Password { get; set; }
        
        [Compare("Password")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmation Password")]
        [Required(ErrorMessage = "{0} is required.")]
        public string PasswordConfirmed { get; set; }

        [StringLength(41, ErrorMessage = "{0} must be less than 42 characters.")]
        public string State { get; set; }
    }
}