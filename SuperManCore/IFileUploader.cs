using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperManCore
{
    public interface IFileUploader
    {
        string[] AllowedImageTypes { get; }

        bool Validate(string fileName, Stream stream, out string reason);

        string FolderName { get; }

        string TempPhysicalPath { get; }

        string TempRelativePath { get; }

        /// <summary>
        /// 文件物理路径
        /// </summary>
        string PhysicalPath { get; }
        /// <summary>
        /// 文件相对路径(网站显示时使用)
        /// </summary>
        string RelativePath { get; }
    }
}
