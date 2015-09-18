using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Dao.Tag;
using Ets.Model.Common;
using Ets.Model.DataModel.Tag;
using Ets.Model.DomainModel.Business;
using Ets.Service.IProvider.Tag;
using ETS.Transaction;

namespace Ets.Service.Provider.Tag
{
    /// <summary>
    /// 标签类
    /// caoheyang 20150917
    /// </summary>
    public class TagRelationProvider : ITagRelationProvider
    {
        private TagRelationDao tagRelationDao = new TagRelationDao();
        /// <summary>
        /// 获取用户 标签关系列表
        /// caoheyang 20150917
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userType"></param>
        /// <returns></returns>
        public IList<TagRelation> GetTagRelationRelationList(int userId, int userType)
        {
            return tagRelationDao.GetTagRelationRelationList(userId, userType);
        }

        /// <summary>
        /// 修改标签
        /// caoheyang 20150917
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="tags"></param>
        /// <param name="optName"></param>
        /// <param name="userType"></param>
        /// <returns></returns>
        public DealResultInfo ModifyTags(int userId, string tags, string optName, int userType)
        {
            var dealResultInfo = new DealResultInfo
            {
                DealFlag = false
            };
            if (!string.IsNullOrEmpty(tags))
            {
                var tag = tags.Split(';');
                using (var tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
                {
                    foreach (var item in tag)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            var temp = item.Split(',');
                            var berm = new TagRelation
                            {
                                TagId = Convert.ToInt32(temp[0]),
                                IsEnable = Convert.ToInt32(temp[1]),
                                UserId = userId,
                                CreateBy = optName,
                                UserType = userType
                            };
                            if (!tagRelationDao.Edit(berm))
                            {
                                dealResultInfo.DealMsg = "编辑标签配置信息失败！";
                                return dealResultInfo;
                            }
                        }
                    }
                    tran.Complete();
                    dealResultInfo.DealMsg = "编辑标签信息成功！";
                    dealResultInfo.DealFlag = true;
                    return dealResultInfo;
                }
            }
            dealResultInfo.DealMsg = "未获取到标签配置信息！";
            return dealResultInfo;
        }
    }
}
