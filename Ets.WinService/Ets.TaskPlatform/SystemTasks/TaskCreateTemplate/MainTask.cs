using System;
using System.Collections.Generic;
using TaskPlatform.TaskInterface;
using System.Resources;
using System.Collections;
using System.Reflection;
using System.IO;
using LuaInterface;
using System.Data;
using System.Net.Mail;
using System.Text;

namespace TaskCreateTemplate
{
    public class MainTask : AbstractTask
    {
        Dictionary<string, string> resources = new Dictionary<string, string>();
        IDBHelper helper;
        private Lua luaMachine = null;
        private DataSet dataSet = new DataSet();
        string exception = string.Empty;

        public MainTask()
        {
            try
            {
                Assembly a = Assembly.GetExecutingAssembly();
                string fileName = Path.GetDirectoryName(a.CodeBase.Replace("file:///", "")) + "\\TaskCreateTemplate.resources";
                ResourceReader resourceReader = new ResourceReader(fileName);
                IDictionaryEnumerator enumerator = resourceReader.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    if (resources.ContainsKey(enumerator.Key.ToString()))
                    {
                        resources[enumerator.Key.ToString()] = (enumerator.Value ?? "").ToString();
                    }
                    else
                    {
                        resources.Add(enumerator.Key.ToString(), (enumerator.Value ?? "").ToString());
                    }
                }
                try
                {
                    if (resources["DBHelperName"] == "MySQLDBHelper")
                    {
                        helper = new MySQLDBHelper();
                    }
                    else
                    {
                        helper = new SQLServerDBHelper();
                    }
                }
                catch
                {
                    helper = new SQLServerDBHelper();
                }
            }
            catch (Exception ex)
            {
                exception += ex.ToString();
            }
        }

