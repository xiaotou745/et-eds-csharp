using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task.Common;
using Task.Model;
using Task.Service.Impl.Purchase;
using TaskPlatform.TaskInterface;

namespace PurchaseEWS
{
    public class PurchaseEWSTask : AbstractTask
    {
        public static string tdStyle = @"valign=""middle"" style=""background-color:transparent;""";
        public static string spanStyle = @"style=""font-size:9pt;font-family:微软雅黑;color:black;""";

        public static string tableStyle =
            @"border=""1"" cellspacing=""0"" cellpadding=""0"" style=""font-family:Simsun;background-color:#FFFFFF;border:none;""";

        public Dictionary<string, string> Purchase24Hour = new Dictionary<string, string>
        {
            {"PurchaseSn", "采购单号"},
            {"eIsStatusString", "状态"},
            {"Accept_name", "收货人"},
            {"AcceptTimeString", "收货时间"},
            {"AcceptBak", "收货备注"},
        };

        public Dictionary<string, string> Purchase7Day = new Dictionary<string, string>
        {
            {"PurchaseSn", "采购单号"},
            {"SupplierName", "供应商名称"},
            {"Purchaser", "采购人"},
            {"AddTimeString", "下单时间"},
            {"PurchaseBak", "备注"},
        };

        public string cc = null; // "niuwenjiang@jiuxian.com,chujianbin@jiuxian.com,caoguisheng@jiuxian.com";
        public string testEmailAddress = "huxiaobing@jiuxian.com";

        public override string TaskName()
        {
            return "采购收验货预警服务";
        }

        public override string TaskDescription()
        {
            return @"1.采购单下单后7天仓库无收货操作时，将会发送邮件给相关负责人；
2.采购单做收货操作后24小时无验货操作时，将会发送邮件给相关负责人。";
        }

        public override RunTaskResult RunTask()
        {
            var taskResult = new RunTaskResult();
            try
            {
                ShowRunningLog(TaskName() + "开始执行...\r\n时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n");
                WriteLog(TaskName() + "开始执行...\r\n时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n");

                var config = new Config
                {
                    ConnectionString = CustomConfig["DataBaseConnectionString"],
                };
                // 采购单状态 =0 表示预下单（可修改）  =1 表示确认下单  （不可修改）=2 表示收货 =4 表示已上架
                // 7天无收货操作的
                var modelF = new Purchase
                {
                    SqlWhere =
                        " AND jep.add_time<=" + (int) TimeHelper.ConvertDateTimeToDouble(DateTime.Now.AddDays(-7)) +
                        " and jep.add_time>=1388505600 AND jep.acceptTime=0 AND jep.isStatus = 1 AND  jep.verify IN('5','4','9');",
                };
                // 24小时无验货操作
                var modelS = new Purchase
                {
                    SqlWhere =
                        " AND jep.acceptTime<=" + (int) TimeHelper.ConvertDateTimeToDouble(DateTime.Now.AddDays(-1)) +
                        " and jep.add_time>=1388505600 AND  jep.acceptTime>0 AND jep.checkTime=0 AND jep.isStatus = 2 AND  jep.verify IN('5','4','9');",
                };

                var inventoryservice = new PurchaseEWSService();
                IList<Purchase> listF = inventoryservice.QueryPurchases(modelF, config);
                IList<Purchase> listS = inventoryservice.QueryPurchases(modelS, config);

                ShowRunningLog(string.Format("时间：{0}\r\n  1.查询到{1}个采购单超过7天无收货操作；\r\n2.查询到{2}个采购单超过24小时无验货操作。\r\n",
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    listF.Count, listS.Count));
                WriteLog(string.Format("时间：{0}\r\n  1.查询到{1}个采购单超过7天无收货操作；\r\n2.查询到{2}个采购单超过24小时无验货操作。\r\n",
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    listF.Count, listS.Count));

                // 发送邮件
                SendEmailByWareHouse(listF, listS);

                ShowRunningLog("执行结束。\r\n=================================\r\n");
                WriteLog("执行结束。\r\n=================================\r\n");

                taskResult.Success = true;
                taskResult.Result = "执行结束！";
            }
            catch (Exception ex)
            {
                SendEmailTo(TaskName() + ex, CustomConfig["EmailAddress"]);
                taskResult.Success = false;
                taskResult.Result = ex.ToString();
                ShowRunningLog(ex.ToString());
                WriteLog(ex.ToString());
            }
            return taskResult;
        }


