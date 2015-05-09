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
    ///  骑士余额流水表 业务逻辑类ClienterBalanceRecordProvider 的摘要说明。
    /// Generate By: tools.etaoshi.com
    /// Generate Time: 2015-05-09 14:59:36
    /// </summary>

    public class ClienterBalanceRecordProvider:IClienterBalanceRecordProvider
    {
        private readonly ClienterBalanceRecordDao _clienterBalanceRecordDao= new ClienterBalanceRecordDao();
        public ClienterBalanceRecordProvider()
		{
		}
        /// <summary>
        /// 新增一条记录
        ///<param name="clienterBalanceRecord">要新增的对象</param>
        /// </summary>
		public long Create(ClienterBalanceRecord clienterBalanceRecord)
		{
            return _clienterBalanceRecordDao.Insert(clienterBalanceRecord);
		}

        /// <summary>
        /// 修改一条记录
        ///<param name="clienterBalanceRecord">要修改的对象</param>
        /// </summary>
        public void Modify(ClienterBalanceRecord clienterBalanceRecord)
		{
            _clienterBalanceRecordDao.Update(clienterBalanceRecord);
		}


        /// <summary>
        /// 删除一条记录
        /// </summary>
        /// <param name="id">id</param>
		public void Remove(long id)
		{
            _clienterBalanceRecordDao.Delete(id);
		}

        /// <summary>
        /// 根据Id得到一个对象实体
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
		public ClienterBalanceRecord GetById(long id)
		{
            return _clienterBalanceRecordDao.GetById(id);
		}

        /// <summary>
        /// 查询方法 
        /// </summary>
        /// <param name="clienterBalanceRecordPm">参数实体</param>
        /// <returns></returns>
        public IList<ClienterBalanceRecord> Query(ClienterBalanceRecordPM clienterBalanceRecordPm)
		{
            return _clienterBalanceRecordDao.Query(clienterBalanceRecordPm);
		}

    }
}
