using KapiNadeApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DatabaseLayer;


namespace KapiNadeApp.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        KapiNadeDBEntities DB= new KapiNadeDBEntities();

        public ActionResult AllCampaigns()
        {

            var date = DateTime.Now.Date;
            var allcampaigns = DB.CampaignTables.Where(c => c.CampaignDate >= date).ToList();
            return View(allcampaigns);
        }

        public ActionResult MainHome()
        {
            var message = ViewData["Message"] == null ? "Welcome to Kapi Nade website!" : ViewData["Message"];
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
                var user = DB.UserTables.Where(u => u.Password == userMV.Password && (u.Email == userMV.UsernameOrEmail || u.Username == userMV.UsernameOrEmail)).FirstOrDefault();
                if(user != null)
                {
                    if(user.AccountStatusID == 1)
                    {
                        ModelState.AddModelError(string.Empty, "Please wait, your account is under review!");
                    }
                    else if(user.AccountStatusID == 3)
                    {
                        ModelState.AddModelError(string.Empty, "Your account is rejected! For more details, comtact us.");
                    }
                    else if (user.AccountStatusID == 5)
                    {
                        ModelState.AddModelError(string.Empty, "Your account is suspended! For more details, comtact us.");
                    }
                    else if(user.AccountStatusID == 2)
                    {
                        Session["UserID"] = user.UserID;
                        Session["Username"] = user.Username;
                        Session["Password"] = user.Password;
                        Session["Email"] = user.Email;
                        Session["AccountStatusID"] = user.AccountStatusID;
                        Session["AccountStatus"] = user.AccountStatusTable.AccountStatus;
                        Session["UserTypeID"] = user.UserTypeID;
                        Session["UserType"] = user.UserTypeTable.UserType;


                        
                        if(user.UserTypeID == 1) // Admin
                        {
                            return RedirectToAction("AllNewUserRequests","Accounts");
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
                                return RedirectToAction("Donor","Dashboard");
                            }
                            else
                            {
                                

                                ModelState.AddModelError(string.Empty, "Username and password is incorrect");
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
                                ModelState.AddModelError(string.Empty, "Username and password is incorrect");
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
                                ModelState.AddModelError(string.Empty, "Username and password is incorrect");
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
                                ModelState.AddModelError(string.Empty, "Username and password is incorrect");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Username and password is incorrect");
                        }


                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Username and password is incorrect");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Username and password is incorrect");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Please provide username/email and password");
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