using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Dao.Tag;
using Ets.Model.DataModel.Tag;
using Ets.Service.IProvider.Tag;

namespace Ets.Service.Provider.Tag
{
    /// <summary>
    /// 标签类
    /// caoheyang 20150917
    /// </summary>
    public class TagProvider : ITagProvider
    {
        private readonly TagDao tagDao = new TagDao();
        /// <summary>
        /// 标签类型（0:门店 1:骑士） 根据标签类型查询所有启用标签
        /// </summary>
        /// <param name="tagType"></param>
        /// <returns></returns>
        public IList<TagModel> GetTagsByTagType(int tagType)
        {
            return tagDao.GetTagsByTagType(tagType);
        }
        /// <summary>
        /// 标签类型（0:门店 1:骑士） 查询所有启用标签
        /// </summary>
        /// <param name="tagType"></param>
        /// <returns></returns>
        public IList<TagModel> GetTagsByTagType()
        {
            return tagDao.GetTagsByTagType();
        }
    }
}
