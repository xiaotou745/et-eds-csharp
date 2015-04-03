using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Task.Common;
using Task.Model;
using Task.Model.Order;
using Task.Service.Impl.Order;
using Task.Service.Order;
using TaskPlatform.TaskInterface;
using System.Configuration;
using System.Security.Cryptography;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace RuFengDaExpressPostOrderInfo
{
    public class RuFengDaExpressPostOrderInfoTask : AbstractTask
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
            return "如风达提交订单（发货）";
        }

        /// <summary>
        ///服务描述
        /// </summary>
        /// <returns></returns>
        public override string TaskDescription()
        {
            return "通过接口推送订单信息给如风达系统";
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
            runTaskResult.Result = "本次自动推送全部完成";
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
#if DEBUG
           if (!CustomConfig.ContainsKey("数据库连接字符串（读）"))
                CustomConfig.Add("数据库连接字符串（读）",
                    Task.Common.Parameters.TestDBReadConnectionString0578);
            if (!CustomConfig.ContainsKey("数据库连接字符串（写）"))
                CustomConfig.Add("数据库连接字符串（写）",
                    Task.Common.Parameters.TestDBConnectionString0578);
            if (!CustomConfig.ContainsKey("EmailAddress"))
                CustomConfig.Add("EmailAddress", "wangchaoyang@jiuxian.com;");
            if (!CustomConfig.ContainsKey("快递"))
                CustomConfig.Add("快递", "25");
            if (!CustomConfig.ContainsKey("订单数量限制"))
                CustomConfig.Add("订单数量限制", Task.Common.Parameters.MaxGetPackCountDeBang.ToString());
            if (!CustomConfig.ContainsKey("2-广州仓"))
                CustomConfig.Add("2-广州仓", "开");
            if (!CustomConfig.ContainsKey("3-上海仓"))
                CustomConfig.Add("3-上海仓", "关");
            if (!CustomConfig.ContainsKey("4-武汉仓"))
                CustomConfig.Add("4-武汉仓", "关");
            if (!CustomConfig.ContainsKey("5-天津仓"))
                CustomConfig.Add("5-天津仓", "关");
#else
            if (!CustomConfig.ContainsKey("数据库连接字符串（读）"))
                CustomConfig.Add("数据库连接字符串（读）",
                    Task.Common.Parameters.OnLineDBConnectionStringRead);
            if (!CustomConfig.ContainsKey("数据库连接字符串（写）"))
                CustomConfig.Add("数据库连接字符串（写）",
                    Task.Common.Parameters.OnLineDBConnectionStringWrite);
            if (!CustomConfig.ContainsKey("EmailAddress"))
                CustomConfig.Add("EmailAddress", "wangxudan@jiuxian.com;");
            if (!CustomConfig.ContainsKey("快递"))
                CustomConfig.Add("快递", "93");
            if (!CustomConfig.ContainsKey("订单数量限制"))
                CustomConfig.Add("订单数量限制", Task.Common.Parameters.MaxGetPackCountDeBang.ToString());
            if (!CustomConfig.ContainsKey("2-广州仓"))
                CustomConfig.Add("2-广州仓", "关");
            if (!CustomConfig.ContainsKey("3-上海仓"))
                CustomConfig.Add("3-上海仓", "关");
            if (!CustomConfig.ContainsKey("4-武汉仓"))
                CustomConfig.Add("4-武汉仓", "关");
            if (!CustomConfig.ContainsKey("5-天津仓"))
                CustomConfig.Add("5-天津仓", "开");
#endif
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
                ParameterInt = 1,
            };
            #endregion
            try
            {
                Log("【" + tasksCommon.DealFullName + "】正在获取需要推送的如风达快递订单……");
                var orderList = mOrderService.QueryNoPushOrdersOfRuFengDaBLL(config);
                if (orderList == null || orderList.Count == 0)
                {
                    #region 没有需要推送的订单

                    runTaskResult.Success = true;
                    runTaskResult.Result = "【" + tasksCommon.DealFullName + "】当前没有需要推送的如风达快递的订单！";
                    Log("【" + tasksCommon.DealFullName + "】当前没有需要推送的如风达快递的订单！");

                    #endregion
                }
                else
                {
                    #region 处理需要推送的订单

                    Log("【" + tasksCommon.DealFullName + "】如风达快递获取到需要推送的订单，共计：【" + orderList.Count + "】单！");
                    var listSieveOrderResultInfo = new List<string>();
                    foreach (var item in orderList)
                    {
                        #region 发货人信息
                        item.MerchantCode = "jxw001";
                        item.OrderType = 0;
                        item.Comment = "";
                        item.GoodsProperty = 2;
                        string[] userInfo = ConfigurationManager.AppSettings[config.WareId].ToString().Split(',');
                        item.FromName = "酒仙网电子商务股份有限公司";
                        item.FromPhone = "400-617-9999";
                        item.FromProvince = userInfo[1];
                        item.FromCity = userInfo[2];
                        item.FromArea = userInfo[3];
                        item.FromAddress = userInfo[4];
                        #endregion
                        var orderDetailsList = mOrderService.QueryGoodsInfoOfRuFengDaBLL(config,item.FormCode);
                        item.OrderDetails = orderDetailsList.ToArray();

                        string xmlInfo = SerializeXml(typeof(SieveOrderPushInfoOfRuFengDa),item).Replace("<SieveOrderPushInfoOfRuFengDa>", string.Empty).Replace("</SieveOrderPushInfoOfRuFengDa>", string.Empty);
                        string strResult = Execute(xmlInfo);
                        if (!string.IsNullOrWhiteSpace(strResult))
                        {
                            string regexStr = @"<ImportResultDetail>(.*)</ImportResultDetail>";
                            Regex r = new Regex(regexStr, RegexOptions.None);
                            Match mc = r.Match(strResult);
                            string dataStr = mc.Groups[1].Value;
                            if (string.IsNullOrEmpty(dataStr))
                            {
                                runTaskResult.Success = false;
                                runTaskResult.Result = "如风达响应信息解析失败";
                                Log("如风达响应信息解析失败:" + strResult);
                                SendEmailTo(TaskName() + "如风达响应信息解析失败:" + strResult, CustomConfig["EmailAddress"]);
                            }
                            else
                            {
                                XmlDocument doc = new XmlDocument();
                                doc.LoadXml(strResult);
                                string resultCode = doc.GetElementsByTagName("ResultCode")[0].InnerText;
                                string waybillNo = doc.GetElementsByTagName("WaybillNo")[0].InnerText;
                                string formCode = doc.GetElementsByTagName("FormCode")[0].InnerText;
                                string resultMessage = doc.GetElementsByTagName("ResultMessage")[0].InnerText;
                                if (resultCode != "IsSuccessWithoutAssign" && resultMessage != "订单编号已提交")
                                {
                                    runTaskResult.Success = false;
                                    runTaskResult.Result = resultMessage;
                                    Log(resultMessage);
                                    SendEmailTo(TaskName() + "OrderSn:" + formCode + "," + resultMessage, CustomConfig["EmailAddress"]);
                                }
                                else
                                {
                                    listSieveOrderResultInfo.Add(formCode);
                                }
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
                    if (listSieveOrderResultInfo.Count > 0)
                    {
                        var sieveOrderDealSF = ResultDealWith(config,listSieveOrderResultInfo);
                        if (sieveOrderDealSF.DealFlag)
                        {
                            runTaskResult.Result = string.Format("【{0}】本次如风达推送{1}单，成功{2}单，订单号集合为：【{3}】，失败{4}单，订单号集合为：【{5}】", "如风达订单推送服务", listSieveOrderResultInfo.Count, sieveOrderDealSF.DealSuccQty, sieveOrderDealSF.ReachOrderId, sieveOrderDealSF.UnReachQty, sieveOrderDealSF.UnReachOrderId);
                            Log(string.Format("【{0}】本次如风达推送{1}单，成功{2}单，订单号集合为：【{3}】，失败{4}单，订单号集合为：【{5}】", "如风达订单推送服务", listSieveOrderResultInfo.Count, sieveOrderDealSF.DealSuccQty, sieveOrderDealSF.ReachOrderId, sieveOrderDealSF.UnReachQty, sieveOrderDealSF.UnReachOrderId));
                            runTaskResult.Success = true;
                        }
                        else
                        {
                            runTaskResult.Result = sieveOrderDealSF.DealMsg;
                            Log(sieveOrderDealSF.DealMsg);
                            runTaskResult.Success = false;
                        }
                    }
                    #endregion
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
        /// 封装日志方法
        /// </summary>
        /// <param name="msg"></param>
        public void Log(string msg)
        {
            ShowRunningLog(msg);
            WriteLog(msg);
        }

        /// <summary>
        /// 对结果进行处理。
        /// 2014年6月24日
        /// </summary>
        /// <param name="responses"></param>
        private SieveOrderDealSF ResultDealWith(Config config, List<string> orderSnList)
        {
            SieveOrderDealSF sieveOrderDealSF = new SieveOrderDealSF();
            foreach (var orderSn in orderSnList)
            {
                var order = new OrderSync
                {
                    OrderSn = orderSn,
                    BackMsg = "创建订单成功",
                    DealFlag = 1,
                };
                try
                {
                    int res = mOrderService.UpdateOrderSyncRFDBLL(config, order);
                    if (res < 0)
                    {
                        sieveOrderDealSF.UnReachQty++;
                        sieveOrderDealSF.UnReachOrderId += order.OrderSn + "【推送后更新数量不一致】,";
                    }
                    sieveOrderDealSF.DealFlag = true;
                    sieveOrderDealSF.DealSuccQty++;
                    sieveOrderDealSF.ReachOrderId += order.OrderSn + "【OK】,";
                    sieveOrderDealSF.ReachOrderId = sieveOrderDealSF.ReachOrderId.Remove(sieveOrderDealSF.ReachOrderId.Length - 1, 1);
                }
                catch (Exception ex)
                {
                    sieveOrderDealSF.DealMsg = ex.Message;
                    sieveOrderDealSF.UnReachQty++;
                    sieveOrderDealSF.UnReachOrderId += order.OrderSn + "【更新异常：" + ex.ToString() + "】,";
                    sieveOrderDealSF.UnReachOrderId = sieveOrderDealSF.UnReachOrderId.Remove(sieveOrderDealSF.UnReachOrderId.Length - 1, 1);
                }
            }
            return sieveOrderDealSF;
        }

        /// <summary>
        /// 签名
        /// </summary>
        /// <param name="data">原始数据串</param>
        /// <returns>签名串</returns>
        private string SignData(byte[] data)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                string posPrivteKey = "<RSAKeyValue><Modulus>1yqPFh3IR5O8tqvDHlx3sYmZyaazVHhoMOYXi5ClvE3yxENi4ZWBh3ZsTYttgRtGK8rb/F4JY/EuKQfqr+Us9Go88d1+TGTVvgx89cJuuYsUXg0nQfXt6R7b4zGIU53UoCQM30rRXDU/tTdBssU6jSvUnTf/jLdcyTa05ShQcNs=</Modulus><Exponent>AQAB</Exponent><P>/UGLwR8pOsk8YAvykqWVPXA1Y2vDs9Wv7wQIbvdd8keF2qAY7p/EyoDUoON9GrRM3jNLu4yzUwcNHkhFUOgp7w==</P><Q>2X9dGo6p/EWyDpjx4tK+5NZpZb4f7k5JKHyjYeMA2rJCYKPgA+GtxJdVrtO4ywVVvQ3K6X77QLc8Sd5XipRD1Q==</Q><DP>IsB4zfJZtQBiMYfSLH9eQEUCfncOLsI21ySFh7yX/qQD4SEw6qWv2l3dT4qH4z80gRUm4kCHZvBDw5EFOfnxrw==</DP><DQ>Rsf8NLhY+ZB08es0TOzo0vA0PLgzDArOJC7rvN9zV7jLgUfTj2/SbTgS2mJssSV7UZs7feGlQtpe9Gs3chHuQQ==</DQ><InverseQ>wISmQR8/R8rswox+RbuPJ6TYMFgcVWKqw2cOwbeJMuVcCgdM3RNGyxKbPZ53CRuMIxkePk1dj4PmcAG7mEN+1g==</InverseQ><D>TxtflhMHEoHXrRWDXENE4mojt4bpgdHvBKNj3rUkqhHCgrP/w85y2/oHIY90iDYd23Xu4V81dqAyh3VYrKjGmLzuqmUXX6Yb5TM0QFAaevhIRNzbZ3XI1c3ulHKgIlUzUJROYJnxb4beHOy59uU5gYP8KUsYDADugjDQEPbmL+k=</D></RSAKeyValue>";//密钥
                rsa.FromXmlString(posPrivteKey);
                return Convert.ToBase64String(rsa.SignData(data, typeof(SHA1)));
            }
            //return null;
        }

        public string Execute(string invoice_nos)
        {

#if DEBUG
            var url = "http://61.51.37.70:8082/api/";//接口地址
#else
             var url = "http://edi.wuliusys.com/api/";
#endif

            var guid = Guid.NewGuid();
            var content = @"<Request><Head><Service>ImportOrders</Service><ServiceVersion>1.0</ServiceVersion><SrcSys>jxw001</SrcSys><DstSys>rfd</DstSys>
<DateTime>" + DateTime.Now.ToString("yyyyMMddhhmmss") + "</DateTime><OnceKey>" + guid.ToString() + "</OnceKey></Head><Body><OrderModel>" + invoice_nos + "</OrderModel></Body></Request>";//请求报文
            WebClient wc = new WebClient();
            wc.Headers.Clear();
            var data = Encoding.UTF8.GetBytes(content);
            wc.Headers.Add("Token", "jxw001|" + SignData(data) + "");//签名
            wc.Encoding = Encoding.UTF8;
            wc.Headers.Add("Content-Type", "application/xml");
            data = wc.UploadData(url, "POST", data);
            var responseStr = Encoding.UTF8.GetString(data);
            return responseStr;
        }

        /// <summary>
        /// 将指定的xml格式的字符反序列化为对应的对象并返回。
        /// </summary>
        /// <param name="t">对象的类型</param>
        /// <param name="xml">待反序列化的xml格式的字符的内容</param>
        /// <returns>返回对应的对象</returns>
        public static Object Deserialize(Type t, string xml)
        {
            Object o = null;
            try
            {
                XmlSerializer serializer = new XmlSerializer(t);
                using (MemoryStream mem = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
                {
                    o = serializer.Deserialize(mem);
                }
            }
            catch { o = null; }
            return o;
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="obj">对象</param>
        /// <returns>返回XML文件</returns>
        public static string SerializeXml(Type type, object obj)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                XmlSerializer xml = new XmlSerializer(type);//创建序列化对象 
                XmlSerializerNamespaces sn = new XmlSerializerNamespaces();
                sn.Add(string.Empty, string.Empty);
                xml.Serialize(ms, obj, sn);
                ms.Position = 0;
                StreamReader sr = new StreamReader(ms, Encoding.UTF8);
                return sr.ReadToEnd().Replace("<?xml version=\"1.0\"?>", string.Empty).Replace(Environment.NewLine, string.Empty).Replace("\n", string.Empty);
            }
        }

    }
}