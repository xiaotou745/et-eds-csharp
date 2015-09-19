using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Model.Common;
using Ets.Model.DataModel.Complain;
using Ets.Model.DomainModel.Complain;
using Ets.Model.ParameterModel.Complain;
using ETS.Data.PageData;

namespace Ets.Service.IProvider.Complain
{
    public interface IComplainProvider
    {
        /// <summary>
        /// 投诉wc
        /// </summary>
        /// <param name="ccb"></param>
        /// <returns></returns>
        ResultModel<object> Complain(ComplainModel ccb);
        /// <summary>
        /// 获取投诉数据wc
        /// </summary>
        /// <param name="complainCriteria"></param>
        /// <returns></returns>
        PageInfo<ComplainDomain> Get(ComplainCriteria complainCriteria);

        /// <summary>
        /// 意见处理
        /// 胡灵波
        /// 2015年9月19日 15:46:16
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool UpdateComplainHandle(ComplainPM model);
    }
}
