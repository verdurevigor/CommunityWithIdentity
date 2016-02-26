using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EugeneCommunity.Models;

namespace EugeneCommunity.Controllers
{
    public class NewsController : Controller
    {

        // GET: News
        public ActionResult Index()// Today's News
        {
            News n = new News();
            n.Title = "Arts Leaders of Eugene and Springfield";
            n.Story = "Connect with the local arts community. Network and share information with fellow artists and arts leaders at the January ALES Meet-Up. ALES holds bimonthly gatherings at local breweries and art venues. Today's event will be held from 4:30 to 6:30 p.m. at Sam Bond's Brewing Company (540 E 8th Ave, Eugene). This is a FREE event!";

            return View(n);
        }

        public ActionResult Archive()
        {
            return View();
        }
    }
}