using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Reflection;

namespace TaskPlatform.PlanEngine
{
    /// <summary>
    /// 计划引擎。为计划任务的计划提供触发执行的引擎。
    /// </summary>
    [Serializable]
    public class PlanEngine
    {
        private Control _control = null;
        private bool _async = false;
        private Heartbeat _heart = null;
        private List<Plan> _plans = new List<Plan>();
        private bool _started = false;

        /// <summary>
        /// 获取计划列表
        /// </summary>
        public List<Plan> Plans
        {
            get
            {
                return _plans;
            }
            set
            {
                _plans = value;
            }
        }

        /// <summary>
        /// 获取引擎是否已启动
        /// </summary>
        public bool Started
        {
            get
            {
                return _started;
            }
        }

        /// <summary>
        /// 获取当前引擎中正在运行的计划个数。
        /// </summary>
        public int Count
        {
            get
            {
                Trim();
                return _plans.Count;
            }
        }

        /// <summary>
        /// 获取或设置一个值，该值指示引擎是否以异步方式生成事件。
        /// 如果使用异步方式生成事件，则计划的LastProcessTimeSpan属性将不准确。
        /// </summary>
        public bool AsyncEvent
        {
            get { return _async; }
            set { _async = value; }
        }

        /// <summary>
        /// 触发计划的执行所必需的委托。
        /// </summary>
        /// <param name="sender">触发计划执行的引擎。</param>
        /// <param name="e">触发计划执行时所包含的数据。</param>
        public delegate void ExecutePlanHandler(object sender, ExecutePlanEventArgs e);
        /// <summary>
        /// 当有需要执行的计划时发生。
        /// </summary>
        public event ExecutePlanHandler ExecutePlan;
        /// <summary>
        /// 触发计划的执行。
        /// </summary>
        /// <param name="e"></param>
        protected void OnExecutePlan(ExecutePlanEventArgs e)
        {
            if (ExecutePlan != null)
                ExecutePlan(this, e);
        }

        /// <summary>
        /// 创建计划引擎。该引擎所引发的事件，不能直接对窗体或别的控件进行修改。除非使用Invoke或者BeginInvoke形式。
        /// </summary>
        public PlanEngine()
        {

        }

        /// <summary>
        /// 在指定的处理控件上创建计划引擎。该控件将作为触发计划执行的委托承载体。引擎将通过调用该控件的Invoke方法，来生成事件。
        /// </summary>
        /// <param name="control">引发计划执行的委托承载体。</param>
        /// <param name="async">指示引擎是否以异步方式生成事件。如果使用异步方式生成事件，则计划的LastProcessTimeSpan属性将不准确。</param>
        public PlanEngine(Control control, bool async = false)
        {
            _control = control;
            _async = async;
        }

        /// <summary>
        /// 初始化引擎
        /// </summary>
        public void InitEngine()
        {
            _heart = new Heartbeat();
            _heart.Beat += Heart_Beat;
        }

        private void Heart_Beat(object sender, BeatEventArgs e)
        {
            Trim();
            // 无法使用Linq。
            List<Plan> trigPlans = new List<Plan>();
            foreach (var plan in _plans)
            {
                if (plan.PlanStartTime <= e.BeatTime && plan.PlanEndTime >= e.BeatTime && (plan.NextRunTime.ToString("yyyy-MM-dd HH:mm:ss") == e.BeatTime.ToString("yyyy-MM-dd HH:mm:ss") || plan.NextRunTime < e.BeatTime))
                    trigPlans.Add(plan);
            }

            foreach (var plan in trigPlans)
            {
                plan.StartPlan();
                ExecutePlanEventArgs args = new ExecutePlanEventArgs();
                args.ExecuteTime = e.BeatTime;
                args.PlanName = plan.PlanName;
                args.LastProcessTimeSpan = plan.LastProcessTimeSpan;
                MethodInvoker methodInvoker = new MethodInvoker(delegate
                {
                    OnExecutePlan(args);
                });
                if (_control != null && _control.IsHandleCreated)
                {
                    if (_async)
                    {
                        _control.BeginInvoke(methodInvoker);
                    }
                    else
                    {
                        _control.Invoke(methodInvoker);
                    }
                }
                else
                {
                    OnExecutePlan(args);
                }
                plan.EndPlan();
            }
        }

        /// <summary>
        /// 开启引擎。
        /// </summary>
        public void StartEngine()
        {
            if (_heart == null)
                throw new InvalidOperationException("计划引擎尚未初始化。");
            _heart.Start();
            _started = true;
        }

        /// <summary>
        /// 关闭引擎
        /// </summary>
        public void StopEngine()
        {
            if (_heart == null)
                throw new InvalidOperationException("计划引擎尚未初始化。");
            _heart.Stop();
            _started = false;
        }

        /// <summary>
        /// 获取具有指定名称的计划。
        /// </summary>
        /// <param name="planName"></param>
        /// <returns></returns>
        public Plan GetPlan(string planName)
        {
            var result = from plan in _plans
                         where plan.PlanName == planName
                         select plan;
            if (result == null || result.Count() <= 0)
                return null;
            else
                return result.First();
        }

        /// <summary>
        /// 判断引擎中是否包含具有指定名称的计划。
        /// </summary>
        /// <param name="planName"></param>
        /// <returns></returns>
        public bool Contains(string planName)
        {
            return GetPlan(planName) != null;
        }

        /// <summary>
        /// 向计划引擎中添加计划
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        public void AddPlan(Plan plan)
        {
            // 如果已经存在就删除，然后添加
            if (Contains(plan.PlanName))
            {
                Remove(plan.PlanName);
            }
            _plans.Add(plan);
        }

        /// <summary>
        /// 移除具有指定名称的计划。
        /// </summary>
        /// <param name="planName"></param>
        public void Remove(string planName)
        {
            if (Contains(planName))
            {
                _plans.Remove(GetPlan(planName));
            }
        }

        /// <summary>
        /// 清空引擎中的计划。
        /// </summary>
        public void Clear()
        {
            _plans.Clear();
        }

        /// <summary>
        /// 清除引擎中已停用的计划。
        /// </summary>
        public void Trim()
        {
            Monitor.Enter(_plans);
            _plans.TrimExcess();
            List<Plan> tmp = new List<Plan>();
            foreach (var plan in _plans)
            {
                if (!plan.Enable || plan.PlanEndTime < DateTime.Now)
                    tmp.Add(plan);
            }
            foreach (var plan in tmp)
            {
                _plans.Remove(plan);
            }
            Monitor.Exit(_plans);
        }

        /// <summary>
        /// 关闭引擎。
        /// </summary>
        ~PlanEngine()
        {
            StopEngine();
        }
    }
}
