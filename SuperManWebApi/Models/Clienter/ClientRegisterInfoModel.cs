using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace SuperManWebApi.Models.Clienter
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

    }

    //public class ClientRegisterInfoModelTranslator : TranslatorBase<clienter, ClientRegisterInfoModel>
    //{
    //    public static readonly ClientRegisterInfoModelTranslator Instance = new ClientRegisterInfoModelTranslator();

    //    public override ClientRegisterInfoModel Translate(clienter from)
    //    {
    //        throw new NotImplementedException();
    //    }



    //    public override clienter Translate(ClientRegisterInfoModel from)
    //    {
    //        var to = new clienter();
    //        //to.Id = Helper.generateCode(from.phoneNo, AppType.C端);
    //        to.Password = from.passWord;
    //        to.PhoneNo = from.phoneNo;
    //        to.InviteCode = from.inviteCode;
    //        to.Status = ConstValues.CLIENTER_NOAUDIT;
    //        to.InsertTime = DateTime.Now;
    //        to.City = from.City;
    //        to.CityId = from.CityId;
    //        to.recommendPhone = from.recommendPhone;
    //        if (from.GroupId != 0)
    //        {
    //            to.GroupId = from.GroupId;
    //        }
    //        //海底捞
    //        //if (ConfigSettings.Instance.IsGroupPush)   //TODO 暂时有效
    //        //{
    //        //    to.GroupId = 2;
    //        //}
    //        return to;
    //    }
    //}
}