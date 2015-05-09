#region
using Ets.Model.DataModel.Finance;
using Ets.Model.ParameterModel.Finance;
using System.Collections.Generic;
#endregion

namespace Ets.Service.IProvider.Finance
{
    /// <summary>
    ///  骑士余额流水表 业务逻辑类IClienterBalanceRecordProvider 的摘要说明。
    /// Generate By: tools.etaoshi.com
    /// Generate Time: 2015-05-09 14:59:36
    /// </summary>
    public interface IClienterBalanceRecordProvider
    {
        /// <summary>
        /// 新增一条记录
        ///<param name="clienterBalanceRecord">要新增的对象</param>
        /// </summary>
        long Create(ClienterBalanceRecord clienterBalanceRecord);

        /// <summary>
        /// 修改一条记录
        ///<param name="clienterBalanceRecordPm">要修改的对象</param>
        /// </summary>
        void Modify(ClienterBalanceRecord clienterBalanceRecordPm);

        /// <summary>
        /// 删除一条记录
        /// </summary>
        /// <param name="id">id</param>
        void Remove(long id);

        /// <summary>
        /// 根据Id得到一个对象实体
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        ClienterBalanceRecord GetById(long id);

        /// <summary>
        /// 查询方法 
        /// </summary>
        /// <param name="clienterBalanceRecordPm">参数实体</param>
        /// <returns></returns>
        IList<ClienterBalanceRecord> Query(ClienterBalanceRecordPM clienterBalanceRecordPm);

    }

}
