﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DomainModel.Clienter
{
    public class ClientOrderModel
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public string OrderNo { get; set; }
        public int OrderFrom { get; set; }
        public string OriginalOrderNo { get; set; }
        public string PubDate { get; set; }
        public string PickUpAddress { get; set; }
        public string ReceviceName { get; set; }
        public string ReceviceCity { get; set; }
        public string ReceviceAddress { get; set; }
        public string RecevicePhoneNo { get; set; }
        public bool IsPay { get; set; }
        public string Remark { get; set; }
        public byte Status { get; set; }
        public double? ReceviceLongitude { get; set; }
        public double? ReceviceLatitude { get; set; }
        public decimal CommissionRate { get; set; }
        public int OrderCount { get; set; }
        public decimal DistribSubsidy { get; set; }
        public decimal WebsiteSubsidy { get; set; }
        public decimal Amount { get; set; }

        public int BusinessId { get; set; }
        public string BusinessName { get; set; }
        public string BusinessPhone { get; set; }
        public string BusinessPhone2 { get; set; }
        public string pickUpCity { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public decimal? OrderCommission { get; set; }
       
        /// <summary>
        /// 集团id
        /// </summary>
        public int GroupId { get; set; }
        /// <summary>
        /// 已经上传的小票数量
        /// </summary>
        public int HadUploadCount { get; set; }
    }
}
