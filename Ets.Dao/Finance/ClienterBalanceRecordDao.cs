﻿#region 引用
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
    /// 骑士余额流水表 数据访问类ClienterBalanceRecordDao。
    /// Generate By: tools.etaoshi.com   caoheyang
    /// Generate Time: 2015-05-09 14:59:36
    /// </summary>
    public class ClienterBalanceRecordDao : DaoBase
    {
        public ClienterBalanceRecordDao()
        {

        }
        #region IClienterBalanceRecordRepos  Members

        /// <summary>
        /// 增加一条记录
        /// </summary>
        /// <param name="clienterBalanceRecord">参数实体</param>
        /// <returns></returns>
        public long Insert(ClienterBalanceRecord clienterBalanceRecord)
        {
            const string insertSql = @"
insert into ClienterBalanceRecord
(ClienterId,Amount,Status,Balance,RecordType,Operator,WithwardId,RelationNo,Remark)
select @ClienterId,@Amount,@Status,c.AccountBalance,@RecordType,@Operator,@WithwardId,@RelationNo,@Remark 
from dbo.clienter as c where Id=@ClienterId
select @@IDENTITY";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("ClienterId", clienterBalanceRecord.ClienterId);//骑士id
            dbParameters.AddWithValue("Amount", clienterBalanceRecord.Amount);//流水金额
            dbParameters.AddWithValue("Status", clienterBalanceRecord.Status); //流水状态(1、交易成功 2、交易中）
            dbParameters.AddWithValue("RecordType", clienterBalanceRecord.RecordType); //交易类型(1佣金 2奖励 3提现 4取消订单赔偿 5无效订单扣款)
            dbParameters.AddWithValue("Operator", clienterBalanceRecord.Operator); //操作人 
            dbParameters.AddWithValue("WithwardId", clienterBalanceRecord.WithwardId); //关联ID
            dbParameters.AddWithValue("RelationNo", clienterBalanceRecord.RelationNo); //关联单号
            dbParameters.AddWithValue("Remark", clienterBalanceRecord.Remark); //描述
            object result = DbHelper.ExecuteScalar(SuperMan_Write, insertSql, dbParameters);
            return ParseHelper.ToLong(result);
        }

        /// <summary>
        /// 更新一条记录
        /// </summary>
        /// <param name="clienterBalanceRecord">参数实体</param>
        public void Update(ClienterBalanceRecord clienterBalanceRecord)
        {
            const string updateSql = @"
update  ClienterBalanceRecord
set  Amount=@Amount,Status=@Status,Balance=@Balance,RecordType=@RecordType,Operator=@Operator,OperateTime=@OperateTime,RelationNo=@RelationNo,Remark=@Remark
where  Id=@Id ";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("Amount", clienterBalanceRecord.Amount);
            dbParameters.AddWithValue("Status", clienterBalanceRecord.Status);
            dbParameters.AddWithValue("Balance", clienterBalanceRecord.Balance);
            dbParameters.AddWithValue("RecordType", clienterBalanceRecord.RecordType);
            dbParameters.AddWithValue("Operator", clienterBalanceRecord.Operator);
            dbParameters.AddWithValue("OperateTime", clienterBalanceRecord.OperateTime);
            dbParameters.AddWithValue("RelationNo", clienterBalanceRecord.RelationNo);
            dbParameters.AddWithValue("Remark", clienterBalanceRecord.Remark);
            DbHelper.ExecuteNonQuery(SuperMan_Write, updateSql, dbParameters);
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
        /// <param name="clienterBalanceRecordPm">参数实体</param>
        /// <returns></returns>
        public IList<ClienterBalanceRecord> Query(ClienterBalanceRecordPM clienterBalanceRecordPm)
        {
            string condition = BindQueryCriteria(clienterBalanceRecordPm);
            IList<ClienterBalanceRecord> models = new List<ClienterBalanceRecord>();
            string querysql = @"
select  Id,ClienterId,Amount,Status,Balance,RecordType,Operator,OperateTime,RelationNo,Remark
from  ClienterBalanceRecord (nolock)" + condition;
            DataTable dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, querysql));
            if (DataTableHelper.CheckDt(dt))
            {
                models = DataTableHelper.ConvertDataTableList<ClienterBalanceRecord>(dt);
            }
            return models;
        }
        /// <summary>
        /// 骑士交易流水API add by caoheyang 20150511
        /// </summary> 
        /// <param name="clienterId">骑士id</param>
        /// <returns></returns>
        public IList<FinanceRecordsDM> GetByClienterId(int clienterId)
        {
            IList<FinanceRecordsDM> models = new List<FinanceRecordsDM>();
            const string querysql = @"
select  Id,ClienterId as UserId,Amount,Status,Balance,RecordType,Operator,OperateTime,WithwardId,RelationNo,Remark,
substring(convert(varchar(100),OperateTime,24),1,5) as TimeInfo,
case convert(varchar(100), OperateTime, 23) 
	when convert(varchar(100), getdate(), 23) then '今日'
    else substring(convert(varchar(100), OperateTime, 23),6,5) end
as DateInfo,
case substring(convert(varchar(100), OperateTime, 23),1,7) 
	when substring(convert(varchar(100), getdate(), 23),1,7)  then '本月'
    else convert(varchar(4),datepart(Year,OperateTime))+'年'+convert(varchar(4),datepart(month,OperateTime)) +'月' end
as MonthInfo
from  ClienterBalanceRecord (nolock)
where  ClienterId=@ClienterId 
order by Id desc";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("ClienterId", clienterId);
            DataTable dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, querysql, dbParameters));
            if (DataTableHelper.CheckDt(dt))
            {
                models = DataTableHelper.ConvertDataTableList<FinanceRecordsDM>(dt);
            }
            return models;
        }

        /// <summary>
        /// 根据ID获取对象
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public ClienterBalanceRecord GetById(long id)
        {
            ClienterBalanceRecord model = new ClienterBalanceRecord();
            const string querysql = @"
select  Id,ClienterId,Amount,Status,Balance,RecordType,Operator,OperateTime,RelationNo,Remark
from  ClienterBalanceRecord (nolock)
where  Id=@Id ";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("Id", id);

            DataTable dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, querysql, dbParameters));
            if (DataTableHelper.CheckDt(dt))
            {
                model = DataTableHelper.ConvertDataTableList<ClienterBalanceRecord>(dt)[0];
            }
            return model;
        }

        #endregion

        #region  Other Members

        /// <summary>
        /// 构造查询条件 
        /// </summary>
        /// <param name="clienterBalanceRecordPm">参数实体</param>
        /// <returns></returns>
        public static string BindQueryCriteria(ClienterBalanceRecordPM clienterBalanceRecordPm)
        {
            var stringBuilder = new StringBuilder(" where 1=1 ");
            if (clienterBalanceRecordPm == null)
            {
                return stringBuilder.ToString();
            }
            //TODO:在此加入查询条件构建代码

            return stringBuilder.ToString();
        }

        #endregion
    }
}

