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

        /// <summary>
        /// 新增商铺时根据集团id为店铺设置外送费，结算比例等财务相关信息 add by caoheyang 20150417
        /// </summary>
        /// <param name="paramodel"></param>
        /// <returns></returns>
        CreatePM_OpenApi SetCcmmissonInfo(CreatePM_OpenApi paramodel);
    }
}
