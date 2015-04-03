using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Task.Model.Order;
using Task.Service.Impl.Order;
using Task.Service.Order;
using TaskPlatform.TaskInterface;

namespace OrderMatchExpress
{
    public class OrderMatchExpressTask : AbstractTask
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
            return "多线程自动匹配快递";
        }
        /// <summary>
        /// 服务内容描述
        /// </summary>
        /// <returns></returns>
        public override string TaskDescription()
        {
            return "分仓库为订单匹配快递";
        }
        /// <summary>
        /// 服务方法
        /// </summary>
        /// <returns></returns>
        public override RunTaskResult RunTask()
        {
            RunTaskResult runTaskResult = new RunTaskResult();
            #region 验证服务配置
            if (string.IsNullOrWhiteSpace(CustomConfig["数据库连接字符串(读)"]))
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
            if (string.IsNullOrWhiteSpace(CustomConfig["一次匹配运单的订单最大数量"]))
            {
                ShowRunningLog("请填写一次匹配运单的订单最大数量！");
                runTaskResult.Result = "请填写一次匹配运单的订单最大数量！";
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
            string strconRead = CustomConfig["数据库连接字符串(读)"];
            string strconWrite = CustomConfig["数据库连接字符串（写）"];
            int maxQty = Convert.ToInt32(CustomConfig["一次匹配运单的订单最大数量"]);
            int mreCount = 0;
            string codOrderSwitch = CustomConfig["货到付款订单"];
            string normalOrderSwitch = CustomConfig["非货到付款订单"];
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
                    TasksCommon tasksCommon = new TasksCommon(wareId, strconRead, strconWrite, maxQty, cc.Key, doneEvents[i], codOrderSwitch, normalOrderSwitch);
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
            ShowRunningLog("=============本次订单匹配快递全部完成=============");
            runTaskResult.Result = "本次订单匹配快递结束";
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
            if (!CustomConfig.ContainsKey("数据库连接字符串(读)"))
                CustomConfig.Add("数据库连接字符串(读)", Task.Common.Parameters.TestDBConnectionString1230);
            if (!CustomConfig.ContainsKey("数据库连接字符串（写）"))
                CustomConfig.Add("数据库连接字符串（写）", Task.Common.Parameters.TestDBConnectionString1230);
            if (!CustomConfig.ContainsKey("一次匹配运单的订单最大数量"))
                CustomConfig.Add("一次匹配运单的订单最大数量", Task.Common.Parameters.MaxMatchCount.ToString());
            if (!CustomConfig.ContainsKey("EmailAddress"))
                CustomConfig.Add("EmailAddress", "wangxudan@jiuxian.com");
            if (!CustomConfig.ContainsKey("货到付款订单"))
                CustomConfig.Add("货到付款订单", "关");
            if (!CustomConfig.ContainsKey("非货到付款订单"))
                CustomConfig.Add("非货到付款订单", "开");
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
            erp_order erpOrder = new erp_order();
            IList<erp_order> orderlist = new List<erp_order>();
            erp_order erpOrderExpress = new erp_order();
            #endregion
            try
            {
                erpOrder.ware_id = tasksCommon.WareId;
                erpOrder.MaxQty = tasksCommon.MaxQty;
                orderlist = mOrderService.GetUnmatchExpressOrderListByThreadBLL(tasksCommon.ConnStrRead, erpOrder);
                int successQty = 0;
                string orderid = "";
                if(orderlist==null||orderlist.Count==0)
                {
                    #region 没有需要关联快递的订单
                    runTaskResult.Success = true;
                    runTaskResult.Result = "<--" + tasksCommon.DealFullName +"]当前没有需要匹配快递的订单-->";
                    ShowRunningLog("<--" + tasksCommon.DealFullName +"]当前没有需要匹配快递的订单-->");
                    WriteLog("<--" + tasksCommon.DealFullName +"]当前没有需要匹配快递的订单-->");
                    #endregion
                }
                else
                {
                    foreach (var item in orderlist)
                    {
                        item.ware_id = tasksCommon.WareId;
                        erpOrderExpress = mOrderService.GetExpressInfoByCityIdAndWareIdBLL(tasksCommon.ConnStrRead, item);

                        if (erpOrderExpress == null)
                        {
                            continue;
                        }
                        erpOrderExpress.order_id = item.order_id;
                        if(mOrderService.UpdateExpressByOrderIdBLL(tasksCommon.ConnStrWrite,erpOrderExpress))
                        {
                            successQty++;
                            orderid += item.order_id.ToString() + ":" + erpOrderExpress.shipping_id+ ",";
                        }
                    
                    }
                    //orderid = orderid.Remove(orderid.Length - 1, 1);
                    #region 匹配成功,输出结果
                    runTaskResult.Success = true;
                    runTaskResult.Result = tasksCommon.DealFullName + "<--" + tasksCommon.DealFullName + "]订单本次匹配快递:{" + orderlist.Count + "}单,成功:{" + successQty.ToString() + "}单,订单Id集合为:" + orderid + "-->";
                    ShowRunningLog("<--" + tasksCommon.DealFullName + "]订单本次匹配快递:{" + orderlist.Count + "}单,成功:{" + successQty.ToString() + "}单,订单Id集合为:" + orderid + "-->");
                    WriteLog("<--" + tasksCommon.DealFullName + "]订单本次匹配快递:{" + orderlist.Count + "}单,成功:{" + successQty.ToString() + "}单,订单Id集合为:" + orderid + "-->");
                    #endregion
                    #region 匹配有失败,发邮件通知处理
                    if (orderlist.Count > successQty)
                    {
                        SendEmailTo(TaskName() +"<--" + tasksCommon.DealFullName + "]订单本次匹配快递:{" + orderlist.Count + "}单,成功:{" + successQty.ToString() + "}单-->", CustomConfig["EmailAddress"]);
                    }
                    #endregion

                }
            }
            catch (Exception ex)
            {
                #region 捕获到异常,发邮件通知处理
                runTaskResult.Success = false;
                runTaskResult.Result = ex.ToString();
                SendEmailTo(TaskName() + ex, CustomConfig["EmailAddress"]);
                #endregion
            }
            finally
            {
                #region 线程结束
                tasksCommon.DoneEvent.Set();
                #endregion
            }

        }
    }
}
