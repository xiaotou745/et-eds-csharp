using System;
using System.Collections.Generic;
using TaskPlatform.TaskInterface;
using System.Resources;
using System.Collections;
using System.Reflection;
using System.IO;
using System.Data;
using System.Net.Mail;
using System.Text;
using System.Diagnostics;

namespace OpenFileTaskTemplate
{
    public class MainTask : AbstractTask
    {
        Dictionary<string, string> resources = new Dictionary<string, string>();

        public MainTask()
        {
            Assembly a = Assembly.GetExecutingAssembly();
            string fileName = Path.GetDirectoryName(a.CodeBase.Replace("file:///", "")) + "\\TestOpenFile.resources";
            ResourceReader resourceReader = new ResourceReader(fileName);
            IDictionaryEnumerator enumerator = resourceReader.GetEnumerator();
            while (enumerator.MoveNext())
            {
                try
                {
                    if (resources.ContainsKey(enumerator.Key.ToString()))
                    {
                        resources[enumerator.Key.ToString()] = (enumerator.Value ?? "").ToString();
                    }
                    else
                    {
                        resources.Add(enumerator.Key.ToString(), (enumerator.Value ?? "").ToString());
                    }
                }
                catch { }
            }
        }

        /// <summary>
        /// 获取计划任务友好名称
        /// </summary>
        /// <returns></returns>
        public override string TaskName()
        {
            return resources["DisplayName"];
        }

        /// <summary>
        /// 获取计划任务的描述
        /// </summary>
        /// <returns></returns>
        public override string TaskDescription()
        {
            return resources["DisplayName"] + "(本计划任务为计划任务平台创建)";
        }

        /// <summary>
        /// 执行计划任务
        /// </summary>
        /// <returns></returns>
        public override RunTaskResult RunTask()
        {
            RunTaskResult runResult = new RunTaskResult();
            runResult.Success = false;
            ShowRunningLog("开始执行计划任务");
            try
            {
                string fileName = string.Empty;
                Process.Start(resources["FileName"]);
                runResult.Success = true;
                runResult.Result = "已打开文件。";
                ShowRunningLog(runResult.Result);
            }
            catch (Exception ex)
            {
                ShowRunningLog("啊哦，异常了……");
                runResult.Result = ex.ToString();
            }
            ShowRunningLog("执行结束。");
            return runResult;
        }
    }
}
