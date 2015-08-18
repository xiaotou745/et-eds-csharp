using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Ets.Dao.Complain;
using Ets.Model.Common;
using Ets.Model.DataModel.Complain;
using Ets.Model.ParameterModel.Complain;
using Ets.Service.IProvider.Complain;
using ETS.Enums;

namespace Ets.Service.Provider.Complain
{
    public class ComplainProvider : IComplainProvider
    {
        readonly ComplainDao _complainDao = new ComplainDao(); 

        public ResultModel<object> Complain(ComplainModel complainModel)
        { 
            int result = _complainDao.Insert(complainModel);
            if (result == 1)
            {
                return ResultModel<object>.Conclude(ComplainEnum.Success);
            }
            else
            {
                return ResultModel<object>.Conclude(ComplainEnum.Fail);
            }
        }

        public object Get(ComplainCriteria complainCriteria)
        {
            throw new NotImplementedException();
        }
    }
}
