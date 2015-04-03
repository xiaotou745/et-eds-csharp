using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using System.Threading;
using System.Collections;
using TaskPlatform.PlatformLog;
using Db4objects.Db4o;
using Db4objects.Db4o.Linq;
using Db4objects.Db4o.Config.Encoding;
using Db4objects.Db4o.Config.Attributes;

namespace TaskPlatform.Commom
{
    /// <summary>
    /// 本地缓存数据对象
    /// </summary>
    [Serializable]
    public class LoaclDataCacheObject
    {
        [Indexed]
        private string _key = "";
        /// <summary>
        /// 获取或设置对象的键。设置该键时，会同步修改更新标志(KeyForUpdate)的值。
        /// </summary>
        public string Key
        {
            get
            {
                return _key;
            }
            set
            {
                _key = value;
                KeyForUpdate = _key;
            }
        }
        /// <summary>
        /// 获取或设置对象的值.
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// 更新标志。用于更新的键，如果需要更新该对象，则应最后为该属性赋值。
        /// </summary>
        public string KeyForUpdate { get; set; }
        /// <summary>
        /// 创建对象的时间
        /// </summary>
        private DateTime _createTime = DateTime.Now;
        /// <summary>
        /// 创建对象的时间
        /// </summary>
        public DateTime CreateTime
        {
            get
            {
                return _createTime;
            }
        }

        /// <summary>
        /// 初始化缓存对象
        /// </summary>
        public LoaclDataCacheObject()
        { }

        /// <summary>
        /// 使用指定键初始化缓存对象
        /// </summary>
        /// <param name="key">指定键</param>
        public LoaclDataCacheObject(string key)
        {
            this._key = key;
        }

        /// <summary>
        /// 使用指定键、值初始化缓存对象
        /// </summary>
        /// <param name="key">指定键</param>
        /// <param name="value">指定值</param>
        public LoaclDataCacheObject(string key, object value)
        {
            this._key = key;
            this.Value = value;
        }

        /// <summary>
        /// 使用指定键、值和更新标志初始化缓存对象
        /// </summary>
        /// <param name="key">指定键</param>
        /// <param name="value">指定值</param>
        /// <param name="keyForUpdate">更新标志</param>
        public LoaclDataCacheObject(string key, object value, string keyForUpdate)
        {
            this._key = key;
            this.Value = value;
            this.KeyForUpdate = keyForUpdate;
        }
    }

    /// <summary>
    /// 本地缓存容器。线程安全。
    /// </summary>
    [Serializable]
    public class LocalDataCacheContainer
    {
        /// <summary>
        /// 本地缓存容器
        /// </summary>
        private Hashtable _container = new Hashtable();
        /// <summary>
        /// 是否保存过
        /// </summary>
        private static bool refreshed = false;

        private static Dictionary<string, IObjectContainer> _safeContainerList = new Dictionary<string, IObjectContainer>();

        private static IObjectContainer GetSafeContainer(string containerName)
        {
            if (_safeContainerList.ContainsKey(containerName))
            {
                return _safeContainerList[containerName];
            }
            else
            {
                lock (objLock)
                {
                    if (_safeContainerList.ContainsKey(containerName))
                    {
                        return _safeContainerList[containerName];
                    }
                    else
                    {
                        Db4objects.Db4o.Config.IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
                        configuration.Common.ActivationDepth = 20;
                        configuration.Common.ObjectClass(typeof(LoaclDataCacheObject)).CascadeOnUpdate(true);
                        //configuration.Common.ObjectClass(typeof(LoaclDataCacheObject)).ObjectField("Key").Indexed(true);
                        //configuration.Common.StringEncoding = StringEncodings.Utf8();
                        IObjectContainer safeContainer = Db4oEmbedded.OpenFile(configuration, LoaclDataCacheManager.LoaclDataCachePath + "\\" + containerName + ".dll");
                        _safeContainerList.Add(containerName, safeContainer);
                        return safeContainer;
                    }
                }
            }
        }


