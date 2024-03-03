using DatabaseLayer;
using KapiNadeApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace KapiNadeApp.Controllers
{
    public class AccountStatusController : Controller
    {
        
        KapiNadeDBEntities DB = new KapiNadeDBEntities();
        public ActionResult AllAccountStatus()
        {
            var accountstatuses = DB.AccountStatusTables.ToList();
            var listaccountstatuses = new List<AccountStatusMV>();
            foreach (var accountstatus in accountstatuses)
            {
                var addaccountstatusmv = new AccountStatusMV();
                addaccountstatusmv.AccountStatusID = accountstatus.AccountStatusID;
                addaccountstatusmv.AccountStatus = accountstatus.AccountStatus;
                listaccountstatuses.Add(addaccountstatusmv);
            }
            return View(listaccountstatuses);
        }

        public ActionResult Create()
        {
            var accountstatus = new AccountStatusMV();
            return View(accountstatus);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AccountStatusMV accountStatusMV)
        {
            if (ModelState.IsValid)
            {
                var checkaccountstatus = DB.AccountStatusTables.Where(b => b.AccountStatus == accountStatusMV.AccountStatus).FirstOrDefault();
                if (checkaccountstatus == null)
                {
                    var accountstatusesTable = new AccountStatusTable();
                    accountstatusesTable.AccountStatusID = accountStatusMV.AccountStatusID;
                    accountstatusesTable.AccountStatus = accountStatusMV.AccountStatus;
                    DB.AccountStatusTables.Add(accountstatusesTable);
                    DB.SaveChanges();

                    return RedirectToAction("AllAccountStatus");
                }
                else
                {
                    ModelState.AddModelError("AccountStatus", "Already Exist!");
                }
            }
            return View(accountStatusMV);

        }

        public ActionResult Edit(int? id)
        {
            var accountstatus = DB.AccountStatusTables.Find(id);
            if (accountstatus == null)
            {
                return HttpNotFound();
            }
            var accountstatusmv = new AccountStatusMV();
            accountstatusmv.AccountStatusID = accountstatus.AccountStatusID;
            accountstatusmv.AccountStatus = accountstatus.AccountStatus;
            return View(accountstatusmv);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AccountStatusMV accountStatusMV)
        {
            if (ModelState.IsValid)
            {
                var checkaccountstatus = DB.AccountStatusTables.Where(b => b.AccountStatus == accountStatusMV.AccountStatus && b.AccountStatusID != accountStatusMV.AccountStatusID).FirstOrDefault();
                if (checkaccountstatus == null)
                {
                    var accountStatusTable = new AccountStatusTable();
                    accountStatusMV.AccountStatusID = accountStatusMV.AccountStatusID;
                    accountStatusMV.AccountStatus = accountStatusMV.AccountStatus;

                    DB.Entry(accountStatusTable).State = EntityState.Modified;
                    DB.SaveChanges();
                    return RedirectToAction("AllAccountStatus");
                }
                else
                {
                    ModelState.AddModelError("AccountStatus", "Already Exist!");
                }
            }
            return View(accountStatusMV);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var accountstatus = DB.AccountStatusTables.Find(id);
            if (accountstatus == null)
            {
                return HttpNotFound();
            }
            var accountStatusMV = new AccountStatusMV();
            accountStatusMV.AccountStatusID = accountstatus.AccountStatusID;
            accountStatusMV.AccountStatus = accountstatus.AccountStatus;
            return View(accountStatusMV);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirm(int? id)
        {
            var accountstatus = DB.AccountStatusTables.Find(id);
            DB.AccountStatusTables.Remove(accountstatus);
            DB.SaveChanges();
            return RedirectToAction("AllAccountStatus");
        }




    }
}