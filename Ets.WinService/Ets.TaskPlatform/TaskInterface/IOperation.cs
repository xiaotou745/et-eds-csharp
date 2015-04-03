using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace TaskPlatform.TaskInterface
{
    /// <summary>
    /// 定义计划任务平台互操作性
    /// </summary>
    public interface IOperation
    {
        /// <summary>
        /// 获取计划任务列表
        /// </summary>
        /// <returns>执行结果</returns>
        DataTable GetTaskList();
        /// <summary>
        /// 立刻执行一次
        /// </summary>
        /// <param name="taskName">计划任务名称</param>
        /// <returns>执行结果</returns>
        RunTaskResult RunOnce(string taskName);
        /// <summary>
        /// 立刻执行一次
        /// </summary>
        /// <param name="taskName">计划任务名称</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>
        /// 执行结果
        /// </returns>
        RunTaskResult RunOnceWithParameters(string taskName, Dictionary<string, string> parameters);
        /// <summary>
        /// 获取计划任务的执行计划
        /// </summary>
        /// <param name="taskName">计划任务名称</param>
        /// <returns>若获取计划任务的执行计划失败，则返回的字符串以"-1"开头，若计划任务尚未配置过执行计划，则返回的字符串以"0"，否则返回以"1"开头的执行计划的XML格式)</returns>
        string GetPlanXML(string taskName);
        /// <summary>
        /// 设置计划任务的执行计划
        /// </summary>
        /// <param name="planXML">执行计划的XML格式</param>
        /// <returns>执行结果</returns>
        string SetPlan(string planXML);
        /// <summary>
        /// 开始计划
        /// </summary>
        /// <param name="taskName">计划任务名称</param>
        /// <returns>执行结果</returns>
        string StartPlan(string taskName);
        /// <summary>
        /// 停用计划
        /// </summary>
        /// <param name="taskName">计划任务名称</param>
        /// <returns>执行结果</returns>
        string StopPlan(string taskName);
        /// <summary>
        /// 卸载计划任务
        /// </summary>
        /// <param name="taskName">计划任务名称</param>
        /// <returns>执行结果</returns>
        string UnloadTask(string taskName);
        /// <summary>
        /// 增量加载计划任务
        /// </summary>
        /// <returns>执行结果</returns>
        string LoadAllTask();
        /// <summary>
        /// 自定义日志写入
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="dateTime">日志时间</param>
        /// <param name="logName">日志名称</param>
        /// <param name="taskName">名称</param>
        /// <returns></returns>
        string CustomWrite(string title, string content, DateTime dateTime, string logName, string taskName);
        /// <summary>
        /// 异步写入执行日志
        /// </summary>
        /// <param name="content">内容</param>
        /// <returns></returns>
        string WriteLog(string taskName, string content);
        /// <summary>
        /// 显示实时运行信息
        /// </summary>
        /// <param name="taskName">计划任务名称</param>
        /// <param name="content">信息内容</param>
        /// <returns></returns>
        string ShowRunningLog(string taskName, string content);
        /// <summary>
        /// 显示并记录实时运行信息
        /// </summary>
        /// <param name="taskName">计划任务名称</param>
        /// <param name="content">信息内容</param>
        /// <returns></returns>
        string ShowWriteRunningLog(string taskName, string content);

        /// <summary>
        /// 获取警报配置信息
        /// </summary>
        /// <param name="taskName">计划任务Key</param>
        /// <returns>警报信息的XML格式</returns>
        string GetAlarmXML(string taskName);

        /// <summary>
        /// 配置警报信息
        /// </summary>
        /// <param name="taskName">计划任务Key</param>
        /// <param name="alarmXML">警报信息的XML格式</param>
        /// <returns></returns>
        string SetAlarm(string taskName, string alarmXML);

        /// <summary>
        /// 获取计划任务的自定义配置
        /// </summary>
        /// <param name="taskName">计划任务Key</param>
        /// <returns></returns>
        Dictionary<string, string> GetCustomConfig(string taskName);

        /// <summary>
        /// 提交计划任务自定义配置项
        /// </summary>
        /// <param name="taskName">计划任务Key</param>
        /// <param name="customConfig">自定义配置项</param>
        /// <returns></returns>
        string SubmitCustomConfig(string taskName, Dictionary<string, string> customConfig);

        /// <summary>
        /// 获取运行时信息
        /// </summary>
        /// <param name="runID">执行ID</param>
        /// <returns></returns>
        string GetRunningInfo(string runID);

        /// <summary>
        /// 从Web端执行一次
        /// </summary>
        /// <param name="taskName">计划任务Key</param>
        /// <param name="runID">执行ID</param>
        /// <returns></returns>
        RunTaskResult RunOnceFromWeb(string taskName, string runID);
    }
}
