using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KapiNadeApp.Models
{
    public class BloodStockMV
    {
        public int BloodStockID { get; set; }
        public int BloodGroupID { get; set; }
        [Display(Name = "Blood Group")]
        public string BloodGroup { get; set; }
        [Display(Name = "Blood Bank")]
        public int BloodBankID { get; set; }
        public string BloodBank { get; set; }
        [Display(Name="Is ready")]
        public string Status { get; set; }
        public int Quantity { get; set; }
        [DataType(DataType.Date)]
        public System.DateTime BestBefore { get; set; }

    }
}