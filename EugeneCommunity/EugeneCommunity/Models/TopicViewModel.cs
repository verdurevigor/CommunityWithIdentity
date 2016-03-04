using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EugeneCommunity.Models
{
    // Note: The LastPostDate property must be accessed only after the Messages property has been set.
    public class TopicViewModel
    {
        // These variable assist with easily retrieving the last post time.
        DateTime lastPostDate = new DateTime();
        List<Message> posts = new List<Message>();

        // Using virtual keyword is NOT necessary (or practical) with ViewModels! Ok in domain model though.
        // Adding virtual means the propety isn't instantiated/existent until it is set.
        public int TopicId { get; set; }
        public string Title { get; set; }

        [Display(Name = "Last Post")]
        public DateTime LastPostDate {
            get 
            {/*
                if (Messages.Count == 0)
                    throw new Exception("The property LastPost cannot be accessed until the Messages property has been set");
                else
                {*/
                    foreach (Message p in posts)
                    {
                        if (lastPostDate < p.Date)
                            lastPostDate = p.Date;
                    }
                    return lastPostDate; 
                //}     
            }
        }

        public List<Message> Messages
        {
            get { return posts; }
            set { posts = value; }
        }
    }
}