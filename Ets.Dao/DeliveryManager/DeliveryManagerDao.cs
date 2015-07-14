using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Dao;
using ETS.Data.PageData;
using Ets.Model.DataModel.Clienter;
using Ets.Model.ParameterModel.Clienter;

namespace Ets.Dao.DeliveryManager
{
    public class DeliveryManagerDao : DaoBase
    {
        /// <summary>
        /// 物流订单管理-获取骑士列表
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public PageInfo<T> GetClienterList<T>(ClienterSearchCriteria criteria)
        {
            string columnList = @"
                 C.[Id]			--ID
                ,C.[PhoneNo]	--电话
                ,ISNULL(C.[TrueName],'') AS TrueName	--姓名
                ,ISNULL(C.[IDCard],'') AS IDCard	--身份照号
                ,ISNULL(C.[PicUrl],'') AS PicUrl	--照片
                ,C.[Status]		--审核状态
                ,C.[InsertTime]	--申请时间
                ,C.[WorkStatus]	--工作状态 
                ,ISNULL(C.PicWithHandUrl,'') AS PicWithHandUrl
                ";
            var sbSqlWhere = new StringBuilder(" 1=1 ");
            if (!string.IsNullOrEmpty(criteria.clienterName))
            {
                sbSqlWhere.AppendFormat(" AND C.TrueName='{0}' ", criteria.clienterName);
            }
            if (!string.IsNullOrEmpty(criteria.clienterPhone))
            {
                sbSqlWhere.AppendFormat(" AND C.PhoneNo='{0}' ", criteria.clienterPhone);
            }
            if (criteria.Status != -1)
            {
                sbSqlWhere.AppendFormat(" AND C.Status={0} ", criteria.Status);
            }
            if (!string.IsNullOrEmpty(criteria.businessCity))
            {
                sbSqlWhere.AppendFormat(" AND C.City='{0}' ", criteria.businessCity.Trim());
            }
            if (!string.IsNullOrWhiteSpace(criteria.deliveryCompany) && criteria.deliveryCompany != "0")
            {
                sbSqlWhere.AppendFormat(" AND C.DeliveryCompanyId={0} ", criteria.deliveryCompany);
            }
            string tableList = @" clienter C WITH (NOLOCK) ";
            string orderByColumn = " C.Id DESC";
            return new PageHelper().GetPages<T>(SuperMan_Read, criteria.PageIndex, sbSqlWhere.ToString(), orderByColumn, columnList, tableList, criteria.PageSize, true);
        }
    }
}
