using System;
using System.Data;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Collections.Generic;

namespace TaskPlatform.TaskInterface
{
    /// <summary>
    /// 连接器
    /// </summary>
    public class Mouthpiece : MarshalByRefObject, IOperation
    {
        #region 连接控制台

        private IOperation operation = null;
        /// <summary>
        /// 连接计划任务平台
        /// </summary>
        private void Connection()
        {
            try
            {
                operation = AppDomain.CurrentDomain.GetData("TaskPlatformController") as IOperation;
            }
            catch (Exception ex)
            {
                operation = null;
                throw new Exception("连接计划任务平台时出错。请参考：" + ex);
            }
            if (operation == null)
            {
                throw new Exception("连接计划任务平台时出错。");
            }
        }
        #endregion

        #region 操作接口

        /// <summary>
        /// 获取计划任务列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetTaskList()
        {
            Connection();
            return operation.GetTaskList();
        }

        /// <summary>
        /// 立刻执行一次
        /// </summary>
        /// <param name="taskName">计划任务名称</param>
        /// <returns></returns>
        public RunTaskResult RunOnce(string taskName)
        {
            Connection();
            return operation.RunOnce(taskName);
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
            Connection();
            return operation.RunOnceWithParameters(taskName, parameters);
        }

        /// <summary>
        /// 开始计划
        /// </summary>
        /// <param name="taskName">计划任务名称</param>
        /// <returns></returns>
        public string StartPlan(string taskName)
        {
            Connection();
            return operation.StartPlan(taskName);
        }

        /// <summary>
        /// 停用计划
        /// </summary>
        /// <param name="taskName">计划任务名称</param>
        /// <returns></returns>
        public string StopPlan(string taskName)
        {
            Connection();
            return operation.StopPlan(taskName);
        }

        /// <summary>
        /// 卸载计划任务
        /// </summary>
        /// <param name="taskName">计划任务名称</param>
        /// <returns></returns>
        public string UnloadTask(string taskName)
        {
            Connection();
            return operation.UnloadTask(taskName);
        }

        /// <summary>
        /// 增量加载计划任务
        /// </summary>
        /// <returns></returns>
        public string LoadAllTask()
        {
            Connection();
            return operation.LoadAllTask();
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
            Connection();
            return operation.GetPlanXML(taskName);
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
            Connection();
            return operation.SetPlan(planXML);
        }

        /// <summary>
        /// 自定义日志写入
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="dateTime">日志时间</param>
        /// <param name="logName">日志名称</param>
        /// <returns></returns>
        public string CustomWrite(string title, string content, DateTime dateTime, string logName, string taskName)
        {
            Connection();
            return operation.CustomWrite(title, content, dateTime, logName, taskName);
        }

        /// <summary>
        /// 异步写入执行日志
        /// </summary>
        /// <param name="taskName"></param>
        /// <param name="content">内容</param>
        /// <returns></returns>
        public string WriteLog(string taskName, string content)
        {
            Connection();
            return operation.WriteLog(taskName, content);
        }

        /// <summary>
        /// 显示实时运行信息
        /// </summary>
        /// <param name="taskName">计划任务名称</param>
        /// <param name="content">信息内容</param>
        /// <returns></returns>
        public string ShowRunningLog(string taskName, string content)
        {
            Connection();
            return operation.ShowRunningLog(taskName, content);
        }

        /// <summary>
        /// 显示并记录实时运行信息
        /// </summary>
        /// <param name="taskName">计划任务名称</param>
        /// <param name="content">信息内容</param>
        /// <returns></returns>
        public string ShowWriteRunningLog(string taskName, string content)
        {
            Connection();
            return operation.ShowWriteRunningLog(taskName, content);
        }

        /// <summary>
        /// 获取警报配置信息
        /// </summary>
        /// <param name="taskName">计划任务Key</param>
        /// <returns>
        /// 警报信息的XML格式
        /// </returns>
        public string GetAlarmXML(string taskName)
        {
            Connection();
            return operation.GetAlarmXML(taskName);
        }

        /// <summary>
        /// 配置警报信息
        /// </summary>
        /// <param name="taskName">计划任务Key</param>
        /// <param name="alarmXML">警报信息的XML格式</param>
        /// <returns></returns>
        public string SetAlarm(string taskName, string alarmXML)
        {
            Connection();
            return operation.SetAlarm(taskName, alarmXML);
        }

        #endregion


        public Dictionary<string, string> GetCustomConfig(string taskName)
        {
            Connection();
            return operation.GetCustomConfig(taskName);
        }

        public string SubmitCustomConfig(string taskName, Dictionary<string, string> customConfig)
        {
            Connection();
            return operation.SubmitCustomConfig(taskName, customConfig);
        }

        public string GetRunningInfo(string runID)
        {
            Connection();
            return operation.GetRunningInfo(runID);
        }

        public RunTaskResult RunOnceFromWeb(string taskName, string runID)
        {
            Connection();
            return operation.RunOnceFromWeb(taskName, runID);
        }
    }
}
