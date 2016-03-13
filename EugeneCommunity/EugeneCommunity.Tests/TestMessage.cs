using EugeneCommunity.Controllers;
using EugeneCommunity.DAL;
using EugeneCommunity.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EugeneCommunity.Tests
{
    [TestClass]
    public class TestMessage
    {
        // Two topics to use in testing
        List<Topic> topics = new List<Topic>() {
            new Topic() { TopicId = 1, Title = "Street Art" },
            new Topic() { TopicId = 2, Title = "Ice Cream" }
        };
        // Begin test with one message
        List<Message> messages = new List<Message>() {
            new Message() {MessageId = 1, Topic = new Topic() { TopicId = 1, Title = "Street Art" }, 
                Body = "The alley on Broadway St. between Charnelton and Olive is covered with a wall of contemparary painting.", 
                Date = DateTime.Now }
        };

        [TestMethod]
        public void IndexTest()
        {
            // arrange
            var repo = new FakeMessageRepository(messages, topics);
            var target = new MessagesController(repo);

            // act
            var result = (ViewResult)target.Index();

            // assert
            var models = (List<Message>)result.Model;
                // ensuring gathered correct amount
            Assert.AreEqual(messages.Count, models.Count);
                // ensure gathered expected messages
            Assert.AreEqual(messages[0].MessageId, models[0].MessageId);
        }

        [TestMethod]
        public void CreateTest()
        {
            // arrange
            var repo = new FakeMessageRepository(messages, topics);
            var target = new MessagesController(repo);


            // act
            Message message = new Message() { Body = "Red Wagon creamery is hands down to thickest, most delicious hand made ice cream in town.", 
                                              Date = DateTime.Now };
            // Simulate selecting Topic's Id from selectlist
            int CurrentTopics = topics[1].TopicId;
            // Message will be created and the MessagesController Index view will be returned
            var result = (ViewResult)target.Create(message, CurrentTopics);


            // assert
            var models = (List<Message>)result.Model;
            Assert.AreEqual(messages.Count, models.Count);
            Assert.AreEqual(messages[1].Body, models[1].Body);

        }

    }
}
