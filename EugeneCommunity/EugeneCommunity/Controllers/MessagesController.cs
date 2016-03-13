using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EugeneCommunity.Models;
using Microsoft.AspNet.Identity;
using EugeneCommunity.DAL;  // Data access layer for unit testing

namespace EugeneCommunity.Controllers
{
    public class MessagesController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // repo is a fake DbContext to be used when unit testing
        private IMessageRepository repo;

        public MessagesController()
        {
            repo = new FakeMessageRepository();
        }

        public MessagesController(IMessageRepository m)
        {
            repo = m;
        }

        // GET: Messages
        public ActionResult Index()
        {
            /* Original
            // Get list of messages where the body contains searchTerm, add to the Messages the associated Topic and Memebr
            var messages = db.Messages.Include("Topic").Include("Member").ToList();
            messages.OrderBy(m => m.Date);
            return View(messages);
             * */

            // For unit testing
            var messages = repo.GetAllMessages();
            return View(messages);
        }

        // GET: Messages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Query db for message matching id parameter and include Member and Topic

            var message = (from m in db.Messages
                           orderby m.Date
                           join t in db.Topics on m.Topic equals t
                           join u in db.Users on m.Member equals u
                           select m).FirstOrDefault();
            
            if (message == null)
            {
                // Redirect bad user to error page and let them suffer!
                return Redirect("/Error");
            }
            return View(message);
        }

        // GET: Messages/Create
        public ActionResult Create(int? topicId)
        {
            // Preselect the topic in the SelectList if client came from a Topic Details view.
            if (topicId != null)
               ViewBag.CurrentTopics = new SelectList(db.Topics.OrderBy(s => s.Title), "TopicId", "Title", topicId);
            else
                ViewBag.CurrentTopics = new SelectList(db.Topics.OrderBy(s => s.Title), "TopicId", "Title");

            return View();
        }

        // POST: Messages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MessageId,Title,Body,Date,TopicId")] Message message, int? CurrentTopics)
        {
            /* Original
            if (ModelState.IsValid)
            {
                // Check if the topic is new, if so create and save a new topic to the database before adding the TopicId to the new message
                Topic topic = (from t in db.Topics
                               where t.TopicId == CurrentTopics
                               select t).FirstOrDefault();
                
                if (topic == null)
                {
                    topic = new Topic() { Title = message.Topic.Title };
                    db.Topics.Add(topic);
                    db.SaveChanges();
                }

                // Create a message object
                Message m = new Message()
                {
                    Body = message.Body,
                    Date = DateTime.Now,
                    Member = db.Users.Find(User.Identity.GetUserId()),
                    Topic = topic
                };

                // Add and save m to db
                db.Messages.Add(m);
                db.SaveChanges();

                // Redirect user to the forum topic which they just added a message to
                return RedirectToAction("Details", "Topics", new { id = topic.TopicId });
            }
            // If form is not completed properly, repopulate the dropdownlist for members and topics
            ViewBag.CurrentUsers = new SelectList(db.Users.OrderBy(m => m.UserName), "MemberId", "UserName");
            ViewBag.CurrentTopics = new SelectList(db.Topics.OrderBy(s => s.Title), "TopicId", "Title");
            return View(message);
             * */

            // For unit testing
            repo.Create(message, CurrentTopics);
            // Return to Index page where all Messages are seen
            // Because return RedirectToAction("Index") was returning and not redirecting, the UnitTest was failing
            // For that reason, I simply mimiced the Index() action body content here.
            var messages = repo.GetAllMessages();
            return View(messages);
        }

        // GET: Messages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Create MessageViewModel from the MessageId to pass to the view

            var message = (from m in db.Messages
                           join t in db.Topics on m.Topic equals t
                           join u in db.Users on m.Member equals u
                           select m).FirstOrDefault();

            if (message == null)
            {
                // Redirect bad user to error page and let them suffer!
                return Redirect("/Error");
            }
            // Ensure that current user is owner
            if (message.Member != db.Users.Find(User.Identity.GetUserId()))
            {
                // Redirect bad user to error page and let them suffer!
                return Redirect("/Error");
            }
            // Create a SelectList to pass the subject to the View; final parameter gives the default value to show in view.
            ViewBag.CurrentTopics = new SelectList(db.Topics.OrderBy(s => s.Title), "TopicId", "Title", message.Topic.TopicId);

            return View(message);
        }

        // POST: Messages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MessageId,Body,Date,Subject,User")] Message message, int? CurrentTopics)
        {
            if (ModelState.IsValid && CurrentTopics != null)
            {
                Message updatedMessage = db.Messages.Find(message.MessageId);

                Topic topic = db.Topics.Find(CurrentTopics);

                if (message.Member == db.Users.Find(User.Identity.GetUserId()))
                {
                    // Update content of message
                    updatedMessage.Topic = topic;
                    updatedMessage.Body = message.Body;
                    updatedMessage.Date = DateTime.Now;

                    db.Entry(updatedMessage).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Details", "Topics", topic.TopicId);
                }
                else//Not same user who posted
                {
                    // Redirect bad user to error page and let them suffer!
                    return Redirect("/Error");
                }
            }// Invalid ModelState
            // Create a SelectList to pass the subject to the View; final parameter gives the default value to show in view.
            ViewBag.CurrentTopics = new SelectList(db.Topics.OrderBy(s => s.Title), "TopicId", "Title", CurrentTopics); // TODO: the last parameter might need to be changed to message.Topic.TopicId

            return View(message);
        }

        // GET: Messages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Query db for message matching id parameter and include Member and Topic to create a full MessageViewModel

            var message = (from m in db.Messages
                           join t in db.Topics on m.Topic equals t
                           join u in db.Users on m.Member equals u
                           select m).FirstOrDefault();

            if (message == null)
            {
                // Redirect bad user to error page and let them suffer!
                return Redirect("/Error");
            }
            // Ensure that current user is owner
            if (message.Member != db.Users.Find(User.Identity.GetUserId()))
            {
                // Redirect bad user to error page and let them suffer!
                return Redirect("/Error");
            }
            return View(message);
        }

        // POST: Messages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Message message = db.Messages.Find(id);
            if (message.Member == db.Users.Find(User.Identity.GetUserId()))
            {
                db.Messages.Remove(message);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return Redirect("/Error");
        }

        // GET: Books/Search
        public ActionResult Search()
        {
            return View();
        }

        // POST: Messages/Search/searchString
        [HttpPost]
        public ActionResult Search(string searchTerm)
        {
            // Get list of messages where the body contains searchTerm, add to the Messages the associated Topic and Memebr
            var messages = db.Messages.Include("Topic").Include("Member").Where(m => m.Body.Contains(searchTerm)).ToList();
            //  Return the search term to display to user
            ViewBag.SearchTerm = searchTerm;
            return View("Search", messages);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
