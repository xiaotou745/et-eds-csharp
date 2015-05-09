using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Dao.Finance;
using Ets.Model.DataModel.Finance;
using Ets.Model.ParameterModel.Finance;
using Ets.Service.IProvider.Finance;

namespace Ets.Service.Provider.Finance
{
    /// <summary>
    ///骑士提现实体类  业务逻辑类ClienterWithdrawFormProvider 的摘要说明。
    /// Generate By: tools.etaoshi.com caoheyang
    /// Generate Time: 2015-05-09 16:00:11
    /// </summary>
    public class ClienterWithdrawFormProvider : IClienterWithdrawFormProvider
    {
        private readonly ClienterWithdrawFormDao _clienterWithdrawFormDao = new ClienterWithdrawFormDao();
        public ClienterWithdrawFormProvider()
        {
        }
        /// <summary>
        /// 新增一条记录
        ///<param name="clienterWithdrawForm">要新增的对象</param>
        /// </summary>
        public long Create(ClienterWithdrawForm clienterWithdrawForm)
        {
            return _clienterWithdrawFormDao.Insert(clienterWithdrawForm);
        }
        /// <summary>
        /// 修改一条记录
        ///<param name="clienterWithdrawForm">要修改的对象</param>
        /// </summary>
        public void Modify(ClienterWithdrawForm clienterWithdrawForm)
        {
            _clienterWithdrawFormDao.Update(clienterWithdrawForm);
        }


        /// <summary>
        /// 删除一条记录
        /// <param name="id">id</param>
        /// </summary>
        public void Remove(long id)
        {
            _clienterWithdrawFormDao.Delete(id);
        }
        /// <summary>
        /// 根据Id得到一个对象实体
        /// <param name="id">id</param>
        /// </summary>
        public ClienterWithdrawForm GetById(long id)
        {
            return _clienterWithdrawFormDao.GetById(id);
        }
        /// <summary>
        /// 查询方法 
        /// </summary>
        /// <param name="clienterWithdrawFormPm">参数实体</param>
        /// <returns></returns>
        public IList<ClienterWithdrawForm> Query(ClienterWithdrawFormPM clienterWithdrawFormPm)
        {
            return _clienterWithdrawFormDao.Query(clienterWithdrawFormPm);
        }
    }
}
