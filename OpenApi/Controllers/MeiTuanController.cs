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
                //实体类赋值
                MeiTuanOrdeModel paramodel = HTTPHelper.BindeModel<MeiTuanOrdeModel>(HttpContext.Current.Request);
                MeiTuanGroup meituan = new MeiTuanGroup();
                if (meituan.PostGetSig(HttpContext.Current.Request) == paramodel.sig || HttpContext.Current.Request.QueryString["testshuadan"]!=null)
                {
                    CreatePM_OpenApi model = meituan.TranslateModel(paramodel);
                    return meituan.AddOrder(model) > 0 ? new { data = "ok" } : new { data = "fail" };
                }
                return new { data = "fail" };  //推送失败
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);  //记录日志
                return new { data = "fail" };  //推送失败
            }
        }

        /// <summary>
        /// 美团更新E代送订单状态   add by caoheyang 20150421  目前美团专用  
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ExecuteTimeApi]
        public object ChangeStatus()
        {
            try
            {
                MeiTuanGroup mgroup = new MeiTuanGroup();
                ChangeStatusPM_OpenApi model = new ChangeStatusPM_OpenApi();
                model.orderfrom = OrderConst.OrderFrom4;// 订单来源  美团订单的订单来源是 4
                model.status = OrderConst.OrderStatus3;// 取消订单
                model.order_no = HttpContext.Current.Request["order_id"];// 订单号
                if (HttpContext.Current.Request["sig"] == mgroup.GetSig(HttpContext.Current.Request))
                {
                    ResultModel<object> res = new OrderProvider().UpdateOrderStatus_Other(model);
                    return res.Status == 0 ? new { data = "ok" } : new { data = "fail" };
                }
                else
                    return new { data = "fail" };
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);  //记录日志
                return new { data = "fail" };  //推送失败
            }
        }


    }
}