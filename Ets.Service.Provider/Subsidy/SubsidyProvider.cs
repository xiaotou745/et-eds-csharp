using Ets.Dao.Subsidy;
using Ets.Model.Common;
using Ets.Model.DataModel.Subsidy;
using Ets.Model.DomainModel.Subsidy;
using Ets.Model.ParameterModel.Subsidy;
using Ets.Service.IProvider.Subsidy;
using ETS.Data.PageData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Service.Provider.Subsidy
{

    public class SubsidyProvider : ISubsidyProvider
    {
        private SubsidyDao subsidyDao = new SubsidyDao();
        /// <summary>
        /// 获取补贴设置  集团可选。
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public SubsidyResultModel GetCurrentSubsidy(int groupId = 0)
        { 
            var subsidyResultModel = subsidyDao.GetCurrentSubsidy(groupId);

            return subsidyResultModel;

        }

        /// <summary>
        /// 获取补贴设置信息
        /// danny-20150320
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public SubsidyManage GetSubsidyList(HomeCountCriteria criteria)
        {
            var pagedQuery = new SubsidyManage();
            PageInfo<subsidy> pageinfo = subsidyDao.GetSubsidyList<subsidy>(criteria);
            NewPagingResult pr = new NewPagingResult() { PageIndex = criteria.PagingRequest.PageIndex, PageSize = criteria.PagingRequest.PageSize, RecordCount = pageinfo.All, TotalCount = pageinfo.All };
            List<subsidy> list = pageinfo.Records.ToList();
            var subsidylists = new SubsidyManageList(list, pr);
            pagedQuery.subsidyManageList = subsidylists;
            return pagedQuery;
        }
        /// <summary>
        /// 添加补贴配置记录
        /// danny-20150320
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool SaveData(SubsidyModel model)
        {
            return subsidyDao.SaveData(model);
        }
    }
}
