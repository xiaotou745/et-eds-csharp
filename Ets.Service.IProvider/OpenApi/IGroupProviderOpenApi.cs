using Ets.Model.Common;
using Ets.Model.ParameterModel.Order;
using ETS.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Service.Provider.OpenApi
{
    /// <summary>
    /// 接口  第三方对接集团的回调业务 add by caoheyang 20150326
    /// </summary>
    public interface IGroupProviderOpenApi
    {
        /// <summary>
        /// 回调第三方接口同步状态  add by caoheyang 20150326
        /// </summary>
        /// <param name="paramodel"></param>
        /// <returns></returns>
        OrderApiStatusType AsyncStatus(ParaModel<AsyncStatusPM_OpenApi> paramodel);
    }
}
