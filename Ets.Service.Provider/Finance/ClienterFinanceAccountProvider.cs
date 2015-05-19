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
    /// 骑士金融账号 业务逻辑类接口 ClienterFinanceAccountProvider 的摘要说明。
    /// Generate By: tools.etaoshi.com caoheyang
    /// Generate Time: 2015-05-09 15:57:21 
    /// </summary>
    public class ClienterFinanceAccountProvider : IClienterFinanceAccountProvider
    {
        private readonly ClienterFinanceAccountDao _clienterFinanceAccountDao = new ClienterFinanceAccountDao();
        public ClienterFinanceAccountProvider()
        {
        }
        /// <summary>
        /// 新增一条记录
        ///<param name="clienterFinanceAccount">要新增的对象</param>
        /// </summary>
        public int Create(ClienterFinanceAccount clienterFinanceAccount)
        {
            return _clienterFinanceAccountDao.Insert(clienterFinanceAccount);
        }
        /// <summary>
        /// 修改一条记录
        ///<param name="clienterFinanceAccount">要修改的对象</param>
        /// </summary>
        public void Modify(ClienterFinanceAccount clienterFinanceAccount)
        {
            _clienterFinanceAccountDao.Update(clienterFinanceAccount);
        }


        /// <summary>
        /// 删除一条记录
        /// <param name="id">id</param>
        /// </summary>
        public void Remove(int id)
        {
            _clienterFinanceAccountDao.Delete(id);
        }
        /// <summary>
        /// 根据Id得到一个对象实体
        /// <param name="id">id</param>
        /// </summary>
        public ClienterFinanceAccount GetById(int id)
        {
            return _clienterFinanceAccountDao.GetById(id);
        }
        /// <summary>
        /// 查询方法 
        /// </summary>
        /// <param name="clienterFinanceAccountPm">参数实体</param>
        /// <returns></returns>
        public IList<ClienterFinanceAccount> Query(ClienterFinanceAccountPM clienterFinanceAccountPm)
        {
            return _clienterFinanceAccountDao.Query(clienterFinanceAccountPm);
        }
    }
}
