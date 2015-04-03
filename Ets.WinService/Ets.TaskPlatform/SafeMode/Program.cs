using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Reflection;
using System.IO;

namespace TaskPlatform.SafeMode
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args != null && args.Length > 0 && args.Contains("/cleanoldfile"))
            {
                // 清除所有计划任务的TaskPlatform.TaskInterface.dll文件
                string path = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase).Replace("file:\\", "") + "\\Tasks";
                CleanPath(path);
            }
            else
            {
                try
                {
                    // 以安全模式启动计划任务执行平台。
                    Process p = new Process();
                    p.StartInfo = new ProcessStartInfo("TaskPlatform.exe", "/safe");
                    p.StartInfo.CreateNoWindow = true;
                    p.Start();
                }
                catch (Exception e)
                {
                    Console.Write(e.ToString());
                    Console.Read();
                }
            }
        }

        static void CleanPath(string path)
        {
            string fileName = path + "\\TaskPlatform.TaskInterface.dll";
            Console.WriteLine(fileName);
            File.Delete(fileName);
            string fileName2 = path + "\\MySQL.Data.dll";
            Console.WriteLine(fileName2);
            File.Delete(fileName2);
            Array.ForEach(Directory.GetDirectories(path),
                item =>
                {
                    if (!item.ToLower().Contains("\\logs\\2"))
                    {
                        CleanPath(item);
                    }
                });
        }
    }
}
