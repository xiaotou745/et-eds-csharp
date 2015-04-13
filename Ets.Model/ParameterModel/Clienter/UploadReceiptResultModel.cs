using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Clienter
{
    public class UploadReceiptResultModel
    {
        /// <summary>
        /// 订单Id
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// 小票的图片地址
        /// </summary>
        public List<string> ImagePath { get; set; }
        /// <summary>
        /// 该订单总共需要上传的小票张数
        /// </summary>
        public int NeedUploadCount { get; set; }

        /// <summary>
        /// 已经上传的小票张数
        /// </summary>
        public int HadUploadCount { get; set; }
    }
}
