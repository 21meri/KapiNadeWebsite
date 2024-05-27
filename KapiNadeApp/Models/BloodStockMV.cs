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
        public int BloodBankID { get; set; }
        [Display(Name = "Blood Bank")]
        public string BloodBank { get; set; }

        public int BloodGroupID { get; set; }
        [Display(Name = "Blood Group")]
        public string BloodGroup { get; set; }
        [Required(ErrorMessage = "*Obavezno ispuniti")]
        [Display(Name = "Quantity")]

        public double Quantity { get; set; }

        [Display(Name="Is ready")]
        public string Status { get; set; }
    }
}