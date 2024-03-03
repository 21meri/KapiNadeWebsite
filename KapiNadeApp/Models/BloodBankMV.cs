using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KapiNadeApp.Models
{
    public class BloodBankMV
    {
        public int BloodBankID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public int CityID { get; set; }
        public string City { get; set; }
        public int UserID { get; set; }
        public string Username { get; set; }
    }
}