using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EugeneCommunity.Models
{
    public class LogInViewModel
    {
        [Display(Name = "User Name")]
        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(16, MinimumLength = 2, ErrorMessage = "{0} must be between 2 and 16 characters.")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [HiddenInput(DisplayValue=false)]
        public string ReturnUrl { get; set; }
    }
}