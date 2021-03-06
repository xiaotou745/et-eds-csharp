﻿using Ets.Service.IProvider.Order;
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
        //美团正式KEY：96DD2B96BB9A7C49DC545DD17463CDFA
        //美团测试KEY：01c33711a7c2e6cf2cc27d838e83006e
        /// <summary>
        /// 接受美团发布订单推送的订单信息    add by caoheyang 20150420  目前仅支持美团
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ExecuteTimeApi]
        public ResultModelToString PullOrderInfo()
        {
            try
            {
                //实体类赋值
                MeiTuanOrdeModel paramodel = HTTPHelper.BindeModel<MeiTuanOrdeModel>(HttpContext.Current.Request);
                MeiTuanGroup meituan = new MeiTuanGroup();
                if (meituan.PostGetSig(HttpContext.Current.Request) == paramodel.sig)
                {
                    CreatePM_OpenApi model = meituan.TranslateModel(paramodel);
                    if (model == null)  //商户在E代送不存在等情况下导致实体translate失败
                        return new ResultModelToString(data: "fail");
                    meituan.AddOrder(model);
                    return new ResultModelToString(data: "ok");
                }
                return new ResultModelToString(data: "fail");  //推送失败
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);  //记录日志
                return new ResultModelToString(data: "fail");  //推送失败
            }
        }

        /// <summary>
        /// 美团取消订单回调E代送接口  add by caoheyang 20150421    
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ExecuteTimeApi]
        public ResultModelToString ChangeStatus()
        {
            try
            {
                MeiTuanGroup mgroup = new MeiTuanGroup();
                ChangeStatusPM_OpenApi model = new ChangeStatusPM_OpenApi();
                model.orderfrom = OrderConst.OrderFrom4;// 订单来源  美团订单的订单来源是 4
                model.status = OrderConst.OrderStatus3;// 取消订单
                model.remark = "第三方集团取消订单，同步E代送系统订单状态";
                model.order_no = HttpContext.Current.Request["order_id"];// 订单号
                if (HttpContext.Current.Request["sig"] == mgroup.GetSig(HttpContext.Current.Request))
                {
                    ResultModel<object> res = new OrderProvider().UpdateOrderStatus_Other(model);
                    return res.Status == 0 ? new ResultModelToString(data: "ok") : new ResultModelToString(data: "fail");
                }
                else
                    return new ResultModelToString(data: "fail");
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);  //记录日志
                return new ResultModelToString(data: "fail");  //推送失败
            }
        }

        /// <summary>
        /// 设置配送范围 暂时不用 add by caoheyang 20150421
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ResultModel<object> SetRound()
        {  //签名信息
            string fileContent = ETS.IO.FileIO.GetFileContent(System.AppDomain.CurrentDomain.BaseDirectory + "/Content/OpenRange.txt", Encoding.Default).Replace("\r", "").Replace("\n", "");
            System.Text.RegularExpressions.MatchCollection matchs = System.Text.RegularExpressions.Regex.Matches(fileContent, "!(.*?)--");
            int tmp = 0;
            foreach (System.Text.RegularExpressions.Match item in matchs)
            {
                tmp++;
                string zuobiao = item.Groups[1].Value;
                List<string> @params = new List<string>() { 
                "timestamp="+TimeHelper.GetTimeStamp(false) ,
                "app_area_code="+tmp,
                "min_price=10",
                "area=["+zuobiao+"]",
                "app_id=33"
                };
                @params.Sort();
                string url = "http://waimaiopen.meituan.com/api/v1/third_shipping/save?";
                string waimd5 = url + string.Join("&", @params) + "96DD2B96BB9A7C49DC545DD17463CDFA"; //consumer_secret
                string sig = ETS.Security.MD5.Encrypt(waimd5).ToLower();
                string paras = string.Join("&", @params) + "&sig=" + sig;
                string json = HTTPHelper.HttpPost(url, paras, accept: "application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            }

            return null;
        }

        [HttpGet]
        public ResultModel<object> DelRound()
        {  //签名信息
            string fileContent = ETS.IO.FileIO.GetFileContent(System.AppDomain.CurrentDomain.BaseDirectory + "/Content/OpenRange.txt", Encoding.Default).Replace("\r", "").Replace("\n", "");
            System.Text.RegularExpressions.MatchCollection matchs = System.Text.RegularExpressions.Regex.Matches(fileContent, "!(.*?)--");
            for (int i = 0; i < 50; i++)
            {
                List<string> @params = new List<string>() { 
                "timestamp="+TimeHelper.GetTimeStamp(false) ,
                "app_area_code="+i,
                "app_id=33"
                };
                @params.Sort();
                string url = "http://waimaiopen.meituan.com/api/v1/third_shipping/delete?";
                string waimd5 = url + string.Join("&", @params) + "96DD2B96BB9A7C49DC545DD17463CDFA"; //consumer_secret
                string sig = ETS.Security.MD5.Encrypt(waimd5).ToLower();
                string paras = string.Join("&", @params) + "&sig=" + sig;
                string json = HTTPHelper.HttpPost(url, paras, accept: "application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            }

            return null;
        }

        /// <summary> 
        /// 设置店铺状态  暂时不用 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ResultModel<object> SetStoreState()
        {  //签名信息
            List<string> @params = new List<string>() { 
            "timestamp="+TimeHelper.GetTimeStamp(false) ,
            "app_poi_code=24",
            "app_id=33"
            };
            @params.Sort();
            string url = "http://test.waimaiopen.meituan.com/api/v1/poi/open?";
            string waimd5 = url + string.Join("&", @params) + "01c33711a7c2e6cf2cc27d838e83006e"; //consumer_secret
            string sig = ETS.Security.MD5.Encrypt(waimd5).ToLower();
            string paras = string.Join("&", @params) + "&sig=" + sig;
            string json = HTTPHelper.HttpPost(url, paras, accept: "application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            return null;
        }
    }
}