using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Data.SqlClient;
using System.Data;

namespace TaskPlatform.PlatformLog
{
    /// <summary>
    /// 通用日志工具
    /// </summary>
    public class Log
    {
        /// <summary>
        /// 日志写入器锁
        /// </summary>
        private static object WriterFileLock = "";
        private static string spliter = "".PadRight(8, '=');

        public static string TaskPlatformDBConnectionString { get; set; }
        private static List<string> CreatedTables = new List<string>();

        private static string _logBasePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase.Remove(0, 8).Replace("/", "\\")).TrimEnd('\\') + "\\Logs";
        private static string queueLock = "queueLock";
        private static Queue<Dictionary<string, object>> queue = new Queue<Dictionary<string, object>>();
        /// <summary>
        /// 获取或设置一个目录该目录作为日志工具的工作目录。日志工具将在该目录中每天建立一个子目录进行日志的写入。
        /// </summary>
        public static string LogBasePath
        {
            get
            {
                return _logBasePath;
            }
            set
            {
                if (!Directory.Exists(value))
                {
                    throw new ArgumentException("指定的路径不存在，请创建后重新指定。");
                }
                _logBasePath = value;
            }
        }

        /// <summary>
        /// 将指定内容写入日志
        /// </summary>
        /// <param name="title">日志标题</param>
        public static void Write(string content)
        {
            Write("", content, DateTime.Now);
        }

        /// <summary>
        /// 将指定内容写入日志
        /// </summary>
        /// <param name="title">日志标题</param>
        /// <param name="taskName">计划任务名称</param>
        public static void Write(string content, string taskName, bool withHeader = true)
        {
            Write("", content, DateTime.Now, LogType.Tasks, taskName: taskName, withHeader: withHeader);
        }

        /// <summary>
        /// 将指定内容写入日志
        /// </summary>
        /// <param name="title">日志标题</param>
        /// <param name="taskName">计划任务名称</param>
        public static void Write(string title, string content, string taskName)
        {
            Write(title, content, DateTime.Now, LogType.Tasks, taskName: taskName);
        }

        /// <summary>
        /// 将指定内容写入指定日志
        /// </summary>
        /// <param name="title">日志标题</param>
        /// <param name="logName">日志文件名称(只有指定日志类型logType为Custom时，该参数才有效。)</param>
        public static void CustomWrite(string content, string logName)
        {
            CustomWrite("", content, logName);
        }

        /// <summary>
        /// 将指定内容写入指定日志
        /// </summary>
        /// <param name="title">日志标题</param>
        /// <param name="content">日志内容</param>
        /// <param name="logName">日志文件名称(只有指定日志类型logType为Custom时，该参数才有效。)</param>
        public static void CustomWrite(string title, string content, string logName)
        {
            Write(title, content, DateTime.Now, LogType.Custom, logName);
        }

        /// <summary>
        /// 将指定内容写入日志
        /// </summary>
        /// <param name="title">日志标题</param>
        /// <param name="content">日志内容</param>
        /// <param name="dateTime">事件发生时间</param>
        /// <param name="logType">日志类型</param>
        /// <param name="logName">日志文件名称(只有指定日志类型logType为Custom时，该参数才有效。)</param>
        /// <param name="taskName">计划任务名称</param>
        public static void Write(string title, string content, DateTime dateTime, LogType logType = LogType.System, string logName = "", string taskName = "", bool withHeader = true)
        {
            LogObject log = new LogObject();
            log.Title = title;
            log.Content = content;
            log.LogName = logName;
            log.TaskName = taskName;
            log.LogType = logType;
            log.DateTime = dateTime;
            Write(log, withHeader);
        }

        /// <summary>
        /// 将指定内容写入日志.
        /// </summary>
        /// <param name="log">The log.</param>
        public static void Write(LogObject log, bool withHeader = true)
        {
            Dictionary<string, object> list = new Dictionary<string, object>();
            list.Add("log", log);
            list.Add("withHeader", withHeader);
            lock (queueLock)
            {
                queue.Enqueue(list);
            }
            // 提交日志到日志写入器--文件
            ThreadPool.QueueUserWorkItem(LogSubmitToFile);
            if (log.LogType == LogType.Tasks)
            {
                // 提交日志到日志写入器--DB
                ThreadPool.QueueUserWorkItem(LogSubmitToDB, log);
            }
        }

        /// <summary>
        /// 日志写入器。传入LogObject对象，由该写入器统一访问文件持久层并写入日志。
        /// </summary>
        /// <param name="args"></param>
        private static void LogSubmitToFile(object args)
        {
            Dictionary<string, object> list = null;
            lock (queueLock)
            {
                list = queue.Dequeue();
            }
            LogObject log = list["log"] as LogObject;
            LogWriteToFile(log, (bool)list["withHeader"]);
        }

