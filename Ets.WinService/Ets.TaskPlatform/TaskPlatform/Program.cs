using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TaskPlatform.PlatformLog;
using System.Configuration;
using System.Threading;
using System.Security.Permissions;

namespace TaskPlatform
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            DevExpress.UserSkins.OfficeSkins.Register();
            DevExpress.Skins.SkinManager.EnableFormSkins();
            DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle("Black");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            AppDomain.CurrentDomain.FirstChanceException += new EventHandler<System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs>(CurrentDomain_FirstChanceException);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            PlatformForm pff = new PlatformForm();
            pff.ConsoleArgs = args;
            Application.Run(pff);
        }

        static void CurrentDomain_FirstChanceException(object sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e)
        {
            // 排除无需理会的异常
            if (e.Exception.ToString().Contains("DevExpress.XtraEditors.v10.2.resources"))
                return;
            // 当异常发生后，第一时间捕获异常并记入日志
            Log.CustomWrite(e.Exception.ToString(), "TaskPlatform.Program--AllException");
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Log.CustomWrite(e.Exception.ToString(), "TaskPlatform.Program--ApplicationThreadException");
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Log.CustomWrite(e.ExceptionObject.ToString(), "TaskPlatform.Program--CurrentDomainUnhandledException");
            if (e.IsTerminating)
            { 
                // 程序将终止，若何？
            }
        }
    }
}
