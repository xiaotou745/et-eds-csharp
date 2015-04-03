/*
 * Jak 20140414
 * 调拨预警服务
 * 主要业务：
 * 1，发货后7天内未收货的发预警
 * 2，收货后1天内未验货的发预警
 */
using System;
using System.Collections.Generic;
using Task.Service.Impl;
using TaskPlatform.TaskInterface;
using Task.Model;
using System.Linq;
using System.Text;
using Task.Common;
using System.Reflection;
using System.Threading.Tasks;

namespace AllocateEWS
{
    public class AllocateEWSTask : AbstractTask
    {
        /// <summary>
        /// 服务名称
        /// </summary>
        /// <returns></returns>
        public override string TaskName()
        {
            return "调拨预警服务";
        }

        /// <summary>
        /// 服务内容描述
        /// </summary>
        /// <returns></returns>
        public override string TaskDescription()
        {
            return "调拨预警服务";
        }

        public override RunTaskResult RunTask()
        {
            RunTaskResult runTaskResult = new RunTaskResult();

            try
            {
                CheckAllocate(EnumAllocateOptType.NotAccept);
                CheckAllocate(EnumAllocateOptType.NotCheck);

                runTaskResult.Success = true;
                runTaskResult.Result = "调拨单收货、验货预警发送完成！";
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

        private void CheckAllocate(EnumAllocateOptType optType)
        {
            var impl = new AllocateService();
            IList<JxErpAllocateDtoExt> list = impl.QueryList(new Task.Model.Config { ConnectionString = CustomConfig["数据库连接字符串"] }, optType);
            ShowRunningLog("======调拨单记录数==========" + list.Count);

            if (list != null && list.Count > 0)
            {
                if (optType.Equals(EnumAllocateOptType.NotAccept))
                {
                    ShowRunningLog("调拨单发货7天未收货记录数：" + list.Count);
                    WriteLog("调拨单发货7天未收货记录数：" + list.Count);
                }
                else
                {
                    ShowRunningLog("调拨单收货24小时未验货记录数：" + list.Count);
                    WriteLog("调拨单收货24小时未验货记录数：" + list.Count);
                }

                int[] arrayWareHouseIds = (int[])typeof(EnumWareHouse).GetEnumValues();
                Parallel.ForEach(arrayWareHouseIds, wareHouseId =>
                {
                    var listFilter = list.Where(o => o.To_ware_house == wareHouseId).ToList();
                    if (listFilter.Count > 0)
                    {
                        ShowRunningLog(string.Format("{0}调拨单{1}记录数：{2}", wareHouseId, optType, listFilter.Count));
                        SendEmail2WareContacts(listFilter, optType, wareHouseId);
                    }
                    else
                    {
                        ShowRunningLog(string.Format("{0}调拨单{1}记录数：{2}", wareHouseId, optType, listFilter.Count));
                    }
                });
            }
        }

        /// <summary>
        /// 发送预警邮件给仓库相关联系人
        /// </summary>
        /// <param name="list">未收、验的调拨单集合</param>
        /// <param name="optType">EnumAllocateOptType</param>
        /// <param name="wareHouseId">仓库编号</param>
        private void SendEmail2WareContacts(IList<JxErpAllocateDtoExt> list, EnumAllocateOptType optType, int wareHouseId)
        {
            int count = list.Count;
            string wareHouseName = EnumUtil.GetEnumDescription((EnumWareHouse)wareHouseId);
            string optTypeName = EnumUtil.GetEnumDescription(optType);
            string emailTitle = string.Format("调拨单{0}未完成预警-{1}", optTypeName, wareHouseName);
            string attachName = string.Format("{0}{1}.xls", emailTitle, DateTime.Now.ToString("yyyyMMddHHmmss"));

            StringBuilder sb = new StringBuilder(Task.Common.Parameters.SystemMsgForEWS);
            if (optType.Equals(EnumAllocateOptType.NotAccept))
            {
                ShowRunningLog(string.Format("发货7天未收货-{0}：{1}", wareHouseName, count));
                WriteLog(string.Format("发货7天未收货-{0}：{1}", wareHouseName, count));
                sb.AppendLine("<br />您好：<br />&nbsp;&nbsp;&nbsp;&nbsp; WMS系统中共有 <strong>" + count + "</strong> 个调拨单发货后7天未完成收货操作！请及时在WMS系统对相关的调拨单进行后续操作。谢谢！<br />");
                if (list.Count > Task.Common.Parameters.MaxCountForEWS)
                {
                    sb.AppendLine(Task.Common.Parameters.SystemMsgAttachment);
                }
                sb.Append(SendEmailCommon.CreateHtmlBody(StructHeadForAccept(), list).ToString());
                if (list.Count > Task.Common.Parameters.MaxCountForEWS)
                {
                    SendEmailTo(sb.ToString(), GetEmailAddress(optTypeName, wareHouseName), emailTitle, GetCCEmailAddress, true, attachName, StructHeadForAccept(), list);
                }
                else
                {
                    SendEmailTo(sb.ToString(), GetEmailAddress(optTypeName, wareHouseName), emailTitle, GetCCEmailAddress, true);
                }
            }
            else
            {
                ShowRunningLog(string.Format("收货24小时未验货-{0}：{1}", wareHouseName, count));
                WriteLog(string.Format("收货24小时未验货-{0}：{1}", wareHouseName, count));
                sb.AppendLine("<br />您好：<br />&nbsp;&nbsp;&nbsp;&nbsp; WMS系统中共有 <strong>" + count + "</strong> 个调拨单收货后24小时未完成验货操作！ 请及时在WMS系统对相关的调拨单进行后续操作。谢谢！<br />");
                sb.Append(SendEmailCommon.CreateHtmlBody(StructHecaForCheck(), list).ToString());
                if (list.Count > Task.Common.Parameters.MaxCountForEWS)
                {
                    SendEmailTo(sb.ToString(), GetEmailAddress(optTypeName, wareHouseName), emailTitle, GetCCEmailAddress, true, attachName, StructHecaForCheck(), list);
                }
                else
                {
                    SendEmailTo(sb.ToString(), GetEmailAddress(optTypeName, wareHouseName), emailTitle, GetCCEmailAddress, true);
                }
            }

            //SendEmailTo(sb.ToString(), CustomConfig["EmailAddress"], emailTitle, true);//test
            //SendEmailTo(sb.ToString(), GetEmailAddress(optTypeName, wareHouseName) , emailTitle, true);
            //SendEmailTo(sb.ToString(), GetEmailAddress(optTypeName, wareHouseName), emailTitle, ccEmail, true);

        }

        private string GetEmailAddress(string optTypeName, string wareHouseName)
        {
            return CustomConfig[string.Format("{0}-调拨{1}预警", wareHouseName, optTypeName)];
        }

        private string GetCCEmailAddress
        {
            get { return CustomConfig["抄送邮件地址"]; }
        }

        private Dictionary<string, string> StructHeadForAccept()
        {
            Dictionary<string, string> dicHead = new Dictionary<string, string>();
            dicHead.Add("FromWareName", "发货仓库");
            dicHead.Add("ToWareName", "收货仓库");
            dicHead.Add("Allocate_sn", "调拨单号");
            dicHead.Add("StatusName", "状态");
            dicHead.Add("Add_name", "发货人");
            dicHead.Add("addDatetime", "发货时间");
            return dicHead;
        }

        private Dictionary<string, string> StructHecaForCheck()
        {
            Dictionary<string, string> dicHead = new Dictionary<string, string>();
            dicHead.Add("FromWareName", "发货仓库");
            dicHead.Add("ToWareName", "收货仓库");
            dicHead.Add("Allocate_sn", "调拨单号");
            dicHead.Add("StatusName", "状态");
            dicHead.Add("AcceptName", "收货人");
            dicHead.Add("receiveDateTime", "收货时间");
            return dicHead;
        }

        //const string ccEmail = "wangxudan@jiuxian.com";
        const string ccEmail ="caoguisheng@jiuxian.com,yebing@jiuxian.com,wangwanyi@jiuxian.com,niuwenjian@jiuxian.com,liyanlin@jiuxian.com,sunyinyin@jiuxian.com";
        const string ccEmail2 = "caoguisheng@jiuxian.com;yebing@jiuxian.com;wangwanyi@jiuxian.com;niuwenjian@jiuxian.com;liyanlin@jiuxian.com;sunyinyin@jiuxian.com;";
        public override Dictionary<string, string> UploadConfig()
        {
            if (!CustomConfig.ContainsKey("数据库连接字符串"))
                CustomConfig.Add("数据库连接字符串", "Server=192.168.11.21;Database=jiuxianweb;User ID=select_limit;password=Only_in_jx_select;Pooling=false;");
            //CustomConfig.Add("数据库连接字符串", "Server=192.168.12.30;Database=jiuxianweb;User ID=test_db_online;password=QwerAs131126dfTyui1209;Pooling=false;");

            if (!CustomConfig.ContainsKey("EmailAddress"))
                CustomConfig.Add("EmailAddress", "liyanlin@jiuxian.com");
            if (!CustomConfig.ContainsKey("抄送邮件地址"))
                CustomConfig.Add("抄送邮件地址", ccEmail);

            #region 正式
            // 收货组，正式
            if (!CustomConfig.ContainsKey("北京仓-调拨收货预警"))
                CustomConfig.Add("北京仓-调拨收货预警", "chengyongyou@jiuxian.com;dongli@jiuxian.com;");
            if (!CustomConfig.ContainsKey("广州仓-调拨收货预警"))
                CustomConfig.Add("广州仓-调拨收货预警", "zhangzhiyuan@jiuxian.com;fanshijie@jiuxian.com;xiejinzhong@jiuxian.com;");
            if (!CustomConfig.ContainsKey("上海仓-调拨收货预警"))
                CustomConfig.Add("上海仓-调拨收货预警", "tangyunfei@jiuxian.com;dongxixi@jiuxian.com;zhaojianhong@jiuxian.com;zhoushuangping@jiuxian.com;gaowenxue@jiuxian.com;");
            if (!CustomConfig.ContainsKey("武汉仓-调拨收货预警"))
                CustomConfig.Add("武汉仓-调拨收货预警", "zhangxin@jiuxian.com;zhumingqiang@jiuxian.com;zhaowei@jiuxian.com;hehaidong@jiuxian.com;");
            if (!CustomConfig.ContainsKey("天津仓-调拨收货预警"))
                CustomConfig.Add("天津仓-调拨收货预警", "hanyitin@jiuxian.com;weishuisheng@jiuxian.com;jiangliguang@jiuxian.com;");
            if (!CustomConfig.ContainsKey("成都仓-调拨收货预警"))
                CustomConfig.Add("成都仓-调拨收货预警", "wuyixin@jiuxian.com;wangyong@jiuxian.com;xiejinzhong@jiuxian.com;menghujun@jiuxian.com;");
            // 验货组，正式
            if (!CustomConfig.ContainsKey("北京仓-调拨验货预警"))
                CustomConfig.Add("北京仓-调拨验货预警", "guoximing@jiuxian.com;yangzhao@jiuxian.com;");
            if (!CustomConfig.ContainsKey("广州仓-调拨验货预警"))
                CustomConfig.Add("广州仓-调拨验货预警", "zhangzhiyuan@jiuxian.com;mofacao@jiuxian.com;huqingrong@jiuxian.com;zhanghong@jiuxian.com;");
            if (!CustomConfig.ContainsKey("上海仓-调拨验货预警"))
                CustomConfig.Add("上海仓-调拨验货预警", "tangyunfei@jiuxian.com;dongxixi@jiuxian.com;zhaojianhong@jiuxian.com;zhoushuangping@jiuxian.com;jiahuibiao@jiuxian.com;");
            if (!CustomConfig.ContainsKey("武汉仓-调拨验货预警"))
                CustomConfig.Add("武汉仓-调拨验货预警", "zhangxin@jiuxian.com;zhumingqiang@jiuxian.com;hechao@jiuxian.com;");
            if (!CustomConfig.ContainsKey("天津仓-调拨验货预警"))
                CustomConfig.Add("天津仓-调拨验货预警", "hanyitin@jiuxian.com;guoaifeng@jiuxian.com;mengtenglong@jiuxian.com;");
            if (!CustomConfig.ContainsKey("成都仓-调拨验货预警"))
                CustomConfig.Add("成都仓-调拨验货预警", "wuyixin@jiuxian.com;xiejinzhong@jiuxian.com;menghujun@jiuxian.com;");
            #endregion


            //#region 测试
               
            //// 收货组，测试
            //if (!CustomConfig.ContainsKey("北京仓-调拨收货预警"))
            //    CustomConfig.Add("北京仓-调拨收货预警", ccEmail);
            //if (!CustomConfig.ContainsKey("广州仓-调拨收货预警"))
            //    CustomConfig.Add("广州仓-调拨收货预警", ccEmail);
            //if (!CustomConfig.ContainsKey("上海仓-调拨收货预警"))
            //    CustomConfig.Add("上海仓-调拨收货预警", ccEmail);
            //if (!CustomConfig.ContainsKey("武汉仓-调拨收货预警"))
            //    CustomConfig.Add("武汉仓-调拨收货预警", ccEmail);
            //if (!CustomConfig.ContainsKey("天津仓-调拨收货预警"))
            //    CustomConfig.Add("天津仓-调拨收货预警", ccEmail);
            //if (!CustomConfig.ContainsKey("成都仓-调拨收货预警"))
            //    CustomConfig.Add("成都仓-调拨收货预警", ccEmail);
            
            ////验货组，测试
            //if (!CustomConfig.ContainsKey("北京仓-调拨验货预警"))
            //    CustomConfig.Add("北京仓-调拨验货预警", ccEmail);
            //if (!CustomConfig.ContainsKey("广州仓-调拨验货预警"))
            //    CustomConfig.Add("广州仓-调拨验货预警", ccEmail);
            //if (!CustomConfig.ContainsKey("上海仓-调拨验货预警"))
            //    CustomConfig.Add("上海仓-调拨验货预警", ccEmail);
            //if (!CustomConfig.ContainsKey("武汉仓-调拨验货预警"))
            //    CustomConfig.Add("武汉仓-调拨验货预警", ccEmail);
            //if (!CustomConfig.ContainsKey("天津仓-调拨验货预警"))
            //    CustomConfig.Add("天津仓-调拨验货预警", ccEmail);
            //if (!CustomConfig.ContainsKey("成都仓-调拨验货预警"))
            //    CustomConfig.Add("成都仓-调拨验货预警", ccEmail);
            
            //#endregion

            return CustomConfig;
        }

    }
}