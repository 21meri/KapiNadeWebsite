﻿using KapiNadeApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DatabaseLayer;
using System.Security.Cryptography;
using System.Text;
using System.Drawing;

namespace KapiNadeApp.Controllers
{
    public class AccountsController : Controller
    {
        KapiNadeDBEntities DB = new KapiNadeDBEntities();
        public ActionResult AllNewUserRequests()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["Username"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var users = DB.UserTables.Where(u => u.AccountStatusID == 1).ToList();
            return View(users);
        }

        public ActionResult UserDetails(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["Username"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var user = DB.UserTables.Find(id);
            return View(user);
        }
        public ActionResult UserApproved(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["Username"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var user = DB.UserTables.Find(id);
            user.AccountStatusID = 2;
            DB.Entry(user).State = System.Data.Entity.EntityState.Modified;
            DB.SaveChanges();
            return RedirectToAction("AllNewUserRequests");
        }

        public ActionResult UserRejected(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["Username"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var user = DB.UserTables.Find(id);
            user.AccountStatusID = 3;
            DB.Entry(user).State = System.Data.Entity.EntityState.Modified;
            DB.SaveChanges();
            return RedirectToAction("AllNewUserRequests");
        }

        public ActionResult AddNewDonorByBloodBank()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["Username"])))
            {
                return RedirectToAction("Login", "Home");
            }

            var collectbloodMV = new CollectBloodMV();

            ViewBag.BloodGroupID = new SelectList(DB.BloodGroupsTables.ToList(), "BloodGroupID", "BloodGroup", "0");
            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "City", "0");
            ViewBag.GenderID = new SelectList(DB.GenderTables.ToList(), "GenderID", "Gender", "0");


