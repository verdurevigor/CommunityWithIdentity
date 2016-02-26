using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EugeneCommunity.Controllers
{
    public class HomeController : Controller
    {
        public List<String> facts = new List<String>();
        public HomeController()
        {
            // Random
            string f1 = "The oldest Black Tartarian cherry tree in the U.S. is located at Owen Rose Garden in Eugene.";
            string f2 = "Eugene is consistently ranked as one of America’s 'Most Bicycle Friendly Cities'.";
            string f3 = "Now a worldwide treasure hunt, geocaching got its start in Oregon. And Oregon's first official GeoTour was launched in Lane County.";
            string f4 = "Eugene is recognized as 'TrackTown USA'";
            
            facts.Add(f1);
            facts.Add(f2);
            facts.Add(f3);
            facts.Add(f4);

        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            Random r = new Random();
            int j = facts.Count;
            int i = r.Next(0, j);

            ViewBag.RandomFact = facts[i];

            return View();
        }

        public ActionResult Contact()
        {

            return View();
        }

        public ActionResult History()
        {
            ViewBag.Message = "A brief history of Eugene...";

            return View();
        }
    }
}