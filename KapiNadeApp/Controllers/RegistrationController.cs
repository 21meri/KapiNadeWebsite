﻿using DatabaseLayer;
using KapiNadeApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KapiNadeApp.Controllers
{
    public class RegistrationController : Controller
    {
        KapiNadeDBEntities DB = new KapiNadeDBEntities();
        static RegistrationMV registrationmv;
        [HttpPost]
        [ValidateAntiForgeryToken]
      public ActionResult SelectUser(RegistrationMV registrationMV)
        {
            registrationmv = registrationMV;
            if (registrationMV.UserTypeID == 2)
            {
                return RedirectToAction("DonorUser");
            }
            else if (registrationMV.UserTypeID == 3)
            {
                return RedirectToAction("SeekerUser");
            }
            else if (registrationMV.UserTypeID == 4)
            {
                return RedirectToAction("HospitalUser");
            }
            else if (registrationMV.UserTypeID == 5)
            {
                return RedirectToAction("BloodBankUser");
            }
            else
            {
                return RedirectToAction("MainHome", "Home");
            }
        }

    
    public ActionResult HospitalUser()
        {
            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "City", registrationmv.CityID);
            return View(registrationmv);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult HospitalUser(RegistrationMV registrationMV)
        {
            if(ModelState.IsValid)
            {
                var checktitle = DB.HospitalTables.Where(h => h.Name == registrationMV.Hospital.Name.Trim()).FirstOrDefault();
                if (checktitle == null)
                {
                    using (var transaction = DB.Database.BeginTransaction())
                    {
                        try
                        {
                            var user = new UserTable();
                            user.Username = registrationMV.User.Username;
                            user.Password = registrationMV.User.Password;
                            user.Email = registrationMV.User.Email;
                            user.AccountStatusID = 1;
                            user.UserTypeID = registrationMV.UserTypeID;
                            DB.UserTables.Add(user);
                            DB.SaveChanges();

                            var hospital = new HospitalTable();
                            hospital.Name = registrationMV.Hospital.Name;
                            hospital.Address = registrationMV.Hospital.Address;
                            hospital.ContactNumber = registrationMV.Hospital.ContactNumber;
                            hospital.Email = registrationMV.Hospital.Email;
                            hospital.CityID = registrationMV.CityID;
                            hospital.UserID = user.UserID;
                            DB.HospitalTables.Add(hospital);
                            DB.SaveChanges();
                            transaction.Commit();
                            ViewData["Message"] = "Thanks for registration, your query will be reviewed shortly!";
                            return RedirectToAction("MainHome", "Home");
                        }
                        catch
                        {
                            ModelState.AddModelError(string.Empty, "Please provide correct information!");
                            transaction.Rollback();
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Hospital already registered!");
                }
    }
            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "City", registrationMV.CityID);
            return View(registrationMV);
        }





        public ActionResult DonorUser()
        {
            ViewBag.BloodGroupID = new SelectList(DB.BloodGroupsTables.ToList(), "BloodGroupID", "BloodGroup", "0");
            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "City", "0");
            ViewBag.GenderID = new SelectList(DB.GenderTables.ToList(), "GenderID", "Gender", "0");
            return View(registrationmv);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult DonorUser(RegistrationMV registrationMV)
        {
            if (ModelState.IsValid)
            {
                var checktitle = DB.DonorTables.Where(h => h.Name == registrationMV.Donor.Name.Trim() && h.CardNumber==registrationMV.Donor.CardNumber).FirstOrDefault();
                if (checktitle == null)
                {
                    using (var transaction = DB.Database.BeginTransaction())
                    {
                        try
                        {
                            var user = new UserTable();
                            user.Username = registrationMV.User.Username;
                            user.Password = registrationMV.User.Password;
                            user.Email = registrationMV.User.Email;
                            user.AccountStatusID = 1;
                            user.UserTypeID = registrationMV.UserTypeID;
                            DB.UserTables.Add(user);
                            DB.SaveChanges();

                            var donor = new DonorTable();
                            donor.Name = registrationMV.Donor.Name;
                            donor.Surname = registrationMV.Donor.Surname;
                            donor.BloodGroupID = registrationMV.BloodGroupID;
                            donor.Address = registrationMV.Donor.Address;
                            donor.CardNumber = registrationMV.Donor.CardNumber;
                            donor.GenderID = registrationMV.GenderID;
                            donor.LastDonationDate = registrationMV.Donor.LastDonationDate;
                            donor.ContactNumber = registrationMV.Donor.ContactNumber;
                            donor.CityID = registrationMV.CityID;
                            donor.UserID = user.UserID;
                            DB.DonorTables.Add(donor);
                            DB.SaveChanges();
                            transaction.Commit();
                            ViewData["Message"] = "Thanks for registration, your query will be reviewed shortly!";
                            return RedirectToAction("MainHome", "Home");
                        }
                        catch
                        {
                            ModelState.AddModelError(string.Empty, "Please provide correct information!");
                            transaction.Rollback();
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Donor already registered!");
                }
            }
            ViewBag.GenderID = new SelectList(DB.GenderTables.ToList(), "GenderID", "Gender", registrationMV.GenderID);
            ViewBag.BloodGroupID = new SelectList(DB.BloodGroupsTables.ToList(), "BloodGroupID", "BloodGroup",registrationMV.BloodGroupID);
            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "City", registrationMV.CityID);
            return View(registrationMV);
        }



        public ActionResult BloodBankUser()
        {
            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "City", registrationmv.CityID);
            return View(registrationmv);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult BloodBankUser(RegistrationMV registrationMV)
        {
            if (ModelState.IsValid)
            {
                var checktitle = DB.BloodBankTables.Where(h => h.Name == registrationMV.BloodBank.Name.Trim() && h.ContactNumber == registrationMV.BloodBank.ContactNumber).FirstOrDefault();
                if (checktitle == null)
                {
                    using (var transaction = DB.Database.BeginTransaction())
                    {
                        try
                        {
                            var user = new UserTable();
                            user.Username = registrationMV.User.Username;
                            user.Password = registrationMV.User.Password;
                            user.Email = registrationMV.User.Email;
                            user.AccountStatusID = 1;
                            user.UserTypeID = registrationMV.UserTypeID;
                            DB.UserTables.Add(user);
                            DB.SaveChanges();

                            var bloodBank = new BloodBankTable();
                            bloodBank.Name = registrationMV.BloodBank.Name;
                            bloodBank.Address = registrationMV.BloodBank.Address;
                            bloodBank.ContactNumber = registrationMV.BloodBank.ContactNumber;
                            bloodBank.Email = registrationMV.BloodBank.Email;
                            bloodBank.CityID = registrationMV.CityID;
                            bloodBank.UserID = user.UserID;
                            DB.BloodBankTables.Add(bloodBank);
                            DB.SaveChanges();
                            transaction.Commit();
                            ViewData["Message"] = "Thanks for registration, your query will be reviewed shortly!";
                            return RedirectToAction("MainHome", "Home");
                        }
                        catch
                        {
                            ModelState.AddModelError(string.Empty, "Please provide correct information!");
                            transaction.Rollback();
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Blood Bank already registered!");
                }
            }
            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "City", registrationMV.CityID);
            return View(registrationMV);
        }



        public ActionResult SeekerUser()
        {
            ViewBag.BloodGroupID = new SelectList(DB.BloodGroupsTables.ToList(), "BloodGroupID", "BloodGroup", "0");
            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "City", "0");
            ViewBag.GenderID = new SelectList(DB.GenderTables.ToList(), "GenderID", "Gender", "0");
            return View(registrationmv);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult SeekerUser(RegistrationMV registrationMV)
        {
            if (ModelState.IsValid)
            {
                var checktitle = DB.SeekerTables.Where(h => h.Name == registrationMV.Seeker.Name.Trim() && h.CardNumber == registrationMV.Seeker.CardNumber).FirstOrDefault();
                if (checktitle == null)
                {
                    using (var transaction = DB.Database.BeginTransaction())
                    {
                        try
                        {
                            var user = new UserTable();
                            user.Username = registrationMV.User.Username;
                            user.Password = registrationMV.User.Password;
                            user.Email = registrationMV.User.Email;
                            user.AccountStatusID = 1;
                            user.UserTypeID = registrationMV.UserTypeID;
                            DB.UserTables.Add(user);
                            DB.SaveChanges();

                            var seeker = new SeekerTable();
                            seeker.Name = registrationMV.Seeker.Name;
                            seeker.Surname = registrationMV.Seeker.Surname;
                            seeker.BloodGroupID = registrationMV.BloodGroupID;
                            seeker.Address = registrationMV.Seeker.Address;
                            seeker.Email = registrationMV.Seeker.Email;
                            seeker.CardNumber = registrationMV.Seeker.CardNumber;
                            seeker.GenderID = registrationMV.GenderID;
                            seeker.RegistrationDate = DateTime.Now;
                            seeker.DateOfBirth = registrationMV.Seeker.DateOfBirth;
                            seeker.ContactNumber = registrationMV.Seeker.ContactNumber;
                            seeker.CityID = registrationMV.CityID;
                            seeker.UserID = user.UserID;
                            DB.SeekerTables.Add(seeker);
                            DB.SaveChanges();
                            transaction.Commit();
                            ViewData["Message"] = "Thanks for registration, your query will be reviewed shortly!";
                            return RedirectToAction("MainHome", "Home");
                        }
                        catch
                        {
                            ModelState.AddModelError(string.Empty, "Please provide correct information!");
                            transaction.Rollback();
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Seeker already registered!");
                }
            }
            ViewBag.BloodGroupID = new SelectList(DB.BloodGroupsTables.ToList(), "BloodGroupID", "BloodGroup", registrationMV.BloodGroupID);
            ViewBag.GenderID = new SelectList(DB.GenderTables.ToList(), "GenderID", "Gender", registrationMV.GenderID);
            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "City", registrationMV.CityID);
            return View(registrationMV);
        }


    }
}