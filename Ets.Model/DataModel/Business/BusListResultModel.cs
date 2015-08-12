using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DataModel.Business
{
    public class BusListResultModel
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
        public Nullable<int> OriginalBusiId { get; set; }
        public string ProvinceCode { get; set; }
        public string CityCode { get; set; }
        public string AreaCode { get; set; }
        public string Province { get; set; }
        public int? CommissionTypeId { get; set; }
        
        /// <summary>
        /// 外送费
        /// </summary>
        public decimal? DistribSubsidy { get; set; }
        
        /// <summary>
        /// 结算比例
        /// </summary>
        public decimal? BusinessCommission { get; set; }
        public string GroupName {get;set;}
        //public virtual ICollection<order> order { get; set; }
        /// <summary>
        /// 结算类型：1：固定比例 2：固定金额
        /// </summary>
        public int CommissionType { get; set; }
        /// <summary>
        /// 固定金额
        /// </summary>
        public decimal CommissionFixValue { get; set; }
        /// <summary>
        /// 分组ID
        /// </summary>
        public int BusinessGroupId { get; set; }
        /// <summary>
        /// 策略ID
        /// </summary>
        public int StrategyId { get; set; }
        /// <summary>
        /// 分组名称
        /// </summary>
        public string BusinessGroupName { get; set; }

        /// <summary>
        /// 餐费结算方式（0：线下结算 1：线上结算）
        /// </summary>
        public int MealsSettleMode { get; set; }
        /// <summary>
        /// 账户余额
        /// </summary>
        public decimal BalancePrice { get; set; }
        /// <summary>
        /// 可提现余额
        /// </summary>
        public decimal AllowWithdrawPrice { get; set; }
        /// <summary>
        /// 是否一键发单
        /// </summary>
        public int OneKeyPubOrder { get; set; }
        /// <summary>
        /// 推荐人手机
        /// </summary>
        public string RecommendPhone { get; set; }
        /// <summary>
        /// 是否允许透支 0不可透支，1可以透支
        /// </summary>
        public int IsAllowOverdraft { get; set; }

        /// <summary>
        ///  是否雇主任务
        /// </summary>
        public int IsEmployerTask { get; set; }
        /// <summary>
        /// 是否需要审核
        /// </summary>
        public int IsOrderChecked { get; set; }


    }
}
