using DatabaseLayer;
using KapiNadeApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KapiNadeApp.Controllers
{
    public class FinderController : Controller
    {
        KapiNadeDBEntities DB = new KapiNadeDBEntities();

        // GET: Finder
        public ActionResult FinderOfDonors()
        {
            ViewBag.BloodGroupID = new SelectList(DB.BloodGroupsTables.ToList(), "BloodGroupID", "BloodGroup", 0);
            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "City", 0);
            return View(new FinderMV());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FinderOfDonors(FinderMV finderMV)
        {
            var setdate = DateTime.Now.AddDays(-120);
            var donors = DB.DonorTables.Where(d => d.BloodGroupID == finderMV.BloodGroupID && d.LastDonationDate < setdate).ToList();
            foreach (var donor in donors)
            {
                var user = DB.UserTables.Find(donor.UserID);
                if (user.AccountStatusID == 2)
                {

                    var adddonor = new FinderSearchResultsMV();
                    adddonor.BloodGroup = donor.BloodGroupsTable.BloodGroup;
                    adddonor.BloodGroupID = donor.BloodGroupID;
                    adddonor.ContactNumber = donor.ContactNumber;
                    adddonor.DonorID = donor.DonorID;
                    adddonor.Name = donor.Name;
                    adddonor.Surname = donor.Surname;
                    adddonor.UserType = "Person";
                    adddonor.UsertypeID = user.UserTypeID;
                    finderMV.SearchResult.Add(adddonor);
                }
            }

            var bloodbanks = DB.BloodStockTables.Where(d => d.BloodGroupID == finderMV.BloodGroupID && d.Quantity > 0).ToList();
            foreach (var bloodbank in bloodbanks)
            {
                var getbloodbank = DB.BloodBankTables.Find(bloodbank.BloodBankID);
                var user = DB.UserTables.Find(getbloodbank.UserID);
                if (user.AccountStatusID == 2)
                {
                    var adddonor = new FinderSearchResultsMV();
                    adddonor.BloodGroup = bloodbank.BloodGroupsTable.BloodGroup;
                    adddonor.BloodGroupID = bloodbank.BloodGroupID;
                    adddonor.ContactNumber = bloodbank.BloodBankTable.ContactNumber;
                    adddonor.DonorID = bloodbank.BloodBankID;
                    adddonor.Name = bloodbank.BloodBankTable.Name;
                    adddonor.UserType = "Blood Bank";
                    adddonor.UsertypeID = user.UserTypeID;
                    finderMV.SearchResult.Add(adddonor);
                }
            }
            ViewBag.BloodGroupID = new SelectList(DB.BloodGroupsTables.ToList(), "BloodGroupID", "BloodGroup", finderMV.BloodGroupID);
            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "City", finderMV.CityID);
            return View(finderMV);
        }


    }
}