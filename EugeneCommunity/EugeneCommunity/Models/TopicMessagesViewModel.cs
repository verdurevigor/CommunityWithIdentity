using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EugeneCommunity.Models
{
    public class TopicMessagesViewModel
    {
        List<MessageMemberViewModel> posts = new List<MessageMemberViewModel>();

        public int TopicId { get; set; }
        public string Title { get; set; }
        public List<MessageMemberViewModel> Posts { get; set; }
    }
}