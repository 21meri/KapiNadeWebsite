﻿using DatabaseLayer;
using KapiNadeApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
            int userid = 0;
            int.TryParse(Convert.ToString(Session["UserID"]), out userid);
            var maleThresholdDate = DateTime.Now.AddDays(-90);
            var femaleThresholdDate = DateTime.Now.AddDays(-120);

            var donors = DB.DonorTables.Where(d =>
                (d.BloodGroupID == finderMV.BloodGroupID && d.CityID == finderMV.CityID && d.GenderID == 1 && d.LastDonationDate < maleThresholdDate) ||
                (d.BloodGroupID == finderMV.BloodGroupID && d.CityID == finderMV.CityID && d.GenderID == 2 && d.LastDonationDate < femaleThresholdDate)
            ).ToList();

            foreach (var donor in donors)
            {
                var user = DB.UserTables.Find(donor.UserID);
                if (userid != user.UserID)
                {
                    if (user.AccountStatusID == 2)
                    {

                        var adddonor = new FinderSearchResultsMV();
                        adddonor.UserID=user.UserID;
                        adddonor.BloodGroup = donor.BloodGroupsTable.BloodGroup;
                        adddonor.BloodGroupID = donor.BloodGroupID;
                        adddonor.ContactNumber = donor.ContactNumber;
                        adddonor.DonorID = donor.DonorID;
                        adddonor.Name = donor.Name;
                        adddonor.Surname = donor.Surname;
                        adddonor.Address = donor.Address;

                        adddonor.Email = donor.Email;
                        adddonor.DateOfBirth = donor.DateOfBirth;

                        adddonor.UserType = "Osoba";
                        adddonor.UserTypeID = user.UserTypeID;
                        finderMV.SearchResult.Add(adddonor);
                    }
                }
            }

            var bloodbanks = DB.BloodStockTables.Where(d => d.BloodGroupID == finderMV.BloodGroupID && d.Quantity > 0).ToList();
            foreach (var bloodbank in bloodbanks)
            {
                var getbloodbank = DB.BloodBankTables.Find(bloodbank.BloodBankID);
                var user = DB.UserTables.Find(getbloodbank.UserID);
                if (userid != user.UserID)
                {
                    if (user.AccountStatusID == 2)
                    {
                        var adddonor = new FinderSearchResultsMV();
                        adddonor.UserID = user.UserID;
                        adddonor.BloodGroup = bloodbank.BloodGroupsTable.BloodGroup;
                        adddonor.BloodGroupID = bloodbank.BloodGroupID;
                        adddonor.ContactNumber = bloodbank.BloodBankTable.ContactNumber;
                        adddonor.Address = bloodbank.BloodBankTable.Address;
                        adddonor.Email = bloodbank.BloodBankTable.Email;
                        adddonor.DonorID = bloodbank.BloodBankID;
                        adddonor.Name = bloodbank.BloodBankTable.Name;
                        adddonor.UserType = "Krvna banka";
                        adddonor.UserTypeID = user.UserTypeID;
                        finderMV.SearchResult.Add(adddonor);
                    }
                }
            }
            ViewBag.BloodGroupID = new SelectList(DB.BloodGroupsTables.ToList(), "BloodGroupID", "BloodGroup", finderMV.BloodGroupID);
            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "City", finderMV.CityID);
            return View(finderMV);
        }

        public ActionResult RequestForBlood(int? donorid,int? usertypeid, int? bloodgroupid)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["Username"])))
            {
                return Redirect("/Home/MainHome#registrationsection");
            }

            var request = new RequestMV();
            request.AcceptedID = (int) donorid;
            if (usertypeid==2)
            {
                request.AcceptedTypeID = 1;
            }
    
            else if(usertypeid == 5)
            {
                request.AcceptedTypeID = 2;
            }
            request.RequiredBloodID = (int)bloodgroupid;

            return View(request);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RequestForBlood(RequestMV requestMV)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["Username"])))
            {
                return Redirect("/Home/MainHome#registrationsection");
            }

            int UserTypeID = 0;
            int RequestTypeID = 0;
            int RequestByID = 0;

            int.TryParse(Convert.ToString(Session["UserTypeID"] ),out UserTypeID);  
         

            if (UserTypeID == 2) // Donor
            {
                  int.TryParse(Convert.ToString(Session["DonorID"]),out RequestByID);           
            }
            else if (UserTypeID == 3) // Seeker
            {
                RequestTypeID = 1;
                int.TryParse(Convert.ToString(Session["SeekerID"]), out RequestByID);
            }

            else if (UserTypeID == 4) // Hospital
            {
                RequestTypeID = 2;
                int.TryParse(Convert.ToString(Session["HospitalID"]), out RequestByID);
            }

            else if (UserTypeID == 5) // Blood Bank
            {
                RequestTypeID = 3;
                int.TryParse(Convert.ToString(Session["BloodBankID"]), out RequestByID);
            }


            if (ModelState.IsValid)
            {
                var request = new RequestTable();
                request.RequestDate = DateTime.Now;
                request.AcceptedTypeID = requestMV.AcceptedTypeID;
                request.AcceptedID = requestMV.AcceptedID;
                request.RequiredBloodID = requestMV.RequiredBloodID;
                request.ExpectedDate = requestMV.ExpectedDate;
                request.RequestDetails = requestMV.RequestDetails;
                request.RequestStatusID = 1;
                request.RequestByID = RequestByID;
                request.RequestTypeID = RequestTypeID;
                DB.RequestTables.Add(request);
                DB.SaveChanges();
                return RedirectToAction("ShowAllRequests");
               
            }    

            return View(requestMV); 
        }
    public ActionResult ShowAllRequests()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["Username"])))
            {
                return RedirectToAction("Login", "Home");
            }

            int UserTypeID = 0;
            int RequestTypeID = 0;
            int RequestByID = 0;

            int.TryParse(Convert.ToString(Session["UserTypeID"]), out UserTypeID);


            if (UserTypeID == 2) // Donor
            {
                int.TryParse(Convert.ToString(Session["DonorID"]), out RequestByID);
            }
            else if (UserTypeID == 3) // Seeker
            {
                RequestTypeID = 1;
                int.TryParse(Convert.ToString(Session["SeekerID"]), out RequestByID);
            }

            else if (UserTypeID == 4) // Hospital
            {
                RequestTypeID = 2;
                int.TryParse(Convert.ToString(Session["HospitalID"]), out RequestByID);
            }

            else if (UserTypeID == 5) // Blood Bamk
            {
                RequestTypeID = 3;
                int.TryParse(Convert.ToString(Session["BloodBankID"]), out RequestByID);
            }

            var requests = DB.RequestTables.Where(r => r.RequestByID == RequestByID && r.RequestTypeID == RequestTypeID).ToList();
            var list = new List<RequestListMV>();
            foreach (var request in requests)
            {
                var addrequest = new RequestListMV();
                addrequest.RequestID = request.RequestID;
                addrequest.RequestDate = request.RequestDate.ToString("dd MMMM,yyyy");
                addrequest.RequestByID = request.RequestByID;
                addrequest.AcceptedID = request.AcceptedID;
                if (request.AcceptedTypeID == 1)//Donor
                {
                    var getdonor = DB.DonorTables.Find(request.AcceptedID);
                    addrequest.AcceptedName = getdonor.Name;
                    addrequest.AcceptedSurname = getdonor.Surname;
                    addrequest.ContactNumber = getdonor.ContactNumber;
                    addrequest.Address = getdonor.Address;
                }
                else if(request.AcceptedTypeID==2)//BloodBank
                {
                    var getbloodbank = DB.BloodBankTables.Find(request.AcceptedID);

                    addrequest.AcceptedName = getbloodbank.Name;
                    addrequest.ContactNumber = getbloodbank.ContactNumber;
                    addrequest.Address = getbloodbank.Address;

                }
 
                addrequest.AcceptedTypeID = request.AcceptedTypeID;
                addrequest.AcceptedType = request.AcceptedTypeTable.AcceptedType;
                addrequest.RequiredBloodID = request.RequiredBloodID;
                var bloodgroup = DB.BloodGroupsTables.Find(addrequest.RequiredBloodID);
                addrequest.BloodGroup = bloodgroup.BloodGroup;
                addrequest.RequestTypeID = request.RequestID;
                addrequest.RequestType = request.RequestTypeTable.RequestType;
                addrequest.RequestStatus = request.RequestStatusTable.RequestStatus;
                addrequest.RequestStatusID = request.RequestStatusID;
                addrequest.ExpectedDate = request.ExpectedDate.ToString("dd MMMM,yyyy");
                addrequest.RequestDetails = request.RequestDetails;
                list.Add(addrequest);
            }
            return View(list);
        }
        public ActionResult CancelRequest (int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["Username"])))
            {
                return RedirectToAction("Login","Home");
            }
            var request =DB.RequestTables.Find(id);
            request.RequestStatusID = 4;
            DB.Entry(request).State = System.Data.Entity.EntityState.Modified;
            DB.SaveChanges();
            return RedirectToAction("ShowAllRequests");

        }

        public ActionResult CancelRequestByDonor(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["Username"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var request = DB.RequestTables.Find(id);
            request.RequestStatusID = 4;
            DB.Entry(request).State = System.Data.Entity.EntityState.Modified;
            DB.SaveChanges();
            return RedirectToAction("DonorRequests");

        }


        public ActionResult AcceptRequest(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["Username"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var request = DB.RequestTables.Find(id);
            request.RequestStatusID = 2;
            DB.Entry(request).State = System.Data.Entity.EntityState.Modified;
            DB.SaveChanges();
            return RedirectToAction("DonorRequests");

        }


        public ActionResult DonorRequests()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["Username"])))
            {
                return RedirectToAction("Login", "Home");
            }

            int UserTypeID = 0;
            int AcceptedTypeID = 0;
            int AcceptedByID = 0;

            int.TryParse(Convert.ToString(Session["UserTypeID"]), out UserTypeID);


            if (UserTypeID == 2) // Donor
            {
                AcceptedTypeID = 1;
                int.TryParse(Convert.ToString(Session["DonorID"]), out AcceptedByID);
            }
            else if (UserTypeID == 3) // Seeker
            {
                int.TryParse(Convert.ToString(Session["SeekerID"]), out AcceptedByID);
            }

            else if (UserTypeID == 4) // Hospital
            {
                int.TryParse(Convert.ToString(Session["HospitalID"]), out AcceptedByID);
            }

            else if (UserTypeID == 5) // Blood Bamk
            {
                AcceptedTypeID = 2;
                int.TryParse(Convert.ToString(Session["BloodBankID"]), out AcceptedByID);
            }

            var requests = DB.RequestTables.Where(r => r.AcceptedID == AcceptedByID && r.AcceptedTypeID == AcceptedTypeID).ToList();
            var list = new List<RequestListMV>();
            foreach (var request in requests)
            {
                var addrequest = new RequestListMV();
                addrequest.RequestID = request.RequestID;
                addrequest.RequestDate = request.RequestDate.ToString("dd MMMM,yyyy");
                addrequest.RequestByID = request.RequestByID;
                addrequest.AcceptedID = request.AcceptedID;

                if (request.RequestTypeID == 1)//Seeker
                {
                    var getseeker = DB.SeekerTables.Find(request.RequestByID);
                    addrequest.RequestBy = getseeker.Name;
                    addrequest.RequestBySurname = getseeker.Surname;
                    addrequest.ContactNumber = getseeker.ContactNumber;
                    addrequest.Address = getseeker.Address;
                }

                else if (request.RequestTypeID == 2)//Hospital
                {
                    var gethospital = DB.HospitalTables.Find(request.RequestByID);

                    addrequest.RequestBy = gethospital.Name;
                    addrequest.ContactNumber = gethospital.ContactNumber;
                    addrequest.Address = gethospital.Address;

                }

                else if (request.RequestTypeID == 3)//BloodBank
                {
                    var getbloodbank = DB.BloodBankTables.Find(request.RequestByID);

                    addrequest.RequestBy = getbloodbank.Name;
                    addrequest.ContactNumber = getbloodbank.ContactNumber;
                    addrequest.Address = getbloodbank.Address;

                }


                addrequest.AcceptedTypeID = request.AcceptedTypeID;
                addrequest.AcceptedType = request.AcceptedTypeTable.AcceptedType;
                addrequest.RequiredBloodID = request.RequiredBloodID;
                var bloodgroup = DB.BloodGroupsTables.Find(addrequest.RequiredBloodID);
                addrequest.BloodGroup = bloodgroup.BloodGroup;
                addrequest.RequestTypeID = request.RequestID;
                addrequest.RequestType = request.RequestTypeTable.RequestType;
                addrequest.RequestStatus = request.RequestStatusTable.RequestStatus;
                addrequest.RequestStatusID = request.RequestStatusID;
                addrequest.ExpectedDate = request.ExpectedDate.ToString("dd MMMM,yyyy");
                addrequest.RequestDetails = request.RequestDetails;
                list.Add(addrequest);
            }
            return View(list);
        }

        public ActionResult CompleteRequest(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["Username"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var request = DB.RequestTables.Find(id);
            if(request.AcceptedTypeID == 1) //Donor
            {
                var donor = DB.DonorTables.Find(request.AcceptedID);
                donor.LastDonationDate = DateTime.Now;
                DB.Entry(donor).State = System.Data.Entity.EntityState.Modified;
                DB.SaveChanges();

                request.RequestStatusID = 3;
                DB.Entry(request).State = System.Data.Entity.EntityState.Modified;
                DB.SaveChanges();
                return RedirectToAction("ShowAllRequests");

            }
           
                var bloodbank = DB.BloodBankTables.Find(request.AcceptedID);
                var bloodstockMV = new BloodStockMV();
                bloodstockMV.BloodStockID = request.RequestID;
                bloodstockMV.BloodBankID = bloodbank.BloodBankID;
                bloodstockMV.BloodGroupID = request.RequiredBloodID;
                bloodstockMV.BloodBank = bloodbank.Name;
                var bloodgroup = DB.BloodGroupsTables.Find(request.RequiredBloodID);
                bloodstockMV.BloodGroup = bloodgroup.BloodGroup;
                bloodstockMV.Quantity = 1;

            return View(bloodstockMV);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CompleteRequest(BloodStockMV bloodStockMV)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["Username"])))
            {
                return RedirectToAction("Login", "Home");
            }
            try
            {
                var request = DB.RequestTables.Find(bloodStockMV.BloodStockID);
                var bloodstock = DB.BloodStockTables.Where(b => b.BloodBankID == bloodStockMV.BloodBankID && b.BloodGroupID == bloodStockMV.BloodGroupID).FirstOrDefault();
                if(bloodstock.Quantity < bloodStockMV.Quantity)
                {
                    ModelState.AddModelError(string.Empty, "Dostupna količina: "+bloodstock.Quantity+"L");
                    return View(bloodStockMV);

                }

                bloodstock.Quantity = bloodstock.Quantity - bloodStockMV.Quantity;
                DB.Entry(bloodstock).State = System.Data.Entity.EntityState.Modified;
                DB.SaveChanges();

                request.RequestStatusID = 3;
                DB.Entry(request).State = System.Data.Entity.EntityState.Modified;
                DB.SaveChanges();
                return RedirectToAction("ShowAllRequests");

            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Molimo Vas da unesete količinu!");
                return View(bloodStockMV);
            }


        }



    }
}