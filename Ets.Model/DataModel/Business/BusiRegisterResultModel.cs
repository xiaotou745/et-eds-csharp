using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DataModel.Business
{
    /// <summary>
    /// B端注册返回结果
    /// </summary>
    public class BusiRegisterResultModel
    {
        /// <summary>
        /// 用户的Id
        /// </summary>
        public int userId { get; set; }

        /// <summary>
        /// 唯一健值guid
        /// </summary>
        public string Appkey { get; set; }

        /// <summary>
        /// Tokey值
        /// </summary>
        public string Token { get; set; }

    }
}
