using ETS.Const;
using Ets.Dao.User;
using Ets.Model.Common;
using System.Collections.Generic;
using Ets.Model.DataModel.Business;
using ETS.Transaction.Common;
using ETS.Transaction;
using Ets.Model.DomainModel.GlobalConfig;
using Ets.Service.IProvider.Business;
using Ets.Dao.Business;
using ETS.Util;
using Ets.Dao.Finance;
using Ets.Model.ParameterModel.Business;
using Ets.Model.ParameterModel.Finance;
using Ets.Model.DataModel.Finance;
using Ets.Model.ParameterModel.Business;
namespace Ets.Service.Provider.Business
{
    /// <summary>
    /// 商家分组业务逻辑接口实现类 
    /// </summary>
    public class GroupBusinessProvider : IGroupBusinessProvider
    {
        private readonly GroupBusinessDao groupBusinessDao = new GroupBusinessDao();

        readonly BusinessBalanceRecordDao businessBalanceRecordDao = new BusinessBalanceRecordDao();
        /// <summary>
        /// 更新集团余额
        /// 胡灵波
        /// 2015年9月14日 11:23:12
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="myOrderInfo"></param>
        public void UpdateGBalance(GroupBusinessPM groupBusinessPM)
        {
            //更集团余额
            groupBusinessDao.UpdateAmount(new UpdateForWithdrawPM()
            {
                Id = groupBusinessPM.GroupId,
                Money = groupBusinessPM.GroupAmount
            });

            //更新商户余额流水          
            businessBalanceRecordDao.InsertGroupRecord(new BusinessBalanceRecord()
            {    
                BusinessId=groupBusinessPM.BusinessId,
                GroupId = groupBusinessPM.GroupId,
                Amount = groupBusinessPM.GroupAmount,
                Status = groupBusinessPM.Status,
                RecordType = groupBusinessPM.RecordType,
                Operator = groupBusinessPM.Operator,
                WithwardId = groupBusinessPM.WithwardId,
                RelationNo = groupBusinessPM.RelationNo,
                Remark = groupBusinessPM.Remark
            });
        }
    }
}