        /// <summary>
        /// 日志写入器。该写入器统一访问文件持久层并写入日志。
        /// </summary>
        /// <param name="log"></param>
        public static void LogWriteToFile(LogObject log, bool withHeader = true)
        {
            lock (WriterFileLock)
            {
                try
                {
                    string _logPath = _logBasePath.TrimEnd('\\') + "\\" + DateTime.Now.ToString("yyyy-MM-dd");
                    if (!Directory.Exists(_logPath))
                    {
                        Directory.CreateDirectory(_logPath);
                    }
                    string logFile = _logPath + "\\{0}.log";
                    if (log.LogType == LogType.System)
                    {
                        logFile = string.Format(logFile, log.LogType.ToString());
                    }
                    else if (log.LogType == LogType.Tasks)
                    {
                        logFile = string.Format(logFile, log.LogType.ToString() + "--" + log.TaskName);
                    }
                    else if (log.LogType == LogType.Custom)
                    {
                        logFile = string.Format(logFile, log.LogType.ToString() + "--" + log.LogName);
                    }
                    string splitRow = spliter + " " + log.DateTime.ToString("yyyy-MM-dd HH:mm:ss") + " " + spliter;
                    try
                    {
                        using (StreamWriter streamWriter = new StreamWriter(logFile, true, Encoding.Default))
                        {
                            if (withHeader)
                            {
                                // 写入首行
                                streamWriter.WriteLine(splitRow);
                            }
                            // 写入标题
                            if (!string.IsNullOrWhiteSpace(log.Title))
                            {
                                streamWriter.WriteLine("    Title--" + log.Title);
                            }
                            streamWriter.Write(streamWriter.NewLine);
                            // 写入内容
                            streamWriter.Write(log.Content);
                            if (withHeader)
                            {
                                streamWriter.Write(streamWriter.NewLine);
                            }
                            if (withHeader)
                            {
                                // 写入尾行
                                streamWriter.WriteLine("".PadRight(splitRow.Length, '='));
                            }
                            streamWriter.Write(streamWriter.NewLine);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }
                        try
                        {
                            FileInfo fileInfo = new FileInfo(logFile);
                            // 文件大于20MB则自动截断
                            if (fileInfo.Length > 20971520L)
                            {
                                fileInfo.MoveTo(logFile + "." + DateTime.Now.ToString("HHmmss"));
                            }
                            fileInfo = null;
                        }
                        catch { }
                    }
                    catch
                    { }
                }
                catch { }
            }
        }

        /// <summary>
        /// 日志写入器。传入LogObject对象，由该写入器统一访问DB持久层并写入日志。
        /// </summary>
        /// <param name="args"></param>
        private static void LogSubmitToDB(object args)
        {
            LogObject log = args as LogObject;
            LogWriteToDB(log);
        }

        /// <summary>
        /// 日志写入器。该写入器统一访问DB持久层并写入日志。
        /// </summary>
        /// <param name="log"></param>
        public static void LogWriteToDB(LogObject log)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                if (!CreatedTables.Contains(log.TaskName))
                {
                    strSql.Append(SQLResource.CreateTableSQL);
                    strSql.AppendLine("");
                }
                strSql.Append("insert into Log.[#TableName#](");
                strSql.Append("TrigTime,ThreadQueueTime,StartExecuteTime,ExecuteTime,EndExecuteTime,IsCancle,IsSuccess,RunTaskResult,LogTime)");
                strSql.Append(" values (");
                strSql.Append("@TrigTime,@ThreadQueueTime,@StartExecuteTime,@ExecuteTime,@EndExecuteTime,@IsCancle,@IsSuccess,@RunTaskResult,GETDATE())");
                strSql.Append(";select @@IDENTITY");
                SqlParameter[] parameters = {
					new SqlParameter("@TrigTime", SqlDbType.DateTime),
					new SqlParameter("@ThreadQueueTime", SqlDbType.VarChar,100),
					new SqlParameter("@StartExecuteTime", SqlDbType.DateTime),
					new SqlParameter("@ExecuteTime", SqlDbType.VarChar,100),
					new SqlParameter("@EndExecuteTime", SqlDbType.DateTime),
					new SqlParameter("@IsCancle", SqlDbType.Bit,1),
					new SqlParameter("@IsSuccess", SqlDbType.Bit,1),
					new SqlParameter("@RunTaskResult", SqlDbType.Text),
					new SqlParameter("@TableDescription", SqlDbType.NVarChar)};
                parameters[0].Value = log.TrigTime ?? DateTime.MinValue;
                parameters[1].Value = log.ThreadQueueTime ?? "";
                parameters[2].Value = log.StartExecuteTime ?? DateTime.MinValue;
                parameters[3].Value = log.ExecuteTime ?? "";
                parameters[4].Value = log.EndExecuteTime ?? DateTime.MinValue;
                parameters[5].Value = log.IsCancle;
                parameters[6].Value = log.IsSuccess;
                parameters[7].Value = log.RunTaskResult ?? "";
                parameters[8].Value = log.Title ?? "";

                using (SqlConnection conn = new SqlConnection(TaskPlatformDBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(strSql.ToString(), conn))
                    {
                        cmd.CommandText = cmd.CommandText.Replace("#TableName#", log.TaskName);
                        conn.Open();
                        cmd.Parameters.AddRange(parameters);
                        cmd.ExecuteNonQuery();
                        if (!CreatedTables.Contains(log.TaskName))
                        {
                            CreatedTables.Add(log.TaskName);
                        }
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// 收缩日志文件
        /// </summary>
        /// <param name="shrinkAllBeforeToday">是否收缩今天之前的所有日志。是：收缩今天之前的所有日志；否：只收缩昨天的所有日志</param>
        public static void ShrinkLog(bool shrinkAllBeforeToday = true)
        {
            DateTime tmpDate = DateTime.Now;
            if (!Directory.Exists(_logBasePath + "\\ShrinkedLog"))
            {
                Directory.CreateDirectory(_logBasePath + "\\ShrinkedLog");
            }
            Directory.GetDirectories(_logBasePath).ToList().ForEach(path =>
            {
                try
                {
                    string pathName = path.Substring(path.LastIndexOf('\\') + 1);
                    if (DateTime.TryParse(pathName, out tmpDate) && tmpDate < DateTime.Now.Date)
                    {
                        ZipHelper.ZipFileDirectory(path, _logBasePath + "\\ShrinkedLog\\" + pathName + ".zip");
                        Directory.Delete(path, true);
                    }
                }
                catch { }
            });
        }
    }
}
