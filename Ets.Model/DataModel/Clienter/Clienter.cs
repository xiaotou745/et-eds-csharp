using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Model.Common;


namespace Ets.Model.DataModel.Clienter
{

    public class clienter
    {
        public int Id { get; set; }
        public string PhoneNo { get; set; }
        public string LoginName { get; set; }
        public string recommendPhone { get; set; }
        public string Password { get; set; }
        public string TrueName { get; set; }
        public string IDCard { get; set; }
        public string PicWithHandUrl { get; set; }
        public string PicUrl { get; set; }
        public Nullable<byte> Status { get; set; }
        public Nullable<decimal> AccountBalance { get; set; }
        public Nullable<System.DateTime> InsertTime { get; set; }
        public string InviteCode { get; set; }
        public string City { get; set; }
        public string CityId { get; set; }
        public Nullable<int> GroupId { get; set; }
        public string HealthCardID { get; set; }
        public string InternalDepart { get; set; }
        public string ProvinceCode { get; set; }
        public string AreaCode { get; set; }
        public string CityCode { get; set; }
        public string Province { get; set; }
        public Nullable<int> BussinessID { get; set; }
        public int WorkStatus { get; set; }
    }

    public class ClientOrderSearchCriteria
    {
        public PagingResult PagingRequest { get; set; }
        public int userId { get; set; }
        public sbyte? status { get; set; }
        public bool isLatest { get; set; }
        public double longitude { get; set; }
        public double latitude { get; set; }
        //城市名称
        public string city { get; set; }
        //城市Id
        public string cityId { get; set; }
        /// <summary>
        /// 订单类型：1送餐订单，2收锅订单
        /// </summary>
        public int OrderType { get; set; }

    }    
}
