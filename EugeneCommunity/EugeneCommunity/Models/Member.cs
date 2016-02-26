using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EugeneCommunity.Models
{
    public class Member
    {
        [Key]
        public virtual int MemberId { get; set; }
        [RegularExpression(@"^[A-Za-z0-9._-]{3,32}$", ErrorMessage = "A valid {0} must be alphnumeric, but may contain . _ - characters.")]
        [Display(Name = "User Name")]
        public virtual string UserName { get; set; }
        [Required(ErrorMessage = "{0} is required.")]
        //[RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9._%+-]+\.[A-Za-z]{2,4}", ErrorMessage = "Please enter a valid {0}.")]
        [DataType(DataType.EmailAddress)]
        public virtual string Email { get; set; }
        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(16, MinimumLength = 6, ErrorMessage = "{0} must be between 6 and 16 characters.")]
        [DataType(DataType.Password)]
        public virtual string Password { get; set; }
        public virtual bool IsAdmin { get; set; }
    }
}