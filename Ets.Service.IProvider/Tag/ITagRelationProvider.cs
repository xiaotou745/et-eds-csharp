using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Model.Common;
using Ets.Model.DataModel.Tag;

namespace Ets.Service.IProvider.Tag
{
    /// <summary>
    /// 标签关系类
    /// caoheyang 20150917
    /// </summary>
    public interface ITagRelationProvider
    {

        /// <summary>
        /// 获取用户 标签关系列表
        /// caoheyang 20150917
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userType"></param>
        /// <returns></returns>
        IList<TagRelation> GetTagRelationRelationList(int userId, int userType);

        /// <summary>
        /// 修改标签
        /// caoheyang 20150917
        /// </summary>
        /// <param name="busiId">商户Id</param>
        /// <param name="tags">标签</param>
        /// <param name="optName">操作人</param>
        /// <param name="userType"></param>
        /// <returns></returns>
        DealResultInfo ModifyTags(int busiId, string tags, string optName, int userType);
    }
}
