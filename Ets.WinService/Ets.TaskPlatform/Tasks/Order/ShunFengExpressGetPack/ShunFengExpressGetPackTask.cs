using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Task.Common;
using Task.Model;
using Task.Model.Order;
using Task.Service.Impl.Order;
using Task.Service.Order;
using TaskPlatform.TaskInterface;
using System.Configuration;
using System.Xml;

namespace ShunFengExpressGetPack
{
    public class ShunFengExpressGetPackTask : AbstractTask
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
            return "顺丰自动筛单";
        }

        /// <summary>
        ///     服务描述
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
                    TasksCommon tasksCommon = new TasksCommon(wareId, strconRead, strconWrite, maxQty, cc.Key, doneEvents[i],shippingId);
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
        /// 线程操作
        /// </summary>
        /// <param name="threadContext"></param>
        private void ThreadPoolCallback(object threadContext)
        {
            #region 声明对象
            RunTaskResult runTaskResult = new RunTaskResult();
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
                #region 筛单线程
                var userInfo = ConfigurationManager.AppSettings[tasksCommon.DealFullName];
                string userkey = "";
                string custid = "";
                if (userInfo != null)
                {
                    userkey = userInfo.Split(';')[0];
                    custid = userInfo.Split(';')[1];
                }
                Log("【" + tasksCommon.DealFullName + "】正在获取需要筛单的顺丰快递订单……");
                IList<SieveOrderPushInfoOfShunFeng> orderList = mOrderService.QueryNoMatchInvoiceNoOrdersSFBLL(config);
                if (orderList.Count == 0)
                {
                    #region 没有需要筛单的订单
                    runTaskResult.Success = true;
                    runTaskResult.Result = "【" + tasksCommon.DealFullName + "】当前没有需要筛单的顺丰快递订单！";
                    Log("【" + tasksCommon.DealFullName + "】当前没有需要筛单的顺丰快递订单！");
                    #endregion
                }
                else
                {
                    #region 处理需要筛单的订单
                    Log("【" + tasksCommon.DealFullName + "】顺丰快递获取到需要筛单的订单，共计：【" + orderList.Count + "】单！");
                    IList<responseGetPackSF> listresponse = new List<responseGetPackSF>();
                    foreach (var item in orderList)
                    {
                        StringBuilder sbOrderInfo = new StringBuilder();
                        string strAddedService = "";
                        if (item.pay_id == 1 && Convert.ToDecimal(item.order_amount)> 0)
                        {
                            strAddedService += string.Format("<AddedService name='COD' value='{0}' value1='{1}' /> ", item.order_amount, custid);     
                        }
                        var orderInfoOfShunFeng = new OrderInfoOfShunFeng
                        {
                            orderid = item.orderid,
                            express_type = item.express_type,
                            d_company = item.d_company,
                            d_contact = item.d_contact,
                            d_tel = item.d_tel,
                            d_mobile = item.d_mobile,
                            d_address = item.d_address,
                        };
                        sbOrderInfo.Append(orderInfoOfShunFeng.ToXml());
                        var strResult = GetPackInfo(sbOrderInfo.ToString(), strAddedService, userkey, custid);
                        if (strResult != "请求失败")
                        {
                            var response = GetPackSFXml(strResult);
                            if (response.head == "OK")
                            {
                                listresponse.Add(response);
                            }
                            else
                            {
                                #region 筛单接口调用失败
                                runTaskResult.Success = false;
                                runTaskResult.Result = response.errorInfo;
                                Log(TaskName() + response.errorInfo + "，订单ID为【" + item.orderid + "】");
                                SendEmailTo(TaskName() + response.errorInfo + "，订单ID为【" + item.orderid + "】", CustomConfig["EmailAddress"]);
                                #endregion
                            }
                        }
                        else
                        {
                            #region 请求失败
                            runTaskResult.Success = true;
                            runTaskResult.Result = "请求失败";
                            Log("请求失败");
                            #endregion
                        }
                    }
                    if (listresponse.Count > 0)
                    {
                        #region 筛单接口调用成功
                        var sieveOrderDealSF = mOrderService.ResultDealWithOfSieveOrderBLL(listresponse, config);
                        if (sieveOrderDealSF.DealFlag)
                        {
                            runTaskResult.Result = string.Format("【{0}】本次顺丰筛单{1}单，成功{2}单，可送达{3}单，订单ID集合为：【{5}】，不能送达{4}单，订单ID集合为：【{6}】", tasksCommon.DealFullName, listresponse.Count, sieveOrderDealSF.DealSuccQty, sieveOrderDealSF.ReachQty, sieveOrderDealSF.UnReachQty,sieveOrderDealSF.ReachOrderId,sieveOrderDealSF.UnReachOrderId);
                            Log(string.Format("【{0}】本次顺丰筛单{1}单，成功{2}单，可送达{3}单，订单ID集合为：【{5}】，不能送达{4}单，订单ID集合为：【{6}】", tasksCommon.DealFullName, listresponse.Count, sieveOrderDealSF.DealSuccQty, sieveOrderDealSF.ReachQty, sieveOrderDealSF.UnReachQty, sieveOrderDealSF.ReachOrderId, sieveOrderDealSF.UnReachOrderId));
                            runTaskResult.Success = true;
                        }
                        else
                        {
                            runTaskResult.Result = sieveOrderDealSF.DealMsg;
                            Log(sieveOrderDealSF.DealMsg);
                            runTaskResult.Success = false;
                        }
                        #endregion
                    }
                    #endregion

                }
                #endregion
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

        #region 测试专用
        ///// <summary>
        ///// 线程操作
        ///// </summary>
        ///// <param name="threadContext"></param>
        //private void ThreadPoolCallbackYb()
        //{
        //    #region 声明对象
        //    RunTaskResult runTaskResult = new RunTaskResult();
        //    //TasksCommon tasksCommon = (TasksCommon)threadContext;
        //    var config = new Config
        //    {
        //        ReadConnectionString = "Server=192.168.5.78;Database=jiuxianweb; User=test_db_online;Password=QwerAs131126dfTyui1209;",
        //        WriteConnectionString = "Server=192.168.5.78;Database=jiuxianweb; User=test_db_online;Password=QwerAs131126dfTyui1209;",
        //        Limit = 2,//执行条数
        //        ShippingId = 25,//快递
        //        WareId = 5,//仓库
        //    };
        //    #endregion
        //    try
        //    {
        //        #region 筛单线程
        //        //var userInfo = ConfigurationManager.AppSettings[tasksCommon.DealFullName];
        //        //string userkey = "";
        //        //string custid = "";
        //        //if (userInfo != null)
        //        //{
        //        //    userkey = userInfo.Split(';')[0];
        //        //    custid = userInfo.Split(';')[1];
        //        //}
        //        Log("【顺丰筛单服务】正在获取需要筛单的顺丰快递订单……");
        //        IList<SieveOrderPushInfoOfShunFeng> orderList = mOrderService.QueryNoMatchInvoiceNoOrdersSFBLL(config);
        //        if (orderList.Count == 0)
        //        {
        //            #region 没有需要筛单的订单
        //            runTaskResult.Success = true;
        //            runTaskResult.Result = "【顺丰筛单服务】当前没有需要筛单的顺丰快递订单！";
        //            Log("【顺丰筛单服务】当前没有需要筛单的顺丰快递订单！");
        //            #endregion
        //        }
        //        else
        //        {
        //            #region 处理需要筛单的订单
        //            Log("【顺丰筛单服务】顺丰快递获取到需要筛单的订单，共计：【" + orderList.Count + "】单！");
        //            IList<responseGetPackSF> listresponse = new List<responseGetPackSF>();
        //            foreach (var item in orderList)
        //            {
        //                StringBuilder sbOrderInfo = new StringBuilder();
        //                string strAddedService = "";
        //                if (item.pay_id == 1)
        //                {
        //                    strAddedService += string.Format("<AddedService name='COD' value='{0}' value1='{1}' /> ", item.order_amount, "0223728269");//此处需要修改
        //                }
        //                var orderInfoOfShunFeng = new OrderInfoOfShunFeng
        //                {
        //                    orderid = item.orderid,
        //                    express_type = item.express_type,
        //                    d_company = item.d_company,
        //                    d_contact = item.d_contact,
        //                    d_tel = item.d_tel,
        //                    d_mobile = item.d_mobile,
        //                    d_address = item.d_address,
        //                };
        //                sbOrderInfo.Append(orderInfoOfShunFeng.ToXml());
        //                var strResult = GetPackInfo(sbOrderInfo.ToString(), strAddedService, "酒仙网_天津仓", "0103710583");//此处需要修改
        //                if (strResult != "请求失败")
        //                {
        //                    var response = GetPackSFXml(strResult);
        //                    if (response.head == "OK")
        //                    {
        //                        listresponse.Add(response);
        //                    }
        //                    else
        //                    {
        //                        #region 筛单接口调用失败
        //                        runTaskResult.Success = false;
        //                        runTaskResult.Result = response.errorInfo;
        //                        Log(TaskName() + response.errorInfo + "，订单ID为【" + item.orderid + "】");
        //                        SendEmailTo(TaskName() + response.errorInfo + "，订单ID为【" + item.orderid + "】", CustomConfig["EmailAddress"]);
        //                        #endregion
        //                    }
        //                }
        //                else
        //                {
        //                    #region 请求失败
        //                    runTaskResult.Success = true;
        //                    runTaskResult.Result = "请求失败";
        //                    Log("请求失败");
        //                    #endregion
        //                }
        //            }
        //            if (listresponse.Count > 0)
        //            {
        //                #region 筛单接口调用成功
        //                var sieveOrderDealSF = mOrderService.ResultDealWithOfSieveOrderBLL(listresponse, config);
        //                if (sieveOrderDealSF.DealFlag)
        //                {
        //                    runTaskResult.Result = string.Format("【{0}】本次顺丰筛单{1}单，成功{2}单，可送达{3}单，订单ID集合为：【{5}】，不能送达{4}单，订单ID集合为：【{6}】", "顺丰筛单服务", listresponse.Count, sieveOrderDealSF.DealSuccQty, sieveOrderDealSF.ReachQty, sieveOrderDealSF.UnReachQty, sieveOrderDealSF.ReachOrderId, sieveOrderDealSF.UnReachOrderId);
        //                    Log(string.Format("【{0}】本次顺丰筛单{1}单，成功{2}单，可送达{3}单，订单ID集合为：【{5}】，不能送达{4}单，订单ID集合为：【{6}】", "顺丰筛单服务", listresponse.Count, sieveOrderDealSF.DealSuccQty, sieveOrderDealSF.ReachQty, sieveOrderDealSF.UnReachQty, sieveOrderDealSF.ReachOrderId, sieveOrderDealSF.UnReachOrderId));
        //                    runTaskResult.Success = true;
        //                }
        //                else
        //                {
        //                    runTaskResult.Result = sieveOrderDealSF.DealMsg;
        //                    Log(sieveOrderDealSF.DealMsg);
        //                    runTaskResult.Success = false;
        //                }
        //                #endregion
        //            }
        //            #endregion

        //        }
        //        #endregion
        //    }
        //    catch (Exception ex)
        //    {
        //        #region 异常邮件通知
        //        SendEmailTo(TaskName() + ex, CustomConfig["EmailAddress"]);
        //        runTaskResult.Success = false;
        //        runTaskResult.Result = ex.ToString();
        //        Log(ex.ToString());
        //        #endregion
        //    }
        //    finally
        //    {
        //        #region 线程结束
        //        //tasksCommon.DoneEvent.Set();
        //        #endregion
        //    }
        //}
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
        /// <summary>
        /// 调用筛单接口获取筛单结果
        /// </summary>
        /// <param name="OrderInfo"></param>
        /// <param name="wareName"></param>
        /// <returns></returns>
        public string GetPackInfo(string OrderInfo, string strAddedService, string userkey,string custid)
        {
            try
            {
                ShunFengGetPack.ServiceClient serviceClient = new ShunFengGetPack.ServiceClient();
#if DEBUG
                string xml = string.Format(@"<Request service='OrderService' lang='zh-CN'><Head>JXW,e1Xc1gW04zBJ3HjL</Head><Body><Order {0}><OrderOption template='{1}' custid='0103710583' cargo='酒水'> {2}</OrderOption></Order></Body></Request>  ", OrderInfo, userkey, strAddedService);
#else
                string xml = string.Format(@"<Request service='OrderService' lang='zh-CN'><Head>jxwdzsw,A0EdBuAMd7iD0aCWSZqQqBOnfzoDOFKK</Head><Body><Order {0}><OrderOption template='{1}' custid='{3}' cargo='酒水'> {2}</OrderOption></Order></Body></Request>  ", OrderInfo, userkey, strAddedService, custid);
#endif
                return serviceClient.sfexpressService(xml);
                #region XMLDemo
               #endregion
            }
            catch (Exception ex)
            {
                Log(ex.ToString());
                SendEmailTo("顺丰接口调用出问题，请及时处理:" + ex.ToString(), CustomConfig["EmailAddress"]);
                return "请求失败";
            }
        }
        
        /// <summary>
        /// 解析筛单返回xml
        /// </summary>
        /// <param name="xmlContent"></param>
        /// <returns></returns>
        public responseGetPackSF GetPackSFXml(string xmlContent)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlContent);
            responseGetPackSF responseSF = new responseGetPackSF();
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
                        responseSF.agent_mailno = xe.GetAttribute("agent_mailno");
                        responseSF.destcode = xe.GetAttribute("destcode");
                        responseSF.filter_result = xe.GetAttribute("filter_result");
                        responseSF.origincode = xe.GetAttribute("origincode");
                        responseSF.return_tracking_no = xe.GetAttribute("return_tracking_no");
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
    }        
}
