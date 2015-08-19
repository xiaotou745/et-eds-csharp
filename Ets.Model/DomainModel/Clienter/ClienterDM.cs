using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Model.DataModel.Finance;
namespace Ets.Model.DomainModel.Clienter
{
    public class ClienterDM
    {
        public ClienterDM() { }
		/// <summary>
		/// 主键Id(自增)
		/// </summary>
		public int Id { get; set; }
		/// <summary>
		/// 手机号码
		/// </summary>
		public string PhoneNo { get; set; }
		/// <summary>
		/// 登录名称(没用？)
		/// </summary>
		public string LoginName { get; set; }
		/// <summary>
		/// 推荐人手机号
		/// </summary>
		public string recommendPhone { get; set; }
		/// <summary>
		/// 密码
		/// </summary>
		public string Password { get; set; }
		/// <summary>
		/// 真实姓名
		/// </summary>
		public string TrueName { get; set; }
		/// <summary>
		/// 身份证号
		/// </summary>
		public string IDCard { get; set; }
		/// <summary>
		/// 手持验证照片
		/// </summary>
		public string PicWithHandUrl { get; set; }
		/// <summary>
		/// 验证照片
		/// </summary>
		public string PicUrl { get; set; }
		/// <summary>
		/// 状态:0被拒绝，1已通过，2未审核，3审核中
		/// </summary>
		public int? Status { get; set; }
		/// <summary>
		/// 帐户余额
		/// </summary>
		public decimal AccountBalance { get; set; }
		/// <summary>
		/// 注册时间
		/// </summary>
		public DateTime? InsertTime { get; set; }
		/// <summary>
		/// 用户编号(没用？)
		/// </summary>
		public string InviteCode { get; set; }
		/// <summary>
		/// 所在城市
		/// </summary>
		public string City { get; set; }
		/// <summary>
		/// 所在城市Id
		/// </summary>
		public string CityId { get; set; }
		/// <summary>
		/// 集团Id
		/// </summary>
		public int? GroupId { get; set; }
		/// <summary>
		/// 健康证ID(海底捞？)
		/// </summary>
		public string HealthCardID { get; set; }
		/// <summary>
		/// 内外部门(海底捞？)
		/// </summary>
		public string InternalDepart { get; set; }
		/// <summary>
		/// 省编码
		/// </summary>
		public string ProvinceCode { get; set; }
		/// <summary>
		/// 区域编码
		/// </summary>
		public string AreaCode { get; set; }
		/// <summary>
		/// 市编码
		/// </summary>
		public string CityCode { get; set; }
		/// <summary>
		/// 省（名称）
		/// </summary>
		public string Province { get; set; }
		/// <summary>
		/// 商户Id(海底捞？)
		/// </summary>
		public int? BussinessID { get; set; }
		/// <summary>
		/// 超人状态 0上班  1下班 默认为0
		/// </summary>
		public int WorkStatus { get; set; }
		/// <summary>
		/// 可提现余额
		/// </summary>
		public decimal AllowWithdrawPrice { get; set; }
		/// <summary>
		/// 累计提现金额
		/// </summary>
		public decimal HasWithdrawPrice { get; set; }

        /// <summary>
        /// 金融账号
        /// </summary>
        public List<ClienterFinanceAccount> listcFAcount { get; set; }

        /// <summary>
        /// 当前骑士消息中心是否有未读消息
        /// </summary>
        public bool HasMessage { get; set; }

        /// <summary>
        /// 是否显示记账模块0不显示，默认1显示
        /// </summary>
        public int IsShowAccount { get; set; }

    }
}
