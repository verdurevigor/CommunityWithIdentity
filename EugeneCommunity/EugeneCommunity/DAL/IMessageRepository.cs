using EugeneCommunity.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EugeneCommunity.DAL
{
    public interface IMessageRepository
    {

        Message Create(Message message, int? CurrentTopics);
        List<Message> GetAllMessages();
    }
}