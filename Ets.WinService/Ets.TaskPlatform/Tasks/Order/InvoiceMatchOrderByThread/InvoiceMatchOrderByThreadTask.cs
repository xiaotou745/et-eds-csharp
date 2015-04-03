using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Task.Model;
using Task.Model.Order;
using Task.Service.Impl.Order;
using Task.Service.Order;
using TaskPlatform.TaskInterface;
using System.Threading;

namespace InvoiceMatchOrderByThread
{
    public class InvoiceMatchOrderByThreadTask : AbstractTask
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
            return "多线程自动为订单匹配运单";
        }
        /// <summary>
        /// 服务内容描述
        /// </summary>
        /// <returns></returns>
        public override string TaskDescription()
        {
            return "分仓库和运单类型自动为订单匹配运单";
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
            string codOrderSwitch = CustomConfig["COD运单"];
            string normalOrderSwitch = CustomConfig["快递公司电子运单"];
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
                    if (!cc.Key.Contains("-") )
                    {
                        continue;
                    }
                    if (cc.Value == "关")
                    {
                        continue;
                    }
                    string[] key = cc.Key.Split(new[] {'-'}, StringSplitOptions.RemoveEmptyEntries);
                    int threadIndex = Convert.ToInt32(key[0]);
                    int wareId = Convert.ToInt32(key[0]);
                    doneEvents[i] = new ManualResetEvent(false);
                    TasksCommon tasksCommon = new TasksCommon(wareId,strconRead,strconWrite , maxQty, cc.Key, doneEvents[i], codOrderSwitch, normalOrderSwitch);
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
            ShowRunningLog("=============本次订单匹配运单全部完成=============");
            runTaskResult.Result = "本次订单匹配运单结束";
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
                CustomConfig.Add("数据库连接字符串(读)", Task.Common.Parameters.TestDBReadConnectionString0578);
            if (!CustomConfig.ContainsKey("数据库连接字符串（写）"))
                CustomConfig.Add("数据库连接字符串（写）", Task.Common.Parameters.TestDBConnectionString0578);
            if (!CustomConfig.ContainsKey("一次匹配运单的订单最大数量"))
                CustomConfig.Add("一次匹配运单的订单最大数量",Task.Common.Parameters.MaxMatchCount.ToString());
            if (!CustomConfig.ContainsKey("EmailAddress"))
                CustomConfig.Add("EmailAddress", "wangxudan@jiuxian.com");
            if (!CustomConfig.ContainsKey("快递公司电子运单"))
                CustomConfig.Add("快递公司电子运单", "开");
            if (!CustomConfig.ContainsKey("COD运单"))
                CustomConfig.Add("COD运单", "开");
            if (!CustomConfig.ContainsKey("特殊运单"))
                CustomConfig.Add("特殊运单", "开");
            if (!CustomConfig.ContainsKey("1-北京仓"))
                CustomConfig.Add("1-北京仓", "关");
            if (!CustomConfig.ContainsKey("2-广州仓"))
                CustomConfig.Add("2-广州仓", "关");
            if (!CustomConfig.ContainsKey("3-上海仓"))
                CustomConfig.Add("3-上海仓", "关");
            if (!CustomConfig.ContainsKey("4-武汉仓"))
                CustomConfig.Add("4-武汉仓", "关");
            if (!CustomConfig.ContainsKey("5-天津仓"))
                CustomConfig.Add("5-天津仓", "开");
            if (!CustomConfig.ContainsKey("6-成都仓"))
                CustomConfig.Add("6-成都仓", "关");
            if (!CustomConfig.ContainsKey("7-山西仓"))
                CustomConfig.Add("7-山西仓", "关");
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
            erp_order erpOrder = new erp_order();
            IList<erp_order> orderlist = new List<erp_order>();
            IList<wms_invoiceInfo> invoicelist = new List<wms_invoiceInfo>();
            #endregion
            try
            {
                var config = new Config
                    {
                        WareId=tasksCommon.WareId,
                        ReadConnectionString=tasksCommon.ConnStrRead,
                        WriteConnectionString=tasksCommon.ConnStrWrite,
                    };
                var wmsShippingConfig = new WmsShippingConfig
                    {
                        WareId=tasksCommon.WareId,
                    };
                var shippingList = mOrderService.GetShippingConfigListInfoBLL(config, wmsShippingConfig);
                if (shippingList != null && shippingList.Count > 0)
                {
                    if (!string.IsNullOrEmpty(shippingList[0].ExceptionStr))
                    {
                        #region 获取快递列表失败,发邮件通知处理

                        runTaskResult.Success = false;
                        runTaskResult.Result = TaskName() + "<--" + tasksCommon.DealFullName +
                                               "订单匹配电子运单获取快递列表时失败：" + shippingList[0].ExceptionStr + "-->";
                        ShowRunningLog(TaskName() + "<--" + tasksCommon.DealFullName + "订单匹配电子运单获取快递列表时失败：" +
                                       shippingList[0].ExceptionStr + "-->");
                        WriteLog(TaskName() + "<--" + tasksCommon.DealFullName + "订单匹配电子运单获取快递列表时失败：" +
                                 shippingList[0].ExceptionStr + "-->");
                        SendEmailTo(
                            TaskName() + "<--" + tasksCommon.DealFullName + "订单匹配电子运单获取快递列表时失败：" +
                            shippingList[0].ExceptionStr + "-->", CustomConfig["EmailAddress"]);

                        #endregion
                    }
                    else
                    {
                        #region 分类匹配电子运单
                        if (tasksCommon.CodOrderSwitch == "开")
                        {
                            #region COD电子运单

                            var codShippingList = shippingList.Where(t => t.InvoiceType == 2 && t.IsSpare == 1).ToList();
                            if (codShippingList.Count == 0)
                            {
                                #region 没有需要关联运单的订单

                                runTaskResult.Success = true;
                                runTaskResult.Result = "<--" + tasksCommon.DealFullName + "【COD快递】中当前没有需要匹配电子运单的订单-->";
                                ShowRunningLog("<--" + tasksCommon.DealFullName + "【COD快递】中当前没有需要匹配电子运单的订单-->");
                                WriteLog("<--" + tasksCommon.DealFullName + "【COD快递】中当前没有需要匹配电子运单的订单-->");

                                #endregion
                            }
                            else
                            {
                                #region 匹配对应快递的运单

                                foreach (var shiplist in codShippingList)
                                {
                                    #region 获取未关联电子运单的订单

                                    erpOrder.shipping_id = shiplist.ShippingId;
                                    erpOrder.ware_id = tasksCommon.WareId;
                                    erpOrder.MaxQty = tasksCommon.MaxQty;
                                    erpOrder.InvoiceType = shiplist.InvoiceType;
                                    erpOrder.SieveOrderType = shiplist.SieveOrderType;
                                    ShowRunningLog("<--" + tasksCommon.DealFullName + "[" + shiplist.ShippingName +
                                                    "]订单未匹配电子运单的订单获取中-->");
                                    WriteLog("<--" + tasksCommon.DealFullName + "[" + shiplist.ShippingName +
                                                "]订单未匹配电子运单的订单获取中-->");
                                    orderlist =
                                        mOrderService.GetUnmatchOrderListByThreadBLL(tasksCommon.ConnStrWrite,
                                                                                        erpOrder);

                                    #endregion

                                    if (orderlist!=null && orderlist.Count > 0)
                                    {
                                        if (!string.IsNullOrEmpty(orderlist[0].ExceptionStr))
                                        {
                                            #region 获取订单列表失败,发邮件通知处理

                                            runTaskResult.Success = false;
                                            runTaskResult.Result = TaskName() + "<--" + tasksCommon.DealFullName +
                                                                    "【COD快递】中订单匹配电子运单获取未匹配运单的订单时失败：" +
                                                                    orderlist[0].ExceptionStr + "-->";
                                            ShowRunningLog(TaskName() + "<--" + tasksCommon.DealFullName +
                                                            "【COD快递】中订单匹配电子运单获取未匹配运单的订单时失败：" +
                                                            orderlist[0].ExceptionStr + "-->");
                                            WriteLog(TaskName() + "<--" + tasksCommon.DealFullName +
                                                        "【COD快递】中订单匹配电子运单获取未匹配运单的订单时失败：" + orderlist[0].ExceptionStr +
                                                        "-->");
                                            SendEmailTo(
                                                TaskName() + "<--" + tasksCommon.DealFullName +
                                                "【COD快递】中订单匹配电子运单获取未匹配运单的订单时失败：" + orderlist[0].ExceptionStr + "-->",
                                                CustomConfig["EmailAddress"]);

                                            #endregion
                                        }
                                        else
                                        {
                                            ShowRunningLog(tasksCommon.DealFullName + "[" + shiplist.ShippingName +
                                                            "]订单未匹配电子运单的订单获取完成,共计:{" + orderlist.Count + "}单");
                                            WriteLog(tasksCommon.DealFullName + "[" + shiplist.ShippingName +
                                                        "]订单未匹配电子运单的订单获取完成,共计:{" + orderlist.Count + "}单");

                                            #region 订单匹配运单

                                            ShowRunningLog(tasksCommon.DealFullName + "[" + shiplist.ShippingName +
                                                            "]订单匹配电子运单进行中……");
                                            var orderinvoicelist =
                                                mOrderService.UpdateCODOrderInvoicesInfoByThreadBLL(orderlist,
                                                                                                    tasksCommon
                                                                                                        .ConnStrWrite);

                                            #endregion

                                            if (orderinvoicelist[0].ResultFlag)
                                            {
                                                #region 匹配成功,输出结果

                                                runTaskResult.Success = true;
                                                runTaskResult.Result = tasksCommon.DealFullName + "[" +
                                                                        shiplist.ShippingName + "]订单匹配电子运单已完成";
                                                ShowRunningLog("<--" + tasksCommon.DealFullName + "[" +
                                                                shiplist.ShippingName + "]订单本次匹配电子运单:{" +
                                                                orderlist.Count + "}单,成功:{" +
                                                                orderinvoicelist[0].successQty + "}单-->");
                                                WriteLog("<--" + tasksCommon.DealFullName + "[" +
                                                            shiplist.ShippingName + "]订单本次匹配电子运单:{" + orderlist.Count +
                                                            "}单,成功:{" + orderinvoicelist[0].successQty + "}单-->");

                                                #endregion

                                                #region 匹配有失败,发邮件通知处理

                                                if (orderlist.Count > orderinvoicelist[0].successQty)
                                                {
                                                    SendEmailTo(
                                                        TaskName() + "<--" + tasksCommon.DealFullName + "[" +
                                                        shiplist.ShippingName + "]订单匹配电子运单时有失败订单,共匹配:{" +
                                                        orderlist.Count + "}单,仅成功:{" +
                                                        orderinvoicelist[0].successQty + "}单-->",
                                                        CustomConfig["EmailAddress"]);
                                                }

                                                #endregion
                                            }
                                            else
                                            {
                                                #region 匹配全部失败,回滚数据,发邮件通知处理

                                                runTaskResult.Success = false;
                                                runTaskResult.Result = tasksCommon.DealFullName + "[" +
                                                                        shiplist.ShippingName + "]订单匹配电子运单失败,数据正在回滚";
                                                if (!string.IsNullOrEmpty(orderinvoicelist[0].Exception))
                                                {
                                                    ShowRunningLog(tasksCommon.DealFullName + "[" +
                                                                    shiplist.ShippingName + "]订单匹配电子运单失败,数据正在回滚,原因:" +
                                                                    orderinvoicelist[0].Exception);
                                                    WriteLog(tasksCommon.DealFullName + "[" + shiplist.ShippingName +
                                                                "]订单匹配电子运单失败,数据正在回滚,原因:" +
                                                                orderinvoicelist[0].Exception);
                                                    SendEmailTo(
                                                        TaskName() + tasksCommon.DealFullName + "[" +
                                                        shiplist.ShippingName + "]订单匹配电子运单失败,数据正在回滚,原因:" +
                                                        orderinvoicelist[0].Exception, CustomConfig["EmailAddress"]);
                                                }
                                                else
                                                {
                                                    ShowRunningLog(tasksCommon.DealFullName + "[" +
                                                                    shiplist.ShippingName + "]订单匹配电子运单失败,数据正在回滚……");
                                                    WriteLog(tasksCommon.DealFullName + "[" + shiplist.ShippingName +
                                                                "]订单匹配电子运单失败,数据正在回滚,原因:" +
                                                                orderinvoicelist[0].Exception);
                                                    SendEmailTo(
                                                        TaskName() + tasksCommon.DealFullName + "[" +
                                                        shiplist.ShippingName + "]订单匹配电子运单失败,数据正在回滚……",
                                                        CustomConfig["EmailAddress"]);
                                                }

                                                #endregion
                                            }
                                        }

                                    }
                                    else
                                    {
                                        #region 没有需要关联运单的订单

                                        runTaskResult.Success = true;
                                        runTaskResult.Result = "<--" + tasksCommon.DealFullName + "[" +
                                                                shiplist.ShippingName +
                                                                "]当前没有需要匹配电子运单的订单-->";
                                        ShowRunningLog("<--" + tasksCommon.DealFullName + "[" +
                                                        shiplist.ShippingName + "]当前没有需要匹配电子运单的订单-->");
                                        WriteLog("<--" + tasksCommon.DealFullName + "[" + shiplist.ShippingName +
                                                    "]当前没有需要匹配电子运单的订单-->");

                                        #endregion
                                    }

                                }

                                #endregion
                            }

                            #endregion
                        }
                        if (tasksCommon.NormalOrderSwitch == "开")
                        {
                            #region 快递公司电子运单

                            var normalShippingList =
                                shippingList.Where(t => t.InvoiceType == 1 && t.IsSpare == 1).ToList();
                            if (normalShippingList.Count == 0)
                            {
                                #region 没有需要关联运单的订单

                                runTaskResult.Success = true;
                                runTaskResult.Result = "<--" + tasksCommon.DealFullName + "【正常快递】中当前没有需要匹配电子运单的订单-->";
                                ShowRunningLog("<--" + tasksCommon.DealFullName + "【正常快递】中当前没有需要匹配电子运单的订单-->");
                                WriteLog("<--" + tasksCommon.DealFullName + "【正常快递】中当前没有需要匹配电子运单的订单-->");

                                #endregion
                            }
                            else
                            {
                                #region 匹配对应快递的运单

                                foreach (var shiplist in normalShippingList)
                                {
                                    #region 获取未关联电子运单的订单

                                    erpOrder.shipping_id = shiplist.ShippingId;
                                    erpOrder.ware_id = tasksCommon.WareId;
                                    erpOrder.MaxQty = tasksCommon.MaxQty;
                                    erpOrder.InvoiceType = shiplist.InvoiceType;
                                    erpOrder.SieveOrderType = shiplist.SieveOrderType;
                                    ShowRunningLog("<--" + tasksCommon.DealFullName + "[" + shiplist.ShippingName +
                                                    "]订单未匹配电子运单的订单获取中-->");
                                    WriteLog("<--" + tasksCommon.DealFullName + "[" + shiplist.ShippingName +
                                                "]订单未匹配电子运单的订单获取中-->");
                                    orderlist =
                                        mOrderService.GetUnmatchOrderListByThreadBLL(tasksCommon.ConnStrWrite,
                                                                                        erpOrder);

                                    #endregion

                                    if (orderlist!=null && orderlist.Count > 0)
                                    {
                                        if (!string.IsNullOrEmpty(orderlist[0].ExceptionStr))
                                        {
                                            #region 获取订单列表失败,发邮件通知处理

                                            runTaskResult.Success = false;
                                            runTaskResult.Result = TaskName() + "<--" + tasksCommon.DealFullName +
                                                                    "【正常快递】中订单匹配电子运单获取未匹配运单的订单时失败：" +
                                                                    orderlist[0].ExceptionStr + "-->";
                                            ShowRunningLog(TaskName() + "<--" + tasksCommon.DealFullName +
                                                            "【正常快递】中订单匹配电子运单获取未匹配运单的订单时失败：" +
                                                            orderlist[0].ExceptionStr + "-->");
                                            WriteLog(TaskName() + "<--" + tasksCommon.DealFullName +
                                                        "【正常快递】中订单匹配电子运单获取未匹配运单的订单时失败：" + orderlist[0].ExceptionStr +
                                                        "-->");
                                            SendEmailTo(
                                                TaskName() + "<--" + tasksCommon.DealFullName +
                                                "【正常快递】中订单匹配电子运单获取未匹配运单的订单时失败：" + orderlist[0].ExceptionStr + "-->",
                                                CustomConfig["EmailAddress"]);

                                            #endregion
                                        }
                                        else
                                        {
                                            #region 获取未使用的电子运单

                                            WmsInvoiceQuery wmsInvoiceQueryDto = new WmsInvoiceQuery();
                                            wmsInvoiceQueryDto.ShippingId = erpOrder.shipping_id;
                                            wmsInvoiceQueryDto.WareHouseId = erpOrder.ware_id;
                                            wmsInvoiceQueryDto.MaxNum = orderlist.Count;
                                            ShowRunningLog(tasksCommon.DealFullName + "[" + shiplist.ShippingName +
                                                            "]订单未匹配电子运单的订单获取完成,共计:{" + orderlist.Count + "}单");
                                            ShowRunningLog(tasksCommon.DealFullName + "[" + shiplist.ShippingName +
                                                            "]运单池中未使用的运单获取中……");
                                            WriteLog(tasksCommon.DealFullName + "[" + shiplist.ShippingName +
                                                        "]运单池中未使用的运单获取中……");
                                            invoicelist =
                                                mOrderService.GetInvoicesListByThreadBLL(wmsInvoiceQueryDto,
                                                                                            tasksCommon.ConnStrRead);
                                            ShowRunningLog(tasksCommon.DealFullName + "[" + shiplist.ShippingName +
                                                            "]运单池中未使用的运单获取完成,共计:{" + invoicelist.Count + "}单");

                                            #endregion

                                            if (invoicelist.Count > 0)
                                            {
                                                #region 运单不足,订单重新获取

                                                if (orderlist.Count > invoicelist.Count)
                                                {
                                                    ShowRunningLog(tasksCommon.DealFullName + "[" +
                                                                    shiplist.ShippingName +
                                                                    "]运单池中未使用的运单已不足,订单重新获取中……");
                                                    WriteLog(tasksCommon.DealFullName + "[" + shiplist.ShippingName +
                                                                "]运单池中未使用的运单已不足,订单重新获取中……");
                                                    erpOrder.MaxQty = invoicelist.Count;
                                                    orderlist =
                                                        mOrderService.GetUnmatchOrderListByThreadBLL(
                                                            tasksCommon.ConnStrWrite, erpOrder);
                                                }

                                                #endregion

                                                #region 订单匹配运单

                                                ShowRunningLog(tasksCommon.DealFullName + "[" +
                                                                shiplist.ShippingName + "]订单匹配电子运单进行中……");
                                                var orderinvoicelist =
                                                    mOrderService.UpdateOrderInvoicesInfoByThreadBLL(orderlist,
                                                                                                        invoicelist,
                                                                                                        tasksCommon
                                                                                                            .ConnStrWrite);

                                                #endregion

                                                if (orderinvoicelist[0].ResultFlag)
                                                {
                                                    #region 匹配成功,输出结果

                                                    runTaskResult.Success = true;
                                                    runTaskResult.Result = tasksCommon.DealFullName + "[" +
                                                                            shiplist.ShippingName + "]订单匹配电子运单已完成";
                                                    ShowRunningLog("<--" + tasksCommon.DealFullName + "[" +
                                                                    shiplist.ShippingName + "]订单本次匹配电子运单:{" +
                                                                    invoicelist.Count + "}单,成功:{" +
                                                                    orderinvoicelist[0].successQty + "}单-->");
                                                    WriteLog("<--" + tasksCommon.DealFullName + "[" +
                                                                shiplist.ShippingName + "]订单本次匹配电子运单:{" +
                                                                invoicelist.Count + "}单,成功:{" +
                                                                orderinvoicelist[0].successQty + "}单-->");

                                                    #endregion

                                                    #region 匹配有失败,发邮件通知处理

                                                    if (invoicelist.Count > orderinvoicelist[0].successQty)
                                                    {
                                                        SendEmailTo(
                                                            TaskName() + "<--" + tasksCommon.DealFullName + "[" +
                                                            shiplist.ShippingName + "]订单匹配电子运单时有失败订单,共匹配:{" +
                                                            invoicelist.Count + "}单,仅成功:{" +
                                                            orderinvoicelist[0].successQty + "}单-->",
                                                            CustomConfig["EmailAddress"]);
                                                    }

                                                    #endregion
                                                }
                                                else
                                                {
                                                    #region 匹配全部失败,回滚数据,发邮件通知处理

                                                    runTaskResult.Success = false;
                                                    runTaskResult.Result = tasksCommon.DealFullName + "[" +
                                                                            shiplist.ShippingName +
                                                                            "]订单匹配电子运单失败,数据正在回滚";
                                                    if (!string.IsNullOrEmpty(orderinvoicelist[0].Exception))
                                                    {
                                                        ShowRunningLog(tasksCommon.DealFullName + "[" +
                                                                        shiplist.ShippingName +
                                                                        "]订单匹配电子运单失败,数据正在回滚,原因:" +
                                                                        orderinvoicelist[0].Exception);
                                                        WriteLog(tasksCommon.DealFullName + "[" +
                                                                    shiplist.ShippingName + "]订单匹配电子运单失败,数据正在回滚,原因:" +
                                                                    orderinvoicelist[0].Exception);
                                                        SendEmailTo(
                                                            TaskName() + tasksCommon.DealFullName + "[" +
                                                            shiplist.ShippingName + "]订单匹配电子运单失败,数据正在回滚,原因:" +
                                                            orderinvoicelist[0].Exception,
                                                            CustomConfig["EmailAddress"]);
                                                    }
                                                    else
                                                    {
                                                        ShowRunningLog(tasksCommon.DealFullName + "[" +
                                                                        shiplist.ShippingName +
                                                                        "]订单匹配电子运单失败,数据正在回滚……");
                                                        WriteLog(tasksCommon.DealFullName + "[" +
                                                                    shiplist.ShippingName + "]订单匹配电子运单失败,数据正在回滚,原因:" +
                                                                    orderinvoicelist[0].Exception);
                                                        SendEmailTo(
                                                            TaskName() + tasksCommon.DealFullName + "[" +
                                                            shiplist.ShippingName + "]订单匹配电子运单失败,数据正在回滚……",
                                                            CustomConfig["EmailAddress"]);
                                                    }

                                                    #endregion
                                                }
                                            }
                                            else
                                            {
                                                #region 运单已用完,发邮件通知处理

                                                runTaskResult.Success = false;
                                                runTaskResult.Result = tasksCommon.DealFullName + "[" +
                                                                        shiplist.ShippingName +
                                                                        "]运单池中当前类型运单已用完,请马上补单!!!";
                                                ShowRunningLog("<--" + tasksCommon.DealFullName + "[" +
                                                                shiplist.ShippingName + "]运单池中当前类型运单已用完,请马上补单!!!-->");
                                                WriteLog(tasksCommon.DealFullName + "[" + shiplist.ShippingName +
                                                            "]运单池中当前类型运单已用完,请马上补单!!!!!!");
                                                SendEmailTo(
                                                    TaskName() + "<--" + tasksCommon.DealFullName + "[" +
                                                    shiplist.ShippingName + "]运单池中当前类型运单已用完,请马上补单!!!-->",
                                                    CustomConfig["EmailAddress"]);

                                                #endregion
                                            }
                                        }

                                    }
                                    else
                                    {
                                        #region 没有需要关联运单的订单

                                        runTaskResult.Success = true;
                                        runTaskResult.Result = "<--" + tasksCommon.DealFullName + "[" +
                                                                shiplist.ShippingName +
                                                                "]当前没有需要匹配电子运单的订单-->";
                                        ShowRunningLog("<--" + tasksCommon.DealFullName + "[" +
                                                        shiplist.ShippingName + "]当前没有需要匹配电子运单的订单-->");
                                        WriteLog("<--" + tasksCommon.DealFullName + "[" + shiplist.ShippingName +
                                                    "]当前没有需要匹配电子运单的订单-->");

                                        #endregion
                                    }
                                }

                                #endregion
                            }

                            #endregion
                        }
                        if (CustomConfig["特殊运单"] == "开")
                        {
                            #region 特殊运单（此步只需改操作状态）

                            var specialShippingList = shippingList.Where(t => t.IsSpare == 2).ToList();
                            if (specialShippingList.Count == 0)
                            {
                                #region 没有需要关联运单的订单

                                runTaskResult.Success = true;
                                runTaskResult.Result = "<--" + tasksCommon.DealFullName + "【特殊运单】中当前没有需要更改操作状态的订单-->";
                                ShowRunningLog("<--" + tasksCommon.DealFullName + "【特殊运单】中当前没有需要更改操作状态的订单-->");
                                WriteLog("<--" + tasksCommon.DealFullName + "【特殊运单】中当前没有需要更改操作状态的订单-->");

                                #endregion
                            }
                            else
                            {
                                #region 修改对应订单的操作状态

                                foreach (var shiplist in specialShippingList)
                                {
                                    #region 获取未修改操作状态的订单

                                    erpOrder.shipping_id = shiplist.ShippingId;
                                    erpOrder.ware_id = tasksCommon.WareId;
                                    erpOrder.MaxQty = tasksCommon.MaxQty;
                                    erpOrder.InvoiceType = shiplist.InvoiceType;
                                    erpOrder.SieveOrderType = shiplist.SieveOrderType;
                                    ShowRunningLog("<--" + tasksCommon.DealFullName + "[" + shiplist.ShippingName +
                                                    "]订单未改操作状态的订单获取中-->");
                                    orderlist =
                                        mOrderService.GetUnmatchOrderListByThreadBLL(tasksCommon.ConnStrWrite,
                                                                                        erpOrder);

                                    #endregion

                                    if (orderlist != null && orderlist.Count > 0)
                                    {
                                        if (!string.IsNullOrEmpty(orderlist[0].ExceptionStr))
                                        {
                                            #region 获取订单列表失败,发邮件通知处理

                                            runTaskResult.Success = false;
                                            runTaskResult.Result = TaskName() + "<--" + tasksCommon.DealFullName +
                                                                    "【特殊运单】中订单改操作状态获取订单列表时失败：" +
                                                                    orderlist[0].ExceptionStr + "-->";
                                            ShowRunningLog(TaskName() + "<--" + tasksCommon.DealFullName +
                                                            "【特殊运单】中订单改操作状态获取订单列表时失败：" + orderlist[0].ExceptionStr +
                                                            "-->");
                                            WriteLog(TaskName() + "<--" + tasksCommon.DealFullName +
                                                        "【特殊运单】中订单改操作状态获取订单列表时失败：" + orderlist[0].ExceptionStr + "-->");
                                            SendEmailTo(
                                                TaskName() + "<--" + tasksCommon.DealFullName +
                                                "【特殊运单】中订单改操作状态获取订单列表时失败：" + orderlist[0].ExceptionStr + "-->",
                                                CustomConfig["EmailAddress"]);

                                            #endregion
                                        }
                                        else
                                        {
                                            ShowRunningLog(tasksCommon.DealFullName + "[" + shiplist.ShippingName +
                                                            "]订单未改操作状态的订单获取完成,共计:{" + orderlist.Count + "}单");
                                            WriteLog(tasksCommon.DealFullName + "[" + shiplist.ShippingName +
                                                        "]订单未改操作状态的订单获取完成,共计:{" + orderlist.Count + "}单");

                                            #region 订单修改操作状态

                                            ShowRunningLog(tasksCommon.DealFullName + "[" + shiplist.ShippingName +
                                                            "]订单修改操作状态进行中……");
                                            var orderinvoicelist =
                                                mOrderService.UpdateSpecialOrderOptStatusBLL(orderlist,
                                                                                                tasksCommon
                                                                                                    .ConnStrWrite);

                                            #endregion

                                            if (orderinvoicelist[0].ResultFlag)
                                            {
                                                #region 订单操作状态修改成功,输出结果

                                                runTaskResult.Success = true;
                                                runTaskResult.Result = tasksCommon.DealFullName + "[" +
                                                                        shiplist.ShippingName + "]订单修改操作状态已完成";
                                                ShowRunningLog("<--" + tasksCommon.DealFullName + "[" +
                                                                shiplist.ShippingName + "]订单本次修改操作状态:{" +
                                                                orderlist.Count + "}单,成功:{" +
                                                                orderinvoicelist[0].successQty + "}单-->");
                                                WriteLog("<--" + tasksCommon.DealFullName + "[" +
                                                            shiplist.ShippingName + "]订单本次修改操作状态:{" + orderlist.Count +
                                                            "}单,成功:{" + orderinvoicelist[0].successQty + "}单-->");

                                                #endregion

                                                #region 订单操作状态修改有失败,发邮件通知处理

                                                if (orderlist.Count > orderinvoicelist[0].successQty)
                                                {
                                                    SendEmailTo(
                                                        TaskName() + "<--" + tasksCommon.DealFullName + "[" +
                                                        shiplist.ShippingName + "]订单修改操作状态时有失败订单,共修改:{" +
                                                        orderlist.Count + "}单,仅成功:{" +
                                                        orderinvoicelist[0].successQty + "}单-->",
                                                        CustomConfig["EmailAddress"]);
                                                }

                                                #endregion
                                            }
                                            else
                                            {
                                                #region 订单操作状态修改全部失败,回滚数据,发邮件通知处理

                                                runTaskResult.Success = false;
                                                runTaskResult.Result = tasksCommon.DealFullName + "[" +
                                                                        shiplist.ShippingName + "]订单修改操作状态败,数据正在回滚";
                                                if (!string.IsNullOrEmpty(orderinvoicelist[0].Exception))
                                                {
                                                    ShowRunningLog(tasksCommon.DealFullName + "[" +
                                                                    shiplist.ShippingName + "]订单修改操作状态失败,数据正在回滚,原因:" +
                                                                    orderinvoicelist[0].Exception);
                                                    WriteLog(tasksCommon.DealFullName + "[" + shiplist.ShippingName +
                                                                "]订单修改操作状态失败,数据正在回滚,原因:" +
                                                                orderinvoicelist[0].Exception);
                                                    SendEmailTo(
                                                        TaskName() + tasksCommon.DealFullName + "[" +
                                                        shiplist.ShippingName + "]订单修改操作状态失败,数据正在回滚,原因:" +
                                                        orderinvoicelist[0].Exception, CustomConfig["EmailAddress"]);
                                                }
                                                else
                                                {
                                                    ShowRunningLog(tasksCommon.DealFullName + "[" +
                                                                    shiplist.ShippingName + "]订单修改操作状态失败,数据正在回滚……");
                                                    WriteLog(tasksCommon.DealFullName + "[" + shiplist.ShippingName +
                                                                "]订单修改操作状态失败,数据正在回滚,原因:" +
                                                                orderinvoicelist[0].Exception);
                                                    SendEmailTo(
                                                        TaskName() + tasksCommon.DealFullName + "[" +
                                                        shiplist.ShippingName + "]订单修改操作状态失败,数据正在回滚……",
                                                        CustomConfig["EmailAddress"]);
                                                }

                                                #endregion
                                            }
                                        }

                                    }
                                    else
                                    {
                                        #region 没有需要修改操作状态的订单

                                        runTaskResult.Success = true;
                                        runTaskResult.Result = "<--" + tasksCommon.DealFullName + "[" +
                                                                shiplist.ShippingName +
                                                                "]当前没有需要修改操作状态的订单-->";
                                        ShowRunningLog("<--" + tasksCommon.DealFullName + "[" +
                                                        shiplist.ShippingName + "]当前没有需要修改操作状态的订单-->");
                                        WriteLog("<--" + tasksCommon.DealFullName + "[" + shiplist.ShippingName +
                                                    "]当前没有需要修改操作状态的订单-->");

                                        #endregion
                                    }

                                }

                                #endregion
                            }

                            #endregion
                        }
                        #endregion
                    }
                }
                else
                {
                    #region 在本仓中未获取到快递列表

                    runTaskResult.Success = true;
                    runTaskResult.Result = TaskName() + "<--" + tasksCommon.DealFullName +
                                           "在本仓中未获取到快递列表-->";
                    ShowRunningLog(TaskName() + "<--" + tasksCommon.DealFullName + "在本仓中未获取到快递列表-->");
                    WriteLog(TaskName() + "<--" + tasksCommon.DealFullName + "在本仓中未获取到快递列表-->");

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
