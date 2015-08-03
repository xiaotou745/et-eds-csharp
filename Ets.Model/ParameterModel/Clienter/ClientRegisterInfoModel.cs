using System; 
using Ets.Model.Common;
using Ets.Model.DataModel.Clienter; 
using ETS.Util;

namespace Ets.Model.ParameterModel.Clienter
{
    /// <summary>
    /// C端注册需要提供的信息
    /// </summary>
    public class ClientRegisterInfoModel
    {
        /// <summary>
        /// 电话号码
        /// </summary>
        public string phoneNo { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string passWord { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public string verifyCode { get; set; }
        /// <summary>
        /// 邀请码
        /// </summary>
        public string inviteCode { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 城市编码
        /// </summary>
        public string CityId { get; set; }

        /// <summary>
        /// 推荐人手机号
        /// </summary>
        public string recommendPhone { get; set; }
        /// <summary>
        /// 集团Id
        /// </summary>
        public int GroupId { get; set; }

        public int DeliveryCompanyId { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public string timespan { get; set; }

        /// <summary>
        /// 手机唯一标识ssid
        /// </summary>
        public string Ssid { get; set; }
    }
    public class ClientRegisterResultModel
    {
        public int userId { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string city { get; set; }
        /// <summary>
        /// 城市编码
        /// </summary>
        public string cityId { get; set; }

        /// <summary>
        /// 配送公司Id
        /// </summary>
        public int DeliveryCompanyId { get; set; }
        /// <summary>
        /// 配送公司名称
        /// </summary>
        public string DeliveryCompanyName { get; set; }
        /// <summary>
        /// 是否显示 金额 0隐藏 1 显示
        /// </summary>
        public int IsDisplay { get; set; }
        /// <summary>
        /// 唯一健值guid
        /// </summary>
        public string Appkey { get; set; }

        /// <summary>
        /// Tokey值
        /// </summary>
        public string Token { get; set; }

    }
    public class ClientRegisterInfoModelTranslator : TranslatorBase<clienter, ClientRegisterInfoModel>
    {
        public static readonly ClientRegisterInfoModelTranslator Instance = new ClientRegisterInfoModelTranslator();

        public override ClientRegisterInfoModel Translate(clienter from)
        {
            throw new NotImplementedException();
        }



        public override clienter Translate(ClientRegisterInfoModel from)
        {
            var to = new clienter();
            string appkey = Guid.NewGuid().ToString();
            to.Appkey = appkey;
            to.Password = from.passWord;
            to.PhoneNo = from.phoneNo;
            to.InviteCode = from.inviteCode;
            to.Status = 2;
            to.InsertTime = DateTime.Now;
            to.City = from.City;
            to.CityId = from.CityId;
            to.recommendPhone = from.recommendPhone;
            to.DeliveryCompanyId = from.DeliveryCompanyId;
            if (from.GroupId != 0)
            {
                to.GroupId = from.GroupId;
            } 
            return to;
        }
    }
}
