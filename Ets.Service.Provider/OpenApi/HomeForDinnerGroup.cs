using Ets.Model.Common;
using Ets.Model.ParameterModel.Order;
using Ets.Service.IProvider.OpenApi;
using Ets.Service.Provider.OpenApi;
using ETS.Const;
using ETS.Enums;
using ETS.Util;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Service.Provider.OpenApi
{
    /// <summary>
    /// 回家吃饭对接回调业务
    /// 徐鹏程
    /// 2015-04-23
    /// </summary>
    public class HomeForDinnerGroup:IGroupProviderOpenApi
    {
        public OrderApiStatusType AsyncStatus(ParaModel<AsyncStatusPM_OpenApi> paramodel)
        {

            return  OrderApiStatusType.Success;
        }

        public CreatePM_OpenApi SetCommissonInfo(CreatePM_OpenApi paramodel)
        {
            paramodel.store_info.delivery_fee = 5;//全时目前外送费统一5
            paramodel.store_info.businesscommission = 0;//万达目前结算比例统一0
            return paramodel;
        }
    }
}
