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
using ETS.Data.Generic;
using ETS.Enums;
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
select  Id,ClienterId as UserId,Amount,Status,Balance,RecordType,Operator,OperateTime,WithwardId,RelationNo,
Remark,
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
where  ClienterId=@ClienterId  and IsEnable=1
order by Id desc";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("ClienterId", clienterId);
            DataTable dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, querysql, dbParameters));

            foreach (DataRow dataReader in dt.Rows)
            {
                FinanceRecordsDM result = new FinanceRecordsDM();
                
                object obj;
                obj = dataReader["Id"];
                if (obj != null && obj != DBNull.Value)
                {
                    result.Id = long.Parse(obj.ToString());
                }
                obj = dataReader["UserId"];
                if (obj != null && obj != DBNull.Value)
                {
                    result.UserId = int.Parse(obj.ToString());
                }
                obj = dataReader["Amount"];
                if (obj != null && obj != DBNull.Value)
                {
                    result.Amount = decimal.Parse(obj.ToString());
                }
                obj = dataReader["Status"];
                if (obj != null && obj != DBNull.Value)
                {
                    int status= int.Parse(obj.ToString());
                    result.Status = status;

                    Enum enu = (ClienterBalanceRecordStatus)status;
                    result.StatusDescription = EnumExtenstion.GetEnumItem(enu.GetType(), enu).Text;  
                }

                obj = dataReader["Balance"];
                if (obj != null && obj != DBNull.Value)
                {
                    result.Balance = decimal.Parse(obj.ToString());
                }
                obj = dataReader["RecordType"];
                if (obj != null && obj != DBNull.Value)
                {
                    int recordType=int.Parse(obj.ToString());
                    result.RecordType = recordType;

                    Enum enu = (ClienterBalanceRecordRecordType)recordType;
                    result.RecordTypeDescription = EnumExtenstion.GetEnumItem(enu.GetType(), enu).Text;                        
                }

                result.Operator = dataReader["Operator"].ToString();
                obj = dataReader["OperateTime"];
                if (obj != null && obj != DBNull.Value)
                {            
                    result.OperateTime = ParseHelper.ToDatetime(obj.ToString(), DateTime.Now).ToString("yyyy-MM-dd HH:mm");
                }
                obj = dataReader["WithwardId"];
                if (obj != null && obj != DBNull.Value)
                {
                    result.WithwardId = long.Parse(obj.ToString());
                }
                result.RelationNo = dataReader["RelationNo"].ToString();

                obj = dataReader["Remark"].ToString();
                if (obj != null && obj != DBNull.Value)
                {
                    result.Remark = obj.ToString();

                    if (obj.ToString().Length > 8)
                    {
                        result.RemarkDescription = obj.ToString().Substring(0, 8) + "...";
                    }
                    else
                    {
                        result.RemarkDescription = obj.ToString();
                    }     
               }

                obj = dataReader["TimeInfo"];
                if (obj != null && obj != DBNull.Value)
                {
                    result.TimeInfo = obj.ToString();
                }

                obj = dataReader["DateInfo"];
                if (obj != null && obj != DBNull.Value)
                {
                    result.DateInfo = obj.ToString();
                }

                obj = dataReader["MonthInfo"];
                if (obj != null && obj != DBNull.Value)
                {
                    result.MonthInfo = obj.ToString();
                }
                models.Add(result);
            }


            return models;            
           
        }

		#endregion


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
        /// <summary>
        /// 根据订单获取对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ClienterBalanceRecord GetByOrderId(long id)
        {
            ClienterBalanceRecord model = new ClienterBalanceRecord();
            const string querysql = @"
select top 1  Id,ClienterId,Amount,Status,Balance,RecordType,Operator,OperateTime,RelationNo,Remark
from  ClienterBalanceRecord (nolock)
where  WithwardId=@Id and Remark='无效订单'";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("Id", id);

            DataTable dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, querysql, dbParameters));
            if (DataTableHelper.CheckDt(dt))
            {
                model = DataTableHelper.ConvertDataTableList<ClienterBalanceRecord>(dt)[0];
            }
            else
            {
                return null;
            }
            return model;
        }


        #region  Other Members

        /// <summary>
        /// 构造查询条件 
        /// </summary>
        /// <param name="clienterBalanceRecordPm">参数实体</param>
        /// <returns></returns>
        public static string BindQueryCriteria(ClienterBalanceRecordPM clienterBalanceRecordPM)
        {
            var stringBuilder = new StringBuilder(" where 1=1 ");
            if (clienterBalanceRecordPM == null)
            {
                return stringBuilder.ToString();
            }
            //TODO:在此加入查询条件构建代码
            if (clienterBalanceRecordPM.ClienterId > 0)
            {
                stringBuilder.Append(" and ClienterId=" + clienterBalanceRecordPM.ClienterId);
            }

            stringBuilder.Append(" order by id ");

            return stringBuilder.ToString();
        }

        #endregion
    }
}

