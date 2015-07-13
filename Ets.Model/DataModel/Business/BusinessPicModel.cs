using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DataModel.Business
{
    /// <summary>
    /// 商家图片信息 add by pengyi 20150709
    /// </summary>
    public class BusinessPicModel
    {
        /// <summary>
        /// 商家Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 验证照片
        /// </summary>
        public string CheckPicUrl { get; set; }
        /// <summary>
        /// 营业照片
        /// </summary>
        public string BusinessLicensePic { get; set; }
    }
}
