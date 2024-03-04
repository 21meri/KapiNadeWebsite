using DatabaseLayer;
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
                    var user = new UserTable();
                    user.Username = registrationMV.User.Username;
                    user.Password = registrationMV.User.Password;
                    user.Email = registrationMV.User.Email;
                    user.AccountStatusID = 1;
                    user.UserTypeID = registrationMV.UserTypeID;
                    DB.UserTables.Add(user);

                    var hospital = new HospitalTable();
                    hospital.Name = registrationMV.Hospital.Name;
                    hospital.Address = registrationMV.Hospital.Address;
                    hospital.ContactNumber = registrationMV.Hospital.ContactNumber;
                    hospital.Email = registrationMV.Hospital.Email;
                    hospital.CityID = registrationMV.CityID;
                    hospital.UserID = user.UserID;
                    DB.HospitalTables.Add(hospital);
                    DB.SaveChanges();
                    return RedirectToAction("MainHome", "Home");
                }
    }
            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "City", registrationMV.CityID);
            return View(registrationMV);
        }





        public ActionResult DonorUser()
        {
            ViewBag.UserTypeID = new SelectList(DB.UserTypeTables.Where(ut => ut.UserTypeID > 1).ToList(), "UserTypeID", "UserType", registrationmv.UserTypeID);
            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "City", registrationmv.CityID);
            return View(registrationmv);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult DonorUser(RegistrationMV registrationMV)
        {
            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "City", registrationmv.CityID);
            return View();
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
            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "City", registrationmv.CityID);
            return View();
        }



        public ActionResult SeekerUser()
        {
            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "City", registrationmv.CityID);
            return View(registrationmv);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult SeekerUser(RegistrationMV registrationMV)
        {
            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "City", registrationmv.CityID);
            return View();
        }


    }
}