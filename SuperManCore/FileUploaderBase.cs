using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace SuperManCore
{
    /// <summary>
    /// 固定尺寸的Uploader
    /// </summary>
    public abstract class FileUploaderBase : IFileUploader
    {
        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="stream"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        public abstract bool Validate(string fileName, Stream stream, out string reason);

        public abstract int MaxBytesLength { get; }
        /// <summary>
        /// 文件类型
        /// </summary>
        public abstract string[] AllowedImageTypes { get; }
        /// <summary>
        /// 文件名称
        /// </summary>
        public abstract string FolderName { get; }

        /// <summary>
        /// 临时文件相对路径
        /// </summary>
        public string TempRelativePath
        {
            get { return Path.Combine("/", Path.GetFileName(ConfigSettings.Instance.FileUploadPath), Path.GetFileName(this.TempPhysicalPath)).ToForwardSlashPath(); }
        }

        private string tempPhysicalPath;
        /// <summary>
        /// 临时文件物理路径
        /// </summary>
        public virtual string TempPhysicalPath
        {
            get
            {
                if (tempPhysicalPath == null)
                {
                    tempPhysicalPath = Path.Combine(ConfigSettings.Instance.FileUploadPath, ConfigSettings.Instance.FileUploadFolderNameTemp);
                    if (!System.IO.Directory.Exists(tempPhysicalPath))
                    {
                        System.IO.Directory.CreateDirectory(tempPhysicalPath);
                    }
                }
                return tempPhysicalPath;
            }
        }

        private string physicalPath;
        /// <summary>
        /// 文件物理路径
        /// </summary>
        public string PhysicalPath
        {
            get
            {
                if (physicalPath == null)
                {
                    physicalPath = Path.Combine(ConfigSettings.Instance.FileUploadPath, this.FolderName);
                    if (!System.IO.Directory.Exists(physicalPath))
                    {
                        System.IO.Directory.CreateDirectory(physicalPath);
                    }
                }
                return physicalPath;
            }
        }

        /// <summary>
        /// 文件相对路径
        /// </summary>
        public string RelativePath
        {
            get { return Path.Combine("/", Path.GetFileName(ConfigSettings.Instance.FileUploadPath), this.FolderName).ToForwardSlashPath(); }
        }

    }
}