        /// <summary>
        /// 获取该本地缓存容器当前存入的对象数目
        /// </summary>
        public int Count
        {
            get
            {
                int result = GetSafeContainer(_containerName).Query<LoaclDataCacheObject>().Count;
                if (result > 0)
                    return result;
                else
                {
                    return _container.Count;
                }
            }
        }
        /// <summary>
        /// 获取本地缓存容器中已存储的所有的键
        /// </summary>
        public ICollection Keys
        {
            get
            {
                var lst = (from LoaclDataCacheObject f in GetSafeContainer(_containerName)
                           select f.Key).ToList();
                if (lst.Count > 0)
                {
                    return lst;
                }
                else
                {
                    return _container.Keys;
                }
            }
        }
        /// <summary>
        /// 本地缓存容器的名称
        /// </summary>
        private string _containerName = "6DDB8DB2-85FF-41F8-9311-7EF924C389EE";

        static string objLock = "1";
        /// <summary>
        /// 获取或设置一个值，该值标识本地缓存容器的名称.
        /// </summary>
        /// <value>
        /// 名称.
        /// </value>
        public string Name
        {
            get
            {
                return _containerName;
            }
        }

        /// <summary>
        /// 使用默认的本地缓存容器。线程安全。
        /// </summary>
        public LocalDataCacheContainer()
        {
            Refresh();
        }

        /// <summary>
        /// 尚未实现，将使用默认的本地缓存容器。线程安全。
        /// </summary>
        /// <param name="name">本地缓存容器名称.</param>
        public LocalDataCacheContainer(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("name属性无效。");
            }
            _containerName = name;
            Refresh();
        }

        /// <summary>
        /// 初始化默认的本地缓存容器，并且不从文件获取信息。
        /// </summary>
        /// <param name="loadData"></param>
        internal LocalDataCacheContainer(bool loadData)
        {

        }

        /// <summary>
        /// 本地缓存容器。线程安全。
        /// </summary>
        /// <param name="key">要获取或设置其值的键。</param>
        /// <returns></returns>
        public LoaclDataCacheObject this[string key]
        {
            get
            {
                LoaclDataCacheObject obj = (from LoaclDataCacheObject f in GetSafeContainer(_containerName)
                                            where f.Key == key
                                            select f).FirstOrDefault();
                return obj ?? (_container[key] as LoaclDataCacheObject);
            }
            set
            {
                if (string.IsNullOrWhiteSpace(key))
                    throw new ArgumentException("参数为空或Key无效。");
                else
                {
                    LoaclDataCacheObject obj = (from LoaclDataCacheObject f in GetSafeContainer(_containerName)
                                                where f.Key == key
                                                select f).FirstOrDefault();
                    if (obj != null)
                    {
                        obj.KeyForUpdate = value.KeyForUpdate;
                        obj.Key = key;
                        obj.Value = value.Value;
                    }
                    else
                    {
                        if (_container.ContainsKey(key))
                            Update(key, value);
                        else
                        {
                            value.Key = key;
                            Add(value);
                        }
                    }
                    GetSafeContainer(_containerName).Store(obj ?? value);
                    GetSafeContainer(_containerName).Commit();
                }
            }
        }

        /// <summary>
        /// 确定该本地缓存容器中是否包含特定键。
        /// </summary>
        /// <param name="key">要判断的键</param>
        /// <returns></returns>
        public bool ContainsKey(string key)
        {
            LoaclDataCacheObject obj = (from LoaclDataCacheObject f in GetSafeContainer(_containerName)
                                        where f.Key == key
                                        select f).FirstOrDefault();
            return obj == null ? _container.ContainsKey(key) : true;
        }

        /// <summary>
        /// 确定该本地缓存容器中是否包含特定值。
        /// </summary>
        /// <param name="value">要判断的值</param>
        /// <returns></returns>
        public bool ContainsValue(LoaclDataCacheObject value)
        {
            LoaclDataCacheObject obj = (from LoaclDataCacheObject f in GetSafeContainer(_containerName)
                                        where f == value
                                        select f).FirstOrDefault();
            return obj == null ? _container.ContainsValue(value) : true;
        }

