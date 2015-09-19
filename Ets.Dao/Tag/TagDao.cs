using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Dao;
using ETS.Data.Core;
using ETS.Extension;
using Ets.Model.DataModel.Tag;

namespace Ets.Dao.Tag
{

    /// <summary>
    /// 标签类  add by caoheyang 20150917
    /// </summary>
    public class TagDao : DaoBase
    {
        /// <summary>
        /// 标签类型（0:门店 1:骑士） 根据标签类型查询所有启用标签
        /// </summary>
        /// <param name="tagType"></param>
        /// <returns></returns>
        public IList<TagModel> GetTagsByTagType(int tagType)
        {
            IList<TagModel> models = new List<TagModel>();
            const string querysql = @"
select  Id,TagName,TagType,BindQuantity,IsEnable,CreateTime,CreateName,ModifyTime,ModifyName,IsDelete
from  tag (nolock) where tagType=@TagType and IsEnable=1 and IsDelete=0";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.Add("@TagType", DbType.Int32).Value = tagType;
            DataTable dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, querysql, dbParameters));
            if (DataTableHelper.CheckDt(dt))
            {
                models = DataTableHelper.ConvertDataTableList<TagModel>(dt);
            }
            return models;
        }

        /// <summary>
        /// 标签类型（0:门店 1:骑士） 查询所有启用标签
        /// </summary>
        /// <returns></returns>
        public IList<TagModel> GetTagsByTagType()
        {
            IList<TagModel> models = new List<TagModel>();
            const string querysql = @"
select  Id,TagName,TagType,BindQuantity,IsEnable,CreateTime,CreateName,ModifyTime,ModifyName,IsDelete
from  tag (nolock) where  IsEnable=1 and IsDelete=0";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            DataTable dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, querysql, dbParameters));
            if (DataTableHelper.CheckDt(dt))
            {
                models = DataTableHelper.ConvertDataTableList<TagModel>(dt);
            }
            return models;
        }
    }
}
