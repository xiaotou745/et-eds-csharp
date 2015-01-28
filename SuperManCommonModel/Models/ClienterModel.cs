using SuperManCore;
using SuperManDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperManCommonModel.Models
{
    /// <summary>
    /// 超人Model
    /// </summary>
    public class ClienterModel
    {
        /// <summary>
        /// 超人Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string PhoneNo { get; set; }
        /// <summary>
        /// 登录名称
        /// </summary>
        public string LoginName { get; set; }
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
        /// 帐户余额
        /// </summary>
        public decimal? AccountBalance { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public sbyte? Status { get; set; }
    }

    public class ClienterViewModel
    {
        /// <summary>
        /// 商家编号
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string PhoneNo { get; set; }
        /// <summary>
        /// 身份证号码
        /// </summary>
        public string IDCard { get; set; }
        /// <summary>
        /// 申请时间
        /// </summary>
        public DateTime? InsertTime { get; set; }
        /// <summary>
        /// 帐户余额
        /// </summary>
        public decimal? AccountBalance { get; set; }
        /// <summary>
        /// 订单数合计
        /// </summary>
        public int? OrderCount { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        public sbyte? Status { get; set; }
    }

    public class ClienterModelTranslator : TranslatorBase<clienter, ClienterModel>
    {
        public static readonly ClienterModelTranslator Instance = new ClienterModelTranslator();
        public override ClienterModel Translate(clienter from)
        {
            var to = new ClienterModel();
            to.Id = from.Id;
            to.PhoneNo = from.PhoneNo;
            to.LoginName = from.LoginName;
            to.Password = from.Password;
            to.TrueName = from.TrueName;
            to.IDCard = from.IDCard;
            to.PicWithHandUrl = from.PicWithHandUrl;
            to.PicUrl = from.PicUrl;
            to.AccountBalance = from.AccountBalance;
            to.Status = from.Status;            
            return to;
        }
        public override clienter Translate(ClienterModel from)
        {
            var to = new clienter();
            to.Id = from.Id;
            to.PhoneNo = from.PhoneNo;
            to.LoginName = from.LoginName;
            to.Password = from.Password;
            to.TrueName = from.TrueName;
            to.IDCard = from.IDCard;
            to.PicWithHandUrl = from.PicWithHandUrl;
            to.PicUrl = from.PicUrl;
            to.AccountBalance = from.AccountBalance;
            to.Status = from.Status;     
            return to;
        }
    }
}
