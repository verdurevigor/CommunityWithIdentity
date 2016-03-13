using EugeneCommunity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EugeneCommunity.DAL
{
    public class FakeMessageRepository : IMessageRepository
    {
        // This is the "database"
        private List<Message> messages;
        private List<Topic> topics;
        // Use this to simulate how the database auto-increments an id
        private int maxMessageId = 0;
        private int maxTopicId = 0;

        // Include this default constructor beause there is a parameterized constructor
        public FakeMessageRepository()
        {
            messages = new List<Message>();
            topics = new List<Topic>();
        }

        // This constructor serves to preload the List<Message> for ease of testing
        public FakeMessageRepository(List<Message> m, List<Topic> t)
        {
            messages = m;
            topics = t;
            // Set the id auto-increment simulator for messages
            foreach (Message next in messages)
            {
                if (next.MessageId > maxMessageId)
                    maxMessageId = next.MessageId;
            }
            // Set the id auto-increment simulator for topics
            foreach (Topic next in topics)
            {
                if (next.TopicId > maxTopicId)
                    maxTopicId = next.TopicId;
            }
        }

        public Message Create(Message message, int? CurrentTopics)
        {
            // Add message to "database" after giving it an id and return the message
            message.MessageId = ++maxMessageId;
            message.Topic = topics.Find(t => t.TopicId == CurrentTopics);
            messages.Add(message);
            return message;
        }

        public List<Message> GetAllMessages()
        {
            return messages;
        }
    }
}