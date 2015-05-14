using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Model.DataModel.Order;
using Ets.Model.ParameterModel.Order;
using Ets.Service.IProvider.Order;
using Ets.Dao.Order;
using Ets.Model.DomainModel.Order;
namespace Ets.Service.Provider.Order
{
    /// <summary>
    /// 订单子表 业务逻辑类IOrderChildService 的摘要说明。
    /// Generate By: tools.etaoshi.com
    /// Generate Time: 2015-05-09 18:48:39
    /// </summary>

    public class OrderChildProvider:IOrderChildProvider
    {
        private readonly OrderChildDao _orderChildDao = new OrderChildDao();
        public OrderChildProvider()
		{
		}
        /// <summary>
        /// 新增一条记录
        ///<param name="clienterFinanceAccount">要新增的对象</param>
        /// </summary>
        public long Create(OrderChild orderChild)
        {
            return _orderChildDao.Insert(orderChild);
        }

        /// <summary>
        /// 修改一条记录
        ///<param name="clienterFinanceAccount">要修改的对象</param>
        /// </summary>
        public void Modify(OrderChild orderChild)
        {
            _orderChildDao.Update(orderChild);
        }

        /// <summary>
        /// 删除一条记录
        /// <param name="id">id</param>
        /// </summary>
        public void Remove(int id)
        {
            _orderChildDao.Delete(id);
        }

        /// <summary>
        /// 根据Id得到一个对象实体
        /// <param name="id">id</param>
        /// </summary>
        public OrderChild GetById(int id)
        {
            return _orderChildDao.GetById(id);
        }
   

        /// <summary>
        /// 获取子订单列表
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150512</UpdateTime>
        /// <param name="orderId">订单Id</param>
        /// <returns></returns>
        public List<OrderChildInfo> GetByOrderId(int orderId)
        {
            return _orderChildDao.GetByOrderId(orderId);
        }
    }
}
