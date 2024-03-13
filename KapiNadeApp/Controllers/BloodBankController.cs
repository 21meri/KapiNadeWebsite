using DatabaseLayer;
using KapiNadeApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace KapiNadeApp.Controllers
{
    public class BloodBankController : Controller
    {
        KapiNadeDBEntities DB = new KapiNadeDBEntities();

        // GET: BloodBank
        public ActionResult BloodStock()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["Username"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var list = new List<BloodStockMV>();
            var bloodbankID = 0;
            int.TryParse(Convert.ToString(Session["BloodBankID"]), out bloodbankID);
            var stocklist = DB.BloodStockTables.Where(b => b.BloodBankID == bloodbankID);

            foreach(var stock in stocklist)
            {
                string bloodbank = stock.BloodBankTable.Name;
                string bloodgroup = stock.BloodGroupsTable.BloodGroup;
                var bloodStockmv = new BloodStockMV();

                bloodStockmv.BloodStockID = stock.BloodStockID;
                bloodStockmv.BloodGroupID = stock.BloodGroupID;
                bloodStockmv.BloodGroup = bloodgroup;
                bloodStockmv.BloodBankID = stock.BloodBankID;
                bloodStockmv.BloodBank = bloodbank;
                bloodStockmv.Status = stock.Status == true ? "Ready for use" : "Not ready for use";
                bloodStockmv.Quantity = stock.Quantity;
                bloodStockmv.BestBefore = stock.BestBefore;
                list.Add(bloodStockmv);

            }
            return View(list);
        }

        public ActionResult NewCampaign()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["Username"])))
            {
                return RedirectToAction("Login", "Home");
            }

            return View();
        }
    }
}