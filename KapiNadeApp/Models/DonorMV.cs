using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KapiNadeApp.Models
{
    public class DonorMV
    {
        public int DonorID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int BloodGroupID { get; set; }
        public string BloodGroup { get; set; }
        public System.DateTime LastDonationDate { get; set; }
        public string ContactNumber { get; set; }
        public string CardNumber { get; set; }
        public string Address { get; set; }
        public int CityID { get; set; }
        public string City { get; set; }
        public int UserID { get; set; }
        public string Username { get; set; }
    }
}