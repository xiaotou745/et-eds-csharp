using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading;
using TaskPlatform.PlatformLog;

namespace TaskPlatform.PlanEngine
{
    /// <summary>
    /// 心跳器，产生心律。自动控制心跳后等待处理结束才开始计时。
    /// </summary>
    [Serializable]
    public class Heartbeat
    {
        private System.Timers.Timer _heart = null;
        private TimeSpan _lastProcessTimeSpan = new TimeSpan(0, 0, 0);

        /// <summary>
        /// 触发心跳事件的委托。
        /// </summary>
        /// <param name="sender">心跳器</param>
        /// <param name="e">包含心跳时的数据</param>
        public delegate void HeartbeatHandler(object sender, BeatEventArgs e);
        /// <summary>
        /// 产生心跳时发生。将自动等候处理程序结束才开始下次心跳的计时。
        /// </summary>
        public event HeartbeatHandler Beat;
        /// <summary>
        /// 触发心跳事件
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnBeat(BeatEventArgs e)
        {
            if (Beat != null)
                Beat(this, e);
        }

        private void FireBeat(object e)
        {
            BeatEventArgs args = e as BeatEventArgs;
            OnBeat(args);
        }

        public Heartbeat()
        {
            _heart = new System.Timers.Timer(1000);
            Stop();
            _heart.AutoReset = true;
            _heart.Elapsed += new ElapsedEventHandler(Heart_Elapsed);
        }

        /// <summary>
        /// 开启心跳
        /// </summary>
        public void Start()
        {
            _heart.Enabled = true;
        }

        /// <summary>
        /// 停止心跳
        /// </summary>
        public void Stop()
        {
            _heart.Enabled = false;
        }

        private void Heart_Elapsed(object sender, ElapsedEventArgs e)
        {
            _heart.Stop();

            // 触发心跳，为了不影响正常心跳，将忽略任何外部异常。
            // 最大限度保证心律的正常。
            try
            {
                BeatEventArgs args = new BeatEventArgs();
                args.BeatTime = e.SignalTime;
                args.LastProcessTimeSpan = _lastProcessTimeSpan;
                ThreadPool.QueueUserWorkItem(FireBeat, args);
            }
            catch (Exception ex)
            {
                Log.CustomWrite(ex.ToString(), "PlanEngine--HeartbeatException");
                throw ex;
            }
            finally
            {
                try
                {
                    // 记录心跳处理时间
                    _lastProcessTimeSpan = DateTime.Now - e.SignalTime;
                }
                catch (Exception ex)
                {
                    Log.CustomWrite(ex.ToString(), "PlanEngine--HeartbeatException");
                }

                _heart.Start();
            }
        }
    }
}
