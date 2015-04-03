using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskPlatform.TaskInterface;
using System.Data;
using Task.Service.Order;
using Task.Service.Impl.Order;
using Task.Model.Order;
namespace Demo
{
    public class DemoTask : AbstractTask
    {
        IOrderService iOrderService = new OrderService();
        /// <summary>
        /// 服务名称
        /// </summary>
        /// <returns></returns>
        public override string TaskName()
        {
            return "自动调整订单佣金";
        }
        /// <summary>
        /// 服务内容描述
        /// </summary>
        /// <returns></returns>
        public override string TaskDescription()
        {
            return "按照配置的时间开始进行自动调整订单佣金";
        }
        public override RunTaskResult RunTask()
        {
            RunTaskResult runTaskResult = new RunTaskResult();
            #region 验证服务配置
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

            string strConRead = CustomConfig["数据库连接字符串（读）"];
            string strConWrite = CustomConfig["数据库连接字符串（写）"];
            var groupList = iOrderService.GetGroupList(strConRead);
            try
            {
                #region 服务执行内容
                ShowRunningLog("开始获取公用配置信息……");
                var globalConfigModel = new GlobalConfigModel() { KeyName = "TimeSubsidies" };
                var globalConfigList = iOrderService.GetGlobalConfigInfo(strConRead, globalConfigModel);
                if (globalConfigList == null || globalConfigList.Count == 0)
                {
                    ShowRunningLog("没有获取到公用配置信息！");
                    WriteLog(TaskName() + globalConfigList[0].ExceptionStr);
                    runTaskResult.Success = true;
                }
                else
                {
                    if (globalConfigList[0].DealFlag)
                    {
                        ShowRunningLog("获取公用配置信息已完成，获取信息共计【" + globalConfigList.Count + "】条，具体信息如下：");
                        foreach (var item in globalConfigList)
                        {
                            ShowRunningLog("键："+item.KeyName+" 值："+item.Value);
                        }
                        runTaskResult.Success = true;
                    }
                    else
                    {
                        #region 确认订单接口调用失败
                        runTaskResult.Success = false;
                        runTaskResult.Result = globalConfigList[0].ExceptionStr;
                        ShowRunningLog(TaskName() + globalConfigList[0].ExceptionStr);
                        WriteLog(TaskName() + globalConfigList[0].ExceptionStr);
                        SendEmailTo(TaskName() + globalConfigList[0].ExceptionStr, CustomConfig["EmailAddress"]);
                        #endregion
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                #region 异常处理
                this.SendEmailTo(this.TaskName() + ex, CustomConfig["EmailAddress"]);
                runTaskResult.Success = false;
                runTaskResult.Result = ex.ToString();
                ShowRunningLog(ex.ToString());
                WriteLog(ex.ToString());
                #endregion
            }
            ShowRunningLog("=============本次获取公用配置信息结束=============");
            return runTaskResult;
        }
        public override Dictionary<string, string> UploadConfig()
        {
            #if DEBUG
                if (!CustomConfig.ContainsKey("数据库连接字符串（读）"))
                    CustomConfig.Add("数据库连接字符串（读）", Task.Common.Parameters.TestDBConnectionStringRead);
                if (!CustomConfig.ContainsKey("数据库连接字符串（写）"))
                    CustomConfig.Add("数据库连接字符串（写）", Task.Common.Parameters.TestDBConnectionStringWrite);
            #else
                if (!CustomConfig.ContainsKey("数据库连接字符串（读）"))
                    CustomConfig.Add("数据库连接字符串（读）", Task.Common.Parameters.OnLineDBConnectionStringRead);
                 if (!CustomConfig.ContainsKey("数据库连接字符串（写）"))
                    CustomConfig.Add("数据库连接字符串（写）", Task.Common.Parameters.OnLineDBConnectionStringWrite);
            #endif
                if (!CustomConfig.ContainsKey("EmailAddress"))
                CustomConfig.Add("EmailAddress", "wang.xudan@etaostars.com;");
            return CustomConfig;
        }
    }
}
