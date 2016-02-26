using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EugeneCommunity.Models
{
    public class News
    {
        public virtual int NewsId { get; set; }
        public virtual string Title { get; set; }
        public virtual string Story { get; set; }
        public virtual DateTime Date { get; set; }
    }
}