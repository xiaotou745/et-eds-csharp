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

namespace Ets.Dao.Finance
{
    /// <summary>
    /// 骑士提现日志表 数据访问类ClienterWithdrawLogDao。
    /// Generate By: tools.etaoshi.com
    /// Generate Time: 2015-05-09 16:02:12
    /// </summary>

    public class ClienterWithdrawLogDao : DaoBase
    {
        public ClienterWithdrawLogDao()
        {

        }
        #region IClienterWithdrawLogRepos  Members

        /// <summary>
        /// 增加一条记录
        /// </summary>
        /// <param name="clienterWithdrawLog">参数实体</param>
        /// <returns></returns>
        public long Insert(ClienterWithdrawLog clienterWithdrawLog)
        {
            const string insertSql = @"
insert into ClienterWithdrawLog(WithwardId,Status,Remark,Operator,OperatTime)
values(@WithwardId,@Status,@Remark,@Operator,@OperatTime)
select @@IDENTITY";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("WithwardId", clienterWithdrawLog.WithwardId); //提现单ID
            dbParameters.AddWithValue("Status", clienterWithdrawLog.Status);  //操作后状态
            dbParameters.AddWithValue("Remark", clienterWithdrawLog.Remark); //备注
            dbParameters.AddWithValue("Operator", clienterWithdrawLog.Operator);  //操作人
            dbParameters.AddWithValue("OperatTime", clienterWithdrawLog.OperatTime);  //操作时间
            object result = DbHelper.ExecuteScalar(SuperMan_Write, insertSql, dbParameters);
            if (result == null)
            {
                return 0;
            }
            return long.Parse(result.ToString());
        }

        /// <summary>
        /// 更新一条记录
        /// </summary>
        /// <param name="clienterWithdrawLog">参数实体</param>
        public void Update(ClienterWithdrawLog clienterWithdrawLog)
        {
       
        }

        /// <summary>
        /// 删除一条记录
        /// </summary>
        public void Delete(long id)
        {

        }

        /// <summary>
        /// 查询对象
        /// </summary>
        /// <param name="clienterWithdrawLogPm">参数实体</param>
        /// <returns></returns>
        public IList<ClienterWithdrawLog> Query(ClienterWithdrawLogPM clienterWithdrawLogPm)
        {
            IList<ClienterWithdrawLog> models = new List<ClienterWithdrawLog>();
            string condition = BindQueryCriteria(clienterWithdrawLogPm);
            string querysql = @"
select  Id,WithwardId,Status,Remark,Operator,OperatTime
from  ClienterWithdrawLog (nolock)" + condition;
            DataTable dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, querysql));
            if (DataTableHelper.CheckDt(dt))
            {
                models = DataTableHelper.ConvertDataTableList<ClienterWithdrawLog>(dt);
            }
            return models;
        }


        /// <summary>
        /// 根据ID获取对象
        /// </summary>
        public ClienterWithdrawLog GetById(long id)
        {
            ClienterWithdrawLog model = new ClienterWithdrawLog();
            const string querysql = @"
select  Id,WithwardId,Status,Remark,Operator,OperatTime
from  ClienterWithdrawLog (nolock)
where  Id=@Id ";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("Id", id);

            DataTable dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, querysql, dbParameters));
            if (DataTableHelper.CheckDt(dt))
            {
                model = DataTableHelper.ConvertDataTableList<ClienterWithdrawLog>(dt)[0];
            }
            return model;
        }

        #endregion

        #region  Other Members

        /// <summary>
        /// 构造查询条件
        /// </summary>
        public static string BindQueryCriteria(ClienterWithdrawLogPM clienterWithdrawLogPm)
        {
            var stringBuilder = new StringBuilder(" where 1=1 ");
            if (clienterWithdrawLogPm == null)
            {
                return stringBuilder.ToString();
            }
            //TODO:在此加入查询条件构建代码
            return stringBuilder.ToString();
        }

        #endregion

    }
}
