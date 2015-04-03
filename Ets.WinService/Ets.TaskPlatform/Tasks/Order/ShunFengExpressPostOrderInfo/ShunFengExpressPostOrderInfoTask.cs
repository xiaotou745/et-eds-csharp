using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Task.Common;
using Task.Model;
using Task.Model.Order;
using Task.Service.Impl.Order;
using Task.Service.Order;
using TaskPlatform.TaskInterface;

namespace ShunFengExpressPostOrderInfo
{
    public class ShunFengExpressPostOrderInfoTask : AbstractTask
    {

        private static IOrderService mOrderService
        {
            get
            {
                return new OrderService();
            }
        }
        /// <summary>
        /// 服务名称
        /// </summary>
        /// <returns></returns>
        public override string TaskName()
        {
            return "顺丰快递提交订单（发货）";
        }
        /// <summary>
        /// 服务描述
        /// </summary>
        /// <returns></returns>
        public override string TaskDescription()
        {
            return "通过接口发送订单信息到顺丰系统";
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
            ShowRunningLog("=============本次自动推送全部完成=============");
            runTaskResult.Result = "本次自动推送结束";
            runTaskResult.Success = true;
            Thread.Sleep(3000);
            return runTaskResult;
            #endregion

        }
        /// <summary>
        /// 服务配置
        /// </summary>
        /// <returns></returns>
        public override Dictionary<string, string> UploadConfig()
        {
            if (!CustomConfig.ContainsKey("数据库连接字符串（读）"))
                CustomConfig.Add("数据库连接字符串（读）",
                    Task.Common.Parameters.OnLineDBConnectionStringRead);
            if (!CustomConfig.ContainsKey("数据库连接字符串（写）"))
                CustomConfig.Add("数据库连接字符串（写）",
                    Task.Common.Parameters.OnLineDBConnectionStringWrite);
            if (!CustomConfig.ContainsKey("EmailAddress"))
                CustomConfig.Add("EmailAddress", "wangxudan@jiuxian.com;");
            if (!CustomConfig.ContainsKey("快递"))
                CustomConfig.Add("快递", "25");
            if (!CustomConfig.ContainsKey("订单数量限制"))
                CustomConfig.Add("订单数量限制", Task.Common.Parameters.MaxGetPackCountShunFeng.ToString());
            if (!CustomConfig.ContainsKey("1-北京仓"))
                CustomConfig.Add("1-北京仓", "关");
            if (!CustomConfig.ContainsKey("2-广州仓"))
                CustomConfig.Add("2-广州仓", "开");
            if (!CustomConfig.ContainsKey("3-上海仓"))
                CustomConfig.Add("3-上海仓", "关");
            if (!CustomConfig.ContainsKey("4-武汉仓"))
                CustomConfig.Add("4-武汉仓", "关");
            if (!CustomConfig.ContainsKey("5-天津仓"))
                CustomConfig.Add("5-天津仓", "关");
            if (!CustomConfig.ContainsKey("6-成都仓"))
                CustomConfig.Add("6-成都仓", "关");
            return CustomConfig;
        }
        /// <summary>
        /// 推送订单信息到顺丰系统
        /// </summary>
        /// <returns></returns>
        private void ThreadPoolCallback(object threadContext)
        {
            #region 声明对象
            RunTaskResult runTaskResultNew = new RunTaskResult();
            TasksCommon tasksCommon = (TasksCommon)threadContext;
            var config = new Config
            {

                ReadConnectionString = tasksCommon.ConnStrRead,
                WriteConnectionString=tasksCommon.ConnStrWrite,
                Limit = tasksCommon.MaxQty,
                ShippingId = tasksCommon.ShippingId,
                WareId = tasksCommon.WareId,
            };
            #endregion
            try
            {
                string strResult = "";
                Log("【" + tasksCommon.DealFullName + "】正在获取需要推送的顺丰快递订单……");
                IList<OrderInfoForSF> orderInfoForSF = new List<OrderInfoForSF>();
                orderInfoForSF = mOrderService.QueryOrderNoSyncSFMainBLL(config);//准备推送的顺丰母单号及信息
                if (orderInfoForSF == null || orderInfoForSF.Count == 0)
                {
                    runTaskResultNew.Success = true;
                    runTaskResultNew.Result = "当前没有需要推送的顺丰快递的订单!";
                    Log("当前没有需要推送的顺丰快递的订单!");
                }
                else
                {
                    var listOrders = orderInfoForSF.Select(o => o.OrderId).Distinct().ToList();
                    Log("【" + tasksCommon.DealFullName + "】顺丰快递获取到需要推送的订单，共计：【" + listOrders.Count + "】单！");
                    List<responsePostOrderSF> listresponse = new List<responsePostOrderSF>();
                    foreach (var listOrder in listOrders)
                    {
                        var listZhuOrders = orderInfoForSF.Where(o => o.OrderId == listOrder).Where(o => o.SyncTime == 0).ToList();
                        var listZiOrders = orderInfoForSF.Where(o => o.OrderId == listOrder).Where(o => o.SyncTime == 1).Select(o => o.InvoiceNo).ToList();
                        string strZiYundan = string.Join(",", listZiOrders.ToArray());
                        #region 拼接xml
                        string sbOrderInfo = string.Format("orderid='{0}' mailno='{1}' dealtype='{2}'", listZhuOrders[0].OrderId,
                            listZhuOrders[0].InvoiceNo, 1);
                        string sbOrderOptionInfo = string.Format("weight='{0}' children_nos='{1}'", listZhuOrders[0].auto_weight, strZiYundan);
                        #endregion
                        strResult = GetPackInfo(sbOrderInfo.ToString(), sbOrderOptionInfo.ToString(), tasksCommon.DealFullName);
                        if (strResult != "请求失败")
                        {
                            Log(strResult);
                            var ostOrderSFXml = PostOrderSFXml(strResult);
                            ostOrderSFXml.mailNoCount = listZhuOrders.Count + listZiOrders.Count;//此单子母单数量总和
                            if (ostOrderSFXml.head == "OK")
                            {
                                listresponse.Add(ostOrderSFXml);
                            }
                            else
                            {
                                #region 确认订单接口调用失败
                                runTaskResultNew.Success = false;
                                runTaskResultNew.Result = ostOrderSFXml.errorInfo;
                                Log(TaskName() + ostOrderSFXml.errorInfo + "，订单ID为【" + listOrder + "】");
                                SendEmailTo(TaskName() + ostOrderSFXml.errorInfo + "，订单ID为【" + listOrder + "】", CustomConfig["EmailAddress"]);
                                #endregion
                            }
                        }
                        else
                        {
                            #region 请求失败
                            runTaskResultNew.Success = true;
                            runTaskResultNew.Result = "请求失败";
                            Log("请求失败");
                            #endregion
                        }
                    }
                    if (listresponse.Count > 0)
                    {
                        #region 订单确认接口调用成功

                        var sieveOrderDealSF = ResultDealWith(config, listresponse);
                        runTaskResultNew.Result = string.Format("【{0}】本次顺丰推送{1}单，成功{2}单，订单ID集合为：【{3}】，失败{4}单，订单ID集合为：【{5}】", tasksCommon.DealFullName, listresponse.Count, sieveOrderDealSF.DealSuccQty, sieveOrderDealSF.ReachOrderId, sieveOrderDealSF.UnReachQty, sieveOrderDealSF.UnReachOrderId);
                        Log(string.Format("【{0}】本次顺丰推送{1}单，成功{2}单，订单ID集合为：【{3}】，失败{4}单，订单ID集合为：【{5}】", tasksCommon.DealFullName, listresponse.Count, sieveOrderDealSF.DealSuccQty, sieveOrderDealSF.ReachOrderId, sieveOrderDealSF.UnReachQty, sieveOrderDealSF.UnReachOrderId));
                        runTaskResultNew.Success = true;
                        #endregion
                    }
                }
                
            }
            catch (Exception ex)
            {
                #region 异常邮件通知
                SendEmailTo(TaskName() + ex, CustomConfig["EmailAddress"]);
                runTaskResultNew.Success = false;
                runTaskResultNew.Result = ex.ToString();
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
        /// 推送订单信息到顺丰系统
        /// </summary>
        /// <returns></returns>
        private void ThreadPoolCallbackYb()
        {
            #region 声明对象
            RunTaskResult runTaskResultNew = new RunTaskResult();
            //TasksCommon tasksCommon = (TasksCommon)threadContext;
            //var config = new Config
            //{
            //    ConnectionString = tasksCommon.ConnStrRead,
            //    Limit = tasksCommon.MaxQty,
            //    ShippingId = tasksCommon.ShippingId,
            //    WareId = tasksCommon.WareId,
            //};
            #endregion

            var config = new Config
            {
                ReadConnectionString = "Server=192.168.5.78;Database=jiuxianweb; User=test_db_online;Password=QwerAs131126dfTyui1209;",
                WriteConnectionString = "Server=192.168.5.78;Database=jiuxianweb; User=test_db_online;Password=QwerAs131126dfTyui1209;",
                Limit = 2,//执行条数
                ShippingId = 25,//快递
                WareId = 5,//仓库
            };

            try
            {
                string strResult = "";
                //var userInfo = ConfigurationManager.AppSettings["5-天津仓"];
                Log("【顺丰订单推送服务】正在获取需要推送的顺丰快递订单……");
                IList<OrderInfoForSF> orderInfoForSF = new List<OrderInfoForSF>();
                orderInfoForSF = mOrderService.QueryOrderNoSyncSFMainBLL(config);//准备推送的顺丰母单号及信息
                if (orderInfoForSF == null)
                {
                    runTaskResultNew.Success = true;
                    runTaskResultNew.Result = "当前没有需要推送的顺丰快递的订单!";
                    Log("当前没有需要推送的顺丰快递的订单!");
                }
                if (orderInfoForSF.Count == 0)
                {
                    runTaskResultNew.Success = true;
                    runTaskResultNew.Result = "当前没有需要推送的顺丰快递的订单！";
                    Log("当前没有需要推送的顺丰快递的订单！");
                }
                var listOrders = orderInfoForSF.Select(o => o.OrderId).Distinct().ToList();
                Log("【顺丰订单推送服务】顺丰快递获取到需要推送的订单，共计：【" + listOrders.Count + "】单！");
                List<responsePostOrderSF> listresponse = new List<responsePostOrderSF>();
                foreach (var listOrder in listOrders)
                {
                    var listZhuOrders = orderInfoForSF.Where(o => o.OrderId == listOrder).Where(o => o.SyncTime == 0).ToList();
                    var listZiOrders = orderInfoForSF.Where(o => o.OrderId == listOrder).Where(o => o.SyncTime == 1).Select(o => o.InvoiceNo).ToList();
                    string strZiYundan = string.Join(",", listZiOrders.ToArray());

                    #region 拼接xml
                    string sbOrderInfo = string.Format("orderid='{0}' mailno='{1}' dealtype='{2}'", listZhuOrders[0].OrderId,
                        listZhuOrders[0].InvoiceNo, 1);
                    string sbOrderOptionInfo = string.Format("weight='{0}' children_nos='{1}'", listZhuOrders[0].auto_weight, strZiYundan);
                    #endregion
                    //string userInfo = ConfigurationManager.AppSettings[wareName].ToString();
                    string userKey = "JXW,e1Xc1gW04zBJ3HjL";
                    strResult = GetPackInfoYb(userKey,sbOrderInfo.ToString(), sbOrderOptionInfo.ToString());
                    if (strResult != "请求失败")
                    {
                        Log(strResult);
                        var ostOrderSFXml = PostOrderSFXml(strResult);
                        ostOrderSFXml.mailNoCount = listZhuOrders.Count + listZiOrders.Count;//此单子母单数量总和
                        if (ostOrderSFXml.head == "OK")
                        {
                            listresponse.Add(ostOrderSFXml);
                        }
                        else
                        {
                            #region 确认订单接口调用失败
                            runTaskResultNew.Success = false;
                            runTaskResultNew.Result = ostOrderSFXml.errorInfo;
                            Log(TaskName() + ostOrderSFXml.errorInfo + "，订单ID为【" + listOrder + "】");
                            SendEmailTo(TaskName() + ostOrderSFXml.errorInfo + "，订单ID为【" + listOrder + "】", CustomConfig["EmailAddress"]);
                            #endregion
                        }
                    }
                    else
                    {
                        #region 请求失败
                        runTaskResultNew.Success = true;
                        runTaskResultNew.Result = "请求失败";
                        Log("请求失败");
                        #endregion
                    }
                }
                if (listresponse.Count > 0)
                {
                    #region 订单确认接口调用成功

                    var sieveOrderDealSF = ResultDealWith(config,listresponse);
                    runTaskResultNew.Result = string.Format("【{0}】本次顺丰推送{1}单，成功{2}单，订单ID集合为：【{3}】，失败{4}单，订单ID集合为：【{5}】", "顺丰订单推送服务", listresponse.Count, sieveOrderDealSF.DealSuccQty, sieveOrderDealSF.ReachOrderId, sieveOrderDealSF.UnReachQty, sieveOrderDealSF.UnReachOrderId);
                    Log(string.Format("【{0}】本次顺丰推送{1}单，成功{2}单，订单ID集合为：【{3}】，失败{4}单，订单ID集合为：【{5}】", "顺丰订单推送服务", listresponse.Count, sieveOrderDealSF.DealSuccQty, sieveOrderDealSF.ReachOrderId, sieveOrderDealSF.UnReachQty, sieveOrderDealSF.UnReachOrderId));
                    runTaskResultNew.Success = true;
                    SendEmailTo(TaskName() + sieveOrderDealSF.UnReachOrderId, "yebing@jiuxian.com");//有异常的发邮件

                    #endregion
                }
            }
            catch (Exception ex)
            {
                #region 异常邮件通知
                SendEmailTo(TaskName() + ex, "yebing@jiuxian.com");
                runTaskResultNew.Success = false;
                runTaskResultNew.Result = ex.ToString();
                Log(ex.ToString());
                #endregion
            }
        }

        /// <summary>
        /// 推送到顺丰
        /// </summary>
        /// <param name="OrderInfo"></param>
        /// <param name="wareName"></param>
        /// <returns></returns>
        public string GetPackInfo(string OrderInfo, string sbOrderOptionInfo, string wareName)
        {
            try
            {
                string userInfo = ConfigurationManager.AppSettings[wareName].ToString();
                ShunFengPushPack.ServiceClient serviceClient = new ShunFengPushPack.ServiceClient();
                string xml = string.Format(@"<Request service='OrderConfirmService' lang='zh-CN'><Head> {1} </Head><Body><OrderConfirm {0}><OrderConfirmOption {2}/></OrderConfirm></Body></Request>  ", OrderInfo, userInfo, sbOrderOptionInfo);
                return serviceClient.sfexpressService(xml);
            }
            catch (Exception ex)
            {
                Log(ex.ToString());
                SendEmailTo("顺丰接口调用出问题，请及时处理:" + ex.ToString(), CustomConfig["EmailAddress"]);
                return "请求失败";
            }
        }

        /// <summary>
        /// 推送到顺丰
        /// </summary>
        /// <param name="OrderInfo"></param>
        /// <param name="wareName"></param>
        /// <returns></returns>
        public string GetPackInfoYb(string userKey,string OrderInfo,string sbOrderOptionInfo)
        {
            try
            {
                ShunFengPushPack.ServiceClient serviceClient = new ShunFengPushPack.ServiceClient();
                string xml = string.Format(@"<Request service='OrderConfirmService' lang='zh-CN'><Head> {1} </Head><Body><OrderConfirm {0}><OrderConfirmOption {2}/></OrderConfirm></Body></Request>  ", OrderInfo, userKey, sbOrderOptionInfo);
                return serviceClient.sfexpressService(xml);
            }
            catch (Exception ex)
            {
                Log(ex.ToString());
                SendEmailTo("顺丰接口调用出问题，请及时处理:" + ex.ToString(), CustomConfig["EmailAddress"]);
                return "请求失败";
            }
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

        /// <summary>
        /// 解析发货确认响应报文
        /// </summary>
        /// <param name="xmlContent"></param>
        /// <returns></returns>
        public responsePostOrderSF PostOrderSFXml(string xmlContent)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlContent);
            responsePostOrderSF responseSF = new responsePostOrderSF();
            XmlNodeList orderChildNodes = doc.SelectNodes("Response");
            foreach (XmlNode orderChild in orderChildNodes)
            {
                responseSF.head = orderChild.SelectSingleNode("Head").InnerText;
                if (responseSF.head == "OK")
                {
                    XmlNodeList orderChildNodesBody = orderChild.SelectSingleNode("Body").ChildNodes;
                    foreach (XmlNode orderChildBody in orderChildNodesBody)
                    {
                        XmlElement xe = (XmlElement)orderChildBody;
                        responseSF.orderid = xe.GetAttribute("orderid");
                        responseSF.mailno = xe.GetAttribute("mailno");
                        responseSF.remark = xe.GetAttribute("remark");
                    }
                }
                else if (responseSF.head == "ERR")
                {
                    XmlNode ERRORNode = orderChild.SelectSingleNode("ERROR");
                    responseSF.errorInfo = ERRORNode.InnerText;
                    XmlElement xe = (XmlElement)ERRORNode;
                    responseSF.code = xe.GetAttribute("code");
                }
            }
            return responseSF;
        }

        /// <summary>
        /// 对结果进行处理。
        /// 2014年6月24日
        /// </summary>
        /// <param name="responses"></param>
        private SieveOrderDealSF ResultDealWith(Config config, List<responsePostOrderSF> responses)
        {
            SieveOrderDealSF sieveOrderDealSF = new SieveOrderDealSF();
            foreach (var response in responses)
            {
                var order = new OrderSync
                {
                    OrderId = response.orderid.ToInt(),
                    BackMsg = "创建订单成功",
                    DealFlag = 1,
                };
                try
                {
                   int res = mOrderService.UpdateOrderSyncSFBLL(config, order);
                   if (res != response.mailNoCount)
                   {
                       sieveOrderDealSF.UnReachQty++;
                       sieveOrderDealSF.UnReachOrderId += order.OrderId + "【推送后更新数量不一致】,";
                   }
                   sieveOrderDealSF.DealSuccQty++;
                   sieveOrderDealSF.ReachOrderId += order.OrderId + "【OK】,";
                   sieveOrderDealSF.ReachOrderId = sieveOrderDealSF.ReachOrderId.Remove(sieveOrderDealSF.ReachOrderId.Length - 1, 1);
                }
                catch (Exception ex)
                {
                    sieveOrderDealSF.UnReachQty++;
                    sieveOrderDealSF.UnReachOrderId += order.OrderId + "【更新异常："+ex.ToString()+"】,";
                    sieveOrderDealSF.UnReachOrderId = sieveOrderDealSF.UnReachOrderId.Remove(sieveOrderDealSF.UnReachOrderId.Length - 1, 1);
                }
            }
            return sieveOrderDealSF;
        }

    }
}
