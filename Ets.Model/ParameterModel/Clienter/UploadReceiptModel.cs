using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Clienter
{
    /// <summary>
    /// 上传图片Model
    /// </summary>
    public class UploadReceiptModel
    {
        /// <summary>
        /// 订单Id 订单表主键
        /// </summary>
        public int OrderId { get; set; }
        
        /// <summary>
        /// 总共需要上传的小票数量
        /// </summary>
        public int NeedUploadCount { get; set; }
        /// <summary>
        /// 图片名称，带目录结构，年月日时 例如 ：  /2014/01/02/12/订单Id/tupian.jpg
        /// </summary>
        public string ReceiptPic { get; set; }
        /// <summary>
        /// 已经上传的小票张数
        /// </summary>
        public int HadUploadCount { get; set; }
    }
}
