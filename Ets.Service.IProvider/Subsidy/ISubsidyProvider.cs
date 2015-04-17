using Ets.Model.DataModel.Subsidy;
using Ets.Model.DomainModel.Subsidy;
using Ets.Model.ParameterModel.Subsidy;
using ETS.Data.PageData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Model.DomainModel.GlobalConfig;
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
        PageInfo<subsidy> GetSubsidyList(HomeCountCriteria criteria);
        /// <summary>
        /// 添加补贴配置记录
        /// danny-20150320
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool SaveData(SubsidyModel model);

        /// <summary>
        /// 跨店补贴
        /// 徐鹏程
        /// 20150414
        /// </summary>
        bool CrossShop(List<GlobalConfigSubsidies> SubsidiesList);
        
        /// <summary>
        /// 跨店补贴短信
        /// 徐鹏程
        /// 20150416
        /// </summary>
        bool ShortMessage();
    }
}
