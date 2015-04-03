using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaskPlatform.Commom
{
    /// <summary>
    /// 提供限制运行中任务数量的单例模式实现
    /// </summary>
    public class ExecutingTaskCountInfo
    {
        private int _maxExecutingTaskCount = 2;

        /// <summary>
        /// 运行执行中的任务数量(默认2)
        /// </summary>
        public int MaxExecutingTaskCount
        {
            get { return _maxExecutingTaskCount; }
            set { _maxExecutingTaskCount = value; }
        }

        private Dictionary<string, int> _executingTaskCountInfo = new Dictionary<string, int>();
        private static string _locker = "_locker";
        private static ExecutingTaskCountInfo _instance;
        /// <summary>
        /// 限制运行中任务数量实现实例
        /// </summary>
        internal static ExecutingTaskCountInfo Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_locker)
                    {
                        if (_instance == null)
                        {
                            _instance = new ExecutingTaskCountInfo();
                            try
                            {
                                if (PlatformForm.AppSettings.ContainsKey("MaxExecutingTaskCount"))
                                {
                                    _instance.MaxExecutingTaskCount = int.Parse(PlatformForm.AppSettings["MaxExecutingTaskCount"]);
                                }
                            }
                            catch { }
                        }
                    }
                }
                return _instance;
            }
        }
        private ExecutingTaskCountInfo()
        {

        }

        /// <summary>
        /// 注册运行并返回是否可以继续加入运行队列
        /// </summary>
        /// <param name="taskKey">计划任务Key值</param>
        /// <returns></returns>
        public bool RegisterExecutingTask(string taskKey)
        {
            if (!_executingTaskCountInfo.ContainsKey(taskKey))
            {
                lock (_locker)
                {
                    if (!_executingTaskCountInfo.ContainsKey(taskKey))
                    {
                        //Console.WriteLine("RNL1");
                        _executingTaskCountInfo.Add(taskKey, 1);
                        return 1 <= _maxExecutingTaskCount;
                    }
                }
            }
            if (_executingTaskCountInfo[taskKey] >= _maxExecutingTaskCount)
            {
                //Console.WriteLine("RF");
                return false;
            }
            else
            {
                lock (_locker)
                {
                    if (_executingTaskCountInfo[taskKey] >= _maxExecutingTaskCount)
                    {
                        //Console.WriteLine("RLF");
                        return false;
                    }
                    else
                    {
                        _executingTaskCountInfo[taskKey] = _executingTaskCountInfo[taskKey] + 1;
                        //Console.WriteLine("RL" + _executingTaskCountInfo[taskKey]);
                        return _executingTaskCountInfo[taskKey] <= _maxExecutingTaskCount;
                    }
                }
            }
        }

        /// <summary>
        /// 反注册运行
        /// </summary>
        /// <param name="taskKey">The task key.</param>
        public void UnRegisterExecutingTask(string taskKey)
        {
            if (_executingTaskCountInfo.ContainsKey(taskKey) && _executingTaskCountInfo[taskKey] > 0)
            {
                lock (_locker)
                {
                    if (_executingTaskCountInfo.ContainsKey(taskKey) && _executingTaskCountInfo[taskKey] > 0)
                    {
                        //Console.WriteLine("UL" + _executingTaskCountInfo[taskKey]);
                        _executingTaskCountInfo[taskKey] = _executingTaskCountInfo[taskKey] - 1;
                    }
                    //else
                    //{
                    //    Console.WriteLine("UL");
                    //}
                }
            }
            //else
            //{
            //    Console.WriteLine("U");
            //}
        }
    }
}
