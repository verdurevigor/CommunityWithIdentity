using EugeneCommunity.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EugeneCommunity.Controllers
{
    // By default authentication is required
    [Authorize(Roles="Admin,Moderator")]
    public class AuthorizationController : Controller
    {
        // Private instance to access database
        private AppDbContext db = new AppDbContext();
        // Private instance of user manager gives access to identity
        UserManager<Member> userManager = new UserManager<Member>(
            new UserStore<Member>(new AppDbContext()));

        // This action presents a page for navigating to the different types of authorized tasks - Roles and Moderating
        // GET: Authorization
        public ActionResult Index()
        {
            return View();
        }
        
        // Manage and Assign Roles
        #region
        // Get: Authorization/ManageRole
        [Authorize(Roles="Admin")]
        public ActionResult ManageRoles()
        {
            // prepopulate roles for the view's dropdown
            var list = PrePopulateRoleList();
            
            ViewBag.Roles = list;
            return View();
        }

        // GET: Authorization/ListRoles
        [Authorize(Roles="Admin")]
        public ActionResult ListRoles()
        {
            var roles = db.Roles.ToList();
            return View(roles);
        }

        // GET: Authorization/DeleteRole?RoleName=
        [Authorize(Roles="Admin")]
        public ActionResult DeleteRole(string RoleName)
        {
            var thisRole = db.Roles.Where(r => r.Name.Equals(RoleName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            db.Roles.Remove(thisRole);
            db.SaveChanges();

            return RedirectToAction("ListRoles");
        }

        //
        // GET: /Roles/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult EditRole(string roleName)
        {
            var thisRole = db.Roles.Where(r => r.Name.Equals(roleName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

            return View(thisRole);
        }

        //
        // POST: /Roles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult EditRole(Microsoft.AspNet.Identity.EntityFramework.IdentityRole role)
        {
            try
            {
                db.Entry(role).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("ListRoles");
            }
            catch
            {
                return RedirectToAction("ListRoles");
            }
        }

        // GET: Authorization/CreateRole
        [Authorize(Roles="Admin")]
        public ActionResult CreateRole()
        {
            return View();
        }

        //
        // POST: /Authorization/CreateRole
        // This is a new way of getting information back from a form! Look at the method parameter, and how collection variable is used.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult CreateRole(FormCollection collection)
        {
            try
            {
                // Add a new role to IdentityRole using this initialization list
                // IdentityRole comes from Microsoft.AspNet.Identity.EntityFramework
                db.Roles.Add(new IdentityRole()
                {
                    Name = collection["RoleName"]
                });
                db.SaveChanges();
                ViewBag.ResultMessage = "Role " + collection["RoleName"] + " was added successfully.";
                return View();
            }
            catch
            {
                ViewBag.ResultMessage = "Failed to create role " + collection["RoleName"];
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult GetRoles(string UserName)
        {
            if (!string.IsNullOrWhiteSpace(UserName))
            {
                Member user = db.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

                ViewBag.RolesForThisUser = userManager.GetRoles(user.Id);

                // prepopulat roles for the view dropdown
                var list = PrePopulateRoleList();
                
                ViewBag.Roles = list;
                ViewBag.username = user.UserName;
            }
            return View("ManageRoles");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult AddRoleToUser(string UserName, string RoleName)
        {
            Member user = db.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            userManager.AddToRole(user.Id, RoleName);

            ViewBag.ResultMessage = "Role created successfully for " + UserName + "!";

            // prepopulat roles for the view dropdown
            var list = PrePopulateRoleList();
            ViewBag.Roles = list;

            return View("ManageRoles");
        }

        // POST: Authorization/DeleteRoleForUser/Username&RoleName
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteRoleForUser(string UserName, string RoleName)
        {
            Member user = db.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

            if (userManager.IsInRole(user.Id, RoleName))
            {
                userManager.RemoveFromRole(user.Id, RoleName);
                ViewBag.ResultMessage = RoleName + " removed from " + UserName + " successfully!";
            }
            else
            {
                ViewBag.ResultMessage = UserName + " doesn't belong to " + RoleName + ".";
            }
            // prepopulat roles for the view dropdown
            var list = PrePopulateRoleList();
            ViewBag.Roles = list;

            return View("ManageRoles");
        }
        #endregion

        // Delete Members
        #region

        // GET: Authorization/ManageMembers
        [Authorize(Roles="Admin")]
        public ActionResult ManageMembers()
        {
            return View();
        }

        // POST: Authorization/SearchMembers/searchString
        [HttpPost]
        [Authorize(Roles="Admin")]
        public ActionResult SearchMembers(string nameSearch)
        {

            // Get members that matches the searchName. Be sure to conduct a case-insensitive search.
            var members = db.Users.ToList().Where(n => n.UserName.ToLower().Contains(nameSearch.ToLower()));

            //  Return the search name to display to user
            ViewBag.NameSeach = nameSearch;
            if (members.Count() == 0)
                ViewBag.ResultMessage = "No members found.";
            return View("ManageMembers", members);
        }

        //
        // GET: Authorization/DeleteMember/id
        [Authorize(Roles="Admin")]
        public ActionResult DeleteMember(string id)
        {
            try
            {
                var member = (from u in db.Users where u.Id == id select u).FirstOrDefault();
                //userManager.Delete(member);
                db.Users.Remove(member);
                ViewBag.ResultMessage = "Member " + member.UserName + " was successfully deleted";
                db.SaveChanges();
                return View("ManageMembers");
            }
            catch
            {
                ViewBag.ResultMessage = "Member was not deleted.";
                return View("ManageMembers");
            }
            
        }

        #endregion

        // GET: Authorization/ManageMembers
        [Authorize(Roles = "Admin,Moderator")]
        public ActionResult ManageMessages()
        {
            return View();
        }

        // POST: Authorization/SearchMessages/searchString
        [HttpPost]
        [Authorize(Roles = "Admin,Moderator")]
        public ActionResult SearchMessages(string searchTerm)
        {
            // Get the messages that matches the searchTerm
            
            /*var messageVms = (from m in db.Messages
                              where m.Body.Contains(searchTerm)
                              select new MessageViewModel()
                              {
                                  MessageId = m.MessageId,
                                  Body = m.Body,
                                  Date = m.Date,
                                  Subject = (from t in db.Topics
                                             where m.TopicId == t.TopicId
                                             select t).FirstOrDefault(),
                                  Memb = (from u in db.Users
                                          where m.MemberId == u.Id
                                          select u).FirstOrDefault()
                              }).ToList();
            */
            var messages = (from m in db.Messages
                           where m.Body.Contains(searchTerm)
                           join t in db.Topics on m.Topic equals t
                           join u in db.Users on m.Member equals u
                           select m).ToList();

            //  Return the search term to display to user
            ViewBag.SearchTerm = searchTerm;
            return View("ManageMessages", messages);
        }

        //
        // GET: Authorization/DeleteMessage/id
        [Authorize(Roles = "Admin,Moderator")]
        public ActionResult DeleteMessage(int id)
        {
            try
            {
                var message = (from m in db.Messages where m.MessageId == id select m).FirstOrDefault();
                
                db.Messages.Remove(message);
                ViewBag.ResultMessage = "Message '" + message.Body.Substring(0, 20) + "...' was successfully deleted";
                db.SaveChanges();
                return View("ManageMessages");
            }
            catch
            {
                ViewBag.ResultMessage = "Message was not deleted.";
                return View("ManageMessages");
            }

        }
        
        // Method is used to repopulate roles for the ManageRole view's dropdown
        private List<SelectListItem> PrePopulateRoleList()
        {
            
            var list = db.Roles.OrderBy(r => r.Name).ToList().Select(rr =>
            new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();

            return list;
        }
    }
}