using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Model.ParameterModel.Order;

namespace Ets.Service.IProvider.OpenApi
{
    /// <summary>
    /// 第三方集团佣金计算
    /// </summary>
   public interface IGroupSetCommissonOpenApi
    {
        /// <summary>
        /// 新增商铺时根据集团id为店铺设置外送费，结算比例等财务相关信息 add by caoheyang 20150417
        /// </summary>
        /// <param name="paramodel"></param>
        /// <returns></returns>
        CreatePM_OpenApi SetCommissonInfo(CreatePM_OpenApi paramodel);
    }
}
