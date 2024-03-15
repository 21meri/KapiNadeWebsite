using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KapiNadeApp.Models
{
    public class CollectBloodMV
    {
        public CollectBloodMV() 
        {
            DonorDetails = new CollectBloodDonorDetailMV();
        }
        public int BloodStockDetailsID { get; set; }
        public int BloodStockID { get; set; }
        public int BloodGroupID { get; set; }
        public int CampaignID { get; set; }
        public double Quantity { get; set; }
        public int DonorID { get; set; }
        public int GenderID { get; set; }
        public int CityID { get; set; }

        public System.DateTime DonationDateTime { get; set; }
        public CollectBloodDonorDetailMV DonorDetails { get; set; }
    }
}