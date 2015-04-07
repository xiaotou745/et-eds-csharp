﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;
using Task.Common;
using Task.Model;
using Task.Model.Order;
using Task.Service.Impl.Order;
using Task.Service.Order;
using TaskPlatform.TaskInterface;

namespace YuDaExpressPostOrderInfo
{
    public class YuDaExpressPostOrderInfoTask : AbstractTask
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
            return "韵达快递提交订单（发货）";
        }
        /// <summary>
        /// 服务描述
        /// </summary>
        /// <returns></returns>
        public override string TaskDescription()
        {
            return "通过接口发送订单信息到韵达系统";
        }
        /// <summary>
        /// 服务实现
        /// </summary>
        /// <returns></returns>
        public override RunTaskResult RunTask()
        {
            RunTaskResult runTaskResult = new RunTaskResult();
            #region 配置验证
            if (string.IsNullOrWhiteSpace(CustomConfig["数据库连接字符串（读）"]))
            {
                ShowRunningLog("数据库连接字符串（读）！");
                runTaskResult.Result = "数据库连接字符串（读）！";
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
            Config config = new Config
                {
                    ReadConnectionString = CustomConfig["数据库连接字符串（读）"],
                    WriteConnectionString = CustomConfig["数据库连接字符串（写）"],
                };
            try
            {
                string strResult = "";
                strResult = YundaGetPack();
                ShowRunningLog(strResult);
                WriteLog(strResult);
                if (string.IsNullOrWhiteSpace(strResult))
                {
                    runTaskResult.Success = false;
                    runTaskResult.Result = "本次推送韵达快递的订单返回值为空串，接口异常请处理！";
                    ShowRunningLog("本次推送韵达快递的订单返回值为空串，接口异常请处理！");
                    WriteLog("本次推送韵达快递的订单返回值为空串，接口异常请处理！");
                    this.SendEmailTo(this.TaskName() + "本次推送韵达快递的订单返回值为空串，接口异常请处理！", CustomConfig["EmailAddress"]);
                    return runTaskResult;
                }
                if (strResult != "0")
                {
                    var model = XMLCommon.Deserialize(typeof(Task.Model.responses), strResult) as responses;
                    IList<OrderSync> listorder = new List<OrderSync>();
                    string strPackId = "";
                    foreach (var item in model.Responses)
                    {
                        OrderSync order = new OrderSync();
                        order.PackId = item.order_serial_no.ToInt();
                        order.BackMsg = item.msg;
                        order.DealFlag = item.status.ToInt();
                        strPackId += item.order_serial_no+",";
                        
                        listorder.Add(order);
                    }
                    strPackId=strPackId.Remove(strPackId.Length - 1, 1);
                    if (string.IsNullOrWhiteSpace(strPackId) || strPackId.Trim()=="0")
                    {
                        this.SendEmailTo(this.TaskName() + model.Responses[0].msg, CustomConfig["EmailAddress"]);
                        runTaskResult.Success = false;
                        runTaskResult.Result = this.TaskName() + model.Responses[0].msg;
                        ShowRunningLog(this.TaskName() + model.Responses[0].msg);
                        WriteLog(this.TaskName() + model.Responses[0].msg);
                        return runTaskResult;
                    }
                    Dictionary<EnumDealFlag, int> dic = new Dictionary<EnumDealFlag, int>();
                    dic = mOrderService.UpdateOrderSync(config, listorder);
                    if (dic[EnumDealFlag.Error]==-1)
                    {
                        this.SendEmailTo(this.TaskName() + "订单提交快递系统成功后回写WMS系统结果时失败,包裹ID集合为：" + strPackId, CustomConfig["EmailAddress"]);
                        runTaskResult.Success = true;
                        runTaskResult.Result = "订单提交快递系统成功后回写WMS系统结果时失败";
                        ShowRunningLog("订单提交快递系统成功后回写WMS系统结果时失败");
                        WriteLog("订单提交快递系统成功后回写WMS系统结果时失败");
                        
                    }
                    else
                    {
                        runTaskResult.Success = true;
                        runTaskResult.Result = "本批订单提交成功【" + dic[EnumDealFlag.One] + "】条，失败【" + dic[EnumDealFlag.Zero] + "】条";
                        ShowRunningLog("本批订单提交成功【" + dic[EnumDealFlag.One] + "】条，失败【" + dic[EnumDealFlag.Zero] + "】条");
                        WriteLog("本批订单提交成功【" + dic[EnumDealFlag.One] + "】条，失败【" + dic[EnumDealFlag.Zero] + "】条");
                    }
                }
                else
                {
                    runTaskResult.Success = true;
                    runTaskResult.Result = "当前没有需要推送的韵达快递的订单";
                    ShowRunningLog("当前没有需要推送的韵达快递的订单");
                    WriteLog("当前没有需要推送的韵达快递的订单");
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
        /// <summary>
        /// 服务配置
        /// </summary>
        /// <returns></returns>
        /// 
        public override Dictionary<string, string> UploadConfig()
        {
            if (!CustomConfig.ContainsKey("数据库连接字符串（读）"))
                CustomConfig.Add("数据库连接字符串（读）", Task.Common.Parameters.TestDBReadConnectionString0578);
            if (!CustomConfig.ContainsKey("数据库连接字符串（写）"))
                CustomConfig.Add("数据库连接字符串（写）", Task.Common.Parameters.TestDBConnectionString0578);
            if (!CustomConfig.ContainsKey("EmailAddress"))
                CustomConfig.Add("EmailAddress", "wangxudan@jiuxian.com;huxiaobing@jiuxian.com");
            if (!CustomConfig.ContainsKey("订单数量限制"))
                CustomConfig.Add("订单数量限制", Task.Common.Parameters.MaxPostCountYunDa.ToString());
            return CustomConfig;
        }
        /// <summary>
        /// 推送订单信息到韵达系统
        /// </summary>
        /// <returns></returns>
        public string YundaGetPack()
        {
            try
            {
                YunDaExpressOrderVo yundaorders = new YunDaExpressOrderVo();
                yundaorders = getOrders();
                if (yundaorders.order!=null )
                {
                    //#region 测试配置
                    //string url = "http://58.40.18.94:10110/cus_order/order_interface/interface_receive_order_binding.php";
                    //YunDaRequest yundarequest = new YunDaRequest
                    //{

                    //    partnerid = "1998855009",
                    //    password = "123456a",
                    //    xmldata = XMLCommon.objToXml(typeof(YunDaExpressOrderVo), yundaorders),
                    //    version = "1.0",
                    //    request = "data",
                    //};
                    //#endregion
                    #region 正式配置
                    string url = "http://order.yundasys.com:10235/cus_order/order_interface/interface_receive_order_binding.php";
                    YunDaRequest yundarequest = new YunDaRequest
                    {

                        partnerid = "1002901305",
                        password = "JUGPKgkmsbyqv47ItdxeVTriNSuwMY",
                        xmldata = XMLCommon.objToXml(typeof(YunDaExpressOrderVo), yundaorders),
                        version = "1.0",
                        request = "data",
                    };
                    #endregion
                    string base64_xmldata = Convert.ToBase64String(System.Text.Encoding.GetEncoding("UTF-8").GetBytes(yundarequest.xmldata));
                    string validation = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(base64_xmldata + yundarequest.partnerid + yundarequest.password, "MD5").ToLower();
                    string signdata = "partnerid=" + yundarequest.partnerid + "&version=" + yundarequest.version + "&request=" + yundarequest.request + "&xmldata=" + HttpUtility.UrlEncode(base64_xmldata) + "&validation=" + validation;
                    return HttpHelper.Post(url, signdata);
                }
                else
                {
                    return "0";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        /// <summary>
        /// 准备推送的订单信息
        /// </summary>
        /// <returns></returns>
        public YunDaExpressOrderVo getOrders()
        {
            try
            {
                Config config = new Config();
                YunDaExpressOrderVo orders = new YunDaExpressOrderVo();
                List<YunDaExpressOrder> orderList = new List<YunDaExpressOrder>();
                config.Limit =CustomConfig["订单数量限制"].ToInt();
                config.ReadConnectionString = CustomConfig["数据库连接字符串（读）"];
                IList<OrderSync> listordersync = new List<OrderSync>();
                listordersync = mOrderService.QueryOrderNoSync(config);
                
                if (listordersync.Count > 0)
                {
                    for (int i = 0; i < listordersync.Count; i++)
                    {
                        #region 整理传送信息
                        YunDaExpressOrder order = new YunDaExpressOrder();
                        order.order_serial_no = listordersync[i].PackId.ToString(); //包裹Id（必须唯一）
                        order.khddh = listordersync[i].OrderId.ToString(); //订单id
                        order.mailno =listordersync[i].InvoiceNo!=""? listordersync[i].InvoiceNo.Substring(0, 13):"";//电子运单号（13位-截取电子运单表中运单的前13位）
                        #region 发件人信息
                        Sender sender = new Sender();
                        sender.name = "酒仙网";//发件人姓名
                        sender.company = "酒仙网电子商务股份有限公司";//发件公司（或个人姓名）
                        sender.city = "北京，北京市，朝阳区";//始发城市（省，市，区\县 中间用逗号分隔）
                        sender.address = "北京市朝阳区十八里店乡张家店村340号 ";//发件地址
                        sender.mobile = "";//发件人手机(手机和固话必须有其一)
                        sender.phone = "4006179999";//发件人固话
                        order.sender = sender;
                        #endregion
                        #region 收件人信息
                        Receiver receiver = new Receiver();
                        receiver.name = listordersync[i].ReceiveName;//收件人姓名
                        receiver.city = string.Format("{0},{1},{2}", listordersync[i].ProvinceName, listordersync[i].CityName, listordersync[i].DistrictName);//送达城市（省，市，区\县 中间用逗号分隔）
                        receiver.address = string.Format("{0}{1}{2}{3}", listordersync[i].ProvinceName, listordersync[i].CityName, listordersync[i].DistrictName, listordersync[i].Address);//收件地址
                        receiver.mobile = listordersync[i].Mobile;//收件人手机(手机和固话必须有其一)
                        receiver.phone = listordersync[i].Phone;//收件人固话
                        order.receiver = receiver;
                        #endregion
                        orderList.Add(order);
                        orders.order = orderList;
                        #endregion
                    }
                }
                //long expressid = 5000001769808;
                return orders;
            }
            catch (Exception ex)
            { 
                throw new Exception(ex.ToString());
            }
            
        }
    }


    
}