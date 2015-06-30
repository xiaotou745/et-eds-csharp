using System.Collections.Generic;
using Ets.Model.DataModel.Finance;
using Ets.Model.ParameterModel.Finance;

namespace Ets.Service.IProvider.Finance
{
    /// <summary>
    ///骑士提现实体类  业务逻辑类IClienterWithdrawFormProvider 的摘要说明。
    /// Generate By: tools.etaoshi.com caoheyang
    /// Generate Time: 2015-05-09 16:00:11
    /// </summary>
    public interface IClienterWithdrawFormProvider
    {
        /// <summary>
        /// 新增一条记录
        ///<param name="clienterWithdrawForm">要新增的对象</param>
        /// </summary>
        long Create(ClienterWithdrawForm clienterWithdrawForm);

        /// <summary>
        /// 修改一条记录
        ///<param name="clienterWithdrawForm">要修改的对象</param>
        /// </summary>
        void Modify(ClienterWithdrawForm clienterWithdrawForm);

        /// <summary>
        /// 删除一条记录
        /// <param name="id">id</param>
        /// </summary>
        void Remove(long id);

        /// <summary>
        /// 根据Id得到一个对象实体
        /// <param name="id">id</param>
        /// </summary>
        ClienterWithdrawForm GetById(long id);

        /// <summary>
        /// 查询方法
        /// <param name="clienterWithdrawFormPm">参数实体</param>
        /// </summary>
        IList<ClienterWithdrawForm> Query(ClienterWithdrawFormPM clienterWithdrawFormPm);

    }
}