        public override Dictionary<string, string> UploadConfig()
        {
            if (!CustomConfig.ContainsKey("DataBaseConnectionString"))
                CustomConfig.Add("DataBaseConnectionString",
                    "Server=192.168.11.21;Database=jiuxianweb; User=select_limit;Password=Only_in_jx_select;Use Procedure Bodies=false;Charset=utf8;Allow Zero Datetime=True; Pooling=true; Max Pool Size=50;Port=3306");
            // 收货组
            if (!CustomConfig.ContainsKey("北京仓-采购收货预警"))
                CustomConfig.Add("北京仓-采购收货预警", testEmailAddress);
            if (!CustomConfig.ContainsKey("广州仓-采购收货预警"))
                CustomConfig.Add("广州仓-采购收货预警", testEmailAddress);
            if (!CustomConfig.ContainsKey("上海仓-采购收货预警"))
                CustomConfig.Add("上海仓-采购收货预警", testEmailAddress);
            if (!CustomConfig.ContainsKey("武汉仓-采购收货预警"))
                CustomConfig.Add("武汉仓-采购收货预警", testEmailAddress);
            if (!CustomConfig.ContainsKey("天津仓-采购收货预警"))
                CustomConfig.Add("天津仓-采购收货预警", testEmailAddress);
            if (!CustomConfig.ContainsKey("成都仓-采购收货预警"))
                CustomConfig.Add("成都仓-采购收货预警", testEmailAddress);
            // 验货组
            if (!CustomConfig.ContainsKey("北京仓-采购验货货预警"))
                CustomConfig.Add("北京仓-采购验货货预警", testEmailAddress);
            if (!CustomConfig.ContainsKey("广州仓-采购验货货预警"))
                CustomConfig.Add("广州仓-采购验货货预警", testEmailAddress);
            if (!CustomConfig.ContainsKey("上海仓-采购验货货预警"))
                CustomConfig.Add("上海仓-采购验货货预警", testEmailAddress);
            if (!CustomConfig.ContainsKey("武汉仓-采购验货货预警"))
                CustomConfig.Add("武汉仓-采购验货货预警", testEmailAddress);
            if (!CustomConfig.ContainsKey("天津仓-采购验货货预警"))
                CustomConfig.Add("天津仓-采购验货货预警", testEmailAddress);
            if (!CustomConfig.ContainsKey("成都仓-采购验货货预警"))
                CustomConfig.Add("成都仓-采购验货货预警", testEmailAddress);
            if (!CustomConfig.ContainsKey("采购收货预警抄送"))
                CustomConfig.Add("采购收货预警抄送", cc);
            if (!CustomConfig.ContainsKey("采购验货预警抄送"))
                CustomConfig.Add("采购验货预警抄送", cc);


            if (!CustomConfig.ContainsKey("WareHouse"))
                CustomConfig.Add("WareHouse", "1,北京仓|2,广州仓|3,上海仓|4,武汉仓|5,天津仓|6,成都仓");

            // 文件保存路径
            if (!CustomConfig.ContainsKey("Path"))
                CustomConfig.Add("Path", @"D:\WMSSaveFile");

            return CustomConfig;
        }

        public void SendEmailByWareHouse(IList<Purchase> listF, IList<Purchase> listS)
        {
            Dictionary<int, string> temp = GetWareList();
            if (temp == null)
                return;
            // 平行发邮件
            Parallel.For(1, temp.Count + 1, index => SendEmailByParallel(listF, listS, index, temp[index]));
        }

