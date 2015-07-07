using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Dao;
using ETS.Data;
using ETS.Data.Core;
using Ets.Model.DataModel.DeliveryCompany;

namespace Ets.Dao.DeliveryCompany
{
    public class DeliveryCompanyDao : DaoBase
    {
        /// <summary>
        /// 获取物流公司列表
        /// danny-20150706
        /// </summary>
        /// <returns></returns>
        public IList<DeliveryCompanyModel> GetDeliveryCompanyList()
        {
            string sql = @"  SELECT  [Id]
                                    ,[DeliveryCompanyName]
                                    ,[DeliveryCompanyCode]
                                    ,[IsEnable]
                                    ,[SettleType]
                                    ,[ClienterFixMoney]
                                    ,[ClienterSettleRatio]
                                    ,[DeliveryCompanySettleMoney]
                                    ,[DeliveryCompanyRatio]
                                    ,[ClienterQuantity]
                            FROM [DeliveryCompany] WITH(NOLOCK)
                            WHERE IsEnable=1
                            ORDER BY Id;";
            var dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql);
            return MapRows<DeliveryCompanyModel>(dt);
        }
    }
}
