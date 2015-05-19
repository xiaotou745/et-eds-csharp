using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Model.DataModel.Finance;
using Ets.Model.ParameterModel.Finance;

namespace Ets.Service.IProvider.Finance
{	
    /// <summary>
    /// 骑士金融账号 业务逻辑类接口 IClienterFinanceAccountProvider 的摘要说明。
    /// Generate By: tools.etaoshi.com caoheyang
    /// Generate Time: 2015-05-09 15:57:21 
    /// </summary>

    public  interface IClienterFinanceAccountProvider
    {
        /// <summary>
        /// 新增一条记录
        ///<param name="clienterFinanceAccount">要新增的对象</param>
        /// </summary>
        int Create(ClienterFinanceAccount clienterFinanceAccount);

        /// <summary>
        /// 修改一条记录
        ///<param name="clienterFinanceAccount">要修改的对象</param>
        /// </summary>
        void Modify(ClienterFinanceAccount clienterFinanceAccount);

        /// <summary>
        /// 删除一条记录
        /// <param name="id">id</param>
        /// </summary>
        void Remove(int id);

        /// <summary>
        /// 根据Id得到一个对象实体
        /// <param name="id">id</param>
        /// </summary>
        ClienterFinanceAccount GetById(int id);

        /// <summary>
        /// 查询方法
        /// <param name="clienterFinanceAccountPm">参数实体</param>
        /// </summary>
        IList<ClienterFinanceAccount> Query(ClienterFinanceAccountPM clienterFinanceAccountPm);

    }
}
