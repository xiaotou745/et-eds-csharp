#region Using
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraNavBar;
using DevExpress.XtraTab;
using TaskPlatform.Commom;
using TaskPlatform.Controls;
using TaskPlatform.Forms;
using TaskPlatform.PlanEngine;
using TaskPlatform.PlatformLog;
using TaskPlatform.TaskDomain;
using TaskPlatform.TaskInterface;
using System.Reflection;
using System.Xml;
using System.Security.Principal;
using TaskPlatform.PlanEngine.V2;
using DevExpress.XtraBars;
using System.Threading.Tasks;
//using LuaInterface;
#endregion

namespace TaskPlatform
{
    /// <summary>
    /// 主框架
    /// </summary>
    public partial class PlatformForm : XtraForm, IOperation
    {
        #region 成员属性
        public static PlatformForm Form { get; set; }
        /// <summary>
        /// 命令行参数
        /// </summary>
        public string[] ConsoleArgs { get; set; }
        /// <summary>
        /// 主框架启动时所在目录
        /// </summary>
        public readonly static string MainPath = Application.StartupPath.TrimEnd('\\');
        /// <summary>
        /// 该主框架下计划任务dll目录
        /// </summary>
        public readonly static string TaskFilesPath = Application.StartupPath.TrimEnd('\\') + "\\Tasks";
        /// <summary>
        /// 该主框架下缓存目录
        /// </summary>
        public readonly static string LocalDataCachePath = Application.StartupPath.TrimEnd('\\') + "\\LoaclDataCache";
        /// <summary>
        /// 该主框架下系统计划任务dll目录
        /// </summary>
        public readonly static string SystemTaskFilesPath = Application.StartupPath.TrimEnd('\\') + "\\SystemTasks";
        /// <summary>
        /// 该主框架下计划任务配置文件目录
        /// </summary>
        public readonly static string TaskConfigFilesPath = Application.StartupPath.TrimEnd('\\') + "\\Configs";
        /// <summary>
        /// 该主框架下计划任务平台资源
        /// </summary>
        public readonly static string ResoucesPath = Application.StartupPath.TrimEnd('\\') + "\\Resouces";
        /// <summary>
        /// 该主框架下计划任务日志目录
        /// </summary>
        public readonly static string MainLogPath = Application.StartupPath.TrimEnd('\\') + "\\Logs";
        /// <summary>
        /// 主框架启动时间
        /// </summary>
        public readonly static DateTime StartupTime = DateTime.Now;
        /// <summary>
        /// 主框架的应用程序配置集合
        /// </summary>
        private static Dictionary<string, string> appSettings = new Dictionary<string, string>();
        /// <summary>
        /// 主框架的应用程序配置集合
        /// </summary>
        public static Dictionary<string, string> AppSettings
        {
            get { return PlatformForm.appSettings; }
            set { PlatformForm.appSettings = value; }
        }
        /// <summary>
        /// 主框架的连接字符串集合
        /// </summary>
        private static Dictionary<string, string> connectionStrings = new Dictionary<string, string>();
        /// <summary>
        /// 主框架的连接字符串集合
        /// </summary>
        public static Dictionary<string, string> ConnectionStrings
        {
            get { return PlatformForm.connectionStrings; }
            set { PlatformForm.connectionStrings = value; }
        }

        /// <summary>
        /// 默认本地缓存容器Key
        /// </summary>
        public string DefaultCacheContainerName
        {
            get
            {
                return "6DDB8DB2-85FF-41F8-9311-7EF924C389EE";
            }
        }
        /// <summary>
        /// 本地缓存
        /// </summary>
        internal static LocalDataCacheContainer PlanPlatformCache = null;// new LocalDataCacheContainer();
        /// <summary>
        /// 报警系统缓存
        /// </summary>
        internal static LocalDataCacheContainer AlertSystemCache = null;// new LocalDataCacheContainer("249D7666-6376-4052-86AE-B8A3A968BD19");
        /// <summary>
        /// 报警系统缓存
        /// </summary>
        internal static LocalDataCacheContainer TaskGroupCache = null;

        private object _tasksScanner = 1;
        private Dictionary<string, Image> imgs = new Dictionary<string, Image>();

        /// <summary>
        /// 启动时是否不启动计划引擎
        /// </summary>
        private bool _noPlanEngine = false;
        /// <summary>
        /// 是否为安全模式
        /// </summary>
        private static bool _safeMode = false;
        /// <summary>
        /// 是否要开启数据升级计划
        /// </summary>
        private bool _updateDataBase = false;
        private bool _canClose = false;
        /// <summary>
        /// 是否为安全模式
        /// </summary>
        public static bool SafeMode
        {
            get { return PlatformForm._safeMode; }
            set { PlatformForm._safeMode = value; }
        }
        /// <summary>
        /// 必须运行的任务列表
        /// </summary>
        static List<string> mustRunTasks = new List<string>();
        /// <summary>
        /// 必须运行的任务列表
        /// </summary>
        public static List<string> MustRunTasks
        {
            get { return mustRunTasks; }
            set
            {
                mustRunTasks = value;
                AlertSystemCache.Update(new LoaclDataCacheObject() { Key = "MustRunTasks", KeyForUpdate = "MustRunTasks", Value = mustRunTasks });
            }
        }
        bool mustRunTasksFirstRun = MustRunTasks.Count < 1;
        /// <summary>
        /// 平台IPC端口
        /// </summary>
        public string PlatformPortName { set; get; }
        /// <summary>
        /// 是否自动创建临时IPC信道
        /// </summary>
        private bool isAutoIPC = false;
        /// <summary>
        /// 已加载的计划任务
        /// </summary>
        private static List<DomainLoader> domainLoaderList = new List<DomainLoader>();
        /// <summary>
        /// 已加载的计划任务
        /// </summary>
        public static List<DomainLoader> DomainLoaderList
        {
            get { return domainLoaderList; }
        }
        /// <summary>
        /// 警报列表(为了兼容旧版本，该属性系新加)
        /// </summary>
        private static List<AlarmObject> _alarmObjects = new List<AlarmObject>();
        /// <summary>
        /// 警报列表(为了兼容旧版本，该属性系新加)
        /// </summary>
        public static List<AlarmObject> AlarmObjects
        {
            get
            {
                if (_alarmObjects == null || _alarmObjects.Count < 1)
                {
                    try
                    {
                        _alarmObjects = AlertSystemCache["AlarmObjects"].Value as List<AlarmObject>;
                    }
                    catch
                    {
                        _alarmObjects = new List<AlarmObject>();
                    }
                    if (_alarmObjects == null)
                    {
                        _alarmObjects = new List<AlarmObject>();
                    }
                }
                return PlatformForm._alarmObjects;
            }
            set { PlatformForm._alarmObjects = value; }
        }

        private string _taskRunStatisticsLock = "_taskRunStatisticsLock";
        private Dictionary<string, List<TaskRunInfo>> _taskRunStatistics = new Dictionary<string, List<TaskRunInfo>>();

        /// <summary>
        /// 任务执行信息统计
        /// </summary>
        public Dictionary<string, List<TaskRunInfo>> TaskRunStatistics
        {
            get
            {
                Dictionary<string, int> _needReverseKeyList = new Dictionary<string, int>();
                foreach (var item in _taskRunStatistics)
                {
                    AlarmObject aobj = AlarmObjects.Find(obj => { return obj.PlanName == item.Key; });
                    if (aobj == null)
                    {
                        continue;
                    }
                    else if (item.Value == null || item.Value.Count <= aobj.RunCount)
                    {
                        continue;
                    }
                    else
                    {
                        _needReverseKeyList.Add(item.Key, aobj.RunCount);
                    }
                }
                lock (_taskRunStatisticsLock)
                {
                    foreach (var item in _needReverseKeyList)
                    {
                        // 反转顺序
                        _taskRunStatistics[item.Key].Reverse();
                        // 取出后n个元素并会写
                        _taskRunStatistics[item.Key] = _taskRunStatistics[item.Key].Take(item.Value).ToList();
                        // 再次反转顺序，使正序
                        _taskRunStatistics[item.Key].Reverse();
                    }
                }
                return _taskRunStatistics;
            }
            set { _taskRunStatistics = value; }
        }

        private NavBarItem _currentClickItem = null;
        private int _smsAlertPriority = -999;
        internal int SMSAlertPriority
        {
            get
            {
                if (_smsAlertPriority <= -999)
                {
                    int result = 0;
                    try
                    {
                        int.TryParse(appSettings["SMSAlertPriority"], out result);
                    }
                    catch { }
                    _smsAlertPriority = result;
                    return result;
                }
                else
                {
                    return _smsAlertPriority;
                }
            }
        }
        private List<string> _receiveMobiles = null;
        internal List<string> ReceiveMobiles
        {
            get
            {
                if (_receiveMobiles == null)
                {
                    try
                    {
                        _receiveMobiles = appSettings["ReceiveMobiles"].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    }
                    catch
                    {
                        _receiveMobiles = new List<string>();
                    }
                }
                return _receiveMobiles;
            }
        }
        private string _smsKey = null;
        internal string SMSKey
        {
            get
            {
                if (_smsKey == null)
                {
                    try
                    {
                        _smsKey = appSettings["SMSKey"] ?? string.Empty;
                    }
                    catch
                    {
                        _smsKey = string.Empty;
                    }
                }
                return _smsKey;
            }
        }
        private string _smsSignature = null;
        internal string SMSSignature
        {
            get
            {
                if (_smsSignature == null)
                {
                    try
                    {
                        _smsSignature = appSettings["SMSSignature"] ?? string.Empty;
                    }
                    catch
                    {
                        _smsSignature = string.Empty;
                    }
                }
                return _smsSignature;
            }
        }
        CRMSMSService.SmsSenderImplService crmSMSService = null;

        private List<string> _loopTasks = null;

        /// <summary>
        /// 需要循环执行的Task列表
        /// </summary>
        public List<string> LoopTasks
        {
            get
            {
                if (_loopTasks == null)
                {
                    _loopTasks = new List<string>();
                }
                return _loopTasks;
            }
            set { _loopTasks = value; }
        }

        private Dictionary<string, Queue<string>> _runningCache = null;

        public Dictionary<string, Queue<string>> RunningCache
        {
            get
            {
                if (_runningCache == null)
                {
                    _runningCache = new Dictionary<string, Queue<string>>();
                }
                return _runningCache;
            }
            private set { _runningCache = value; }
        }

        #endregion

        #region 计划引擎、计划任务运行时域
        /// <summary>
        /// 全局计划引擎
        /// </summary>
        internal static PlanEngine.PlanEngine PlansEngine = null;
        private List<DomainLoader> _taskDomains = new List<DomainLoader>();
        private List<TaskGroup> _taskGroupList = null;
        /// <summary>
        /// 任务分组列表
        /// </summary>
        internal List<TaskGroup> TaskGroupList
        {
            get
            {
                if (_taskGroupList == null)
                {
                    _taskGroupList = new List<TaskGroup>();
                }
                return _taskGroupList;
            }
            set { _taskGroupList = value; }
        }

        #endregion

        #region 初始化等

        public PlatformForm()
        {
            InitializeComponent();
            Form = this;
        }

