using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Ets.Dao.Complain;
using Ets.Model.Common;
using Ets.Model.DataModel.Complain;
using Ets.Model.DomainModel.Complain;
using Ets.Model.ParameterModel.Complain;
using Ets.Service.IProvider.Complain;
using ETS.Data.PageData;
using ETS.Enums;

namespace Ets.Service.Provider.Complain
{
    /// <summary>
    /// 投诉相关
    /// wc
    /// </summary>
    public class ComplainProvider : IComplainProvider
    {
        readonly ComplainDao _complainDao = new ComplainDao(); 
        /// <summary>
        /// 投诉wc
        /// </summary>
        /// <param name="complainModel"></param>
        /// <returns></returns>
        public ResultModel<object> Complain(ComplainModel complainModel)
        { 
            int result = _complainDao.Insert(complainModel);
            if (result >0 )
            {
                return ResultModel<object>.Conclude(ComplainEnum.Success);
            }
            else
            {
                return ResultModel<object>.Conclude(ComplainEnum.Fail);
            }
        }
        /// <summary>
        /// 获取投诉数据wc
        /// </summary>
        /// <param name="complainCriteria"></param>
        /// <returns></returns>
        public PageInfo<ComplainDomain> Get(ComplainCriteria complainCriteria)
        {
            return _complainDao.Get<ComplainDomain>(complainCriteria);
        } 
    }
}
