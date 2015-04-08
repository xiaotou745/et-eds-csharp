using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperManCore
{
    public class CustomerIconUploader : FixedDimensionUploader
    {
        public static readonly CustomerIconUploader Instance = new CustomerIconUploader();
        private CustomerIconUploader() { }

        public override int Width
        {
            get { return 150; }
        }

        public override int Height
        {
            get { return 150; }
        }

        /// <summary>
        /// 50k
        /// </summary>
        public override int MaxBytesLength
        {
            get { return 50 * 1024; }
        }

        public override string[] AllowedImageTypes
        {
            get { return new string[] { ".jpg" }; }
        }

        public override string FolderName
        {
            get { return ConfigSettings.Instance.FileUploadFolderNameCustomerIcon; }
        }
    }
}
