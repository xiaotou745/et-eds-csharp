using System;
using System.Collections.Generic;
using Task.Model;
using Task.Service.Impl;
using TaskPlatform.TaskInterface;

namespace InventoryLog
{
    public class InventoryLogTask : AbstractTask
    {
        /// <summary>
        ///     服务名称
        /// </summary>
        /// <returns></returns>
        public override string TaskName()
        {
            return "酒仙网WMS库存日志服务";
        }

        /// <summary>
        ///     服务描述
        /// </summary>
        /// <returns></returns>
        public override string TaskDescription()
        {
            return "记录erp系统中jx_erp_goods_ware表中的数据变化的日志。";
        }

        public override RunTaskResult RunTask()
        {
            var taskResult = new RunTaskResult();
            try
            {
                ShowRunningLog(TaskName() + "开始执行...\r\n时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n");

                var config = new Config
                {
                    ConnectionString = CustomConfig["数据库连接字符串读"],
                };

                var inventoryservice = new InventoryService();
                IList<InvInventoryLog> list = inventoryservice.QueryList(config);

                ShowRunningLog(string.Format("时间：{0}  查询到{1}条数据。\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    list.Count));
                WriteLog(string.Format("时间：{0}  查询到{1}条数据。\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    list.Count));
                config.ConnectionString = CustomConfig["数据库连接字符串写"];
                int count = inventoryservice.Insert(list, config);
                ShowRunningLog(string.Format("时间：{0}  写入了{1}条数据。\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    count));
                ShowRunningLog("执行结束。\r\n=================================\r\n");
                WriteLog(string.Format("时间：{0}  写入了{1}条数据。\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    count));

                taskResult.Success = true;
            }
            catch (Exception ex)
            {
                //SendEmailTo(TaskName() + ex, CustomConfig["EmailAddress"]);
                taskResult.Success = false;
                taskResult.Result = ex.ToString();
                ShowRunningLog(ex.ToString());
                WriteLog(ex.ToString());
            }
            return taskResult;
        }

        public override Dictionary<string, string> UploadConfig()
        {
            if (!CustomConfig.ContainsKey("数据库连接字符串写"))
                CustomConfig.Add("数据库连接字符串写",
                    "Server=localhost;Database=jiuxianweb; User=root;Password=jx103203;Use Procedure Bodies=false;Charset=utf8;Allow Zero Datetime=True; Pooling=true; Max Pool Size=50;Port=3306");
            if (!CustomConfig.ContainsKey("数据库连接字符串读"))
                CustomConfig.Add("数据库连接字符串读",
                    "Server=192.168.12.30;Database=jiuxianweb; User=test_db_online;Password=QwerAs131126dfTyui1209;Use Procedure Bodies=false;Charset=utf8;Allow Zero Datetime=True; Pooling=true; Max Pool Size=50;Port=3306");
            //if (!CustomConfig.ContainsKey("执行的存储过程名"))
            //    CustomConfig.Add("执行的存储过程名", "[JiuXian].[WMS].[ProcName]");
            //if (!CustomConfig.ContainsKey("EmailAddress"))
            //    CustomConfig.Add("EmailAddress", "wangxudan@jiuxian.com");
            return CustomConfig;
        }
    }
}