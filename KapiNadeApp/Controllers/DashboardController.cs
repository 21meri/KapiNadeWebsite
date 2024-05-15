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
            string name = Session["Name"] as string;
            ViewBag.Name = name;
            return View();
        }

        public ActionResult Seeker()
        {
            string name = Session["Name"] as string;
            ViewBag.Name = name;
            return View();
        }

        public ActionResult Hospital()
        {
            string name = Session["Name"] as string;
            ViewBag.Name = name;
            return View();
        }


        public ActionResult BloodBank()
        {
            string name = Session["Name"] as string;
            ViewBag.Name = name;
            return View();
        }

 


    }
}