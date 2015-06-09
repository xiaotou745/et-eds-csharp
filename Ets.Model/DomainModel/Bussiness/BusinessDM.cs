using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Model.DataModel.Finance;
namespace Ets.Model.DomainModel.Bussiness
{
    public class BusinessDM
    {
        public BusinessDM() { }
		/// <summary>
		/// 主键Id(自增)
		/// </summary>
		public int Id { get; set; }
		/// <summary>
		/// 名称
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// 所在城市（二级）
		/// </summary>
		public string City { get; set; }
		/// <summary>
		/// 区域
		/// </summary>
		public string district { get; set; }
		/// <summary>
		/// 联系方式
		/// </summary>
		public string PhoneNo { get; set; }
		/// <summary>
		/// 备用联系方式
		/// </summary>
		public string PhoneNo2 { get; set; }
		/// <summary>
		/// 系统登录密码
		/// </summary>
		public string Password { get; set; }
        /// <summary>
        /// 执照照片
        /// </summary>
        public string BusinessLicensePic { get; set; }
		/// <summary>
		/// 验证照片
		/// </summary>
		public string CheckPicUrl { get; set; }
		/// <summary>
		/// 身份证号码
		/// </summary>
		public string IDCard { get; set; }
		/// <summary>
		/// 地址
		/// </summary>
		public string Address { get; set; }
		/// <summary>
		/// 固话
		/// </summary>
		public string Landline { get; set; }
		/// <summary>
		/// 经度
		/// </summary>
		public decimal? Longitude { get; set; }
		/// <summary>
		/// 纬度
		/// </summary>
		public decimal? Latitude { get; set; }
		/// <summary>
		/// 状态:0未审核，1已通过，2未审核且未添加地址，3审核中，4审核被拒绝
		/// </summary>
		public int? Status { get; set; }
		/// <summary>
		/// 入驻时间
		/// </summary>
		public DateTime? InsertTime { get; set; }
		/// <summary>
		/// 区域（三级）
		/// </summary>
		public string districtId { get; set; }
		/// <summary>
		/// 所在城市（二级）
		/// </summary>
		public string CityId { get; set; }
		/// <summary>
		/// 集团Id
		/// </summary>
		public int? GroupId { get; set; }
		/// <summary>
		/// 第三方平台推送过来的商家Id
		/// </summary>
		public int? OriginalBusiId { get; set; }
		/// <summary>
		/// 省份编码
		/// </summary>
		public string ProvinceCode { get; set; }
		/// <summary>
		/// 市编码
		/// </summary>
		public string CityCode { get; set; }
		/// <summary>
		/// 区编码
		/// </summary>
		public string AreaCode { get; set; }
		/// <summary>
		/// 省
		/// </summary>
		public string Province { get; set; }
		/// <summary>
		/// 佣金类型Id
		/// </summary>
		public int? CommissionTypeId { get; set; }
		/// <summary>
		/// 单次配送的外送费
		/// </summary>
		public decimal? DistribSubsidy { get; set; }
		/// <summary>
		/// 结算比例
		/// </summary>
		public decimal? BusinessCommission { get; set; }
		/// <summary>
		/// 结算类型：1：固定比例 2：固定金额
		/// </summary>
		public int CommissionType { get; set; }
		/// <summary>
		/// 固定金额
		/// </summary>
		public decimal CommissionFixValue { get; set; }
		/// <summary>
		/// 商家分组ID
		/// </summary>
		public int BusinessGroupId { get; set; }
		/// <summary>
		/// 余额
		/// </summary>
		public decimal BalancePrice { get; set; }
		/// <summary>
		/// 可提现金额
		/// </summary>
		public decimal AllowWithdrawPrice { get; set; }
		/// <summary>
		/// 累计提现金额
		/// </summary>
		public decimal HasWithdrawPrice { get; set; }

        /// <summary>
        /// 金融信息集合
        /// </summary>
        public List<BusinessFinanceAccount> listBFAcount { get; set; }

    }

    public class BusinessInfo
    {
        public BusinessInfo() { }

        /// <summary>
        /// 单次配送的外送费
        /// </summary>
        public decimal? DistribSubsidy { get; set; }
    }
}
