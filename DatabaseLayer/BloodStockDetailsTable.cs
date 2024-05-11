//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DatabaseLayer
{
    using System;
    using System.Collections.Generic;
    
    public partial class BloodStockDetailsTable
    {
        public int BloodStockDetailsID { get; set; }
        public int BloodStockID { get; set; }
        public int BloodGroupID { get; set; }
        public int? CampaignID { get; set; }
        public double Quantity { get; set; }
        public int DonorID { get; set; }
        public System.DateTime DonationDateTime { get; set; }
    
        public virtual BloodGroupsTable BloodGroupsTable { get; set; }
        public virtual CampaignTable CampaignTable { get; set; }
        public virtual DonorTable DonorTable { get; set; }
        public virtual BloodStockTable BloodStockTable { get; set; }
    }
}