        /// <summary>
        /// 向在该本地缓存容器中添加缓存项
        /// </summary>
        /// <param name="item">要添加的项.</param>
        private void Add(LoaclDataCacheObject item)
        {
            int retryCount = 0;
            while (retryCount < 3)
            {
                try
                {
                    retryCount++;
                    LoaclDataCacheObject obj = (from LoaclDataCacheObject f in GetSafeContainer(_containerName)
                                                where f.Key == item.Key
                                                select f).FirstOrDefault();
                    if (obj != null)
                    {
                        obj.Key = item.Key;
                        obj.KeyForUpdate = item.KeyForUpdate;
                        obj.Value = item.Value;
                    }
                    else
                    {
                        Check(item, throwOnNotContains: false);
                        if (_container.ContainsKey(item.Key))
                        {
                            Update(item);
                        }
                        else
                        {
                            Monitor.Enter(objLock);
                            refreshed = true;
                            _container.Add(item.Key, item);
                            Monitor.Exit(objLock);
                            Save();
                        }
                    }
                    GetSafeContainer(_containerName).Store(obj ?? item);
                    GetSafeContainer(_containerName).Commit();
                    break;
                }
                catch
                {
                    if (retryCount > 2)
                        throw;
                    else
                    {
                        Thread.Sleep(500);
                    }
                }
            }
        }

        /// <summary>
        /// 在该本地缓存容器中更新与指定键相符的缓存对象
        /// </summary>
        /// <param name="item"></param>
        public void Update(string keyForUpdate, LoaclDataCacheObject item)
        {
            item.KeyForUpdate = keyForUpdate;
            Update(item);
        }

        /// <summary>
        /// 在该本地缓存容器中更新与指定项具有相同键的缓存对象
        /// </summary>
        /// <param name="item"></param>
        public void Update(LoaclDataCacheObject item)
        {
            int retryCount = 0;
            while (retryCount < 3)
            {
                try
                {
                    retryCount++;
                    LoaclDataCacheObject obj = (from LoaclDataCacheObject f in GetSafeContainer(_containerName)
                                                where f.Key == item.KeyForUpdate
                                                select f).FirstOrDefault();
                    if (obj != null)
                    {
                        obj.Key = item.Key;
                        obj.KeyForUpdate = item.KeyForUpdate;
                        obj.Value = item.Value;
                    }
                    else
                    {
                        try
                        {
                            Check(item, true, false);
                            Monitor.Enter(objLock);
                            refreshed = true;
                            _container.Remove(item.KeyForUpdate);
                            item.KeyForUpdate = item.Key;
                            Add(item);
                            Monitor.Exit(objLock);
                            Save();
                        }
                        catch { }
                    }
                    GetSafeContainer(_containerName).Store(obj ?? item);
                    GetSafeContainer(_containerName).Commit();
                    break;
                }
                catch
                {
                    if (retryCount > 2)
                        throw;
                    else
                    {
                        Thread.Sleep(500);
                    }
                }
            }
        }

        /// <summary>
        /// 在该本地缓存容器中删除与指定项具有相同键的缓存对象
        /// </summary>
        /// <param name="item"></param>
        public void Delete(LoaclDataCacheObject item)
        {
            int retryCount = 0;
            while (retryCount < 3)
            {
                try
                {
                    retryCount++;
                    LoaclDataCacheObject obj = (from LoaclDataCacheObject f in GetSafeContainer(_containerName)
                                                where f.Key == item.Key
                                                select f).FirstOrDefault();
                    if (obj != null)
                    {
                        GetSafeContainer(_containerName).Delete(obj);
                        GetSafeContainer(_containerName).Commit();
                    }
                    try
                    {
                        Check(item);
                        if (!_container.ContainsKey(item.Key))
                        {
                            throw new ArgumentException("指定的Key不存在。");
                        }
                        Monitor.Enter(objLock);
                        refreshed = true;
                        _container.Remove(item.Key);
                        Monitor.Exit(objLock);
                        Save();
                    }
                    catch { }
                    break;
                }
                catch
                {
                    if (retryCount > 2)
                        throw;
                    else
                    {
                        Thread.Sleep(500);
                    }
                }
            }
        }

