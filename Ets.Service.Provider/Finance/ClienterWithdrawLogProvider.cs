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
    /// 骑士提现日志  业务逻辑类ClienterWithdrawLogProvider 的摘要说明。
    /// Generate By: tools.etaoshi.com
    /// Generate Time: 2015-05-09 16:02:12
    /// </summary>
    public class ClienterWithdrawLogProvider:IClienterWithdrawLogProvider
    {
          private readonly ClienterWithdrawLogDao _clienterWithdrawLogDao = new ClienterWithdrawLogDao();
          public ClienterWithdrawLogProvider()
        {
        }
        /// <summary>
        /// 新增一条记录
          ///<param name="clienterWithdrawLog">要新增的对象</param>
        /// </summary>
          public long Create(ClienterWithdrawLog clienterWithdrawLog)
        {
            return _clienterWithdrawLogDao.Insert(clienterWithdrawLog);
        }
        /// <summary>
        /// 修改一条记录
          ///<param name="clienterWithdrawLog">要修改的对象</param>
        /// </summary>
          public void Modify(ClienterWithdrawLog clienterWithdrawLog)
        {
            _clienterWithdrawLogDao.Update(clienterWithdrawLog);
        }


        /// <summary>
        /// 删除一条记录
        /// <param name="id">id</param>
        /// </summary>
        public void Remove(long id)
        {
            _clienterWithdrawLogDao.Delete(id);
        }
        /// <summary>
        /// 根据Id得到一个对象实体
        /// <param name="id">id</param>
        /// </summary>
        public ClienterWithdrawLog GetById(long id)
        {
            return _clienterWithdrawLogDao.GetById(id);
        }
        /// <summary>
        /// 查询方法 
        /// </summary>
        /// <param name="clienterWithdrawLogPm">参数实体</param>
        /// <returns></returns>
        public IList<ClienterWithdrawLog> Query(ClienterWithdrawLogPM clienterWithdrawLogPm)
        {
            return _clienterWithdrawLogDao.Query(clienterWithdrawLogPm);
        }
    }
}
