﻿using DatabaseLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;
using KapiNadeApp.Models;

namespace KapiNadeApp.Controllers
{
    public class RequestTypeController : Controller
    {
        KapiNadeDBEntities DB = new KapiNadeDBEntities();
        public ActionResult AllRequestType()
        {
            var requesttypes = DB.RequestTypeTables.ToList();
            var listrequesttypes = new List<RequestTypeMV>();
            foreach (var requesttype in requesttypes)
            {
                var addrequesttype = new RequestTypeMV();
                addrequesttype.RequestTypeID = requesttype.RequestTypeID;
                addrequesttype.RequestType = requesttype.RequestType;
                listrequesttypes.Add(addrequesttype);
            }
            return View(listrequesttypes);
        }

        public ActionResult Create()
        {
            var requesttype = new RequestTypeMV();
            return View(requesttype);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RequestTypeMV requestTypeMV)
        {
            if (ModelState.IsValid)
            {
                var requestTypeTable = new RequestTypeTable();
                requestTypeTable.RequestTypeID = requestTypeMV.RequestTypeID;
                requestTypeTable.RequestType = requestTypeMV.RequestType;
                DB.RequestTypeTables.Add(requestTypeTable);
                DB.SaveChanges();

                return RedirectToAction("AllRequestType");
            }
            return View(requestTypeMV);
        }

        public ActionResult Edit(int? id)
        {
            var requesttype = DB.RequestTypeTables.Find(id);
            if(requesttype == null)
            {
                return HttpNotFound();
            }
            var requesttypemv = new RequestTypeMV();
            requesttypemv.RequestTypeID = requesttype.RequestTypeID;
            requesttypemv.RequestType = requesttype.RequestType;
            return View(requesttypemv);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RequestTypeMV requestTypeMV)
        {
            if (ModelState.IsValid)
            {
                var requestTypeTable = new RequestTypeTable();
                requestTypeTable.RequestTypeID = requestTypeMV.RequestTypeID;
                requestTypeTable.RequestType = requestTypeMV.RequestType;

                DB.Entry(requestTypeTable).State = EntityState.Modified;
                DB.SaveChanges();
                return RedirectToAction("AllRequestType");
            }
            return View(requestTypeMV);
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
            var requesttypemv = new RequestTypeMV();
            requesttypemv.RequestTypeID = requesttype.RequestTypeID;
            requesttypemv.RequestType = requesttype.RequestType;
            return View(requesttypemv);
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

    