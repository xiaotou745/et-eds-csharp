using Ets.Dao.Common;
using Ets.Model.Common;
using Ets.Model.DataModel.Group;
using Ets.Model.DomainModel.Group;
using Ets.Model.ParameterModel.Order;
using Ets.Service.IProvider.Common;
using ETS.Security;
using ETS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Service.Provider.OpenApi
{

    /// <summary>
    /// sign签名信息处理类  add by caoheyang 20150327
    /// </summary>
    public static class ExtensionParaModel
    {
        /// <summary>
        /// 为当前集团参数实体生成sign签名信息 add by caoheyang 20150327
        /// </summary>
        /// <param name="groupId">参数实体必须有集团id </param>
        /// <returns></returns>
        public static ParaModel<AsyncStatusPM_OpenApi> GetSign(this ParaModel<AsyncStatusPM_OpenApi> paraModel)
        {
            GroupApiConfigDao dao = new GroupApiConfigDao();
            GroupApiConfig model = dao.GetGroupApiConfigByGroupID(paraModel.group);
            if (model == null)
                return null;
            else {
                paraModel.app_key = model.AppKey;
                paraModel.v = model.AppVersion;
                paraModel.timestamp = TimeHelper.GetTimeStamp();
                paraModel.sign = MD5.Encrypt(model.AppSecret + "app_key=" + model.AppKey + "timestamp"
                    + paraModel.timestamp + "v=" + model.AppVersion + model.AppSecret);
            }
            return paraModel;
        }
    }
}
