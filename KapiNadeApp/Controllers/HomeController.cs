﻿using KapiNadeApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DatabaseLayer;
using System.Web.Helpers;
using System.Security.Cryptography;
using System.Text;



namespace KapiNadeApp.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        KapiNadeDBEntities DB= new KapiNadeDBEntities();

        public ActionResult AllCampaigns()
        {

            var date = DateTime.Now.Date;
            var allcampaigns = DB.CampaignTables.Where(c => c.CampaignDate >= date).OrderBy(c => c.CampaignDate).ToList();
            return View(allcampaigns);
        }

        public ActionResult MainHome()
        {
            var message = ViewData["Message"] == null ? "Dobro došli na web stranicu Kapi Nade!" : ViewData["Message"];
            ViewData["Message"] = message;

            var date = DateTime.Now.Date;
            var allcampaigns = DB.CampaignTables.Where(c => c.CampaignDate >= date).ToList();
            ViewBag.AllCampaigns = allcampaigns;

            var registration = new RegistrationMV();
            ViewBag.UserTypeID = new SelectList(DB.UserTypeTables.Where(ut=>ut.UserTypeID > 1).ToList(), "UserTypeID", "UserType", "0");
            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "City", "0");
            return View(registration);
        }

        public ActionResult Login()
        {
            var usermv = new UserMV();
            return View(usermv);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserMV userMV)
        {
            if(ModelState.IsValid)
            {
                if (userMV.Password != null && userMV.UsernameOrEmail != null)
                {

                    string decryptedPassword;
                    using (SHA256 sha256 = SHA256.Create())
                    {
                        byte[] inputBytes = Encoding.UTF8.GetBytes(userMV.Password);
                        byte[] hashBytes = sha256.ComputeHash(inputBytes);
                        decryptedPassword = Convert.ToBase64String(hashBytes);
                    }



                    var user = DB.UserTables.Where(u => u.Password == decryptedPassword && (u.Email == userMV.UsernameOrEmail || u.Username == userMV.UsernameOrEmail)).FirstOrDefault();
                    if (user != null)
                    {
                        if (user.AccountStatusID == 1)
                        {
                            ModelState.AddModelError(string.Empty, "Vaš račun još nije odobren! Molimo Vas da pričekate.");
                        }
                        else if (user.AccountStatusID == 3)
                        {
                            ModelState.AddModelError(string.Empty, "Vaš račun je odbijen! Za više informacija, kontaktirajte nas.");
                        }
                        else if (user.AccountStatusID == 2)
                        {
                            Session["UserID"] = user.UserID;
                            Session["Username"] = user.Username;
                            Session["Password"] = user.Password;
                            Session["Email"] = user.Email;
                            Session["AccountStatusID"] = user.AccountStatusID;
                            Session["AccountStatus"] = user.AccountStatusTable.AccountStatus;
                            Session["UserTypeID"] = user.UserTypeID;
                            Session["UserType"] = user.UserTypeTable.UserType;



                            if (user.UserTypeID == 1) // Admin
                            {
                                return RedirectToAction("AllNewUserRequests", "Accounts");
                            }
                            else if (user.UserTypeID == 2) // Donor
                            {
                                var donor = DB.DonorTables.Where(u => u.UserID == user.UserID).FirstOrDefault();
                                if (donor != null)
                                {
                                    Session["DonorID"] = donor.DonorID;
                                    Session["Name"] = donor.Name;
                                    Session["Surname"] = donor.Surname;

                                    Session["Email"] = donor.Email;
                                    Session["DateOfBirth"] = donor.DateOfBirth;

                                    Session["GenderID"] = donor.GenderID;
                                    Session["BloodGroupID"] = donor.BloodGroupID;
                                    Session["BloodGroup"] = donor.BloodGroupsTable.BloodGroup;
                                    Session["LastDonationDate"] = donor.LastDonationDate;
                                    Session["ContactNumber"] = donor.ContactNumber;
                                    Session["CardNumber"] = donor.CardNumber;
                                    Session["Address"] = donor.Address;
                                    Session["CityID"] = donor.CityID;
                                    Session["City"] = donor.CityTable.City;
                                    return RedirectToAction("Donor", "Dashboard");
                                }
                                else
                                {


                                    ModelState.AddModelError(string.Empty, "Podaci nisu ispravni!");
                                }
                            }
                            else if (user.UserTypeID == 3) // Seeker
                            {
                                var seeker = DB.SeekerTables.Where(u => u.UserID == user.UserID).FirstOrDefault();
                                if (seeker != null)
                                {
                                    Session["SeekerID"] = seeker.SeekerID;
                                    Session["Name"] = seeker.Name;
                                    Session["Surname"] = seeker.Surname;
                                    Session["DateOfBirth"] = seeker.DateOfBirth;
                                    Session["CityID"] = seeker.CityID;
                                    Session["City"] = seeker.CityTable.City;
                                    Session["BloodGroupID"] = seeker.BloodGroupID;
                                    Session["BloodGroup"] = seeker.BloodGroupsTable.BloodGroup;
                                    Session["ContactNumber"] = seeker.ContactNumber;
                                    Session["CardNumber"] = seeker.CardNumber;
                                    Session["GenderID"] = seeker.GenderID;
                                    Session["Gender"] = seeker.GenderTable.Gender;
                                    Session["RegistrationDate"] = seeker.RegistrationDate;
                                    Session["Address"] = seeker.Address;
                                    Session["Email"] = seeker.Email;
                                    return RedirectToAction("Seeker", "Dashboard");
                                }
                                else
                                {
                                    ModelState.AddModelError(string.Empty, "Podaci nisu ispravni!");
                                }
                            }

                            else if (user.UserTypeID == 4) // Hospital
                            {
                                var hospital = DB.HospitalTables.Where(u => u.UserID == user.UserID).FirstOrDefault();
                                if (hospital != null)
                                {
                                    Session["HospitalID"] = hospital.HospitalID;
                                    Session["Name"] = hospital.Name;
                                    Session["Address"] = hospital.Address;
                                    Session["ContactNumber"] = hospital.ContactNumber;
                                    Session["Email"] = hospital.Email;
                                    Session["CityID"] = hospital.CityID;
                                    Session["City"] = hospital.CityTable.City;
                                    return RedirectToAction("Hospital", "Dashboard");
                                }
                                else
                                {
                                    ModelState.AddModelError(string.Empty, "Podaci nisu ispravni!");
                                }
                            }

                            else if (user.UserTypeID == 5) // Blood Bamk
                            {
                                var bloodbank = DB.BloodBankTables.Where(u => u.UserID == user.UserID).FirstOrDefault();
                                if (bloodbank != null)
                                {
                                    Session["BloodBankID"] = bloodbank.BloodBankID;
                                    Session["Name"] = bloodbank.Name;
                                    Session["Address"] = bloodbank.Address;
                                    Session["ContactNumber"] = bloodbank.ContactNumber;
                                    Session["Email"] = bloodbank.Email;
                                    Session["CityID"] = bloodbank.CityID;
                                    Session["City"] = bloodbank.CityTable.City;
                                    return RedirectToAction("BloodBank", "Dashboard");

                                }
                                else
                                {
                                    ModelState.AddModelError(string.Empty, "Podaci nisu ispravni!");
                                }
                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, "Podaci nisu ispravni!");
                            }


                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Podaci nisu ispravni!");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Podaci nisu ispravni!");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Molimo vas da unesete korisničko ime/e-mail i šifru.");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Molimo vas da unesete korisničko ime/e-mail i šifru.");
            }
            ClearSession();
            return View(userMV);
        }


        private void ClearSession()
        {
            Session["UserID"] = string.Empty;
            Session["Username"] = string.Empty;
            Session["Password"] = string.Empty;
            Session["Email"] = string.Empty;
            Session["AccountStatusID"] = string.Empty;
            Session["AccountStatus"] = string.Empty;
            Session["UserTypeID"] = string.Empty;
            Session["UserType"] = string.Empty;
        }
        public ActionResult Logout()
        {
            ClearSession();
            return RedirectToAction("MainHome");
        }

        public ActionResult Index()
        {
            
            return RedirectToAction("MainHome");
        }

        public ActionResult Contact()
        {
            return View();
        }
        public ActionResult Faq()
        {
            return View();
        }
    }
}