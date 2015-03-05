using SuperManDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperManCommonModel.Models;
using SuperManCore;
using SuperManCommonModel.Entities;
using SuperManWebApi.Models.Business;
using SuperManCore.Common;
using SuperManCommonModel;
//using ifunction.JPush;
using SuperManBusinessLogic.CommonLogic;
namespace SuperManBusinessLogic.Order_Logic
{
    public class OrderLogic
    {
        //private static OrderLogic _orderLogic=null;
        //public static OrderLogic Instance()
        //{
        //    if (_orderLogic == null)
        //    {
        //        _orderLogic = new OrderLogic();
        //    }
        //    return _orderLogic;
        //}
        private volatile static OrderLogic _instance = null;
        private static readonly object lockHelper = new object();
        private OrderLogic() { }
        public static OrderLogic orderLogic()
        {
            if (_instance == null)
            {
                lock (lockHelper)
                {
                    if (_instance == null)
                        _instance = new OrderLogic();
                }
            }
            return _instance;
        }
        public PagedList<OrderModel> resultModel { get; set; }
        /// <summary>
        /// 根据参数获取订单
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public OrderManage GetOrders(OrderSearchCriteria criteria)
        {
            using (var db = new supermanEntities())
            {
                var items = db.order.AsQueryable();
                if (!string.IsNullOrWhiteSpace(criteria.businessName))
                {
                    items = items.Where(p => p.business.Name == criteria.businessName);
                }
                if (!string.IsNullOrWhiteSpace(criteria.businessPhone))
                {
                    items = items.Where(p => p.business.PhoneNo == criteria.businessPhone);
                }
                if (!string.IsNullOrWhiteSpace(criteria.orderId))
                {
                    items = items.Where(p => p.OrderNo == criteria.orderId);
                }
                if (!string.IsNullOrWhiteSpace(criteria.OriginalOrderNo))
                {
                    items = items.Where(p => p.OriginalOrderNo == criteria.OriginalOrderNo);
                }
                if (criteria.orderStatus != -1)
                {
                    items = items.Where(p => p.Status == criteria.orderStatus);
                }
                if (!string.IsNullOrWhiteSpace(criteria.superManName))
                {
                    var superman = db.clienter.FirstOrDefault(p => p.TrueName == criteria.superManName);
                    if (superman != null)
                        items = items.Where(p => p.clienterId == superman.Id);
                }
                if (!string.IsNullOrWhiteSpace(criteria.superManPhone))
                {
                    var superman = db.clienter.FirstOrDefault(p => p.PhoneNo == criteria.superManPhone);
                    if (superman != null)
                        items = items.Where(p => p.clienterId == superman.Id);
                }
                if (!string.IsNullOrWhiteSpace(criteria.orderPubStart))
                {
                    var dt = DateTime.Parse(criteria.orderPubStart);
                    items = items.Where(p => p.PubDate.Value >= dt);
                }
                if (!string.IsNullOrWhiteSpace(criteria.orderPubEnd))
                {
                    var dt = DateTime.Parse(criteria.orderPubEnd);
                    items = items.Where(p => p.PubDate.Value <= dt);
                }
                if (criteria.GroupId != null)
                {
                    items = items.Where(p => p.business.GroupId == criteria.GroupId);
                }
                var pagedQuery = new OrderManage();
                var orderModel = OrderModelTranslator.Instance.Translate(items.ToList());
                resultModel = new PagedList<OrderModel>(orderModel, criteria.PagingRequest.PageIndex, criteria.PagingRequest.PageSize);

                var orderlists = new OrderManageList(resultModel.ToList(), resultModel.PagingResult);
                pagedQuery.orderManageList = orderlists;
                return pagedQuery;
            }
        }

