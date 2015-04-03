using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Resources;

namespace TaskPlatform.Commom
{
    public class TaskBuilder
    {
        private string path = string.Empty;
        public void Build(SystemTask systemTask)
        {
            Build(systemTask, false);
        }

        public void ReBuild(SystemTask systemTask)
        {
            Build(systemTask, true);
        }

        private void Build(SystemTask systemTask, bool rebuild)
        {
            string code = PlatformForm.GetStringTemplate(systemTask.CreateTaskTemplateName).Replace("<%TaskSystemName%>", systemTask.SystemName);
            string taskSystemName = string.Format("Sys-{0}", systemTask.SystemName);
            CheckSystemTasksPath(taskSystemName);
            taskSystemName = string.Format("{0}\\{1}.dll", path, taskSystemName);
            CheckSystemTasksDLLPath(taskSystemName, rebuild);
            ResourcesBuilder(systemTask);
            AssemblyBuilder(taskSystemName, code, PlatformForm.GetStringTemplate("AppConfig"));
        }

        private void CheckSystemTasksPath(string taskSystemName)
        {
            path = string.Format("{0}\\{1}", PlatformForm.SystemTaskFilesPath, taskSystemName);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        private void CheckSystemTasksDLLPath(string taskSystemName, bool rebuild)
        {
            if (!rebuild && File.Exists(taskSystemName))
            {
                throw new Exception("该任务可能已存在，请使用其他的系统名称。");
            }
        }

        public void ResourcesBuilder(SystemTask systemTask)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                path = string.Format("{0}\\Sys-{1}", PlatformForm.SystemTaskFilesPath, systemTask.SystemName);
            }
            else if (string.IsNullOrWhiteSpace(systemTask.DBHelperName))
            {
                systemTask.DBHelperName = "SQLServerDBHelper";
            }
            ResourceWriter rw = new ResourceWriter(path + "\\" + systemTask.SystemName + ".resources");
            rw.AddResource("DisplayName", systemTask.DisplayName);
            rw.AddResource("DBHelperName", systemTask.DBHelperName);
            rw.AddResource("ConnectionString", systemTask.ConnectionString);
            rw.AddResource("SQL", systemTask.SQL);
            rw.AddResource("LuaScript", systemTask.LuaScript);
            rw.AddResource("FileName", systemTask.FileName ?? "");
            rw.AddResource("CreateTaskTemplateName", systemTask.CreateTaskTemplateName ?? "");
            rw.Generate();
            rw.Close();
        }

        private void AssemblyBuilder(string fileName, string code, string config)
        {
            CompilerParameters parameters = new CompilerParameters();
            parameters.IncludeDebugInformation = false;
            parameters.GenerateExecutable = false;
            parameters.GenerateInMemory = false;
            parameters.OutputAssembly = fileName;
            parameters.ReferencedAssemblies.Add("System.dll");
            parameters.ReferencedAssemblies.Add("System.Data.dll");
            parameters.ReferencedAssemblies.Add("System.XML.dll");
            parameters.ReferencedAssemblies.Add("TaskPlatform.TaskInterface.dll");
            parameters.ReferencedAssemblies.Add("LuaInterface.dll");
            CSharpCodeProvider compiler = new CSharpCodeProvider();
            CompilerResults compilerResults = compiler.CompileAssemblyFromSource(parameters, code, GetVersionCode());
            List<CompilerError> errors = compilerResults.Errors.Cast<CompilerError>().ToList();
            if (errors.FirstOrDefault(item => { return !item.IsWarning; }) == null)
            {
                File.WriteAllText(parameters.OutputAssembly + ".config", config);
            }
            else
            {
                try
                {
                    Directory.Delete(path, true);
                }
                catch { }
            }
        }

        private string GetVersionCode(string title = "计划任务")
        {
            StringBuilder sbVersionCode = new StringBuilder();
            sbVersionCode.AppendLine("using System.Reflection;");
            sbVersionCode.AppendLine("using System.Runtime.CompilerServices;");
            sbVersionCode.AppendLine("using System.Runtime.InteropServices;");
            sbVersionCode.AppendLine(string.Format("[assembly: AssemblyTitle(\"{0}\")]", title));
            sbVersionCode.AppendLine(string.Format("[assembly: AssemblyDescription(\"{0}\")]", ""));
            sbVersionCode.AppendLine(string.Format("[assembly: AssemblyConfiguration(\"{0}\")]", ""));
            sbVersionCode.AppendLine(string.Format("[assembly: AssemblyCompany(\"{0}\")]", "wangxudan"));
            sbVersionCode.AppendLine(string.Format("[assembly: AssemblyProduct(\"{0}\")]", "计划任务平台创建的计划任务"));
            sbVersionCode.AppendLine(string.Format("[assembly: AssemblyCopyright(\"{0}\")]", "Copyright © wangxudan 2014"));
            sbVersionCode.AppendLine(string.Format("[assembly: AssemblyTrademark(\"{0}\")]", ""));
            sbVersionCode.AppendLine(string.Format("[assembly: AssemblyCulture(\"{0}\")]", ""));
            sbVersionCode.AppendLine(string.Format("[assembly: Guid(\"{0}\")]", Guid.NewGuid().ToString()));
            sbVersionCode.AppendLine(string.Format("[assembly: AssemblyVersion(\"{0}\")]", "1.0.0.0"));
            sbVersionCode.AppendLine(string.Format("[assembly: AssemblyFileVersion(\"{0}\")]", "1.0.0.0"));
            return sbVersionCode.ToString();
        }
    }
}
