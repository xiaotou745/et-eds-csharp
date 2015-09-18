using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Model.DataModel.Tag;

namespace Ets.Service.IProvider.Tag
{
    /// <summary>
    /// 标签
    /// caoheyang 20150917
    /// </summary>
    public interface ITagProvider
    {
        /// <summary>
        /// 标签类型（0:门店 1:骑士） 根据标签类型查询所有启用标签
        /// </summary>
        /// <param name="tagType"></param>
        /// <returns></returns>
        IList<TagModel> GetTagsByTagType(int tagType);
        /// <summary>
        /// 标签类型（0:门店 1:骑士）查询所有启用标签
        /// </summary>
        /// <returns></returns>
        IList<TagModel> GetTagsByTagType();
    }
}
