//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SuperManDataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class business
    {
        public business()
        {
            this.order = new HashSet<order>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string district { get; set; }
        public string PhoneNo { get; set; }
        public string PhoneNo2 { get; set; }
        public string Password { get; set; }
        public string CheckPicUrl { get; set; }
        public string IDCard { get; set; }
        public string Address { get; set; }
        public string Landline { get; set; }
        public Nullable<double> Longitude { get; set; }
        public Nullable<double> Latitude { get; set; }
        public Nullable<sbyte> Status { get; set; }
        public Nullable<System.DateTime> InsertTime { get; set; }
        public string districtId { get; set; }
        public string CityId { get; set; }
        public Nullable<int> GroupId { get; set; }
        public Nullable<int> OriginalBusiId { get; set; }
        public string ProvinceCode { get; set; }
        public string CityCode { get; set; }
        public string AreaCode { get; set; }
        public string Province { get; set; }
        public Nullable<int> CommissionTypeId { get; set; }
        public Nullable<decimal> DistribSubsidy { get; set; }
    
        public virtual ICollection<order> order { get; set; }
    }
}
