using System.Data;
using ETS.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Data.Core;
using ETS.Extension;
using Ets.Model.DataModel.Finance;
using Ets.Model.ParameterModel.Finance;
using ETS.Util;

namespace Ets.Dao.Finance
{
    /// <summary>
    /// 商户提现日志表 数据访问类BusinessWithdrawLogDao。
    /// Generate By: tools.etaoshi.com
    /// Generate Time: 2015-05-09 16:02:12
    /// </summary>

    public class BusinessWithdrawLogDao : DaoBase
    {
        public BusinessWithdrawLogDao()
        {

        }
        #region IBusinessWithdrawLogRepos  Members

        /// <summary>
        /// 增加一条记录
        /// </summary>
        /// <param name="businessWithdrawLog">参数实体</param>
        /// <returns></returns>
        public long Insert(BusinessWithdrawLog businessWithdrawLog)
        {
            const string insertSql = @"
insert into BusinessWithdrawLog(WithwardId,Status,Remark,Operator)
values(@WithwardId,@Status,@Remark,@Operator)
select @@IDENTITY";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("WithwardId", businessWithdrawLog.WithwardId); //提现单ID
            dbParameters.AddWithValue("Status", businessWithdrawLog.Status);  //操作后状态
            dbParameters.AddWithValue("Remark", businessWithdrawLog.Remark); //备注
            dbParameters.AddWithValue("Operator", businessWithdrawLog.Operator);  //操作人
            object result = DbHelper.ExecuteScalar(SuperMan_Write, insertSql, dbParameters);
            return ParseHelper.ToLong(result);
        }    

    
        #endregion

    }
}
