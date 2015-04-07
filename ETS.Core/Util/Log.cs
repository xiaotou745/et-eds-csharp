using System.Configuration;
using Common.Logging;
using System.IO;
using System.Text;

namespace ETS.Util
{
    public class Log
    {
        private string LogName { get; set; }
        private static Log _log;
        private static readonly object lockObject = new object();
        protected Log()
        {
            LogName = "DZLogger";
            this.Logger = LogManager.GetLogger(LogName);
        }
        protected Log(string logName)
        {
            LogName = logName;
            this.Logger = LogManager.GetLogger(LogName);
        }
        public ILog Logger { get; set; }
        public static ILog GetLogger()
        {
            lock (lockObject)
            {
                if (_log == null)
                {
                    if (_log == null)
                    {
                        var sitename = ConfigurationManager.AppSettings["SiteName"];
                        if (!string.IsNullOrEmpty(sitename))
                        {
                            _log = new Log(sitename);
                        }
                        else
                        {
                            _log = new Log();
                        }
                    }
                }
            }
            return _log.Logger;
        }
        public static ILog GetLogger(string LogName)
        {
            lock (lockObject)
            {
                if (_log == null)
                {
                    _log = new Log(LogName);
                }
            }
            return _log.Logger;
        }



        #region 将文本写成任意扩展名的文件
        public static void WriteTextToFile(string Text, string FileFullPathAndName)
        {
            WriteTextToFile(Text, FileFullPathAndName, false, "gb2312");
        }
        public static void WriteTextToFile(string Text, string FileFullPathAndName, bool IsAppend)
        {
            WriteTextToFile(Text, FileFullPathAndName, IsAppend, "gb2312");
        }
        public static void WriteTextToFile(string Text, string FileFullPathAndName, bool IsAppend, string encoding)
        {
            if (Text == "" || Text == null)
            {
                return;
            }
            string DirName = GetDirByFileName(FileFullPathAndName);
            if (!Directory.Exists(DirName))
            {
                Directory.CreateDirectory(DirName);
            }
            if (!File.Exists(FileFullPathAndName))
            {
                using (File.Create(FileFullPathAndName)) { };
            }
            Encoding gbEncode = Encoding.GetEncoding(encoding);
            using (StreamWriter swFromFile = new StreamWriter(FileFullPathAndName, IsAppend, gbEncode))
            {
                swFromFile.Write(Text);
                swFromFile.Close();
            }
        }
        #endregion

        #region 根据文件名,取得目录名
        public static string GetDirByFileName(string FileName)
        {
            int position = FileName.LastIndexOf(@"\");
            if (position > 0)
            {
                return FileName.Substring(0, position);
            }
            else
            {
                return "";
            }
        }

        #endregion
    }
}
