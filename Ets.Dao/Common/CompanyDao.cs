using System.Collections;
using System.Data;
using ETS.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Data.Core;
using ETS.Extension;
using Ets.Model.Common;
using Ets.Model.DomainModel.DeliveryCompany;

namespace Ets.Dao.Common
{
    public class CompanyDao : DaoBase
    {
        /// <summary>
        /// 获取公司名称和ID(下拉框使用) 2015年7月6日10:48:56 ruhuaiao
        /// </summary>
        /// <returns></returns>
        public IList<CompanyModel> GetCompanyList()
        {
            const string sql =
                @"SELECT DeliveryCompanyName AS CompanyName,[ID] AS CompanyId 
                    FROM DeliveryCompany (NOLOCK)
                    WHERE IsEnable=1";
            DataSet ds = DbHelper.ExecuteDataset(SuperMan_Read, sql);
            IList<CompanyModel> list = new List<CompanyModel>();
            if (!ds.HasData())
            {
                list=MapRows<CompanyModel>(DataTableHelper.GetTable(ds));
            }
            return list;
        }
        /// <summary>
        /// 物流订单管理-骑士管理 下拉框
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public IList<CompanyModel> GetCompanyListByAccountID(int accountId)
        {
            const string sql =
                @"SELECT DC.DeliveryCompanyName AS CompanyName,DC.[ID] AS CompanyId FROM AccountDeliveryRelation AD(NOLOCK)
                    JOIN DeliveryCompany DC(NOLOCK) ON AD.DeliveryCompanyID=DC.Id
                    WHERE AD.AccountId=@AccountId AND AD.IsEnable=1";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.Add("@AccountId", DbType.Int32);
            dbParameters.SetValue("@AccountId",accountId);
            DataSet ds = DbHelper.ExecuteDataset(SuperMan_Read, sql,dbParameters);
            IList<CompanyModel> list = new List<CompanyModel>();
            if (!ds.HasData())
            {
                list = MapRows<CompanyModel>(DataTableHelper.GetTable(ds));
            }
            return list;
        }
    }
}
