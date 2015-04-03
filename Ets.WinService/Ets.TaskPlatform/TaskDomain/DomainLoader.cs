using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.Runtime.Remoting;
using TaskPlatform.PlatformLog;
using System.Text;

namespace TaskPlatform.TaskDomain
{
    /// <summary>
    /// 计划任务运行时域加载器。提供对应用程序域的装载和卸载操作，以及对计划任务的跨域访问操作代理。
    /// <para>使用方法：</para>
    /// <para>1、创建该对象；</para>
    /// <para>2、为FileName属性赋值；</para>
    /// <para>3、CreateTaskDomain。</para>
    /// </summary>
    [Serializable]
    public class DomainLoader : IDisposable
    {

        #region 成员和属性
        private AppDomain _taskDomain = null;
        private TaskObject _task = null;
        /// <summary>
        /// 主框架的应用程序配置集合
        /// </summary>
        public Dictionary<string, string> PlatformAppSettings { get; set; }
        /// <summary>
        /// 主框架的连接字符串集合
        /// </summary>
        public Dictionary<string, string> PlatformConnectionStrings { get; set; }
        /// <summary>
        /// 计划任务运行时域名称
        /// </summary>
        public string DomainName { get; set; }
        /// <summary>
        /// 提供对计划任务的跨域访问操作代理。请注意，暂时不支持对一个程序集内有多个计划任务实现类的代理操作。
        /// </summary>
        public TaskObject TaskOperator
        {
            get
            {
                return _task;
            }
        }
        private bool _domainCreated = false;
        public bool TaskDomainCreated
        {
            get
            {
                return _domainCreated;
            }
        }
        private string _fileName = string.Empty;
        private bool _fileNameSetted = false;
        /// <summary>
        /// 获取或设置计划任务可执行程序集完整路径。
        /// 如果该对象的路径已被用于创建计划任务运行时域，则不允许再次更改该值。
        /// </summary>
        public string FileName
        {
            get
            {
                return _fileName;
            }
            set
            {
                if (_domainCreated)
                {
                    throw new Exception("路径已被用于创建计划任务运行时域，不允许再次更改该值。");
                }
                else if (string.IsNullOrWhiteSpace(value) || !value.ToLower().EndsWith(".dll") || !File.Exists(value))
                {
                    throw new FileNotFoundException("指定的文件不是dll文件或者文件不存在。");
                }
                _fileName = value;
                _fileNameSetted = true;
            }
        }
        private DateTime _taskDomainCreateTime = DateTime.MinValue;
        /// <summary>
        /// 获取计划任务运行时域创建的时间。
        /// </summary>
        public DateTime TaskDomainCreateTime
        {
            get
            {
                return _taskDomainCreateTime;
            }
        }

        /// <summary>
        /// 计划任务所用到的计划
        /// </summary>
        public PlanEngine.Plan Plan { get; set; }
        /// <summary>
        /// 计划任务执行代理类对象完全路径
        /// </summary>
        private static string taskObjectTypeName = new TaskObject().GetType().FullName;

        private TaskConfiguration _taskConfig = null;
        /// <summary>
        /// 计划任务配置
        /// </summary>
        public TaskConfiguration TaskConfig
        {
            get
            {
                if (_taskConfig == null)
                {
                    _taskConfig = new TaskConfiguration();
                }
                return _taskConfig;
            }
            set { _taskConfig = value; }
        }

        /// <summary>
        /// 是否为镜像任务
        /// </summary>
        public bool IsMirrorTask { get; set; }

        /// <summary>
        /// 镜像名称
        /// </summary>
        public string MirrorName { get; set; }

        /// <summary>
        /// 主计划任务名称
        /// </summary>
        public string TaskName { get; set; }

        #endregion

        #region 事件
        /// <summary>
        /// 计划任务运行时域内部出现未经处理的异常处理事件委托。
        /// </summary>
        /// <param name="loader">The loader.</param>
        /// <param name="args">The args.</param>
        public delegate void DomainExceptionHandle(object loader, DomainExceptionEventArgs e);
        /// <summary>
        /// 在计划任务运行时域内部出现未经处理的异常时发生。
        /// </summary>
        public event DomainExceptionHandle DomainException;

        /// <summary>
        /// 触发 <see cref="E:DomainException"/> 事件.
        /// </summary>
        /// <param name="args">包含事件发生时的数据，以 <see cref="TaskPlatform.TaskDomain.DomainExceptionEventArgs"/> 对象承载。</param>
        protected virtual void OnDomainException(DomainExceptionEventArgs e)
        {
            if (DomainException != null)
            {
                DomainException(this, e);
            }
        }
        #endregion

        /// <summary>
        /// 实例化一个 <see cref="DomainLoader"/> 类对象。
        /// </summary>
        public DomainLoader()
        {
        }

        /// <summary>
        /// 根据指定文件路径，实例化一个 <see cref="DomainLoader"/> 类对象。
        /// </summary>
        /// <param name="fileName">文件路径</param>
        public DomainLoader(string fileName)
        {
            this.FileName = fileName;
        }

