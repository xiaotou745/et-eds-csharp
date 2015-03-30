using ETS.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Extension;
namespace Ets.Model.ParameterModel.Clienter
{
    public class CustomerIconUploader
    {
        public static CustomerIconUploader Instance = new CustomerIconUploader();
        public  int Width
        {
            get { return 150; }
        }

        public  int Height
        {
            get { return 150; }
        }

        /// <summary>
        /// 50k
        /// </summary>
        public  int MaxBytesLength
        {
            get { return 50 * 1024; }
        }
        /// <summary>
        /// 文件名称
        /// </summary>
        public string FolderName 
        { 
            get { return ConfigSettings.Instance.FileUploadFolderNameCustomerIcon; }
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
