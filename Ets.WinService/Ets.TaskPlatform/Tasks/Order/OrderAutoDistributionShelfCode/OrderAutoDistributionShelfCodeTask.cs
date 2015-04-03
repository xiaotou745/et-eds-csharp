using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Task.Model.Order;
using Task.Service.Impl.Order;
using Task.Service.Order;
using TaskPlatform.TaskInterface;

namespace OrderAutoDistributionShelfCode
{
    public class OrderAutoDistributionShelfCodeTask : AbstractTask
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
            return "自动为订单分配拣货位";
        }
        /// <summary>
        /// 服务内容描述
        /// </summary>
        /// <returns></returns>
        public override string TaskDescription()
        {
            return "分仓库自动为订单分配拣货位";
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
            if (string.IsNullOrWhiteSpace(CustomConfig["一次分配拣货位订单最大数量"]))
            {
                ShowRunningLog("请填写一次分配拣货位订单最大数量！");
                runTaskResult.Result = "请填写一次分配拣货位订单最大数量！";
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
            int maxQty = Convert.ToInt32(CustomConfig["一次分配拣货位订单最大数量"]);
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
                    TasksCommon tasksCommon = new TasksCommon(wareId, strconRead, strconWrite, maxQty, cc.Key, doneEvents[i]);
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
            ShowRunningLog("=============本次订单分配拣货位全部完成=============");
            runTaskResult.Result = "本次订单分配拣货位结束";
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
                CustomConfig.Add("数据库连接字符串(读)", Task.Common.Parameters.TestDBConnectionString0578);
            if (!CustomConfig.ContainsKey("数据库连接字符串（写）"))
                CustomConfig.Add("数据库连接字符串（写）", Task.Common.Parameters.TestDBConnectionString0578);
            if (!CustomConfig.ContainsKey("一次分配拣货位订单最大数量"))
                CustomConfig.Add("一次分配拣货位订单最大数量", Task.Common.Parameters.MaxDistributionCount.ToString());
            if (!CustomConfig.ContainsKey("EmailAddress"))
                CustomConfig.Add("EmailAddress", "wangxudan@jiuxian.com");
            if (!CustomConfig.ContainsKey("正常订单"))
                CustomConfig.Add("正常订单", "开");
            if (!CustomConfig.ContainsKey("返货区订单"))
                CustomConfig.Add("返货区订单", "开");
            if (!CustomConfig.ContainsKey("1-北京仓"))
                CustomConfig.Add("1-北京仓", "开");
            if (!CustomConfig.ContainsKey("2-广州仓"))
                CustomConfig.Add("2-广州仓", "关");
            if (!CustomConfig.ContainsKey("3-上海仓"))
                CustomConfig.Add("3-上海仓", "关");
            if (!CustomConfig.ContainsKey("4-武汉仓"))
                CustomConfig.Add("4-武汉仓", "关");
            if (!CustomConfig.ContainsKey("5-天津仓"))
                CustomConfig.Add("5-天津仓", "关");
            if (!CustomConfig.ContainsKey("6-成都仓"))
                CustomConfig.Add("6-成都仓", "关");
            if (!CustomConfig.ContainsKey("8-中酿天津仓"))
                CustomConfig.Add("8-中酿天津仓", "关");
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
            #endregion
            try
            {
                if (CustomConfig["正常订单"] == "开")
                {
                    #region 正常订单
                    #region 声明对象
                    IList<WmsReceiptTaskDetailDto> unDistributionShelfCodeOrderList = new List<WmsReceiptTaskDetailDto>();
                    erp_order erpOrder = new erp_order();
                    bool dealFlag = false;
                    string successOrderId = "";
                    string failureOrderId = "";
                    int successQty = 0;
                    #endregion
                    #region 获取需要分配拣货位的订单
                    erpOrder.ware_id = tasksCommon.WareId; 
                    erpOrder.MaxQty = tasksCommon.MaxQty;
                    erpOrder.stocks = 1;
                    ShowRunningLog("<--" + tasksCommon.DealFullName + "【正常订单】未分配拣货位的订单获取中-->");
                    unDistributionShelfCodeOrderList = mOrderService.GetUnDistributionShelfCodeOrderListByThreadBLL(tasksCommon.ConnStrRead, erpOrder);
                    #endregion
                    if (unDistributionShelfCodeOrderList!=null && unDistributionShelfCodeOrderList.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(unDistributionShelfCodeOrderList[0].ExceptionStr))
                        {
                            #region 程序异常
                            runTaskResult.Success = false;
                            runTaskResult.Result = unDistributionShelfCodeOrderList[0].ExceptionStr;
                            ShowRunningLog("<--" + tasksCommon.DealFullName + "【正常订单】分配拣货位异常，详情如下：" + unDistributionShelfCodeOrderList[0].ExceptionStr + "-->");
                            WriteLog("<--" + tasksCommon.DealFullName + "【正常订单】分配拣货位异常，详情如下：" + unDistributionShelfCodeOrderList[0].ExceptionStr + "-->");
                            SendEmailTo(TaskName() + tasksCommon.DealFullName + "【正常订单】分配拣货位异常，详情如下：" + unDistributionShelfCodeOrderList[0].ExceptionStr, CustomConfig["EmailAddress"]);
                            #endregion
                        }
                        #region 有需要分配拣货位的订单
                        var listReceiptIdsAll = unDistributionShelfCodeOrderList.Select(o => o.ReceiptId).Distinct().ToList();
                        var listReceiptIdsExcpt = unDistributionShelfCodeOrderList.Where(o => o.StockCode == null).Select(o => o.ReceiptId).Distinct().ToList();
                        var listReceiptIds = listReceiptIdsAll.Except(listReceiptIdsExcpt).ToList();
                        ShowRunningLog(tasksCommon.DealFullName + "【正常订单】未分配拣货位的订单获取完成,共计:{" + listReceiptIds.Count + "}单");
                        WriteLog(tasksCommon.DealFullName + "【正常订单】未分配拣货位的订单获取完成,共计:{" + listReceiptIds + "}单");
                        foreach (var receiptId in listReceiptIds)
                        {
                            IList<WmsReceiptTaskDetailDto> sublist = unDistributionShelfCodeOrderList.Where(o => o.ReceiptId == receiptId).ToList();
                            dealFlag = mOrderService.DistributionShelfCodeOrderListByThreadBLL(tasksCommon.ConnStrWrite, sublist);
                            if (dealFlag)
                            {
                                successOrderId += receiptId.ToString() + ',';
                                successQty++;
                            }
                            else
                            {
                                failureOrderId += receiptId.ToString() + ',';
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(successOrderId))
                        {
                            successOrderId = successOrderId.Remove(successOrderId.Length - 1, 1);
                        }
                        if (!string.IsNullOrWhiteSpace(failureOrderId))
                        {
                            failureOrderId = failureOrderId.Remove(failureOrderId.Length - 1, 1);
                        }
                        #region 匹配成功,输出结果
                        runTaskResult.Success = true;
                        runTaskResult.Result = tasksCommon.DealFullName + "【正常订单】分配拣货位已完成";
                        ShowRunningLog("<--" + tasksCommon.DealFullName + "【正常订单】分配拣货位:{" + listReceiptIds.Count + "}单,成功:{" + successQty + "}单,订单ID集合为：【" + successOrderId + "】-->");
                        WriteLog("<--" + tasksCommon.DealFullName + "【正常订单】分配拣货位:{" + listReceiptIds.Count + "}单,成功:{" + successQty + "}单,订单ID集合为：【" + successOrderId + "】-->");
                        #endregion
                        #region 匹配有失败,发邮件通知处理
                        if (listReceiptIds.Count > successQty)
                        {
                            SendEmailTo(TaskName() + "<--" + tasksCommon.DealFullName + "【正常订单】分配拣货位,共分配:{" + listReceiptIds.Count + "}单,仅成功:{" + successQty + "}单，失败的订单ID集合为：【" + failureOrderId + "】-->", CustomConfig["EmailAddress"]);
                        }
                        #endregion
                        #endregion

                    }
                    else
                    {
                         #region 没有需要分配拣货位的订单
                            runTaskResult.Success = true;
                            runTaskResult.Result = "<--" + tasksCommon.DealFullName + "【正常订单】当前没有需要分配拣货位的订单-->";
                            ShowRunningLog("<--" + tasksCommon.DealFullName + "【正常订单】当前没有需要分配拣货位的订单-->");
                            WriteLog("<--" + tasksCommon.DealFullName + "【正常订单】当前没有需要分配拣货位的订单-->");
                            #endregion
                    }
                    #endregion
                }
                if (CustomConfig["返货区订单"] == "开")
                {
                    #region 返货区订单
                    #region 声明对象
                    IList<WmsReceiptTaskDetailDto> unDistributionShelfCodeOrderList = new List<WmsReceiptTaskDetailDto>();
                    erp_order erpOrder = new erp_order();
                    bool dealFlag = false;
                    string successOrderId = "";
                    string failureOrderId = "";
                    int successQty = 0;
                    #endregion
                    #region 获取需要分配拣货位的订单
                    erpOrder.ware_id = tasksCommon.WareId;
                    erpOrder.MaxQty = tasksCommon.MaxQty;
                    erpOrder.stocks = 2;
                    ShowRunningLog("<--" + tasksCommon.DealFullName + "【返货区订单】未分配拣货位的订单获取中-->");
                    unDistributionShelfCodeOrderList = mOrderService.GetUnDistributionShelfCodeOrderListByThreadBLL(tasksCommon.ConnStrRead, erpOrder);
                    #endregion
                    if (unDistributionShelfCodeOrderList!=null && unDistributionShelfCodeOrderList.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(unDistributionShelfCodeOrderList[0].ExceptionStr))
                        {
                            #region 程序异常
                            runTaskResult.Success = false;
                            runTaskResult.Result = unDistributionShelfCodeOrderList[0].ExceptionStr;
                            ShowRunningLog("<--" + tasksCommon.DealFullName + "【返货区订单】分配拣货位异常，详情如下：" + unDistributionShelfCodeOrderList[0].ExceptionStr + "-->");
                            WriteLog("<--" + tasksCommon.DealFullName + "【返货区订单】分配拣货位异常，详情如下：" + unDistributionShelfCodeOrderList[0].ExceptionStr + "-->");
                            SendEmailTo(TaskName() + tasksCommon.DealFullName + "【返货区订单】分配拣货位异常，详情如下：" + unDistributionShelfCodeOrderList[0].ExceptionStr, CustomConfig["EmailAddress"]);
                            #endregion
                        }
                        else
                        {
                            #region 有需要分配拣货位的订单
                            var listReceiptIds = unDistributionShelfCodeOrderList.Select(o => o.ReceiptId).Distinct().ToList();
                            ShowRunningLog(tasksCommon.DealFullName + "【返货区订单】未分配拣货位的订单获取完成,共计:{" + listReceiptIds.Count + "}单");
                            WriteLog(tasksCommon.DealFullName + "【返货区订单】未分配拣货位的订单获取完成,共计:{" + listReceiptIds.Count + "}单");
                            foreach (int receiptId in listReceiptIds)
                            {
                                IList<WmsReceiptTaskDetailDto> sublist = unDistributionShelfCodeOrderList.Where(o => o.ReceiptId == receiptId).ToList();
                                //var list = unDistributionShelfCodeOrderList.Select(m => m.ReceiptId).ToList();
                                dealFlag = mOrderService.DistributionShelfCodeOrderListByThreadBLL(tasksCommon.ConnStrWrite, sublist);
                                if (dealFlag)
                                {
                                    successOrderId += receiptId.ToString() + ',';
                                    successQty++;
                                }
                                else
                                {
                                    failureOrderId += receiptId.ToString() + ',';
                                }
                            }
                            if (!string.IsNullOrWhiteSpace(successOrderId))
                            {
                                successOrderId = successOrderId.Remove(successOrderId.Length - 1, 1);
                            }
                            if (!string.IsNullOrWhiteSpace(failureOrderId))
                            {
                                failureOrderId = failureOrderId.Remove(failureOrderId.Length - 1, 1);
                            }
                            #region 匹配成功,输出结果
                            runTaskResult.Success = true;
                            runTaskResult.Result = tasksCommon.DealFullName + "【返货区订单】分配拣货位已完成";
                            ShowRunningLog("<--" + tasksCommon.DealFullName + "【返货区订单】分配拣货位:{" + listReceiptIds.Count + "}单,成功:{" + successQty + "}单,订单ID集合为：【" + successOrderId + "】-->");
                            WriteLog("<--" + tasksCommon.DealFullName + "【返货区订单】分配拣货位:{" + listReceiptIds.Count + "}单,成功:{" + successQty + "}单,订单ID集合为：【" + successOrderId + "】-->");
                            #endregion
                            #region 匹配有失败,发邮件通知处理
                            if (listReceiptIds.Count > successQty)
                            {
                                SendEmailTo(TaskName() + "<--" + tasksCommon.DealFullName + "【返货区订单】分配拣货位,共分配:{" + listReceiptIds.Count + "}单,仅成功:{" + successQty + "}单，失败的订单ID集合为：【" + failureOrderId + "】-->", CustomConfig["EmailAddress"]);
                            }
                            #endregion
                            #endregion
                        }

                    }
                    else
                    {
                        #region 没有需要分配拣货位的订单
                        runTaskResult.Success = true;
                        runTaskResult.Result = "<--" + tasksCommon.DealFullName + "【返货区订单】当前没有需要分配拣货位的订单-->";
                        ShowRunningLog("<--" + tasksCommon.DealFullName + "【返货区订单】当前没有需要分配拣货位的订单-->");
                        WriteLog("<--" + tasksCommon.DealFullName + "【返货区订单】当前没有需要分配拣货位的订单-->");
                        #endregion
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
