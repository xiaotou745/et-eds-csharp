using System;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using System.Diagnostics;

namespace Letao.Util
{
    public class FilesHelper
    {

        /// <summary>
        /// 获取文件夹下全部文件-包括子文件夹下的文件
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        public static List<FileInfo> GetFiles(string folder)
        {
            List<FileInfo> list = new List<FileInfo>();
            try
            {

                DirectoryInfo di = new DirectoryInfo(folder);
                DirectoryInfo[] dirs = di.GetDirectories();
                FileInfo[] fis = di.GetFiles();
                list.AddRange(fis);
                foreach (DirectoryInfo d in dirs)
                {
                    List<FileInfo> files = GetFiles(d.FullName);
                    list.AddRange(files);
                }

            }
            catch
            {

            }
            return list;
        }
        /// <summary>
        /// 获取文件夹下的文件的Hashtable-便于查询比对文件
        /// </summary>
        /// <param name="folder">根文件夹</param>
        /// <param name="list">文件集合</param>
        /// <returns></returns>
        public static Hashtable GetHashtable(string folder, List<FileInfo> list)
        {
            Hashtable hashTab = new Hashtable();
            foreach (FileInfo fi in list)
            {
                string path = fi.FullName.Replace(folder, "");
                if (!string.IsNullOrEmpty(path) && !hashTab.Contains(path))
                {
                    hashTab.Add(path, fi);
                }
            }
            return hashTab;
        }
        /// <summary>
        /// 获取文件夹下的文件的Hashtable-便于查询比对文件
        /// </summary>
        /// <param name="folder">根文件夹</param>
        /// <param name="list">文件集合</param>
        /// <returns></returns>
        public static Hashtable GetHashtable(string folder)
        {
            List<FileInfo> list = GetFiles(folder);
            return GetHashtable(folder, list);
        }
        public static List<string> GetExtensions(List<FileInfo> list)
        {
            List<string> eList = new List<string>();
            foreach (FileInfo f in list)
            {
                string exetension = f.Extension.ToLower();
                if (!eList.Contains(exetension))
                {
                    eList.Add(exetension);
                }
            }
            return eList;
        }
        /// <summary>
        /// 得到文件夹下的全部扩展名称
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        public static List<string> GetExtensions(string folder)
        {
            List<FileInfo> list = GetFiles(folder);
            return GetExtensions(list);
        }

        /// <summary>
        /// 删除一个文件夹全部文件
        /// </summary>
        /// <param name="folder"></param>
        public static void DeleteFiles(string folder)
        {
            List<FileInfo> list = GetFiles(folder);
            foreach (FileInfo fi in list)
            {
                if (fi.IsReadOnly)
                {
                    fi.Attributes = FileAttributes.Normal;
                }
                fi.Delete();
            }
        }
        /// <summary>
        /// 删除一个文件，返回true表示成功，false表示失败，为简化编程，不区分失败原因
        /// 如果需要详细的失败原因，自己写另外的函数吧
        /// </summary>
        /// <param name="file_path">文件路径</param>
        public static bool DeleteFile(string file_path)
        {
            try
            {
                System.IO.File.Delete(file_path);
            }
            catch
            {
                return false;
            }

            return true;
        }
        public static void CreateFileDir(string path)
        {
            string dirName = Path.GetDirectoryName(path);
            CreateDir(dirName);
        }

        public static void CreateDir(string dirPath)
        {
            if (!Directory.Exists(dirPath))
            {
                string dirName = Path.GetDirectoryName(dirPath);
                CreateDir(dirName);
                Directory.CreateDirectory(dirPath);
            }
        }
        public static bool Exists(string path)
        {
            return File.Exists(path);
        }
        public static bool FolderExists(string path)
        {
            return Directory.Exists(path);
        }
        public static void CopyFile(FileInfo fi, string path)
        {
            if (File.Exists(path))
            {
                FileInfo f = new FileInfo(path);
                if (f.IsReadOnly)
                {
                    f.Attributes = FileAttributes.Normal;
                }
                f.Delete();
            }
            else
            {
                CreateFileDir(path);
            }
            fi.CopyTo(path, false);
        }
        public static DateTime GetLastWriteTime(string filePath)
        {
            FileInfo fi = new FileInfo(filePath);
            return fi.LastWriteTime;
        }
        public static string GetVersion(string filePath)
        {
            string version = "1.0.0.0";
            try
            {
                string ex = Path.GetExtension(filePath).ToLower();
                if (ex.ToLower() == ".dll" || ex.ToLower() == ".exe")
                {
                    FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(filePath);
                    int major = fvi.FileMajorPart;
                    int minor = fvi.FileMinorPart;
                    int build = fvi.FileBuildPart;
                    int revision = fvi.FilePrivatePart;
                    version = new Version(major, minor, build, revision).ToString();
                }
            }
            catch
            {
            }
            return version;
        }
        public static void CutFiles(string source, string target)
        {
            CopyFiles(source, target);
            DeleteFiles(source);
        }
        /// <summary>
        /// 从一个文件夹下拷贝文件到另外一个文件夹
        /// </summary>
        /// <param name="source">原文件夹</param>
        /// <param name="target">目标文件夹</param>
        /// <param name="exclueds">排除的后缀列表</param>
        /// <param name="allOverWrite">是否全部覆盖</param>
        public static void CopyFiles(string source, string target)
        {
            List<FileInfo> list = GetFiles(source);
            foreach (FileInfo fi in list)
            {
                string extension = fi.Extension.ToLower();
               
                string relativePath = fi.FullName.Replace(source, "");
               
                    string path = Path.Combine(target, relativePath.Trim('\\'));
                    CopyFile(fi, path);
                }
            
        }
        public static void CopyFile(string source, string target)
        {
            string fullpath = target+"\\" + Path.GetFileName(source);
            if (File.Exists(fullpath))
            {
                FileInfo f = new FileInfo(fullpath);
                if (f.IsReadOnly)
                {
                    f.Attributes = FileAttributes.Normal;
                }
                f.Delete();
            }
            else
            {
                CreateFileDir(target+"\\");
            }
            FileInfo fi = new FileInfo(source);
            fi.CopyTo(fullpath, false);
        }
    }
}