        /// <summary>
        /// 平行发邮件。一系列的操作都在这里。按仓库筛选、附件发送等等。
        /// </summary>
        /// <param name="listF"></param>
        /// <param name="listS"></param>
        /// <param name="index"></param>
        /// <param name="name"></param>
        public void SendEmailByParallel(IList<Purchase> listF, IList<Purchase> listS, int index, string name)
        {
            try
            {
                string path;
                if (listF != null && listF.Count > 0)
                {
                    List<Purchase> f = listF.Where(c => c.WareId.Equals(index)).ToList();
                    if (f.Count > 0)
                    {
                        var cc = GetCc(f);
                        if (f.Count > Task.Common.Parameters.MaxCountForEWS)
                        {
                            SendEmailTo(
                               CreateHtmlBody(f, EnumEWSType.Purchase7Day, name, out path),
                               CustomConfig[name + "-采购收货预警"], name + "-采购单未收货预警邮件",
                               cc, true, name + "-采购单未收货预警邮件.xls", Purchase7Day, f);
                        }
                        else
                        {
                            SendEmailTo(
                                CreateHtmlBody(f, EnumEWSType.Purchase7Day, name, out path),
                                CustomConfig[name + "-采购收货预警"], name + "-采购单未收货预警邮件",
                                cc, true);
                        }
                        ShowRunningLog(string.Format("时间：{0}  {1}-采购单7天未收货邮件发送成功。\r\n",
                            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), name));
                        WriteLog(string.Format("时间：{0}  {1}-采购单7天未收货邮件发送成功。\r\n",
                            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), name));
                    }
                }
                if (listS != null && listS.Count > 0)
                {
                    List<Purchase> f = listS.Where(c => c.WareId.Equals(index)).ToList();
                    if (f.Count > 0)
                    {
                        if (f.Count > Task.Common.Parameters.MaxCountForEWS)
                        {
                            SendEmailTo(
                                CreateHtmlBody(f, EnumEWSType.Purchase24Hour, name, out path),
                                CustomConfig[name + "-采购验货货预警"], name + "-采购单未完成验货预警邮件",
                                CustomConfig["采购验货预警抄送"], true, name + "-采购单未收货预警邮件.xls", Purchase24Hour, f);
                        }
                        else
                        {
                            SendEmailTo(
                                CreateHtmlBody(f, EnumEWSType.Purchase24Hour, name, out path),
                                CustomConfig[name + "-采购验货货预警"], name + "-采购单未完成验货预警邮件",
                                CustomConfig["采购验货预警抄送"], true);
                        }
                        ShowRunningLog(string.Format("时间：{0}  {1}-采购单24小时未验货邮件发送成功。\r\n",
                            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), name));
                        WriteLog(string.Format("时间：{0}  {1}-采购单24小时未验货邮件发送成功。\r\n",
                            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            name));
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取采购抄送列表。
        /// 2014年5月13日14:37:07
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public string GetCc(List<Purchase> list)
        {
            List<string> emaiList = new List<string>();
            foreach (var purchase in list)
            {
                if (emaiList.Count == 0)
                    emaiList.Add(purchase.Email);
                if (!emaiList.Contains(purchase.Email))
                    emaiList.Add(purchase.Email);
            }
            return string.IsNullOrEmpty(CustomConfig["采购收货预警抄送"])
                ? string.Join(",", emaiList)
                : CustomConfig["采购收货预警抄送"] + "," + string.Join(",", emaiList);
        }

        /// <summary>
        ///     获取仓库信息。
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, string> GetWareList()
        {
            try
            {
                var dic = new Dictionary<int, string>();
                string wareList = CustomConfig["WareHouse"];
                if (wareList == null) return null;
                string[] w = wareList.Split(new[] {"|"}, StringSplitOptions.RemoveEmptyEntries);
                if (w == null) return null;
                foreach (string s in w)
                {
                    string[] content = s.Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries);
                    if (content == null)
                        return null;
                    int num;
                    if (int.TryParse(content[0], out num))
                        if (!dic.ContainsKey(num)) dic.Add(num, content[1]);
                }
                return dic;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        ///     获取内容和一些其他的操作。比如路径，和附件等等。
        /// </summary>
        /// <param name="tList"></param>
        /// <param name="type"></param>
        /// <param name="wareHouse"></param>
        /// <returns></returns>
        public string CreateHtmlBody(IList<Purchase> tList, EnumEWSType type, string wareHouse, out string path)
        {
            path = string.Empty;
            if (tList == null)
                return null;
            if (tList.Count == 0)
                return null;
            var builder = new StringBuilder(Task.Common.Parameters.SystemMsgForEWS);
            builder.Append(string.Format(@"<br /><p class=""MsoNormal"" style=""text-align:justify;margin-left:0pt;text-indent:10pt;font-size:10.5pt;font-family:Calibri;background-color:#FFFFFF;""><span style=""font-size:9pt;font-family:微软雅黑;color:black;"">各位好：</span><span style=""font-size:9pt;font-family:微软雅黑;color:black;""></span></p><p class=""MsoNormal"" style=""text-align:justify;margin-left:0pt;text-indent:10pt;font-size:10.5pt;font-family:Calibri;background-color:#FFFFFF;""><span style=""font-size:9pt;font-family:微软雅黑;color:black;"">{0}</span><span style=""font-size:9pt;font-family:微软雅黑;color:black;""></span>
",
                "共" + tList.Count + "个采购单，" +
                (type == EnumEWSType.Purchase7Day
                    ? "采购单下单7天后没有后续操作，请帮忙确认采购单是否已经到达库房，如果已经收货，请及时在WMS进行收货确认的操作。谢谢。"
                    : "采购单收货后24小时未完成验货操作，请及时在WMS系统对采购单进行相应的操作。谢谢。")));

            IList<Purchase> temp = null;
            var description = new Dictionary<string, string>();
            switch (type)
            {
                case EnumEWSType.Purchase7Day:
                    temp = tList.OrderBy(c => c.Add_time).ToList();
                    description = Purchase7Day;
                    break;
                case EnumEWSType.Purchase24Hour:
                    temp = tList.OrderBy(c => c.AcceptTime).ToList();
                    description = Purchase24Hour;
                    break;
            }
            if (temp != null)
            {
                if (temp.Count > Task.Common.Parameters.MaxCountForEWS)
                {
                    builder.Append(Task.Common.Parameters.SystemMsgAttachment);
                }
                builder.Append(SendEmailCommon.CreateHtmlBody(description, temp));
            }
            return builder.ToString();
        }
    }
}