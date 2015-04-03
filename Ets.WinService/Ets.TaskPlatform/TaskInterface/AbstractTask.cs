using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Remoting.Channels;
using System.Collections;
using System.Runtime.Remoting;
using System.Threading;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Net.Mail;
using System.IO;
using Task.Common;

namespace TaskPlatform.TaskInterface
{
    /// <summary>
    /// 计划任务抽象类。
    /// <para>除必须实现的函数外，你可以：</para>
    /// <para>1、可调用ShowRunningLog来显示实时运行信息</para>
    /// <para>2、可调用WriteLog和CustomWriteLog函数写日志；</para>
    /// <para>3、重写TaskKey函数重新指定Key值；</para>
    /// <para>4、重写InitTask函数来自定义初始化计划任务；</para>
    /// <para>5、重写UploadConfig、DownloadConfig来管理自定义配置项；</para>
    /// </summary>
    [ComVisibleAttribute(true)]
    public abstract class AbstractTask : MarshalByRefObject, ITask
    {
        #region 成员属性
        private Dictionary<string, string> _platformAppSettings = new Dictionary<string, string>();

        /// <summary>
        /// 平台配置项
        /// </summary>
        public Dictionary<string, string> PlatformAppSettings
        {
            get { return _platformAppSettings; }
            set
            {
                _platformAppSettings = value;
                if (_platformAppSettings == null)
                    _platformAppSettings = new Dictionary<string, string>();
            }
        }

        private Dictionary<string, string> _platformConnectionStrings = new Dictionary<string, string>();

        /// <summary>
        /// 平台连接字符串
        /// </summary>
        public Dictionary<string, string> PlatformConnectionStrings
        {
            get { return _platformConnectionStrings; }
            set
            {
                _platformConnectionStrings = value;
                if (_platformConnectionStrings == null)
                    _platformConnectionStrings = new Dictionary<string, string>();
            }
        }

        private Dictionary<string, string> _customConfig = new Dictionary<string, string>();

        /// <summary>
        /// 自定义配置项
        /// </summary>
        public Dictionary<string, string> CustomConfig
        {
            get { return _customConfig; }
            set
            {
                _customConfig = value;
                if (_customConfig == null)
                    _customConfig = new Dictionary<string, string>();
            }
        }

        private string taskName = "";

        private static string _serviceListLocker = "_serviceListLocker";
        private static Dictionary<string, PlatformService> _platformServiceList = null;

        private static Dictionary<string, PlatformService> PlatformServiceList
        {
            get
            {
                if (_platformServiceList == null)
                {
                    lock (_serviceListLocker)
                    {
                        if (_platformServiceList == null)
                        {
                            _platformServiceList = new Dictionary<string, PlatformService>();
                        }
                    }
                }
                return AbstractTask._platformServiceList;
            }
        }

        private Lazy<TimeMachine> _timeMachine = new Lazy<TimeMachine>();
        /// <summary>
        /// 时间管控机器
        /// </summary>
        public Lazy<TimeMachine> TimeMachine
        {
            get { return _timeMachine; }
            set { _timeMachine = value; }
        }

        private bool _loop = false;
        /// <summary>
        /// 是否循环执行
        /// </summary>
        public bool Loop
        {
            get { return _loop; }
            protected set { _loop = value; }
        }

        private long _runningBatch = 0;

        /// <summary>
        /// 执行批次
        /// </summary>
        public long RunningBatch
        {
            get { return _runningBatch; }
            set
            {
                CheckCaller("RunningBatch");
                _runningBatch = value;
            }
        }

        private string _runID = string.Empty;
        /// <summary>
        /// 执行ID
        /// </summary>
        public string RunID
        {
            get { return _runID; }
            set
            {
                CheckCaller("RunID");
                _runID = value;
            }
        }

        /// <summary>
        /// 只允许TaskObject调用。
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        /// <exception cref="System.ArgumentException">限只读。 + propertyName + 属性仅限平台内部使用，应避免在代码中设置。</exception>
        private void CheckCaller(string propertyName)
        {
            System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();
            System.Diagnostics.StackFrame frame = stackTrace.GetFrame(2);
            if (frame.GetMethod().ReflectedType.Name != "TaskObject")
            {
                throw new ArgumentException("限只读。" + propertyName + "属性仅限平台内部使用，应避免在代码中设置。");
            }
        }

