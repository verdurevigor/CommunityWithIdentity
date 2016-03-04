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

    internal sealed class Configuration : DbMigrationsConfiguration<EugeneCommunity.Models.AppDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;           // This is set so that any data added while testing is simply removed and set back with the data seeded below when 'update-database' is ran.
            ContextKey = "EugeneCommunity.Models.AppDbContext";
        }
        
        protected override void Seed(EugeneCommunity.Models.AppDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //


            // DO NOT FORGET: in order for this seed code to run, the command 'update-database' must be run in the console.


            // Add Users to the database
            
            // First create a UserManager object in order to add a new Identity
            var userManager = new UserManager<Member>(
                new UserStore<Member>(
                    new AppDbContext()));

            AppDbContext db = new AppDbContext();
            
            // Initialize some users
            Member u1 = new Member()
            {
                Email = "brody@email.com",
                UserName = "Brodster"
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
                Email = "zach@email.com",
                UserName = "Zach"
            };
            
            // Add users with userManager
            userManager.Create(u1, "password");
            userManager.Create(u2, "password");
            userManager.Create(u3, "password");
            userManager.Create(u4, "password");
           
            // Get user Ids explicitly to add role to the user
            string id2 = (from u in db.Users
                          where u.UserName == u2.UserName
                          select u.Id).FirstOrDefault();
            string id3 = (from u in db.Users
                          where u.UserName == u3.UserName
                          select u.Id).FirstOrDefault();

            db.Roles.AddOrUpdate(r => r.Name, new IdentityRole() { Name = "Admin" });
            db.Roles.AddOrUpdate(r => r.Name, new IdentityRole() { Name = "Moderator" });
            db.SaveChanges();
            userManager.AddToRole(id2, "Admin");
            userManager.AddToRole(id3, "Moderator");
            context.SaveChanges();
            
            /*
            // Create Topics and Messages to be seeded into database
            Topic t1 = new Topic() { Title = "IPA Crave" };
            Topic t2 = new Topic() { Title = "Tap Houses" };
            context.Topics.AddOrUpdate(t => t.Title, t1);
            context.Topics.AddOrUpdate(t => t.Title, t2);
            context.SaveChanges();

            t1.TopicId = (from t in db.Topics
                          where t.Title == t1.Title
                          select t.TopicId).FirstOrDefault();
            t2.TopicId = (from t in db.Topics
                          where t.Title == t2.Title
                          select t.TopicId).FirstOrDefault();
                        // TODO: Just tried to set the topicId. Attempt the same with members so that it is a whole entity without pulling a second object of the same thing (EF doesn't like that)
            
            // Retrieve members and users explicityly to add to messages
            var u1 = (from u in db.Users
                        where u.UserName == m1.UserName
                        select u).FirstOrDefault();
            var u4 = (from u in db.Users
                        where u.UserName == m4.UserName
                        select u).FirstOrDefault();
            
            var s1 = (from t in db.Topics
                          where t.Title == t1.Title
                          select t).FirstOrDefault();

            var s2 = (from t in db.Topics
                        where t.Title == t2.Title
                        select t).FirstOrDefault();
           
            // Attempting to only grab users and topics without creating them as above. As if they were already in the db.
            var m1 = (from u in db.Users
                      where u.UserName == "Brodster"
                      select u).FirstOrDefault();

            var m4 = (from u in db.Users
                      where u.UserName == "Zach"
                      select u).FirstOrDefault();

            var t1 = (from t in db.Topics
                      where t.Title == "IPA Crave"
                      select t).FirstOrDefault();

            var t2 = (from t in db.Topics
                      where t.Title == "Tap Houses"
                      select t).FirstOrDefault();

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
            context.Messages.AddOrUpdate(m => m.Body, p1);
            context.Messages.AddOrUpdate(m => m.Body, p2);
            context.Messages.AddOrUpdate(m => m.Body, p3);
            context.SaveChanges();
            */

            /*

            // This section of an attempt does not place the messages into the database as hoped...     Even attempted again after deleting the db files and recreating users/roles

            // Attempting to create a list of messages and place them in the each topic, then saving the entire Topic object to the db
            // Currently Members and Roles are intitialized.
            
            // Members to use
            
            // New Topics to add Messages to
            Topic t1 = new Topic() { Title = "IPA Crave" };
            Topic t2 = new Topic() { Title = "Tap Houses" };

            // New Message to add to Topics
            Message m1 = new Message()
            {
                Body = "Pelican Brewery makes a delicious single hop IPA.",
                Date = new DateTime(2016, 2, 13, 12, 30, 00),
                Member = u1,
            };
            Message m2 = new Message()
            {
                Body = "Hopworks Urban Brewing from Portland also has an incredible single hope IPA made with Simcoe hops. Give it a try!",
                Date = new DateTime(2016, 2, 15, 11, 33, 00),
                Member = u2
            };
            Message m3 = new Message()
            {
                Body = "Beir Stein is a wonderful location with an unbelievable selection both on tap and in the fridge!",
                Date = new DateTime(2016, 2, 20, 8, 30, 00),
                Member = u1
            };
            
            // Add Messages to Topics
            t1.Messages.Add(m1);
            t1.Messages.Add(m2);
            t2.Messages.Add(m3);

            // Save Topics to database
            context.Topics.AddOrUpdate(t=> t.Title, t1);
            context.Topics.AddOrUpdate(t => t.Title, t2);
            */

            /*
            // Still gave error 'Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.'
            // Attempting to use the topic objects when creating the messages.
            

            // New Topics to add Messages to
            Topic t1 = new Topic() { Title = "IPA Crave" };
            Topic t2 = new Topic() { Title = "Tap Houses" };
            context.Topics.AddOrUpdate(t => t.Title, t1);
            context.Topics.AddOrUpdate(t => t.Title, t2);
            context.SaveChanges();

            t1 = (from t in db.Topics
                  where t.Title == t1.Title
                  select t).FirstOrDefault();
            t2 = (from t in db.Topics
                  where t.Title == t2.Title
                  select t).FirstOrDefault();
            // New Message to add to Topics
            Message m1 = new Message()
            {
                Body = "Pelican Brewery makes a delicious single hop IPA.",
                Date = new DateTime(2016, 2, 13, 12, 30, 00),
                Member = u1,
                Topic = t1
            };
            Message m2 = new Message()
            {
                Body = "Hopworks Urban Brewing from Portland also has an incredible single hope IPA made with Simcoe hops. Give it a try!",
                Date = new DateTime(2016, 2, 15, 11, 33, 00),
                Member = u2,
                Topic = t1
            };
            Message m3 = new Message()
            {
                Body = "Beir Stein is a wonderful location with an unbelievable selection both on tap and in the fridge!",
                Date = new DateTime(2016, 2, 20, 8, 30, 00),
                Member = u1,
                Topic = t2
            };

            // Save Topics to database
            context.Messages.AddOrUpdate(m => m.Body, m1);
            context.Messages.AddOrUpdate(m => m.Body, m2);
            context.Messages.AddOrUpdate(m => m.Body, m3);
            context.SaveChanges();
            */

            base.Seed(context);

            // TODO: I am getting an error when updating the database:
            // 'Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.'
            // No Topics or Messages are being seeded, but the Users and Roles seem to be updated...
        }
    }
}
