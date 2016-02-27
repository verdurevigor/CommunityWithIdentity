using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EugeneCommunity.Models
{
    public class AppDbContext : IdentityDbContext<Member>
    {
        public AppDbContext()
            : base("DefaultConnection")
        {
        }
    }
}