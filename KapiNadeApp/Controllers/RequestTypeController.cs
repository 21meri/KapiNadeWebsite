using DatabaseLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;

namespace KapiNadeApp.Controllers
{
    public class RequestTypeController : Controller
    {
        KapiNadeDBEntities DB = new KapiNadeDBEntities();
        public ActionResult AllRequestType()
        {
            var requesttypes = DB.RequestTypeTables.ToList();
            return View(requesttypes);
        }

        public ActionResult Create()
        {
            var requesttype = new RequestTypeTable();
            return View(requesttype);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RequestTypeTable requestTypeTable)
        {
            if (ModelState.IsValid)
            {
                DB.RequestTypeTables.Add(requestTypeTable);
                DB.SaveChanges();

                return RedirectToAction("AllRequestType");
            }
            return View(requestTypeTable);
        }

        public ActionResult Edit(int? id)
        {
            var requesttype = DB.RequestTypeTables.Find(id);
            if(requesttype == null)
            {
                return HttpNotFound();
            }
            return View(requesttype);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RequestTypeTable requestTypeTable)
        {
            if (ModelState.IsValid)
            {
                DB.Entry(requestTypeTable).State = EntityState.Modified;
                DB.SaveChanges();
                return RedirectToAction("AllRequestType");
            }
            return View(requestTypeTable);
        }

        public ActionResult Delete(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var requesttype = DB.RequestTypeTables.Find(id);
            if (requesttype == null)
            {
                return HttpNotFound();
            }
            return View(requesttype);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirm(int? id)
        {
            var requesttype = DB.RequestTypeTables.Find(id);
            DB.RequestTypeTables.Remove(requesttype);
            DB.SaveChanges();
            return RedirectToAction("AllRequestType");
        }

    } 
}

    