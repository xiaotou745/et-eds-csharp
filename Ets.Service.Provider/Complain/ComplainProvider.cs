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
    public class ComplainProvider : IComplainProvider
    {
        readonly ComplainDao _complainDao = new ComplainDao(); 

        public ResultModel<object> Complain(ComplainModel complainModel)
        {
            //int hadComplain = _complainDao.HadComplain(complainModel);
            //if (hadComplain > 0)
            //{
            //    return ResultModel<object>.Conclude(ComplainEnum.HadComplain);
            //} 
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
        public PageInfo<ComplainDomain> Get(ComplainCriteria complainCriteria)
        {
            return _complainDao.Get<ComplainDomain>(complainCriteria);
        } 
    }
}
