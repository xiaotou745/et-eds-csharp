using System.Collections;
using Ets.Dao.Finance;
using Ets.Dao.User;
using ETS.Enums;
using ETS.Extension;
using Ets.Model.Common;
using Ets.Model.DataModel.Business;
using Ets.Model.DataModel.Finance;
using Ets.Model.DomainModel.Finance;
using Ets.Model.ParameterModel.Finance;
using ETS.Security;
using Ets.Service.IProvider.Finance;
using ETS.Data.PageData;
using System;
using System.Collections.Generic;
using System.Text;
using ETS.Transaction;
using ETS.Transaction.Common;
using ETS.Util;
using Ets.Dao.Business;
namespace Ets.Service.Provider.Finance
{
    /// <summary>
    /// 商家余额流水表
    /// </summary>
    /// <UpdateBy>hulingbo</UpdateBy>
    /// <UpdateTime>20150629</UpdateTime>
    public class BusinessBalanceRecordProvider : IBusinessBalanceRecordProvider
    {
        readonly BusinessBalanceRecordDao _businessBalanceRecordDao = new BusinessBalanceRecordDao();
        /// <summary>
        ///  商户交易流水API add by caoheyang 20150512
        /// </summary>
        /// <returns></returns>
        public ResultModel<object> GetRecords(int businessId)
        {
            IList<FinanceRecordsDM> records = _businessBalanceRecordDao.GetByBusinessId(businessId);
            return ResultModel<object>.Conclude(SystemState.Success,
              TranslateRecords(records));
        }

        #region 用户自定义方法
        /// <summary>
        /// 商户交易流水API 信息处理转换 add by caoheyang 20150512
        /// </summary>
        /// <param name="records">原始流水记录</param>
        /// <returns></returns>
        private IList<FinanceRecordsDMList> TranslateRecords(IList<FinanceRecordsDM> records)
        {
            IList<FinanceRecordsDMList> datas = new List<FinanceRecordsDMList>();
            datas.Add(new FinanceRecordsDMList() { MonthIfo = "本月", Datas = new List<FinanceRecordsDM>() });
            int index = 0;
            foreach (var temp in records)
            {
                temp.StatusStr = ((BusinessBalanceRecordStatus)Enum.Parse(typeof(BusinessBalanceRecordStatus),
                    temp.Status.ToString(), false)).GetDisplayText(); //流水状态文本
                temp.RecordTypeStr =
                    ((BusinessBalanceRecordRecordType)Enum.Parse(typeof(BusinessBalanceRecordRecordType),
                        temp.RecordType.ToString(), false)).GetDisplayText(); //交易类型文本
                if (datas[index].MonthIfo == temp.MonthInfo)
                {
                    datas[index].Datas.Add(temp);
                }
                else
                {
                    datas.Add(new FinanceRecordsDMList() { MonthIfo = temp.MonthInfo, Datas = new List<FinanceRecordsDM>() });
                    index = index + 1;
                    datas[index].Datas.Add(temp);
                }
            }
            return datas;
        }

        #endregion
    }
}
