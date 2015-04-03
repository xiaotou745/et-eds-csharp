using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JiuXianWms.Model;
using Task.Model;
using Task.Service.Impl;
using TaskPlatform.TaskInterface;

namespace AllocateInTransit
{
    public class AllocateInTransitTask : AbstractTask
    {
        /// <summary>
        /// 服务名称
        /// </summary>
        /// <returns></returns>
        public override string TaskName()
        {
            return "调拨运单收货确认服务";
        }
        /// <summary>
        /// 服务内容描述
        /// </summary>
        /// <returns></returns>
        public override string TaskDescription()
        {
            return "调拨运单收货确认服务";
        }
        public override RunTaskResult RunTask()
        {
            RunTaskResult runTaskResult = new RunTaskResult();
            IList<JxErpAllocateSyncDto> erpAllocate;
            try
            {
                ShowRunningLog(TaskName() + "开始执行...\r\n时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n");
                //1.获取配置参数
                var config = new Config
                {
                    ConnectionString = CustomConfig["wms数据库连接字符串"],
                };
                var configTwo = new Config()
                {
                    ConnectionString = CustomConfig["tms数据库连接字符串"],
                };
                string timeStr = CustomConfig["查询数据间隔隔段（天）"];
                DateTime begintime = DateTime.Now.AddDays(Convert.ToDouble(timeStr));
                string whereStr = string.Empty;
                //2.验证配置参数组合查询条件
                if (!string.IsNullOrEmpty(timeStr))
                {
                    whereStr += " AND a.ShippingTime>" + "'" + begintime + "'";
                }
                //3.执行查询
                var Service = new AllocateInTransitService();
                IList<OrdersInTransitDto> list = Service.QueryListAllocateInTransit(configTwo, whereStr);
                erpAllocate = new List<JxErpAllocateSyncDto>();
                string str;
                IList<JxErpAllocateGoodsDto> erpAllocateGoods = new List<JxErpAllocateGoodsDto>();
                JxErpAllocateDtoExt allobj = new JxErpAllocateDtoExt();
                int[] allcount;
                foreach (var ordersInTransitDto in list)
                {
                    //1.根据运单查询出调拨单
                    erpAllocate = Service.QueryAllsyncList(configTwo, ordersInTransitDto);
                    str = string.Empty;
                    foreach (var a in erpAllocate)
                    {
                        if (a.WmsAllocateId > 0)
                        {
                            str += a.WmsAllocateId + ",";
                        }
                    }
                    if (str.Length > 0)
                        str = str.Substring(0, str.Length - 1);
                    //2.查询出调单
                    erpAllocateGoods = Service.QueryAllGoodsList(config, str);
                    int isflag = erpAllocateGoods.Count(p => p.Status == 2);
                    if (isflag == 0 && erpAllocateGoods != null)
                    {
                        allobj.SqlWhere = str;
                        allcount = Service.UpdateAllocateInTransitSync(configTwo, ordersInTransitDto, allobj, erpAllocateGoods.ToList());
                        if (allcount == null)
                        {
                            runTaskResult.Success = false;
                            runTaskResult.Result = "服务执行未失败";
                        }
                        ShowRunningLog(string.Format("时间：{0}  修改TMS调拨运单表{1}条数据。\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            allcount[0] != 0 ? allcount[0] : 0));
                        WriteLog(string.Format("时间：{0}  修改TMS调拨运单表{1}条数据。\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            allcount[0] != 0 ? allcount[0] : 0));
                        ShowRunningLog(string.Format("时间：{0}  修改TMS调拨主表{1}条数据。\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        allcount[1] != 0 ? allcount[1] : 0));
                        WriteLog(string.Format("时间：{0}  修改TMS调拨主表{1}条数据。\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        allcount[1] != 0 ? allcount[1] : 0));
                        ShowRunningLog(string.Format("时间：{0}  修改TMS调拨明细{1}条数据。\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        allcount[2] != 0 ? allcount[2] : 0));
                        WriteLog(string.Format("时间：{0}  修改TMS调拨明细{1}条数据。\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        allcount[2] != 0 ? allcount[2] : 0));
                    }
                }
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
            if (!CustomConfig.ContainsKey("wms数据库连接字符串"))
                CustomConfig.Add("wms数据库连接字符串", Task.Common.Parameters.OnLineDBConnectionString1121);
            if (!CustomConfig.ContainsKey("tms数据库连接字符串"))
                CustomConfig.Add("tms数据库连接字符串", Task.Common.Parameters.DBCONNECT_TMS55);
            if (!CustomConfig.ContainsKey("查询数据间隔隔段（天）"))
                CustomConfig.Add("查询数据间隔隔段（天）", "-7");
            if (!CustomConfig.ContainsKey("EmailAddress"))
                CustomConfig.Add("EmailAddress", "niuwenjiang@jiuxian.com");
            return CustomConfig;
        }
    }

}
