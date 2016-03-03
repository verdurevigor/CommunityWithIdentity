using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace EugeneCommunity.Models
{
    public class CommunityInitializer : DropCreateDatabaseAlways<AppDbContext>
    {
         protected override void Seed(AppDbContext context)
        {
             // Add Users to the database
             
             // First create a UserManager object in order to add a new Identity
             var userManager = new UserManager<Member>(
                 new UserStore<Member>(
                     new AppDbContext()));

             AppDbContext db = new AppDbContext();
             
             // Initialize some users
             Member m1 = new Member()
             {
                 Email = "brody@email.com",
                 UserName = "Brodster"
             };
              
             Member m2 = new Member()
             {
                 Email = "admin@email.com",
                 UserName = "Admin"
             };

             Member m3 = new Member()
             {
                 Email = "moderator@email.com",
                 UserName = "Moderator"
             };

             Member m4 = new Member()
             {
                 Email = "zach@email.com",
                 UserName = "Zach"
             };
             /*
             // Add users with userManager
             userManager.Create(m1, "password");
             userManager.Create(m2, "password");
             userManager.Create(m3, "password");
             userManager.Create(m4, "password");

             db.Roles.Add(new IdentityRole() { Name = "Admin" });
             db.Roles.Add(new IdentityRole() {Name = "Moderator"});
             userManager.AddToRole(m2.Id, "Admin");
             userManager.AddToRole(m3.Id, "Moderator");
             */

             // Attempting to seed Members in database like Lonnie's example from the forum
             var members = new List<Member>() { m1, m2, m3, m4 };
             string psswd = "password";
             foreach (var member in members)
             {
                 var result = userManager.Create(member, psswd);
                 
                 if (result.Succeeded)
                 {
                     var identity = userManager.CreateIdentity(
                         member, DefaultAuthenticationTypes.ApplicationCookie);
                     //log them IN
                     HttpContext.Current.GetOwinContext().Authentication.SignIn(identity); 
                     //and right back OUT
                     HttpContext.Current.GetOwinContext().Authentication.SignOut("ApplicationCookie"); 
                 }
             }

             // Add roles to members
             db.Roles.Add(new IdentityRole() { Name = "Admin" });
             db.Roles.Add(new IdentityRole() { Name = "Moderator" });
             userManager.AddToRole(m2.Id, "Admin");
             userManager.AddToRole(m3.Id, "Moderator");

             // Create Topics and Messages to be seeded into database

             Topic t1 = new Topic() { Title = "IPA Crave" };
             Topic t2 = new Topic() { Title = "Tap Houses" };
             context.Topics.Add(t1);
             context.Topics.Add(t2);

             Message p1 = new Message()
             {
                 Body = "Pelican Brewery makes a delicious single hop IPA.",
                 Date = new DateTime(2016, 2, 13, 12, 30, 00),
                 Member = m1,
                 Topic = t1
             };
             Message p2 = new Message()
             {
                 Body = "Hopworks Urban Brewing from Portland also has an incredible single hope IPA made with Simcoe hops. Give it a try!",
                 Date = new DateTime(2016, 2, 15, 11, 33, 00),
                 Member = m4,
                 Topic = t1
             };
             Message p3 = new Message()
             {
                 Body = "Beir Stein is a wonderful location with an unbelievable selection both on tap and in the fridge!",
                 Date = new DateTime(2016, 2, 20, 8, 30, 00),
                 Member = m1,
                 Topic = t2
             };
             context.Messages.Add(p1);
             context.Messages.Add(p2);
             context.Messages.Add(p3);

             
             base.Seed(context);
        }
    }
}