            return View(collectbloodMV);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult AddNewDonorByBloodBank(CollectBloodMV collectBloodMV)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["Username"])))
            {
                return RedirectToAction("Login", "Home");
            }


            int bloodbankID = 0;
            string bloodbankid = Convert.ToString(Session["BloodBankID"]);
            int.TryParse(bloodbankid, out bloodbankID);
            var currentdate = DateTime.Now.Date;

            var currentcampaign = DB.CampaignTables.Where(c => c.CampaignDate == currentdate && c.BloodBankID == bloodbankID).FirstOrDefault();
            if (currentcampaign != null)
            {

                if (ModelState.IsValid)
                {
                    using (var transaction = DB.Database.BeginTransaction())
                    {
                        try
                        {

                            var checkdonor = DB.DonorTables.Where(d => d.CardNumber.Trim().Replace("-", "") == collectBloodMV.DonorDetails.CardNumber.Trim().Replace("-", "")).FirstOrDefault();
                            if (checkdonor != null)
                            {
                                int donationThresholdDays = checkdonor.GenderID == 1 ? 90 : 120;
                                if ((DateTime.Now - checkdonor.LastDonationDate).TotalDays < donationThresholdDays)
                                {
                                    ModelState.AddModelError(string.Empty, "Davalac je već nedavno donirao krv.");
                                    transaction.Rollback();



                                }
                                else
                                {
                                    var checkbloodgroupstock = DB.BloodStockTables.Where(s => s.BloodBankID == bloodbankID && s.BloodGroupID == collectBloodMV.BloodGroupID).FirstOrDefault();
                                    if (checkbloodgroupstock == null)
                                    {
                                        var bloodstock = new BloodStockTable();


                                        bloodstock.BloodGroupID = collectBloodMV.BloodGroupID;
                                        bloodstock.BloodBankID = bloodbankID;
                                        bloodstock.Status = true;
                                        bloodstock.Quantity = 0;
                                        DB.BloodStockTables.Add(bloodstock);
                                        DB.SaveChanges();

                                        checkbloodgroupstock = DB.BloodStockTables.Where(s => s.BloodBankID == bloodbankID && s.BloodGroupID == collectBloodMV.BloodGroupID).FirstOrDefault();

                                    }
                                    checkbloodgroupstock.Quantity += collectBloodMV.Quantity;
                                    DB.Entry(checkbloodgroupstock).State = System.Data.Entity.EntityState.Modified;
                                    DB.SaveChanges();

                                    var collectblooddetails = new BloodStockDetailsTable();
                                    collectblooddetails.BloodStockID = checkbloodgroupstock.BloodStockID;
                                    collectblooddetails.BloodGroupID = collectBloodMV.BloodGroupID;
                                    collectblooddetails.CampaignID = currentcampaign.CampaignID;

                                    collectblooddetails.Quantity = collectBloodMV.Quantity;
                                    collectblooddetails.DonorID = checkdonor.DonorID;
                                    collectblooddetails.DonationDateTime = DateTime.Now;
                                    DB.BloodStockDetailsTables.Add(collectblooddetails);
                                    DB.SaveChanges();

                                    checkdonor.LastDonationDate = DateTime.Now;
                                    DB.Entry(checkdonor).State = System.Data.Entity.EntityState.Modified;
                                    DB.SaveChanges();
                                    transaction.Commit();
                                    return RedirectToAction("BloodStock", "BloodBank");
                                }
                            }

                            else
                            {
                                var user = new UserTable();

                                Random random = new Random();
                                int randomNumber = random.Next(0, 9999);

                                string firstName = collectBloodMV.DonorDetails.Name.Trim();
                                string lastName = collectBloodMV.DonorDetails.Surname.Trim();
                                string username = firstName.ToLower() + "." + lastName.ToLower() + randomNumber;
                                user.Username = username;
                                string hashedPassword = EncryptPassword("kapinade2024");
                                user.Password = hashedPassword;
                                user.Email = collectBloodMV.DonorDetails.Email;
                                user.AccountStatusID = 2;
                                user.UserTypeID = 2;
                                DB.UserTables.Add(user);
                                DB.SaveChanges();

                                var donor = new DonorTable();
                                donor.Name = collectBloodMV.DonorDetails.Name;
                                donor.Surname = collectBloodMV.DonorDetails.Surname;
                                donor.BloodGroupID = collectBloodMV.BloodGroupID;
                                donor.Address = collectBloodMV.DonorDetails.Address;
                                donor.CardNumber = collectBloodMV.DonorDetails.CardNumber;
                                donor.GenderID = collectBloodMV.GenderID;
                                donor.LastDonationDate = DateTime.Now;

                                donor.Email = collectBloodMV.DonorDetails.Email;
                                donor.DateOfBirth = collectBloodMV.DonorDetails.DateOfBirth;

                                donor.ContactNumber = collectBloodMV.DonorDetails.ContactNumber;
                                donor.CityID = collectBloodMV.CityID;
                                donor.UserID = user.UserID;
                                DB.DonorTables.Add(donor);
                                DB.SaveChanges();
                                checkdonor = DB.DonorTables.Where(d => d.CardNumber.Trim().Replace("-", "") == collectBloodMV.DonorDetails.CardNumber.Trim().Replace("-", "")).FirstOrDefault();

                                var checkbloodgroupstock = DB.BloodStockTables.Where(s => s.BloodBankID == bloodbankID && s.BloodGroupID == collectBloodMV.BloodGroupID).FirstOrDefault();
                                if (checkbloodgroupstock == null)
                                {
                                    var bloodstock = new BloodStockTable();


                                    bloodstock.BloodGroupID = collectBloodMV.BloodGroupID;
                                    bloodstock.BloodBankID = bloodbankID;
                                    bloodstock.Status = true;
                                    bloodstock.Quantity = 0;
                                    DB.BloodStockTables.Add(bloodstock);
                                    DB.SaveChanges();

                                    checkbloodgroupstock = DB.BloodStockTables.Where(s => s.BloodBankID == bloodbankID && s.BloodGroupID == collectBloodMV.BloodGroupID).FirstOrDefault();

                                }
                                checkbloodgroupstock.Quantity += collectBloodMV.Quantity;
                                DB.Entry(checkbloodgroupstock).State = System.Data.Entity.EntityState.Modified;
                                DB.SaveChanges();

                                var collectblooddetails = new BloodStockDetailsTable();
                                collectblooddetails.BloodStockID = checkbloodgroupstock.BloodStockID;
                                collectblooddetails.BloodGroupID = collectBloodMV.BloodGroupID;
                                collectblooddetails.CampaignID = currentcampaign.CampaignID;
                                collectblooddetails.Quantity = collectBloodMV.Quantity;
                                collectblooddetails.DonorID = checkdonor.DonorID;
                                collectblooddetails.DonationDateTime = DateTime.Now;
                                DB.BloodStockDetailsTables.Add(collectblooddetails);
                                DB.SaveChanges();

                                checkdonor.LastDonationDate = DateTime.Now;
                                DB.Entry(checkdonor).State = System.Data.Entity.EntityState.Modified;
                                DB.SaveChanges();
                                transaction.Commit();
                                return RedirectToAction("BloodStock", "BloodBank");
                            }
                        }

                        catch
                        {
                            ModelState.AddModelError(string.Empty, "Molimo Vas da unesete ispravne podatke!");
                            transaction.Rollback();
                        }


                    }

                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Molimo Vas da unesete sve potrebne podatke!");

                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Trenutno nema organizovane kampanje za prikupljanje krvi!");

            }

                ViewBag.BloodGroupID = new SelectList(DB.BloodGroupsTables.ToList(), "BloodGroupID", "BloodGroup", collectBloodMV.BloodGroupID);
            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "City", collectBloodMV.CityID);
            ViewBag.GenderID = new SelectList(DB.GenderTables.ToList(), "GenderID", "Gender", collectBloodMV.GenderID);
            return View(collectBloodMV);

        }

        private string EncryptPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}