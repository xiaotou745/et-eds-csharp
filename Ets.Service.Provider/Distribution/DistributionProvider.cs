﻿using ETS.Const;
using Ets.Dao.Distribution;
using Ets.Model.Common;
using Ets.Model.DataModel.Clienter;
using Ets.Model.DomainModel.Bussiness;
using Ets.Model.DomainModel.Clienter;
using Ets.Model.ParameterModel.Clienter;
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
        public PageInfo<ClienterListModel> GetClienteres(ClienterSearchCriteria criteria)
        {
            var pagedQuery = new ClienterManage();
            PageInfo<ClienterListModel> pageinfo = dao.GetClienteres<ClienterListModel>(criteria);
            return pageinfo;
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
            ETS.NoSql.RedisCache.RedisCache redis = new ETS.NoSql.RedisCache.RedisCache();
            string cacheKey = string.Format(RedissCacheKey.ClienterProvider_GetUserStatus, id);
            redis.Delete(cacheKey);
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
        /// 添加骑士
        /// danny-20150318
        /// </summary>
        /// <param name="clienter"></param>
        /// <returns></returns>
        public bool AddClienter(ClienterListModel clienter)
        {
            return dao.AddClienter(clienter);
        }
        /// <summary>
        /// 根据集团id获取超人列表
        /// danny-20150319
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public IList<ClienterListModel> GetClienterModelByGroupID(int? groupId)
        {
            return dao.GetClienterModelByGroupID(groupId);
        }
        /// <summary>
        /// 骑士统计
        /// danny-20150326
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public Ets.Model.Common.ClienterCountManageList GetClienteresCount(ClienterSearchCriteria criteria)
        {
            PageInfo<ClienterViewModel> pageinfo = dao.GetClienteresCount<ClienterViewModel>(criteria);
            NewPagingResult pr = new NewPagingResult() { PageIndex = criteria.PagingRequest.PageIndex, PageSize = criteria.PagingRequest.PageSize, RecordCount = pageinfo.All, TotalCount = pageinfo.All };
            List<ClienterViewModel> list = pageinfo.Records.ToList();
            var clienterCountManageList = new Ets.Model.Common.ClienterCountManageList(list, pr);
            return clienterCountManageList;
        }
        
    }
}
