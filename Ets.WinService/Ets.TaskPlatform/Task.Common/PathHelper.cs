using System;
using System.IO;

namespace Task.Common
{
    public class PathHelper
    {
        /// <summary>
        ///     获取文件路径，按主目录，文件名，文件扩展名来创建路径。并返回处理好的全路径。
        /// </summary>
        /// <param name="mainPath"></param>
        /// <param name="fileName"></param>
        /// <param name="ext"></param>
        /// <returns></returns>
        public static string CheckPath(string mainPath, string fileName, string ext = ".xls")
        {
            string date = DateTime.Now.ToString("yyyyMMdd");
            string path = Path.Combine(mainPath, date);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return Path.Combine(path, fileName + ext);
        }
    }
}