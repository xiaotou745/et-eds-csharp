using Ets.Service.IProvider.Order;
using Ets.Service.Provider.Order;
using Ets.Service.IProvider.Clienter;
using Ets.Service.Provider.Clienter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Ets.Model.Common;
using ETS.Enums;
using Ets.Model.ParameterModel.Order;
using Ets.Service.Provider.Common;
using Ets.Service.IProvider.Common;
using Ets.Model.DomainModel.Group;
using ETS.Security;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;
using System.IO;
using System.Web.Script.Serialization;
using ETS.Util;
using System.Configuration;
using ETS.Const;
using Ets.Service.Provider.OpenApi;
using Ets.Model.DataModel.Order;
using Ets.Service.IProvider.OpenApi;
using System.Reflection;
using Letao.Util;

namespace OpenApi.Controllers
{
    public class MeiTuanController : ApiController
    {
        // POST: Order PullOrder   paramodel 固定 必须是 paramodel  
        /// <summary>
        /// 接受美团发布订单推送的订单信息    add by caoheyang 20150420  目前仅支持美团
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ExecuteTimeApi]
        public object PullOrderInfo()
        {
            try
            {
                ////实体类赋值
                MeiTuanOrdeModel paramodel = HTTPHelper.BindeModel<MeiTuanOrdeModel>(HttpContext.Current.Request);
                MeiTuanGroup meituan = new MeiTuanGroup();
                if (!meituan.ValiditeSig(paramodel))
                {
                    CreatePM_OpenApi model = meituan.TranslateModel(paramodel);
                    return meituan.AddOrder(model) > 0 ? new { data = "ok" } : new { data = "fail" };
                }
                return new { data = "fail" };  //推送失败
            }
            catch (Exception ex) {
                LogHelper.LogWriterFromFilter(ex);  //记录日志
                return new { data = "fail" };  //推送失败
            }
          
        }
    }
}