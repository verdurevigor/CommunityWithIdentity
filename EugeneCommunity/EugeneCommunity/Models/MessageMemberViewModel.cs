using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EugeneCommunity.Models
{
    public class MessageMemberViewModel
    {
        DateTime date = new DateTime();

        public int MessageId { get; set; }
        public string MemberId { get; set; }
        public string UserName { get; set; }
        public string Body { get; set; }

        public DateTime Date
        { get { return date; } set { date = value; } }
        [DisplayFormat(DataFormatString = "{0:HH:mm}")]
        public DateTime Time
        { get { return date; } }
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime DateYear
        { get { return date; } }

    }
}