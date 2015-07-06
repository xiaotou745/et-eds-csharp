using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DataModel.Clienter
{
    public class ClienterListModel
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
        /// <summary>
        /// 跨店ID
        /// </summary>
        public int CSID { get; set; }

        public decimal AllowWithdrawPrice { get; set; }
        /// <summary>
        /// 是否绑定（0：否 1：是）
        /// </summary>
        public int IsBind { get; set; }
        /// <summary>
        /// 物流公司名称
        /// </summary>
        public string CompanyName { get; set; }

    }
}
