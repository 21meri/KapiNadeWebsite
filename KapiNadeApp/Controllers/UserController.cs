using DatabaseLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KapiNadeApp.Controllers
{
    public class UserController : Controller
    {
        KapiNadeDBEntities DB = new KapiNadeDBEntities();

        // GET: User
        public ActionResult UserProfile(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["Username"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var user = DB.UserTables.Find(id);
            return View(user);
        }
    }
}