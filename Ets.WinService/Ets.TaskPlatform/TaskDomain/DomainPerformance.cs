using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Lifetime;

namespace TaskPlatform.TaskDomain
{
    /// <summary>
    /// 计划任务运行时域性能指标
    /// </summary>
    [Serializable]
    [ComVisibleAttribute(true)]
    public class DomainPerformance : MarshalByRefObject
    {
        /// <summary>
        /// 初始化一个计划任务运行时域性能指标对象
        /// </summary>
        public DomainPerformance()
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            this.ID = currentDomain.Id;
            this.MonitoringSurvivedMemorySize = currentDomain.MonitoringSurvivedMemorySize;
            this.MonitoringSurvivedProcessMemorySize = AppDomain.MonitoringSurvivedProcessMemorySize;
            this.MonitoringTotalAllocatedMemorySize = currentDomain.MonitoringTotalAllocatedMemorySize;
            this.MonitoringTotalProcessorTime = currentDomain.MonitoringTotalProcessorTime;
            this.Name = _domainName;
        }

        private static string _domainName = string.Empty;
        /// <summary>
        /// 名称
        /// </summary>
        [Obsolete("请勿使用该属性。该属性不准确")]
        internal static string DomainName
        {
            get { return _domainName; }
            set { _domainName = value; }
        }

        /// <summary>
        /// 获得一个整数，该整数唯一标识进程中的应用程序域。
        /// </summary>
        public int ID { get; internal set; }
        /// <summary>
        /// 获取名称
        /// </summary>
        [Obsolete("请勿使用该属性。该属性不准确")]
        public string Name { get; internal set; }
        /// <summary>
        /// 获取上次完全阻止回收后保留下来的、已知由当前应用程序域引用的字节数。
        /// </summary>
        public long MonitoringSurvivedMemorySize { get; internal set; }
        /// <summary>
        /// 获取进程中所有应用程序域的上次完全阻止回收后保留下来的总字节数。
        /// </summary>
        public long MonitoringSurvivedProcessMemorySize { get; internal set; }
        /// <summary>
        /// 获取自从创建应用程序域后由应用程序域进行的所有内存分配的总大小（以字节为单位，不扣除已回收的内存）。
        /// </summary>
        public long MonitoringTotalAllocatedMemorySize { get; internal set; }
        /// <summary>
        /// 获取自从进程启动后所有线程在当前应用程序域中执行时所使用的总处理器时间。
        /// </summary>
        public TimeSpan MonitoringTotalProcessorTime { get; internal set; }

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

        /// <summary>
        /// 获取计划任务运行时域性能指标字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}({1}):MonitoringSurvivedMemorySize-{2},MonitoringSurvivedProcessMemorySize-{3},MonitoringTotalAllocatedMemorySize-{4},MonitoringTotalProcessorTime-{5}", this.Name, this.ID, this.MonitoringSurvivedMemorySize, this.MonitoringSurvivedProcessMemorySize, this.MonitoringTotalAllocatedMemorySize, this.MonitoringTotalProcessorTime.ToString("G"));
        }
    }
}
