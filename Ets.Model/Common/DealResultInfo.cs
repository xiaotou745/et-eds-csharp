using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.Common
{
    public class DealResultInfo
    {
        private bool _dealFlag = true;
        /// <summary>
        /// 处理结果标识
        /// </summary>
        public bool DealFlag
        {
            get { return _dealFlag; }
            set { _dealFlag = value; }
        }
        /// <summary>
        /// 处理返回信息
        /// </summary>
        public string DealMsg { get; set; }
        /// <summary>
        /// 处理成功订单的数量
        /// </summary>
        public int DealSuccQty { get; set; }
        /// <summary>
        /// 处理成功的Id
        /// </summary>
        public string SuccessId { get; set; }
        /// <summary>
        /// 处理失败的Id
        /// </summary>
        public string FailId { get; set; }

    }
}
