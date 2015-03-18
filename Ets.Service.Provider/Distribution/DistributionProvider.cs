using Ets.Dao.Distribution;
using Ets.Model.Common;
using Ets.Model.DataModel.Clienter;
using Ets.Model.DomainModel.Bussiness;
using Ets.Model.DomainModel.Clienter;
using Ets.Service.IProvider.Distribution;
using ETS.Data.PageData;
using ETS.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Service.Provider.Distribution
{
    /// <summary>
    /// 骑士业务逻辑接口实现类
    /// danny-20150318
    /// </summary>
    public class DistributionProvider : IDistributionProvider
    {
        DistributionDao dao = new DistributionDao();
        /// <summary>
        /// 获取骑士信息
        /// danny-20150318
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public ClienterManage GetClienteres(ClienterSearchCriteria criteria)
        {
            var pagedQuery = new ClienterManage();
            PageInfo<ClienterListModel> pageinfo = dao.GetClienteres<ClienterListModel>(criteria);
            NewPagingResult pr = new NewPagingResult() { PageIndex = criteria.PagingRequest.PageIndex, PageSize = criteria.PagingRequest.PageSize, RecordCount = pageinfo.All, TotalCount = pageinfo.All };
            List<ClienterListModel> list = pageinfo.Records.ToList();
            var clenterlists = new ClienterManageList(list, pr);
            pagedQuery.clienterManageList = clenterlists;
            return pagedQuery;
        }
        /// <summary>
        /// 更新审核状态
        /// danny-20150318
        /// </summary>
        /// <param name="id"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        public bool UpdateAuditStatus(int id, EnumStatusType enumStatusType)
        {
            return dao.UpdateAuditStatus(id, enumStatusType);
        }
        /// <summary>
        /// 清空帐户余额
        /// danny-20150318
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool ClearSuperManAmount(int id)
        {
            return dao.ClearSuperManAmount(id);
        }
        /// <summary>
        /// 检查骑士手机是否存在
        /// danny-20150318
        /// </summary>
        /// <param name="phoneNo"></param>
        /// <returns></returns>
        public bool CheckExistPhone(string phoneNo)
        {
            return dao.CheckExistPhone(phoneNo);
        }
        /// <summary>
        /// 添加骑士
        /// danny-20150318
        /// </summary>
        /// <param name="clienter"></param>
        /// <returns></returns>
        public bool AddClienter(ClienterListModel clienter)
        {
            return dao.AddClienter(clienter);
        }
    }
}
