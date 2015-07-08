using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DataModel.Clienter
{
    /// <summary>
    /// 登录成功返回的数据
    /// </summary>
    public class ClienterLoginResultModel
    {
        public int userId { get; set; }
        public byte? status { get; set; }
        public decimal? Amount { get; set; }
        public string phoneNo { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        public string city { get; set; }
        /// <summary>
        /// 城市编码
        /// </summary>
        public string cityId { get; set; }

        /// <summary>
        /// 是否已绑定商家
        /// </summary>
        public int IsBind { get; set; }
        /// <summary>
        /// 是否只显示雇主任务
        /// </summary>
        public int IsOnlyShowBussinessTask { get; set; }
        
    }
}
