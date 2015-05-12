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
    /// 商户余额流水表 数据访问类BusinessBalanceRecordDao。
    /// Generate By: tools.etaoshi.com   caoheyang
    /// Generate Time: 2015-05-09 14:59:36
    /// </summary>
    public class BusinessBalanceRecordDao : DaoBase
    {
        public BusinessBalanceRecordDao()
        {

        }
        #region IClienterBalanceRecordRepos  Members

        /// <summary>
        /// 增加一条记录
        /// </summary>
        /// <param name="businessBalanceRecord">参数实体</param>
        /// <returns></returns>
        public long Insert(BusinessBalanceRecord businessBalanceRecord)
        {
            const string insertSql = @"
insert into BusinessBalanceRecord
(BusinessId,Amount,Status,Balance,RecordType,Operator,WithwardId,RelationNo,Remark)
values
(@BusinessId,@Amount,@Status,@Balance,@RecordType,@Operator,@WithwardId,@RelationNo,@Remark)
select @@IDENTITY";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("BusinessId", businessBalanceRecord.BusinessId);//商户id
            dbParameters.AddWithValue("Amount", businessBalanceRecord.Amount);//流水金额
            dbParameters.AddWithValue("Status", businessBalanceRecord.Status); //流水状态(1、交易成功 2、交易中）
            dbParameters.AddWithValue("Balance", businessBalanceRecord.Balance); //交易后余额
            dbParameters.AddWithValue("RecordType", businessBalanceRecord.RecordType); //交易类型(1佣金 2奖励 3提现 4取消订单赔偿 5无效订单扣款)
            dbParameters.AddWithValue("Operator", businessBalanceRecord.Operator); //操作人 
            dbParameters.AddWithValue("WithwardId", businessBalanceRecord.WithwardId); //关联ID
            dbParameters.AddWithValue("RelationNo", businessBalanceRecord.RelationNo); //关联单号
            dbParameters.AddWithValue("Remark", businessBalanceRecord.Remark); //描述
            object result = DbHelper.ExecuteScalar(SuperMan_Write, insertSql, dbParameters);
            return ParseHelper.ToLong(result);
        }

        /// <summary>
        /// 更新一条记录
        /// </summary>
        /// <param name="businessBalanceRecord">参数实体</param>
        public void Update(BusinessBalanceRecord businessBalanceRecord)
        {
            const string updateSql = @"
update  BusinessBalanceRecord
set  Amount=@Amount,Status=@Status,Balance=@Balance,RecordType=@RecordType,Operator=@Operator,OperateTime=@OperateTime,RelationNo=@RelationNo,Remark=@Remark
where  Id=@Id ";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("Amount", businessBalanceRecord.Amount);
            dbParameters.AddWithValue("Status", businessBalanceRecord.Status);
            dbParameters.AddWithValue("Balance", businessBalanceRecord.Balance);
            dbParameters.AddWithValue("RecordType", businessBalanceRecord.RecordType);
            dbParameters.AddWithValue("Operator", businessBalanceRecord.Operator);
            dbParameters.AddWithValue("OperateTime", businessBalanceRecord.OperateTime);
            dbParameters.AddWithValue("RelationNo", businessBalanceRecord.RelationNo);
            dbParameters.AddWithValue("Remark", businessBalanceRecord.Remark);
            DbHelper.ExecuteNonQuery(SuperMan_Write, updateSql, dbParameters);
        }

        /// <summary>
        /// 根据ID获取对象
        /// <param name="businessId">商户id</param>
        /// </summary>
        public IList<FinanceRecordsDM> GetByBusinessId(long businessId)
        {
            IList<FinanceRecordsDM> models = new List<FinanceRecordsDM>();
            const string querysql = @"
select  Id,BusinessId as UserId,Amount,Status,Balance,RecordType,Operator,OperateTime,WithwardId,RelationNo,Remark
,convert(varchar(4),DATEPART(Year,OperateTime))+'年'+convert(varchar(4),DATEPART(month,OperateTime)) +'月' as YearInfo
from  BusinessBalanceRecord (nolock)
where  BusinessId=@BusinessId 
order by Id desc";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("BusinessId", businessId);
            DataTable dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, querysql, dbParameters));
            if (DataTableHelper.CheckDt(dt))
            {
                models = DataTableHelper.ConvertDataTableList<FinanceRecordsDM>(dt);
            }
            return models;
        }
        #endregion
    }
}

