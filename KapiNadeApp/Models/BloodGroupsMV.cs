using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace KapiNadeApp.Models
{
    public class BloodGroupsMV
    {


        public int BloodGroupID { get; set; }
        [Required(ErrorMessage = "*Obavezno ispuniti")]
        [Display(Name = "Blood Group")]
        public string BloodGroup { get; set; }


    }
}