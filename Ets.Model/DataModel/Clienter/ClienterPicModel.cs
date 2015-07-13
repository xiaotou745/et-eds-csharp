using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DataModel.Clienter
{
    /// <summary>
    /// 骑士图片信息 add by pengyi 20150709
    /// </summary>
    public class ClienterPicModel
    {
        /// <summary>
        /// 骑士Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 验证照片
        /// </summary>
        public string PicUrl { get; set; }
        /// <summary>
        /// 手持验证照片
        /// </summary>
        public string PicWithHandUrl { get; set; }
    }
}
