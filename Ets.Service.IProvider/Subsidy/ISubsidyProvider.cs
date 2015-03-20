using Ets.Model.DomainModel.Subsidy;
using Ets.Model.ParameterModel.Subsidy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Service.IProvider.Subsidy
{
    public interface ISubsidyProvider
    {
        SubsidyResultModel GetCurrentSubsidy(int groupId = 0);
        /// <summary>
        /// 获取补贴设置信息
        /// danny-20150320
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        SubsidyManage GetSubsidyList(HomeCountCriteria criteria);
        /// <summary>
        /// 添加补贴配置记录
        /// danny-20150320
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool SaveData(SubsidyModel model);
    }
}
