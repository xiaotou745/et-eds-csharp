using Ets.Dao.Subsidy;
using Ets.Model.DomainModel.Subsidy;
using Ets.Service.IProvider.Subsidy;
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
    }
}
