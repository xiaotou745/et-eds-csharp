﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DataModel.Bussiness
{
    public partial class Business
    {
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
        public Nullable<byte> Status { get; set; }
        public Nullable<System.DateTime> InsertTime { get; set; }
        public string districtId { get; set; }
        public string CityId { get; set; }
        public Nullable<int> GroupId { get; set; }
        
        /// <summary>
        /// 第三方商户id
        /// </summary>
        public Nullable<int> OriginalBusiId { get; set; }
        public string ProvinceCode { get; set; }
        public string CityCode { get; set; }
        public string AreaCode { get; set; }
        public string Province { get; set; }
        public Nullable<int> CommissionTypeId { get; set; }
        public Nullable<decimal> DistribSubsidy { get; set; }
        public Nullable<decimal> BusinessCommission { get; set; }

        
        public int oldOriginalBusiId { get; set; }
        public int oldGroupId { get; set; }
    }

}
