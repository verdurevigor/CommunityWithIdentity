namespace EugeneCommunity.Migrations
{
    using EugeneCommunity.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Validation;
    using System.Linq;
    using System.Text;

    internal sealed class Configuration : DbMigrationsConfiguration<EugeneCommunity.Models.AppDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "EugeneCommunity.Models.AppDbContext";
        }
        
        protected override void Seed(EugeneCommunity.Models.AppDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            // DO NOT FORGET: in order for this seed code to run, the command 'update-database' must be run in the console.
           
            // Turn on debugging (be sure to add a breakpoint somewhere in this Seed method)
            //if (System.Diagnostics.Debugger.IsAttached == false)
            //    System.Diagnostics.Debugger.Launch();

            // Add Users to the database
            
            // First create a UserManager object in order to add a new Identity
            var userManager = new UserManager<Member>(
                new UserStore<Member>(
                    new AppDbContext()));

            
            // If the members have already been created, comment out the member and role creations and uncomment the member queries above the Topic creation section
            
            // Initialize some users
            Member u1 = new Member()
            {
                Email = "xyz@email.com",
                UserName = "XYZ"
            };

            Member u2 = new Member()
            {
                Email = "admin@email.com",
                UserName = "Admin"
            };

            Member u3 = new Member()
            {
                Email = "moderator@email.com",
                UserName = "Moderator"
            };

            Member u4 = new Member()
            {
                Email = "zachariah@email.com",
                UserName = "BigZ"
            };
            
            // Add users with userManager
            userManager.Create(u1, "XyzPswd!");
            userManager.Create(u2, "AdminPswd!");
            userManager.Create(u3, "ModeratorPswd!");
            userManager.Create(u4, "ZachariahPswd!");
           
            // Get user Ids explicitly to add role to the user
            string id2 = (from u in context.Users
                          where u.UserName == u2.UserName
                          select u.Id).FirstOrDefault();
            string id3 = (from u in context.Users
                          where u.UserName == u3.UserName
                          select u.Id).FirstOrDefault();

            context.Roles.AddOrUpdate(r => r.Name, new IdentityRole() { Name = "Admin" });
            context.Roles.AddOrUpdate(r => r.Name, new IdentityRole() { Name = "Moderator" });
            context.SaveChanges();
            userManager.AddToRole(id2, "Admin");
            userManager.AddToRole(id3, "Moderator");
            context.SaveChanges();
           
            /* 
            // Get users if they are already in the database
            var u1 = (from u in context.Users
                      where u.UserName == "XYZ"
                      select u).FirstOrDefault();

            var u4 = (from u in context.Users
                      where u.UserName == "BigZ"
                      select u).FirstOrDefault();
            */

            Message p1 = new Message()
            {
                Body = "Pelican Brewery makes a delicious single hop IPA.",
                Date = new DateTime(2016, 2, 13, 12, 30, 00),
                Member = u1
            };
            Message p2 = new Message()
            {
                Body = "Hopworks Urban Brewing from Portland also has an incredible single hope IPA made with Simcoe hops. Give it a try!",
                Date = new DateTime(2016, 2, 15, 11, 33, 00),
                Member = u4
            };
            Message p3 = new Message()
            {
                Body = "Beir Stein is a wonderful location with an unbelievable selection both on tap and in the 'fridge'!",
                Date = new DateTime(2016, 2, 20, 8, 30, 00),
                Member = u1
            };

            Message p4 = new Message()
            {
                Body = "Falling Sky is a brewery, delicatessen, and taphouse all in one. Great food and brews to match!",
                Date = new DateTime(2016, 2, 21, 9, 47, 00),
                Member = u1
            };

            Message p5 = new Message()
            {
                Body = "Especially great for Sunday is Falling Sky. When you commute by bike to Falling Sky on Sundays you get a discout :)",
                Date = new DateTime(2016, 2, 21, 15, 12, 00),
                Member = u1
            };

            Topic t1 = new Topic() { Title = "IPA Crave!", Messages = {p1, p2} };
            Topic t2 = new Topic() { Title = "Tap Hauses", Messages = {p3, p4, p5} };

            context.Topics.AddOrUpdate(t => t.Title, t1, t2);
            SaveChanges(context);
        }
        
        // This custome method displays detailed error output to the console.
        private static void SaveChanges(DbContext context)
        {
            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                var sb = new StringBuilder();
                foreach (var failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }
                throw new DbEntityValidationException(
                    "Entity Validation Failed - errors follow:\n" +
                    sb.ToString(), ex
                );
            }
        }
    }
}
