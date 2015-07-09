using ETS.Enums;
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
        public string PicHost
        {
            get { return ConfigSettings.Instance.PicHost; }
        }
        private string businessPicPhysicalPath;
        /// <summary>
        /// 商户图片物理路径  add by pengyi 20150709
        /// </summary>
        public string BusinessPicPhysicalPath
        {
            get
            {
                if (businessPicPhysicalPath == null)
                {
                    businessPicPhysicalPath = Path.Combine(PhysicalPath, ConfigSettings.Instance.FileUploadFolderNameBusiness);
                    if (!System.IO.Directory.Exists(businessPicPhysicalPath))
                    {
                        System.IO.Directory.CreateDirectory(businessPicPhysicalPath);
                    }
                }
                return businessPicPhysicalPath;
            }
        }
        private string clienterPicPhysicalPath;
        /// <summary>
        /// 骑士图片物理路径  add by pengyi 20150709
        /// </summary>
        public string ClienterPicPhysicalPath
        {
            get
            {
                if (clienterPicPhysicalPath == null)
                {
                    clienterPicPhysicalPath = Path.Combine(PhysicalPath, ConfigSettings.Instance.FileUploadFolderNameClienter);
                    if (!System.IO.Directory.Exists(clienterPicPhysicalPath))
                    {
                        System.IO.Directory.CreateDirectory(clienterPicPhysicalPath);
                    }
                }
                return clienterPicPhysicalPath;
            }
        }

        public string GetPhysicalPath(int orderId,UserType userType)
        {
            //如果orderId为0,则为图片为商家或骑士的验证图片,需要保存在CustomerIcon/Business(或Clients)/中,否则为小票,放在CustomerIcon/中
            if (orderId > 0)
            {
                return PhysicalPath;
            }
            else
            {
                if (userType == UserType.Business)
                {
                    return BusinessPicPhysicalPath;
                }
                else
                {
                    return ClienterPicPhysicalPath;
                }
            }
        }
    }
}