        private bool _runFromWeb = false;
        /// <summary>
        /// 获取或设置一个值该值指示本次执行是否从Web端发起。
        /// </summary>
        public bool RunFromWeb
        {
            get { return _runFromWeb; }
            set
            {
                CheckCaller("RunFromWeb");
                _runFromWeb = value;
            }
        }

        #endregion

        #region  实现 ITask 接口

        /// <summary>
        /// 获取计划任务的唯一标识符
        /// </summary>
        /// <returns></returns>
        public virtual string TaskKey()
        {
            return "TaskPlatform.TaskInterface.TaskBase";
        }

        /// <summary>
        /// 获取计划任务友好名称
        /// </summary>
        /// <returns></returns>
        public abstract string TaskName();

        /// <summary>
        /// 获取计划任务的描述
        /// </summary>
        /// <returns></returns>
        public abstract string TaskDescription();

        /// <summary>
        /// 自定义加载程序集
        /// </summary>
        /// <param name="name">程序集名称</param>
        /// <returns></returns>
        public virtual Assembly CustomAssemblyResolve(string name)
        {
            return null;
        }

        /// <summary>
        /// 加载当前程序集
        /// </summary>
        /// <param name="name">程序集名称</param>
        /// <returns></returns>
        protected Assembly GetThisAssembly(string name)
        {
            ShowRunningLog("正在重新加载程序集：" + name);
            if (name.ToLower().Contains(this.GetType().Name.ToLower() + ","))
            {
                return this.GetType().Assembly;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 初始化计划任务时将被执行。平台将通过此函数通知所以计划任务，以便获取平台的相关配置信息。
        /// </summary>
        /// <param name="platformAppSettings"></param>
        /// <param name="platformConnectionStrings"></param>
        public virtual void InitTask(Dictionary<string, string> platformAppSettings, Dictionary<string, string> platformConnectionStrings)
        {
            PlatformAppSettings = platformAppSettings;
            PlatformConnectionStrings = platformConnectionStrings;
            MSMQBusService<string>.RegisterAllBus(platformAppSettings, 1);
        }

        /// <summary>
        /// 返回配置项，以供平台统一配置。该列表将可以通过平台提供的配置编辑器编辑，编辑后由平台调用DownloadConfig函数回发给计划任务。
        /// </summary>
        /// <returns></returns>
        public virtual Dictionary<string, string> UploadConfig()
        {
            return CustomConfig;
        }

        /// <summary>
        /// 接收平台发过来的配置项。
        /// </summary>
        /// <param name="configs">配置项集合</param>
        public virtual void DownloadConfig(Dictionary<string, string> configs)
        {
            if (CustomConfig == null)
            {
                CustomConfig = new Dictionary<string, string>();
            }
            CustomConfig.Clear();
            foreach (var item in configs)
            {
                if (!CustomConfig.ContainsKey(item.Key))
                {
                    CustomConfig.Add(item.Key, item.Value);
                }
            }
        }

        /// <summary>
        /// 执行计划任务
        /// </summary>
        /// <returns></returns>
        public abstract RunTaskResult RunTask();

        #endregion

        #region  连接平台IPC

        private static Mouthpiece _mouthpiece = null;
        /// <summary>
        /// IPC连接器
        /// </summary>
        private static Mouthpiece Mouthpiece
        {
            get
            {
                if (_mouthpiece == null)
                {
                    GetMouthpiece();
                }
                return AbstractTask._mouthpiece;
            }
            set { AbstractTask._mouthpiece = value; }
        }

        /// <summary>
        /// 获取代理
        /// </summary>
        /// <returns></returns>
        private static Mouthpiece GetMouthpiece()
        {
            if (_mouthpiece == null)
            {
                string ipc = string.Empty;
                try
                {
                    ipc = AppDomain.CurrentDomain.GetData("PlatformPortName").ToString();
                }
                catch
                {
                    ipc = "TaskPlatform";
                }
                _mouthpiece = Activator.GetObject(typeof(Mouthpiece), string.Format("ipc://{0}/Controller", ipc)) as Mouthpiece;
            }
            return _mouthpiece;
        }

        /// <summary>
        /// 获取控制此实例的生存期策略的生存期服务对象。
        /// </summary>
        public override Object InitializeLifetimeService()
        {
            // 重写函数，改变租约的期限，设置为永久有效
            ILease lease = (ILease)base.InitializeLifetimeService();
            if (lease.CurrentState == LeaseState.Initial)
            {
                lease.InitialLeaseTime = TimeSpan.FromSeconds(0);
            }
            else if (lease.CurrentState == LeaseState.Null)
            {
                //lease.SponsorshipTimeout = TimeSpan.FromSeconds(10);
                //lease.RenewOnCallTime = TimeSpan.FromSeconds(2);
            }
            return lease;
        }

        #endregion

        #region 向平台写日志

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="title">日志标题</param>
        /// <param name="content">日志内容</param>
        /// <param name="dateTime">日志时间</param>
        /// <param name="logName">日志名称</param>
        /// <param name="throwOnError">当发生异常，是否抛出</param>
        protected void CustomWriteLog(string title, string content, DateTime dateTime, string logName, bool throwOnError = false)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(taskName))
                {
                    taskName = AppDomain.CurrentDomain.GetData("PlatformTaskName").ToString();
                }
                Mouthpiece.CustomWrite(title, content, dateTime, logName, taskName);
            }
            catch
            {
                if (throwOnError)
                    throw;
            }
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="content">日志内容</param>
        /// <param name="throwOnError">当发生异常，是否抛出</param>
        protected void WriteLog(string content, bool throwOnError = false)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(taskName))
                {
                    taskName = AppDomain.CurrentDomain.GetData("PlatformTaskName").ToString();
                }
                Mouthpiece.WriteLog(taskName, content);
            }
            catch
            {
                if (throwOnError)
                    throw;
            }
        }

        /// <summary>
        /// (不写入文件)在计划任务展示台显示实时运行信息
        /// </summary>
        /// <param name="content">信息内容</param>
        /// <param name="throwOnError">异常后是否抛出</param>
        protected void ShowRunningLog(string content, bool throwOnError = false)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(taskName))
                {
                    taskName = AppDomain.CurrentDomain.GetData("PlatformTaskName").ToString();
                }
                Mouthpiece.ShowRunningLog(string.Concat(taskName, @"\", RunningBatch.ToString(), @"\", RunFromWeb ? "1" : "0", @"\", RunID), content);
            }
            catch
            {
                if (throwOnError)
                    throw;
            }
        }

        /// <summary>
        /// (会写入文件)在计划任务展示台显示实时运行信息
        /// </summary>
        /// <param name="content">信息内容</param>
        /// <param name="throwOnError">异常后是否抛出</param>
        protected void ShowRunningLogEx(string content, bool throwOnError = false)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(taskName))
                {
                    taskName = AppDomain.CurrentDomain.GetData("PlatformTaskName").ToString();
                }
                Mouthpiece.ShowWriteRunningLog(string.Concat(taskName, @"\", RunningBatch.ToString(), @"\", RunFromWeb ? "1" : "0", @"\", RunID), content);
            }
            catch
            {
                if (throwOnError)
                    throw;
            }
        }

        #endregion

        #region 高级功能

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="body">邮件内容</param>
        /// <param name="emailAddress">收件人地址</param>
        /// <param name="throwOnError">当发送邮件过程中出现异常，是否抛出异常。</param>
        public bool SendEmailTo(string body, string emailAddress, bool throwOnError = false)
        { 
            return SendEmailTo(body, emailAddress, "e代送Worker检查服务报警(来自计划任务平台：" + Environment.MachineName + ")", "", false,null);
        }
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="body">邮件内容</param>
        /// <param name="emailAddress">收件人地址</param>
        /// <param name="throwOnError">当发送邮件过程中出现异常，是否抛出异常。</param>
        public bool SendEmailTo(string body, string emailAddress, string copyto, bool throwOnError = false)
        {
            return SendEmailTo(body, emailAddress, "e代送Worker检查服务报警(来自计划任务平台：" + Environment.MachineName + ")", copyto, false, null);
        }

        ///// <summary>
        /////     发送邮件
        ///// </summary>
        ///// <param name="body">邮件内容</param>
        ///// <param name="emailAddress">收件人地址</param>
        ///// <param name="title"></param>
        ///// <param name="isBodyHtml"></param>
        ///// <param name="throwOnError">当发送邮件过程中出现异常，是否抛出异常。</param>
        //public bool SendEmailTo(string body, string emailAddress,string title, bool isBodyHtml, bool throwOnError = false)
        //{

        //    return SendEmailTo(body, emailAddress, title, null, isBodyHtml);
        //}
        public bool SendEmailTo<ExportList>(string body, string emailAddress, string title, string copyto, bool isBodyHtml, string attachment , Dictionary<string, string> dicHead, IList<ExportList> exportList)
        {
            using (var ms = new MemoryStream())
            {
                //生成excel文件
                ExcelHelper.ExportExcel(ms, exportList, dicHead);
                return SendEmailTo(body, emailAddress, title, copyto, isBodyHtml, stream:ms, attachName: attachment);
            }
           
        }

        /// <summary>
        ///     发送邮件<B>(Dictionary<string, string> dicHead, IList<B> b)
        /// </summary>
        /// <param name="body">邮件内容</param>
        /// <param name="emailAddress">收件人地址</param>
        /// <param name="title">标题</param>
        /// <param name="copyto">抄送</param>
        /// <param name="isBodyHtml">邮件格式</param>
        /// <param name="throwOnError">当发送邮件过程中出现异常，是否抛出异常。</param>
        /// <param name="displayName">显示的名称</param>
        public bool SendEmailTo(string body,string emailAddress,string title,string copyto,bool isBodyHtml,Stream stream=null,bool throwOnError = false,string displayName = "e代送预警系统", string attachName="附件.xls")
        {
            try
            {
                string address = "";
                string[] mailNames = emailAddress.Split(';');
                var from = new MailAddress("wang.xudan@etaostars.com", displayName);
                var client = new SmtpClient
                {
                    Host = "smtp.exmail.qq.com",
                    Credentials = new NetworkCredential("wang.xudan@etaostars.com", "asd123"),
                    Port = 25
                };
                var mail = new MailMessage();
                mail.From = from;
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
                mail.Subject = title;
                mail.Body = body;
                mail.BodyEncoding = Encoding.UTF8;
                mail.IsBodyHtml = isBodyHtml;
                //抄送
                if (!string.IsNullOrEmpty(copyto)) mail.CC.Add(copyto);
                //附件
                if (stream != null)
                {
                    mail.Attachments.Add(new Attachment(stream, attachName));
                }
                // 回复至
                mail.ReplyToList.Add("wang.xudan@etaostars.com");
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Send(mail);
                return true;
            }
            catch
            {
                if (throwOnError)
                {
                    throw;
                }
                return false;
            }
        }

        /// <summary>
        /// 调用一个任务
        /// <para>1、如果非异步调用：同样需要进行任务线程排队，由于方法为同步方法，请考虑线程等待超时的情况</para>
        /// <para>2、请勿自调用</para>
        /// <para>3、底层实现为调用RunOnce方法</para>
        /// <para>4、如果是异步调用，则返回值为null</para>
        /// </summary>
        /// <param name="invokeTaskName">任务名称</param>
        /// <param name="parameters">参数列表(被调用方可从InvokeParameters属性中检索传入参数)</param>
        /// <param name="async">是否异步调用</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">invokeTaskName参数不能为空。</exception>
        /// <exception cref="System.StackOverflowException">堆栈可能不够用呀，99%的可能原因是你自调用了。</exception>
        protected RunTaskResult InvokeTask(string invokeTaskName, Dictionary<string, string> parameters = null, bool async = false)
        {
            if (string.IsNullOrWhiteSpace(taskName))
            {
                taskName = AppDomain.CurrentDomain.GetData("PlatformTaskName").ToString();
            }
            if (string.IsNullOrWhiteSpace(invokeTaskName))
            {
                throw new ArgumentNullException("invokeTaskName参数不能为空。");
            }
            else if (invokeTaskName.Trim().ToLower() == taskName.ToLower())
            {
                throw new StackOverflowException("堆栈可能不够用呀，99%的可能原因是你自调用了。");
            }
            else
            {
                if (parameters == null)
                {
                    parameters = new Dictionary<string, string>();
                }
                if (async)
                {
                    ThreadPool.QueueUserWorkItem(AsyncInvokeTask, new object[] { invokeTaskName, parameters });
                    return null;
                }
                else
                {
                    return Mouthpiece.RunOnceWithParameters(invokeTaskName, parameters);
                }
            }
        }

        /// <summary>
        /// 异步调用任务
        /// </summary>
        /// <param name="parameters">任务名称</param>
        private void AsyncInvokeTask(object parameters)
        {
            object[] parameterList = parameters as object[];
            Dictionary<string, string> p = parameterList[1] as Dictionary<string, string>;
            Mouthpiece.RunOnceWithParameters(parameterList[0].ToString(), p);
        }

        private Dictionary<string, string> _invokeParameters = null;

        /// <summary>
        /// 当该任务是被外界使用InvokeTask调用时，可以从该属性中检索传入参数
        /// </summary>
        public Dictionary<string, string> InvokeParameters
        {
            get
            {
                if (_invokeParameters == null)
                {
                    _invokeParameters = new Dictionary<string, string>();
                }
                return _invokeParameters;
            }
            set { _invokeParameters = value; }
        }

        private List<TaskParameter> _parameters = null;
        /// <summary>
        /// 计划任务参数列表
        /// </summary>
        protected List<TaskParameter> Parameters
        {
            get
            {
                if (_parameters == null)
                {
                    _parameters = new List<TaskParameter>();
                }
                return _parameters;
            }
            set
            {
                _parameters = value;
                ParametersChanged();
            }
        }

        /// <summary>
        /// 获取计划任务参数列表
        /// </summary>
        /// <returns></returns>
        public List<TaskParameter> GetParameters()
        {
            if (Parameters.Count > 0)
            {
                return (from f in Parameters
                        select f.Copy()).ToList();
            }
            else
            {
                return new List<TaskParameter>();
            }
        }

        /// <summary>
        /// 设置计划任务参数列表
        /// </summary>
        /// <param name="parameters"></param>
        public void SetParameters(List<TaskParameter> parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters不允许null");
            }
            else
            {
                int keyCount = (from f in parameters
                                select f.Key.ToLower()).Distinct().Count();
                if (keyCount != parameters.Count)
                {
                    throw new ArgumentException("不允许出现重复键。");
                }
                else
                {
                    Parameters = parameters;
                }
            }
        }

        /// <summary>
        /// 获取或设置参数值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string this[string key]
        {
            get
            {
                if (string.IsNullOrWhiteSpace(key))
                    return null;
                var t = (from f in Parameters
                         where f.Key.ToLower() == key.ToLower()
                         select f).FirstOrDefault();
                if (t == null)
                    return t.Value;
                else
                    return null;
            }
        }

        /// <summary>
        /// 当参数列表改变时执行
        /// </summary>
        protected virtual void ParametersChanged() { }

        /// <summary>
        /// 获取该任务的AppSettings配置
        /// </summary>
        /// <returns></returns>
        protected Dictionary<string, string> GetTaskAppSettings()
        {
            return null;
        }

        #endregion

        #region 服务相关

        /// <summary>
        /// 获取服务
        /// </summary>
        /// <typeparam name="T">服务类型</typeparam>
        /// <returns></returns>
        private T GetService<T>() where T : PlatformService
        {
            T result = PlatformServiceList.GetValueSafe(typeof(T)) as T;
            return result;
        }

        #endregion
    }
}
