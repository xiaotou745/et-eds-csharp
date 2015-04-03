using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Task.Model.Order;
using TaskPlatform.TaskInterface;
using System.Data;
using Task.Service.Order;
using Task.Service.Impl.Order;
using System.Threading;
namespace InvoiceMatchOrder
{
    public class InvoiceMatchOrderTask : AbstractTask
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
            return "运单自动匹配订单";
        }
        /// <summary>
        /// 服务内容描述
        /// </summary>
        /// <returns></returns>
        public override string TaskDescription()
        {
            return "在打包前从运单池中找出运单与订单进行匹配绑定";
        }
        public override RunTaskResult RunTask()
        {
            RunTaskResult runTaskResult = new RunTaskResult();
            string strcon = CustomConfig["数据库连接字符串"];
            try
            {
                //ThreadPool.QueueUserWorkItem(ThreadPoolCallback);
                ShowRunningLog("运单匹配订单自动处理服务开始执行……");
                IList<erp_order> orderlist = new List<erp_order>();
                IList<erp_order> codorderlist = new List<erp_order>();
                IList<wms_invoiceInfo> invoicelist = new List<wms_invoiceInfo>();
                ShowRunningLog("获取未关联运单的订单……");
                orderlist = mOrderService.GetUnmatchOrderListBLL(strcon);
                if(orderlist.Count>0)
                {
                    int PreWareId = 0;
                    if (orderlist[0].pre_wareid == 0)
                    {
                        PreWareId = orderlist[0].ware_id;
                    }
                    else
                    {
                        PreWareId = orderlist[0].pre_wareid;
                    }
                    #region 快递公司电子运单
                    if (orderlist[0].InvoiceType == 1)
                    {
                        WmsInvoiceQuery wmsInvoiceQueryDto = new WmsInvoiceQuery();
                        wmsInvoiceQueryDto.ShippingId =orderlist[0].shipping_id;
                        wmsInvoiceQueryDto.WareHouseId = PreWareId;
                        wmsInvoiceQueryDto.MaxNum = 1;
                        ShowRunningLog("从运单池中获取未使用的运单……");
                        #region 订单关联运单
                        invoicelist = mOrderService.GetInvoicesListBLL(wmsInvoiceQueryDto,strcon);
                        if (invoicelist.Count > 0)
                        {
                            #region 修改订单表和运单表
                            wms_invoiceInfo invoicesinfo = new wms_invoiceInfo();
                            invoicesinfo.OrderId = orderlist[0].order_id;
                            invoicesinfo.order_sn = orderlist[0].order_sn;
                            invoicesinfo.InvoiceNo = invoicelist[0].InvoiceNo;
                            invoicesinfo.Id = invoicelist[0].Id;
                            invoicesinfo.UpdateBy = "服务平台";
                            invoicesinfo.WareHouseId = orderlist[0].ware_id;
                            invoicesinfo.ShippingId = orderlist[0].shipping_id;
                            invoicesinfo.pre_wareid = PreWareId;
                            invoicesinfo.IsUsed = 1; //表示该运单已经使用
                            var orderinvoicelist = mOrderService.UpdateOrderInvoicesInfoBLL(invoicesinfo, strcon);
                            if(orderinvoicelist[0].ResultFlag)
                            {
                                runTaskResult.Success = true;
                                runTaskResult.Result = "订单匹配运单已完成...";
                                ShowRunningLog("订单:" + orderinvoicelist[0].order_sn + "已与运单:"+orderinvoicelist[0].InvoiceNo+"匹配成功……");
                            }
                            else
                            {
                                runTaskResult.Success = false;
                                runTaskResult.Result = "订单匹配运单失败,数据正在回滚";
                                if (!string.IsNullOrEmpty(orderinvoicelist[0].Exception))
                                {
                                    ShowRunningLog("订单匹配运单失败,数据正在回滚,原因:"+orderinvoicelist[0].Exception);
                                }
                                else
                                {
                                    ShowRunningLog("订单匹配运单失败,数据正在回滚……");
                                }
                            }
#endregion
                        }
                        else
                        {
                            runTaskResult.Success = false;
                            runTaskResult.Result = "运单池中已没有当前类型未使用的运单...";
                            ShowRunningLog("运单池中已没有当前类型未使用的运单……");
                        }
                        #endregion 
                    }
                    #endregion
                    #region COD电子运单
                    else if (orderlist[0].InvoiceType == 2)
                    {
                        erp_order orderInfo = new erp_order(); 
                        orderInfo.order_id = orderlist[0].order_id;
                        orderInfo.ware_id = orderlist[0].ware_id;
                        orderInfo.invoice_no = "JX" + orderlist[0].order_id;
                        codorderlist=mOrderService.UpdateOrderInfoByCODBLL(orderInfo, strcon);
                        ShowRunningLog("当期没有需要关联运单的订单……");
                        if (codorderlist[0].ResultFlag)
                        {
                            runTaskResult.Success = true;
                            runTaskResult.Result = "订单匹配运单已完成...";
                            ShowRunningLog("订单:" + orderlist[0].order_sn + "已与运单:JX" + orderlist[0].order_id + "匹配成功……");
                        }
                        else
                        {
                            runTaskResult.Success = false;
                            runTaskResult.Result = "订单匹配运单失败,数据正在回滚";
                            if (!string.IsNullOrEmpty(codorderlist[0].ExceptionStr))
                            {
                                ShowRunningLog("订单匹配运单失败,数据正在回滚,原因:" + codorderlist[0].ExceptionStr);
                            }
                            else
                            {
                                ShowRunningLog("订单匹配运单失败,数据正在回滚……");
                            }
                        }
                    }
                    #endregion
                }
                else
                {
                    runTaskResult.Success = true;
                    runTaskResult.Result = "当期没有需要关联运单的订单……";
                    ShowRunningLog("当期没有需要关联运单的订单……");
                }
            }
            catch (Exception ex)
            {
                this.SendEmailTo(this.TaskName() + ex, CustomConfig["EmailAddress"]);
                runTaskResult.Success = false;
                runTaskResult.Result = ex.ToString();
                ShowRunningLog(ex.ToString());
                WriteLog(ex.ToString());
            }
            return runTaskResult;
        }
        public override Dictionary<string, string> UploadConfig()
        {
            if (!CustomConfig.ContainsKey("数据库连接字符串"))
                CustomConfig.Add("数据库连接字符串", "Server=192.168.12.30;Database=jiuxianweb;User ID=test_db_online;password=QwerAs131126dfTyui1209;Pooling=false;");
            if (!CustomConfig.ContainsKey("EmailAddress"))
                CustomConfig.Add("EmailAddress", "wangxudan@jiuxian.com");
            return CustomConfig;
        }
        //private void ThreadPoolCallback(object threadContext)
        //{
        //    RunTaskResult runTaskResult = new RunTaskResult();
        //    SQLServerDBHelper sqlServerDbHelper = new SQLServerDBHelper();
        //    FMSCommon fmsCommon = (FMSCommon)threadContext;
        //    ShowRunningLog(fmsCommon.DealFullName + "开始执行……");
        //    WriteLog(fmsCommon.DealFullName + "开始执行……");
        //    string addTime = "";
        //    try
        //    {
        //        sqlServerDbHelper.ConnectionString = fmsCommon.ConnStr;
        //        TransactionOptions option = new TransactionOptions();
        //        option.Timeout = new TimeSpan(0, 10, 0);
        //        option.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
        //        List<SqlParameter> listSqlParameter = new List<SqlParameter>();
        //        listSqlParameter.Add(new SqlParameter("@startTime", fmsCommon.StartTime));
        //        listSqlParameter.Add(new SqlParameter("@endTime", fmsCommon.EndTime));
        //        using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, option))
        //        {
        //            DataTable dtResult = sqlServerDbHelper.RunProcedureDataSet(fmsCommon.ProName, listSqlParameter.ToArray()).Tables[0];
        //            int result;
        //            int.TryParse(dtResult.Rows[0][0].ToString(), out result);
        //            addTime = dtResult.Rows[0][1].ToString();
        //            // 等于-1表示结束时间大于当前时间
        //            if (result != -1)
        //            {
        //                sqlServerDbHelper.ExecuteSql(
        //                    @"INSERT INTO multicompany.WMSInfo.SummaryDateLog (WmsDocType, AddedOn,IsException,Remark) VALUES (" +
        //                    fmsCommon.DocId + ",'" + addTime + "',0,'" + result.ToString() + "')");
        //                runTaskResult.Success = true;
        //                runTaskResult.Result = fmsCommon.DealFullName + addTime + "：本次共计汇总【" + result + "】条！";
        //                ts.Complete();
        //            }
        //            else
        //            {
        //                runTaskResult.Success = true;
        //                runTaskResult.Result = fmsCommon.DealFullName + "本时间段汇总已经结束";
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        sqlServerDbHelper.ExecuteSql(@"INSERT INTO Multicompany.WMSInfo.SummaryDateLog (WmsDocType, AddedOn,IsException,Remark) VALUES (" + fmsCommon.DocId + ", '" + addTime + "',1,'" + ex.Message + "')");
        //        runTaskResult.Success = false;
        //        runTaskResult.Result = ex.ToString();
        //        this.SendEmailTo(this.TaskName() + ex, CustomConfig["EmailAddress"]);
        //    }
        //    fmsCommon.DoneEvent.Set();
        //    ShowRunningLog(runTaskResult.Result);
        //    WriteLog(runTaskResult.Result);
        //}
    }
}
