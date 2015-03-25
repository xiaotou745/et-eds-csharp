using Ets.Model.DataModel.Clienter;
using Ets.Model.DomainModel.Clienter;
using Ets.Model.ParameterModel.Clienter;
using ETS.Data.PageData;
using ETS.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Service.IProvider.Distribution
{
    /// <summary>
    /// 骑士业务逻辑接口 
    /// danny-20150318
    /// </summary>
    public interface IDistributionProvider
    {
        /// <summary>
        /// 获取骑士信息
        /// danny-20150318
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        PageInfo<ClienterListModel> GetClienteres(ClienterSearchCriteria criteria);
        /// <summary>
        /// 更新审核状态
        /// danny-20150318
        /// </summary>
        /// <param name="id"></param>
        /// <param name="enumStatusType"></param>
        /// <returns></returns>
        bool UpdateAuditStatus(int id, EnumStatusType enumStatusType);
        /// <summary>
        ///  清空帐户余额
        /// danny-20150318
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool ClearSuperManAmount(int id);
        /// <summary>
        /// 检查骑士手机号码是否存在
        /// danny-20150318
        /// </summary>
        /// <param name="phoneNo"></param>
        /// <returns></returns>
        bool CheckExistPhone(string phoneNo);
        /// <summary>
        /// 添加骑士
        /// danny-20150318
        /// </summary>
        /// <param name="clienter"></param>
        /// <returns></returns>
        bool AddClienter(ClienterListModel clienter);
        /// <summary>
        /// 根据集团id获取超人列表
        /// danny-20150319
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        IList<ClienterListModel> GetClienterModelByGroupID(int? groupId);
    }
}
