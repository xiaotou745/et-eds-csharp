using System.Collections.Generic;
using Ets.Model.DataModel.Finance;
using Ets.Model.ParameterModel.Finance;

namespace Ets.Service.IProvider.Finance
{
    /// <summary>
    /// 骑士提现日志  业务逻辑类IClienterWithdrawLogService 的摘要说明。
	/// Generate By: tools.etaoshi.com
	/// Generate Time: 2015-05-09 16:02:12
	/// </summary>
    public interface IClienterWithdrawLogProvider
    {
        /// <summary>
        /// 新增一条记录
        ///<param name="clienterWithdrawLog">要新增的对象</param>
        /// </summary>
        long Create(ClienterWithdrawLog clienterWithdrawLog);

        /// <summary>
        /// 修改一条记录
        ///<param name="clienterWithdrawLog">要修改的对象</param>
        /// </summary>
        void Modify(ClienterWithdrawLog clienterWithdrawLog);

        /// <summary>
        /// 删除一条记录
        /// <param name="id">id</param>
        /// </summary>
        void Remove(long id);

        /// <summary>
        /// 根据Id得到一个对象实体
        /// <param name="id">id</param>
        /// </summary>
        ClienterWithdrawLog GetById(long id);

        /// <summary>
        /// 查询方法
        /// <param name="clienterWithdrawLogPm">参数实体</param>
        /// </summary>
        IList<ClienterWithdrawLog> Query(ClienterWithdrawLogPM clienterWithdrawLogPm);

    }
}
