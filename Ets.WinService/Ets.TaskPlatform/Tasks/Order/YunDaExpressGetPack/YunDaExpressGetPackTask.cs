using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Xml.Serialization;
using Task.Common;
using Task.Model;
using Task.Model.Order;
using Task.Service.Impl.Order;
using Task.Service.Order;
using TaskPlatform.TaskInterface;

namespace YunDaExpressGetPack
{
    public class YunDaExpressGetPackTask : AbstractTask
    {
        private static IOrderService mOrderService
        {
            get { return new OrderService(); }
        }

        /// <summary>
        ///     服务名称
        /// </summary>
        /// <returns></returns>
        public override string TaskName()
        {
            return "韵达自动筛单";
        }

        /// <summary>
        ///     服务描述
        /// </summary>
        /// <returns></returns>
        public override string TaskDescription()
        {
            return "通过接口发送订单号，韵达验证订单是否能送达，能送达返回对应信息";
        }

        /// <summary>
        ///     服务实现
        ///     思路：
        ///     1.先查出50条。
        ///     2.向接口推送。
        ///     3.分析数据。
        ///     4.失败重新筛单。
        /// </summary>
        /// <returns></returns>
        public override RunTaskResult RunTask()
        {
            var runTaskResult = new RunTaskResult();
            var config = new Config
            {
                WriteConnectionString = CustomConfig["数据库连接字符串（写）"],
                ReadConnectionString = CustomConfig["数据库连接字符串（读）"],
                ParameterInt = 0,
                Limit =Convert.ToInt32(CustomConfig["订单数量限制"]),
                ShippingId=64,
            };
            try
            {
                string strResult = "";
                //// 获取未筛单的订单
                IList<erp_order> orderList = mOrderService.QueryNoMatchInvoiceNoOrders(config);
                if (orderList == null)
                {
                    runTaskResult.Success = true;
                    runTaskResult.Result = "当前没有需要推送的韵达快递的订单!";
                    Log("当前没有需要推送的韵达快递的订单!");
                    return runTaskResult;
                }
                if (orderList.Count == 0)
                {
                    runTaskResult.Success = true;
                    runTaskResult.Result = "当前没有需要推送的韵达快递的订单！";
                    Log("当前没有需要推送的韵达快递的订单！");
                    return runTaskResult;
                }
                // 类转换
                orders temp = ConvertClass(orderList);
                string xmlData = XMLCommon.Serializer(typeof(orders), temp);
                if (!string.IsNullOrWhiteSpace(xmlData))
                {
                    strResult = YundaGetPack(xmlData);
                    if (strResult != "请求失败")
                    {
                        //// 状态改成已筛单。
                        //if (mOrderService.UpdateMatchOrderStatus(config, orderList) == orderList.Count)
                        //{
                        Log(strResult);
                        // 将xml字符串转成对象。
                        var model = XMLCommon.Deserialize(typeof(responses), strResult) as responses;
                        if (model.response[0].msg != "接口运行成功")
                        {
                            runTaskResult.Success = false;
                            runTaskResult.Result = model.response[0].msg;
                            Log(model.response[0].msg);
                            SendEmailTo(TaskName() + model.response[0].msg, CustomConfig["EmailAddress"]);
                            return runTaskResult;
                        }
                        ResultDealWith(model, config);

                        runTaskResult.Success = true;
                        runTaskResult.Result = "请求成功";

                        #region 2014年4月26日17:00:59 修改。逻辑都移植到ResultDealWith方法。
                        //if (model != null)
                        //{
                        //    if (model.response != null && model.response.Count == temp.order.Count)
                        //    {
                        //        // 对结果进行解析，然后批量更新。
                        //        ResultDealWith(model);

                        //        runTaskResult.Success = true;
                        //        runTaskResult.Result = strResult;
                        //    }
                        //}
                        //else
                        //{
                        //    // 需要直接改快递吗？不超区，只是推送结果与预期不符。
                        //    mOrderService.UpdateMatchOrderShippingId(config, orderList);
                        //}
                        //}
                        //else
                        //{
                        //    // 不超区，只是推送结果与预期不符。状态重新修正。再推。
                        //    config.ParameterInt = 0;
                        //    mOrderService.UpdateMatchOrderStatus(config, orderList);
                        //} 
                        #endregion
                    }
                    else
                    {
                        runTaskResult.Success = true;
                        runTaskResult.Result = "请求失败";
                    }
                }
            }
            catch (Exception ex)
            {
                SendEmailTo(TaskName() + ex, CustomConfig["EmailAddress"]);
                runTaskResult.Success = false;
                runTaskResult.Result = ex.ToString();
                Log(ex.ToString());
            }
            return runTaskResult;
        }

