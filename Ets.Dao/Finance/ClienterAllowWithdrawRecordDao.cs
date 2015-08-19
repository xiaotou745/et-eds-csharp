#region 引用
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Data;
using Ets.Model.DataModel.Finance;
using ETS.Dao;
using ETS.Data.Core;
using ETS.Extension;
using Ets.Model.DomainModel.Finance;
using ETS.Util;
using Ets.Model.ParameterModel.Finance;
#endregion

namespace Ets.Dao.Finance
{
    /// <summary>
    /// 数据访问类ClienterAllowWithdrawRecordDao。
    /// Generate By: tools.etaoshi.com
    /// Generate Time: 2015-08-03 15:34:49
    /// </summary>
    public class ClienterAllowWithdrawRecordDao : DaoBase
    {
        /// <summary>
        /// 增加一条记录
        /// </summary>
        public long Insert(ClienterAllowWithdrawRecord clienterAllowWithdrawRecord)
        {
            const string insertSql = @"
insert into ClienterAllowWithdrawRecord
(ClienterId,Amount,Status,Balance,RecordType,Operator,WithwardId,RelationNo,Remark)
select @ClienterId,@Amount,@Status,c.AllowWithdrawPrice,@RecordType,@Operator,@WithwardId,@RelationNo,@Remark 
from dbo.clienter as c where Id=@ClienterId
select @@IDENTITY";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("ClienterId", clienterAllowWithdrawRecord.ClienterId);//骑士id
            dbParameters.AddWithValue("Amount", clienterAllowWithdrawRecord.Amount);//流水金额
            dbParameters.AddWithValue("Status", clienterAllowWithdrawRecord.Status); //流水状态(1、交易成功 2、交易中）
            dbParameters.AddWithValue("RecordType", clienterAllowWithdrawRecord.RecordType); //交易类型(1佣金 2奖励 3提现 4取消订单赔偿 5无效订单扣款)
            dbParameters.AddWithValue("Operator", clienterAllowWithdrawRecord.Operator); //操作人 
            dbParameters.AddWithValue("WithwardId", clienterAllowWithdrawRecord.WithwardId); //关联ID
            dbParameters.AddWithValue("RelationNo", clienterAllowWithdrawRecord.RelationNo); //关联单号
            dbParameters.AddWithValue("Remark", clienterAllowWithdrawRecord.Remark); //描述
            object result = DbHelper.ExecuteScalar(SuperMan_Write, insertSql, dbParameters);
            return ParseHelper.ToLong(result);
        }

        /// <summary>
        /// 查询对象
        /// </summary>
        public IList<ClienterAllowWithdrawRecord> Query(ClienterAllowWithdrawRecordPM clienterAllowWithdrawRecordPM)
        {
            string condition = BindQueryCriteria(clienterAllowWithdrawRecordPM);
            IList<ClienterAllowWithdrawRecord> models = new List<ClienterAllowWithdrawRecord>();
            string querysql = @"
select  Id,ClienterId,Amount,Status,Balance,RecordType,Operator,OperateTime,WithwardId,RelationNo,Remark,IsEnable
from  ClienterAllowWithdrawRecord (nolock)" + condition;
            DataTable dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, querysql));
            if (DataTableHelper.CheckDt(dt))
            {
                models = DataTableHelper.ConvertDataTableList<ClienterAllowWithdrawRecord>(dt);
            }
            return models;
        }


        #region  Other Members

        /// <summary>
        /// 构造查询条件
        /// </summary>
        public static string BindQueryCriteria(ClienterAllowWithdrawRecordPM clienterAllowWithdrawRecordPM)
        {
            var stringBuilder = new StringBuilder(" where 1=1 ");
            if (clienterAllowWithdrawRecordPM == null)
            {
                return stringBuilder.ToString();
            }

            //TODO:在此加入查询条件构建代码
            if (clienterAllowWithdrawRecordPM.ClienterId > 0)
            {
                stringBuilder.Append(" and ClienterId=" + clienterAllowWithdrawRecordPM.ClienterId);
            }

            stringBuilder.Append(" order by id ");

            return stringBuilder.ToString();
        }

        #endregion


    }
}

