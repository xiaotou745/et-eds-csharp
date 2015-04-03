using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using System.Xml.Serialization;
using Task.Common;
using Task.Model;
using Task.Model.Order;
using Task.Service.Impl.Order;
using Task.Service.Order;
using TaskPlatform.TaskInterface;

namespace YunDaExpressGetPackByThread
{
    public class YunDaExpressGetPackByThreadTask : AbstractTask
    {
        private static IOrderService mOrderService
        {
            get { return new OrderService(); }
        }

        /// <summary>
        ///服务名称
        /// </summary>
        /// <returns></returns>
        public override string TaskName()
        {
            return "韵达自动筛单(多线程)";
        }

        /// <summary>
        ///服务描述
        /// </summary>
        /// <returns></returns>
        public override string TaskDescription()
        {
            return "通过接口推送订单信息给顺丰，顺丰验证订单是否能送达，能送达返回对应信息，不能送达就直接改为宅急送";
        }

        /// <summary>
        /// 多线程处理
        /// </summary>
        /// <returns></returns>
        public override RunTaskResult RunTask()
        {
            RunTaskResult runTaskResult = new RunTaskResult();
            #region 验证服务配置
            if (string.IsNullOrWhiteSpace(CustomConfig["数据库连接字符串（读）"]))
            {
                ShowRunningLog("数据库连接字符串(读)！");
                runTaskResult.Result = "数据库连接字符串(读)！";
                runTaskResult.Success = false;
                return runTaskResult;
            }
            if (string.IsNullOrWhiteSpace(CustomConfig["数据库连接字符串（写）"]))
            {
                ShowRunningLog("数据库连接字符串（写）！");
                runTaskResult.Result = "数据库连接字符串（写）！";
                runTaskResult.Success = false;
                return runTaskResult;
            }
            if (string.IsNullOrWhiteSpace(CustomConfig["订单数量限制"]))
            {
                ShowRunningLog("请填写订单数量限制的订单最大数量！");
                runTaskResult.Result = "请填写订单数量限制的订单最大数量！";
                runTaskResult.Success = false;
                return runTaskResult;
            }
            if (string.IsNullOrWhiteSpace(CustomConfig["EmailAddress"]))
            {
                ShowRunningLog("请填写邮件地址！");
                runTaskResult.Result = "请填写邮件地址！";
                runTaskResult.Success = false;
                return runTaskResult;
            }
            #endregion
            #region 对象声明
            string strconRead = CustomConfig["数据库连接字符串（读）"];
            string strconWrite = CustomConfig["数据库连接字符串（写）"];
            int maxQty = CustomConfig["订单数量限制"].ToInt();
            int shippingId = CustomConfig["快递"].ToInt();
            int mreCount = 0;
            #endregion
            #region 线程开关控制及线程调用
            foreach (var mm in CustomConfig)
            {
                if (mm.Key.Contains("-") && mm.Value == "开")
                {
                    ++mreCount;
                }
            }
            ManualResetEvent[] doneEvents = new ManualResetEvent[mreCount];
            try
            {
                int i = 0;
                foreach (var cc in CustomConfig)
                {
                    if (!cc.Key.Contains("-"))
                    {
                        continue;
                    }
                    if (cc.Value == "关")
                    {
                        continue;
                    }
                    string[] key = cc.Key.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                    int threadIndex = Convert.ToInt32(key[0]);
                    int wareId = Convert.ToInt32(key[0]);
                    doneEvents[i] = new ManualResetEvent(false);
                    TasksCommon tasksCommon = new TasksCommon(wareId, strconRead, strconWrite, maxQty, cc.Key, doneEvents[i], shippingId);
                    ThreadPool.QueueUserWorkItem(ThreadPoolCallback, tasksCommon);
                    i++;
                }
            }
            catch (Exception ex)
            {
                runTaskResult.Result = ex.ToString();
                runTaskResult.Success = false;
                ShowRunningLog(ex.ToString());
                WriteLog(ex.ToString());
            }
            WaitHandle.WaitAll(doneEvents);
            ShowRunningLog("=============本次自动筛单全部完成=============");
            runTaskResult.Result = "本次自动筛单结束";
            runTaskResult.Success = true;
            Thread.Sleep(3000);
            return runTaskResult;
            #endregion
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
                CustomConfig.Add("EmailAddress", "wangxudan@jiuxian.com;");
            if (!CustomConfig.ContainsKey("快递"))
                CustomConfig.Add("快递", "64");
            if (!CustomConfig.ContainsKey("订单数量限制"))
                CustomConfig.Add("订单数量限制", Task.Common.Parameters.MaxGetPackCountShunFeng.ToString());
            if (!CustomConfig.ContainsKey("1-北京仓"))
                CustomConfig.Add("1-北京仓", "关");
            if (!CustomConfig.ContainsKey("5-天津仓"))
                CustomConfig.Add("5-天津仓", "开");
            return CustomConfig;
        }
        /// <summary>
        /// 线程操作
        /// </summary>
        /// <param name="threadContext"></param>
        private void ThreadPoolCallback(object threadContext)
        {
            #region 声明对象
            var runTaskResult = new RunTaskResult();
            var tasksCommon = (TasksCommon)threadContext;
            var config = new Config
            {
                ReadConnectionString = tasksCommon.ConnStrRead,
                WriteConnectionString = tasksCommon.ConnStrWrite,
                Limit = tasksCommon.MaxQty,
                ShippingId = tasksCommon.ShippingId,
                WareId = tasksCommon.WareId,
            };
            #endregion
            try
            {
                string strResult = "";
                //// 获取未筛单的订单
                IList<erp_order> orderList = mOrderService.QueryUnCheckOrdersOfYunDaBLL(config);
                if (orderList == null || orderList.Count == 0)
                {
                    runTaskResult.Success = true;
                    runTaskResult.Result = "当前没有需要筛单的韵达快递订单!";
                    Log("当前没有需要筛单的韵达快递订单!");
                }
                else
                {
                    // 类转换
                    orders temp = ConvertClass(orderList);
                    string xmlData = XMLCommon.Serializer(typeof(orders), temp);
                    if (!string.IsNullOrWhiteSpace(xmlData))
                    {
#if DEBUG
                        var userinfo = ConfigurationManager.AppSettings[tasksCommon.DealFullName + "_测试"];
#else
                        var userinfo = ConfigurationManager.AppSettings[tasksCommon.DealFullName];
#endif
                        if (!string.IsNullOrWhiteSpace(userinfo))
                        {
                            config.UserInfo = userinfo;
                        }
                        strResult = YundaGetPack(xmlData, config);
                        if (strResult != "请求失败")
                        {
                            Log(strResult);
                            var model = XMLCommon.Deserialize(typeof(responses), strResult) as responses;
                            if (model.response[0].msg != "接口运行成功")
                            {
                                runTaskResult.Success = false;
                                runTaskResult.Result = model.response[0].msg;
                                Log(model.response[0].msg);
                                SendEmailTo(TaskName() + model.response[0].msg, CustomConfig["EmailAddress"]);

                            }
                            ResultDealWith(model, config);
                            runTaskResult.Success = true;
                            runTaskResult.Result = "请求成功";
                        }
                        else
                        {
                            runTaskResult.Success = true;
                            runTaskResult.Result = "请求失败";
                        }
                    }
                }
                    
                
                
            }
            catch (Exception ex)
            {
                #region 异常邮件通知
                SendEmailTo(TaskName() + ex, CustomConfig["EmailAddress"]);
                runTaskResult.Success = false;
                runTaskResult.Result = ex.ToString();
                Log(ex.ToString());
                #endregion
            }
            finally
            {
                #region 线程结束
                tasksCommon.DoneEvent.Set();
                #endregion
            }
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
        /// <summary>
        /// 调用韵达筛单接口
        /// </summary>
        /// <param name="xmlData"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public string YundaGetPack(string xmlData,Config config)
        {
            try
            {
                #if DEBUG
                    string url = "http://58.40.18.94:10110/cus_order/order_interface/interface_new_package.php";
                #else
                    string url = "http://order.yundasys.com:10235/cus_order/order_interface/interface_new_package.php";
                #endif
                var yundarequest = new YunDaRequest
                {

                    partnerid = config.UserInfo.Split(';')[0],
                    password = config.UserInfo.Split(';')[1],
                    xmldata = xmlData,
                    version = "1.0",
                    request = "data",
                };
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
        private void ResultDealWith(responses responses,Config config)
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
                list3.AddRange(responses.response.Select(c => new erp_order { OrderId = Convert.ToInt32(c.id) }));
                // 调用sql将信息插入到相应的表中。
                int r = InsertOrderServiceResults(config, orderServiceResult, list3);
                Log(string.Format("{0}个订单筛单成功。", r));
                config.ParameterInt = 1;
                list2.AddRange(orderServiceResult.Select(c => new erp_order { OrderId = c.OrderId }));
                int m = mOrderService.UpdateMatchOrderStatus(config, list2);
                Log(string.Format("{0}个订单更改为已筛单。", m / 2));
            }
        }

        // 组装调用接口后的数据。全部插入到xx表。
        private int InsertOrderServiceResults(Config config, IList<OrderServiceResult> list, IList<erp_order> list3)
        {
            mOrderService.DeleteMatchOrder(config, list3);
            return mOrderService.InsertMatchInvoiceOrderInfo(config, list);
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
        /// <summary>
        /// 封装日志方法
        /// </summary>
        /// <param name="msg"></param>
        public void Log(string msg)
        {
            ShowRunningLog(msg);
            WriteLog(msg);
        }
        
    }
}