        /// <summary>
        /// 获取计划任务友好名称
        /// </summary>
        /// <returns></returns>
        public override string TaskName()
        {
            try
            {
                return resources["DisplayName"];
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// 获取计划任务的描述
        /// </summary>
        /// <returns></returns>
        public override string TaskDescription()
        {
            try
            {
                return resources["DisplayName"] + "(本计划任务为计划任务平台创建)" + exception;
            }
            catch
            {
                return exception;
            }
        }

        /// <summary>
        /// 执行计划任务
        /// </summary>
        /// <returns></returns>
        public override RunTaskResult RunTask()
        {
            RunTaskResult runResult = new RunTaskResult();
            runResult.Success = false;
            ShowRunningLog("开始执行计划任务");
            try
            {
                helper.ConnectionString = resources["ConnectionString"];
                ShowRunningLog("开始执行查询……");
                string excption = "";
                try
                {
                    dataSet = helper.Query(resources["SQL"]);
                }
                catch (Exception e)
                {
                    excption = e.ToString();
                }
                ShowRunningLog("查询结束，开始执行脚本……");
                if (luaMachine == null)
                {
                    ShowRunningLog("初始化Lua解释器……");
                    luaMachine = new Lua();
                    ShowRunningLog("注册函数……");
                    luaMachine.RegisterFunction("GetTableValue", this, typeof(MainTask).GetMethod("GetTableValue"));
                    luaMachine.RegisterFunction("SendMailTo", this, typeof(MainTask).GetMethod("SendMailTo"));
                    luaMachine.RegisterFunction("GetTableRowsCount", this, typeof(MainTask).GetMethod("GetTableRowsCount"));
                    luaMachine.RegisterFunction("GetTableColumnsCount", this, typeof(MainTask).GetMethod("GetTableColumnsCount"));
                    luaMachine.RegisterFunction("GetTableContent", this, typeof(MainTask).GetMethod("GetTableContent"));
                    luaMachine.RegisterFunction("GetNowTime", this, typeof(MainTask).GetMethod("GetNowTime"));
                    luaMachine.RegisterFunction("GetDiffTime", this, typeof(MainTask).GetMethod("GetDiffTime"));
                }
                if (dataSet == null || dataSet.Tables.Count < 1)
                {
                    DataTable[] dataTables = new DataTable[0];
                    luaMachine["TableCount"] = 0;
                    luaMachine["Tables"] = dataTables;
                }
                else
                {
                    DataTable[] dataTables = new DataTable[dataSet.Tables.Count];
                    dataSet.Tables.CopyTo(dataTables, 0);
                    luaMachine["TableCount"] = dataSet.Tables.Count;
                    luaMachine["Tables"] = dataTables;
                }
                luaMachine["Exception"] = excption;
                ShowRunningLog("开始执行脚本……");
                luaMachine.DoString(resources["LuaScript"]);
                runResult.Success = true;
                runResult.Result = "脚本执行结束。脚本返回值为：" + (luaMachine["Result"] ?? "").ToString();
            }
            catch (Exception ex)
            {
                ShowRunningLog("啊哦，异常了……");
                runResult.Result = ex.ToString();
                luaMachine.Dispose();
                luaMachine = null;
            }
            ShowRunningLog("执行结束。");
            return runResult;
        }

        public string GetNowTime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public string GetDiffTime(int year, int month, int day, int hour, int minute, int second)
        {
            return DateTime.Now.AddYears(year).AddMonths(month).AddDays(day).AddHours(hour).AddMinutes(minute).AddSeconds(second).ToString("yyyy-MM-dd HH:mm:ss");
        }

        public int GetTableRowsCount(int tableIndex)
        {
            try
            {
                ShowRunningLog(string.Format("脚本请求GetTableRowsCount({0})", tableIndex));
                return dataSet.Tables[tableIndex].Rows.Count;
            }
            catch
            {
                return 0;
            }
        }

        public int GetTableColumnsCount(int tableIndex)
        {
            try
            {
                ShowRunningLog(string.Format("脚本请求GetTableColumnsCount({0})", tableIndex));
                return dataSet.Tables[tableIndex].Columns.Count;
            }
            catch
            {
                return 0;
            }
        }

        public string GetTableValue(int tableIndex, int rowIndex, int columnIndex)
        {
            ShowRunningLog(string.Format("脚本请求GetTableValue({0},{1},{2})", tableIndex, rowIndex, columnIndex));
            return dataSet.Tables[tableIndex].Rows[rowIndex][columnIndex].ToString();
        }

        public string GetTableContent(int tableIndex)
        {
            ShowRunningLog(string.Format("脚本请求GetTableContent({0})", tableIndex));
            try
            {
                StringBuilder sbContent = new StringBuilder();
                sbContent.Append(@"<table style='border-collapse: collapse;' cellspacing='0' cellpadding='0' border='0'>
        <thead align='center'>
            <tr>");
                DataTable table = dataSet.Tables[tableIndex];
                foreach (DataColumn dc in table.Columns)
                {
                    sbContent.Append(string.Format(@"<td style='border-top: windowtext 0.5pt solid; height: 13.5pt; border-right: windowtext 0.5pt solid;
                    border-bottom: windowtext 0.5pt solid; border-left: windowtext 0.5pt solid; background-color: transparent;'>{0}</td>", dc.ColumnName));
                }
                sbContent.Append(@"</tr>
        </thead>
        <tbody align='center'>");
                foreach (DataRow row in table.Rows)
                {
                    sbContent.Append(@"<tr height='18'>");
                    foreach (DataColumn dc in table.Columns)
                    {
                        object obj = row[dc.ColumnName] == DBNull.Value ? "" : row[dc.ColumnName];
                        obj = (obj == null ? "" : obj.ToString());
                        sbContent.Append(string.Format(@"<td style='border-top: windowtext 0.5pt solid; height: 13.5pt; border-right: windowtext 0.5pt solid;
                    border-bottom: windowtext 0.5pt solid; border-left: windowtext 0.5pt solid; background-color: transparent;'>{0}</td>", (obj == null ? "" : obj.ToString()).Replace("&", "&amp;")
                            .Replace("<", "&lt;")
                            .Replace(">", "&gt;")
                            .Replace("'", "&apos;")
                            .Replace("\"", "&quot;")
                            .Replace(" ", "&nbsp;")
                            .Replace("©", "&copy;")
                            .Replace("®", "&reg;")));
                    }
                    sbContent.Append(@"</tr>");
                }
                sbContent.Append(@"</tbody>
    </table>");
                return sbContent.ToString();
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        public void SendMailTo(string subject, string maillAddresses, string mailBody)
        {
            try
            {
                ShowRunningLog(string.Format("脚本请求SendMailTo(\"{0}\",\"{1}\",\"<参数>({2})\")", subject, maillAddresses, mailBody.Length));
                MailAddress from = new MailAddress("wang.xudan@etaostars.com", "计划任务平台");
                MailMessage mail = new MailMessage();
                mail.Subject = subject;
                mail.From = from;
                string address = "";
                string displayName = "";
                string[] mailNames = maillAddresses.Split(';');
                foreach (string name in mailNames)
                {
                    if (name != string.Empty)
                    {
                        if (name.IndexOf('<') > 0)
                        {
                            displayName = name.Substring(0, name.IndexOf('<'));
                            address = name.Substring(name.IndexOf('<') + 1).Replace('>', ' ');
                        }
                        else
                        {
                            displayName = string.Empty;
                            address = name.Substring(name.IndexOf('<') + 1).Replace('>', ' ');
                        }
                        mail.To.Add(new MailAddress(address, displayName));
                    }
                }
                mail.Body = "<div><font size=\"2\">" + mailBody + "</font></div>";
                mail.BodyEncoding = System.Text.Encoding.UTF8;
                File.WriteAllText("123.log", "<div><font size=\"2\">" + mailBody + "</font></div>");
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.Normal;
                mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                SmtpClient client = new SmtpClient();
                client.Host = "smtp.exmail.qq.com";
                client.Port = 25;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential("wang.xudan@etaostars.com", "asd123");
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                ShowRunningLog("开始发送……");
                client.Send(mail);
            }
            catch (Exception ex)
            {
                ShowRunningLog(ex.ToString());
            }
        }
    }
}