        /// <summary>
        ///     服务配置
        /// </summary>
        /// <returns></returns>
        public override Dictionary<string, string> UploadConfig()
        {
            if (!CustomConfig.ContainsKey("数据库连接字符串（读）"))
                CustomConfig.Add("数据库连接字符串（读）",
                    Task.Common.Parameters.TestDBReadConnectionString0578);
            if (!CustomConfig.ContainsKey("数据库连接字符串（写）"))
                CustomConfig.Add("数据库连接字符串（写）",
                    Task.Common.Parameters.TestDBConnectionString0578);
            if (!CustomConfig.ContainsKey("EmailAddress"))
                CustomConfig.Add("EmailAddress", "wangxudan@jiuxian.com;huxiaobing@jiuxian.com");
            if (!CustomConfig.ContainsKey("快递"))
                CustomConfig.Add("快递", "64");
            if (!CustomConfig.ContainsKey("订单数量限制"))
                CustomConfig.Add("订单数量限制", Task.Common.Parameters.MaxGetPackCountYunDa.ToString());
            return CustomConfig;
        }

        /// <summary>
        ///     日志
        /// </summary>
        /// <param name="msg"></param>
        public void Log(string msg)
        {
            ShowRunningLog(msg);
            WriteLog(msg);
        }

        public string YundaGetPack(string xmlData)
        {
            try
            {
                //#region 测试配置
                //string url = "http://58.40.18.94:10110/cus_order/order_interface/interface_new_package.php";
                //var yundarequest = new YunDaRequest
                //{
                //    partnerid = "1998855009",
                //    password = "123456a",
                //    xmldata = xmlData,
                //    version = "1.0",
                //    request = "data",
                //};
                //#endregion
                #region 正式配置
                string url = "http://order.yundasys.com:10235/cus_order/order_interface/interface_new_package.php";
                var yundarequest = new YunDaRequest
                {
                    partnerid = "1002901305",
                    password = "JUGPKgkmsbyqv47ItdxeVTriNSuwMY",
                    xmldata = xmlData,
                    version = "1.0",
                    request = "data",
                };
                #endregion
                string base64_xmldata =
                    Convert.ToBase64String(Encoding.GetEncoding("UTF-8").GetBytes(yundarequest.xmldata));

                string validation =
                    FormsAuthentication.HashPasswordForStoringInConfigFile(
                        base64_xmldata + yundarequest.partnerid + yundarequest.password, "MD5").ToLower();
                string signdata = "partnerid=" + yundarequest.partnerid + "&version=" + yundarequest.version +
                                  "&request=" + yundarequest.request + "&xmldata=" +
                                  HttpUtility.UrlEncode(base64_xmldata) + "&validation=" + validation;
                return HttpHelper.Post(url, signdata);
            }
            catch (Exception ex)
            {
                Log(ex.ToString());
                SendEmailTo("韵达接口调用出问题，请及时处理:" + ex.ToString(), CustomConfig["EmailAddress"]);
                return "请求失败";
                
            }
        }

