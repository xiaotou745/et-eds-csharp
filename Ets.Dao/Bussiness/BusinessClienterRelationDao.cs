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
using Ets.Model.DataModel.Bussiness;
using Ets.Model.DomainModel.Bussiness;
using Ets.Model.ParameterModel.Bussiness;
using ETS.Util;
using Ets.Model.ParameterModel.Order;

namespace Ets.Dao.Bussiness
{
    /// <summary>
    /// 商户绑定骑士关系dao  caoheyang 20150608
    /// </summary>
    public class BusinessClienterRelationDao : DaoBase
    {
        /// <summary> 
        /// 根据骑士id查询骑士绑定商家列表   caoheyang 20150608
        /// </summary>
        public PageInfo<BCRelationGetByClienterIdDM> GetByClienterId(BCRelationGetByClienterIdPM model)
        {
            string where = string.Format(" ClienterId={0}", model.ClienterId);
            return new PageHelper().GetPages<BCRelationGetByClienterIdDM>(SuperMan_Read, model.PageIndex, where, "a.Id desc", "a.BusinessId,a.ClienterId,b.Name as BusinessName,b.PhoneNo as BusinessPhoneNo,b.Address as BusinessAddress,a.IsBind,a.UpdateBy,a.UpdateTime,c.TrueName as ClienterName",
                "dbo.BusinessClienterRelation a ( nolock )  join dbo.business b ( nolock ) on a.BusinessId = b.Id join dbo.clienter c ( nolock ) on c.Id = a.ClienterId", SystemConst.PageSize, true);
        }

        /// <summary>
        /// 增加一条记录
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150609</UpdateTime>
        /// <param name="businessBalanceRecord"></param>
        /// <returns></returns>
        public int Insert(BusinessClienterRelation businessClienterRelation)
        {
            const string insertSql = @"
insert into BusinessClienterRelation(BusinessId,ClienterId,CreateBy,UpdateBy)
values(@BusinessId,@ClienterId,@CreateBy,@UpdateBy)

select @@IDENTITY";

            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("BusinessId", businessClienterRelation.BusinessId);
            dbParameters.AddWithValue("ClienterId", businessClienterRelation.ClienterId);           
            dbParameters.AddWithValue("CreateBy", businessClienterRelation.CreateBy);       
            dbParameters.AddWithValue("UpdateBy", businessClienterRelation.UpdateBy);         

            object result = DbHelper.ExecuteScalar(SuperMan_Write, insertSql, dbParameters);
            if (result == null)
            {
                return 0;
            }
            return ParseHelper.ToInt(result.ToString());
           
        }

        /// <summary>
        /// 更新一条记录
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150609</UpdateTime>
        /// <param name="businessBalanceRecord">参数实体</param>
        public void Update(BusinessClienterRelationPM businessClienterRelation)
        {
            const string updateSql = @"
update  BusinessClienterRelation
set  IsEnable=1,IsBind=1
where  BusinessId=@BusinessId  and ClienterId=@ClienterId";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("BusinessId", businessClienterRelation.BusinessId);
            dbParameters.AddWithValue("ClienterId", businessClienterRelation.ClienterId); 
            DbHelper.ExecuteNonQuery(SuperMan_Write, updateSql, dbParameters);
        }

        /// <summary>
        /// 获取商户骑士对应关系     
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150609</UpdateTime>  
        /// <param name="id"></param>
        /// <returns></returns>
        public BusinessClienterRelation GetDetails(BusinessClienterRelationPM businessClienterRelation)
        {
            BusinessClienterRelation model = new BusinessClienterRelation();
            
            string querySql = @" 
select Id,BusinessId,ClienterId,IsEnable,CreateBy,CreateTime,UpdateBy,UpdateTime,IsBind
 FROM   dbo.[business] (nolock ) 
 WHERE  BusinessId=@BusinessId  and ClienterId=@ClienterId";

            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("BusinessId", businessClienterRelation.BusinessId);
            dbParameters.AddWithValue("ClienterId", businessClienterRelation.ClienterId);

            DataTable dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, querySql, dbParameters));
            if (DataTableHelper.CheckDt(dt))
            {
                model = MapRows<BusinessClienterRelation>(dt)[0];
            }
            return model;
        }

    }
}
