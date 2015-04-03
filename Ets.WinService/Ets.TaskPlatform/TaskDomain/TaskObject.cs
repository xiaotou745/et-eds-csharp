using System;
using System.Collections.Generic;
using TaskPlatform.TaskInterface;
using TaskPlatform.PlatformLog;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Lifetime;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace TaskPlatform.TaskDomain
{
    /// <summary>
    /// 提供跨域代理操作
    /// </summary>
    [ComVisibleAttribute(true)]
    public class TaskObject : MarshalByRefObject, ITask
    {
        #region 成员

        static Assembly _interfaceAssembly = null;
        static Assembly _luaAssembly = null;
        static Assembly _luaInterfaceAssembly = null;
        static Assembly _mysqlAssembly = null;
        Assembly _assembly = null;
        Type[] _types = null;
        ITask _task = null;
        AbstractTask _advancedTask = null;
        bool _loaded = false;
        object lockRun = 1;
        MethodInfo[] methods = null;
        string _fileName = "";
        private string _name = string.Empty;
        private static string _assemblyLocation = Assembly.GetExecutingAssembly().Location;

        public static string AssemblyLocation
        {
            get { return TaskObject._assemblyLocation; }
            set { TaskObject._assemblyLocation = value; }
        }
        private static string _performanceTypeName = typeof(DomainPerformance).FullName;

        public static string PerformanceTypeName
        {
            get { return TaskObject._performanceTypeName; }
            set { TaskObject._performanceTypeName = value; }
        }
        /// <summary>
        /// 获取名称
        /// </summary>
        public string Name
        {
            get { return _name; }
            internal set { _name = value; DomainPerformance.DomainName = value; }
        }
        private int _timeoutSeconds = 5;
        private bool _supportTaskParameters = false;
        private long _runningBatch = 0;
        private Dictionary<int, string> _fromList = new Dictionary<int, string>();
        /// <summary>
        /// 执行锁
        /// </summary>
        private static object _runLocker = new object();

        #endregion

        #region 内部处理

        /// <summary>
        /// 获取计划任务对象.
        /// </summary>
        public ITask Task
        {
            get
            {
                return _task;
            }
        }

        /// <summary>
        /// 获取计划任务抽象类对象.
        /// </summary>
        public AbstractTask AdvancedTask
        {
            get
            {
                return _advancedTask;
            }
        }

        /// <summary>
        /// 计划任务是否支持高级功能
        /// </summary>
        public bool SupportAdvancedFeatures
        {
            get
            {
                return _advancedTask != null;
            }
        }

        /// <summary>
        /// 获取一个值该值指示计划任务对象是否已经加载到内存。
        /// </summary>
        /// <value>
        ///   如果已经加载则为true，否则则为false。
        /// </value>
        public bool Loaded
        {
            get
            {
                return _loaded;
            }
        }

        public string[] GetPublicMethodNames()
        {
            methods = (from method in _task.GetType().GetMethods()
                       where method.IsPublic
                       select method).ToArray();
            string[] names = (from name in methods
                              select name.Name).ToArray();
            return names;
        }

        /// <summary>
        /// 加载计划任务对象，暂时不支持一个程序集内多个计划任务实现类。
        /// 如果出现多个，则以第一个被找到的实现类作为计划任务执行。
        /// </summary>
        /// <param name="fileName">文件路径</param>
        internal void LoadTask(string fileName)
        {
            if (_loaded)
            {
                throw new Exception("计划任务对象已经加载到内存。");
            }
            _loaded = false;
            _fileName = fileName;
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            AppDomain.CurrentDomain.TypeResolve += new ResolveEventHandler(CurrentDomain_TypeResolve);
            AppDomain.CurrentDomain.ResourceResolve += new ResolveEventHandler(CurrentDomain_ResourceResolve);
            _assembly = Assembly.LoadFrom(fileName);
            _types = _assembly.GetTypes();
            foreach (System.Type type in _types)
            {
                if (type != null && type.IsClass && type.GetInterface(typeof(ITask).FullName) != null)
                {
                    _task = _assembly.CreateInstance(type.FullName, true) as ITask;
                    //_task = Activator.CreateInstance(type) as ITask;
                    if (_task != null)
                    {
                        try
                        {
                            _advancedTask = _task as AbstractTask;
                        }
                        catch { }
                        _loaded = true;
                        break;
                    }
                }
            }

            // 此处处理该运行时域所有和未经处理的异常并记入日志

            try
            {
                AppDomain.CurrentDomain.FirstChanceException += new EventHandler<System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs>(CurrentDomain_FirstChanceException);
            }
            catch (Exception ex)
            {
                string per = "";
                try
                {
                    per = string.Format("({0})", _assembly.FullName);
                }
                catch
                {
                    per = "";
                }
                Log.CustomWrite(ex.ToString(), "TaskDomain--TaskObjectAllException");
            }
            try
            {
                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            }
            catch (Exception ex)
            {
                Log.CustomWrite(ex.ToString(), "TaskDomain--TaskObjectUnhandledException");
            }
        }

        Assembly CurrentDomain_TypeResolve(object sender, ResolveEventArgs args)
        {
            throw new NotImplementedException();
        }

        Assembly CurrentDomain_ResourceResolve(object sender, ResolveEventArgs args)
        {

            throw new NotImplementedException("ResourceResolve失败:" + args.Name);
        }

        bool loadingAssembly = false;

        Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (loadingAssembly)
            {
                return args.RequestingAssembly;
            }
            loadingAssembly = true;
            string parentPath = Directory.GetParent(Path.GetDirectoryName(_fileName)).Parent.FullName;
            //File.WriteAllText("123.log", parentPath + "\\" + args.Name);
            if (args != null && !string.IsNullOrWhiteSpace(args.Name))
            {
                if (args.Name.ToLower().Contains("taskplatform.taskinterface"))
                {
                    if (_interfaceAssembly == null)
                    {
                        _interfaceAssembly = Assembly.LoadFrom(parentPath + @"\TaskPlatform.TaskInterface.dll");
                    }
                    loadingAssembly = false;
                    return _interfaceAssembly;
                }
                else if (args.Name.ToLower().Contains("luainterface"))
                {
                    if (_luaInterfaceAssembly == null)
                    {
                        _luaInterfaceAssembly = Assembly.LoadFrom(parentPath + @"\LuaInterface.dll");
                    }
                    loadingAssembly = false;
                    return _luaInterfaceAssembly;
                }
                else if (args.Name.ToLower().Contains("lua51"))
                {
                    if (_luaAssembly == null)
                    {
                        _luaAssembly = Assembly.LoadFrom(parentPath + @"\Lua51.dll");
                    }
                    loadingAssembly = false;
                    return _luaAssembly;
                }
                else if (args.Name.ToLower().Contains("mysql.data"))
                {
                    if (args.Name.ToLower().Contains(".resources"))
                    {
                        loadingAssembly = false;
                        return args.RequestingAssembly;
                    }
                    if (_mysqlAssembly == null)
                    {
                        _mysqlAssembly = Assembly.LoadFrom(parentPath + @"\MySQL.Data.dll");
                    }
                    loadingAssembly = false;
                    return _mysqlAssembly;
                }
                else
                {
                    Assembly assembly = null;
                    try
                    {
                        assembly = (_task as AbstractTask).CustomAssemblyResolve(args.Name);
                    }
                    catch { }
                    if (assembly == null)
                    {
                        try
                        {
                            assembly = Assembly.Load(args.Name);
                        }
                        catch { }
                    }
                    loadingAssembly = false;
                    return assembly ?? args.RequestingAssembly;
                }
            }
            else
            {
                loadingAssembly = false;
                return args.RequestingAssembly;
            }
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                Log.CustomWrite(((Exception)e.ExceptionObject).ToString(), "TaskDomain--TaskObjectUnhandledException");
            }
            catch { }
            // 将事件广播出去<目前我的技术无法做到跨域传出，暂时先这样吧。>
            //OnDomainException(e);
        }

        private void CurrentDomain_FirstChanceException(object sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e)
        {
            Log.CustomWrite(e.Exception.ToString(), "TaskDomain--TaskObjectAllException");
        }

        private void Check(bool checkFull = false)
        {
            if (!_loaded)
                throw new Exception("计划任务对象尚未被加载到内存,请调用LoadTask方法，以便将计划任务对象加载到内存。");
            if (checkFull && !SupportAdvancedFeatures)
            {
                throw new Exception("计划任务对象不支持高级功能。请联系开发者改用AbstractTask抽象类作为计划任务的基类而不是集成ITask接口。");
            }
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

        /// <summary>
        /// 指定超时时间运行一次。
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public RunTaskResult RunTaskWithTime(int seconds)
        {
            _timeoutSeconds = seconds;
            if (_timeoutSeconds < 1)
                _timeoutSeconds = 1;
            return RunTask();
        }

        /// <summary>
        /// 从网页执行一次，运行时信息将被缓存。
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public RunTaskResult RunTaskFromWeb(int seconds, string runID)
        {
            if (!_fromList.ContainsKey(Thread.CurrentThread.ManagedThreadId))
            {
                _fromList.Add(Thread.CurrentThread.ManagedThreadId, runID);
            }
            return RunTaskWithTime(seconds);
        }

        #region 计划任务执行代码

        /// <summary>
        /// 获取计划任务的唯一标识符
        /// </summary>
        /// <returns></returns>
        public string TaskKey()
        {
            Check();
            return _task.TaskKey();
        }

        /// <summary>
        /// 获取计划任务友好名称
        /// </summary>
        /// <returns></returns>
        public string TaskName()
        {
            Check();
            return _task.TaskName();
        }

        /// <summary>
        /// 获取计划任务的描述
        /// </summary>
        /// <returns></returns>
        public string TaskDescription()
        {
            Check();
            return _task.TaskDescription();
        }

        /// <summary>
        /// 初始化计划任务时将被执行。平台将通过此函数通知所以计划任务，以便或许平台的相关配置信息。
        /// </summary>
        /// <param name="platformAppSettings"></param>
        /// <param name="platformConnectionStrings"></param>
        public void InitTask(Dictionary<string, string> platformAppSettings, Dictionary<string, string> platformConnectionStrings)
        {
            Check();
            _task.InitTask(platformAppSettings, platformConnectionStrings);
        }

        /// <summary>
        /// 返回配置项，以供平台统一配置。该列表将可以通过平台提供的配置编辑器编辑，编辑后由平台调用DownloadConfig函数回发给计划任务。
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> UploadConfig()
        {
            Check();
            return _task.UploadConfig();
        }

        /// <summary>
        /// 接收平台发过来的配置项。
        /// </summary>
        /// <param name="configs"></param>
        public void DownloadConfig(Dictionary<string, string> configs)
        {
            Check();
            _task.DownloadConfig(configs);
        }

        /// <summary>
        /// 执行计划任务
        /// </summary>
        /// <returns></returns>
        public RunTaskResult RunTask()
        {
            //  ^_^'
            if (Debugger.IsAttached && !SupportAdvancedFeatures && DateTime.Now > DateTime.Parse("2014-01-28 23:59:59"))
            {
                return new RunTaskResult() { IsCanceled = true, Success = false, Result = "不支持ITask接口的任务，请改用AbstractTask。" };
            }
            DateTime waitStart = DateTime.Now;
            //System.Threading.Semaphore sem = new System.Threading.Semaphore(1, 1);
            //sem.WaitOne(1);
            //sem.Release(1);
            lock (_runLocker)
            {
                DateTime runStart = DateTime.Now;
                if ((runStart - waitStart).TotalSeconds > _timeoutSeconds)
                {
                    RunTaskResult resultCancel = new RunTaskResult();
                    resultCancel.Success = false;
                    resultCancel.IsCanceled = true;
                    resultCancel.Result = "%sys%WaitTimeOut";
                    return resultCancel;
                }
                Check();
                RunTaskResult result = new RunTaskResult();
                DateTime runEnd = DateTime.Now;
                if (_runningBatch == long.MaxValue)
                    _runningBatch = 0;
                _runningBatch++;
                if (SupportAdvancedFeatures)
                {
                    _advancedTask.RunningBatch = _runningBatch;
                    if (_fromList.ContainsKey(Thread.CurrentThread.ManagedThreadId))
                    {
                        _advancedTask.RunFromWeb = true;
                        _advancedTask.RunID = _fromList[Thread.CurrentThread.ManagedThreadId];
                    }
                }
                try
                {
                    result = _task.RunTask();
                    runEnd = DateTime.Now;
                }
                catch (Exception ex)
                {
                    result.Success = false;
                    result.Result = "(执行异常)" + ex.ToString();
                    runEnd = DateTime.Now;
                    Log.CustomWrite(ex.ToString(), "TaskDomain--TaskObjectException");
                }
                result.CustomerArgs["TaskRunStartTime"] = runStart;
                result.CustomerArgs["TaskRunTimeSpan"] = runEnd - runStart;
                result.RunningBatch = _runningBatch;
                return result;
            }
        }

        /// <summary>
        /// 获取计划任务参数列表
        /// </summary>
        /// <returns></returns>
        public List<TaskParameter> GetParameters()
        {
            Check(true);
            return _advancedTask.GetParameters();
        }

        /// <summary>
        /// 获取计划任务参数列表
        /// </summary>
        /// <returns></returns>
        public void SetParameters(List<TaskParameter> parameters)
        {
            Check(true);
            _advancedTask.SetParameters(parameters);
        }

        #endregion

        #region 事件
        /// <summary>
        /// 计划任务运行时域内部出现未经处理的异常处理事件委托。
        /// </summary>
        /// <param name="loader">The loader.</param>
        /// <param name="args">The args.</param>
        public delegate void DomainExceptionHandle(object domain, UnhandledExceptionEventArgs e);
        /// <summary>
        /// 在计划任务运行时域内部出现未经处理的异常时发生。
        /// </summary>
        public event DomainExceptionHandle DomainException;

        /// <summary>
        /// 触发 <see cref="E:DomainException"/> 事件.
        /// </summary>
        /// <param name="args">包含事件发生时的数据，以 <see cref="TaskPlatform.TaskDomain.DomainExceptionEventArgs"/> 对象承载。</param>
        protected virtual void OnDomainException(UnhandledExceptionEventArgs e)
        {
            if (DomainException != null)
            {
                DomainException(AppDomain.CurrentDomain, e);
            }
        }
        #endregion

        #region 性能监控

        /// <summary>
        /// 获取远程对象所在应用程序域
        /// </summary>
        /// <returns></returns>
        public AppDomain GetDomain()
        {
            return AppDomain.CurrentDomain;
        }

        #endregion
    }
}