        /// <summary>
        /// 根据指定文件，创建计划任务运行时域。请确保FileName属性已赋值。
        /// </summary>
        public string CreateTaskDomain(string platformPortName)
        {
            if (!_fileNameSetted)
                return "FileName属性尚未设置。";
            try
            {
                AppDomainSetup setup = new AppDomainSetup();
                setup.LoaderOptimization = LoaderOptimization.MultiDomain;
                string path = Path.GetDirectoryName(_fileName);
                string fileShortName = Path.GetFileNameWithoutExtension(_fileName);
                setup.ApplicationBase = path;
                setup.PrivateBinPath = path;
                if (!fileShortName.ToLower().Contains(@"noshadowcopy"))
                {
                    //设置域启用卷影复制，以便在卸载域后可以更新程序集文件。
                    setup.ShadowCopyFiles = "true";
                    setup.ShadowCopyDirectories = path;
                }
                setup.ConfigurationFile = _fileName + ".config";
                try
                {
                    _taskDomain = System.AppDomain.CreateDomain(fileShortName, null, setup);
                    _taskDomain.SetData("PlatformPortName", platformPortName);
                    _taskDomain.SetData("PlatformTaskName", DomainName);
                    string className = taskObjectTypeName;
                    string location = Assembly.GetExecutingAssembly().Location;
                    ObjectHandle objLoader = _taskDomain.CreateComInstanceFrom(location, className);
                    _task = objLoader.Unwrap() as TaskObject;
                    _task.Name = DomainName;
                    _domainCreated = true;
                    _taskDomainCreateTime = DateTime.Now;
                    _task.LoadTask(_fileName);
                }
                catch (Exception ex)
                {
                    Log.CustomWrite(ex.ToString(), "TaskDomain--DomainLoaderException--LoadTask");
                    return ex.ToString();
                }
            }
            catch (Exception ex)
            {
                Log.CustomWrite(ex.ToString(), "TaskDomain--DomainLoaderException");
                return ex.ToString();
            }
            return "";
        }

        void _task_DomainException(object domain, UnhandledExceptionEventArgs e)
        {
            DomainExceptionEventArgs args = new DomainExceptionEventArgs(domain, e.ExceptionObject, e.IsTerminating);
            OnDomainException(args);
        }

        private void _taskDomain_FirstChanceException(object sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e)
        {
            // 当域内异常发生后，第一时间捕获异常并记入日志
            Log.CustomWrite(e.Exception.ToString(), "TaskDomain--EveryDomainAllException");
        }

        /// <summary>
        /// 当某个异常未被捕获时发生。
        /// </summary>
        private void _taskDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            DomainExceptionEventArgs args = new DomainExceptionEventArgs(sender, e.ExceptionObject, e.IsTerminating);
            OnDomainException(args);
        }

        /// <summary>
        /// 卸载计划任务运行时域。
        /// </summary>
        public void UnLoadTaskDomian(DeleteTaskType deleteTaskType = DeleteTaskType.None)
        {
            if (!_domainCreated)
                throw new Exception("计划任务运行时域尚未创建。");
            try
            {
                System.AppDomain.Unload(_taskDomain);
                _task = null;
                _taskDomain = null;
                _domainCreated = false;
                GC.Collect();
            }
            catch (Exception ex)
            {
                Log.CustomWrite(ex.ToString(), "TaskDomain--DomainLoaderException");
                throw ex;
            }
        }

        private void CheckDelete()
        {
            if (_domainCreated)
                throw new Exception("请先卸载计划任务的运行时域。");
        }

        /// <summary>
        /// 删除日志信息.
        /// </summary>
        private void DeleteLogs()
        {
            CheckDelete();
            throw new Exception("先不实现。");
        }
        /// <summary>
        /// 删除计划任务dll文件.
        /// </summary>
        private void DeleteTaskFile()
        {
            CheckDelete();
            File.Delete(_fileName);
        }
        /// <summary>
        /// 删除计划任务的配置文件.
        /// </summary>
        private void DeleteTaskConfigFile()
        {
            CheckDelete();
            throw new Exception("先不实现。");
        }

        /// <summary>
        /// 删除计划任务相关信息，并指定删除范围。
        /// </summary>
        private void DeleteTaskData(DeleteTaskType deleteTaskType)
        {
            switch (deleteTaskType)
            {
                case DeleteTaskType.All:
                    DeleteTaskFile();
                    DeleteTaskConfigFile();
                    DeleteLogs();
                    break;
                case DeleteTaskType.Logs:
                    DeleteLogs();
                    break;
                case DeleteTaskType.LogsAndConfigFile:
                    DeleteLogs();
                    DeleteTaskConfigFile();
                    break;
                case DeleteTaskType.TaskConfigFile:
                    DeleteTaskConfigFile();
                    break;
                case DeleteTaskType.TaskFile:
                    DeleteTaskFile();
                    break;
                case DeleteTaskType.TaskFileAndConfigFile:
                    DeleteTaskFile();
                    DeleteTaskConfigFile();
                    break;
                case DeleteTaskType.TaskFileAndLogs:
                    DeleteTaskFile();
                    DeleteLogs();
                    break;
            }
        }

        public void Dispose()
        {
            GC.Collect();
        }
    }
}
