using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace EugeneCommunity.Models
{
    public class AppDbContext : IdentityDbContext<Member>
    {
        public AppDbContext() : base("DefaultConnection")
        {
        }

        public System.Data.Entity.DbSet<EugeneCommunity.Models.Topic> Topics { get; set; }

        public System.Data.Entity.DbSet<EugeneCommunity.Models.Message> Messages { get; set; }
    }
    /*
    // This is required during dev
    public class AppInitializer : DropCreateDatabaseAlways<AppDbContext>
    {

    }*/
}