using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Model.DataModel.Order;
using Ets.Model.ParameterModel.Order;
using Ets.Model.DomainModel.Order;
namespace Ets.Service.IProvider.Order
{
    /// <summary>
    /// 订单明细表 业务逻辑类IOrderChildService 的摘要说明。
    /// Generate By: tools.etaoshi.com
    /// Generate Time: 2015-05-09 18:48:39
    /// </summary>

    public interface IOrderChildProvider
    {
        /// <summary>
        /// 新增一条记录
        ///<param name="clienterFinanceAccount">要新增的对象</param>
        /// </summary>
        long Create(OrderChild orderChild);

        /// <summary>
        /// 修改一条记录
        ///<param name="clienterFinanceAccount">要修改的对象</param>
        /// </summary>
        void Modify(OrderChild orderChild);

        /// <summary>
        /// 删除一条记录
        /// <param name="id">id</param>
        /// </summary>
        void Remove(int id);

        /// <summary>
        /// 根据Id得到一个对象实体
        /// <param name="id">id</param>
        /// </summary>
        OrderChild GetById(int id);

        /// <summary>
        /// 根据订单ID得到一个子订单集合
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>    
        List<OrderChildInfo> GetByOrderId(int orderId);        

        /// <summary>
        /// 查询方法
        /// <param name="clienterFinanceAccountPm">参数实体</param>
        /// </summary>
        IList<OrderChild> Query(OrderChildPM orderChildPM);

    }
}
