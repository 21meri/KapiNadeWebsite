using DatabaseLayer;
using KapiNadeApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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


        public ActionResult EditUserProfile(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["Username"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var userprofile = new RegistrationMV();
            var user = DB.UserTables.Find(id);

            userprofile.UserTypeID = user.UserTypeID;


            userprofile.User.UserID = user.UserID;
            userprofile.User.Username = user.Username;
            userprofile.User.Email = user.Email;
            userprofile.User.AccountStatusID = user.AccountStatusID;
            userprofile.User.UserTypeID = user.UserTypeID;




            if (user.SeekerTables.Count > 0)
            {
                var seeker = user.SeekerTables.FirstOrDefault();
                userprofile.Seeker.SeekerID = seeker.SeekerID;
                userprofile.Seeker.Name = seeker.Name;
                userprofile.Seeker.Surname = seeker.Surname;
                userprofile.Seeker.DateOfBirth = seeker.DateOfBirth;
                userprofile.Seeker.CityID = seeker.CityID;
                userprofile.Seeker.BloodGroupID = seeker.BloodGroupID;
                userprofile.Seeker.ContactNumber = seeker.ContactNumber;
                userprofile.Seeker.Email = seeker.Email;
                userprofile.Seeker.CardNumber = seeker.CardNumber;
                userprofile.Seeker.GenderID = seeker.GenderID;
                userprofile.Seeker.Address = seeker.Address;
                userprofile.Seeker.UserID = seeker.UserID;

                userprofile.ContactNumber = seeker.ContactNumber;
                userprofile.CityID = seeker.CityID;
                userprofile.BloodGroupID = seeker.BloodGroupID;
                userprofile.GenderID = seeker.GenderID;



            }

            else if (user.HospitalTables.Count > 0)
            {
                var hospital = user.HospitalTables.FirstOrDefault();
                userprofile.Hospital.HospitalID = hospital.HospitalID;
                userprofile.Hospital.Name = hospital.Name;
                userprofile.Hospital.Address = hospital.Address;
                userprofile.Hospital.ContactNumber = hospital.ContactNumber;
                userprofile.Hospital.Email = hospital.Email;
                userprofile.Hospital.CityID = hospital.CityID;
                userprofile.Hospital.UserID = hospital.UserID;

                userprofile.ContactNumber = hospital.ContactNumber;
                userprofile.CityID = hospital.CityID;



            }

            else if (user.BloodBankTables.Count > 0)
            {
                var bloodbank = user.BloodBankTables.FirstOrDefault();
                userprofile.BloodBank.BloodBankID = bloodbank.BloodBankID;
                userprofile.BloodBank.Name = bloodbank.Name;
                userprofile.BloodBank.Address = bloodbank.Address;
                userprofile.BloodBank.ContactNumber = bloodbank.ContactNumber;
                userprofile.BloodBank.Email = bloodbank.Email;
                userprofile.BloodBank.CityID = bloodbank.CityID;
                userprofile.BloodBank.UserID = bloodbank.UserID;

                userprofile.ContactNumber = bloodbank.ContactNumber;
                userprofile.CityID = bloodbank.CityID;



            }

            else if (user.DonorTables.Count > 0)
            {
                var donor = user.DonorTables.FirstOrDefault();
                userprofile.Donor.DonorID = donor.DonorID;
                userprofile.Donor.Name = donor.Name;
                userprofile.Donor.Surname = donor.Surname;
                userprofile.Donor.GenderID = donor.GenderID;
                userprofile.Donor.BloodGroupID = donor.BloodGroupID;
                userprofile.Donor.LastDonationDate = donor.LastDonationDate;
                userprofile.Donor.ContactNumber = donor.ContactNumber;
                userprofile.Donor.CardNumber = donor.CardNumber;
                userprofile.Donor.Address = donor.Address;
                userprofile.Donor.CityID = donor.CityID;
                userprofile.Donor.UserID = donor.UserID;

                userprofile.ContactNumber = donor.ContactNumber;
                userprofile.CityID = donor.CityID;
                userprofile.BloodGroupID = donor.BloodGroupID;
                userprofile.GenderID = donor.GenderID;





            }

            ViewBag.BloodGroupID = new SelectList(DB.BloodGroupsTables.ToList(), "BloodGroupID", "BloodGroup", userprofile.BloodGroupID);
            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "City", userprofile.CityID);
            ViewBag.GenderID = new SelectList(DB.GenderTables.ToList(), "GenderID", "Gender", userprofile.GenderID);


            return View(userprofile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUserProfile(RegistrationMV userprofile)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["Username"])))
            {
                return RedirectToAction("Login", "Home");
            }
            //var userprofile = new RegistrationMV();
            //var user = DB.UserTables.Find(id);
            ViewBag.BloodGroupID = new SelectList(DB.BloodGroupsTables.ToList(), "BloodGroupID", "BloodGroup", userprofile.BloodGroupID);
            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "City", userprofile.CityID);
            ViewBag.GenderID = new SelectList(DB.GenderTables.ToList(), "GenderID", "Gender", userprofile.GenderID);

            return View(userprofile);
           
        }
    }
}