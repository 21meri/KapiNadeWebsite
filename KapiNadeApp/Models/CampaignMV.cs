using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KapiNadeApp.Models
{
    public class CampaignMV
    {
        public int CampaignID { get; set; }
        [Display(Name = "Campaign Photo")]
        public string CampaignPhoto { get; set; }
        [Display(Name = "Campaign Title")]
        [Required(ErrorMessage = "*Obavezno ispuniti")]
        public string CampaignTitle { get; set; }
        [Display(Name="Blood Bank")]
        [Required(ErrorMessage = "*Obavezno ispuniti")]
        public int BloodBankID { get; set; }
        [Display(Name = "Campaign Date")]
        [Required(ErrorMessage = "*Obavezno ispuniti")]
        [DataType(DataType.Date)]
        public System.DateTime CampaignDate { get; set; }
        [Display(Name = "Start Time")]
        [Required(ErrorMessage = "*Obavezno ispuniti")]

        public System.TimeSpan StartTime { get; set; }
        [Display(Name = "End Time")]
        [Required(ErrorMessage = "*Obavezno ispuniti")]


        public System.TimeSpan EndTime { get; set; }
        [Display(Name = "location")]
        [Required(ErrorMessage = "*Obavezno ispuniti")]
        public string Location { get; set; }
        [Display(Name = "Campaign Details")]
        [Required(ErrorMessage = "*Obavezno ispuniti")]
        public string CampaignDetails { get; set; }

        [NotMapped]
        public HttpPostedFileBase CampaignPhotoFile { get; set; }

    }
}