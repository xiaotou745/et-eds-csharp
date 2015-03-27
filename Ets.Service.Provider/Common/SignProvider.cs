using Ets.Dao.Common;
using Ets.Model.DataModel.Group;
using Ets.Model.DomainModel.Group;
using Ets.Service.IProvider.Common;
using ETS.Security;
using ETS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Service.Provider.Common
{

    /// <summary>
    /// sign签名信息处理类  add by caoheyang 20150327
    /// </summary>
    public class SignProvider
    {
        /// <summary>
        /// 获取该集团的sign签名信息 add by caoheyang 20150327
        /// </summary>
        /// <param name="groupId">集团id</param>
        /// <returns></returns>
        public string GetSign(int groupId)
        {
            GroupApiConfigDao dao = new GroupApiConfigDao();
            GroupApiConfig model = dao.GetGroupApiConfigByGroupID(groupId);
            if (model == null)
                return null;
            else
                return MD5.Encrypt(model.AppSecret + "app_key=" + model.AppKey + "timestamp"
                        + TimeHelper.GetTimeStamp() + "v=" + model.AppVersion + model.AppSecret);
        }
    }
}
