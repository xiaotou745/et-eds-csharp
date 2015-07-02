using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Model.Common;
using Ets.Model.ParameterModel.Order;

namespace Ets.Service.IProvider.Order
{

    /// <summary>
    /// 收货人地址业务逻辑接口  add By  caoheyang   20150702
    /// </summary>
    public interface IReceviceAddressProvider
    {
        /// <summary>
        ///  B端商户拉取收货人地址缓存到本地 add By  caoheyang   20150702 
        /// </summary>
        /// <param name="model">参数实体</param>
        /// <returns></returns>
        ResultModel<object> ConsigneeAddressB(ConsigneeAddressBPM model);
    }
}
