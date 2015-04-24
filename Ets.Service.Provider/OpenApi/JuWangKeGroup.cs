using Ets.Service.IProvider.OpenApi;
using ETS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Service.Provider.OpenApi
{
    public class JuWangKeGroup : IGroupProviderOpenApi
    {
        public string app_key = ConfigSettings.Instance.JuWangKeAppkey;
        public const string v = "1.0";
        public string app_secret = ConfigSettings.Instance.JuWangKeAppsecret;


        public ETS.Enums.OrderApiStatusType AsyncStatus(Model.Common.ParaModel<Model.ParameterModel.Order.AsyncStatusPM_OpenApi> paramodel)
        {
            throw new NotImplementedException();
        }

        public Model.ParameterModel.Order.CreatePM_OpenApi SetCcmmissonInfo(Model.ParameterModel.Order.CreatePM_OpenApi paramodel)
        {
            throw new NotImplementedException();
        }
    }
}
