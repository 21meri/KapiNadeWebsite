using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KapiNadeApp.Models
{
    public class FinderSearchResultsMV
    {
        public int DonorID { get; set; }
        public int UserID { get; set; }

        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        [DataType(DataType.Date)]
        public System.DateTime DateOfBirth { get; set; }
        public string ContactNumber { get; set; }
        public string Address { get; set; }
        public string UserType { get; set; }
        public int UserTypeID { get; set; }
        public int BloodGroupID { get; set; }
        public string BloodGroup { get; set; }








    }
}