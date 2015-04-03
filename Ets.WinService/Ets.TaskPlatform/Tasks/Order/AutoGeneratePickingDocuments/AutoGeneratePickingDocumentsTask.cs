using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Task.Model;
using Task.Model.Order;
using Task.Service.Impl.Order;
using Task.Service.Order;
using TaskPlatform.TaskInterface;

namespace AutoGeneratePickingDocuments
{
    public class AutoGeneratePickingDocumentsTask : AbstractTask
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
            return "自动生成拣货批次";
        }
        /// <summary>
        /// 服务内容描述
        /// </summary>
        /// <returns></returns>
        public override string TaskDescription()
        {
            return "多线程分仓库自动生成拣货批次（优化拣货路径）";
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
                    TasksCommon tasksCommon = new TasksCommon(wareId, strconRead, strconWrite, cc.Key, doneEvents[i]);
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
            ShowRunningLog("=============本次生成拣货批次全部完成=============");
            runTaskResult.Result = "本次生成拣货批次全部完成";
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
            if (!CustomConfig.ContainsKey("EmailAddress"))
                CustomConfig.Add("EmailAddress", "wangxudan@jiuxian.com");
            if (!CustomConfig.ContainsKey("满载量上限"))
                CustomConfig.Add("满载量上限", "40");
            if (!CustomConfig.ContainsKey("满载量下限"))
                CustomConfig.Add("满载量下限", "30");
            if (!CustomConfig.ContainsKey("促销套装数下限"))
                CustomConfig.Add("促销套装数下限", "10");
            if (!CustomConfig.ContainsKey("促销套装订单数上限"))
                CustomConfig.Add("促销套装订单数上限", "20");
            if (!CustomConfig.ContainsKey("酒仙自送批次"))
                CustomConfig.Add("酒仙自送批次", "关");
            if (!CustomConfig.ContainsKey("EMS批次"))
                CustomConfig.Add("EMS批次", "关");
            if (!CustomConfig.ContainsKey("促销套装批次"))
                CustomConfig.Add("促销套装批次", "开");
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
                WriteConnectionString = tasksCommon.ConnStrWrite,
                WareId = tasksCommon.WareId,
                MinQty =Convert.ToInt32(CustomConfig["满载量下限"]),
                MaxQty = Convert.ToInt32(CustomConfig["满载量上限"]),
                MinSuitQty = Convert.ToInt32(CustomConfig["促销套装数下限"]),
                MaxOrderQty = Convert.ToInt32(CustomConfig["促销套装订单数上限"]),
                
            };
            WmsReceiptTaskDetailDto wmsReceiptTaskDetailDto = new WmsReceiptTaskDetailDto
            {
                CreateTime = DateTime.Now.ToLocalTime(),
                CreateType = 1,
                WmsOrderType=1,
            };
            #endregion

            try
            {
                #region 1-4:促销套装订单拣货批次生成

                if (CustomConfig["促销套装批次"] == "开")
                {
                    #region 1：促销套装需发票订单拣货批次生成
                    Log("<--" + tasksCommon.DealFullName + "中促销套装需发票订单获取中……-->");
                    wmsReceiptTaskDetailDto.IsInvoice = 1;
                    wmsReceiptTaskDetailDto.IsAllocate = 0;
                    var suitInvOrderList = mOrderService.GetUnGeneratePickingBatchInfoOfSuitBLL(config, wmsReceiptTaskDetailDto);
                    if (suitInvOrderList != null && suitInvOrderList.Count > 0 && string.IsNullOrEmpty(suitInvOrderList[0].ExceptionStr))
                    {
                        Log("<--" + tasksCommon.DealFullName + "中促销套装需发票订单获取完成，共计【" + suitInvOrderList.Count + "】种套装，拣货批次生成中……-->");
                        string batchNoList = "";
                        int successQty = 0;
                        foreach (var item in suitInvOrderList)
                        {
                            wmsReceiptTaskDetailDto.GoodsId = item.GoodsId;
                            wmsReceiptTaskDetailDto.ShippingId = item.ShippingId;
                            wmsReceiptTaskDetailDto.BatchType = 7;
                            wmsReceiptTaskDetailDto.OrTypePrivateCus = 2;
                            wmsReceiptTaskDetailDto.OrTypeProduct = 0;
                            wmsReceiptTaskDetailDto.OrTypeStocks = 1;
                            wmsReceiptTaskDetailDto.OrTypeInvoices = 1;
                            wmsReceiptTaskDetailDto.OrderBatchDetailType = 1;
                            wmsReceiptTaskDetailDto.PreWareId = 0;
                            var dealResult = mOrderService.GeneratePickingBatchOfSuitOrderBLL(config, wmsReceiptTaskDetailDto);
                            if (dealResult.DealFlag)
                            {
                                batchNoList += dealResult.SuccessId + ',';
                                successQty++;
                            }
                            else
                            {
                                Log(dealResult.DealMsg);
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(batchNoList))
                        {
                            batchNoList = batchNoList.Remove(batchNoList.Length - 1, 1);
                            Log("<--" + tasksCommon.DealFullName + "中同货位单品需发票订单生成拣货批次完成，共计【" + successQty + "】个批次，批次号集合为【" + batchNoList + "】-->");
                        }
                    }
                    else
                    {
                        if (suitInvOrderList.Count > 0 && !string.IsNullOrEmpty(suitInvOrderList[0].ExceptionStr))
                        {
                            SendEmailTo(tasksCommon.DealFullName + "中促销套装需发票订单获取中遇到异常，异常信息为：" + suitInvOrderList[0].ExceptionStr, CustomConfig["EmailAddress"]);
                        }
                        else
                        {
                            Log("<--" + tasksCommon.DealFullName + "中没有需要生成拣货批次的促销套装需发票订单-->");
                        }

                    }

                    #endregion

                    #region 2：促销套装订单拣货批次生成
                    Log("<--" + tasksCommon.DealFullName + "中促销套装订单获取中……-->");
                    wmsReceiptTaskDetailDto.IsInvoice = 0;
                    wmsReceiptTaskDetailDto.IsAllocate = 0;
                    var suitOrderList = mOrderService.GetUnGeneratePickingBatchInfoOfSuitBLL(config, wmsReceiptTaskDetailDto);
                    if (suitOrderList != null && suitOrderList.Count > 0 && string.IsNullOrEmpty(suitOrderList[0].ExceptionStr))
                    {
                        Log("<--" + tasksCommon.DealFullName + "中促销套装订单获取完成，共计【" + suitOrderList.Count + "】种套装，拣货批次生成中……-->");
                        string batchNoList = "";
                        int successQty = 0;
                        foreach (var item in suitOrderList)
                        {
                            wmsReceiptTaskDetailDto.GoodsId = item.GoodsId;
                            wmsReceiptTaskDetailDto.ShippingId = item.ShippingId;
                            wmsReceiptTaskDetailDto.BatchType = 7;
                            wmsReceiptTaskDetailDto.OrTypePrivateCus = 2;
                            wmsReceiptTaskDetailDto.OrTypeProduct = 0;
                            wmsReceiptTaskDetailDto.OrTypeStocks = 1;
                            wmsReceiptTaskDetailDto.OrTypeInvoices = 2;
                            wmsReceiptTaskDetailDto.OrderBatchDetailType = 2;
                            wmsReceiptTaskDetailDto.PreWareId = 0;
                            var dealResult = mOrderService.GeneratePickingBatchOfSuitOrderBLL(config, wmsReceiptTaskDetailDto);
                            if (dealResult.DealFlag)
                            {
                                batchNoList += dealResult.SuccessId + ',';
                                successQty++;
                            }
                            else
                            {
                                Log(dealResult.DealMsg);
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(batchNoList))
                        {
                            batchNoList = batchNoList.Remove(batchNoList.Length - 1, 1);
                            Log("<--" + tasksCommon.DealFullName + "中同货位单品订单生成拣货批次完成，共计【" + successQty + "】个批次，批次号集合为【" + batchNoList + "】-->");
                        }
                    }
                    else
                    {
                        if (suitOrderList.Count > 0 && !string.IsNullOrEmpty(suitOrderList[0].ExceptionStr))
                        {
                            SendEmailTo(tasksCommon.DealFullName + "中促销套装订单获取中遇到异常，异常信息为：" + suitOrderList[0].ExceptionStr, CustomConfig["EmailAddress"]);
                        }
                        else
                        {
                            Log("<--" + tasksCommon.DealFullName + "中没有需要生成拣货批次的促销套装订单-->");
                        }

                    }

                    #endregion

                    #region 3:促销套装调仓订单拣货批次生成

                    if (config.WareId == 5)
                    {
                        #region 3：促销套装需发票调仓订单拣货批次生成
                        Log("<--" + tasksCommon.DealFullName + "中促销套装需发票调仓订单获取中……-->");
                        wmsReceiptTaskDetailDto.IsInvoice = 1;
                        wmsReceiptTaskDetailDto.IsAllocate = 1;
                        var suitInvAllocateOrderList = mOrderService.GetUnGeneratePickingBatchInfoOfSuitBLL(config, wmsReceiptTaskDetailDto);
                        if (suitInvAllocateOrderList != null && suitInvAllocateOrderList.Count > 0 && string.IsNullOrEmpty(suitInvAllocateOrderList[0].ExceptionStr))
                        {
                            Log("<--" + tasksCommon.DealFullName + "中促销套装需发票调仓订单获取完成，共计【" + suitInvAllocateOrderList.Count + "】种套装，拣货批次生成中……-->");
                            string batchNoList = "";
                            int successQty = 0;
                            foreach (var item in suitInvAllocateOrderList)
                            {
                                wmsReceiptTaskDetailDto.GoodsId = item.GoodsId;
                                wmsReceiptTaskDetailDto.ShippingId = item.ShippingId;
                                wmsReceiptTaskDetailDto.BatchType = 7;
                                wmsReceiptTaskDetailDto.OrTypePrivateCus = 2;
                                wmsReceiptTaskDetailDto.OrTypeProduct = 0;
                                wmsReceiptTaskDetailDto.OrTypeStocks = 1;
                                wmsReceiptTaskDetailDto.OrTypeInvoices = 1;
                                wmsReceiptTaskDetailDto.OrderBatchDetailType = 3;
                                wmsReceiptTaskDetailDto.PreWareId = item.PreWareId;
                                var dealResult = mOrderService.GeneratePickingBatchOfSuitOrderBLL(config, wmsReceiptTaskDetailDto);
                                if (dealResult.DealFlag)
                                {
                                    batchNoList += dealResult.SuccessId + ',';
                                    successQty++;
                                }
                                else
                                {
                                    Log(dealResult.DealMsg);
                                }
                            }
                            if (!string.IsNullOrWhiteSpace(batchNoList))
                            {
                                batchNoList = batchNoList.Remove(batchNoList.Length - 1, 1);
                                Log("<--" + tasksCommon.DealFullName + "中促销套装需发票调仓订单生成拣货批次完成，共计【" + successQty + "】个批次，批次号集合为【" + batchNoList + "】-->");
                            }
                        }
                        else
                        {
                            if (suitInvAllocateOrderList.Count > 0 && !string.IsNullOrEmpty(suitInvAllocateOrderList[0].ExceptionStr))
                            {
                                SendEmailTo(tasksCommon.DealFullName + "中促销套装需发票调仓订单获取中遇到异常，异常信息为：" + suitInvAllocateOrderList[0].ExceptionStr, CustomConfig["EmailAddress"]);
                            }
                            else
                            {
                                Log("<--" + tasksCommon.DealFullName + "中没有需要生成拣货批次的促销套装需发票调仓订单-->");
                            }

                        }

                        #endregion

                        #region 4：促销套装调仓订单拣货批次生成
                        Log("<--" + tasksCommon.DealFullName + "中促销套装调仓订单获取中……-->");
                        wmsReceiptTaskDetailDto.IsInvoice = 0;
                        wmsReceiptTaskDetailDto.IsAllocate = 1;
                        var suitAllocateOrderList = mOrderService.GetUnGeneratePickingBatchInfoOfSuitBLL(config, wmsReceiptTaskDetailDto);
                        if (suitAllocateOrderList != null && suitAllocateOrderList.Count > 0 && string.IsNullOrEmpty(suitAllocateOrderList[0].ExceptionStr))
                        {
                            Log("<--" + tasksCommon.DealFullName + "中促销套装调仓订单获取完成，共计【" + suitAllocateOrderList.Count + "】种套装，拣货批次生成中……-->");
                            string batchNoList = "";
                            int successQty = 0;
                            foreach (var item in suitAllocateOrderList)
                            {
                                wmsReceiptTaskDetailDto.GoodsId = item.GoodsId;
                                wmsReceiptTaskDetailDto.ShippingId = item.ShippingId;
                                wmsReceiptTaskDetailDto.BatchType = 7;
                                wmsReceiptTaskDetailDto.OrTypePrivateCus = 2;
                                wmsReceiptTaskDetailDto.OrTypeProduct = 0;
                                wmsReceiptTaskDetailDto.OrTypeStocks = 1;
                                wmsReceiptTaskDetailDto.OrTypeInvoices = 2;
                                wmsReceiptTaskDetailDto.OrderBatchDetailType = 4;
                                wmsReceiptTaskDetailDto.PreWareId = item.PreWareId;
                                var dealResult = mOrderService.GeneratePickingBatchOfSuitOrderBLL(config, wmsReceiptTaskDetailDto);
                                if (dealResult.DealFlag)
                                {
                                    batchNoList += dealResult.SuccessId + ',';
                                    successQty++;
                                }
                                else
                                {
                                    Log(dealResult.DealMsg);
                                }
                            }
                            if (!string.IsNullOrWhiteSpace(batchNoList))
                            {
                                batchNoList = batchNoList.Remove(batchNoList.Length - 1, 1);
                                Log("<--" + tasksCommon.DealFullName + "中促销套装调仓订单生成拣货批次完成，共计【" + successQty + "】个批次，批次号集合为【" + batchNoList + "】-->");
                            }
                        }
                        else
                        {
                            if (suitAllocateOrderList.Count > 0 && !string.IsNullOrEmpty(suitAllocateOrderList[0].ExceptionStr))
                            {
                                SendEmailTo(tasksCommon.DealFullName + "中促销套装调仓订单获取中遇到异常，异常信息为：" + suitAllocateOrderList[0].ExceptionStr, CustomConfig["EmailAddress"]);
                            }
                            else
                            {
                                Log("<--" + tasksCommon.DealFullName + "中没有需要生成拣货批次的促销套装调仓订单-->");
                            }

                        }

                        #endregion
                    }

                    #endregion
                }

                #endregion

                #region 5：私人订制订单拣货批次生成
                Log("<--" + tasksCommon.DealFullName + "中私人订制订单获取中……-->");
                var diyOrderList = mOrderService.GetUnGeneratePickingBatchInfoOfDiyBLL(config, wmsReceiptTaskDetailDto);
                if (diyOrderList != null && diyOrderList.Count > 0 && string.IsNullOrEmpty(diyOrderList[0].ExceptionStr))
                {
                    Log("<--" + tasksCommon.DealFullName + "中私人订制订单获取完成，拣货批次生成中……-->");
                    string batchNoList = "";
                    wmsReceiptTaskDetailDto.WareId = config.WareId;
                    wmsReceiptTaskDetailDto.BatchType = 1;
                    wmsReceiptTaskDetailDto.OrTypePrivateCus = 1;
                    wmsReceiptTaskDetailDto.OrTypeProduct = 0;
                    wmsReceiptTaskDetailDto.OrTypeStocks = 1;
                    wmsReceiptTaskDetailDto.OrTypeInvoices = 0;
                    wmsReceiptTaskDetailDto.OrderBatchDetailType = 5;
                    var dealResult = mOrderService.GeneratePickingBatchOfDiyOrderBLL(config, wmsReceiptTaskDetailDto);
                    if (dealResult.DealFlag)
                    {
                        batchNoList = dealResult.SuccessId;
                        Log("<--" + tasksCommon.DealFullName + "中私人订制订单生成拣货批次完成，批次号为【" + batchNoList + "】-->");
                    }
                    else
                    {
                        Log(dealResult.DealMsg);
                    }
                }
                else
                {
                    if (diyOrderList.Count > 0 && !string.IsNullOrEmpty(diyOrderList[0].ExceptionStr))
                    {
                        SendEmailTo(tasksCommon.DealFullName + "中私人订制订单获取中遇到异常，异常信息为：" + diyOrderList[0].ExceptionStr, CustomConfig["EmailAddress"]);
                    }
                    else
                    {
                        Log("<--" + tasksCommon.DealFullName + "中没有需要生成拣货批次的私人订制订单-->");
                    }

                }

                #endregion

                #region 6：返货区订单拣货批次生成

                Log("<--" + tasksCommon.DealFullName + "中返货区订单获取中……-->");
                var returnOrderList = mOrderService.GetUnGeneratePickingBatchInfoOfReturnBLL(config, wmsReceiptTaskDetailDto);
                if (returnOrderList != null && returnOrderList.Count > 0 && string.IsNullOrEmpty(returnOrderList[0].ExceptionStr))
                {
                    Log("<--" + tasksCommon.DealFullName + "中返货区订单获取完成，拣货批次生成中……-->");
                    wmsReceiptTaskDetailDto.WareId = config.WareId;
                    wmsReceiptTaskDetailDto.BatchType = 6;
                    wmsReceiptTaskDetailDto.OrTypePrivateCus = 2;
                    wmsReceiptTaskDetailDto.OrTypeProduct = 0;
                    wmsReceiptTaskDetailDto.OrTypeStocks = 2;
                    wmsReceiptTaskDetailDto.OrTypeInvoices = 0;
                    wmsReceiptTaskDetailDto.OrderBatchDetailType = 6;
                    var dealResult = mOrderService.GeneratePickingBatchOfReturnOrderBLL(config, wmsReceiptTaskDetailDto);
                    if (dealResult.DealFlag)
                    {
                        Log("<--" + tasksCommon.DealFullName + "中返货区订单生成拣货批次完成，批次号为【" + dealResult.SuccessId + "】-->");
                    }
                    else
                    {
                        Log(dealResult.DealMsg);
                    }
                }
                else
                {
                    if (returnOrderList.Count > 0 && !string.IsNullOrEmpty(returnOrderList[0].ExceptionStr))
                    {
                        SendEmailTo(tasksCommon.DealFullName + "中返货区订单获取中遇到异常，异常信息为：" + returnOrderList[0].ExceptionStr, CustomConfig["EmailAddress"]);
                    }
                    else
                    {
                        Log("<--" + tasksCommon.DealFullName + "中没有需要生成拣货批次的返货区订单-->");
                    }

                }

                #endregion

                #region 7-10：特殊快递暂时停用

                if (CustomConfig["酒仙自送批次"] == "开")
                {
                    #region 7：特殊快递（酒仙自送）需发票订单拣货批次生成

                    Log("<--" + tasksCommon.DealFullName + "中特殊快递（酒仙自送）需发票订单获取中……-->");
                    wmsReceiptTaskDetailDto.WmsOrderType = 0;
                    wmsReceiptTaskDetailDto.ShippingId = 12;
                    wmsReceiptTaskDetailDto.IsInvoice = 1;
                    var jxInvOrderList = mOrderService.GetUnGeneratePickingBatchInfoOfSpecialExpressBLL(config, wmsReceiptTaskDetailDto);
                    if (jxInvOrderList != null && jxInvOrderList.Count > 0 && string.IsNullOrEmpty(jxInvOrderList[0].ExceptionStr))
                    {
                        Log("<--" + tasksCommon.DealFullName + "中特殊快递（酒仙自送）需发票订单获取完成，共计【" + jxInvOrderList.Count + "】个订单，拣货批次生成中……-->");
                        wmsReceiptTaskDetailDto.WareId = config.WareId;
                        wmsReceiptTaskDetailDto.BatchType = 3;
                        wmsReceiptTaskDetailDto.OrTypePrivateCus = 2;
                        wmsReceiptTaskDetailDto.OrTypeProduct = 0;
                        wmsReceiptTaskDetailDto.OrTypeStocks = 1;
                        wmsReceiptTaskDetailDto.OrTypeInvoices = 1;
                        wmsReceiptTaskDetailDto.OrderBatchDetailType = 7;
                        var dealResult = mOrderService.GeneratePickingBatchOfSpecialExpressOrderBLL(config, wmsReceiptTaskDetailDto);
                        if (dealResult.DealFlag)
                        {
                            Log("<--" + tasksCommon.DealFullName + "中特殊快递（酒仙自送）需发票订单生成拣货批次完成，批次号为【" + dealResult.SuccessId + "】-->");
                        }
                        else
                        {
                            Log(dealResult.DealMsg);
                        }
                    }
                    else
                    {
                        if (jxInvOrderList.Count > 0 && !string.IsNullOrEmpty(jxInvOrderList[0].ExceptionStr))
                        {
                            SendEmailTo(tasksCommon.DealFullName + "中特殊快递（酒仙自送）需发票订单获取中遇到异常，异常信息为：" + jxInvOrderList[0].ExceptionStr, CustomConfig["EmailAddress"]);
                        }
                        else
                        {
                            Log("<--" + tasksCommon.DealFullName + "中没有需要生成拣货批次的特殊快递（酒仙自送）需发票订单-->");
                        }

                    }

                    #endregion

                    #region 8：特殊快递（酒仙自送）订单拣货批次生成

                    Log("<--" + tasksCommon.DealFullName + "中特殊快递（酒仙自送）订单获取中……-->");
                    wmsReceiptTaskDetailDto.WmsOrderType = 0;
                    wmsReceiptTaskDetailDto.ShippingId = 12;
                    wmsReceiptTaskDetailDto.IsInvoice = 0;
                    var jxOrderList = mOrderService.GetUnGeneratePickingBatchInfoOfSpecialExpressBLL(config, wmsReceiptTaskDetailDto);
                    if (jxOrderList != null && jxOrderList.Count > 0 && string.IsNullOrEmpty(jxOrderList[0].ExceptionStr))
                    {
                        Log("<--" + tasksCommon.DealFullName + "中特殊快递（酒仙自送）订单获取完成，共计【" + jxOrderList.Count + "】个订单，拣货批次生成中……-->");
                        wmsReceiptTaskDetailDto.WareId = config.WareId;
                        wmsReceiptTaskDetailDto.BatchType = 3;
                        wmsReceiptTaskDetailDto.OrTypePrivateCus = 2;
                        wmsReceiptTaskDetailDto.OrTypeProduct = 0;
                        wmsReceiptTaskDetailDto.OrTypeStocks = 1;
                        wmsReceiptTaskDetailDto.OrTypeInvoices = 2;
                        wmsReceiptTaskDetailDto.OrderBatchDetailType = 8;
                        var dealResult = mOrderService.GeneratePickingBatchOfSpecialExpressOrderBLL(config, wmsReceiptTaskDetailDto);
                        if (dealResult.DealFlag)
                        {
                            Log("<--" + tasksCommon.DealFullName + "中特殊快递（酒仙自送）订单生成拣货批次完成，批次号为【" + dealResult.SuccessId + "】-->");
                        }
                        else
                        {
                            Log(dealResult.DealMsg);
                        }
                    }
                    else
                    {
                        if (jxOrderList.Count > 0 && !string.IsNullOrEmpty(jxOrderList[0].ExceptionStr))
                        {
                            SendEmailTo(tasksCommon.DealFullName + "中特殊快递（酒仙自送）订单获取中遇到异常，异常信息为：" + jxOrderList[0].ExceptionStr, CustomConfig["EmailAddress"]);
                        }
                        else
                        {
                            Log("<--" + tasksCommon.DealFullName + "中没有需要生成拣货批次的特殊快递（酒仙自送）订单-->");
                        }

                    }

                    #endregion
                }

                if (CustomConfig["EMS批次"] == "开")
                {
                    #region 9：特殊快递（EMS）需发票订单拣货批次生成

                    //Log("<--" + tasksCommon.DealFullName + "中特殊快递（EMS）需发票订单获取中……-->");
                    //wmsReceiptTaskDetailDto.WmsOrderType = 0;
                    //wmsReceiptTaskDetailDto.ShippingId = 21;
                    //wmsReceiptTaskDetailDto.IsInvoice = 1;
                    //var emsInvOrderList = mOrderService.GetUnGeneratePickingBatchInfoOfSpecialExpressBLL(config, wmsReceiptTaskDetailDto);
                    //if (emsInvOrderList != null && emsInvOrderList.Count > 0 && string.IsNullOrEmpty(emsInvOrderList[0].ExceptionStr))
                    //{
                    //    Log("<--" + tasksCommon.DealFullName + "中特殊快递（EMS）需发票订单获取完成，共计【" + emsInvOrderList.Count + "】个订单，拣货批次生成中……-->");
                    //    wmsReceiptTaskDetailDto.WareId = config.WareId;
                    //    wmsReceiptTaskDetailDto.BatchType = 4;
                    //    wmsReceiptTaskDetailDto.OrTypePrivateCus = 2;
                    //    wmsReceiptTaskDetailDto.OrTypeProduct = 0;
                    //    wmsReceiptTaskDetailDto.OrTypeStocks = 1;
                    //    wmsReceiptTaskDetailDto.OrTypeInvoices = 1;
                    //    wmsReceiptTaskDetailDto.OrderBatchDetailType = 9;
                    //    var dealResult = mOrderService.GeneratePickingBatchOfSpecialExpressOrderBLL(config, wmsReceiptTaskDetailDto);
                    //    if (dealResult.DealFlag)
                    //    {
                    //        Log("<--" + tasksCommon.DealFullName + "中特殊快递（EMS）需发票订单生成拣货批次完成，批次号为【" + dealResult.SuccessId + "】-->");
                    //    }
                    //    else
                    //    {
                    //        Log(dealResult.DealMsg);
                    //    }
                    //}
                    //else
                    //{
                    //    if (emsInvOrderList.Count > 0 && !string.IsNullOrEmpty(emsInvOrderList[0].ExceptionStr))
                    //    {
                    //        SendEmailTo(tasksCommon.DealFullName + "中特殊快递（EMS）需发票订单获取中遇到异常，异常信息为：" + emsInvOrderList[0].ExceptionStr, CustomConfig["EmailAddress"]);
                    //    }
                    //    else
                    //    {
                    //        Log("<--" + tasksCommon.DealFullName + "中没有需要生成拣货批次的特殊快递（EMS）需发票订单-->");
                    //    }

                    //}

                    #endregion

                    #region 10：特殊快递（EMS）订单拣货批次生成

                //Log("<--" + tasksCommon.DealFullName + "中特殊快递（EMS）订单获取中……-->");
                //wmsReceiptTaskDetailDto.WmsOrderType = 0;
                //wmsReceiptTaskDetailDto.ShippingId = 21;
                //wmsReceiptTaskDetailDto.IsInvoice = 0;
                //var emsOrderList = mOrderService.GetUnGeneratePickingBatchInfoOfSpecialExpressBLL(config, wmsReceiptTaskDetailDto);
                //if (emsOrderList != null && emsOrderList.Count > 0 && string.IsNullOrEmpty(emsOrderList[0].ExceptionStr))
                //{
                //    Log("<--" + tasksCommon.DealFullName + "中特殊快递（EMS）订单获取完成，共计【" + emsOrderList.Count + "】个订单，拣货批次生成中……-->");
                //    wmsReceiptTaskDetailDto.WareId = config.WareId;
                //    wmsReceiptTaskDetailDto.BatchType = 4;
                //    wmsReceiptTaskDetailDto.OrTypePrivateCus = 2;
                //    wmsReceiptTaskDetailDto.OrTypeProduct = 0;
                //    wmsReceiptTaskDetailDto.OrTypeStocks = 1;
                //    wmsReceiptTaskDetailDto.OrTypeInvoices = 2;
                //    wmsReceiptTaskDetailDto.OrderBatchDetailType = 10;
                //    var dealResult = mOrderService.GeneratePickingBatchOfSpecialExpressOrderBLL(config, wmsReceiptTaskDetailDto);
                //    if (dealResult.DealFlag)
                //    {
                //        Log("<--" + tasksCommon.DealFullName + "中特殊快递（EMS）订单生成拣货批次完成，批次号为【" + dealResult.SuccessId + "】-->");
                //    }
                //    else
                //    {
                //        Log(dealResult.DealMsg);
                //    }
                //}
                //else
                //{
                //    if (emsOrderList.Count > 0 && !string.IsNullOrEmpty(emsOrderList[0].ExceptionStr))
                //    {
                //        SendEmailTo(tasksCommon.DealFullName + "中特殊快递（EMS）订单获取中遇到异常，异常信息为：" + emsOrderList[0].ExceptionStr, CustomConfig["EmailAddress"]);
                //    }
                //    else
                //    {
                //        Log("<--" + tasksCommon.DealFullName + "中没有需要生成拣货批次的特殊快递（EMS）订单-->");
                //    }

                //}

                #endregion
                }

                #endregion

                #region 11：同货位单品需发票订单拣货批次生成

                Log("<--" + tasksCommon.DealFullName + "中同货位单品需发票订单获取中……-->");
                wmsReceiptTaskDetailDto.WmsOrderType = 1;
                wmsReceiptTaskDetailDto.IsInvoice = 1;
                var sameShelfCodeSingleInvOrderList = mOrderService.GetUnGeneratePickingBatchInfoBLL(config, wmsReceiptTaskDetailDto);
                if (sameShelfCodeSingleInvOrderList != null && sameShelfCodeSingleInvOrderList.Count > 0 && string.IsNullOrEmpty(sameShelfCodeSingleInvOrderList[0].ExceptionStr))
                {
                    Log("<--" + tasksCommon.DealFullName + "中同货位单品需发票订单获取完成，共计【" + sameShelfCodeSingleInvOrderList.Count + "】个货位，拣货批次生成中……-->");
                    string batchNoList = "";
                    int successQty = 0;
                    foreach (var item in sameShelfCodeSingleInvOrderList)
                    {
                        wmsReceiptTaskDetailDto.WareId = item.WareId;
                        wmsReceiptTaskDetailDto.AreaId = item.AreaId;
                        wmsReceiptTaskDetailDto.ChannelId = item.ChannelId;
                        wmsReceiptTaskDetailDto.StockId = item.StockId;
                        wmsReceiptTaskDetailDto.BatchType = 2;
                        wmsReceiptTaskDetailDto.OrTypePrivateCus = 2;
                        wmsReceiptTaskDetailDto.OrTypeProduct = 1;
                        wmsReceiptTaskDetailDto.OrTypeStocks = 1;
                        wmsReceiptTaskDetailDto.OrTypeInvoices = 1;
                        wmsReceiptTaskDetailDto.OrderBatchDetailType = 11;
                        var dealResult = mOrderService.GeneratePickingBatchBLL(config, wmsReceiptTaskDetailDto);
                        if (dealResult.DealFlag)
                        {
                            batchNoList += dealResult.SuccessId + ',';
                            successQty++;
                        }
                        else
                        {
                            Log(dealResult.DealMsg);
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(batchNoList))
                    {
                        batchNoList = batchNoList.Remove(batchNoList.Length - 1, 1);
                        Log("<--" + tasksCommon.DealFullName + "中同货位单品需发票订单生成拣货批次完成，共计【" + successQty + "】个批次，批次号集合为【" + batchNoList + "】-->");
                    }
                }
                else
                {
                    if (sameShelfCodeSingleInvOrderList.Count > 0 && !string.IsNullOrEmpty(sameShelfCodeSingleInvOrderList[0].ExceptionStr))
                    {
                        SendEmailTo(tasksCommon.DealFullName + "中同货位单品需发票订单获取中遇到异常，异常信息为：" + sameShelfCodeSingleInvOrderList[0].ExceptionStr, CustomConfig["EmailAddress"]);
                    }
                    else
                    {
                        Log("<--" + tasksCommon.DealFullName + "中没有需要生成拣货批次的同货位单品需发票订单-->");
                    }

                }
                #endregion

                #region 12：同货位单品订单拣货批次生成

                Log("<--" + tasksCommon.DealFullName + "中同货位单品订单获取中……-->");
                wmsReceiptTaskDetailDto.IsInvoice = 0;
                var sameShelfCodeSingleOrderList = mOrderService.GetUnGeneratePickingBatchInfoBLL(config, wmsReceiptTaskDetailDto);
                if (sameShelfCodeSingleOrderList != null && sameShelfCodeSingleOrderList.Count > 0 && string.IsNullOrEmpty(sameShelfCodeSingleOrderList[0].ExceptionStr))
                {
                    Log("<--" + tasksCommon.DealFullName + "中同货位单品订单获取完成，共计【" + sameShelfCodeSingleOrderList.Count + "】个货位，拣货批次生成中……-->");
                    string batchNoList = "";
                    int successQty = 0;
                    foreach (var item in sameShelfCodeSingleOrderList)
                    {
                        wmsReceiptTaskDetailDto.WareId = item.WareId;
                        wmsReceiptTaskDetailDto.AreaId = item.AreaId;
                        wmsReceiptTaskDetailDto.ChannelId = item.ChannelId;
                        wmsReceiptTaskDetailDto.StockId = item.StockId;
                        wmsReceiptTaskDetailDto.BatchType = 2;
                        wmsReceiptTaskDetailDto.OrTypePrivateCus = 2;
                        wmsReceiptTaskDetailDto.OrTypeProduct = 1;
                        wmsReceiptTaskDetailDto.OrTypeStocks = 1;
                        wmsReceiptTaskDetailDto.OrTypeInvoices = 2;
                        wmsReceiptTaskDetailDto.OrderBatchDetailType = 12;
                        var dealResult = mOrderService.GeneratePickingBatchBLL(config, wmsReceiptTaskDetailDto);
                        if (dealResult.DealFlag)
                        {
                            batchNoList += dealResult.SuccessId + ',';
                            successQty++;
                        }
                        else
                        {
                            Log(dealResult.DealMsg);
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(batchNoList))
                    {
                        batchNoList = batchNoList.Remove(batchNoList.Length - 1, 1);
                        Log("<--" + tasksCommon.DealFullName + "中同货位单品订单生成拣货批次完成，共计【" + successQty + "】个批次，批次号集合为【" + batchNoList + "】-->");
                    }
                }
                else
                {
                    if (sameShelfCodeSingleOrderList.Count > 0 && !string.IsNullOrEmpty(sameShelfCodeSingleOrderList[0].ExceptionStr))
                    {
                        SendEmailTo(tasksCommon.DealFullName + "中同货位单品订单获取中遇到异常，异常信息为：" + sameShelfCodeSingleOrderList[0].ExceptionStr, CustomConfig["EmailAddress"]);
                    }
                    else
                    {
                        Log("<--" + tasksCommon.DealFullName + "中没有需要生成拣货批次的同货位单品订单-->");
                    }

                }
                #endregion

                #region 13：同通道单品需发票订单拣货批次生成
                Log("<--" + tasksCommon.DealFullName + "中同通道单品需发票订单获取中……-->");
                wmsReceiptTaskDetailDto.CreateType = 2;
                wmsReceiptTaskDetailDto.IsInvoice = 1;
                var sameChannelSingleInvOrderList = mOrderService.GetUnGeneratePickingBatchInfoBLL(config, wmsReceiptTaskDetailDto);
                if (sameChannelSingleInvOrderList != null && sameChannelSingleInvOrderList.Count > 0 && string.IsNullOrEmpty(sameChannelSingleInvOrderList[0].ExceptionStr))
                {
                    Log("<--" + tasksCommon.DealFullName + "中同通道单品需发票订单获取完成，共计【" + sameChannelSingleInvOrderList.Count + "】个通道，拣货批次生成中……-->");
                    string batchNoList = "";
                    int successQty = 0;
                    foreach (var item in sameChannelSingleInvOrderList)
                    {
                        wmsReceiptTaskDetailDto.WareId = item.WareId;
                        wmsReceiptTaskDetailDto.AreaId = item.AreaId;
                        wmsReceiptTaskDetailDto.ChannelId = item.ChannelId;
                        wmsReceiptTaskDetailDto.StockId = 0;
                        wmsReceiptTaskDetailDto.BatchType = 2;
                        wmsReceiptTaskDetailDto.OrTypePrivateCus = 2;
                        wmsReceiptTaskDetailDto.OrTypeProduct = 1;
                        wmsReceiptTaskDetailDto.OrTypeStocks = 1;
                        wmsReceiptTaskDetailDto.OrTypeInvoices = 1;
                        wmsReceiptTaskDetailDto.OrderBatchDetailType = 13;
                        var dealResult = mOrderService.GeneratePickingBatchBLL(config, wmsReceiptTaskDetailDto);
                        if (dealResult.DealFlag)
                        {
                            batchNoList += dealResult.SuccessId + ',';
                            successQty++;
                        }
                        else
                        {
                            Log(dealResult.DealMsg);
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(batchNoList))
                    {
                        batchNoList = batchNoList.Remove(batchNoList.Length - 1, 1);
                        Log("<--" + tasksCommon.DealFullName + "中同通道单品需发票订单生成拣货批次完成，共计【" + successQty + "】个批次，批次号集合为【" + batchNoList + "】-->");
                    }
                }
                else
                {
                    if (sameChannelSingleInvOrderList.Count > 0 && !string.IsNullOrEmpty(sameChannelSingleInvOrderList[0].ExceptionStr))
                    {
                        SendEmailTo(tasksCommon.DealFullName + "中同通道单品需发票订单获取中遇到异常，异常信息为：" + sameChannelSingleInvOrderList[0].ExceptionStr, CustomConfig["EmailAddress"]);
                    }
                    else
                    {
                        Log("<--" + tasksCommon.DealFullName + "中没有需要生成拣货批次的同通道单品需发票订单-->");
                    }

                }
                #endregion

                #region 14：同通道单品订单拣货批次生成
                Log("<--" + tasksCommon.DealFullName + "中同通道单品订单获取中……-->");
                wmsReceiptTaskDetailDto.CreateType = 2;
                wmsReceiptTaskDetailDto.IsInvoice = 0;
                var sameChannelSingleOrderList = mOrderService.GetUnGeneratePickingBatchInfoBLL(config, wmsReceiptTaskDetailDto);
                if (sameChannelSingleOrderList != null && sameChannelSingleOrderList.Count > 0 && string.IsNullOrEmpty(sameChannelSingleOrderList[0].ExceptionStr))
                {
                    Log("<--" + tasksCommon.DealFullName + "中同通道单品订单获取完成，共计【" + sameChannelSingleOrderList.Count + "】个通道，拣货批次生成中……-->");
                    string batchNoList = "";
                    int successQty = 0;
                    foreach (var item in sameChannelSingleOrderList)
                    {
                        wmsReceiptTaskDetailDto.WareId = item.WareId;
                        wmsReceiptTaskDetailDto.AreaId = item.AreaId;
                        wmsReceiptTaskDetailDto.ChannelId = item.ChannelId;
                        wmsReceiptTaskDetailDto.StockId = 0;
                        wmsReceiptTaskDetailDto.BatchType = 2;
                        wmsReceiptTaskDetailDto.OrTypePrivateCus = 2;
                        wmsReceiptTaskDetailDto.OrTypeProduct = 1;
                        wmsReceiptTaskDetailDto.OrTypeStocks = 1;
                        wmsReceiptTaskDetailDto.OrTypeInvoices = 2;
                        wmsReceiptTaskDetailDto.OrderBatchDetailType = 14;
                        var dealResult = mOrderService.GeneratePickingBatchBLL(config, wmsReceiptTaskDetailDto);
                        if (dealResult.DealFlag)
                        {
                            batchNoList += dealResult.SuccessId + ',';
                            successQty++;
                        }
                        else
                        {
                            Log(dealResult.DealMsg);
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(batchNoList))
                    {
                        batchNoList = batchNoList.Remove(batchNoList.Length - 1, 1);
                        Log("<--" + tasksCommon.DealFullName + "中同通道单品订单生成拣货批次完成，共计【" + successQty + "】个批次，批次号集合为【" + batchNoList + "】-->");
                    }
                }
                else
                {
                    if (sameChannelSingleOrderList.Count > 0 && !string.IsNullOrEmpty(sameChannelSingleOrderList[0].ExceptionStr))
                    {
                        SendEmailTo(tasksCommon.DealFullName + "中同通道单品订单获取中遇到异常，异常信息为：" + sameChannelSingleOrderList[0].ExceptionStr, CustomConfig["EmailAddress"]);
                    }
                    else
                    {
                        Log("<--" + tasksCommon.DealFullName + "中没有需要生成拣货批次的同通道单品订单-->");
                    }

                }
                #endregion

                #region 15：同货区单品需发票订单拣货批次生成
                Log("<--" + tasksCommon.DealFullName + "中同货区单品需发票订单获取中……-->");
                wmsReceiptTaskDetailDto.CreateType = 3;
                wmsReceiptTaskDetailDto.IsInvoice = 1;
                var sameAreaSingleInvOrderList = mOrderService.GetUnGeneratePickingBatchInfoBLL(config, wmsReceiptTaskDetailDto);
                if (sameAreaSingleInvOrderList != null && sameAreaSingleInvOrderList.Count > 0 && string.IsNullOrEmpty(sameAreaSingleInvOrderList[0].ExceptionStr))
                {
                    Log("<--" + tasksCommon.DealFullName + "中同货区单品需发票订单获取完成，共计【" + sameAreaSingleInvOrderList.Count + "】个货区，拣货批次生成中……-->");
                    string batchNoList = "";
                    int successQty = 0;
                    foreach (var item in sameAreaSingleInvOrderList)
                    {
                        wmsReceiptTaskDetailDto.WareId = item.WareId;
                        wmsReceiptTaskDetailDto.AreaId = item.AreaId;
                        wmsReceiptTaskDetailDto.ChannelId = 0;
                        wmsReceiptTaskDetailDto.StockId = 0;
                        wmsReceiptTaskDetailDto.BatchType = 2;
                        wmsReceiptTaskDetailDto.OrTypePrivateCus = 2;
                        wmsReceiptTaskDetailDto.OrTypeProduct = 1;
                        wmsReceiptTaskDetailDto.OrTypeStocks = 1;
                        wmsReceiptTaskDetailDto.OrTypeInvoices = 1;
                        wmsReceiptTaskDetailDto.OrderBatchDetailType = 15;
                        var dealResult = mOrderService.GeneratePickingBatchBLL(config, wmsReceiptTaskDetailDto);
                        if (dealResult.DealFlag)
                        {
                            batchNoList += dealResult.SuccessId + ',';
                            successQty++;
                        }
                        else
                        {
                            Log(dealResult.DealMsg);
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(batchNoList))
                    {
                        batchNoList = batchNoList.Remove(batchNoList.Length - 1, 1);
                        Log("<--" + tasksCommon.DealFullName + "中同货区单品需发票订单生成拣货批次完成，共计【" + successQty + "】个批次，批次号集合为【" + batchNoList + "】-->");
                    }
                }
                else
                {
                    if (sameAreaSingleInvOrderList.Count > 0 && !string.IsNullOrEmpty(sameAreaSingleInvOrderList[0].ExceptionStr))
                    {
                        SendEmailTo(tasksCommon.DealFullName + "中同货区单品需发票订单获取中遇到异常，异常信息为：" + sameAreaSingleInvOrderList[0].ExceptionStr, CustomConfig["EmailAddress"]);
                    }
                    else
                    {
                        Log("<--" + tasksCommon.DealFullName + "中没有需要生成拣货批次的同货区单品需发票订单-->");
                    }

                }
                #endregion

                #region 16：同货区单品订单拣货批次生成
                Log("<--" + tasksCommon.DealFullName + "中同货区单品订单获取中……-->");
                wmsReceiptTaskDetailDto.CreateType = 3;
                wmsReceiptTaskDetailDto.IsInvoice = 0;
                var sameAreaSingleOrderList = mOrderService.GetUnGeneratePickingBatchInfoBLL(config, wmsReceiptTaskDetailDto);
                if (sameAreaSingleOrderList != null && sameAreaSingleOrderList.Count > 0 && string.IsNullOrEmpty(sameAreaSingleOrderList[0].ExceptionStr))
                {
                    Log("<--" + tasksCommon.DealFullName + "中同货区单品订单获取完成，共计【" + sameAreaSingleOrderList.Count + "】个货区，拣货批次生成中……-->");
                    string batchNoList = "";
                    int successQty = 0;
                    foreach (var item in sameAreaSingleOrderList)
                    {
                        wmsReceiptTaskDetailDto.WareId = item.WareId;
                        wmsReceiptTaskDetailDto.AreaId = item.AreaId;
                        wmsReceiptTaskDetailDto.ChannelId = 0;
                        wmsReceiptTaskDetailDto.StockId = 0;
                        wmsReceiptTaskDetailDto.BatchType = 2;
                        wmsReceiptTaskDetailDto.OrTypePrivateCus = 2;
                        wmsReceiptTaskDetailDto.OrTypeProduct = 1;
                        wmsReceiptTaskDetailDto.OrTypeStocks = 1;
                        wmsReceiptTaskDetailDto.OrTypeInvoices = 2;
                        wmsReceiptTaskDetailDto.OrderBatchDetailType = 16;
                        var dealResult = mOrderService.GeneratePickingBatchBLL(config, wmsReceiptTaskDetailDto);
                        if (dealResult.DealFlag)
                        {
                            batchNoList += dealResult.SuccessId + ',';
                            successQty++;
                        }
                        else
                        {
                            Log(dealResult.DealMsg);
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(batchNoList))
                    {
                        batchNoList = batchNoList.Remove(batchNoList.Length - 1, 1);
                        Log("<--" + tasksCommon.DealFullName + "中同货区单品订单生成拣货批次完成，共计【" + successQty + "】个批次，批次号集合为【" + batchNoList + "】-->");
                    }
                }
                else
                {
                    if (sameAreaSingleOrderList.Count > 0 && !string.IsNullOrEmpty(sameAreaSingleOrderList[0].ExceptionStr))
                    {
                        SendEmailTo(tasksCommon.DealFullName + "中同货区单品订单获取中遇到异常，异常信息为：" + sameAreaSingleOrderList[0].ExceptionStr, CustomConfig["EmailAddress"]);
                    }
                    else
                    {
                        Log("<--" + tasksCommon.DealFullName + "中没有需要生成拣货批次的同货区单品订单-->");
                    }

                }
                #endregion

                #region 17：同仓库单品需发票订单拣货批次生成
                Log("<--" + tasksCommon.DealFullName + "中同仓库单品需发票订单获取中……-->");
                wmsReceiptTaskDetailDto.CreateType = 4;
                wmsReceiptTaskDetailDto.IsInvoice = 1;
                var sameWareSingleInvOrderList = mOrderService.GetUnGeneratePickingBatchInfoBLL(config, wmsReceiptTaskDetailDto);
                if (sameWareSingleInvOrderList != null && sameWareSingleInvOrderList.Count > 0 && string.IsNullOrEmpty(sameWareSingleInvOrderList[0].ExceptionStr))
                {
                    Log("<--" + tasksCommon.DealFullName + "中同仓库单品需发票订单获取完成，共计【" + sameWareSingleInvOrderList.Count + "】个仓库，拣货批次生成中……-->");
                    string batchNoList = "";
                    int successQty = 0;
                    foreach (var item in sameWareSingleInvOrderList)
                    {
                        wmsReceiptTaskDetailDto.WareId = item.WareId;
                        wmsReceiptTaskDetailDto.AreaId = 0;
                        wmsReceiptTaskDetailDto.ChannelId = 0;
                        wmsReceiptTaskDetailDto.StockId = 0;
                        wmsReceiptTaskDetailDto.BatchType = 2;
                        wmsReceiptTaskDetailDto.OrTypePrivateCus = 2;
                        wmsReceiptTaskDetailDto.OrTypeProduct = 1;
                        wmsReceiptTaskDetailDto.OrTypeStocks = 1;
                        wmsReceiptTaskDetailDto.OrTypeInvoices = 1;
                        wmsReceiptTaskDetailDto.OrderBatchDetailType = 17;
                        var dealResult = mOrderService.GeneratePickingBatchBLL(config, wmsReceiptTaskDetailDto);
                        if (dealResult.DealFlag)
                        {
                            batchNoList += dealResult.SuccessId + ',';
                            successQty++;
                        }
                        else
                        {
                            Log(dealResult.DealMsg);
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(batchNoList))
                    {
                        batchNoList = batchNoList.Remove(batchNoList.Length - 1, 1);
                        Log("<--" + tasksCommon.DealFullName + "中同仓库单品需发票订单生成拣货批次完成，共计【" + successQty + "】个批次，批次号集合为【" + batchNoList + "】-->");
                    }
                }
                else
                {
                    if (sameWareSingleInvOrderList.Count > 0 && !string.IsNullOrEmpty(sameWareSingleInvOrderList[0].ExceptionStr))
                    {
                        SendEmailTo(tasksCommon.DealFullName + "中同仓库单品需发票订单获取中遇到异常，异常信息为：" + sameWareSingleInvOrderList[0].ExceptionStr, CustomConfig["EmailAddress"]);
                    }
                    else
                    {
                        Log("<--" + tasksCommon.DealFullName + "中没有需要生成拣货批次的同仓库单品需发票订单-->");
                    }

                }
                #endregion

                #region 18：同仓库单品订单拣货批次生成
                Log("<--" + tasksCommon.DealFullName + "中同仓库单品订单获取中……-->");
                wmsReceiptTaskDetailDto.CreateType = 4;
                wmsReceiptTaskDetailDto.IsInvoice = 0;
                var sameWareSingleOrderList = mOrderService.GetUnGeneratePickingBatchInfoBLL(config, wmsReceiptTaskDetailDto);
                if (sameWareSingleOrderList != null && sameWareSingleOrderList.Count > 0 && string.IsNullOrEmpty(sameWareSingleOrderList[0].ExceptionStr))
                {
                    Log("<--" + tasksCommon.DealFullName + "中同仓库单品订单获取完成，共计【" + sameWareSingleOrderList.Count + "】个仓库，拣货批次生成中……-->");
                    string batchNoList = "";
                    int successQty = 0;
                    foreach (var item in sameWareSingleOrderList)
                    {
                        wmsReceiptTaskDetailDto.WareId = item.WareId;
                        wmsReceiptTaskDetailDto.AreaId = 0;
                        wmsReceiptTaskDetailDto.ChannelId = 0;
                        wmsReceiptTaskDetailDto.StockId = 0;
                        wmsReceiptTaskDetailDto.BatchType = 2;
                        wmsReceiptTaskDetailDto.OrTypePrivateCus = 2;
                        wmsReceiptTaskDetailDto.OrTypeProduct = 1;
                        wmsReceiptTaskDetailDto.OrTypeStocks = 1;
                        wmsReceiptTaskDetailDto.OrTypeInvoices = 2;
                        wmsReceiptTaskDetailDto.OrderBatchDetailType = 18;
                        var dealResult = mOrderService.GeneratePickingBatchBLL(config, wmsReceiptTaskDetailDto);
                        if (dealResult.DealFlag)
                        {
                            batchNoList += dealResult.SuccessId + ',';
                            successQty++;
                        }
                        else
                        {
                            Log(dealResult.DealMsg);
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(batchNoList))
                    {
                        batchNoList = batchNoList.Remove(batchNoList.Length - 1, 1);
                        Log("<--" + tasksCommon.DealFullName + "中同仓库单品订单生成拣货批次完成，共计【" + successQty + "】个批次，批次号集合为【" + batchNoList + "】-->");
                    }
                }
                else
                {
                    if (sameWareSingleOrderList.Count > 0 && !string.IsNullOrEmpty(sameWareSingleOrderList[0].ExceptionStr))
                    {
                        SendEmailTo(tasksCommon.DealFullName + "中同仓库单品订单获取中遇到异常，异常信息为：" + sameWareSingleOrderList[0].ExceptionStr, CustomConfig["EmailAddress"]);
                    }
                    else
                    {
                        Log("<--" + tasksCommon.DealFullName + "中没有需要生成拣货批次的同仓库单品订单-->");
                    }

                }
                #endregion

                #region 19：多品大宗需发票订单拣货批次生成

                Log("<--" + tasksCommon.DealFullName + "中多品大宗需发票订单获取中……-->");
                wmsReceiptTaskDetailDto.WmsOrderType = 2;
                wmsReceiptTaskDetailDto.IsInvoice = 1;
                var bulkInvOrderList = mOrderService.GetUnGeneratePickingBatchInfoOfBulkBLL(config, wmsReceiptTaskDetailDto);
                if (bulkInvOrderList != null && bulkInvOrderList.Count > 0 && string.IsNullOrEmpty(bulkInvOrderList[0].ExceptionStr))
                {
                    Log("<--" + tasksCommon.DealFullName + "中多品大宗需发票订单获取完成，共计【" + bulkInvOrderList.Count + "】个订单，拣货批次生成中……-->");
                    string batchNoList = "";
                    int successQty = 0;
                    foreach (var item in bulkInvOrderList)
                    {
                        wmsReceiptTaskDetailDto.WareId = item.WareId;
                        wmsReceiptTaskDetailDto.ReceiptId = item.ReceiptId;
                        wmsReceiptTaskDetailDto.BatchType = 5;
                        wmsReceiptTaskDetailDto.OrTypePrivateCus = 2;
                        wmsReceiptTaskDetailDto.OrTypeProduct = 2;
                        wmsReceiptTaskDetailDto.OrTypeStocks = 1;
                        wmsReceiptTaskDetailDto.OrTypeInvoices = 1;
                        wmsReceiptTaskDetailDto.OrderBatchDetailType = 19;
                        var dealResult = mOrderService.GeneratePickingBatchOfSingleOrderBLL(config, wmsReceiptTaskDetailDto);
                        if (dealResult.DealFlag)
                        {
                            batchNoList += dealResult.SuccessId + ',';
                            successQty++;
                        }
                        else
                        {
                            Log(dealResult.DealMsg);
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(batchNoList))
                    {
                        batchNoList = batchNoList.Remove(batchNoList.Length - 1, 1);
                        Log("<--" + tasksCommon.DealFullName + "中多品大宗需发票订单生成拣货批次完成，共计【" + successQty + "】个批次，批次号集合为【" + batchNoList + "】-->");
                    }
                }
                else
                {
                    if (bulkInvOrderList.Count > 0 && !string.IsNullOrEmpty(bulkInvOrderList[0].ExceptionStr))
                    {
                        SendEmailTo(tasksCommon.DealFullName + "中多品大宗需发票订单获取中遇到异常，异常信息为：" + bulkInvOrderList[0].ExceptionStr, CustomConfig["EmailAddress"]);
                    }
                    else
                    {
                        Log("<--" + tasksCommon.DealFullName + "中没有需要生成拣货批次的多品大宗需发票订单-->");
                    }

                }

                #endregion

                #region 20：多品大宗订单拣货批次生成

                Log("<--" + tasksCommon.DealFullName + "中多品大宗需发票订单获取中……-->");
                wmsReceiptTaskDetailDto.WmsOrderType = 2;
                wmsReceiptTaskDetailDto.IsInvoice = 0;
                var bulkOrderList = mOrderService.GetUnGeneratePickingBatchInfoOfBulkBLL(config, wmsReceiptTaskDetailDto);
                if (bulkOrderList != null && bulkOrderList.Count > 0 && string.IsNullOrEmpty(bulkOrderList[0].ExceptionStr))
                {
                    Log("<--" + tasksCommon.DealFullName + "中多品大宗需发票订单获取完成，共计【" + bulkOrderList.Count + "】个订单，拣货批次生成中……-->");
                    string batchNoList = "";
                    int successQty = 0;
                    foreach (var item in bulkOrderList)
                    {
                        wmsReceiptTaskDetailDto.WareId = item.WareId;
                        wmsReceiptTaskDetailDto.ReceiptId = item.ReceiptId;
                        wmsReceiptTaskDetailDto.BatchType = 5;
                        wmsReceiptTaskDetailDto.OrTypePrivateCus = 2;
                        wmsReceiptTaskDetailDto.OrTypeProduct = 2;
                        wmsReceiptTaskDetailDto.OrTypeStocks = 1;
                        wmsReceiptTaskDetailDto.OrTypeInvoices = 2;
                        wmsReceiptTaskDetailDto.OrderBatchDetailType = 20;
                        var dealResult = mOrderService.GeneratePickingBatchOfSingleOrderBLL(config, wmsReceiptTaskDetailDto);
                        if (dealResult.DealFlag)
                        {
                            batchNoList += dealResult.SuccessId + ',';
                            successQty++;
                        }
                        else
                        {
                            Log(dealResult.DealMsg);
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(batchNoList))
                    {
                        batchNoList = batchNoList.Remove(batchNoList.Length - 1, 1);
                        Log("<--" + tasksCommon.DealFullName + "中多品大宗需发票订单生成拣货批次完成，共计【" + successQty + "】个批次，批次号集合为【" + batchNoList + "】-->");
                    }
                }
                else
                {
                    if (bulkOrderList.Count > 0 && !string.IsNullOrEmpty(bulkOrderList[0].ExceptionStr))
                    {
                        SendEmailTo(tasksCommon.DealFullName + "中多品大宗需发票订单获取中遇到异常，异常信息为：" + bulkOrderList[0].ExceptionStr, CustomConfig["EmailAddress"]);
                    }
                    else
                    {
                        Log("<--" + tasksCommon.DealFullName + "中没有需要生成拣货批次的多品大宗需发票订单-->");
                    }

                }

                #endregion

                #region 21：同通道多品需发票订单拣货批次生成
                Log("<--" + tasksCommon.DealFullName + "中同通道多品需发票订单获取中……-->");
                wmsReceiptTaskDetailDto.WmsOrderType = 2;
                wmsReceiptTaskDetailDto.CreateType = 2;
                wmsReceiptTaskDetailDto.IsInvoice = 1;
                var sameChannelInvOrderList = mOrderService.GetUnGeneratePickingBatchInfoOfMultiOrderBLL(config, wmsReceiptTaskDetailDto);
                if (sameChannelInvOrderList != null && sameChannelInvOrderList.Count > 0 && string.IsNullOrEmpty(sameChannelInvOrderList[0].ExceptionStr))
                {
                    Log("<--" + tasksCommon.DealFullName + "中同通道多品需发票订单获取完成，共计【" + sameChannelInvOrderList.Count + "】个通道，拣货批次生成中……-->");
                    string batchNoList = "";
                    int successQty = 0;
                    foreach (var item in sameChannelInvOrderList)
                    {
                        wmsReceiptTaskDetailDto.WareId = item.WareId;
                        wmsReceiptTaskDetailDto.AreaId = item.AreaId;
                        wmsReceiptTaskDetailDto.ChannelId = item.ChannelId;
                        wmsReceiptTaskDetailDto.StockId = 0;
                        wmsReceiptTaskDetailDto.BatchType = 2;
                        wmsReceiptTaskDetailDto.OrTypePrivateCus = 2;
                        wmsReceiptTaskDetailDto.OrTypeProduct = 2;
                        wmsReceiptTaskDetailDto.OrTypeStocks = 1;
                        wmsReceiptTaskDetailDto.OrTypeInvoices = 1;
                        wmsReceiptTaskDetailDto.OrderBatchDetailType = 21;
                        var dealResult = mOrderService.GeneratePickingBatchBLL(config, wmsReceiptTaskDetailDto);
                        if (dealResult.DealFlag)
                        {
                            batchNoList += dealResult.SuccessId + ',';
                            successQty++;
                        }
                        else
                        {
                            Log(dealResult.DealMsg);
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(batchNoList))
                    {
                        batchNoList = batchNoList.Remove(batchNoList.Length - 1, 1);
                        Log("<--" + tasksCommon.DealFullName + "中同通道多品需发票订单生成拣货批次完成，共计【" + successQty + "】个批次，批次号集合为【" + batchNoList + "】-->");
                    }
                }
                else
                {
                    if (sameChannelInvOrderList.Count > 0 && !string.IsNullOrEmpty(sameChannelInvOrderList[0].ExceptionStr))
                    {
                        SendEmailTo(tasksCommon.DealFullName + "中同通道多品需发票订单获取中遇到异常，异常信息为：" + sameChannelInvOrderList[0].ExceptionStr, CustomConfig["EmailAddress"]);
                    }
                    else
                    {
                        Log("<--" + tasksCommon.DealFullName + "中没有需要生成拣货批次的同通道多品需发票订单-->");
                    }

                }
                #endregion

                #region 22：同通道多品订单拣货批次生成
                Log("<--" + tasksCommon.DealFullName + "中同通道多品订单获取中……-->");
                wmsReceiptTaskDetailDto.WmsOrderType = 2;
                wmsReceiptTaskDetailDto.CreateType = 2;
                wmsReceiptTaskDetailDto.IsInvoice = 0;
                var sameChannelOrderList = mOrderService.GetUnGeneratePickingBatchInfoOfMultiOrderBLL(config, wmsReceiptTaskDetailDto);
                if (sameChannelOrderList != null && sameChannelOrderList.Count > 0 && string.IsNullOrEmpty(sameChannelOrderList[0].ExceptionStr))
                {
                    Log("<--" + tasksCommon.DealFullName + "中同通道多品订单获取完成，共计【" + sameChannelOrderList.Count + "】个通道，拣货批次生成中……-->");
                    string batchNoList = "";
                    int successQty = 0;
                    foreach (var item in sameChannelOrderList)
                    {
                        wmsReceiptTaskDetailDto.WareId = item.WareId;
                        wmsReceiptTaskDetailDto.AreaId = item.AreaId;
                        wmsReceiptTaskDetailDto.ChannelId = item.ChannelId;
                        wmsReceiptTaskDetailDto.StockId = 0;
                        wmsReceiptTaskDetailDto.BatchType = 2;
                        wmsReceiptTaskDetailDto.OrTypePrivateCus = 2;
                        wmsReceiptTaskDetailDto.OrTypeProduct = 2;
                        wmsReceiptTaskDetailDto.OrTypeStocks = 1;
                        wmsReceiptTaskDetailDto.OrTypeInvoices = 2;
                        wmsReceiptTaskDetailDto.OrderBatchDetailType = 22;
                        var dealResult = mOrderService.GeneratePickingBatchBLL(config, wmsReceiptTaskDetailDto);
                        if (dealResult.DealFlag)
                        {
                            batchNoList += dealResult.SuccessId + ',';
                            successQty++;
                        }
                        else
                        {
                            Log(dealResult.DealMsg);
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(batchNoList))
                    {
                        batchNoList = batchNoList.Remove(batchNoList.Length - 1, 1);
                        Log("<--" + tasksCommon.DealFullName + "中同通道多品订单生成拣货批次完成，共计【" + successQty + "】个批次，批次号集合为【" + batchNoList + "】-->");
                    }
                }
                else
                {
                    if (sameChannelOrderList.Count > 0 && !string.IsNullOrEmpty(sameChannelOrderList[0].ExceptionStr))
                    {
                        SendEmailTo(tasksCommon.DealFullName + "中同通道多品订单获取中遇到异常，异常信息为：" + sameChannelOrderList[0].ExceptionStr, CustomConfig["EmailAddress"]);
                    }
                    else
                    {
                        Log("<--" + tasksCommon.DealFullName + "中没有需要生成拣货批次的同通道多品订单-->");
                    }

                }
                #endregion

                #region 23：同货区多品需发票订单拣货批次生成
                Log("<--" + tasksCommon.DealFullName + "中同货区多品需发票订单获取中……-->");
                wmsReceiptTaskDetailDto.CreateType = 3;
                wmsReceiptTaskDetailDto.IsInvoice = 1;
                var sameAreaInvOrderList = mOrderService.GetUnGeneratePickingBatchInfoOfMultiOrderBLL(config, wmsReceiptTaskDetailDto);
                if (sameAreaInvOrderList != null && sameAreaInvOrderList.Count > 0 && string.IsNullOrEmpty(sameAreaInvOrderList[0].ExceptionStr))
                {
                    Log("<--" + tasksCommon.DealFullName + "中同货区多品需发票订单获取完成，共计【" + sameAreaInvOrderList.Count + "】个货区，拣货批次生成中……-->");
                    string batchNoList = "";
                    int successQty = 0;
                    foreach (var item in sameAreaInvOrderList)
                    {
                        wmsReceiptTaskDetailDto.WareId = item.WareId;
                        wmsReceiptTaskDetailDto.AreaId = item.AreaId;
                        wmsReceiptTaskDetailDto.ChannelId = 0;
                        wmsReceiptTaskDetailDto.StockId = 0;
                        wmsReceiptTaskDetailDto.BatchType = 2;
                        wmsReceiptTaskDetailDto.OrTypePrivateCus = 2;
                        wmsReceiptTaskDetailDto.OrTypeProduct = 2;
                        wmsReceiptTaskDetailDto.OrTypeStocks = 1;
                        wmsReceiptTaskDetailDto.OrTypeInvoices = 1;
                        wmsReceiptTaskDetailDto.OrderBatchDetailType = 23;
                        var dealResult = mOrderService.GeneratePickingBatchBLL(config, wmsReceiptTaskDetailDto);
                        if (dealResult.DealFlag)
                        {
                            batchNoList += dealResult.SuccessId + ',';
                            successQty++;
                        }
                        else
                        {
                            Log(dealResult.DealMsg);
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(batchNoList))
                    {
                        batchNoList = batchNoList.Remove(batchNoList.Length - 1, 1);
                        Log("<--" + tasksCommon.DealFullName + "中同货区多品需发票订单生成拣货批次完成，共计【" + successQty + "】个批次，批次号集合为【" + batchNoList + "】-->");
                    }
                }
                else
                {
                    if (sameAreaInvOrderList.Count > 0 && !string.IsNullOrEmpty(sameAreaInvOrderList[0].ExceptionStr))
                    {
                        SendEmailTo(tasksCommon.DealFullName + "中同货区多品需发票订单获取中遇到异常，异常信息为：" + sameAreaInvOrderList[0].ExceptionStr, CustomConfig["EmailAddress"]);
                    }
                    else
                    {
                        Log("<--" + tasksCommon.DealFullName + "中没有需要生成拣货批次的同货区多品需发票订单-->");
                    }

                }
                #endregion

                #region 24：同货区多品订单拣货批次生成
                Log("<--" + tasksCommon.DealFullName + "中同货区多品订单获取中……-->");
                wmsReceiptTaskDetailDto.CreateType = 3;
                wmsReceiptTaskDetailDto.IsInvoice = 0;
                var sameAreaOrderList = mOrderService.GetUnGeneratePickingBatchInfoOfMultiOrderBLL(config, wmsReceiptTaskDetailDto);
                if (sameAreaOrderList != null && sameAreaOrderList.Count > 0 && string.IsNullOrEmpty(sameAreaOrderList[0].ExceptionStr))
                {
                    Log("<--" + tasksCommon.DealFullName + "中同货区多品订单获取完成，共计【" + sameAreaOrderList.Count + "】个货区，拣货批次生成中……-->");
                    string batchNoList = "";
                    int successQty = 0;
                    foreach (var item in sameAreaOrderList)
                    {
                        wmsReceiptTaskDetailDto.WareId = item.WareId;
                        wmsReceiptTaskDetailDto.AreaId = item.AreaId;
                        wmsReceiptTaskDetailDto.ChannelId = 0;
                        wmsReceiptTaskDetailDto.StockId = 0;
                        wmsReceiptTaskDetailDto.BatchType = 2;
                        wmsReceiptTaskDetailDto.OrTypePrivateCus = 2;
                        wmsReceiptTaskDetailDto.OrTypeProduct = 2;
                        wmsReceiptTaskDetailDto.OrTypeStocks = 1;
                        wmsReceiptTaskDetailDto.OrTypeInvoices = 2;
                        wmsReceiptTaskDetailDto.OrderBatchDetailType = 24;
                        var dealResult = mOrderService.GeneratePickingBatchBLL(config, wmsReceiptTaskDetailDto);
                        if (dealResult.DealFlag)
                        {
                            batchNoList += dealResult.SuccessId + ',';
                            successQty++;
                        }
                        else
                        {
                            Log(dealResult.DealMsg);
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(batchNoList))
                    {
                        batchNoList = batchNoList.Remove(batchNoList.Length - 1, 1);
                        Log("<--" + tasksCommon.DealFullName + "中同货区多品订单生成拣货批次完成，共计【" + successQty + "】个批次，批次号集合为【" + batchNoList + "】-->");
                    }
                }
                else
                {
                    if (sameAreaOrderList.Count > 0 && !string.IsNullOrEmpty(sameAreaOrderList[0].ExceptionStr))
                    {
                        SendEmailTo(tasksCommon.DealFullName + "中同货区多品订单获取中遇到异常，异常信息为：" + sameAreaOrderList[0].ExceptionStr, CustomConfig["EmailAddress"]);
                    }
                    else
                    {
                        Log("<--" + tasksCommon.DealFullName + "中没有需要生成拣货批次的同货区多品订单-->");
                    }

                }
                #endregion

                #region 25：同仓库多品需发票订单拣货批次生成
                Log("<--" + tasksCommon.DealFullName + "中同仓库多品需发票订单获取中……-->");
                wmsReceiptTaskDetailDto.CreateType = 4;
                wmsReceiptTaskDetailDto.IsInvoice = 1;
                wmsReceiptTaskDetailDto.WmsOrderType = 0;
                var sameWareInvOrderList = mOrderService.GetUnGeneratePickingBatchInfoOfMultiOrderBLL(config, wmsReceiptTaskDetailDto);
                if (sameWareInvOrderList != null && sameWareInvOrderList.Count > 0 && string.IsNullOrEmpty(sameWareInvOrderList[0].ExceptionStr))
                {
                    Log("<--" + tasksCommon.DealFullName + "中同仓库多品需发票订单获取完成，共计【" + sameWareInvOrderList.Count + "】个仓库，拣货批次生成中……-->");
                    string batchNoList = "";
                    int successQty = 0;
                    foreach (var item in sameWareInvOrderList)
                    {
                        wmsReceiptTaskDetailDto.WareId = item.WareId;
                        wmsReceiptTaskDetailDto.AreaId = 0;
                        wmsReceiptTaskDetailDto.ChannelId = 0;
                        wmsReceiptTaskDetailDto.StockId = 0;
                        wmsReceiptTaskDetailDto.BatchType = 2;
                        wmsReceiptTaskDetailDto.OrTypePrivateCus = 2;
                        wmsReceiptTaskDetailDto.OrTypeProduct = 0;
                        wmsReceiptTaskDetailDto.OrTypeStocks = 1;
                        wmsReceiptTaskDetailDto.OrTypeInvoices = 1;
                        wmsReceiptTaskDetailDto.OrderBatchDetailType = 25;
                        var dealResult = mOrderService.GeneratePickingBatchBLL(config, wmsReceiptTaskDetailDto);
                        if (dealResult.DealFlag)
                        {
                            batchNoList += dealResult.SuccessId + ',';
                            successQty++;
                        }
                        else
                        {
                            Log(dealResult.DealMsg);
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(batchNoList))
                    {
                        batchNoList = batchNoList.Remove(batchNoList.Length - 1, 1);
                        Log("<--" + tasksCommon.DealFullName + "中同仓库多品需发票订单生成拣货批次完成，共计【" + successQty + "】个批次，批次号集合为【" + batchNoList + "】-->");
                    }
                }
                else
                {
                    if (sameWareInvOrderList.Count > 0 && !string.IsNullOrEmpty(sameWareInvOrderList[0].ExceptionStr))
                    {
                        SendEmailTo(tasksCommon.DealFullName + "中同仓库多品需发票订单获取中遇到异常，异常信息为：" + sameWareInvOrderList[0].ExceptionStr, CustomConfig["EmailAddress"]);
                    }
                    else
                    {
                        Log("<--" + tasksCommon.DealFullName + "中没有需要生成拣货批次的同仓库多品需发票订单-->");
                    }

                }
                #endregion

                #region 26：同仓库多品订单拣货批次生成
                Log("<--" + tasksCommon.DealFullName + "中同仓库多品订单获取中……-->");
                wmsReceiptTaskDetailDto.CreateType = 4;
                wmsReceiptTaskDetailDto.IsInvoice = 0;
                var sameWareOrderList = mOrderService.GetUnGeneratePickingBatchInfoOfMultiOrderBLL(config, wmsReceiptTaskDetailDto);
                if (sameWareOrderList != null && sameWareOrderList.Count > 0 && string.IsNullOrEmpty(sameWareOrderList[0].ExceptionStr))
                {
                    Log("<--" + tasksCommon.DealFullName + "中同仓库多品订单获取完成，共计【" + sameWareOrderList.Count + "】个仓库，拣货批次生成中……-->");
                    string batchNoList = "";
                    int successQty = 0;
                    foreach (var item in sameWareOrderList)
                    {
                        wmsReceiptTaskDetailDto.WareId = item.WareId;
                        wmsReceiptTaskDetailDto.AreaId = 0;
                        wmsReceiptTaskDetailDto.ChannelId = 0;
                        wmsReceiptTaskDetailDto.StockId = 0;
                        wmsReceiptTaskDetailDto.BatchType = 2;
                        wmsReceiptTaskDetailDto.OrTypePrivateCus = 2;
                        wmsReceiptTaskDetailDto.OrTypeProduct = 0;
                        wmsReceiptTaskDetailDto.OrTypeStocks = 1;
                        wmsReceiptTaskDetailDto.OrTypeInvoices = 2;
                        wmsReceiptTaskDetailDto.OrderBatchDetailType = 26;
                        var dealResult = mOrderService.GeneratePickingBatchBLL(config, wmsReceiptTaskDetailDto);
                        if (dealResult.DealFlag)
                        {
                            batchNoList += dealResult.SuccessId + ',';
                            successQty++;
                        }
                        else
                        {
                            Log(dealResult.DealMsg);
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(batchNoList))
                    {
                        batchNoList = batchNoList.Remove(batchNoList.Length - 1, 1);
                        Log("<--" + tasksCommon.DealFullName + "中同仓库多品订单生成拣货批次完成，共计【" + successQty + "】个批次，批次号集合为【" + batchNoList + "】-->");
                    }
                }
                else
                {
                    if (sameWareOrderList.Count > 0 && !string.IsNullOrEmpty(sameWareOrderList[0].ExceptionStr))
                    {
                        SendEmailTo(tasksCommon.DealFullName + "中同仓库多品订单获取中遇到异常，异常信息为：" + sameWareOrderList[0].ExceptionStr, CustomConfig["EmailAddress"]);
                    }
                    else
                    {
                        Log("<--" + tasksCommon.DealFullName + "中没有需要生成拣货批次的同仓库多品订单-->");
                    }

                }
                #endregion

                runTaskResult.Success = true;
                runTaskResult.Result = tasksCommon.DealFullName + "生成拣货批次成功！";
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
