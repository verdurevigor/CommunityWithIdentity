namespace EugeneCommunity.Migrations
{
    using EugeneCommunity.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<EugeneCommunity.Models.AppDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
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

            // Add users with userManager
            userManager.Create(m1, "password");
            userManager.Create(m2, "password");
            userManager.Create(m3, "password");
            userManager.Create(m4, "password");

            db.Roles.Add(new IdentityRole() { Name = "Admin" });
            db.Roles.Add(new IdentityRole() { Name = "Moderator" });
            db.SaveChanges();
            userManager.AddToRole(m2.Id, "Admin");
            userManager.AddToRole(m3.Id, "Moderator");
            context.SaveChanges();

            // Create Topics and Messages to be seeded into database
            Topic t1 = new Topic() { Title = "IPA Crave" };
            Topic t2 = new Topic() { Title = "Tap Houses" };
            context.Topics.AddOrUpdate(t => t.Title, t1);
            context.Topics.AddOrUpdate(t => t.Title, t2);
            context.SaveChanges();

            Message p1 = new Message()
            {
                Body = "Pelican Brewery makes a delicious single hop IPA.",
                Date = new DateTime(2016, 2, 13, 12, 30, 00),
                MemberId = m1.Id,
                TopicId = t1.TopicId
            };
            Message p2 = new Message()
            {
                Body = "Hopworks Urban Brewing from Portland also has an incredible single hope IPA made with Simcoe hops. Give it a try!",
                Date = new DateTime(2016, 2, 15, 11, 33, 00),
                MemberId = m4.Id,
                TopicId = t1.TopicId
            };
            Message p3 = new Message()
            {
                Body = "Beir Stein is a wonderful location with an unbelievable selection both on tap and in the fridge!",
                Date = new DateTime(2016, 2, 20, 8, 30, 00),
                MemberId = m1.Id,
                TopicId = t2.TopicId
            };
            context.Messages.AddOrUpdate(m => m.Body, p1);
            context.Messages.AddOrUpdate(m => m.Body, p2);
            context.Messages.AddOrUpdate(m => m.Body, p3);
            context.SaveChanges();

            base.Seed(context);

            // TODO: I am getting an error when updating the database:
            // 'Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.'
            // No Topics or Messages are being seeded, but the Users and Roles seem to be updated...
        }
    }
}
