using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EugeneCommunity.Models
{
    public class Topic
    {
        private List<Message> posts = new List<Message>();
        [Key]
        public virtual int TopicId { get; set; }
        [Required]
        [Display(Name="Topic Title")]
        [StringLength(160, MinimumLength=2, ErrorMessage="{0} must be between 2 and 160 characters.")]
        public virtual string Title { get; set; } 
        public List<Message> Posts
        {
            get { return posts; }
        }
    }
}