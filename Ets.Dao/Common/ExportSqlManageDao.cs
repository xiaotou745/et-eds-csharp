using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Const;
using ETS.Dao;
using ETS.Data.Core;
using ETS.Data.PageData;
using ETS.Extension;
using Ets.Model.DataModel.Common;
using Ets.Model.ParameterModel.Common;
using ETS.Util;

namespace Ets.Dao.Common
{
    /// <summary>
    ///  sql定时导出发送数据   数据库访问层 add by caoheyang 20150601
    /// </summary>
    public class ExportSqlManageDao : DaoBase
    {
        public ExportSqlManageDao()
        { }
        #region IExportSqlManageRepos  Members

        /// <summary>
        /// 增加一条记录
        /// </summary>
        public int Insert(ExportSqlManage model)
        {
            const string insertSql = @"
insert into ExportSqlManage(Name,SqlText,Executetime,ReceiveEmail,IsEnable)
values(@Name,@SqlText,@Executetime,@ReceiveEmail,@IsEnable)
select @@identity
";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("Name", model.Name);
            dbParameters.AddWithValue("SqlText", model.SqlText);
            dbParameters.AddWithValue("Executetime", model.Executetime);
            dbParameters.AddWithValue("ReceiveEmail", model.ReceiveEmail);
            dbParameters.AddWithValue("IsEnable", model.IsEnable);
            return ParseHelper.ToInt(DbHelper.ExecuteScalar(SuperMan_Write, insertSql, dbParameters));
        }

        /// <summary>
        /// 删除一条记录
        /// </summary>
        public int Delete(long id)
        {
            const string deleteSql = @"delete from ExportSqlManage where ID=@ID";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("ID", id);
            return ParseHelper.ToInt(DbHelper.ExecuteNonQuery(SuperMan_Write, deleteSql, dbParameters));
        }

        /// <summary>
        /// 查询对象
        /// </summary>
        public PageInfo<ExportSqlManage> Query(DataManageSearchCriteria search)
        {
            string where = " 1=1 ";
            if (!string.IsNullOrWhiteSpace(search.Name))
            {
                where = where + string.Format(" and Name like '%{0}%'", search.Name);
            }
            return new PageHelper().GetPages<ExportSqlManage>(SuperMan_Read, search.PageIndex, where, "Id desc", " Id,Name,SqlText,Executetime,ReceiveEmail,IsEnable", " ExportSqlManage (nolock)", SystemConst.PageSize, true);
        }

        /// <summary>
        /// 根据id查询对象
        /// </summary>
        public ExportSqlManage GetById(long id)
        {
            ExportSqlManage model = null;
            const string getbyidSql = @"
select  Id,Name,SqlText,Executetime,ReceiveEmail,IsEnable
from  ExportSqlManage (nolock)
where  id=@ID";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("ID", id);
            DataTable dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, getbyidSql, dbParameters));
            if (DataTableHelper.CheckDt(dt))
            {
                model = DataTableHelper.ConvertDataTableList<ExportSqlManage>(dt)[0];
            }
            return model;
        }

        /// <summary>
        /// 更新一条记录
        /// </summary>
        public int Update(ExportSqlManage model)
        {
            const string updateSql = @"
update  ExportSqlManage
set  Name=@Name,SqlText=@SqlText,Executetime=@Executetime,ReceiveEmail=@ReceiveEmail,IsEnable=@IsEnable
where  id=@ID";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("Id", model.Id);
            dbParameters.AddWithValue("Name", model.Name);
            dbParameters.AddWithValue("SqlText", model.SqlText);
            dbParameters.AddWithValue("Executetime", model.Executetime);
            dbParameters.AddWithValue("ReceiveEmail", model.ReceiveEmail);
            dbParameters.AddWithValue("IsEnable", model.IsEnable);
            return ParseHelper.ToInt(DbHelper.ExecuteNonQuery(SuperMan_Write, updateSql, dbParameters));
        }


        #endregion

    }
}
