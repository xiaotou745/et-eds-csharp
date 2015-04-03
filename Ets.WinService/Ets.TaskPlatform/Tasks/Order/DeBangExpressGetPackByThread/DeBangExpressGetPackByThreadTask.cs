using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
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

namespace DeBangExpressGetPackByThread
{
    public class DeBangExpressGetPackByThreadTask : AbstractTask
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
            return "德邦自动筛单(多线程)";
        }

        /// <summary>
        ///服务描述
        /// </summary>
        /// <returns></returns>
        public override string TaskDescription()
        {
            return "通过接口推送订单信息给德邦，德邦验证订单是否能送达，能送达返回对应信息，不能送达就改为人工筛单";
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
#if DEBUG
           if (!CustomConfig.ContainsKey("数据库连接字符串（读）"))
                CustomConfig.Add("数据库连接字符串（读）",
                    Task.Common.Parameters.TestDBReadConnectionString0578);
            if (!CustomConfig.ContainsKey("数据库连接字符串（写）"))
                CustomConfig.Add("数据库连接字符串（写）",
                    Task.Common.Parameters.TestDBConnectionString0578);
            if (!CustomConfig.ContainsKey("EmailAddress"))
                CustomConfig.Add("EmailAddress", "wangxudan@jiuxian.com;");
            if (!CustomConfig.ContainsKey("快递"))
                CustomConfig.Add("快递", "93");
            if (!CustomConfig.ContainsKey("订单数量限制"))
                CustomConfig.Add("订单数量限制", Task.Common.Parameters.MaxGetPackCountDeBang.ToString());
            if (!CustomConfig.ContainsKey("2-广州仓"))
                CustomConfig.Add("2-广州仓", "关");
            if (!CustomConfig.ContainsKey("3-上海仓"))
                CustomConfig.Add("3-上海仓", "开");
            if (!CustomConfig.ContainsKey("4-武汉仓"))
                CustomConfig.Add("4-武汉仓", "开");
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
                ParameterInt=1,
            };
            #endregion
            try
            {
                Log("【" + tasksCommon.DealFullName + "】正在获取需要筛单的德邦快递订单……");
                var orderList = mOrderService.QueryNoSieveOrdersOfDeBangBLL(config);
                if (orderList==null || orderList.Count==0)
                {
                    #region 没有需要筛单的订单

                    runTaskResult.Success = true;
                    runTaskResult.Result = "【" + tasksCommon.DealFullName + "】当前没有需要筛单的德邦快递订单！";
                    Log("【" + tasksCommon.DealFullName + "】当前没有需要筛单的德邦快递订单！");

                    #endregion
                }
                else
                {
                    #region 处理需要筛单的订单

                    Log("【" + tasksCommon.DealFullName + "】德邦快递获取到需要筛单的订单，共计：【" + orderList.Count + "】单！");
                    var listSieveOrderResultInfo = new List<SieveOrderReceiveInfoOfDeBang>();
                    foreach (var item in orderList)
                    {
                        var sieveOrderPushInfoOfDeBang = new SieveOrderPushInfoOfDeBang
                        {
                            logisticCompanyID = "DEPPON",
                            province = item.province,
                            city = item.city,
                            county = item.county,
                            address = item.address,
                            longitude = "",
                            latitude = "",
                            //matchType = 6
                        };
                        string orderJson = XMLCommon.ObjToJson(sieveOrderPushInfoOfDeBang);
                        //orderJson ="{"+ orderJson.Substring(13);
                        //string json = @"{""logisticCompanyID"":""DEPPON "",""province"":""上海"",""city"":""上海市"",""county"":""青浦区"",""address"":""徐泾镇明珠路1018号"",""longitude"":"""",""latitude"":"""",""matchType"":6 }";
                        //orderJson = @"{""address"":""杨村雍阳西道118号"",""city"":""天津市"",""county"":""武清区"",""latitude"":"""",""logisticCompanyID"":""DEPPON"",""longitude"":"""",""province"":""天津""}";
                        string strResult = DeBangGetPack(orderJson);
                        //string strResult = "jfjdod";
                        if (!string.IsNullOrWhiteSpace(strResult))
                        {
                            //var sieveOrderResultInfo = new SieveOrderReceiveInfoOfDeBang
                            //    {
                            //        result = true,
                            //        reason = "",
                            //        responseParam = new responseParam
                            //            {
                            //                expressDelivery = true,
                            //                logisticCompanyID = "DEPPON"
                            //            },
                            //        resultCode = "1000",
                            //        uniquerRequestNumber = "3467932705110701"
                            //    };
                            var sieveOrderResultInfo = (SieveOrderReceiveInfoOfDeBang)XMLCommon.JsonToObj(strResult, typeof(SieveOrderReceiveInfoOfDeBang));
                            if (!sieveOrderResultInfo.result)
                            {
                                runTaskResult.Success = false;
                                runTaskResult.Result = sieveOrderResultInfo.reason;
                                Log(sieveOrderResultInfo.reason);
                                SendEmailTo(TaskName() + sieveOrderResultInfo.reason, CustomConfig["EmailAddress"]);
                            }
                            else
                            {
                                sieveOrderResultInfo.OrderId = item.OrderId.ToString();
                                listSieveOrderResultInfo.Add(sieveOrderResultInfo);
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
                        var sieveOrderDealSF = mOrderService.DealSieveOrderResultOfDeBang(listSieveOrderResultInfo, config);
                        if (sieveOrderDealSF.DealFlag)
                        {
                            runTaskResult.Result = string.Format("【{0}】本次德邦筛单{1}单，成功{2}单，可送达{3}单，订单ID集合为：【{5}】，不能送达{4}单，订单ID集合为：【{6}】", tasksCommon.DealFullName, listSieveOrderResultInfo.Count, sieveOrderDealSF.DealSuccQty, sieveOrderDealSF.ReachQty, sieveOrderDealSF.UnReachQty, sieveOrderDealSF.ReachOrderId, sieveOrderDealSF.UnReachOrderId);
                            Log(string.Format("【{0}】本次德邦筛单{1}单，成功{2}单，可送达{3}单，订单ID集合为：【{5}】，不能送达{4}单，订单ID集合为：【{6}】", tasksCommon.DealFullName, listSieveOrderResultInfo.Count, sieveOrderDealSF.DealSuccQty, sieveOrderDealSF.ReachQty, sieveOrderDealSF.UnReachQty, sieveOrderDealSF.ReachOrderId, sieveOrderDealSF.UnReachOrderId));
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
        /// 德邦筛单方法
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public string DeBangGetPack(string json)
        {
            string timespan = CreateTimestamp();
            string COMPARNY = "JIUXIANWANG";
#if DEBUG
            string key = "doptest" + timespan;
            string value = json + key;
            string digest = Createdigest(value);
            Uri myUri = new Uri("http://58.40.17.67/dop/dept/queryExpressDeptByAddress.action");
            //Uri myUri = new Uri("http://58.40.17.67/dop/dept/queryDeptByAddress.action");
#else
            Uri myUri = new Uri("http://api.deppon.com/dop/dept/queryExpressDeptByAddress.action");
            string key = "E35C7A384A00CE602B64FCD27F54D7C0" + timespan;
            string value = json + key;
            string digest = Createdigest(value);
#endif

            Encoding myEncoding = Encoding.GetEncoding("gb2312");
            UTF8Encoding encoding2 = new UTF8Encoding();
            string sendPrams2 = HttpUtility.UrlEncode("companyCode", myEncoding) + "=" +
                        HttpUtility.UrlEncode(COMPARNY, myEncoding) + "&" +
                        HttpUtility.UrlEncode("params", myEncoding) + "=" +
                        HttpUtility.UrlEncode(json, encoding2) + "&" +
                        HttpUtility.UrlEncode("digest", myEncoding) + "=" +
                        HttpUtility.UrlEncode(digest, myEncoding) + "&" +
                        HttpUtility.UrlEncode("timestamp", myEncoding) + "=" +
                        HttpUtility.UrlEncode(timespan, myEncoding);
            return PostDataGetHtml(myUri, sendPrams2);
           
        }
        /// <summary>
        /// 调用德邦筛单接口
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public static string PostDataGetHtml(Uri url, string postData)
        {
            byte[] data = Encoding.GetEncoding("UTF-8").GetBytes(postData);
            var req = WebRequest.Create(url) as HttpWebRequest;
            req.Method = "POST";
            req.KeepAlive = true;
            req.ContentType = "application/x-www-form-urlencoded";
            req.ContentLength = data.Length;
            req.AllowAutoRedirect = true;
            Stream outStream = req.GetRequestStream();
            outStream.Write(data, 0, data.Length);
            outStream.Close();
            HttpWebResponse res;
            try
            {
                res = req.GetResponse() as HttpWebResponse;
            }
            catch (WebException ex)
            {
                res = (HttpWebResponse)ex.Response;
            }

            var inStream = res.GetResponseStream();
            var sr = new StreamReader(inStream, Encoding.GetEncoding("UTF-8"));
            string htmlResult = sr.ReadToEnd();
            return htmlResult;
        }

        /// <summary>
        /// 德邦密文生成器
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string Createdigest(string param)
        {
            byte[] buffer = Encoding.GetEncoding("UTF-8").GetBytes(param);
            System.Security.Cryptography.MD5CryptoServiceProvider check;
            check = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] somme = check.ComputeHash(buffer);
            string ret = "";
            foreach (byte a in somme)
            {
                if (a < 16)
                    ret += "0" + a.ToString("X");
                else
                    ret += a.ToString("X");
            }
            string re = Convert.ToBase64String(Encoding.GetEncoding("UTF-8").GetBytes(ret.ToLower()));
            return re;
        }

        /// <summary>
        /// 德邦时间差生成
        /// </summary>
        /// <returns></returns>
        public static string CreateTimestamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            string time = Convert.ToInt64(ts.TotalMilliseconds).ToString();
            return time;
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

    }
}
