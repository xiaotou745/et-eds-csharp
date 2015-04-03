using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaskPlatform.TaskInterface
{
    /// <summary>
    /// 计划任务接口
    /// </summary>
    [Obsolete("已过时，请改用AbstractTask。")]
    public interface ITask
    {
        /// <summary>
        /// 获取计划任务的唯一标识符
        /// </summary>
        string TaskKey();

        /// <summary>
        /// 获取计划任务友好名称
        /// </summary>
        string TaskName();

        /// <summary>
        /// 获取计划任务的描述
        /// </summary>
        string TaskDescription();

        /// <summary>
        /// 初始化计划任务时将被执行。平台将通过此函数通知所以计划任务，以便获取平台的相关配置信息。
        /// </summary>
        /// <param name="platformAppSettings"></param>
        /// <param name="platformConnectionStrings"></param>
        void InitTask(Dictionary<string, string> platformAppSettings, Dictionary<string, string> platformConnectionStrings);

        /// <summary>
        /// 返回配置项，以供平台统一配置。该列表将可以通过平台提供的配置编辑器编辑，编辑后由平台调用DownloadConfig函数回发给计划任务。
        /// </summary>
        Dictionary<string, string> UploadConfig();

        /// <summary>
        /// 接收平台发过来的配置项。
        /// </summary>
        void DownloadConfig(Dictionary<string, string> configs);

        /// <summary>
        /// 执行计划任务
        /// </summary>
        RunTaskResult RunTask();
    }
}
