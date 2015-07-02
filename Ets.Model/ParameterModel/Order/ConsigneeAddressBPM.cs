using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Order
{
    /// <summary>
    ///  B端商户拉取收货人地址缓存到本地接口参数实体 add By  caoheyang   20150702
    /// </summary>
    public class ConsigneeAddressBPM
    {
        ///<summary>
        /// 商户ID
        /// </summary>
        public int BusinessId { get; set; }

        ///<summary>
        /// 本次拉取地址的数据库开始id，第一次或者app端无缓存数据时拉取传0
        /// </summary>
        public long AddressId { get; set; }
        ///<summary>
        /// 版本号1.0
        /// </summary>
        public string Version { get; set; }
    }
}