        private void PlatformForm_Load(object sender, EventArgs e)
        {
            #region 判断管理员身份、注册IPC通道

            if (!IsRunAsAdmin())
            {
                // 以管理员身份重启
                ProcessStartInfo proc = new ProcessStartInfo();
                proc.UseShellExecute = true;
                proc.WorkingDirectory = Environment.CurrentDirectory;
                proc.FileName = Application.ExecutablePath;
                proc.Verb = "runas";

                try
                {
                    Process.Start(proc);
                    Process.GetCurrentProcess().Kill();
                }
                catch
                {
                    XtraMessageBox.Show("程序无法自动获取管理员身份运行。\n您可以手动以管理员身份运行。");
                }
            }
            ProgramMutexAndRegisterChannel();

            #endregion

            // 汉化控件
            new DevChinese();
            tabPageHome.Name = "%sysNoClose%tabPageHome";
            this.Text += string.Format("(当前版本：{0}，IPC：{1})", Application.ProductVersion, PlatformPortName);
            tabContainer.CloseButtonClick += tabContainer_CloseButtonClick;
            var consoleArgs = from arg in ConsoleArgs
                              select arg.ToLower();
            _noPlanEngine = consoleArgs.Contains("/noplanengine");
            _safeMode = consoleArgs.Contains("/safe");
            if (_safeMode)
            {
                SetStatusInfo("计划任务执行平台已在安全模式下启动。");
                this.Text += "----安全模式";
                btnRefresh.Enabled = false;
                nbcContainer.ActiveGroup = nbgPlatformManager;
                return;
            }
            // 如果不存在新版数据库文件，则强制启动数据升级计划
            if (!File.Exists(LoaclDataCacheManager.LoaclDataCachePath + "\\" + DefaultCacheContainerName + ".dll") || !File.Exists(LoaclDataCacheManager.LoaclDataCachePath + "\\249D7666-6376-4052-86AE-B8A3A968BD19.dll"))
            {
                _updateDataBase = true;
            }
            else
            {
                _updateDataBase = consoleArgs.Contains("/updatedatabase") || consoleArgs.Contains("/updatedb");
            }
            PlanPlatformCache = new LocalDataCacheContainer(DefaultCacheContainerName);
            AlertSystemCache = new LocalDataCacheContainer("249D7666-6376-4052-86AE-B8A3A968BD19");
            TaskGroupCache = new LocalDataCacheContainer("EA9F008B-E743-4BE5-BD1A-2A38427856CA");
            #region 加载任务分组信息

            if (TaskGroupCache.ContainsKey("TaskGroupList"))
            {
                _taskGroupList = TaskGroupCache["TaskGroupList"].Value as List<TaskGroup>;
            }
            else
            {
                TaskGroupCache.Update(new LoaclDataCacheObject() { Key = "TaskGroupList", KeyForUpdate = "TaskGroupList", Value = TaskGroupList });
            }
            TaskGroup defalutNameGroup = (from f in TaskGroupList
                                          where f.GroupName == "默认分组"
                                          select f).FirstOrDefault();
            if (defalutNameGroup == null)
            {
                defalutNameGroup = new TaskGroup();
                defalutNameGroup.GroupName = "默认分组";
                defalutNameGroup.IsDefault = true;
                defalutNameGroup.GroupControl = nbgTasks;
                TaskGroupList.Add(defalutNameGroup);
            }
            else
            {
                defalutNameGroup.GroupControl = nbgTasks;
            }

            var groupList = (from f in TaskGroupList
                             where f.GroupControl == null
                             select f).ToList();
            foreach (TaskGroup group in groupList)
            {
                NavBarGroup groupControl = new NavBarGroup(group.GroupName);
                groupControl.SmallImageIndex = 1;
                nbcContainer.Groups.Insert(nbcContainer.Groups.Count - 1, groupControl);
                group.GroupControl = groupControl;
            }
            SaveTaskGroupCache();

            #endregion
            this.ntfPlatformNotify.Text = this.Text;
            SetStatusInfo("计划任务执行平台启动");
            if (_updateDataBase)
            {
                #region 升级缓存

                SetStatusInfo("监测到需要进行数据升级，即将启动数据升级计划……");
                this.BeginInvoke(new Action(() =>
                {
                    DirScannerAlone();
                    UpdateDataBaseForm updateDBForm = new UpdateDataBaseForm();
                    updateDBForm.ShowDialog();
                    if (updateDBForm.IsOK)
                    {
                        XtraMessageBox.Show(this, "数据升级成功完成，即将重新启动……", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Process.Start(Application.ExecutablePath);
                        _canClose = true;
                        this.Close();
                    }
                    else
                    {
                        XtraMessageBox.Show(this, "数据升级失败，请重试……", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        _canClose = true;
                        this.Close();
                    }
                }));

                #endregion
            }
            else
            {
                // 检查是否需要强制升级TaskPlatform.TaskInterface.dll文件
                string fileName = LocalDataCachePath + "\\PlatformVersion.ini";
                if (!File.Exists(fileName) || File.ReadAllText(fileName) != Application.ProductVersion)
                {
                    //版本不是当前版本，则强制升级TaskPlatform.TaskInterface.dll文件
                    Process process = new Process();
                    process.StartInfo = new ProcessStartInfo("SafeMode.exe", "/cleanoldfile");
                    process.StartInfo.CreateNoWindow = true;
                    process.Start();
                    process.WaitForExit();
                }
                File.WriteAllText(fileName, Application.ProductVersion);
                ExecutingTaskCountInfo.Instance.MaxExecutingTaskCount = 2;
                LoadPlanEngine(true);
                // 启动参数加载器
                // 异步加载，加载完成后再启动其他的扫描器，避免出现执行顺序错误而导致部分Worker无法获取参数
                ArgsLoad(null);

                // 向计划引擎中载入系统计划
                ThreadPool.QueueUserWorkItem(AddSystemPlan);
                // 启动物理框架扫描
                ThreadPool.QueueUserWorkItem(DirScanner);
            }
        }

        #endregion

        #region 权限、创建IPC通道

        /// <summary>
        /// 判断程序是否是以管理员身份运行。
        /// </summary>
        internal bool IsRunAsAdmin()
        {
            WindowsIdentity id = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(id);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        /// <summary>
        /// 进程互斥(缺点：无法保证端口一致)
        /// </summary>
        private void ProgramMutexAndRegisterChannel()
        {
            RunningForm rf = new RunningForm();
            rf.Template = "该目录下计划任务平台已在运行，请勿重复开启。\r\n如果您是执行重启时看到的该提示，请进入该平台目录手工启动。\r\n\r\n(进程将在2FE2DB93-782A-4519-A73C-7C11B1E32C5E秒后退出)";
            rf.TemplateKeywords = "2FE2DB93-782A-4519-A73C-7C11B1E32C5E";
            if (RunningInstance() != null)
            {
                rf.Height -= 38;
                rf.btnStart.Enabled = false;
                rf.ShowDialog();
            }
            string portName = ConfigurationManager.AppSettings["IpcServerPortName"];
            if (!string.IsNullOrWhiteSpace(PlatformPortName))
            {
                portName = PlatformPortName;
            }
            AppDomain.CurrentDomain.SetData("TaskPlatformController", this);
            PlatformPortName = portName;
            //AppDomain.CurrentDomain.SetData("PlatformPortName", portName);
            try
            {
                BinaryClientFormatterSinkProvider clientProvider = new BinaryClientFormatterSinkProvider();
                BinaryServerFormatterSinkProvider serverProvider = new BinaryServerFormatterSinkProvider();
                IDictionary prop = new Hashtable();
                prop["portName"] = portName;
                prop["authorizedGroup"] = "Everyone";
                IpcChannel serverChannel = new IpcChannel(prop, clientProvider, serverProvider);
                ChannelServices.RegisterChannel(serverChannel, false);

                RemotingConfiguration.RegisterWellKnownServiceType(typeof(Mouthpiece), "Controller", WellKnownObjectMode.Singleton);
            }
            catch (Exception ex)
            {
                Log.CustomWrite(ex.ToString(), "TaskPlatform--PlatformFormException");
                rf.Template = string.Format("启动计划任务平台失败，Web控制台将无法连接至此平台。\r\n详情参见：{0}\r\n(进程将在2FE2DB93-782A-4519-A73C-7C11B1E32C5E秒后退出)", ex.Message);
                rf.TipText = ex.ToString();
                rf.ShowDialog();
                if (!isAutoIPC)
                {
                    PlatformPortName = Guid.NewGuid().ToString();
                    isAutoIPC = true;
                    XtraMessageBox.Show("系统即将注册临时IPC端口：\n\n" + PlatformPortName, "提示");
                    ProgramMutexAndRegisterChannel();
                }
            }
        }

        /// <summary>
        /// 运行中的实例
        /// </summary>
        /// <returns></returns>
        public static Process RunningInstance()
        {
            System.Diagnostics.Process current = System.Diagnostics.Process.GetCurrentProcess();
            System.Diagnostics.Process[] processes = System.Diagnostics.Process.GetProcessesByName(current.ProcessName);
            foreach (System.Diagnostics.Process process in processes)
            {
                if (process.Id != current.Id)
                {
                    if (System.Reflection.Assembly.GetExecutingAssembly().Location.Replace("/", @"\") == process.MainModule.FileName)
                    {
                        return process;
                    }
                }
            }
            return null;
        }

        #endregion

        #region 组件、参数加载和系统内置任务的添加

        /// <summary>
        /// 加载计划引擎
        /// </summary>
        private void LoadPlanEngine(bool first = false)
        {
            if (PlansEngine == null)
            {
                SetStatusInfo("加载计划引擎……");
                // 加载计划引擎
                PlansEngine = new PlanEngine.PlanEngine(tabContainer);
                PlansEngine.ExecutePlan += new TaskPlatform.PlanEngine.PlanEngine.ExecutePlanHandler(PlanEngine_ExecutePlan);
                // 初始化计划引擎
                PlansEngine.InitEngine();
            }
            else
            {
                SetStatusInfo("计划引擎已加载，无需再次加载。");
            }
            // 启动引擎，如果是启动时执行代码，则在扫描了dll之后启动引擎
            if (!first)
            {
                StartPlanEngine();
            }
        }

        /// <summary>
        /// 向计划引擎中载入系统计划
        /// </summary>
        /// <param name="state"></param>
        private void AddSystemPlan(object state)
        {
            if (PlansEngine == null)
                return;

            #region 内存清理

            Plan plan = new Plan();
            plan.Type = PlanType.Repeat;
            plan.PlanUnit = PlanTimeUnit.Minute;
            plan.Interval = 5;
            plan.PlanStartTime = DateTime.Now;
            plan.PlanEndTime = DateTime.MaxValue;
            plan.Enable = true;
            plan.PlanName = "%sys%ClearMemory";
            PlansEngine.AddPlan(plan);

            #endregion

            #region 统计线程池线程数

            Plan planThreadsCount = new Plan();
            planThreadsCount.Type = PlanType.Repeat;
            planThreadsCount.PlanUnit = PlanTimeUnit.Minute;
            planThreadsCount.Interval = 1;
            planThreadsCount.PlanStartTime = DateTime.Now;
            planThreadsCount.PlanEndTime = DateTime.MaxValue;
            planThreadsCount.Enable = true;
            planThreadsCount.PlanName = "%sys%CollectThreadsCount";
            PlansEngine.AddPlan(planThreadsCount);

            #endregion

            #region 内存深度清理

            Plan plan1 = new Plan();
            plan1.Type = PlanType.Repeat;
            plan1.PlanUnit = PlanTimeUnit.Minute;
            plan1.Interval = 10;
            plan1.PlanStartTime = DateTime.Now;
            plan1.PlanEndTime = DateTime.MaxValue;
            plan1.Enable = true;
            plan1.PlanName = "%sys%DeepClearMemory";
            PlansEngine.AddPlan(plan1);

            #endregion

            #region 收缩日志

            Plan plan3 = new Plan();
            plan3.Type = PlanType.Repeat;
            plan3.PlanUnit = PlanTimeUnit.Hour;
            plan3.Interval = 12;
            plan3.PlanStartTime = DateTime.Now;
            plan3.PlanEndTime = DateTime.MaxValue;
            plan3.Enable = true;
            plan3.PlanName = "%sys%ShrinkLog";
            PlansEngine.AddPlan(plan3);

            #endregion

            #region 检测运行中的任务是否异常

            Plan plan4 = new Plan();
            plan4.Type = PlanType.Repeat;
            plan4.PlanUnit = PlanTimeUnit.Minute;
            plan4.Interval = 30;
            plan4.PlanStartTime = DateTime.Now;
            plan4.PlanEndTime = DateTime.MaxValue;
            plan4.Enable = true;
            plan4.PlanName = "%sys%CheckRunningTasks";
            PlansEngine.AddPlan(plan4);

            #endregion
        }

        /// <summary>
        /// 参数加载器
        /// </summary>
        /// <param name="state"></param>
        private void ArgsLoad(object state)
        {
            if (appSettings == null || appSettings.Count <= 0)
            {
                foreach (var key in ConfigurationManager.AppSettings.AllKeys)
                {
                    appSettings.Add(key, ConfigurationManager.AppSettings[key]);
                }
                foreach (ConnectionStringSettings connection in ConfigurationManager.ConnectionStrings)
                {
                    connectionStrings.Add(connection.Name, connection.ConnectionString);
                }
                try
                {
                    Log.TaskPlatformDBConnectionString = connectionStrings["TaskPlatformDBConnectionString"];
                }
                catch { }
            }
            try
            {
                if (appSettings["MonitoringIsEnabled"].ToLower() == "true")
                {
                    if (!AppDomain.MonitoringIsEnabled)
                    {
                        AppDomain.MonitoringIsEnabled = true;
                    }
                }
            }
            catch { }
            string mustRunKey = "MustRunTasks";
            if (AlertSystemCache.ContainsKey(mustRunKey))
            {
                mustRunTasks = AlertSystemCache[mustRunKey].Value as List<string>;
            }
        }

        #endregion

        #region 扫描器

        /// <summary>
        /// 尝试启动计划任务引擎
        /// </summary>
        private void StartPlanEngine()
        {
            // 如果计划引擎已经加载
            // 如果计划引擎尚未启动，就启动计划引擎
            if (PlansEngine != null)
            {
                if (!_noPlanEngine)
                {
                    if (!PlansEngine.Started)
                    {
                        SetStatusInfo("启动计划引擎……");
                        PlansEngine.StartEngine();
                        SetStatusInfo("计划引擎启动成功。");
                    }
                    else
                    {
                        SetStatusInfo("计划引擎已启动，无需再次启动。");
                    }
                }
                else
                {
                    SetStatusInfo("不加载计划引擎模式下启动。");
                }
            }
        }

        /// <summary>
        /// 物理框架扫描器(该方法只支持主线程内调用，非UI线程内请勿调用)
        /// </summary>
        /// <param name="state">不使用该参数，不用传值。</param>
        private void DirScanner(object state)
        {
            DirScannerAlone();
            // 启动增量计划任务扫描器
            ThreadPool.QueueUserWorkItem(TasksScanner);
        }

        /// <summary>
        /// 物理框架扫描器(该方法只支持主线程内调用，非UI线程内请勿调用)
        /// </summary>
        private void DirScannerAlone()
        {
            // 显示进度条
            ResetProcessBar(14);
            // 扫描器已启动
            SetStatusInfo("检查物理框架完整性……");
            ProcessBar(2);

            #region 检查物理框架完整性

            if (!Directory.Exists(TaskFilesPath))
            {
                Directory.CreateDirectory(TaskFilesPath);
            }

            if (!Directory.Exists(LocalDataCachePath))
            {
                Directory.CreateDirectory(LocalDataCachePath);
            }
            ProcessBar(2);
            if (!Directory.Exists(SystemTaskFilesPath))
            {
                Directory.CreateDirectory(SystemTaskFilesPath);
            }
            ProcessBar(2);
            if (!Directory.Exists(ResoucesPath))
            {
                Directory.CreateDirectory(ResoucesPath);
            }
            if (!Directory.Exists(ResoucesPath + "\\Templates"))
            {
                Directory.CreateDirectory(ResoucesPath + "\\Templates");
            }
            ProcessBar(2);
            string logPath = MainLogPath + string.Format("\\{0}", DateTime.Now.ToString("yyyy-MM-dd"));
            ProcessBar(2);
            if (!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath);
            }
            ProcessBar(2);

            #endregion

            SetStatusInfo("启动计划任务目录卷影复制……");
            Thread.Sleep(200);
            ProcessBar(2);
        }

        /// <summary>
        /// 启动增量计划任务扫描器。
        /// </summary>
        /// <param name="state"></param>
        private void TasksScanner(object state)
        {
            Monitor.Enter(_tasksScanner);
            try
            {
                // 启动缓存
                if (PlanPlatformCache == null)
                {
                    PlanPlatformCache = new LocalDataCacheContainer(DefaultCacheContainerName);
                }
                SetStatusInfo("增量计划任务扫描……");
                ResetProcessBar(1);
                List<string> files = new List<string>();
                string[] tasksRootPath = { TaskFilesPath, SystemTaskFilesPath };
                int count = 0;
                foreach (string taskRootPath in tasksRootPath)
                {
                    string[] tasksFolders = Directory.GetDirectories(taskRootPath);
                    foreach (string item in tasksFolders)
                    {
                        string itemCopy = item.TrimEnd('\\');
                        int startIndex = itemCopy.LastIndexOf('\\') + 1;
                        itemCopy = itemCopy + "\\" + itemCopy.Substring(startIndex) + ".dll";
                        if (File.Exists(itemCopy))
                        {
                            files.Add(itemCopy);
                        }
                    }
                    ProcessBar(1);
                    count = files == null ? 0 : files.Count;
                    if (count <= 0)
                    {
                        SetStatusInfo("扫描结束。未发现新计划任务。");
                        ResetProcessBar(0);
                        continue;
                    }
                    // 移除已经加载过的计划任务
                    foreach (TaskGroup group in TaskGroupList)
                    {
                        if (group == null || group.GroupControl == null)
                        {
                            continue;
                        }
                        foreach (NavBarItemLink item in group.GroupControl.ItemLinks)
                        {
                            string etask = item.Item.Name.Replace("%NC%", "").Replace("%NP%", "");
                            etask = taskRootPath + "\\" + etask + "\\" + etask + ".dll";
                            if (files.Contains(etask))
                            {
                                files.Remove(etask);
                            }
                        }
                    }
                }

                count = files == null ? 0 : files.Count;
                if (count <= 0)
                {
                    SetStatusInfo("扫描结束。未发现新计划任务。");
                    ResetProcessBar(0);
                    return;
                }
                ResetProcessBar(files.Count());
                foreach (var filePath in files)
                {
                    LoadTask(filePath);
                }
                SetStatusInfo("计划任务加载完成。");
                ResetProcessBar(0);
            }
            catch { }
            Monitor.Exit(_tasksScanner);
            StartPlanEngine();
            ResetTaskList();
            SaveAlterSystemCache();
            SaveTaskGroupCache();
            CollectDomainList();
        }

        /// <summary>
        /// 加载一个计划任务
        /// </summary>
        /// <param name="filePath">计划任务文件路径</param>
        /// <returns>0未加载1无配置文件2加载器出错3加载出错4加载成功尚未配置计划6加载成功计划未启用7加载成功计划已启用</returns>
        internal int LoadTask(string filePath, bool addDomainList = false)
        {
            int result = 0;
            string taskName;
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            List<string> mirrorList = MirrorService.GetTaskMirror(fileName);
            int taskCount = mirrorList.Count + 1;
            ResetProcessBar(taskCount);
            AlarmObjectComparer alarmComparer = new AlarmObjectComparer();
            for (int i = 0; i < taskCount; i++)
            {
                if (i == 0)
                {
                    taskName = string.Concat(fileName, "");
                }
                else
                {
                    taskName = string.Concat(fileName, "-", mirrorList[i - 1]);
                }
                if (GetDomainLoader(taskName) != null)
                {
                    ProcessBar(1);
                    continue;
                }
                SetStatusInfo("开始加载：" + taskName);
                if (!File.Exists(String.Format("{0}.config", filePath)))
                {
                    SetStatusInfo("计划任务 " + taskName + " 没有配置文件，将生成链接，但不加载计划任务。");
                    CreateTaskWithNoConfig(filePath);
                    ProcessBar(1);
                    result = 1;
                }
                else
                {
                    SetStatusInfo("为 " + taskName + " 创建运行时域。");
                    DomainLoader loader = new DomainLoader(filePath);
                    loader.DomainName = taskName;
                    loader.TaskName = fileName;
                    loader.PlatformAppSettings = appSettings;
                    loader.PlatformConnectionStrings = connectionStrings;
                    loader.IsMirrorTask = i > 0;
                    if (loader.IsMirrorTask)
                    {
                        loader.MirrorName = mirrorList[i - 1];
                    }
                    try
                    {
                        result = CreateTask(loader);
                        if (addDomainList && result > 3)
                        {
                            if (GetDomainLoader(loader.DomainName, true) == null)
                            {
                                domainLoaderList.Add(loader);
                            }
                            try
                            {
                                AlarmObject alarmObject = new AlarmObject() { PlanName = loader.DomainName };
                                if (!AlarmObjects.Contains<AlarmObject>(alarmObject, alarmComparer))
                                {
                                    alarmObject.TaskName = loader.TaskOperator.TaskName();
                                    alarmObject.AlarmType = 1;
                                    AlarmObjects.Add(alarmObject);
                                }
                            }
                            catch { }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.CustomWrite(ex.ToString(), "TaskPlatform--PlatformFormException");
                        result = 2;
                    }
                    ProcessBar(1);
                }
            }
            ResetProcessBar(0);
            return result;
        }

        /// <summary>
        /// 收集已加载的计划任务
        /// </summary>
        private void CollectDomainList()
        {
            try
            {
                //domainLoaderList.Clear();
                List<DomainLoader> domainList = new List<DomainLoader>();
                AlarmObjectComparer alarmComparer = new AlarmObjectComparer();
                foreach (NavBarGroup group in nbcContainer.Groups)
                {
                    if (group.Tag != null && group.Tag.ToString() == "PaltformManagementGroup")
                    {
                        continue;
                    }
                    foreach (NavBarItemLink item in group.ItemLinks)
                    {
                        try
                        {
                            DomainLoader loader = item.Item.Tag as DomainLoader;
                            if (loader != null)
                            {
                                domainList.Add(loader);
                                try
                                {
                                    AlarmObject _alarmObject = new AlarmObject() { PlanName = loader.DomainName };
                                    if (!AlarmObjects.Contains<AlarmObject>(_alarmObject, alarmComparer))
                                    {
                                        _alarmObject.TaskName = loader.TaskOperator.TaskName();
                                        _alarmObject.AlarmType = 1;
                                        AlarmObjects.Add(_alarmObject);
                                    }
                                }
                                catch { }
                            }
                        }
                        catch { }
                    }
                }
                domainLoaderList = domainList;
            }
            catch
            {
                SetStatusInfo("收集计划任务性能指标出错。");
            }
        }

        #endregion

        #region 创建计划任务

        /// <summary>
        /// 创建没有配置文件的任务链接。
        /// </summary>
        /// <param name="fileName"></param>
        private void CreateTaskWithNoConfig(string fileName)
        {
            string name = Path.GetFileNameWithoutExtension(fileName);
            NavBarItem nbiNoConfigItem = new NavBarItem();
            nbiNoConfigItem.Caption = Path.GetFileNameWithoutExtension(fileName);
            nbiNoConfigItem.CanDrag = false;
            nbiNoConfigItem.LinkClicked += new NavBarLinkEventHandler(nbiNoConfigItem_LinkClicked);
            nbiNoConfigItem.Tag = fileName;
            nbiNoConfigItem.Name = "%NC%" + name;
            nbiNoConfigItem.Hint = string.Format("计划任务 {0} 因没有配置文件而未加载。\n点击尝试重新加载。", Path.GetFileName(fileName));
            // 没有配置文件的计划任务链接
            nbiNoConfigItem.SmallImage = images.Images["Annotate_Warning.ico"];
            nbcContainer.Invoke(new MethodInvoker(() =>
            {
                GetDefalutTaskGroup().GroupControl.ItemLinks.Add(nbiNoConfigItem);
            }));
        }

        /// <summary>
        /// 创建计划任务链接
        /// </summary>
        /// <param name="loader">计划任务运行时域加载器.</param>
        /// <returns>0未加载1无配置文件2加载器出错3加载出错4加载成功尚未配置计划5加载成功计划未启用6加载成功计划已启用</returns>
        private int CreateTask(DomainLoader loader)
        {
            int result = 3;
            NavBarItem nbiTaskItem = new NavBarItem();
            nbiTaskItem.CanDrag = false;
            nbiTaskItem.Tag = loader;
            if (loader.IsMirrorTask)
            {
                nbiTaskItem.Appearance.Options.UseForeColor = true;
                nbiTaskItem.Appearance.ForeColor = Color.Gray;
            }
            try
            {
                string erroeInfo = loader.CreateTaskDomain(PlatformPortName);
                if (!string.IsNullOrWhiteSpace(erroeInfo))
                {
                    throw new Exception(erroeInfo);
                }
                try
                {
                    loader.TaskOperator.InitTask(appSettings, connectionStrings);
                }
                catch (Exception ex)
                {
                    Log.CustomWrite(ex.ToString(), "TaskPlatform--PlatformFormException");
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                #region 加载错误计划任务
                Log.CustomWrite(ex.ToString(), "TaskPlatform--PlatformFormException");
                // 载入计划任务失败，添加点击删除项链接。
                nbiTaskItem.Tag = "%ER%" + loader.FileName;
                nbiTaskItem.Caption = Path.GetFileNameWithoutExtension(loader.FileName);
                try
                {
                    // 卸载域
                    loader.UnLoadTaskDomian();
                    loader = null;
                }
                catch (Exception exception)
                {
                    loader = null;
                    Log.CustomWrite(exception.ToString(), "TaskPlatform--PlatformFormException");
                }
                nbiTaskItem.Hint = string.Format("计划任务载入失败。可能是无效dll。\n点击清除该链接并删除相应dll。\r\n\r\n请参考：\r\n{0}", ex.ToString());
                nbiTaskItem.SmallImage = GetImage("Annotate_Error.ico");
                // 添加点击处理事件。移除该计划任务链接。
                nbiTaskItem.LinkClicked += (sender, item) =>
                {
                    GetDefalutTaskGroup().GroupControl.ItemLinks.Remove(item.Link.Item);
                    ResetTaskList();
                    if (XtraMessageBox.Show("该计划任务存在错误，已经被移出平台。\n\n是否删除对应的dll文件？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        try
                        {
                            File.Delete(item.Link.Item.Tag.ToString().Replace("%ER%", ""));
                            XtraMessageBox.Show("链接已清除，dll文件已被删除。", "平台提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception exe)
                        {
                            Log.CustomWrite(exe.ToString(), "TaskPlatform--PlatformFormException");
                            XtraMessageBox.Show("链接已清除，但dll文件删除失败。", "平台提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                };
                result = 3;
                goto AddItem;
                #endregion
            }
            nbiTaskItem.LinkClicked += new NavBarLinkEventHandler(nbiTaskItem_LinkClicked);
            // 判断是否缓存有计划
            if (PlanPlatformCache.ContainsKey("Plan:" + loader.DomainName))
            {
                Plan plan = PlanPlatformCache["Plan:" + loader.DomainName].Value as Plan;
                loader.Plan = plan;
                // 正常载入的计划任务链接
                nbiTaskItem.Name = loader.DomainName;
                CheckTaskConfig(loader);
                try
                {
                    // 尝试一次获取计划任务名称
                    nbiTaskItem.Caption = loader.TaskOperator.TaskName();
                }
                catch { }
                if (plan.Enable)
                {
                    nbiTaskItem.Hint = string.Format("计划任务 {0}({1}) 已创建成功。\n已载入平台。\n正在运行。\r\n\r\n描述：\n{2}", nbiTaskItem.Caption, loader.DomainName, loader.TaskOperator.TaskDescription());
                    nbiTaskItem.SmallImage = GetImage("Right");
                    if (mustRunTasksFirstRun && !MustRunTasks.Contains(loader.DomainName))
                    {
                        MustRunTasks.Add(loader.DomainName);
                    }
                    result = 6;
                }
                else
                {
                    nbiTaskItem.Hint = string.Format("计划任务 {0}({1}) 已创建成功。\n已载入平台。\n其计划已停用。\r\n\r\n描述：\n{2}", nbiTaskItem.Caption, loader.DomainName, loader.TaskOperator.TaskDescription());
                    nbiTaskItem.SmallImage = GetImage("Wait");
                    result = 5;
                }
                if (plan != null)
                    PlansEngine.AddPlan(plan);
            }
            else
            {
                CheckTaskConfig(loader);
                try
                {
                    // 尝试一次获取计划任务名称
                    nbiTaskItem.Caption = loader.TaskOperator.TaskName();
                }
                catch { }
                // 没有计划的计划任务链接
                nbiTaskItem.Name = "%NP%" + loader.DomainName;
                nbiTaskItem.Hint = string.Format("计划任务 {0}({1}) 已创建成功。\n已载入平台。\n但尚未配置计划，点击可配置计划。\r\n\r\n描述：\n{2}", nbiTaskItem.Caption, loader.DomainName, loader.TaskOperator.TaskDescription());
                nbiTaskItem.SmallImage = GetImage("Annotate_info.ico");
                result = 4;
            }
        AddItem:
            NavBarGroup groupControl = nbgTasks;
            if (loader != null)
            {
                try
                {
                    // 尝试一次获取计划任务名称
                    nbiTaskItem.Caption = loader.TaskOperator.TaskName();
                }
                catch { }
                #region 获取任务分组
                try
                {
                    TaskGroup group = GetTaskGroup(loader.DomainName);
                    if (group != null && group.GroupControl != null)
                    {
                        groupControl = group.GroupControl;
                    }
                    else
                    {
                        GetDefalutTaskGroup().TaskList.Add(loader.DomainName);
                    }
                }
                catch (Exception ex)
                {
                    groupControl = nbgTasks;
                    Log.CustomWrite(ex.ToString(), "TaskPlatform--PlatformFormException");
                }
                #endregion
            }
            SetStatusInfo("计划任务 " + nbiTaskItem.Caption + "(" + (loader == null ? nbiTaskItem.Caption : loader.DomainName) + ") 载入完成。");
            nbcContainer.Invoke(new MethodInvoker(() =>
            {
                groupControl.ItemLinks.Add(nbiTaskItem);
                nbiTaskItem.NavBar.MouseDown += new MouseEventHandler(NavBar_MouseDown);
            }));
            return result;
        }

        /// <summary>
        /// 右键菜单是否已经打开
        /// </summary>
        bool executed = false;
        private void NavBar_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                NavBarControl navBarControl = sender as NavBarControl;
                if (navBarControl.HotTrackedLink != null && !executed)
                {
                    // 可以显示右键菜单喽！！！
                    //ShowInfo(navBarControl.HotTrackedLink.Item.Caption);
                    _currentClickItem = navBarControl.HotTrackedLink.Item;
                    executed = true;
                    OpenPopup();
                }
            }
        }

        private void popupMenu_CloseUp(object sender, EventArgs e)
        {
            // 当右键菜单关闭，就置[右键菜单是否已经打开]为false
            executed = false;
        }

        /// <summary>
        /// 获取任务分组
        /// </summary>
        /// <param name="taskKey">计划任务Key值</param>
        /// <param name="create">不存在分组控件时是否创建(默认创建)</param>
        /// <returns></returns>
        private TaskGroup GetTaskGroup(string taskKey, bool create = true)
        {
            TaskGroup group = (from f in TaskGroupList
                               where f.TaskList.Contains(taskKey)
                               select f).FirstOrDefault();
            if (create && group != null && group.GroupControl == null)
            {
                NavBarGroup navBarGroup = new NavBarGroup(group.GroupName);
                navBarGroup.SmallImageIndex = 1;
                navBarGroup.Hint = string.Format("列出分组【{0}】的计划任务信息", group.GroupName);
                nbcContainer.Groups.Insert(nbcContainer.Groups.Count - 1, navBarGroup);
            }
            return group;
        }

        /// <summary>
        /// 获取默认分组
        /// </summary>
        /// <returns></returns>
        private TaskGroup GetDefalutTaskGroup()
        {
            TaskGroup group = (from f in TaskGroupList
                               where f.IsDefault
                               select f).FirstOrDefault();
            return group;
        }

        /// <summary>
        /// 更新显示项名称
        /// </summary>
        /// <param name="loader">运行时域</param>
        public void UpdateItemCaption(DomainLoader loader)
        {
            try
            {
                GetNavBarItem(loader.DomainName).Caption = loader.TaskOperator.Task.TaskName();
            }
            catch { }
            try
            {
                foreach (XtraTabPage tab in tabContainer.TabPages)
                {
                    if (tab.Name == loader.DomainName)
                    {
                        tab.Text = loader.TaskOperator.Task.TaskName();
                        break;
                    }
                }
            }
            catch { }
            try
            {
                OnTaskUpdate(loader.DomainName);
            }
            catch { }
        }

        private static void CheckTaskConfig(DomainLoader loader)
        {
            try
            {
                // 处理计划任务的自定义配置项
                if (PlanPlatformCache.ContainsKey("TaskConfig:" + loader.DomainName))
                {
                    Dictionary<string, string> configs = PlanPlatformCache["TaskConfig:" + loader.DomainName].Value as Dictionary<string, string>;
                    loader.TaskOperator.DownloadConfig(configs);
                    // 更新缓存
                    PlanPlatformCache.Update(new LoaclDataCacheObject("TaskConfig:" + loader.DomainName, loader.TaskOperator.UploadConfig(), "TaskConfig:" + loader.DomainName));
                }
                else
                {
                    PlanPlatformCache.Update(new LoaclDataCacheObject("TaskConfig:" + loader.DomainName, loader.TaskOperator.UploadConfig()));
                }
            }
            catch (Exception exe)
            {
                Log.CustomWrite(exe.ToString(), "TaskPlatform--PlatformFormException");
            }
        }

        #endregion

        #region 业务处理

        private void nbiNoConfigItem_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            if (!e.Link.Item.Name.StartsWith("%NC%"))
                return;
            string fileName = e.Link.Item.Tag.ToString();
            string configPath = fileName + ".config";
            if (!File.Exists(configPath))
            {
                XtraMessageBox.Show(this, "未找到相应的配置文件。", "载入失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                GetDefalutTaskGroup().GroupControl.ItemLinks.Remove(e.Link.Item);
                // 启动增量计划任务扫描器
                ThreadPool.QueueUserWorkItem(TasksScanner);
            }
        }

        private void nbiTaskItem_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            // 此时右键菜单肯定没开着，置[右键菜单是否已经打开]为false
            executed = false;
            _currentClickItem = e.Link.Item;
            //bsubItemGroup.ItemLinks.Clear();
            //OpenPopup();
            DomainLoader loader = e.Link.Item.Tag as DomainLoader;
            if (loader != null)
            {
                OpenTaskStage(loader);
            }
        }

        private void OpenPopup()
        {
            if (barbtnOpenDirectory.ImageIndex != 16)
            {
                barbtnOpenDirectory.ImageIndex = 16;
            }
            foreach (TaskGroup groupItem in TaskGroupList)
            {
                var item = (from f in bsubMoveToGroup.ItemLinks.Cast<BarItemLink>()
                            where f.Caption == groupItem.GroupName
                            select f).FirstOrDefault();
                if (item != null)
                    continue;

                #region 为移动到分组创建菜单

                BarButtonItem btnItem = new BarButtonItem(barManager, groupItem.GroupName);
                btnItem.ItemClick += (bitem, ie) =>
                {
                    if (_currentClickItem != null)
                    {
                        // 移动到分组
                        DomainLoader loader = _currentClickItem.Tag as DomainLoader;
                        if (MoveTaskToGroup(loader, ie.Item.Caption))
                        {
                            SaveTaskGroupCache();
                        }
                    }
                };
                bsubMoveToGroup.ItemLinks.Add(btnItem);

                #endregion

                if (groupItem.GroupName != "默认分组")
                {
                    #region 删除分组

                    BarButtonItem btnDelItem = new BarButtonItem(barManager, groupItem.GroupName);
                    btnDelItem.ItemClick += (bitem, ie) =>
                    {
                        // 删除分组
                        if (DeleteTaskGroup(ie.Item.Caption))
                        {
                            bsubDeleteGroup.ItemLinks.Remove(ie.Link);
                            var link = (from f in bsubMoveToGroup.ItemLinks.Cast<BarItemLink>()
                                        where f.Caption == ie.Item.Caption
                                        select f).FirstOrDefault();
                            if (link != null)
                            {
                                bsubMoveToGroup.ItemLinks.Remove(link);
                            }
                            var linkRename = (from f in bsubRenameGroup.ItemLinks.Cast<BarItemLink>()
                                              where f.Caption == ie.Item.Caption
                                              select f).FirstOrDefault();
                            if (linkRename != null)
                            {
                                bsubRenameGroup.ItemLinks.Remove(linkRename);
                            }
                            SaveTaskGroupCache();
                        }
                    };
                    bsubDeleteGroup.ItemLinks.Add(btnDelItem);

                    #endregion

                    #region 重命名分组

                    BarButtonItem btnRenameItem = new BarButtonItem(barManager, groupItem.GroupName);
                    btnRenameItem.ItemClick += (bitem, ie) =>
                    {
                        // 重命名分组
                        ManageTextForm form = new ManageTextForm();
                        form.StringText = ie.Item.Caption;
                        form.ShowDialog();
                        if (form.IsOK && form.StringText.ToLower() != ie.Item.Caption.ToLower())
                        {
                            TaskGroup group = (from f in TaskGroupList
                                               where f.GroupName.ToLower() == form.StringText.ToLower()
                                               select f).FirstOrDefault();
                            if (group != null)
                            {
                                ShowWarning(SetStatusInfo(string.Format("分组【{0}】已存在，不允许重命名为该名称。", form.StringText)));
                            }
                            else
                            {
                                group = (from f in TaskGroupList
                                         where f.GroupName.ToLower() == ie.Item.Caption.ToLower()
                                         select f).FirstOrDefault();
                                if (group == null)
                                {
                                    ShowWarning("未找到该分组相对应控件。");
                                }
                                else
                                {
                                    group.GroupControl.Caption = group.GroupName = form.StringText;
                                    var link = (from f in bsubMoveToGroup.ItemLinks.Cast<BarItemLink>()
                                                where f.Caption == ie.Item.Caption
                                                select f).FirstOrDefault();
                                    if (link != null)
                                    {
                                        link.Caption = form.StringText;
                                    }
                                    var linkDel = (from f in bsubDeleteGroup.ItemLinks.Cast<BarItemLink>()
                                                   where f.Caption == ie.Item.Caption
                                                   select f).FirstOrDefault();
                                    if (linkDel != null)
                                    {
                                        linkDel.Caption = form.StringText;
                                    }
                                    ie.Item.Caption = form.StringText;
                                    SaveTaskGroupCache();
                                }
                            }
                        }
                    };
                    bsubRenameGroup.ItemLinks.Add(btnRenameItem);

                    #endregion
                }
            }

            popupMenu.ShowPopup(MousePosition);
        }

        /// <summary>
        /// 移动计划任务到分组
        /// </summary>
        /// <param name="loader"></param>
        /// <param name="groupName"></param>
        private bool MoveTaskToGroup(DomainLoader loader, string groupName)
        {
            if (string.IsNullOrWhiteSpace(groupName))
                return false;
            TaskGroup group = (from f in TaskGroupList
                               where f.GroupName.ToLower() == groupName.ToLower()
                               select f).FirstOrDefault();
            if (group == null)
            {
                ShowError("没有找到该分组。");
            }
            else
            {
                NavBarItem taskItem = GetNavBarItem(loader.DomainName);
                TaskGroup fromGroup = GetTaskGroup(loader.DomainName);
                fromGroup.TaskList.Remove(loader.DomainName);
                group.TaskList.Add(loader.DomainName);
                fromGroup.GroupControl.ItemLinks.Remove(taskItem);
                group.GroupControl.ItemLinks.Add(taskItem);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 删除分组
        /// </summary>
        /// <param name="groupName"></param>
        private bool DeleteTaskGroup(string groupName)
        {
            if (string.IsNullOrWhiteSpace(groupName))
                return false;
            TaskGroup group = (from f in TaskGroupList
                               where f.GroupName.ToLower() == groupName.ToLower()
                               select f).FirstOrDefault();
            if (group == null)
            {
                ShowError("没有找到该分组。");
            }
            else
            {
                if (group.TaskList.Count > 0)
                {
                    if (XtraMessageBox.Show(string.Format("该分组里存在{0}个计划任务。\n删除该分组后，该分组下的计划任务将被移到默认分组中。\n\n是否继续删除？", group.TaskList.Count), "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return false;
                    }
                }
                if (group.GroupControl == null)
                {
                    ShowWarning("没有找到该分组对应的任务列表控件。");
                }
                else
                {
                    TaskGroup defalutGroup = GetDefalutTaskGroup();
                    if (group.TaskList.Count > 0)
                    {
                        var taskItems = group.GroupControl.ItemLinks.Cast<NavBarItemLink>().ToArray();
                        group.GroupControl.ItemLinks.Clear();
                        defalutGroup.GroupControl.ItemLinks.AddRange(taskItems);
                        group.TaskList.Distinct().ToList().ForEach(taskKey =>
                        {
                            if (!defalutGroup.TaskList.Contains(taskKey))
                            {
                                defalutGroup.TaskList.Add(taskKey);
                            }
                        });
                    }
                    TaskGroupList.Remove(group);
                    nbcContainer.Groups.Remove(group.GroupControl);
                    return true;
                }
            }
            return false;
        }

        private void btnAddGroup_ItemClick(object sender, ItemClickEventArgs e)
        {
            ManageTextForm form = new ManageTextForm();
            form.ShowDialog();
            if (form.IsOK)
            {
                TaskGroup group = (from f in TaskGroupList
                                   where f.GroupName.ToLower() == form.StringText.ToLower()
                                   select f).FirstOrDefault();
                if (group != null)
                {
                    ShowWarning(SetStatusInfo(string.Format("分组【{0}】已存在，不允许重复添加。", form.StringText)));
                }
                else
                {
                    group = new TaskGroup();
                    group.GroupName = form.StringText;
                    group.IsDefault = false;
                    NavBarGroup groupControl = new NavBarGroup(group.GroupName);
                    groupControl.SmallImageIndex = 1;
                    nbcContainer.Groups.Insert(nbcContainer.Groups.Count - 1, groupControl);
                    group.GroupControl = groupControl;
                    TaskGroupList.Add(group);
                    SaveTaskGroupCache();
                }
            }
        }

        /// <summary>
        /// 显示普通通知信息
        /// </summary>
        /// <param name="content"></param>
        public void ShowInfo(string content)
        {
            XtraMessageBox.Show(content, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 显示警告信息
        /// </summary>
        /// <param name="content"></param>
        public void ShowWarning(string content)
        {
            XtraMessageBox.Show(content, "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// 显示错误信息
        /// </summary>
        /// <param name="content"></param>
        public void ShowError(string content)
        {
            XtraMessageBox.Show(content, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// 显示确认信息
        /// </summary>
        /// <param name="content">确认内容</param>
        public bool ShowConfirm(string content, bool defaultYes = true)
        {
            return XtraMessageBox.Show(content, "请确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question, defaultYes ? MessageBoxDefaultButton.Button1 : MessageBoxDefaultButton.Button2) == DialogResult.Yes;
        }

        /// <summary>
        /// 显示决定信息
        /// </summary>
        /// <param name="content">决定信息</param>
        public DialogResult ShowDecide(string content)
        {
            return XtraMessageBox.Show(content, "请决定", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
        }

        public void OpenTaskStage(DomainLoader loader)
        {
            // 判断是否已经打开，如果已经打开，就选中。
            // 如果找不到，就创建选项卡，并选中。
            foreach (XtraTabPage tab in tabContainer.TabPages)
            {
                if (tab.Name == loader.DomainName)
                {
                    tabContainer.SelectedTabPage = tab;
                    return;
                }
            }
            XtraTabPage tabTask = new XtraTabPage();
            tabTask.Name = loader.DomainName;
            tabTask.Text = loader.TaskOperator.TaskName();
            tabTask.ShowCloseButton = DevExpress.Utils.DefaultBoolean.True;
            TaskStage taskStage = new TaskStage();
            taskStage.Name = "taskStage" + tabTask.Name;
            taskStage.Loader = loader;
            this.ExecutedPlan += taskStage.ExecutedPlan;
            this.ExecutePlan += taskStage.ExecutePlan;
            this.TaskUpdate += taskStage.TaskUpdate;
            tabTask.Controls.Add(taskStage);
            tabContainer.TabPages.Add(tabTask);
            tabContainer.SelectedTabPage = tabTask;
        }

        private void TaskDomain_DomainException(object loader, DomainExceptionEventArgs e)
        {
            // 暂时不会执行到此处的代码<暂时没有做到跨域传出异常>
            StringBuilder logContent = new StringBuilder();
            logContent.AppendLine("计划任务:" + ((DomainLoader)loader).DomainName);
            logContent.AppendLine("CLR是否终止:" + (e.IsTerminating ? "是" : "否"));
            logContent.AppendLine("域内发送者对象:" + (e.ExceptionSender ?? "").ToString());
            try
            {
                logContent.AppendLine("异常对象:" + (e.ExceptionObject as Exception).ToString());
            }
            catch { }
            Log.CustomWrite(logContent.ToString(), "System--DomainException");
        }

        private void PlanEngine_ExecutePlan(object sender, PlanEngine.ExecutePlanEventArgs e)
        {
            //Parallel.Invoke(() =>
            //{
            //    FireExecutedPlan(e);
            //});

            // 如果已被标记循环执行，则不再排入任务
            if (LoopTasks.Contains(e.PlanName))
            {
                return;
            }
            ThreadPool.QueueUserWorkItem(FireExecutedPlan, e);
        }

        /// <summary>
        /// 触发计划执行后的通知事件。
        /// </summary>
        /// <param name="args">The args.</param>
        private void FireExecutedPlan(object args)
        {
            string planName = string.Empty;
            bool loop = false;
            ExecutePlanEventArgs e = args as ExecutePlanEventArgs;
            try
            {
                if (e == null)
                {
                    return;
                }
                // 注册运行
                bool register = ExecutingTaskCountInfo.Instance.RegisterExecutingTask(e.PlanName);
                // 如果不允许继续加入队列，就直接结束
                if (!register)
                {
                    //Console.WriteLine("R-No");
                    return;
                }
                planName = e.PlanName;
                if (e.PlanName.StartsWith("%sys%"))
                {
                    BeginInvoke(new MethodInvoker(delegate()
                    {
                        ExecuteSystemPlan(e);
                    }));
                    return;
                }

                try
                {
                    ExecutingPlanEventArgs earg = new ExecutingPlanEventArgs();
                    earg.ExecuteType = ExecuteType.Before;
                    earg.ExecuteTime = DateTime.Now;
                    earg.PlanName = e.PlanName;
                    OnExecutePlan(earg);
                }
                catch (Exception exceptionArg)
                {
                    Log.CustomWrite(exceptionArg.ToString(), "TaskPlatform--PlatformFormException");
                }

                ExecutedPlanEventArgs arg = new ExecutedPlanEventArgs();
                arg.ExecuteTime = e.ExecuteTime;
                arg.PlanName = e.PlanName;
                DomainLoader loader = GetDomainLoader(e.PlanName);
                TimeSpan factExecuteTime = new TimeSpan(0, 0, 0);
                // 预先初始化字段
                arg.ProcessTraceTimeSpan = arg.ProcessTimeSpan = factExecuteTime;
                DateTime runStartTime = DateTime.Now;
                string waitTimeOutKey = "%sys%WaitTimeOut";
                LogObject log = new LogObject();
                log.TrigTime = e.ExecuteTime;
                RunTaskResult result = null;
                try
                {
                    AlarmObject alarmObject = AlarmObjects.Find(item => { return item.PlanName == e.PlanName; });
                    int timeOut = (int)((loader.Plan == null ? 5 : loader.Plan.TotalSeconds) * (alarmObject == null ? 1 : alarmObject.TimeOutRatio));
                    result = loader.TaskOperator.RunTaskWithTime(timeOut);

                    if (loader.Plan != null && loader.Plan.Enable && loader.TaskOperator.SupportAdvancedFeatures)
                    {
                        loop = loader.TaskOperator.AdvancedTask.Loop;
                    }

                    try
                    {
                        ExecutingPlanEventArgs earg = new ExecutingPlanEventArgs();
                        earg.ExecuteType = ExecuteType.After;
                        earg.ExecuteTime = DateTime.Now;
                        earg.PlanName = e.PlanName;
                        OnExecutePlan(earg);
                    }
                    catch (Exception exceptionArg)
                    {
                        Log.CustomWrite(exceptionArg.ToString(), "TaskPlatform--PlatformFormException");
                    }

                    // 释放注册的运行队列位置
                    ExecutingTaskCountInfo.Instance.UnRegisterExecutingTask(planName);

                    // 如果没要求循环执行，就看一下是否要求下一秒再次执行
                    if (!loop && result.RunAgain)
                    {
                        try
                        {
                            loader.Plan.ComputeTime(true, true);
                        }
                        catch (Exception exce)
                        {
                            Log.CustomWrite(exce.ToString(), "TaskPlatform--PlatformFormException");
                        }
                    }
                    arg.RunResult = result.Result;
                    arg.Success = result.Success;
                    log.Title = loader.TaskOperator.TaskName();
                    log.TaskName = e.PlanName;
                    log.EndExecuteTime = DateTime.Now;
                    if (!result.IsCanceled)
                    {
                        factExecuteTime = (TimeSpan)result.CustomerArgs["TaskRunTimeSpan"];
                        arg.ProcessTraceTimeSpan = (DateTime)result.CustomerArgs["TaskRunStartTime"] - runStartTime;
                        log.StartExecuteTime = (DateTime)result.CustomerArgs["TaskRunStartTime"];
                    }
                }
                catch (Exception exception)
                {
                    Log.CustomWrite(exception.ToString(), "TaskPlatform--PlatformFormException");
                    arg.RunResult = "(执行异常)" + exception.ToString();
                    arg.Success = false;
                }
                arg.ProcessTimeSpan = factExecuteTime;
                StringBuilder logContent = new StringBuilder();
                logContent.AppendLine("触发时间：" + arg.ExecuteTime.ToString("yyyy-MM-dd HH:mm:ss"));
                TaskRunInfo runInfo = new TaskRunInfo() { PlanName = arg.PlanName };
                if (result != null && !result.IsCanceled)
                {
                    runInfo.RunResult = arg.Success ? 1 : 0;
                    logContent.AppendLine("线程排队耗时：" + arg.ProcessTraceTimeSpan.ToString("G"));
                    logContent.AppendLine("实际执行耗时：" + arg.ProcessTimeSpan.ToString("G"));
                    log.ThreadQueueTime = arg.ProcessTraceTimeSpan.ToString("G");
                    log.ExecuteTime = arg.ProcessTimeSpan.ToString("G");
                }
                else
                {
                    runInfo.RunResult = -1;
                    arg.RunResult = arg.RunResult.Replace(waitTimeOutKey, "<线程等待超时，已自动取消执行。>");
                    log.IsCancle = true;
                }

                #region 写入运行信息统计

                if (!_taskRunStatistics.ContainsKey(runInfo.PlanName))
                {
                    lock (_taskRunStatisticsLock)
                    {
                        if (!_taskRunStatistics.ContainsKey(runInfo.PlanName))
                        {
                            _taskRunStatistics.Add(runInfo.PlanName, new List<TaskRunInfo>() { runInfo });
                        }
                    }
                }
                else
                {
                    lock (_taskRunStatisticsLock)
                    {
                        _taskRunStatistics[runInfo.PlanName].Add(runInfo);
                    }
                }

                #endregion

                logContent.AppendLine("是否成功：" + (arg.Success ? "是" : "否"));
                logContent.Append("执行结果：" + arg.RunResult);
                log.IsSuccess = arg.Success;
                log.RunTaskResult = arg.RunResult;
                log.Content = logContent.ToString();
                log.DateTime = DateTime.Now;
                log.LogType = LogType.Tasks;
                Log.Write(log);
                OnExecutedPlan(arg);
            }
            catch (Exception exception)
            {
                if (!string.IsNullOrWhiteSpace(planName))
                {
                    try
                    {
                        ExecutingTaskCountInfo.Instance.UnRegisterExecutingTask(planName);
                    }
                    catch { }
                }
                Log.CustomWrite(exception.ToString(), "TaskPlatform--PlatformFormException");
            }

            // 如果要求循环执行，则再次排入任务
            if (loop)
            {
                if (!LoopTasks.Contains(planName))
                {
                    try
                    {
                        LoopTasks.Add(planName);
                    }
                    catch { }
                }
                e.ExecuteTime = DateTime.Now;
                ThreadPool.QueueUserWorkItem(FireExecutedPlan, e);
            }
            else
            {
                if (LoopTasks.Contains(planName))
                {
                    try
                    {
                        LoopTasks.Remove(planName);
                    }
                    catch { }
                }
            }
        }

        /// <summary>
        /// 执行系统内置计划任务
        /// </summary>
        /// <param name="e"></param>
        private void ExecuteSystemPlan(ExecutePlanEventArgs e)
        {
            switch (e.PlanName)
            {
                case "%sys%ClearMemory":
                    #region ClearMemory

                    try
                    {
                        GC.Collect();
                        SetStatusInfo("执行GC垃圾回收。");
                    }
                    catch (Exception exception)
                    {
                        Log.CustomWrite(exception.ToString(), "TaskPlatform--PlatformFormException");
                    }

                    #endregion
                    break;
                case "%sys%CollectThreadsCount":
                    #region CollectThreadsCount

                    try
                    {
                        lbWorkThreadsCount.BeginInvoke(new MethodInvoker(delegate()
                        {
                            int workerThreads = 0, compl = 0;
                            ThreadPool.GetAvailableThreads(out workerThreads, out  compl);
                            lbWorkThreadsCount.Text = workerThreads.ToString();
                            lbCompletionThreadsCount.Text = compl.ToString();
                        }));
                    }
                    catch (Exception exception)
                    {
                        Log.CustomWrite(exception.ToString(), "TaskPlatform--PlatformFormException");
                    }

                    #endregion
                    break;
                case "%sys%DeepClearMemory":
                    #region DeepClearMemory

                    try
                    {
                        Process process = Process.GetCurrentProcess();
                        long currentPrivateMemory = process.PrivateMemorySize64;
                        long currentWorkingSet = process.WorkingSet64;
                        Win32Helper.EmptyWorkingSet(process.Handle);
                        SetStatusInfo(string.Format("执行内存深度整理，整理前：专用内存{0}物理内存{1}", Commom.Commom.ConvertFileSize(currentPrivateMemory), Commom.Commom.ConvertFileSize(currentWorkingSet)));
                    }
                    catch (Exception exception)
                    {
                        Log.CustomWrite(exception.ToString(), "TaskPlatform--PlatformFormException");
                    }

                    #endregion
                    break;
                case "%sys%ShrinkLog":
                    #region ShrinkLog

                    try
                    {
                        Log.ShrinkLog();
                        SetStatusInfo("自动收缩日志。");
                    }
                    catch (Exception exception)
                    {
                        Log.CustomWrite(exception.ToString(), "TaskPlatform--PlatformFormException");
                    }

                    #endregion
                    break;
                case "%sys%CheckRunningTasks":
                    #region CheckRunningTasks

                    try
                    {
                        StringBuilder sbRunInfo = new StringBuilder();
                        StringBuilder sbEmail = new StringBuilder();
                        StringBuilder sbSMS = new StringBuilder();

                        bool needSendSMS = false;

                        #region 查找需报警信息

                        var waitCheckList = from f in
                                                (from trs in TaskRunStatistics
                                                 where trs.Value != null && trs.Value.Count > 0
                                                 select new { Info = trs, aobj = AlarmObjects.Find(item => { return item.PlanName == trs.Key; }) })
                                            where f.aobj != null && f.aobj.AlarmType != 4 && f.aobj.RunCount <= f.Info.Value.Count && f.aobj.PlanName != "%sys%-All"
                                            select f;
                        var cancelList = (from f in waitCheckList
                                          where f.aobj.AlarmType == 3 && f.Info.Value.Select(item => { return item.RunResult == -1; }).Count() >= f.aobj.AlarmWhenCancelCount
                                          select f).ToList();
                        var failedList = (from f in waitCheckList
                                          where f.aobj.AlarmType == 2 && f.Info.Value.Select(item => { return item.RunResult == 0; }).Count() >= f.aobj.AlarmWhenFailCount
                                          select f).ToList();
                        var allList = (from f in waitCheckList
                                       where f.aobj.AlarmType == 1 && ((from fail in f.Info.Value where fail.RunResult == 0 select fail).ToList().Count >= f.aobj.AlarmWhenFailCount || (from cancel in f.Info.Value where cancel.RunResult == -1 select cancel).ToList().Count >= f.aobj.AlarmWhenCancelCount)
                                       select f).ToList();

                        List<string> mustRunTaskList = (from f in AlarmObjects
                                                        where f.AlarmType != 4 && f.PlanName != "%sys%-All"
                                                        select f.PlanName).Distinct().ToList();

                        int rowNumber = 1;
                        bool hasError = false;
                        mustRunTaskList.ForEach(item =>
                        {
                            #region 检查
                            DomainLoader loader = GetDomainLoader(item);
                            if (loader == null)
                            {
                                hasError = true;
                                sbRunInfo.AppendFormat("{0}、任务【{1}】{2}；<br/>", rowNumber++, Commom.Commom.GetBlueFont(item), Commom.Commom.GetRedFont("未找到"));
                            }
                            else
                            {
                                try
                                {
                                    if (loader == null)
                                    {
                                        hasError = true;
                                        sbRunInfo.AppendFormat("{0}、任务【{1}-{2}】{3}；<br/>", rowNumber++, Commom.Commom.GetBlueFont(loader.TaskOperator.TaskName()), Commom.Commom.GetBlueFont(item), Commom.Commom.GetRedFont("执行域错误"));
                                    }
                                    else if (loader.Plan == null)
                                    {
                                        hasError = true;
                                        sbRunInfo.AppendFormat("{0}、任务【{1}-{2}】{3}；<br/>", rowNumber++, Commom.Commom.GetBlueFont(loader.TaskOperator.TaskName()), Commom.Commom.GetBlueFont(item), Commom.Commom.GetRedFont("尚未配置执行计划"));
                                    }
                                    else if (!loader.Plan.Enable)
                                    {
                                        hasError = true;
                                        sbRunInfo.AppendFormat("{0}、任务【{1}-{2}】{3}；<br/>", rowNumber++, Commom.Commom.GetBlueFont(loader.TaskOperator.TaskName()), Commom.Commom.GetBlueFont(item), Commom.Commom.GetRedFont("未运行"));
                                    }
                                }
                                catch (Exception exception)
                                {
                                    hasError = true;
                                    sbRunInfo.AppendFormat("{0}、检查任务【{1}】时{2}。错误信息<br/>{3}；<br/>", rowNumber++, Commom.Commom.GetBlueFont(item), Commom.Commom.GetRedFont("出错"), exception.ToString());
                                }
                            }
                            #endregion
                        });
                        if (hasError && SMSAlertPriority > 0)
                        {
                            needSendSMS = true;
                            sbSMS.Append(string.Format("未运行{0}", rowNumber - 1));
                        }

                        #endregion

                        int errorCountRun = cancelList.Count + failedList.Count + allList.Count;
                        if (errorCountRun > 0)
                        {
                            #region 生成报警内容

                            if (failedList.Count > 0 && SMSAlertPriority > 2)
                            {
                                needSendSMS = true;
                                sbSMS.Append(string.Format("失败{0}", failedList.Count));
                            }
                            failedList.ForEach(item =>
                            {
                                try
                                {
                                    sbRunInfo.AppendFormat("{0}、任务【{1}】配置为运行【{2}】次后失败【{3}】次报警，但任务在运行了【{4}】次后失败了【{5}】；<br/>", rowNumber++, Commom.Commom.GetBlueFont(item.aobj.TaskName), Commom.Commom.GetGreenFont(item.aobj.RunCount), Commom.Commom.GetRedFont(item.aobj.AlarmWhenFailCount), Commom.Commom.GetGreenFont(item.Info.Value.Count), Commom.Commom.GetRedFont(item.Info.Value.Select(v => { return v.RunResult == 0; }).Count()));
                                    if (!string.IsNullOrWhiteSpace(item.aobj.AlarmEmailAddress))
                                    {
                                        sbEmail.Append(item.aobj.AlarmEmailAddress).Append(";");
                                    }
                                    lock (_taskRunStatisticsLock)
                                    {
                                        _taskRunStatistics[item.aobj.PlanName].Clear();
                                    }
                                }
                                catch { }
                            });
                            if (allList.Count > 0 && SMSAlertPriority > 1)
                            {
                                needSendSMS = true;
                                sbSMS.Append(string.Format("复合{0}", allList.Count));
                            }
                            allList.ForEach(item =>
                            {
                                try
                                {
                                    int fc = (from f in item.Info.Value
                                              where f.RunResult == 0
                                              select f).ToList().Count;
                                    int cc = (from f in item.Info.Value
                                              where f.RunResult == -1
                                              select f).ToList().Count;
                                    sbRunInfo.AppendFormat("{0}、任务【{1}】配置为运行【{2}】次后失败或取消执行【{3}/{4}】次报警，但任务在运行了【{5}】次后失败或取消执行了【{6}/{7}】；<br/>", rowNumber++, Commom.Commom.GetBlueFont(item.aobj.TaskName), Commom.Commom.GetGreenFont(item.aobj.RunCount), Commom.Commom.GetRedFont(item.aobj.AlarmWhenFailCount), Commom.Commom.GetRedFont(item.aobj.AlarmWhenCancelCount), Commom.Commom.GetGreenFont(item.Info.Value.Count), Commom.Commom.GetRedFont(fc), Commom.Commom.GetRedFont(cc));
                                    if (!string.IsNullOrWhiteSpace(item.aobj.AlarmEmailAddress))
                                    {
                                        sbEmail.Append(item.aobj.AlarmEmailAddress).Append(";");
                                    }
                                    lock (_taskRunStatisticsLock)
                                    {
                                        _taskRunStatistics[item.aobj.PlanName].Clear();
                                    }
                                }
                                catch { }
                            });
                            if (cancelList.Count > 0 && SMSAlertPriority > 3)
                            {
                                needSendSMS = true;
                                sbSMS.Append(string.Format("取消{0}", cancelList.Count));
                            }
                            cancelList.ForEach(item =>
                            {
                                try
                                {
                                    sbRunInfo.AppendFormat("{0}、任务【{1}】配置为运行【{2}】次后失败【{3}】次报警，但任务在运行了【{4}】次后失败了【{5}】；<br/>", rowNumber++, Commom.Commom.GetBlueFont(item.aobj.TaskName), Commom.Commom.GetGreenFont(item.aobj.RunCount), Commom.Commom.GetRedFont(item.aobj.AlarmWhenCancelCount), Commom.Commom.GetGreenFont(item.Info.Value.Count), Commom.Commom.GetRedFont(item.Info.Value.Select(v => { return v.RunResult == -1; }).Count()));
                                    if (!string.IsNullOrWhiteSpace(item.aobj.AlarmEmailAddress))
                                    {
                                        sbEmail.Append(item.aobj.AlarmEmailAddress).Append(";");
                                    }
                                    lock (_taskRunStatisticsLock)
                                    {
                                        _taskRunStatistics[item.aobj.PlanName].Clear();
                                    }
                                }
                                catch { }
                            });

                            #endregion
                        }

                        if (needSendSMS)
                        {
                            try
                            {
                                if (ReceiveMobiles.Count < 1)
                                {
                                    sbRunInfo.AppendFormat(Commom.Commom.GetRedFont("<br/><br/>短信警报发送失败：<br/>要求发送短信警报但未设置接收手机号。"));
                                }
                                else
                                {
                                    if (crmSMSService == null)
                                    {
                                        crmSMSService = new CRMSMSService.SmsSenderImplService();
                                    }
                                    CRMSMSService.smsRequest request = new CRMSMSService.smsRequest();
                                    request.content = string.Format("计划任务平台警报：[{0}][{1}]{2}{3}", Environment.MachineName, PlatformPortName, sbSMS.ToString(), SMSSignature);
                                    request.extendId = "21000";
                                    request.guid = string.Format("[{0}][{1}][{2}]", Environment.MachineName, PlatformPortName, Guid.NewGuid().ToString("D"));
                                    request.mobiles = ReceiveMobiles.ToArray();
                                    request.skey = SMSKey;
                                    CRMSMSService.smsResponse response = crmSMSService.send(request);
                                    if (response == null)
                                    {
                                        sbRunInfo.AppendFormat(Commom.Commom.GetRedFont("<br/><br/>短信警报发送失败：<br/>接口返回值为null"));
                                    }
                                    else if (response.code == 0)
                                    {
                                        sbRunInfo.AppendFormat(Commom.Commom.GetGreenFont("<br/><br/>短信警报发送成功。"));
                                    }
                                    else
                                    {
                                        sbRunInfo.AppendFormat(Commom.Commom.GetBlueFont("<br/><br/>短信警报发送失败：[{0}]{1}。"), response.code, response.message);
                                    }
                                }
                            }
                            catch (Exception exception2)
                            {
                                sbSMS.Clear();
                                sbRunInfo.AppendFormat(Commom.Commom.GetRedFont("<br/><br/>短信警报发送失败：<br/>{0}"), exception2.ToString());
                                Log.CustomWrite(exception2.ToString(), "TaskPlatform--PlatformFormException");
                            }
                        }
                        else
                        {
                            sbRunInfo.AppendFormat(Commom.Commom.GetGreenFont("<br/><br/>短信警报未发送：未开启短信报警或警报级别太低。"));
                        }
                        if (needSendSMS || errorCountRun > 0)
                        {
                            try
                            {
                                string text = string.Empty;
                                this.Invoke(new Action(() => { text = this.Text; }));
                                string emailBody = string.Format("<div style=\"font-size: 10pt;\">报警系统似乎检测到了一些问题，共【{0}】条，请查看并及时处理：<br/><br/>{1}</div>{2}", Commom.Commom.GetRedFont(rowNumber - 1), sbRunInfo.ToString(), "<br/><div style=\"font-size: 10pt;color: Red;\">本邮件为正式警报信息，请引起高度关注，并及时处理警报中所标示的问题。<br/>From：王旭丹<br/><br/>附注：" + text + "</div>");
                                sbEmail.Append(appSettings["DefaultEmailAddressForAlertSystem"]);
                                Commom.Commom.SendEmail(emailBody, string.Join(";", sbEmail.ToString().Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToList()));
                                SetStatusInfo("检测到运行时计划任务异常，已发送报警邮件。");
                            }
                            catch (Exception exception1)
                            {
                                Log.CustomWrite(exception1.ToString(), "TaskPlatform--PlatformFormException");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.CustomWrite(ex.ToString(), "TaskPlatform--PlatformFormException");
                    }
                    #endregion
                    break;
            }
            ExecutingTaskCountInfo.Instance.UnRegisterExecutingTask(e.PlanName);
        }

        /// <summary>
        /// 更新图标
        /// </summary>
        /// <param name="planName"></param>
        /// <param name="isOK"></param>
        public void UpdateIcon(string planName, bool isOK = true)
        {
            ResetTaskList();
            NavBarItem item = GetNavBarItem(planName);
            item.Name = planName;
            if (isOK)
            {
                item.SmallImage = GetImage("Right");
                item.Hint = string.Format("更新计划成功，已启用计划。任务正在执行。");
            }
            else
            {
                item.SmallImage = GetImage("Wait");
                item.Hint = string.Format("更新计划成功，已停用计划。任务已停止执行。");
            }
        }

        /// <summary>
        /// 获取资源中的图片对象
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private Image GetImage(string name = "Annotate_Default.ico")
        {
            if (imgs.ContainsKey(name))
                return imgs[name];
            else
            {
                Image img = images.Images[name];
                imgs.Add(name, img);
                return img;
            }
        }

        /// <summary>
        /// 按名称查找特定项
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public NavBarItem GetNavBarItem(string name)
        {
            foreach (TaskGroup group in TaskGroupList)
            {
                if (group.GroupControl == null)
                {
                    break;
                }
                foreach (NavBarItemLink item in group.GroupControl.ItemLinks)
                {
                    if (item.Item.Name.Replace("%NC%", "").Replace("%NP%", "") == name)
                    {
                        return item.Item;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 检查特定DomainLoader
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private DomainLoader GetDomainLoader(string name, bool onlyFromList = false)
        {
            DomainLoader loader = (from dl in DomainLoaderList
                                   where dl.DomainName == name
                                   select dl).FirstOrDefault();
            if (!onlyFromList)
            {
                if (loader == null || loader.TaskOperator == null)
                {
                    NavBarItem nbiTask = GetNavBarItem(name);
                    if (nbiTask != null)
                    {
                        loader = nbiTask.Tag as DomainLoader;
                    }
                }
            }
            return loader;
        }

        /// <summary>
        /// 卸载指定的计划任务
        /// </summary>
        /// <param name="taskName"></param>
        public void UnLoadTaskFull(string taskName, bool showResult = true, bool reload = false)
        {
            bool comf = true;
            if (showResult)
            {
                this.Invoke(new Action(delegate()
                {
                    string tipMessage = reload ? "确定要重新加载该计划任务吗？" : "确定要卸载该计划任务吗？";
                    if (XtraMessageBox.Show(this, tipMessage, "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.No)
                    {
                        comf = false;
                    }
                }));
            }
            if (!comf)
            {
                return;
            }
            this.BeginInvoke(new Action(delegate()
            {
                ResetTaskList();
                NavBarItem nbiTask = GetNavBarItem(taskName);
                DomainLoader loader = GetDomainLoader(taskName);
                if (loader == null)
                {
                    if (showResult)
                    {
                        XtraMessageBox.Show(this, "获取计划任务运行时域失败，请确保该计划任务已正确加载。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    return;
                }
                string unLoadInfo = string.Format("计划任务 {0}({1}) 卸载成功。", loader.TaskOperator.TaskName(), loader.DomainName);
                string fileName = loader.FileName;
                // 如果计划任务已经配置有执行计划，则停用计划
                if (loader.Plan != null)
                {
                    loader.Plan.Enable = false;
                    LoaclDataCacheObject item = new LoaclDataCacheObject();
                    item.Key = "Plan:" + loader.Plan.PlanName;
                    item.KeyForUpdate = item.Key;
                    item.Value = loader.Plan;
                    PlanPlatformCache.Update(item);
                }
                TaskGroup group = GetTaskGroup(loader.DomainName);
                // 卸载计划任务运行时域
                loader.UnLoadTaskDomian();
                DomainLoaderList.Remove(loader);
                if (group == null)
                {
                    group = GetDefalutTaskGroup();
                }
                // 移除菜单
                group.GroupControl.ItemLinks.Remove(nbiTask);
                // 关闭选项卡
                bool isOpened = CloseTabPage(null, null, taskName);
                SetStatusInfo(unLoadInfo);
                if (showResult && !reload)
                {
                    XtraMessageBox.Show(this, "卸载成功。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (reload)
                {
                    // 加载计划任务
                    if (LoadTask(fileName) > 3)
                    {
                        if (isOpened)
                        {
                            NavBarItem nbiTaskNew = GetNavBarItem(taskName);
                            DomainLoader newLoader = nbiTaskNew.Tag as DomainLoader;
                            if (newLoader == null)
                            {
                                if (showResult)
                                {
                                    XtraMessageBox.Show(this, "加载器指示计划任务已经重新加载成功，但是尝试打开计划任务详情时获取运行时域失败。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                return;
                            }
                            else
                            {
                                OpenTaskStage(newLoader);
                                if (showResult)
                                {
                                    ShowInfo("重新加载成功完成。");
                                }
                            }
                        }
                        else
                        {
                            if (showResult)
                            {
                                ShowInfo("重新加载成功完成。");
                            }
                        }
                    }
                    else
                    {
                        if (showResult)
                        {
                            ShowError("重新载入过程出错。请参考：\n\n1、卸载成功；\n2、加载失败。\n\n请尝试平台信息选项卡中的刷新功能或者重新启动平台。");
                        }
                    }
                }
            }));
        }

        /// <summary>
        /// 重新加载计划任务
        /// </summary>
        internal void ReloadTask(string taskName, bool showMessage = false)
        {
            if (taskName.ToLower().Contains("noshadowcopy"))
            {
                ShowWarning("该计划任务强制关闭了卷影复制功能，不支持动态重新加载。\n\n若要重新加载计划任务，请重新启动计划任务平台。");
            }
            else
            {
                UnLoadTaskFull(taskName, showMessage, true);
            }
        }

        #endregion

        #region  平台执行计划任务后通知各选项卡更新信息的事件

        /// <summary>
        /// 通知事件所需的委托。
        /// </summary>
        /// <param name="e"></param>
        public delegate void ExecutedPlanHandler(object sender, ExecutedPlanEventArgs e);
        /// <summary>
        /// 计划任务执行完成通知。平台执行完成一个计划任务后触发。
        /// </summary>
        public event ExecutedPlanHandler ExecutedPlan;
        /// <summary>
        /// 触发通知事件
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnExecutedPlan(ExecutedPlanEventArgs e)
        {
            if (ExecutedPlan != null)
                ExecutedPlan(this, e);
        }

        /// <summary>
        /// 通知事件所需的委托。
        /// </summary>
        /// <param name="e"></param>
        public delegate void ExecutePlanHandler(object sender, ExecutingPlanEventArgs e);
        /// <summary>
        /// 计划任务执行前通知。平台执行一个计划任务前触发。
        /// </summary>
        public event ExecutePlanHandler ExecutePlan;
        /// <summary>
        /// 触发通知事件
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnExecutePlan(ExecutingPlanEventArgs e)
        {
            if (ExecutePlan != null)
                ExecutePlan(this, e);
        }

        /// <summary>
        /// 通知计划任务更新所需的委托
        /// </summary>
        /// <param name="taskName"></param>
        public delegate void TaskUpdateHandler(string taskName);

        /// <summary>
        /// 计划任务更新事件
        /// </summary>
        public event TaskUpdateHandler TaskUpdate;

        /// <summary>
        /// 触发计划任务更新事件
        /// </summary>
        /// <param name="taskName"></param>
        protected virtual void OnTaskUpdate(string taskName)
        {
            if (TaskUpdate != null)
                TaskUpdate(taskName);
        }

        #endregion

        #region 管理计划任务、平台信息、数据库设置、关于、关闭按钮的处理、刷新计划任务

        private void nbiManageTask_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            // 判断是否已经打开，如果已经打开，就选中。
            // 如果找不到，就创建选项卡，并选中。
            string name = "%sys%ManageTasks";
            foreach (XtraTabPage tab in tabContainer.TabPages)
            {
                if (tab.Name == name)
                {
                    tabContainer.SelectedTabPage = tab;
                    return;
                }
            }
            XtraTabPage tabManageTask = new XtraTabPage();
            tabManageTask.Name = name;
            tabManageTask.Text = e.Link.Item.Caption;
            tabManageTask.ShowCloseButton = DevExpress.Utils.DefaultBoolean.True;
            TasksManager tasksManager = new TasksManager();
            tasksManager.Name = tabManageTask.Name;
            tasksManager.TaskItems = nbgTasks.ItemLinks;
            tabManageTask.Controls.Add(tasksManager);
            tabContainer.TabPages.Add(tabManageTask);
            tabContainer.SelectedTabPage = tabManageTask;
        }

        private void deadAlarmManager_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            // 判断是否已经打开，如果已经打开，就选中。
            // 如果找不到，就创建选项卡，并选中。
            string name = "%sys%DeadAlarmManager";
            foreach (XtraTabPage tab in tabContainer.TabPages)
            {
                if (tab.Name == name)
                {
                    tabContainer.SelectedTabPage = tab;
                    return;
                }
            }
            XtraTabPage tabManageDeadAlarm = new XtraTabPage();
            tabManageDeadAlarm.Name = name;
            tabManageDeadAlarm.Text = e.Link.Item.Caption;
            tabManageDeadAlarm.ShowCloseButton = DevExpress.Utils.DefaultBoolean.True;
            DeadAlarmManager deadalarmManager = new DeadAlarmManager();
            deadalarmManager.Name = tabManageDeadAlarm.Name;
            tabManageDeadAlarm.Controls.Add(deadalarmManager);
            tabContainer.TabPages.Add(tabManageDeadAlarm);
            tabContainer.SelectedTabPage = tabManageDeadAlarm;
        }

        private void nbiCacheManager_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            if (!_safeMode)
            {
                XtraMessageBox.Show(this, "只有在安全模式下才能使用该功能。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 判断是否已经打开，如果已经打开，就选中。
            // 如果找不到，就创建选项卡，并选中。
            string name = "%sys%CacheManager";
            foreach (XtraTabPage tab in tabContainer.TabPages)
            {
                if (tab.Name == name)
                {
                    tabContainer.SelectedTabPage = tab;
                    return;
                }
            }
            XtraTabPage tabCacheManager = new XtraTabPage();
            tabCacheManager.Name = name;
            tabCacheManager.Text = e.Link.Item.Caption;
            tabCacheManager.ShowCloseButton = DevExpress.Utils.DefaultBoolean.True;
            CacheManager cacheManager = new CacheManager();
            cacheManager.Name = tabCacheManager.Name;
            tabCacheManager.Controls.Add(cacheManager);
            tabContainer.TabPages.Add(tabCacheManager);
            tabContainer.SelectedTabPage = tabCacheManager;
        }

        private void nbiPlatformInfo_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            tabContainer.SelectedTabPage = tabPageHome;
        }

        private void nbiAbout_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            using (AboutBox af = new AboutBox())
            {
                af.ShowDialog();
            }
        }

        private void nbiDBConfig_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            // 判断是否已经打开，如果已经打开，就选中。
            // 如果找不到，就创建选项卡，并选中。
            string name = "%sys%DBConfig";
            foreach (XtraTabPage tab in tabContainer.TabPages)
            {
                if (tab.Name == name)
                {
                    tabContainer.SelectedTabPage = tab;
                    return;
                }
            }
            XtraTabPage tabDBConfig = new XtraTabPage();
            tabDBConfig.Name = name;
            tabDBConfig.Text = e.Link.Item.Caption;
            tabDBConfig.ShowCloseButton = DevExpress.Utils.DefaultBoolean.True;
            ConfigsEditor dbConfig = new ConfigsEditor();
            tabDBConfig.Controls.Add(dbConfig);
            tabContainer.TabPages.Add(tabDBConfig);
            tabContainer.SelectedTabPage = tabDBConfig;
        }

        private void nbiDomainLoaderPerformance_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            // 判断是否已经打开，如果已经打开，就选中。
            // 如果找不到，就创建选项卡，并选中。
            string name = "%sys%DomainLoaderPerformance";
            foreach (XtraTabPage tab in tabContainer.TabPages)
            {
                if (tab.Name == name)
                {
                    tabContainer.SelectedTabPage = tab;
                    return;
                }
            }
            XtraTabPage tabDBConfig = new XtraTabPage();
            tabDBConfig.Name = name;
            tabDBConfig.Text = e.Link.Item.Caption;
            tabDBConfig.ShowCloseButton = DevExpress.Utils.DefaultBoolean.True;
            DomainLoaderPerformance domainLoaderPerformance = new DomainLoaderPerformance();
            tabDBConfig.Controls.Add(domainLoaderPerformance);
            tabContainer.TabPages.Add(tabDBConfig);
            tabContainer.SelectedTabPage = tabDBConfig;
        }

        private void tabContainer_CloseButtonClick(object sender, EventArgs e)
        {
            CloseTabPage(sender, e);
        }

        /// <summary>
        /// 关闭选项卡
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="domainName"></param>
        private bool CloseTabPage(object sender, EventArgs e, string domainName = "")
        {
            XtraTabPage tabPage = tabContainer.SelectedTabPage;
            if (!string.IsNullOrWhiteSpace(domainName))
            {
                foreach (XtraTabPage page in tabContainer.TabPages)
                {
                    if (page.Name == domainName)
                    {
                        tabPage = page;
                        break;
                    }
                }
            }
            // 如果是平台不允许被关闭的
            if (tabPage == null || tabPage.Name.StartsWith("%sysNoClose%") || (!string.IsNullOrWhiteSpace(domainName) && tabPage.Name != domainName))
                return false;
            // 卸载计划任务时，选项卡必须是和计划任务对应
            bool donotRemove = true;
            if (!string.IsNullOrWhiteSpace(domainName))
            {
                if (!tabPage.Name.Contains(domainName))
                {
                    donotRemove = false;
                }
            }
            // 如果不是平台内置特定功能的选项卡
            if (!tabPage.Name.Contains("%sys%"))
            {
                TaskStage taskStage = tabPage.Controls["taskStage" + tabPage.Name] as TaskStage;
                this.ExecutedPlan -= taskStage.ExecutedPlan;
                this.ExecutePlan -= taskStage.ExecutePlan;
                this.TaskUpdate -= taskStage.TaskUpdate;
            }
            if (donotRemove)
            {
                tabContainer.TabPages.Remove(tabPage);
                tabPage.Dispose();
                tabContainer.SelectedTabPageIndex = tabContainer.TabPages.Count - 1;
            }
            return true;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            // 启动增量计划任务扫描器
            ThreadPool.QueueUserWorkItem(TasksScanner);
        }

        private void PlatformForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_canClose)
            {
                DialogResult result = XtraMessageBox.Show(this, "退出计划任务执行平台，将导致该平台下所有计划任务无法执行，\n并由此导致部分业务中断。\n\n您是否要退出平台？\n请谨慎选择您要执行的操作：\n\n点击“是”：平台将终止运行；\n点击“否”：平台将退出并重新启动；\n点击“取消”：平台将保持运行。\n", "警告", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
                else if (result == DialogResult.Yes)
                {
                    ntfPlatformNotify.Visible = false;
                    Application.ExitThread();
                }
                else
                {
                    ntfPlatformNotify.Visible = false;
                    _canClose = true;
                    Application.Restart();
                }
            }
        }

        /// <summary>
        /// 强制保存报警系统缓存
        /// </summary>
        public static void SaveAlterSystemCache()
        {
            try
            {
                mustRunTasks = (from f in mustRunTasks
                                select f).Distinct().ToList();
            }
            catch { }
            AlertSystemCache.Update(new LoaclDataCacheObject() { Key = "MustRunTasks", KeyForUpdate = "MustRunTasks", Value = mustRunTasks });
            AlertSystemCache.Update(new LoaclDataCacheObject() { Key = "AlarmObjects", KeyForUpdate = "AlarmObjects", Value = AlarmObjects });
        }

        /// <summary>
        /// 强制保存任务分组缓存
        /// </summary>
        public void SaveTaskGroupCache()
        {
            TaskGroupList.ForEach(group => { group.TaskList = group.TaskList.Distinct().ToList(); });
            TaskGroupCache.Update(new LoaclDataCacheObject() { Key = "TaskGroupList", KeyForUpdate = "TaskGroupList", Value = TaskGroupList });
        }

        private void btnCreateTask_Click(object sender, EventArgs e)
        {
            TaskCreateForm taskCreateForm = new TaskCreateForm();
            taskCreateForm.ShowDialog();
        }

        PlanV2 planV2 = null;

        private void btnCreatePlanV2_Click(object sender, EventArgs e)
        {
            //EditTaskParameter t = new EditTaskParameter();
            //t.ShowDialog();
            //try
            //{
            //    Environment.FailFast("nimei");
            PlanV2ConfigForm configForm = new PlanV2ConfigForm(planV2);
            configForm.ShowDialog();
            if (configForm.IsOK)
            {
                planV2 = configForm.Plan;
            }
            //}
            //catch (Exception x)
            //{
            //    MessageBox.Show(x.ToString());
            //}
            //finally
            //{
            //    Application.Restart();
            //}
        }

        private void btnOpenTaskStage_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (_currentClickItem != null)
            {
                DomainLoader loader = _currentClickItem.Tag as DomainLoader;
                OpenTaskStage(loader);
            }
        }

        private void btnUnloadTask_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (_currentClickItem != null)
            {
                DomainLoader loader = _currentClickItem.Tag as DomainLoader;
                UnLoadTaskFull(loader.DomainName);
            }
        }

        private void barbtnOpenDirectory_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (_currentClickItem != null)
            {
                DomainLoader loader = _currentClickItem.Tag as DomainLoader;
                if (loader != null)
                {
                    try
                    {
                        Process.Start("explorer", Path.GetDirectoryName(loader.FileName));
                    }
                    catch (Exception ex)
                    {
                        PlatformForm.Form.ShowError(ex.ToString());
                    }
                }
            }
        }

        private void barbtnReload_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (_currentClickItem != null)
            {
                DomainLoader loader = _currentClickItem.Tag as DomainLoader;
                ReloadTask(loader.DomainName, true);
            }
        }

        public bool TaskDirectoryExists(string taskName)
        {
            string[] tasksRootPath = { TaskFilesPath, SystemTaskFilesPath };
            foreach (string path in tasksRootPath)
            {

            }
            return true;
        }

        #endregion

        #region 更新进度和状态信息
        /// <summary>
        /// 更新进度条进度
        /// </summary>
        /// <param name="value"></param>
        private void ProcessBar(int value)
        {
            try
            {
                this.BeginInvoke(new MethodInvoker(delegate
                {
                    if (value < 0)
                    {
                        value = 0;
                    }
                    if (pgbProcess.Value + value > pgbProcess.Maximum)
                    {
                        pgbProcess.Maximum += value;
                    }
                    pgbProcess.Value += value;
                }));
            }
            catch (Exception exception)
            {
                Log.CustomWrite(exception.ToString(), "TaskPlatform--PlatformFormException");
            }
        }

        /// <summary>
        /// 重置进度条。
        /// </summary>
        /// <param name="maxValue">最大值</param>
        private void ResetProcessBar(int maxValue)
        {
            try
            {
                this.BeginInvoke(new MethodInvoker(delegate
                {
                    pgbProcess.Visible = maxValue > 0;
                    pgbProcess.Maximum = Math.Abs(maxValue);
                    pgbProcess.Value = 0;
                }));
            }
            catch (Exception exception)
            {
                Log.CustomWrite(exception.ToString(), "TaskPlatform--PlatformFormException");
            }
        }

        /// <summary>
        /// 更新状态信息
        /// </summary>
        /// <param name="text"></param>
        public string SetStatusInfo(string text)
        {
            try
            {
                this.BeginInvoke(new MethodInvoker(delegate
                {
                    txtInfo.Text = text;
                    Log.Write(text);
                }));
            }
            catch (Exception exception)
            {
                Log.CustomWrite(exception.ToString(), "TaskPlatform--PlatformFormException");
            }
            return text;
        }
        #endregion

        #region 资源处理等

        /// <summary>
        /// 从资源库获取字符串模版
        /// </summary>
        /// <param name="templatePath">模版路径，如CreateTaskTemplate</param>
        /// <returns></returns>
        public static string GetStringTemplate(string templatePath)
        {
            if (string.IsNullOrWhiteSpace(templatePath))
            {
                templatePath = "CreateTaskTemplate";
            }
            return File.ReadAllText(ResoucesPath + "\\Templates\\" + templatePath + ".t");
        }

        #endregion

        #region  Web控制台接口

        /// <summary>
        /// 立刻执行一次
        /// </summary>
        /// <param name="taskName">计划任务名称</param>
        /// <returns>
        /// 执行结果
        /// </returns>
        public RunTaskResult RunOnce(string taskName)
        {
            return PrivateRunOnce(taskName, false);
        }

        private RunTaskResult PrivateRunOnce(string taskName, bool fromWeb, string runID = "")
        {
            RunTaskResult result = new RunTaskResult();
            result.Success = false;
            if (string.IsNullOrWhiteSpace(runID))
            {
                fromWeb = false;
            }
            try
            {
                DomainLoader loader = GetDomainLoader(taskName);
                AlarmObject alarmObject = AlarmObjects.Find(item => { return item.PlanName == loader.DomainName; });
                int timeOut = (int)((loader.Plan == null ? 5 : loader.Plan.TotalSeconds) * (alarmObject == null ? 1 : alarmObject.TimeOutRatio));
                bool register = ExecutingTaskCountInfo.Instance.RegisterExecutingTask(taskName);
                if (!register)
                {
                    result.Result = "该任务执行队列已满，将不排入执行。";
                    return result;
                }

                try
                {
                    ExecutingPlanEventArgs earg = new ExecutingPlanEventArgs();
                    earg.ExecuteType = ExecuteType.Before;
                    earg.ExecuteTime = DateTime.Now;
                    earg.PlanName = taskName;
                    OnExecutePlan(earg);
                }
                catch (Exception exceptionArg)
                {
                    Log.CustomWrite(exceptionArg.ToString(), "TaskPlatform--PlatformFormException");
                }

                if (fromWeb)
                {
                    if (!RunningCache.ContainsKey(runID))
                    {
                        RunningCache.Add(runID, new Queue<string>());
                    }
                    RunningCache[runID].Enqueue("[sys:RunOnceStart]");
                    result = loader.TaskOperator.RunTaskFromWeb(timeOut, runID);
                    RunningCache[runID].Enqueue("[sys:RunOnceEnd]");
                }
                else
                {
                    result = loader.TaskOperator.RunTaskWithTime(timeOut);
                }

                try
                {
                    ExecutingPlanEventArgs earg = new ExecutingPlanEventArgs();
                    earg.ExecuteType = ExecuteType.After;
                    earg.ExecuteTime = DateTime.Now;
                    earg.PlanName = taskName;
                    OnExecutePlan(earg);
                }
                catch (Exception exceptionArg)
                {
                    Log.CustomWrite(exceptionArg.ToString(), "TaskPlatform--PlatformFormException");
                }

                ExecutingTaskCountInfo.Instance.UnRegisterExecutingTask(taskName);
                string waitTimeOutKey = "%sys%WaitTimeOut";
                if (result.Result == waitTimeOutKey)
                {
                    result.Result = "<线程等待超时，已自动取消执行。>";
                }
            }
            catch (Exception ex)
            {
                ExecutingTaskCountInfo.Instance.UnRegisterExecutingTask(taskName);
                result.Result = ex.ToString();
            }
            return result;
        }

        /// <summary>
        /// 立刻执行一次
        /// </summary>
        /// <param name="taskName">计划任务名称</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>
        /// 执行结果
        /// </returns>
        public RunTaskResult RunOnceWithParameters(string taskName, Dictionary<string, string> parameters)
        {
            RunTaskResult result = new RunTaskResult();
            result.Success = false;
            try
            {
                DomainLoader loader = GetDomainLoader(taskName);
                AlarmObject alarmObject = AlarmObjects.Find(item => { return item.PlanName == loader.DomainName; });
                int timeOut = (int)((loader.Plan == null ? 5 : loader.Plan.TotalSeconds) * (alarmObject == null ? 1 : alarmObject.TimeOutRatio));
                if (parameters != null && parameters.Count > 0)
                {
                    if (!loader.TaskOperator.SupportAdvancedFeatures)
                    {
                        throw new NotSupportedException(string.Format("任务[{0}]不支持传入参数的高级功能，请保证该任务从AbstractTask继承实现。", taskName));
                    }
                    else
                    {
                        loader.TaskOperator.AdvancedTask.InvokeParameters = parameters;
                    }
                }
                bool register = ExecutingTaskCountInfo.Instance.RegisterExecutingTask(taskName);
                if (!register)
                {
                    result.Result = "该任务执行队列已满，将不排入执行。";
                    return result;
                }

                try
                {
                    ExecutingPlanEventArgs earg = new ExecutingPlanEventArgs();
                    earg.ExecuteType = ExecuteType.Before;
                    earg.ExecuteTime = DateTime.Now;
                    earg.PlanName = taskName;
                    OnExecutePlan(earg);
                }
                catch (Exception exceptionArg)
                {
                    Log.CustomWrite(exceptionArg.ToString(), "TaskPlatform--PlatformFormException");
                }

                result = loader.TaskOperator.RunTaskWithTime(timeOut);

                try
                {
                    ExecutingPlanEventArgs earg = new ExecutingPlanEventArgs();
                    earg.ExecuteType = ExecuteType.After;
                    earg.ExecuteTime = DateTime.Now;
                    earg.PlanName = taskName;
                    OnExecutePlan(earg);
                }
                catch (Exception exceptionArg)
                {
                    Log.CustomWrite(exceptionArg.ToString(), "TaskPlatform--PlatformFormException");
                }

                ExecutingTaskCountInfo.Instance.UnRegisterExecutingTask(taskName);
                string waitTimeOutKey = "%sys%WaitTimeOut";
                if (result.Result == waitTimeOutKey)
                {
                    result.Result = "<线程等待超时，已自动取消执行。>";
                }
            }
            catch (Exception ex)
            {
                ExecutingTaskCountInfo.Instance.UnRegisterExecutingTask(taskName);
                result.Result = ex.ToString();
            }
            return result;
        }

        /// <summary>
        /// 开始计划
        /// </summary>
        /// <param name="taskName">计划任务名称</param>
        /// <returns>
        /// 执行结果
        /// </returns>
        public string StartPlan(string taskName)
        {
            return StartOrStopPlan(taskName, true);
        }

        /// <summary>
        /// 停用计划
        /// </summary>
        /// <param name="taskName">计划任务名称</param>
        /// <returns>
        /// 执行结果
        /// </returns>
        public string StopPlan(string taskName)
        {
            return StartOrStopPlan(taskName, false);
        }

        /// <summary>
        /// 启用或停用计划
        /// </summary>
        /// <param name="taskName">计划任务名称</param>
        /// <param name="startOrStop">启用或停用</param>
        /// <returns></returns>
        private string StartOrStopPlan(string taskName, bool startOrStop)
        {
            ResetTaskList();
            ThreadPool.QueueUserWorkItem(StartOrStopPlanThread, (startOrStop ? "1" : "0") + taskName);
            return "1" + (startOrStop ? "启用" : "停用") + "操作完成，请刷新以查看是否" + (startOrStop ? "启用" : "停用") + "成功。";
        }

        public void StartOrStopPlanThread(object args)
        {
            try
            {
                bool startOrStop = args.ToString().StartsWith("1");
                string taskName = args.ToString().Substring(1);
                this.BeginInvoke(new Action(delegate()
                {
                    NavBarItem nbiTask = GetNavBarItem(taskName);
                    DomainLoader loader = nbiTask.Tag as DomainLoader;
                    loader.Plan.Enable = startOrStop;
                    LoaclDataCacheObject item = new LoaclDataCacheObject();
                    item.Key = "Plan:" + loader.Plan.PlanName;
                    nbiTask.Name = loader.Plan.PlanName;
                    item.KeyForUpdate = item.Key;
                    item.Value = loader.Plan;
                    TaskPlatform.PlatformForm.PlanPlatformCache.Update(item);
                    TaskPlatform.PlatformForm.PlansEngine.AddPlan(loader.Plan);
                    UpdateIcon(loader.Plan.PlanName, startOrStop);
                }));
            }
            catch (Exception ex)
            {
                Log.CustomWrite(ex.ToString(), "TaskPlatform--PlatformFormException");
            }
        }

        /// <summary>
        /// 卸载计划任务
        /// </summary>
        /// <param name="taskName">计划任务名称</param>
        /// <returns>
        /// 执行结果
        /// </returns>
        public string UnloadTask(string taskName)
        {
            ThreadPool.QueueUserWorkItem(UnLoadTaskThread, taskName);
            return "1卸载操作完成，请刷新以查看是否卸载成功。";
        }

        private void UnLoadTaskThread(object taskName)
        {
            try
            {
                UnLoadTaskFull(taskName.ToString(), false);
            }
            catch (Exception ex)
            {
                Log.CustomWrite(ex.ToString(), "TaskPlatform--PlatformFormException");
            }
        }

        /// <summary>
        /// 计划任务列表(共Web控制台使用)
        /// </summary>
        DataTable taskList = new DataTable();
        PropertyInfo[] planProperties = null;
        PropertyAccess<Plan> planAccess = null;
        PropertyInfo[] alarmProperties = null;
        PropertyAccess<AlarmObject> alarmAccess = null;

        /// <summary>
        /// 获取计划任务列表
        /// </summary>
        /// <returns>
        /// 执行结果
        /// </returns>
        public DataTable GetTaskList()
        {
            int taskCount = (from f in TaskGroupList
                             where f != null && f.GroupControl != null
                             select f.GroupControl.ItemLinks.Count).Sum();
            if (taskList != null && taskCount != taskList.Rows.Count)
            {
                taskList = null;
            }
            if (taskList == null)
            {
                taskList = new DataTable();
                taskList.Columns.Add("%sys%TaskName");
                taskList.Columns.Add("%sys%Type");
                taskList.Columns.Add("名称");
                taskList.Columns.Add("状态");
                taskList.Columns.Add("描述");
                try
                {
                    DataTable dtCopy = taskList.Copy();
                    foreach (TaskGroup group in TaskGroupList)
                    {
                        if (group == null || group.GroupControl == null)
                        {
                            continue;
                        }
                        foreach (NavBarItemLink item in group.GroupControl.ItemLinks)
                        {
                            DataRow dr = dtCopy.NewRow();
                            if (item.Item.Tag.ToString().Contains("%ER%"))
                            {
                                dr["%sys%TaskName"] = item.Item.Name;
                                dr["%sys%Type"] = "ER";
                                dr["名称"] = item.Item.Name;
                                dr["状态"] = "错误";
                                dr["描述"] = "可能是无效dll";
                                dtCopy.Rows.Add(dr);
                                continue;
                            }

                            if (item.Item.Name.Contains("%NC%"))
                            {
                                dr["%sys%TaskName"] = item.Item.Caption;
                                dr["%sys%Type"] = "NC";
                                dr["名称"] = item.Item.Caption;
                                dr["状态"] = "未找到配置文件";
                                dr["描述"] = "未找到配置文件，请尝试刷新以便加载配置文件。";
                                dtCopy.Rows.Add(dr);
                                continue;
                            }

                            DomainLoader loader = item.Item.Tag as TaskDomain.DomainLoader;
                            dr["%sys%TaskName"] = loader.DomainName;
                            dr["名称"] = loader.TaskOperator.TaskName();
                            dr["描述"] = loader.TaskOperator.TaskDescription();
                            if (item.Item.Name.Contains("%NP%"))
                            {
                                dr["%sys%Type"] = "NP";
                                dr["状态"] = "未配置计划";
                            }
                            else
                            {
                                dr["%sys%Type"] = loader.Plan.Enable ? "Run" : "Stop";
                                dr["状态"] = loader.Plan.Enable ? "已启用" : "已停用";
                            }
                            dtCopy.Rows.Add(dr);
                        }
                    }
                    taskList = dtCopy.Copy();
                }
                catch { }
            }
            return taskList;
        }

        /// <summary>
        /// 重置计划任务列表
        /// </summary>
        private void ResetTaskList()
        {
            if (taskList != null)
                taskList = null;
        }

        /// <summary>
        /// 增量加载计划任务
        /// </summary>
        /// <returns>
        /// 执行结果
        /// </returns>
        public string LoadAllTask()
        {
            string result = "1已启动增量计划任务扫描器，请刷新以查看结果。";
            try
            {
                // 启动增量计划任务扫描器
                ThreadPool.QueueUserWorkItem(TasksScanner);
            }
            catch (Exception ex)
            {
                Log.CustomWrite(ex.ToString(), "TaskPlatform--PlatformFormException");
                result = "0" + ex.ToString();
            }
            return result;
        }

        /// <summary>
        /// 获取计划任务的执行计划
        /// </summary>
        /// <param name="taskName">计划任务名称</param>
        /// <returns>
        /// 若获取计划任务的执行计划失败，则返回的字符串以"-1"开头，若计划任务尚未配置过执行计划，则返回的字符串以"0"，否则返回以"1"开头的执行计划的XML格式)
        /// </returns>
        public string GetPlanXML(string taskName)
        {
            StringBuilder planXML = new StringBuilder();
            try
            {
                DomainLoader loader = GetDomainLoader(taskName);
                if (loader == null)
                {
                    planXML.Append("0不存在该计划任务。");
                }
                else
                {
                    if (loader.Plan == null)
                    {
                        planXML.Append("0计划任务尚未配置过执行计划。");
                    }
                    else
                    {
                        if (planProperties == null)
                        {
                            planProperties = typeof(Plan).GetProperties();
                        }
                        if (planAccess == null)
                        {
                            planAccess = new PropertyAccess<Plan>();
                        }
                        planXML.Append("1");
                        foreach (var item in planProperties)
                        {
                            planXML.Append("<");
                            planXML.Append(item.Name);
                            planXML.Append(">");

                            planXML.Append(planAccess.GetValue(loader.Plan, item.Name).ToString());

                            planXML.Append("</");
                            planXML.Append(item.Name);
                            planXML.Append(">");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                planXML.Clear();
                planXML.Append("-1");
                planXML.Append(ex.ToString());
            }
            return planXML.ToString();
        }

        /// <summary>
        /// 设置计划任务的执行计划
        /// </summary>
        /// <param name="planXML">执行计划的XML格式</param>
        /// <returns>
        /// 执行结果
        /// </returns>
        public string SetPlan(string planXML)
        {
            string result = "";
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(planXML);
                Plan plan = new Plan();
                XmlNode root = doc.SelectSingleNode("/Plan");
                plan.PlanName = root.SelectSingleNode("PlanName").InnerText;
                if (string.IsNullOrWhiteSpace(plan.PlanName))
                {
                    result = "0执行计划名称无效。";
                }
                DomainLoader loader = GetDomainLoader(plan.PlanName);
                if (loader.Plan != null)
                {
                    plan = loader.Plan.Copy();
                }
                plan.Enable = bool.Parse(root.SelectSingleNode("Enable").InnerText);
                plan.Type = root.SelectSingleNode("Type").InnerText.ToLower() == "repeat" ? PlanType.Repeat : PlanType.Once;
                if (plan.Type == PlanType.Once)
                {
                    plan.NextRunTime = DateTime.Parse(root.SelectSingleNode("NextRunTime").InnerText);
                }
                else
                {
                    switch (root.SelectSingleNode("PlanUnit").InnerText)
                    {
                        case "Second":
                            plan.PlanUnit = PlanTimeUnit.Second;
                            break;
                        case "Minute":
                            plan.PlanUnit = PlanTimeUnit.Minute;
                            break;
                        case "Hour":
                            plan.PlanUnit = PlanTimeUnit.Hour;
                            break;
                        case "Day":
                            plan.PlanUnit = PlanTimeUnit.Day;
                            break;
                        case "Month":
                            plan.PlanUnit = PlanTimeUnit.Month;
                            break;
                        case "Year":
                            plan.PlanUnit = PlanTimeUnit.Year;
                            break;
                        default:
                            throw new ArgumentException("执行间隔设置不正确。");
                    }
                    plan.Interval = int.Parse(root.SelectSingleNode("Interval").InnerText);
                    plan.PlanStartTime = DateTime.Parse(root.SelectSingleNode("PlanStartTime").InnerText);
                    plan.NoOverTime = bool.Parse(root.SelectSingleNode("NoOverTime").InnerText);
                    if (plan.NoOverTime)
                    {
                        plan.PlanEndTime = DateTime.MaxValue;
                    }
                    else
                    {
                        plan.PlanEndTime = DateTime.Parse(root.SelectSingleNode("PlanEndTime").InnerText);
                    }
                }
                this.Invoke(new MethodInvoker(delegate()
                {
                    loader.Plan = plan;
                    LoaclDataCacheObject item = new LoaclDataCacheObject();
                    item.Key = "Plan:" + loader.Plan.PlanName;
                    item.KeyForUpdate = item.Key;
                    item.Value = loader.Plan;
                    TaskPlatform.PlatformForm.PlanPlatformCache.Update(item);
                    // 如果是启用状态，就载入计划引擎
                    if (loader.Plan.Enable)
                    {
                        PlansEngine.AddPlan(loader.Plan);
                    }
                    UpdateIcon(loader.Plan.PlanName, loader.Plan.Enable);
                }));
                result = "1配置执行计划成功，请刷新以查看结果。";
            }
            catch (Exception ex)
            {
                result = "0" + ex.ToString();
            }
            return result;
        }

        public string CustomWrite(string title, string content, DateTime dateTime, string logName, string taskName)
        {
            Log.Write(title, content, dateTime, LogType.Custom, logName, taskName, false);
            return "已通知日志写入器写入日志。";
        }

        public string WriteLog(string taskName, string content)
        {
            Log.Write(content, taskName, false);
            return "已通知日志写入器写入日志。";
        }

        /// <summary>
        /// 显示实时运行信息
        /// </summary>
        /// <param name="taskName">计划任务名称</param>
        /// <param name="content">信息内容</param>
        /// <returns></returns>
        public string ShowRunningLog(string taskName, string content)
        {
            return PrivateShowRunningInfo(taskName, content, false);
        }

        /// <summary>
        /// 显示并记录实时运行信息
        /// </summary>
        /// <param name="taskName">计划任务名称</param>
        /// <param name="content">信息内容</param>
        /// <returns></returns>
        public string ShowWriteRunningLog(string taskName, string content)
        {
            return PrivateShowRunningInfo(taskName, content, true);
        }

        private string PrivateShowRunningInfo(string taskName, string content, bool writeLog)
        {
            long runningBatch = 0;
            bool runFromWeb = false;
            string runID = string.Empty;
            string[] keys = taskName.Split('\\');
            taskName = keys[0];
            switch (keys.Length)
            {
                case 2:
                    runningBatch = long.Parse(keys[1]);
                    break;
                case 3:
                    runningBatch = long.Parse(keys[1]);
                    runFromWeb = keys[2] == "1";
                    break;
                case 4:
                    runningBatch = long.Parse(keys[1]);
                    runFromWeb = keys[2] == "1";
                    runID = keys[3];
                    break;
            }

            #region 显示实时运行信息

            ExecutingPlanEventArgs e = new ExecutingPlanEventArgs();
            e.ExecuteTime = DateTime.Now;
            e.Information = content;
            e.PlanName = taskName;
            e.ExecuteType = ExecuteType.Executing;
            OnExecutePlan(e);

            #endregion

            if (runFromWeb && !string.IsNullOrWhiteSpace(runID))
            {
                if (RunningCache.ContainsKey(runID))
                {
                    RunningCache[runID].Enqueue(content);
                }
            }
            if (writeLog)
            {
                Log.CustomWrite(taskName, content, string.Format("Running--{0}", taskName));
                return "已通知事件协调器通知相应计划任务展示台显示实时运行信息并通知日志记录器记录日志信息。";
            }
            else
            {
                return "已通知事件协调器通知相应计划任务展示台显示实时运行信息。";
            }
        }

        public string GetAlarmXML(string taskName)
        {
            StringBuilder alarmXML = new StringBuilder();
            try
            {
                DomainLoader loader = GetDomainLoader(taskName);
                if (loader == null)
                {
                    alarmXML.Append("0不存在该计划任务。");
                }
                else
                {
                    int totalSeconds = 0;
                    if (loader.Plan != null)
                    {
                        totalSeconds = loader.Plan.TotalSeconds;
                    }
                    AlarmObject alarm = AlarmObjects.Find(a => { return a.PlanName == taskName; });
                    if (alarm == null)
                    {
                        alarm = new AlarmObject()
                        {
                            PlanName = loader.DomainName,
                            AlarmType = 4,
                            RunCount = 0,
                            AlarmWhenCancelCount = 0,
                            AlarmWhenFailCount = 0,
                            TaskName = loader.TaskOperator.TaskName()
                        };
                        TaskPlatform.PlatformForm.AlarmObjects.Add(alarm);
                    }
                    if (alarmProperties == null)
                    {
                        alarmProperties = typeof(AlarmObject).GetProperties();
                    }
                    if (alarmAccess == null)
                    {
                        alarmAccess = new PropertyAccess<AlarmObject>();
                    }
                    alarmXML.Append("1");
                    foreach (var item in alarmProperties)
                    {
                        alarmXML.Append("<");
                        alarmXML.Append(item.Name);
                        alarmXML.Append(">");

                        alarmXML.Append((alarmAccess.GetValue(alarm, item.Name) ?? "").ToString());

                        alarmXML.Append("</");
                        alarmXML.Append(item.Name);
                        alarmXML.Append(">");
                    }
                    alarmXML.Append("<TotalSeconds>");
                    alarmXML.Append(totalSeconds);
                    alarmXML.Append("</TotalSeconds>");
                    alarmXML.Append("<AlarmTypeString>");
                    alarmXML.Append(AlarmObject.GetAlarmTypeString(alarm));
                    alarmXML.Append("</AlarmTypeString>");
                }
            }
            catch (Exception ex)
            {
                alarmXML.Clear();
                alarmXML.Append("-1");
                alarmXML.Append(ex.ToString());
            }
            return alarmXML.ToString();
        }

        public string SetAlarm(string taskName, string alarmXML)
        {
            try
            {
                AlarmObject alarm = AlarmObjects.Find(item => { return item.PlanName == taskName; });
                if (alarm == null)
                {
                    return "0未找到匹配的警报信息。";
                }
                else
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(alarmXML);
                    bool enable = bool.Parse(doc.SelectSingleNode("/AlarmObject/Enable").InnerText);
                    decimal timeOutRatio = decimal.Parse(doc.SelectSingleNode("/AlarmObject/TimeOutRatio").InnerText);
                    int runCount = int.Parse(doc.SelectSingleNode("/AlarmObject/RunCount").InnerText);
                    int failCount = int.Parse(doc.SelectSingleNode("/AlarmObject/FailCount").InnerText);
                    int cancelCount = int.Parse(doc.SelectSingleNode("/AlarmObject/CancelCount").InnerText);
                    string email = doc.SelectSingleNode("/AlarmObject/Email").InnerText;
                    alarm = AlarmConfigForm.SetAlarmObject(alarm, runCount, failCount, cancelCount);
                    alarm.TimeOutRatio = timeOutRatio < 0 ? 1 : timeOutRatio;
                    alarm.AlarmEmailAddress = email;
                    if (!enable)
                    {
                        alarm.AlarmType = 4;
                    }
                    PlatformForm.SaveAlterSystemCache();
                    return "1配置成功，可重新打开配置报警页面查看是否确实成功。";
                }
            }
            catch (Exception ex)
            {
                return "0" + ex.ToString();
            }
        }

        /// <summary>
        /// 获取计划任务的自定义配置
        /// </summary>
        /// <param name="taskName">计划任务Key</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException"></exception>
        public Dictionary<string, string> GetCustomConfig(string taskName)
        {
            DomainLoader loader = GetDomainLoader(taskName);
            if (loader == null)
            {
                throw new ArgumentException(string.Format("未找到[{0}]任务。", taskName));
            }
            return loader.TaskOperator.UploadConfig();
        }

        /// <summary>
        /// 提交计划任务自定义配置项
        /// </summary>
        /// <param name="taskName">计划任务Key</param>
        /// <param name="customConfig">自定义配置项</param>
        /// <returns></returns>
        public string SubmitCustomConfig(string taskName, Dictionary<string, string> customConfig)
        {
            DomainLoader loader = GetDomainLoader(taskName);
            if (loader == null)
            {
                return string.Format("0未找到[{0}]任务。", taskName);
            }
            else
            {
                loader.TaskOperator.DownloadConfig(customConfig);
                string key = "TaskConfig:" + loader.DomainName;
                PlanPlatformCache.Update(new LoaclDataCacheObject(key, loader.TaskOperator.UploadConfig(), key));
                UpdateItemCaption(loader);
                return "1已提交自定义配置项。";
            }
        }

        /// <summary>
        /// 获取运行时信息
        /// </summary>
        /// <param name="runID">执行ID</param>
        /// <returns></returns>
        public string GetRunningInfo(string runID)
        {
            string content = "";
            if (RunningCache.ContainsKey(runID))
            {
                try
                {
                    content = RunningCache[runID].Dequeue();
                }
                catch
                {
                    content = "";
                }
                if (content == "[sys:RunOnceEnd]")
                {
                    RunningCache.Remove(runID);
                }
            }
            else
            {
                content = "[sys:RunOnceEnd]";
            }
            return content;
        }

        /// <summary>
        /// 从Web端执行一次
        /// </summary>
        /// <param name="taskName">计划任务Key</param>
        /// <param name="runID">执行ID</param>
        /// <returns></returns>
        public RunTaskResult RunOnceFromWeb(string taskName, string runID)
        {
            if (string.IsNullOrWhiteSpace(runID))
            {
                runID = "";
            }
            else
            {
                runID = runID.Replace(@"\", "");
            }
            return PrivateRunOnce(taskName, true, runID);
        }

        #endregion

        #region 通知栏处理
        FormWindowState fws = FormWindowState.Normal;
        private void tsmiShow_Click(object sender, EventArgs e)
        {
            this.Show();
            this.Activate();
            this.Visible = true;
            this.WindowState = fws;
            this.Focus();
            this.BringToFront();
        }

        private void tsmiHide_Click(object sender, EventArgs e)
        {
            fws = this.WindowState;
            this.Hide();
        }

        private void tsmiExit_Click(object sender, EventArgs e)
        {
            tsmiShow_Click(null, null);
            this.Close();
        }

        private void PlatformForm_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
            }
            else
            {
                fws = this.WindowState;
            }
        }

        private void ntfPlatformNotify_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
            {
                ntfPlatformNotify.ShowBalloonTip(5000, "计划任务平台提示", string.Format("计划任务平台正在运行:{3}1、IPC：{0}；{3}2、是否安全模式：{1}；{3}3、版本：{2}。", PlatformPortName, SafeMode ? "是" : "否", Application.ProductVersion, Environment.NewLine), ToolTipIcon.Info);
            }
        }

        #endregion

    }
}
