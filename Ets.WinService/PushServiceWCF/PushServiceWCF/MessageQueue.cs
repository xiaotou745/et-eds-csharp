using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Xml.Serialization;

namespace PushServiceWCF
{
    /// <summary>
    /// 定义全局静态队列 保证推送消息队列唯一
    /// </summary>
    public static class MessageQueue
    {
        // private static CYPLog.TextLogger _log;
        private static IPushSer push;
        private static Thread PushThread = null;
        /// <summary>
        /// List作为队列容器
        /// </summary>
        public static List<LateMsgModel> Queue;
        /// <summary>
        /// 全局唯一锁(添加删除)
        /// </summary>
        private static object MyLock = new object();
        /// <summary>
        /// 全局唯一锁(线程循环)
        /// </summary>
        private static object MyLockThread = new object();
        static MessageQueue()
        {
            // _log = CYPLog.TextLogManager.Create(typeof(MessageQueue));
            PushThread = new Thread(new ThreadStart(PushNow));
            PushThread.Name = "PushTh";
            push = new PushSer();
            Queue = new List<LateMsgModel>();
            PushThread.Start();
        }

        /// <summary>
        /// 向队列中添加消息
        /// </summary>
        /// <param name="addItem"></param>
        public static void AddItem(LateMsgModel addItem)
        {
            try
            {
                lock (MyLock)
                {
                    Queue.Add(addItem);
                }
            }
            catch (Exception e)
            {

                // _log.Error(e.ToString());
            }

        }
        /// <summary>
        /// 移除某条消息
        /// </summary>
        /// <param name="addItem"></param>
        public static void RemItem(LateMsgModel addItem)
        {
            try
            {
                lock (MyLock)
                {
                    Queue.Remove(addItem);
                }
            }
            catch (Exception e)
            {

                //_log.Error(e.ToString());
            }

        }


        /// <summary>
        /// 开始线程
        /// </summary>
        public static void StartPushThread()
        {
            try
            {
                if (PushThread.ThreadState != ThreadState.Stopped)
                {
                    PushThread.Start();
                }
            }
            catch (ThreadStateException e)
            {
                // _log.Error(e.ToString());
                //throw;
            }

        }
        /// <summary>
        /// 循环队列中的数据
        /// </summary>
        private static void PushNow()
        {

            try
            {
                while (true)
                {
                    //队列数据大于0 循环
                    lock (MyLock)
                    {
                        //加锁消息池
                        for (int i = 0; i < Queue.Count; i++)
                        {
                            if (Queue[i].PushTime <= DateTime.Now)
                            {
                                //IPushSer push = new PushSer();
                                push.PushForMobile(Queue[i].MsgInfo);
                                Queue.Remove(Queue[i]);
                            }
                        }
                    }
                }

            }
            catch (ThreadStateException e)
            {

                //_log.Error(e.ToString());
            }

        }
    }
}