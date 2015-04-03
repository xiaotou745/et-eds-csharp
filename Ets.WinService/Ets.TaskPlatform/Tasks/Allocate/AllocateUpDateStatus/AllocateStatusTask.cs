using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TaskPlatform.TaskInterface;
using System.Data;
using Task.Model;
using Task.Service.Impl;
namespace Demo
{
    public class AllocateStatusTask : AbstractTask
    {
        /// <summary>
        /// 服务名称
        /// </summary>
        /// <returns></returns>
        public override string TaskName()
        {
            return "调拨单筛选缺货服务";
        }
        /// <summary>
        /// 服务内容描述
        /// </summary>
        /// <returns></returns>
        public override string TaskDescription()
        {
            return "调拨单拣货打印前筛选缺货调拨单服务";
        }
        public override RunTaskResult RunTask()
        {
            RunTaskResult runTaskResult = new RunTaskResult();

            try
            {
                ShowRunningLog(TaskName() + "开始执行...\r\n时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n");
                //1.获取配置参数
                var config = new Config
                {
                    ConnectionString = CustomConfig["数据库连接字符串"],
                };
                //string WareStr = CustomConfig["各仓仓库代码配置"];
                string WareStr =null;
                string timeStr = CustomConfig["查询数据间隔隔段（天）"];
                string whereStr = string.Empty;
                //2.验证配置参数组合查询条件
                if (!string.IsNullOrEmpty(WareStr))
                    whereStr = " AND a.From_ware_house IN (" + WareStr + ")";
                if (!string.IsNullOrEmpty(timeStr))
                {
                    string Timebigen = DateTime.Now.AddDays(-Convert.ToInt32(timeStr)).ToString();
                    string Timeend = DateTime.Now.ToString();
                    whereStr += " AND a.add_time>UNIX_TIMESTAMP("+"'" + Timebigen +"'"+ ") AND add_time<UNIX_TIMESTAMP(" +"'"+ Timeend +"'"+ ")";
                }

                //3.执行查询
                var Service = new AllocateService();
                IList<JxErpAllocateDtoExt> list = Service.QueryAllList(config, whereStr);

                ShowRunningLog(string.Format("时间：{0}  查询到{1}条数据。\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    list.Count));
                WriteLog(string.Format("时间：{0}  查询到{1}条数据。\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    list.Count));

                //4.标记状态 status 10为缺货 11为不缺货 
                IList<JxErpAllocateDtoExt> result = Service.UpdateAlllist(config, list);
                ShowRunningLog(string.Format("时间：{0}  修改了{1}条数据。\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    result[0].successQty));
                ShowRunningLog("执行结束。\r\n=================================\r\n");
                WriteLog(string.Format("时间：{0}  修改了{1}条数据。\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    result[0].successQty));

                runTaskResult.Success = true;
                runTaskResult.Result = "服务执行成功";
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
                CustomConfig.Add("数据库连接字符串", Task.Common.Parameters.TestDBConnectionString0213);
            if (!CustomConfig.ContainsKey("各仓仓库代码配置"))
                CustomConfig.Add("各仓仓库代码配置", "1,2,3,4,5,6");
            if (!CustomConfig.ContainsKey("查询数据间隔隔段（天）"))
                CustomConfig.Add("查询数据间隔隔段（天）", "60");
            if (!CustomConfig.ContainsKey("EmailAddress"))
                CustomConfig.Add("EmailAddress", "niuwenjiang@jiuxian.com");
            return CustomConfig;
        }
    }
}
