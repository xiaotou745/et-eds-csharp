using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Messaging;
using System.Configuration;
using System.Collections.Specialized;
using System.Net.Mail;

namespace TaskPlatform.TaskInterface
{
    public class PlatformService
    { }
    public static class SafeExecutor
    {
        private static string _getValueSafeLocker = "_getValueSafeLocker";
        /// <summary>
        /// 获取值(安全)
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static PlatformService GetValueSafe(this Dictionary<string, PlatformService> instance, Type type)
        {
            string key = type.Name;
            if (instance.ContainsKey(key))
            {
                return instance[key];
            }
            else
            {
                lock (_getValueSafeLocker)
                {
                    if (instance.ContainsKey(key))
                    {
                        return instance[key];
                    }
                    else
                    {
                        PlatformService service = Activator.CreateInstance(type) as PlatformService;
                        instance.Add(key, service);
                        return service;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 提供XML序列化反序列化支持
    /// </summary>
    /// <typeparam name="T">要操作的对象类型</typeparam>
    public class XmlSerializerService<T> where T : class
    {
        XmlSerializer serializer;
        /// <summary>
        /// 初始化服务实例
        /// </summary>
        public XmlSerializerService()
        {
            //SerializationBinder

            serializer = new XmlSerializer(typeof(T));
        }
        /// <summary>
        /// 获取服务ID
        /// </summary>
        /// <returns></returns>
        public string GetServiceID()
        {
            return this.GetHashCode().ToString();
        }
        /// <summary>
        /// 将对象序列化为字符串
        /// </summary>
        /// <param name="TObject">要序列化的对象</param>
        /// <returns></returns>
        public string Serializer(T TObject)
        {
            string result = string.Empty;
            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, TObject);
                result = writer.ToString();
            }
            return result;
        }
        /// <summary>
        /// 将字符串反序列化为对象
        /// </summary>
        /// <param name="content">要反序列化的字符串</param>
        /// <returns></returns>
        public T DeSerializer(string content)
        {
            T result = default(T);
            using (StringReader reader = new StringReader(content))
            {
                result = serializer.Deserialize(reader) as T;
            }
            return result;
        }
    }

    /// <summary>
    /// 消息队列操作类库
    /// </summary>
    /// <typeparam name="MessageType">消息内容类型</typeparam>
    public class MSMQ<MessageType> where MessageType : class
    {
        private string _path = "";
        /// <summary>
        /// 消息队列路径
        /// </summary>
        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }
        private int _timeOut = 1;
        /// <summary>
        /// 读取超时时间
        /// </summary>
        public int TimeOut
        {
            get { return _timeOut; }
            set { _timeOut = value; }
        }
        /// <summary>
        /// 消息队列名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 执行增量
        /// </summary>
        public long ExecuteIncremental { get; internal set; }
        private long _order = 1;
        /// <summary>
        /// 顺序号
        /// </summary>
        public long Order
        {
            get { return _order; }
            internal set { _order = value; }
        }
        private static XmlSerializerService<MessageType> _serializerService;
        /// <summary>
        /// 序列化服务
        /// </summary>
        public static XmlSerializerService<MessageType> SerializerService
        {
            get
            {
                if (_serializerService == null)
                {
                    _serializerService = new XmlSerializerService<MessageType>();
                }
                return _serializerService;
            }
        }
        private MessageQueue _messageQueue;
        /// <summary>
        /// 消息队列
        /// </summary>
        public MessageQueue Queue
        {
            get
            {
                if (_messageQueue == null)
                {
                    _messageQueue = new MessageQueue(_path);
                    ((XmlMessageFormatter)_messageQueue.Formatter).TargetTypes = new Type[] { typeof(string) };
                }
                return _messageQueue;
            }
        }

        private string _lastMessageID = string.Empty;
        /// <summary>
        /// 接收到的最后一条消息的ID
        /// </summary>
        public string LastMessageID
        {
            get { return _lastMessageID; }
            set { _lastMessageID = value; }
        }

        /// <summary>
        /// 实例化MSMQ类。
        /// </summary>
        /// <param name="path">消息队列路径</param>
        /// <param name="timeout">读取超时时间(默认10秒)</param>
        public MSMQ(string path, int timeout = 1)
        {
            _path = path;
            _timeOut = timeout;
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message">要发送的消息</param>
        /// <param name="label">消息标签。建议传入能确定消息身份和唯一内容的值。</param>
        public void SendMessage(MessageType message, string label = "")
        {
            string messageXML = string.Empty;
            messageXML = SerializerService.Serializer(message);
            System.Messaging.Message msg = new System.Messaging.Message();
            msg.Label = label;
            msg.Body = messageXML;
            Queue.Send(msg, label, MessageQueueTransactionType.Automatic);
        }
        /// <summary>
        /// 接收一条消息
        /// </summary>
        /// <returns></returns>
        public MessageType ReceiveMessage()
        {
            System.Messaging.Message message = null;
            try
            {
                message = Queue.Receive(new TimeSpan(0, 0, TimeOut), MessageQueueTransactionType.Automatic);
            }
            catch (MessageQueueException exception)
            {
                if (exception.MessageQueueErrorCode == MessageQueueErrorCode.IOTimeout)
                {
                    return null;
                }
                else
                {
                    throw exception;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            MessageType msg = SerializerService.DeSerializer(message.Body.ToString());
            return msg;
        }
        /// <summary>
        /// 获取第一条消息(但不会删除)。该方法为阻塞模式，会等到第一个消息入队或超时时间到。
        /// </summary>
        /// <returns></returns>
        public MessageType PeekMessage()
        {
            System.Messaging.Message message = null;
            try
            {
                message = Queue.Peek(new TimeSpan(0, 0, TimeOut));
            }
            catch (MessageQueueException exception)
            {
                if (exception.MessageQueueErrorCode == MessageQueueErrorCode.IOTimeout)
                {
                    return null;
                }
                else
                {
                    throw exception;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            MessageType msg = SerializerService.DeSerializer(message.Body.ToString());
            return msg;
        }
        /// <summary>
        /// 获取消息枚举。
        /// <para>可以调用RemoveCurrent()删除当前消息。</para>
        /// </summary>
        public MessageEnumerator GetMessageEnumerator2()
        {
            MessageEnumerator enumerator = null;
            try
            {
                enumerator = Queue.GetMessageEnumerator2();
            }
            catch (MessageQueueException exception)
            {
                if (exception.MessageQueueErrorCode == MessageQueueErrorCode.IOTimeout)
                {
                    return null;
                }
                else
                {
                    throw exception;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return enumerator;
        }
        /// <summary>
        /// 获取指定数量的消息
        /// </summary>
        /// <param name="count">要获取的消息数量</param>
        /// <returns></returns>
        public List<MessageType> GetMessageList(int count)
        {
            List<MessageType> list = new List<MessageType>();
            if (count < 1)
                return list;
            else
            {
                MessageEnumerator enumerator = GetMessageEnumerator2();
                if (enumerator == null)
                    return list;
                else
                {
                    int nowCount = 0;
                    TimeSpan ts = new TimeSpan(0, 0, 0, 0, 200);
                    while (enumerator.MoveNext(ts) && nowCount < count)
                    {
                        list.Add(SerializerService.DeSerializer(enumerator.RemoveCurrent(MessageQueueTransactionType.Automatic).Body.ToString()));
                        nowCount++;
                        enumerator.Reset();
                    }
                }
            }
            return list;
        }
    }

    /// <summary>
    /// 消息队列总线
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class MSMQBus<T> where T : class
    {
        internal MSMQBus()
        { }
        private string _msmqKey;
        /// <summary>
        /// 总线Key值
        /// </summary>
        public string MSMQKey
        {
            get { return _msmqKey; }
            set { _msmqKey = value; }
        }

        private string _busLocker = "_busLocker";
        private List<MSMQ<T>> _msmqList = null;
        /// <summary>
        /// 消息队列总线队列
        /// </summary>
        internal List<MSMQ<T>> MSMQList
        {
            get
            {
                if (_msmqList == null)
                {
                    lock (_busLocker)
                    {
                        if (_msmqList == null)
                        {
                            _msmqList = new List<MSMQ<T>>();
                        }
                    }
                }
                return _msmqList;
            }
            private set { _msmqList = value; }
        }

        /// <summary>
        /// 注册消息队列
        /// </summary>
        /// <param name="name">消息队列名称</param>
        /// <param name="path">消息队列路径</param>
        internal MSMQ<T> RegisterMSMQ(string msmqName, string msmqPath, int timeOut = 1)
        {
            MSMQ<T> msmq = MSMQList.FirstOrDefault(item => { return item != null && item.Name == msmqName; });
            if (msmq == null)
            {
                long maxIncremental = 0;
                if (MSMQList.Count > 0)
                {
                    maxIncremental = MSMQList.Max(item => { return item.ExecuteIncremental; });
                }
                msmq = new MSMQ<T>(msmqPath, timeOut) { Name = msmqName, Order = maxIncremental + 1, ExecuteIncremental = maxIncremental + 1 };
                MSMQList.Add(msmq);
            }
            return msmq;
        }

        /// <summary>
        /// 反注册消息队列
        /// </summary>
        /// <param name="msmqName">消息队列名称</param>
        internal void UnRegisterMSMQ(string msmqName)
        {
            MSMQ<T> msmq = MSMQList.FirstOrDefault(item => { return item != null && item.Name == msmqName; });
            if (msmq != null)
            {
                MSMQList.Remove(msmq);
            }
        }

        /// <summary>
        /// 分发消息
        /// </summary>
        /// <param name="message">需要分发的消息</param>
        /// <param name="label">消息标签</param>
        /// <exception cref="System.Exception">MSMQBus was damaged......</exception>
        internal void DistributeMessage(T message, string label = "")
        {
            if (MSMQList.Count < 1)
            {
                throw new Exception("总线内没有消息队列可用。");
            }
            long minIncremental = MSMQList.Min(item => { return item.ExecuteIncremental; });
            long maxIncremental = MSMQList.Max(item => { return item.ExecuteIncremental; });
            MSMQ<T> msmq = MSMQList.FirstOrDefault(item => { return item.ExecuteIncremental == minIncremental; });
            if (msmq == null)
            {
                throw new Exception("MSMQBus was damaged......");
            }
            else
            {
                msmq.SendMessage(message, label);
                msmq.ExecuteIncremental = maxIncremental + 1;
            }
        }
    }

    /// <summary>
    /// 消息队列总线服务
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class MSMQBusService<T> where T : class
    {
        internal MSMQBusService()
        { }

        static MSMQBusService()
        {
            // 在不同域，无法静态注册
            // 在任务的初始化时注册
        }

        private static bool _isRegisterFinished = false;
        /// <summary>
        /// 是否已注册完成
        /// </summary>
        public static bool IsRegisterFinished
        {
            get { return MSMQBusService<T>._isRegisterFinished; }
            set { MSMQBusService<T>._isRegisterFinished = value; }
        }
        private static string _busLocker = "_busLocker";
        private static string _lockerContains = "_lockerContains";
        private static Dictionary<string, MSMQBus<T>> _msmqBusList = null;
        /// <summary>
        /// 消息队列总线队列
        /// </summary>
        private static Dictionary<string, MSMQBus<T>> MSMQBusList
        {
            get
            {
                if (_msmqBusList == null)
                {
                    lock (_busLocker)
                    {
                        if (_msmqBusList == null)
                        {
                            _msmqBusList = new Dictionary<string, MSMQBus<T>>();
                        }
                    }
                }
                return MSMQBusService<T>._msmqBusList;
            }
            set { MSMQBusService<T>._msmqBusList = value; }
        }

        /// <summary>
        /// 获取消息队列总线
        /// </summary>
        /// <param name="busName">总线名称</param>
        /// <returns></returns>
        private static MSMQBus<T> GetMSMQBus(string busName, bool autoRegister = true)
        {
            if (MSMQBusList.ContainsKey(busName))
                return MSMQBusList[busName];
            else
            {
                if (autoRegister)
                {
                    lock (_lockerContains)
                    {
                        if (MSMQBusList.ContainsKey(busName))
                            return MSMQBusList[busName];
                        else
                        {
                            MSMQBus<T> bus = new MSMQBus<T>();
                            MSMQBusList.Add(busName, bus);
                            return bus;
                        }
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 注册消息队列总线
        /// </summary>
        /// <param name="busName">总线名称</param>
        public static void RegisterMSMQBus(string busName)
        {
            if (MSMQBusList.ContainsKey(busName))
                return;
            else
            {
                lock (_lockerContains)
                {
                    if (MSMQBusList.ContainsKey(busName))
                        return;
                    else
                    {
                        MSMQBusList.Add(busName, new MSMQBus<T>());
                    }
                }
            }
        }

        /// <summary>
        /// 反注册消息队列总线
        /// </summary>
        /// <param name="busName">总线名称</param>
        public static void UnRegisterMSMQBus(string busName)
        {
            if (!MSMQBusList.ContainsKey(busName))
                return;
            else
            {
                lock (_lockerContains)
                {
                    if (!MSMQBusList.ContainsKey(busName))
                        return;
                    else
                    {
                        MSMQBusList.Remove(busName);
                    }
                }
            }
        }

        /// <summary>
        /// 注册消息队列到总线(如果总线中没有相应Key的消息队列列表，将自动注册该Key的总线)
        /// </summary>
        /// <param name="busName">总线名称</param>
        /// <param name="msmqName">消息队列名称</param>
        /// <param name="msmqPath">消息队列路径</param>
        public static MSMQ<T> RegisterMSMQToBus(string busName, string msmqName, string msmqPath, int timeOut = 1)
        {
            MSMQBus<T> bus = GetMSMQBus(busName);
            return bus.RegisterMSMQ(msmqName, msmqPath, timeOut);
        }

        /// <summary>
        /// 从总线反注册消息队列
        /// </summary>
        /// <param name="busName"></param>
        /// <param name="msmqName"></param>
        public static void UnRegisterMSMQFromBus(string busName, string msmqName)
        {
            MSMQBus<T> bus = GetMSMQBus(busName);
            bus.UnRegisterMSMQ(msmqName);
        }

        /// <summary>
        /// 注册所有消息队列总线
        /// </summary>
        /// <param name="appSettings">应用程序配置</param>
        /// <param name="timeOut">接收消息超时时间</param>
        public static void RegisterAllBus(NameValueCollection appSettings, int timeOut = 1)
        {
            if (appSettings == null)
                return;

            string preStringForMSMQPath = "MSMQPath_";

            var pathList = (from f in appSettings.AllKeys
                            where !string.IsNullOrWhiteSpace(f) && f.StartsWith(preStringForMSMQPath)
                            select f).ToList();
            var busNameList = (from busName in
                                   (from f in pathList
                                    select f.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries))
                               where busName.Length > 2
                               select busName[1]).Distinct().ToList();
            if (busNameList.Count > 0)
            {
                busNameList.ForEach(busName =>
                {
                    RegisterMSMQBus(busName);
                    string busKey = string.Concat(preStringForMSMQPath, busName, "_");
                    var msmqNameList = (from f in pathList
                                        where f.StartsWith(busKey)
                                        select new { Key = f, Name = f.Substring(preStringForMSMQPath.Length) }).ToList();
                    msmqNameList.ForEach(msmq =>
                    {
                        RegisterMSMQToBus(busName, msmq.Name, appSettings[msmq.Key]);
                    });
                });
            }
        }

        /// <summary>
        /// 注册所有消息队列总线
        /// </summary>
        /// <param name="configs">应用程序配置</param>
        /// <param name="timeOut">接收消息超时时间</param>
        public static void RegisterAllBus(Dictionary<string, string> configs, int timeOut = 1)
        {
            if (configs == null)
                return;

            string preStringForMSMQPath = "MSMQPath_";

            var pathList = (from f in configs.Keys
                            where !string.IsNullOrWhiteSpace(f) && f.StartsWith(preStringForMSMQPath)
                            select f).ToList();
            var busNameList = (from busName in
                                   (from f in pathList
                                    select f.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries))
                               where busName.Length > 2
                               select busName[1]).Distinct().ToList();
            if (busNameList.Count > 0)
            {
                busNameList.ForEach(busName =>
                {
                    RegisterMSMQBus(busName);
                    string busKey = string.Concat(preStringForMSMQPath, busName, "_");
                    var msmqNameList = (from f in pathList
                                        where f.StartsWith(busKey)
                                        select new { Key = f, Name = f.Substring(preStringForMSMQPath.Length) }).ToList();
                    msmqNameList.ForEach(msmq =>
                    {
                        RegisterMSMQToBus(busName, msmq.Name, configs[msmq.Key]);
                    });
                });
            }
        }

        /// <summary>
        /// 分发消息到总线
        /// </summary>
        /// <param name="busName">总线名称</param>
        /// <param name="message">需要分发的消息</param>
        /// <param name="label">消息标签</param>
        public static void DistributeMessageToBus(string busName, T message, string label = "")
        {
            MSMQBus<T> bus = GetMSMQBus(busName, false);
            if (bus == null)
            {
                throw new Exception("消息队列总线尚未注册。");
            }
            else
            {
                bus.DistributeMessage(message, label);
            }
        }
    }
}