        /// <summary>
        /// 对结果进行处理。
        /// 2014年4月24日11:01:49
        /// </summary>
        /// <param name="responses"></param>
        private void ResultDealWith(responses responses, Config config)
        {
            if (responses == null)
                return;
            if (responses.response == null) return;
            // 要改快递的。
            IList<erp_order> list = new List<erp_order>();
            // 要改已筛单状态的。
            List<erp_order> list2 = new List<erp_order>();
            // 软删除的。
            List<erp_order> list3 = new List<erp_order>();
            var orderServiceResult = new List<OrderServiceResult>();
            foreach (response response in responses.response)
            {
                if (string.IsNullOrWhiteSpace(response.id)) continue;
                if (response.id == "0") continue;
                if (string.IsNullOrWhiteSpace(response.reach)) continue;
                // 是否送达，1送达，0不送达
                // 将符合要求的订单组合信息。加入到集合中。现在只有成功的。
                if (response.reach == "1")
                {
                    var tempOrderServiceResult = new OrderServiceResult
                    {
                        OrderId = Convert.ToInt32(response.id),
                        ShippingId = Convert.ToInt32(CustomConfig["快递"]),
                        ReturnStatus = Convert.ToInt32(response.status),
                        Reach = Convert.ToInt32(response.reach),
                        DistricenterCode = response.districenter_code,
                        DistricenterName = response.districenter_name,
                        Position = response.position,
                        OneCode = response.one_code,
                        TwoCode = response.two_code,
                        ThreeCode = response.three_code,
                        PadMailNo = response.pad_mailno,
                        ReturnMsg = response.msg,
                        Remark = "订单自动筛单服务--调用韵达筛单接口",
                    };
                    orderServiceResult.Add(tempOrderServiceResult);
                }
                else
                {
                    // 不送达的，就要改默认快递
                    var erpOrder = new erp_order
                    {
                        OrderId = Convert.ToInt32(response.id)
                    };
                    if (erpOrder.OrderId != 0)
                        list.Add(erpOrder);
                }
            }
            if (list.Count > 0)
            {
                int r = mOrderService.UpdateMatchOrderShippingId(config, list);
                Log(string.Format("{0}个订单更改快递成功。", r / 2));
            }
            if (orderServiceResult.Count > 0)
            {
                list3.AddRange(responses.response.Select(c => new erp_order {OrderId = Convert.ToInt32(c.id)}));
                // 调用sql将信息插入到相应的表中。
                int r = InsertOrderServiceResults(config, orderServiceResult, list3);
                Log(string.Format("{0}个订单筛单成功。", r));
                config.ParameterInt = 1;
                list2.AddRange(orderServiceResult.Select(c => new erp_order {OrderId = c.OrderId}));
                int m = mOrderService.UpdateMatchOrderStatus(config, list2);
                Log(string.Format("{0}个订单更改为已筛单。", m/2));
            }
        }

        // 组装调用接口后的数据。全部插入到xx表。
        private int InsertOrderServiceResults(Config config, IList<OrderServiceResult> list,IList<erp_order> list3)
        {
            mOrderService.DeleteMatchOrder(config, list3);
            return mOrderService.InsertMatchInvoiceOrderInfo(config, list);
        }

        /// <summary>
        /// 将查询的结果组合封装成要发送的对象。
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private orders ConvertClass(IList<erp_order> list)
        {
            var temp = new orders { order = new List<order>() };
            foreach (erp_order erpOrder in list)
            {
                var orderClass = new order
                {
                    id = erpOrder.OrderId,
                    receiver_address =
                        erpOrder.ProvinceName + erpOrder.CityName + erpOrder.DistrictName + erpOrder.Address,
                };
                temp.order.Add(orderClass);
            }
            return temp;
        }
    }

    #region 要发送的xml字符串对应的类
    [XmlRoot(ElementName = "orders")]
    public class orders
    {
        [XmlElement(ElementName = "order")]
        public List<order> order { get; set; }
    }

    /// <summary>
    ///     即将拼接发送给韵达接口的数据。
    /// </summary>
    public class order
    {
        /// <summary>
        ///     唯一标识，即OrderId
        /// </summary>
        public int id { get; set; }

        /// <summary>
        ///     地址，省市县区。
        /// </summary>
        public string receiver_address { get; set; }
    }
    #endregion

    #region 要接收的xml字符串对应的类
    [XmlRoot(ElementName = "responses")]
    public class responses
    {
        [XmlElement(ElementName = "response")]
        public List<response> response { get; set; }
    }

    public class response
    {
        public string id { get; set; }
        public string status { get; set; }
        public string reach { get; set; }
        public string districenter_code { get; set; }
        public string districenter_name { get; set; }
        public string bigpen_code { get; set; }
        public string position { get; set; }
        public string position_no { get; set; }
        public string one_code { get; set; }
        public string two_code { get; set; }
        public string three_code { get; set; }
        public string pad_mailno { get; set; }
        public string msg { get; set; }
    }
    #endregion
}