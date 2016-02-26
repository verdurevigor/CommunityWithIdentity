using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EugeneCommunity.Models
{
    public class TopicViewModel
    {
        List<Message> posts = new List<Message>();
        // Using virtual keyword is NOT necessary (or practical) with ViewModels! Ok in domain model though.
        // Adding virtual means the propety isn't instantiated/existent until it is set.
        public int TopicId { get; set; }
        public string Title { get; set; }
        // This property should be set by querying the posts for the most recent dated message
        [Display(Name = "Last Post")]
        public DateTime LastPost { get; set; }

        public List<Message> Posts
        {
            get { return posts; }
            set { posts = value; }
        }
    }
}