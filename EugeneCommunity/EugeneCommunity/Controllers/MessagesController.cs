using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EugeneCommunity.Models;

namespace EugeneCommunity.Controllers
{
    public class MessagesController : Controller
    {
        private EugeneCommunityContext db = new EugeneCommunityContext();

        // GET: Messages
        public ActionResult Index()
        {
            // Query db for list of messages, attaching user and subject to the message
            var messages = (from m in db.Messages
                            orderby m.Date
                            select new MessageViewModel
                            {
                                MessageId = m.MessageId,
                                Body = m.Body,
                                Date = m.Date,
                                Subject = (from t in db.Topics
                                           where m.TopicId == t.TopicId
                                           select t).FirstOrDefault(),
                                User = (from u in db.Members
                                        where m.MemberId == u.MemberId
                                        select u).FirstOrDefault()
                            }).ToList();
            // Order messages by most recent            Not sure if this is working...
            messages.OrderBy(m => m.Date);
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
                            where id == m.MessageId
                            select new MessageViewModel
                            {
                                MessageId = m.MessageId,
                                Body = m.Body,
                                Date = m.Date,
                                Subject = (from t in db.Topics
                                           where m.TopicId == t.TopicId
                                           select t).FirstOrDefault(),
                                User = (from u in db.Members
                                        where m.MemberId == u.MemberId
                                        select u).FirstOrDefault()
                            }).FirstOrDefault();
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

            // For now, send a SelectList of users for client to use as an identity
            ViewBag.CurrentUsers = new SelectList(db.Members.OrderBy(m => m.UserName), "MemberId", "UserName");
            return View();
        }

        // POST: Messages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MessageId,Title,Body,Date,TopicId,MemberId")] MessageViewModel messageVm, int? CurrentTopics, int? CurrentUsers)
        {           
            if (ModelState.IsValid)
            {
                // Check if the topic is new, if so create and save a new topic to the database before adding the TopicId to the new message
                Topic topic = (from t in db.Topics
                               where t.TopicId == CurrentTopics
                               select t).FirstOrDefault();
                
                if (topic == null)
                {
                    topic = new Topic() { Title = messageVm.Subject.Title };
                    db.Topics.Add(topic);
                    db.SaveChanges();
                }

                // Using the MessageViewModel input, create a message object
                Message message = new Message()
                {
                    Body = messageVm.Body,
                    Date = DateTime.Now,
                    MemberId = (int)CurrentUsers,       // Cast possibly null int, app will still crash if it is not filled when form is submitted. TODO: modify when Member is stored in session
                    TopicId = topic.TopicId
                };

                // Add and save Message to db
                db.Messages.Add(message);
                db.SaveChanges();

                // Redirect user to the forum topic which they just added a message to
                return RedirectToAction("Details", "Topics", new { id = topic.TopicId });
            }
            // If form is not completed properly, repopulate the dropdownlist for members and topics
            ViewBag.CurrentUsers = new SelectList(db.Members.OrderBy(m => m.UserName), "MemberId", "UserName");
            ViewBag.CurrentTopics = new SelectList(db.Topics.OrderBy(s => s.Title), "TopicId", "Title");


            return View(messageVm);
        }

        // GET: Messages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Create MessageViewModel from the MessageId to pass to the view
            MessageViewModel message = (from m in db.Messages
                                        where m.MessageId == id
                                        select new MessageViewModel(){
                                            MessageId = m.MessageId,
                                            Body = m.Body,
                                            //Date = m.Date, Date is not required to present on the View so take it out.
                                            Subject = (from t in db.Topics
                                                       where m.TopicId == t.TopicId
                                                       select t).FirstOrDefault(),
                                            User = (from u in db.Members
                                                    where m.MemberId == u.MemberId
                                                    select u).FirstOrDefault()
                                        }).FirstOrDefault();
            if (message == null)
            {
                // Redirect bad user to error page and let them suffer!
                return Redirect("/Error");
            }
            // Create a SelectList to pass the subject to the View; final parameter gives the default value to show in view.
            ViewBag.CurrentTopics = new SelectList(db.Topics.OrderBy(s => s.Title), "TopicId", "Title", message.Subject.TopicId);

            // For now, send a SelectList of users for client to use as an identity
            ViewBag.CurrentUsers = new SelectList(db.Members.OrderBy(m => m.UserName), "MemberId", "UserName");
            return View(message);
        }

        // POST: Messages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MessageId,Body,Date,Subject,User")] MessageViewModel messageVm, int? CurrentUsers, int? CurrentTopics)
        {
            if (ModelState.IsValid && CurrentTopics != null)
            {
                Message message = (from m in db.Messages
                             where m.MessageId == messageVm.MessageId
                             select m).FirstOrDefault();

                if (message.MemberId == CurrentUsers)
                {
                    // Update content of message with MessageViewModel passed from View
                    message.TopicId = (int)CurrentTopics;
                    message.Body = messageVm.Body;
                    message.Date = DateTime.Now;

                    db.Entry(message).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else//Not same user who posted
                {
                    // Redirect bad user to error page and let them suffer!
                    return Redirect("/Error");
                }
            }// Invalid ModelState
            // Create a SelectList to pass the subject to the View; final parameter gives the default value to show in view.
            ViewBag.CurrentTopics = new SelectList(db.Topics.OrderBy(s => s.Title), "TopicId", "Title");    // Because a topic wasn't associated with the MessageView object a topicId cannot be preselected after going through the POST Edit method...

            // For now, send a SelectList of users for client to use as an identity
            ViewBag.CurrentUsers = new SelectList(db.Members.OrderBy(m => m.UserName), "MemberId", "UserName");
            return View(messageVm);
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
                           where id == m.MessageId
                           select new MessageViewModel
                           {
                               MessageId = m.MessageId,
                               Body = m.Body,
                               Date = m.Date,
                               Subject = (from t in db.Topics
                                          where m.TopicId == t.TopicId
                                          select t).FirstOrDefault(),
                               User = (from u in db.Members
                                       where m.MemberId == u.MemberId
                                       select u).FirstOrDefault()
                           }).FirstOrDefault();
            if (message == null)
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
            db.Messages.Remove(message);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Books/Search
        public ActionResult Search()
        {
            return View();
        }

        // POST: Books/Search/searchString
        [HttpPost]
        public ActionResult Search(string searchTerm)
        {
            // Get the messages that matches the searchTerm
            var messageVms = (from m in db.Messages
                              where m.Body.Contains(searchTerm)
                                select new MessageViewModel()
                                {
                                    MessageId = m.MessageId,
                                    Body = m.Body,
                                    Date = m.Date,
                                    Subject = (from t in db.Topics
                                            where m.TopicId == t.TopicId
                                            select t).FirstOrDefault(),
                                    User = (from u in db.Members
                                            where u.MemberId == m.MessageId
                                            select u).FirstOrDefault()
                                }).ToList();
            //  Return the search term to display to user
            ViewBag.SearchTerm = searchTerm;
            return View("Search", messageVms);

            // Partial Views are the perfect thing to do when trying to show the search result page based on the index. But for now just make a new View for the return.
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
