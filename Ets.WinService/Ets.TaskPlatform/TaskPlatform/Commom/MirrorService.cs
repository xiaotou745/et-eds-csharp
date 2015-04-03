using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaskPlatform.Commom
{
    public class MirrorService
    {
        /// <summary>
        /// 镜像服务缓存
        /// </summary>
        private static LocalDataCacheContainer MirrorServiceCache = null;
        private static string _cacheContainerName = "1DDC82EF-7ADB-4CF6-9EE1-F325CCAED0FD";
        static MirrorService()
        {
            MirrorServiceCache = new LocalDataCacheContainer(_cacheContainerName);
        }

        /// <summary>
        /// 检查指定镜像是否已存在
        /// </summary>
        /// <param name="taskName">任务名称</param>
        /// <returns></returns>
        internal static List<string> GetTaskMirror(string taskName)
        {
            List<string> result = new List<string>();
            if (!MirrorServiceCache.ContainsKey(taskName))
            {
                MirrorServiceCache.Update(new LoaclDataCacheObject(taskName, result, taskName));
            }
            else
            {
                LoaclDataCacheObject item = MirrorServiceCache[taskName];
                result = item.Value as List<string>;
                if (result == null)
                {
                    result = new List<string>();
                }
            }
            return result;
        }

        /// <summary>
        /// 添加任务镜像
        /// </summary>
        /// <param name="taskName">任务名称</param>
        /// <param name="mirrorName">镜像名称</param>
        internal static void AddTaskMirror(string taskName, string mirrorName)
        {
            List<string> mirrorList = GetTaskMirror(taskName);
            if (mirrorList.Contains(mirrorName))
            {
                throw new ArgumentException(string.Format("该任务已存在镜像[{0}]。", mirrorName));
            }
            else
            {
                mirrorList.Add(mirrorName);
                MirrorServiceCache.Update(new LoaclDataCacheObject(taskName, mirrorList, taskName));
            }
        }

        /// <summary>
        /// 添加任务镜像
        /// </summary>
        /// <param name="taskName">任务名称</param>
        /// <param name="mirrorName">镜像名称</param>
        /// <param name="throwOnNotExists">如果镜像不存在是否抛出异常。</param>
        /// <exception cref="System.ArgumentException">如果镜像不存在则抛出异常。</exception>
        internal static void RemoveTaskMirror(string taskName, string mirrorName, bool throwOnNotExists = false)
        {
            List<string> mirrorList = GetTaskMirror(taskName);
            if (mirrorList.Count < 1)
            {
                if (throwOnNotExists)
                {
                    throw new ArgumentException(string.Format("该任务不存在镜像[{0}]。", mirrorName));
                }
            }
            else
            {
                mirrorList.Remove(mirrorName);
                MirrorServiceCache.Update(new LoaclDataCacheObject(taskName, mirrorList, taskName));
            }
        }

        /// <summary>
        /// 添加任务镜像
        /// </summary>
        /// <param name="taskName">任务名称</param>
        internal static void ClearTaskMirror(string taskName)
        {
            MirrorServiceCache.Update(new LoaclDataCacheObject(taskName, new List<string>(), taskName));
        }
    }
}
