using System.Runtime.InteropServices;
using cn.jpush.api.report;
using SuperManBusinessLogic.CommonLogic;
using SuperManCommonModel;
using SuperManCommonModel.Entities;
using SuperManCommonModel.Models;
using SuperManCore;
using SuperManCore.Common;
using SuperManDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SuperManBusinessLogic.C_Logic
{
    public class ClienterLogic
    {        
        
        //public static ClienterLogic clienterLogic()
        //{
        //    return new ClienterLogic();
        //}
        private volatile static ClienterLogic _instance = null;
        private static readonly object lockHelper = new object();
        private ClienterLogic() { }
        public static ClienterLogic clienterLogic()
        {
            if (_instance == null)
            {
                lock (lockHelper)
                {
                    if (_instance == null)
                        _instance = new ClienterLogic();
                }
            }
            return _instance;
        }

        public PagedList<ClienterViewModel> resultModel { get; set; }


        /// <summary>
        /// 根据集团id获取超人列表 add by 平扬 2015.03.2
        /// </summary>
        /// <param name="groupId">集团id</param>
        /// <returns>IList<ClienterModel></returns>
        public IList<ClienterModel> GetClienterModelByGroupID(int groupId)
        {
            using (var db = new supermanEntities())
            { 
                var item =db.clienter.Where(p => p.GroupId == groupId && p.Status==1).ToList();
                return ClienterModelTranslator.Instance.Translate(item);
            }
        }

        /// <summary>
        /// 超人列表查询 add by caohheyang 20150212
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public ClienterManage GetClienteres(ClienterSearchCriteria criteria)
        {
            using (var db = new supermanEntities())
            {
                var items = db.clienter.AsQueryable();
                if (!string.IsNullOrEmpty(criteria.clienterName))
                    items = items.Where(p => p.TrueName == criteria.clienterName);
                if (!string.IsNullOrEmpty(criteria.clienterPhone))
                    items = items.Where(p => p.PhoneNo == criteria.clienterPhone);
                if (criteria.Status != -1)
                    items = items.Where(p => p.Status == criteria.Status);
                if (criteria.GroupId != null)  //集团查询
                    items = items.Where(p => p.GroupId == criteria.GroupId);
                var pagedQuery = new ClienterManage();
                var clienters = new PagedList<clienter>(items.ToList(), criteria.PagingRequest.PageIndex, criteria.PagingRequest.PageSize);
                var businesslists = new ClienterManageList(clienters, clienters.PagingResult);
                pagedQuery.clienterManageList = businesslists;

                return pagedQuery;
            }
        }
        /// <summary>
        /// 按超人分组获取超人
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public ClienterCountManage GetClienteresCount(ClienterSearchCriteria criteria)
        {
            IQueryable<ClienterViewModel> items = null;
            using (var db = new supermanEntities())
            {
                if(criteria.searchType==1)
                {
                    items = from m in db.clienter
                                join n in db.order
                                on m.Id equals n.clienterId
                                where n.PubDate.Value.Day == DateTime.Now.Day
                                group n by new { n.clienterId } into g
                                select new ClienterViewModel
                                {
                                    Id = g.FirstOrDefault().clienterId != null ? g.FirstOrDefault().clienterId.Value : 0,
                                    OrderCount = g.Count(),
                                };
                }
                else if(criteria.searchType==2)
                {
                    DateTime givenDate = DateTime.Today;
                    DateTime startOfWeek = givenDate.AddDays(-1 * (int)givenDate.DayOfWeek);
                    DateTime endOfWeek = startOfWeek.AddDays(7);
                     items = from m in db.clienter
                                join n in db.order
                                on m.Id equals n.clienterId
                             where n.PubDate.Value >= startOfWeek && n.PubDate.Value < endOfWeek
                                group n by new { n.clienterId } into g
                                select new ClienterViewModel
                                {
                                    Id = g.FirstOrDefault().clienterId != null ? g.FirstOrDefault().clienterId.Value : 0,
                                    OrderCount = g.Count(),
                                };
                }
                else if (criteria.searchType == 3)
                {
                    items = from m in db.clienter
                            join n in db.order
                            on m.Id equals n.clienterId
                            where n.PubDate.Value.Month == DateTime.Now.Month
                            group n by new { n.clienterId } into g
                            select new ClienterViewModel
                            {
                                Id = g.FirstOrDefault().clienterId != null ? g.FirstOrDefault().clienterId.Value : 0,
                                OrderCount = g.Count(),
                            };
                }
                else
                {
                    items = from m in db.clienter
                            join n in db.order
                            on m.Id equals n.clienterId
                            group n by new { n.clienterId } into g
                            select new ClienterViewModel
                            {
                                Id = g.FirstOrDefault().clienterId != null ? g.FirstOrDefault().clienterId.Value : 0,
                                OrderCount = g.Count(),
                            };
                }
                var listclienters = new List<ClienterViewModel>();
                foreach (var item in items)
                {
                    var clienter = clienterLogic().GetClienterById(item.Id);
                    item.Name = clienter.TrueName;
                    item.AccountBalance = clienter.AccountBalance;
                    listclienters.Add(item);
                }
                var clienters = listclienters.AsQueryable<ClienterViewModel>();
                if (!string.IsNullOrEmpty(criteria.clienterName))
                {
                    clienters = clienters.Where(p => p.Name == criteria.clienterName);
                }
                if (!string.IsNullOrEmpty(criteria.clienterPhone))
                {
                    clienters = clienters.Where(p => p.PhoneNo == criteria.clienterPhone);
                }
                //if (criteria.Status != -1)
                //{
                //    items = items.Where(p => p.Status == criteria.Status);
                //}
                var pagedQuery = new ClienterCountManage();
                resultModel = new PagedList<ClienterViewModel>(clienters.ToList(), criteria.PagingRequest.PageIndex, criteria.PagingRequest.PageSize);
                var businesslists = new ClienterCountManageList(resultModel.ToList(), resultModel.PagingResult);
                pagedQuery.clienterCountManageList = businesslists;

                return pagedQuery;
            }
        }

        /// <summary>
        /// 检查号码是否存在
        /// </summary>
        /// <param name="phoneNo"></param>
        /// <returns></returns>
        public bool CheckExistPhone(string phoneNo)
        {
            bool bResult = false;
            using (var db = new supermanEntities())
            {
                var item = db.clienter.Where(p => p.PhoneNo == phoneNo).FirstOrDefault();
                if (item != null)
                {
                    bResult = true;
                }
            }
            return bResult;
        }

        /// <summary>
        /// 添加一个超人
        /// </summary>
        /// <param name="clienter"></param>
        /// <returns></returns>
        public bool Add(clienter clienter)
        {
            bool result = false;
            try
            {
                using (var db = new supermanEntities())
                {
                    if (clienter != null)
                    {
                        db.clienter.Add(clienter);
                        int i = db.SaveChanges();
                        if (i != 0)
                            result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogWriter("添加超人异常", new { ex = ex, clienter = clienter });
            }
            return result;
        }

        /// <summary>
        /// 登录时根据电话号码和密码查找超人
        /// </summary>
        /// <param name="phoneNo"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public clienter GetClienter(string phoneNo, string pwd)
        {
            using (var db = new supermanEntities())
            {
                var query = db.clienter.Where(p => p.PhoneNo == phoneNo && p.Password == pwd);
                LogHelper.LogWriter("登录结果",new { query = query });
                return query.FirstOrDefault();
            }
        }

        /// <summary>
        /// 查询电话号码是否存在
        /// </summary>
        /// <param name="phoneNo"></param>
        /// <returns></returns>
        public clienter GetClienter(string phoneNo)
        {
            using (var db = new supermanEntities())
            {
                var query = db.clienter.Where(p => p.PhoneNo == phoneNo);
                return query.FirstOrDefault();
            }
        }

        /// <summary>
        /// 根据Id查找超人
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public clienter GetClienterById(int userId)
        {
            using (var db = new supermanEntities())
            {
                var query = db.clienter.Where(p => p.Id == userId);
                return query.FirstOrDefault();
            }
        }

        /// <summary>
        /// 获取我的任务订单 PagedList
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public PagedList<order> GetOrders(ClientOrderSearchCriteria criteria)
        {
            using (var dbEntity = new supermanEntities())
            {
                var query = dbEntity.order.AsQueryable();
                if (criteria.userId != 0)
                {
                    query = query.Where(i => i.clienterId == criteria.userId);
                }
                if (!string.IsNullOrWhiteSpace(criteria.city))
                {
                    query = query.Where(i => i.business.City == criteria.city.Trim());
                }
                if (!string.IsNullOrWhiteSpace(criteria.cityId))
                {
                    query = query.Where(i => i.business.CityId == criteria.cityId.Trim());
                }
                if (criteria.status != -1 && criteria.status!=null)
                {
                    query = query.Where(i => i.Status.Value == criteria.status);
                }
                else
                {
                    query = query.Where(i => i.Status.Value == ConstValues.ORDER_NEW);
                }
                 
                //query = query.OrderByDescending(i => i.Id);

                var result = new PagedList<order>(query.ToList(), criteria.PagingRequest.PageIndex, criteria.PagingRequest.PageSize);
                return result;
            }
        }

        public PagedList<order> GetMyOrders(ClientOrderSearchCriteria criteria)
        {
            using (var dbEntity = new supermanEntities())
            {
                var query = dbEntity.order.AsQueryable();
                if (criteria.userId != 0)
                {
                    query = query.Where(i => i.clienterId == criteria.userId);
                }
                if (criteria.status != null && criteria.status.Value != -1)
                {
                    query = query.Where(i => i.Status.Value == criteria.status.Value);
                }
                else
                {
                    query = query.Where(i => i.Status.Value == ConstValues.ORDER_ACCEPT);
                }
                
                query = query.OrderByDescending(i => i.Id);

                var result = new PagedList<order>(query.ToList(), criteria.PagingRequest.PageIndex, criteria.PagingRequest.PageSize);
                return result;
            }
        }

        /// <summary>
        /// 获取附近任务 / 最新
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public PagedList<order> GetOrdersNoLogin(ClientOrderSearchCriteria criteria)
        {
            using (var db = new supermanEntities())
            {
                var query = db.order.AsQueryable();
                if (criteria.status != null && criteria.status.Value != -1)
                {
                    query = query.Where(i => i.Status.Value == criteria.status.Value);
                } 
                query = query.OrderByDescending(i => i.PubDate);

                var result = new PagedList<order>(query.ToList(), criteria.PagingRequest.PageIndex, criteria.PagingRequest.PageSize);
                return result;
            }
        }
        /// <summary>
        /// 未登录时获取最新任务 edit by caoheyang 20150130
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns></returns>
        public List<order> GetOrdersNoLoginLatest(ClientOrderSearchCriteria criteria)
        {
            using (var dbEntity = new supermanEntities())
            {
                var query = dbEntity.order.AsQueryable();
                if (!string.IsNullOrWhiteSpace(criteria.city))
                    query = query.Where(i => i.business.City == criteria.city.Trim());
                if (!string.IsNullOrWhiteSpace(criteria.cityId))
                    query = query.Where(i => i.business.CityId == criteria.cityId.Trim());
                query = query.Where(i => i.Status.Value == ConstValues.ORDER_NEW);
                query = query.OrderByDescending(i => i.PubDate);
                var result = query.ToList();
                return result;
            }
        }
        /// <summary>
        /// 获取送餐 或者 取餐盒 订单
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public List<order> GetOrdersForSongCanOrQuCan(ClientOrderSearchCriteria criteria)
        {
            using (var dbEntity = new supermanEntities())
            {
                var query = dbEntity.order.AsQueryable();
                if (!string.IsNullOrWhiteSpace(criteria.city))
                    query = query.Where(i => i.business.City == criteria.city.Trim());
                if (!string.IsNullOrWhiteSpace(criteria.cityId))
                    query = query.Where(i => i.business.CityId == criteria.cityId.Trim());
                //1送餐订单 还是  2取餐盒订单
                query = query.Where(i => i.OrderType == criteria.OrderType);
                //订单状态
                query = query.Where(i => i.Status.Value == ConstValues.ORDER_NEW);
                //排序
                query = query.OrderByDescending(i => i.PubDate);
                var result = query.ToList();
                return result;
            }

        }
        /// <summary>
        /// 修改超人密码
        /// </summary>
        /// <param name="clienter"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public bool ModifyPwd(clienter clienter, string newPassword)
        {
            bool result = false;
            using (var db = new supermanEntities())
            {
                if (clienter != null)
                {
                    var query = db.clienter.Where(p=>p.Id==clienter.Id).FirstOrDefault();
                    if(query!=null)
                    {
                        query.Password = newPassword;
                        int i = db.SaveChanges();
                        if (i != 0)
                            result = true;
                    }                    
                }
            }
            return result;
        }

        /// <summary>
        /// 查询当前订单是否可以抢
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns>true为可以抢，false为不可以抢</returns>
        public bool CheckOrderIsAllowRush(string orderNo)
        {
            bool result = false;
            using (var db = new supermanEntities())
            {
                var query = db.order.Where(p => p.OrderNo == orderNo && p.Status == ConstValues.ORDER_NEW).FirstOrDefault();  //状态为未抢单的订单
                if (query != null)
                {
                    result = true;
                }
            }
            return result;
        }

        /// <summary>
        /// 根据Id查订单
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public order GetOrderByNo(string orderNo)
        {
            using (var db = new supermanEntities())
            {
                var query = db.order.Where(p => p.OrderNo == orderNo).FirstOrDefault();
                if (query != null)
                {
                    return query;
                }
            }
            return null;
        }

        /// <summary>
        /// 抢单
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public bool RushOrder(int userId, string orderNo)
        {
            bool result = false;
            using (var db = new supermanEntities())
            {
                var query = db.order.Where(p => p.OrderNo == orderNo && p.Status==ConstValues.ORDER_NEW).FirstOrDefault();
                if (query != null)
                {
                    query.clienterId = userId;
                    query.Status = ConstValues.ORDER_ACCEPT;
                }
                int i = db.SaveChanges();
                if (i != 0)
                {
                    Push.PushMessage(1, "订单提醒", "有订单被抢了！", "有超人抢了订单！", query.businessId.Value.ToString(), string.Empty);
                    result = true;
                }
            }
            return result;
        }

        /// <summary>
        /// 完成订单 edit by caoheyang 20150204
        /// </summary>
        /// <param name="userId">C端用户id</param>
        /// <param name="orderNo">订单号码</param>
        /// <returns></returns>
        public int FinishOrder(int userId, string orderNo)
        {
            
            var result = -1;
            using (var db = new supermanEntities())
            {
                var query = db.order.Where(p => p.OrderNo == orderNo).FirstOrDefault();
                if (query != null)
                {
                    if(query.Status==ConstValues.ORDER_FINISH) //已完成状态不做处理
                    {
                        result = 1;
                        return result;
                    }
                    query.clienterId = userId;
                    query.Status = ConstValues.ORDER_FINISH;
                    query.ActualDoneDate = DateTime.Now;
                }
                var client = db.clienter.Where(p => p.Id == userId).FirstOrDefault();//查询用户
                if (client != null)  //更新用户相关金额数据
                {
                    if (client.AccountBalance != null)
                        client.AccountBalance = client.AccountBalance.Value + query.DistribSubsidy + query.OrderCommission + query.WebsiteSubsidy;
                    else
                        client.AccountBalance = query.DistribSubsidy + query.OrderCommission + query.WebsiteSubsidy;
                }

                // add 完成订单时添加收入
                var model = new myincome();
                model.PhoneNo = client.PhoneNo;
                model.MyIncome1 = "收入";
                model.MyInComeAmount = query.DistribSubsidy + query.OrderCommission + query.WebsiteSubsidy;
                model.InsertTime = DateTime.Now;
                db.myincome.Add(model);
                //end add 
                int i = db.SaveChanges(); 
                if (i != 0)
                {
                    Push.PushMessage(1, "订单提醒", "有订单完成了！", "有超人完成了订单！", query.businessId.Value.ToString(),string.Empty);
                    result = 2;
                } 
            }
            return result;
        }

        /// <summary>
        /// 更新审核状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="enumStatusType"></param>
        /// <returns></returns>
        public bool UpdateAuditStatus(int id, EnumStatusType enumStatusType)
        {
            bool bResult = false;
            using (var db = new supermanEntities())
            {
                var item = db.clienter.Find(id);
                if (item != null)
                {
                    if (enumStatusType == EnumStatusType.审核通过)
                    {
                        item.Status = ConstValues.CLIENTER_AUDITPASS;
                    }
                    else if (enumStatusType == EnumStatusType.审核取消)
                    {
                        item.Status = ConstValues.CLIENTER_AUDITCANCEL;
                    }
                    int i = db.SaveChanges();
                    if (i == 1)
                        bResult = true;
                }
            }
            return bResult;
        }

        /// <summary>
        /// 清空帐户余额
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool ClearSuperManAmount(int id)
        {
            var bResult = false;
            using (var db = new supermanEntities())
            {
                var query = db.clienter.Where(p => p.Id == id).FirstOrDefault();
                if (query != null)
                {
                    query.AccountBalance = 0;
                    int i = db.SaveChanges();
                    if (i != 0)
                    {
                        bResult = true;
                    }
                }
            }
            return bResult;
        }
        /// <summary>
        /// 获取手机号得到超人当前余额
        /// </summary>
        /// <param name="phoneNo"></param>
        /// <returns></returns>
        public decimal GetMyBalanceByPhoneNo(string phoneNo)
        {
            decimal bResult = 0.0M;
            using (var db = new supermanEntities())
            {
                var query = db.clienter.Where(p => p.PhoneNo == phoneNo).FirstOrDefault();
                if (query != null)
                {
                    if (query.AccountBalance != null)
                        bResult = query.AccountBalance.Value;
                }
            }
            return bResult;
        }
        /// <summary>
        /// 获取我的余额列表
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public PagedList<myincome> GetMyIncomeList(MyIncomeSearchCriteria criteria)
        {
            using (var db = new supermanEntities())
            {
                var query = db.myincome.AsQueryable();
                if (!string.IsNullOrEmpty(criteria.phoneNo))
                {
                    query = query.Where(i => i.PhoneNo == criteria.phoneNo);
                }
                query = query.OrderByDescending(i => i.InsertTime);
                var result = new PagedList<myincome>(query.ToList(), criteria.PagingRequest.PageIndex, criteria.PagingRequest.PageSize);
                return result;
            }
        }
        /// <summary>
        /// 更新一个超人的图片
        /// </summary>
        /// <param name="clienter"></param>
        /// <param name="picUrl"></param>
        /// <param name="picUrlWithHand"></param>
        public void UpdateClient(clienter clienter, string picUrl,string picUrlWithHand,string trueName, string IDCard)
        {
            using (var db = new supermanEntities())
            {
                var query = db.clienter.Where(i => i.Id == clienter.Id).FirstOrDefault();
                if(query!=null)
                {
                    query.PicUrl = picUrl;
                    query.PicWithHandUrl = picUrlWithHand;
                    query.TrueName = trueName;
                    query.IDCard = IDCard;
                    //query.Status = ConstValues.CLIENTER_AUDITPASSING;
                    query.Status = ConstValues.CLIENTER_AUDITPASSING;
                    db.SaveChanges();
                }
            }
        }
    }
}
