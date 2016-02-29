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

namespace EugeneCommunity.Controllers
{
    public class TopicsController : Controller
    {
        //private EugeneCommunityContext db = new EugeneCommunityContext();
        AppDbContext db = new AppDbContext();

        // GET: Topics
        public ActionResult Index()
        {
            // List of TopicViewModels
            var topic = (from t in db.Topics
                        select new TopicViewModel   // Unnecessary to get TopicViewModel unless I am to count posts or order by post date
                        {
                            TopicId = t.TopicId,
                            Title = t.Title,
                            // Add list of messages
                            Posts = (from p in db.Messages
                                     where t.TopicId == p.TopicId
                                     select p).ToList(),
                            // Date topic by most recent post for proper ordering
                            LastPost = (from p in db.Messages
                                        where p.TopicId == t.TopicId
                                        orderby p.Date
                                        select p.Date).FirstOrDefault()
                        }).ToList();
            topic.OrderBy(t => t.LastPost);     // TODO: this ordering function is not creating correct output...
            return View(topic);
        }

        // GET: Topics/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Use linq query to retrieve the topic of given id. Retrieve list of posts on that topic (posts with that topic's fk).
            // Set the topics posts with the retrieved list of messages.
            // Then pass this topic with list of messages to the View.

            TopicMessagesViewModel topic = new TopicMessagesViewModel();

            topic = (from t in db.Topics
                     where t.TopicId == id
                     select new TopicMessagesViewModel
                     {
                         TopicId = t.TopicId,
                         Title = t.Title,
                         Posts = (from p in db.Messages
                                  join u in db.Users on p.MemberId equals u.Id
                                  where p.TopicId == t.TopicId
                                  select new MessageMemberViewModel
                                  {
                                      MessageId = p.MessageId,
                                      MemberId = u.Id,
                                      Date = p.Date,
                                      UserName = u.UserName,
                                      Body = p.Body,                               
                                  }).ToList()
                     }).FirstOrDefault();

            if (topic == null)
            {
                return HttpNotFound();
            }
            return View(topic);
        }

        // GET: Topics/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Topics/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MessageId,Body,Date,Subject,User")] MessageViewModel messageVm)
        {
            // Verify form has appropriate data and that a user has been marked
            if (ModelState.IsValid)
            {
                // Add new Topic to db, then create a message under that topic

                Topic topic = new Topic() { Title = messageVm.Subject.Title };
                db.Topics.Add(topic);
                db.SaveChanges();

                Message message = new Message() { Body = messageVm.Body, Date = DateTime.Now, MemberId = User.Identity.GetUserId(), TopicId = topic.TopicId };
                db.Messages.Add(message);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            // If form is invalid repopulate form with sent data, also repopulate the user dropdownlist
            ViewBag.CurrentUsers = new SelectList(db.Users.OrderBy(m => m.UserName), "MemberId", "UserName");
            return View(messageVm);
        }

        // GET: Topics/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topic topic = db.Topics.Find(id);
            if (topic == null)
            {
                return HttpNotFound();
            }
            return View(topic);
        }

        // POST: Topics/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TopicId,Title")] Topic topic)
        {
            if (ModelState.IsValid)
            {
                db.Entry(topic).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(topic);
        }

        // GET: Topics/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topic topic = db.Topics.Find(id);
            if (topic == null)
            {
                return HttpNotFound();
            }
            return View(topic);
        }

        // POST: Topics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Topic topic = db.Topics.Find(id);
            db.Topics.Remove(topic);
            db.SaveChanges();
            return RedirectToAction("Index");
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
