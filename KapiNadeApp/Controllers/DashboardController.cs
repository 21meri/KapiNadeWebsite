using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KapiNadeApp.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard

        public ActionResult Donor()
        {
            // Retrieve the username from the session
            string name = Session["Name"] as string;

            // Pass the username to the viewbag
            ViewBag.Name = name;

            return View();
        }

        public ActionResult Seeker()
        {
            // Retrieve the username from the session
            string name = Session["Name"] as string;

            // Pass the username to the viewbag
            ViewBag.Name = name;
            return View();
        }

        public ActionResult Hospital()
        {
            // Retrieve the username from the session
            string name = Session["Name"] as string;

            // Pass the username to the viewbag
            ViewBag.Name = name;
            return View();
        }


        public ActionResult BloodBank()
        {
            // Retrieve the username from the session
            string name = Session["Name"] as string;

            // Pass the username to the viewbag
            ViewBag.Name = name;
            return View();
        }

 


    }
}