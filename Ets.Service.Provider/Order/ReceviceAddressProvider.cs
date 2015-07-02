﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Dao.Order;
using Ets.Model.Common;
using Ets.Model.DomainModel.Order;
using Ets.Model.ParameterModel.Order;
using Ets.Service.IProvider.Order;

namespace Ets.Service.Provider.Order
{
    /// <summary>
    /// 收货人地址业务逻辑实现  add By  caoheyang   20150702
    /// </summary>
    public class ReceviceAddressProvider : IReceviceAddressProvider
    {
        private readonly ReceviceAddressDao receviceAddressDao = new ReceviceAddressDao();
        /// <summary>
        ///  B端商户拉取收货人地址缓存到本地 add By  caoheyang   20150702 
        /// </summary>
        /// <param name="model">参数实体</param>
        /// <returns></returns>
        public ResultModel<object> ConsigneeAddressB(ConsigneeAddressBPM model)
        {
            IList<ConsigneeAddressBDM> models = receviceAddressDao.ConsigneeAddressB(model); 
            return null;
        }
    }
}