        /// <summary>
        /// 清空该本地缓存容器
        /// </summary>
        public void Clear()
        {
            int retryCount = 0;
            while (retryCount < 3)
            {
                try
                {
                    retryCount++;
                    var lst = (from LoaclDataCacheObject f in GetSafeContainer(_containerName)
                               select f).ToList();
                    lst.ForEach(item =>
                    {
                        GetSafeContainer(_containerName).Delete(item);
                        GetSafeContainer(_containerName).Commit();
                    });
                    try
                    {
                        Monitor.Enter(objLock);
                        _container.Clear();
                        Monitor.Exit(objLock);
                        Save();
                    }
                    catch { }
                    break;
                }
                catch
                {
                    if (retryCount > 2)
                        throw;
                    else
                    {
                        Thread.Sleep(500);
                    }
                }
            }
        }

        /// <summary>
        /// 验证有效性.
        /// </summary>
        /// <param name="item">参数.</param>
        /// <param name="checkKeyForUpdate">是否检查checkKeyForUpdate的值</param>
        private void Check(LoaclDataCacheObject item, bool checkKeyForUpdate = false, bool throwOnNotContains = false)
        {
            if (item == null || string.IsNullOrWhiteSpace(item.Key))
            {
                throw new ArgumentException("参数为空或Key无效。");
            }
            if (checkKeyForUpdate)
            {
                if (string.IsNullOrWhiteSpace(item.KeyForUpdate))
                {
                    item.KeyForUpdate = item.Key;
                }
                if (string.IsNullOrWhiteSpace(item.KeyForUpdate))
                {
                    throw new ArgumentException("KeyForUpdate值无效。");
                }
                else
                {
                    if (GetSafeContainer(_containerName).Query<LoaclDataCacheObject>().Count > 0)
                    {
                        var obj = (from LoaclDataCacheObject f in GetSafeContainer(_containerName)
                                   where f.Key == item.KeyForUpdate
                                   select f).FirstOrDefault();
                        if (obj == null && throwOnNotContains)
                        {
                            throw new ArgumentException("指定的KeyForUpdate值不存在。");
                        }
                    }
                    else
                    {
                        if (!_container.ContainsKey(item.KeyForUpdate) && throwOnNotContains)
                        {
                            throw new ArgumentException("指定的KeyForUpdate值不存在。");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 通知管理器保存该本地缓存容器.
        /// </summary>
        public void Save()
        {
            if (refreshed)
            {
                ThreadPool.QueueUserWorkItem(SaveAync);
            }
        }

        /// <summary>
        /// 通知管理器刷新该本地缓存容器。
        /// </summary>
        /// <param name="saveBeforeRefresh">刷新前是否保存该容器。如果不保存，可能造成数据丢失。</param>
        public void Refresh(bool saveBeforeRefresh = true)
        {
            if (!File.Exists(LoaclDataCacheManager.LoaclDataCachePath + string.Format(@"\{0}.ldc", this._containerName)))
            {
                return;
            }
            if (saveBeforeRefresh)
                Save();
            try
            {
                LocalDataCacheContainer tempContainer = new LocalDataCacheContainer(false);
                try
                {
                    tempContainer = LoaclDataCacheManager.GetManager(this._containerName).GetLocalDataCacheContainerFromFile(this._containerName);
                }
                catch (Exception ex)
                {
                    Log.CustomWrite(ex.ToString(), "TaskPlatform.Commom--LocalDataCacheContainerException");
                    tempContainer = new LocalDataCacheContainer(false);
                }
                Monitor.Enter(objLock);
                _container = tempContainer._container;
                Monitor.Exit(objLock);
                refreshed = true;
            }
            catch (Exception ex)
            {
                Log.CustomWrite(ex.ToString(), "TaskPlatform.Commom--LocalDataCacheContainerException");
            }
        }

        /// <summary>
        /// 异步保存缓存数据
        /// </summary>
        /// <param name="state"></param>
        private void SaveAync(object state)
        {
            LoaclDataCacheManager.GetManager(this._containerName).SaveForcibly(this);
        }
    }

    /// <summary>
    /// 管理本地数据缓存
    /// </summary>
    internal class LoaclDataCacheManager
    {
        private static string _filePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).Trim(' ', '\\') + @"\LoaclDataCache";
        static object obj = 1;
        static string locker = "locker";

        static Dictionary<string, LoaclDataCacheManager> managerList = new Dictionary<string, LoaclDataCacheManager>();

        public static LoaclDataCacheManager GetManager(string containerName)
        {
            lock (locker)
            {
                if (managerList.ContainsKey(containerName))
                {
                    return managerList[containerName];
                }
                else
                {
                    managerList.Add(containerName, new LoaclDataCacheManager());
                    return managerList[containerName];
                }
            }
        }

        public static string LoaclDataCachePath
        {
            get
            {
                return _filePath;
            }
        }
        /// <summary>
        /// 强制将指定本地缓存容器保存到持久对象。
        /// </summary>
        /// <param name="localDataCacheContainer">需要保存的本地缓存容器</param>
        public void SaveForcibly(LocalDataCacheContainer localDataCacheContainer)
        {
            if (localDataCacheContainer == null || string.IsNullOrWhiteSpace(localDataCacheContainer.Name))
            {
                throw new ArgumentException("localDataCacheContainer参数为null或Name属性无效。");
            }
            CheckDirectory();
            Monitor.Enter(obj);
            try
            {
                using (FileStream fs = new FileStream(_filePath + string.Format(@"\{0}.ldc", localDataCacheContainer.Name), FileMode.OpenOrCreate))
                {
                    if (fs.CanWrite)
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        bf.Serialize(fs, localDataCacheContainer);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.CustomWrite(ex.ToString(), "TaskPlatform.Commom--LocalDataCacheManagerException");
                throw;
            }
            Monitor.Exit(obj);
        }

        /// <summary>
        /// 获取指定名称的本地缓存容器。
        /// </summary>
        /// <param name="name">指定名称</param>
        /// <returns></returns>
        public LocalDataCacheContainer GetLocalDataCacheContainerFromFile(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("name属性无效。");
            }
            CheckDirectory();
            LocalDataCacheContainer localDataCacheContainer = new LocalDataCacheContainer(false);
            Monitor.Enter(obj);
            try
            {
                string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                using (FileStream fs = new FileStream(_filePath + string.Format(@"\{0}.ldc", name), FileMode.OpenOrCreate))
                {
                    if (fs.CanRead)
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        localDataCacheContainer = bf.Deserialize(fs) as LocalDataCacheContainer;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.CustomWrite(ex.ToString(), "TaskPlatform.Commom--LocalDataCacheManagerException");
                throw;
            }
            Monitor.Exit(obj);
            if (localDataCacheContainer == null)
                localDataCacheContainer = new LocalDataCacheContainer(false);
            return localDataCacheContainer;
        }

        /// <summary>
        /// 获取可用的本地缓存容器名称列表。
        /// </summary>
        /// <returns></returns>
        public static string[] GetLoaclDataCacheManagerNameList()
        {
            return Directory.GetFiles(_filePath, "*.ldc");
        }

        /// <summary>
        /// 检查缓存目录
        /// </summary>
        private static void CheckDirectory()
        {
            try
            {
                if (!Directory.Exists(_filePath))
                {
                    Directory.CreateDirectory(_filePath);
                }
            }
            catch (Exception ex)
            {
                Log.CustomWrite(ex.ToString(), "TaskPlatform.Commom--LocalDataCacheManagerException");
            }
        }
    }
}
