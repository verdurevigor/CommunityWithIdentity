using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EugeneCommunity.Models
{
    public class Message
    {
        [Key]
        public virtual int MessageId { get; set; }
        [Required(ErrorMessage = "The {0} of the message is required.")]
        public virtual string Body { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}")]
        public virtual DateTime Date { get; set; }
        // FK to Topic
        public virtual Topic Topic { get; set; }
        // FK to Member
        public virtual Member Member { get; set; }
    }
}