        /// <summary>
        /// 添加一个订单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddModel(order model)
        {
            bool result = false;
            try
            {
                using (var db = new supermanEntities())
                {
                    if (model != null)
                    {
                        business businessmodel = db.business.Where(p => p.Id == model.businessId).FirstOrDefault();
                        if (businessmodel == null)
                            return result;
                        db.order.Add(model);
                        int g = db.SaveChanges();
                        if (g != 0)
                        {
                            result = true;
                            if (model.Status != 4)  //订单状态是 4待审核 时不触发极光推送
                            {
                                Push.PushMessage(0, "有新订单了！", "有新的订单可以抢了！", "有新的订单可以抢了！", string.Empty, businessmodel.City); //激光推送
                            }                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogWriter("添加订单异常", new { ex = ex, model = model });
            }
            return result;
        }

        /// <summary>
        /// B端接口 查询当前登录商户的所有订单  PagedList
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public PagedList<order> GetOrders(BusiOrderSearchCriteria criteria)
        {
            using (var dbEntity = new supermanEntities())
            {
                var query = dbEntity.order.AsQueryable();
                if (criteria.userId != 0)
                {
                    query = query.Where(i => i.businessId == criteria.userId);
                }
                if (criteria.Status != null)
                {
                    if (criteria.Status == 4)
                    {
                        query = query.Where(i => i.Status == ConstValues.ORDER_NEW || i.Status == ConstValues.ORDER_ACCEPT);
                    }
                    else
                    {
                        query = query.Where(i => i.Status == criteria.Status);
                    }
                }
                query = query.OrderByDescending(i => i.Id);
                var result = new PagedList<order>(query.ToList(), criteria.PagingRequest.PageIndex, criteria.PagingRequest.PageSize);
                return result;
            }
        }

        /// <summary>
        /// 根据订单号获取订单
        /// </summary>
        /// <param name="OrderId"></param>
        /// <returns></returns>
        public order GetOrderById(string OrderNo)
        {
            using (var dbEntity = new supermanEntities())
            {
                var query = dbEntity.order.Where(p => p.OrderNo == OrderNo);
                return query.FirstOrDefault();
            }
        }

        /// <summary>
        /// 更新订单状态
        /// </summary>
        /// <param name="order"></param>
        /// <param name="orderStatus"></param>
        public bool UpdateOrder(order order, OrderStatus orderStatus)
        {
            bool bResult = false;
            using (var db = new supermanEntities())
            {
                var query = db.order.Where(p => p.OrderNo == order.OrderNo && p.Status == ConstValues.ORDER_NEW).FirstOrDefault();
                if (query != null)
                {
                    if (orderStatus == OrderStatus.订单已取消)
                    {
                        query.Status = ConstValues.ORDER_CANCEL;
                    }
                    int i = db.SaveChanges();
                    if (i == 1)
                    {
                        bResult = true;
                    }
                }
            }
            return bResult;
        }

        /// <summary>
        /// 更新订单状态，通过第三方订单号 和 订单 来源更新
        /// </summary>
        /// <param name="order"></param>
        /// <param name="orderStatus"></param>
        public bool UpdateOrder(string oriOrderNo, int orderFrom, OrderStatus orderStatus)
        {
            bool bResult = false;
            try
            {
                using (var db = new supermanEntities())
                {
                    var query = db.order.Where(p => p.OriginalOrderNo == oriOrderNo && p.OrderFrom == orderFrom).FirstOrDefault();
                    if (query != null)
                    {
                        if ((OrderStatus)query.Status == OrderStatus.订单已取消)
                        {
                            bResult = true;
                        }
                        else
                        {
                            query.Status = ConstValues.ORDER_CANCEL;
                            int i = db.SaveChanges();
                            if (i == 1)
                            {
                                Push.PushMessage(0, "订单提醒", "订单被客户取消了！", "订单被客户取消了！", query.clienterId.Value.ToString(), string.Empty);
                                bResult = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogWriter("第三方取消订单异常：", new { ex = ex });
            }
            return bResult;
        }

        /// <summary>
        /// 更新订单配送骑士和订单佣金
        /// </summary>
        /// <param name="order"></param>
        public bool UpdateOrderInfo(order order)
        {
            bool bResult = false;
            using (var db = new supermanEntities())
            {
                var query = db.order.Where(p => p.OrderNo == order.OrderNo).FirstOrDefault();
                if (query != null)
                {
                    query.OrderCommission = order.OrderCommission;
                    int i = db.SaveChanges();
                    if (i == 1)
                    {
                        bResult = true;
                    }
                }
            }
            return bResult;
        }

        /// <summary>
        /// 订单统计
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public OrderCountManageList GetOrderCount(HomeCountCriteria criteria)
        {
            IQueryable<OrderCountModel> items = null;
            using (var db = new supermanEntities())
            {
                if (criteria.searchType == 1)//当天
                {
                    items = from m in db.business
                            join n in db.order
                            on m.Id equals n.businessId
                            where n.PubDate.Value.Day == DateTime.Now.Day
                            group n by m.district into g
                            select new OrderCountModel
                            {
                                district = g.FirstOrDefault().business.district,
                                orderCount = g.Count(),
                                distribSubsidy = g.Sum(i => i.DistribSubsidy.Value),
                                websiteSubsidy = g.Sum(i => i.WebsiteSubsidy.Value),
                                orderCommission = g.Sum(i => i.OrderCommission.Value),
                                deliverAmount = g.Sum(i => i.DistribSubsidy.Value + i.WebsiteSubsidy.Value + i.OrderCommission.Value),
                                orderAmount = g.Sum(i => i.Amount).Value
                            };
                }
                else if (criteria.searchType == 2) //本周
                {
                    DateTime givenDate = DateTime.Today;
                    DateTime startOfWeek = givenDate.AddDays(-1 * (int)givenDate.DayOfWeek);
                    DateTime endOfWeek = startOfWeek.AddDays(7);
                    items = from m in db.business
                            join n in db.order
                            on m.Id equals n.businessId
                            where n.PubDate.Value >= startOfWeek && n.PubDate.Value < endOfWeek
                            group n by m.district into g
                            select new OrderCountModel
                            {
                                district = g.FirstOrDefault().business.district,
                                orderCount = g.Count(),
                                distribSubsidy = g.Sum(i => i.DistribSubsidy.Value),
                                websiteSubsidy = g.Sum(i => i.WebsiteSubsidy.Value),
                                orderCommission = g.Sum(i => i.OrderCommission.Value),
                                deliverAmount = g.Sum(i => i.DistribSubsidy.Value + i.WebsiteSubsidy.Value + i.OrderCommission.Value),
                                orderAmount = g.Sum(i => i.Amount).Value
                            };
                }
                else if (criteria.searchType == 3) //本月
                {
                    items = from m in db.business
                            join n in db.order
                            on m.Id equals n.businessId
                            where n.PubDate.Value.Month == DateTime.Now.Month
                            group n by m.district into g
                            select new OrderCountModel
                            {
                                district = g.FirstOrDefault().business.district,
                                orderCount = g.Count(),
                                distribSubsidy = g.Sum(i => i.DistribSubsidy.Value),
                                websiteSubsidy = g.Sum(i => i.WebsiteSubsidy.Value),
                                orderCommission = g.Sum(i => i.OrderCommission.Value),
                                deliverAmount = g.Sum(i => i.DistribSubsidy.Value + i.WebsiteSubsidy.Value + i.OrderCommission.Value),
                                orderAmount = g.Sum(i => i.Amount).Value
                            };
                }
                else
                {
                    items = from m in db.business
                            join n in db.order
                            on m.Id equals n.businessId
                            group n by m.district into g
                            select new OrderCountModel
                            {
                                district = g.FirstOrDefault().business.district,
                                orderCount = g.Count(),
                                distribSubsidy = g.Sum(i => i.DistribSubsidy.Value),
                                websiteSubsidy = g.Sum(i => i.WebsiteSubsidy.Value),
                                orderCommission = g.Sum(i => i.OrderCommission.Value),
                                deliverAmount = g.Sum(i => i.DistribSubsidy.Value + i.WebsiteSubsidy.Value + i.OrderCommission.Value),
                                orderAmount = g.Sum(i => i.Amount).Value
                            };
                }
                OrderCountManageList pagedQuery = null;
                try
                {
                    var resultModel = new PagedList<OrderCountModel>(items.ToList(), criteria.PagingRequest.PageIndex, criteria.PagingRequest.PageSize);
                    var businesslists = new OrderCountManageList(resultModel.ToList(), resultModel.PagingResult);
                      pagedQuery = businesslists;
                }
                catch (Exception ex)
                {
                    LogHelper.LogWriter("获取订单XX异常", new { ex = ex });
                    
                }
                return pagedQuery;
            }
        }

        public HomeCountTitleModel GetHomeCountTitle()
        {
            var model = new HomeCountTitleModel();

            using (var db = new supermanEntities())
            {
                var busi = db.business.Where(p => p.Status == ConstValues.BUSINESS_AUDITPASS);
                var ApplySuperMan = db.clienter.Where(p => p.Status != ConstValues.CLIENTER_AUDITCANCEL);
                var AuditPassSuperMan = db.clienter.Where(p => p.Status == ConstValues.CLIENTER_AUDITPASS);
                var TodayOrders = db.order.Where(p => p.PubDate.Value == DateTime.Now);//今日订单为今日发布的订单
                model.SignBusiness = busi.Count();
                model.ApplySuperMan = ApplySuperMan.Count();
                model.AuditPassSuperMan = AuditPassSuperMan.Count();
                model.TodayOrders = TodayOrders.Count();
                model.TodayOrdersAmount = TodayOrders.Count() == 0 ? 0.00m : TodayOrders.Sum(p => p.Amount).Value;
            }
            return model;
        }
        /// <summary>
        /// 根据
        /// </summary>
        /// <param name="orderNO">第三方平台的原订单号</param>
        /// <param name="orderFrom">订单来源</param>
        /// <returns></returns>
        public order GetOrderByOrderNoAndOrderFrom(string orderNO, int orderFrom)
        {
            using (var dbEntity = new supermanEntities())
            {
                var query = dbEntity.order.Where(p => p.OriginalOrderNo == orderNO && p.OrderFrom == orderFrom);
                return query.FirstOrDefault();
            }
        }
    }
}
