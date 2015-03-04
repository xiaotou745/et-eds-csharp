using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperManDataAccess;
using SuperManCommonModel.Entities;
using SuperManCommonModel.Models;
using SuperManCore.Common;
using SuperManCore;
using SuperManCommonModel;
namespace SuperManBusinessLogic.B_Logic
{
    public class BusiLogic
    {
        //public static BusiLogic busiLogic()
        //{
        //    return new BusiLogic();
        //}
        private volatile static BusiLogic _instance = null;
        private static readonly object lockHelper = new object();
        private BusiLogic() { }
        public static BusiLogic busiLogic()
        {
            if (_instance == null)
            {
                lock (lockHelper)
                {
                    if (_instance == null)
                        _instance = new BusiLogic();
                }
            }
            return _instance;
        }

        public PagedList<BusinessViewModel> resultModel { get; set; }

        /// <summary>
        ///  根据城市信息查询当前城市下该集团的所有商户信息  add by caoheyang 20150302         
        /// </summary>
        /// <param name="criteria">条件 </param>
        /// <returns></returns>
        public dynamic GetBussinessByCityInfo(BusinessSearchCriteria criteria)
        {
            List<business> bussiness = new List<business>();
            supermanEntities db = new supermanEntities();
            var items = db.business.AsQueryable();
            if (criteria.GroupId != null && criteria.GroupId!=0)
                items = items.Where(p => p.GroupId == criteria.GroupId);
            if (!string.IsNullOrWhiteSpace(criteria.ProvinceCode))
                items = items.Where(p => p.ProvinceCode == criteria.ProvinceCode);
            if (!string.IsNullOrWhiteSpace(criteria.CityCode))
                items = items.Where(p => p.CityCode == criteria.CityCode);
            var res = from p in items select new { Name = p.Name, Id = p.Id };
            return res;
        }

        public BusinessManage GetBusinesses(BusinessSearchCriteria criteria)
        {
            using (var db = new supermanEntities())
            {
                var items = db.business.AsQueryable();
                if (!string.IsNullOrEmpty(criteria.businessName))
                {
                    items = items.Where(p => p.Name == criteria.businessName);
                }
                if (!string.IsNullOrEmpty(criteria.businessPhone))
                {
                    items = items.Where(p => p.PhoneNo == criteria.businessPhone);
                }
                if (criteria.Status != -1)
                {
                    items = items.Where(p => p.Status == criteria.Status);
                }
                if (criteria.GroupId != null)
                {
                    items = items.Where(p => p.GroupId == criteria.GroupId);
                }

                var pagedQuery = new BusinessManage();
                var resultModel = new PagedList<business>(items.ToList(), criteria.PagingRequest.PageIndex, criteria.PagingRequest.PageSize);
                var businesslists = new BusinessManageList(resultModel.ToList(), resultModel.PagingResult);
                pagedQuery.businessManageList = businesslists;
                return pagedQuery;
            }
        }



        public BusinessCountManage GetBusinessesCount(BusinessSearchCriteria criteria)
        {
            IQueryable<BusinessViewModel> items = null;
            using (var db = new supermanEntities())
            {
                if (criteria.searchType == 1)
                {
                    items = from m in db.business
                            join n in db.order
                            on m.Id equals n.businessId
                            where n.PubDate.Value.Day == DateTime.Now.Day
                            group n by new { n.businessId } into g
                            select new BusinessViewModel
                            {
                                Name = g.FirstOrDefault().business.Name,
                                OrderCount = g.Count(),
                                OrderAmountCount = g.Sum(m => m.Amount)
                            };
                }
                else if (criteria.searchType == 2)
                {
                    DateTime givenDate = DateTime.Today;
                    DateTime startOfWeek = givenDate.AddDays(-1 * (int)givenDate.DayOfWeek);
                    DateTime endOfWeek = startOfWeek.AddDays(7);
                    items = from m in db.business
                            join n in db.order
                            on m.Id equals n.businessId
                            where n.PubDate.Value >= startOfWeek && n.PubDate.Value < endOfWeek
                            group n by new { n.businessId } into g
                            select new BusinessViewModel
                            {
                                Name = g.FirstOrDefault().business.Name,
                                OrderCount = g.Count(),
                                OrderAmountCount = g.Sum(m => m.Amount)
                            };
                }
                else if (criteria.searchType == 3)
                {
                    items = from m in db.business
                            join n in db.order
                            on m.Id equals n.businessId
                            where n.PubDate.Value.Month == DateTime.Now.Month
                            group n by new { n.businessId } into g
                            select new BusinessViewModel
                            {
                                Name = g.FirstOrDefault().business.Name,
                                OrderCount = g.Count(),
                                OrderAmountCount = g.Sum(m => m.Amount)
                            };
                }
                else
                {
                    items = from m in db.business
                            join n in db.order
                            on m.Id equals n.businessId
                            group n by new { n.businessId } into g
                            select new BusinessViewModel
                            {
                                Name = g.FirstOrDefault().business.Name,
                                OrderCount = g.Count(),
                                OrderAmountCount = g.Sum(m => m.Amount)
                            };
                }
                if (!string.IsNullOrEmpty(criteria.businessName))
                {
                    items = items.Where(p => p.Name == criteria.businessName);
                }
                if (!string.IsNullOrEmpty(criteria.businessPhone))
                {
                    items = items.Where(p => p.PhoneNo == criteria.businessPhone);
                }
                //if (criteria.Status != -1)
                //{
                //    items = items.Where(p => p.Status == criteria.Status);
                //}
                var pagedQuery = new BusinessCountManage();
                resultModel = new PagedList<BusinessViewModel>(items.ToList(), criteria.PagingRequest.PageIndex, criteria.PagingRequest.PageSize);
                var businesslists = new BusinessCountManageList(resultModel.ToList(), resultModel.PagingResult);
                pagedQuery.businessCountManageList = businesslists;
                return pagedQuery;
            }
        }

        /// <summary>
        /// 更新审核状态
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        public bool UpdateAuditStatus(int id, EnumStatusType enumStatusType)
        {
            bool bResult = false;
            using (var db = new supermanEntities())
            {
                var item = db.business.Find(id);
                if (item != null)
                {
                    if (enumStatusType == EnumStatusType.审核通过)
                    {
                        item.Status = ConstValues.BUSINESS_AUDITPASS;
                    }
                    else if (enumStatusType == EnumStatusType.审核取消)
                    {
                        item.Status = ConstValues.BUSINESS_AUDITCANCEL;
                    }
                    int i = db.SaveChanges();
                    if (i == 1)
                        bResult = true;
                }
            }
            return bResult;
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
                if (!string.IsNullOrEmpty(phoneNo))
                {
                    var item = db.business.Where(p => p.PhoneNo == phoneNo).FirstOrDefault();
                    if (item != null)
                    {
                        bResult = true;
                    }
                }
            }
            return bResult;
        }

        public bool CheckExistBusi(int originalBusiId, int groupId)
        {
            bool bResult = false;
            using (var db = new supermanEntities())
            {
                if (!string.IsNullOrEmpty(originalBusiId.ToString()))
                {
                    var item = db.business.Where(p => p.OriginalBusiId == originalBusiId && p.GroupId == groupId).FirstOrDefault();
                    if (item != null)
                    {
                        bResult = true;
                    }
                }
            }
            return bResult;
        }

        /// <summary>
        /// 添加一个商户
        /// </summary>
        /// <param name="business"></param>
        /// <returns></returns>
        public bool Add(business business, bool isAuditPass = false)
        {
            bool result = false;
            try
            {
                using (var db = new supermanEntities())
                {
                    if (business != null)
                    {
                        if (!isAuditPass)
                        {
                            business.Status = ConstValues.BUSINESS_NOADDRESS;
                        }
                        else
                        {
                            business.Status = ConstValues.BUSINESS_AUDITPASS;
                        }
                        db.business.Add(business);
                        int i = db.SaveChanges();
                        if (i != 0)
                            result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogWriter("添加商户异常", new { ex = ex, business = business });
            }
            return result;
        }

        /// <summary>
        /// 得到一个商户信息
        /// </summary>
        /// <param name="phoneNo"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public business GetBusiness(string phoneNo, string pwd)
        {
            using (var db = new supermanEntities())
            {
                var query = db.business.Where(p => p.PhoneNo == phoneNo && p.Password == pwd);
                return query.FirstOrDefault();
            }
        }

        /// <summary>
        /// 地址管理更新方法
        /// </summary>
        /// <param name="business"></param>
        /// <returns></returns>
        public string Update(business business)
        {
            var bResult = string.Empty;
            using (var db = new supermanEntities())
            {
                if (business.Id != 0)
                {
                    var query = db.business.Find(business.Id);
                    if (query.Status == ConstValues.BUSINESS_NOADDRESS)
                    {
                        query.Status = ConstValues.BUSINESS_NOAUDIT;
                    }
                    query.Address = business.Address;
                    query.PhoneNo2 = business.PhoneNo2;
                    query.Name = business.Name;
                    query.Landline = business.Landline;
                    query.districtId = business.districtId;
                    query.district = business.district;
                    query.Longitude = business.Longitude;
                    query.Latitude = business.Latitude;

                    int i = db.SaveChanges();
                    bResult = query.Status.ToString();
                }
            }
            return bResult;
        }

        /// <summary>
        /// 根据商户Id统计订单信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public BusiOrderCountResultModel GetOrderCountData(int userId)
        {
            BusiOrderCountResultModel model = new BusiOrderCountResultModel();
            using (var db = new supermanEntities())
            {
                if (userId != 0)
                {
                    var todayPublish = from n in db.order
                                       where n.businessId == userId && n.PubDate.Value.Day == DateTime.Now.Day && (n.Status == ConstValues.ORDER_NEW || n.Status == ConstValues.ORDER_FINISH)  //未接单
                                       group n by new { n.businessId } into g
                                       select new BusiOrderCountResultModel
                                       {
                                           todayPublish = g.Count(),
                                           todayPublishAmount = Math.Round(g.Sum(m => m.Amount).Value, 2).ToString()
                                       };

                    var tydayDone = from n in db.order
                                    where n.businessId == userId && n.PubDate.Value.Day == DateTime.Now.Day && n.Status == ConstValues.ORDER_FINISH //已完成
                                    group n by new { n.businessId } into g
                                    select new BusiOrderCountResultModel
                                    {
                                        todayDone = g.Count(),
                                        todayDoneAmount = Math.Round(g.Sum(m => m.Amount).Value, 2).ToString()
                                    };

                    var monthPublish = from n in db.order
                                       where n.businessId == userId && n.PubDate.Value.Month == DateTime.Now.Month && (n.Status == ConstValues.ORDER_NEW || n.Status == ConstValues.ORDER_FINISH)//未接单
                                       group n by new { n.businessId } into g
                                       select new BusiOrderCountResultModel
                                       {
                                           monthPublish = g.Count(),
                                           monthPublishAmount = Math.Round(g.Sum(m => m.Amount).Value, 2).ToString()
                                       };

                    var monthDone = from n in db.order
                                    where n.businessId == userId && n.ActualDoneDate.Value.Month == DateTime.Now.Month && n.Status == ConstValues.ORDER_FINISH //已完成
                                    group n by new { n.businessId } into g
                                    select new BusiOrderCountResultModel
                                    {
                                        monthDone = g.Count(),
                                        monthDoneAmount = Math.Round(g.Sum(m => m.Amount).Value, 2).ToString()
                                    };
                    if (todayPublish.FirstOrDefault() != null)
                    {
                        model.todayPublish = todayPublish.FirstOrDefault().todayPublish;
                        model.todayPublishAmount = todayPublish.FirstOrDefault().todayPublishAmount;
                        if (string.IsNullOrEmpty(model.todayPublishAmount))
                            model.todayPublishAmount = "0.00";
                    }
                    if (tydayDone.FirstOrDefault() != null)
                    {
                        model.todayDone = tydayDone.FirstOrDefault().todayDone;
                        model.todayDoneAmount = tydayDone.FirstOrDefault().todayDoneAmount;
                        if (string.IsNullOrEmpty(model.todayDoneAmount))
                            model.todayDoneAmount = "0.00";
                    }
                    if (monthPublish.FirstOrDefault() != null)
                    {
                        model.monthPublish = monthPublish.FirstOrDefault().monthPublish;
                        model.monthPublishAmount = monthPublish.FirstOrDefault().monthPublishAmount;
                        if (string.IsNullOrEmpty(model.monthPublishAmount))
                            model.monthPublishAmount = "0.00";
                    }

                    if (monthDone.FirstOrDefault() != null)
                    {
                        model.monthDone = monthDone.FirstOrDefault().monthDone;
                        model.monthDoneAmount = monthDone.FirstOrDefault().monthDoneAmount;
                        if (string.IsNullOrEmpty(model.monthDoneAmount))
                            model.monthDoneAmount = "0.00";
                    }
                }
            }
            return model;
        }



        /// <summary>
        /// 根据商户Id查询城市
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetCityByBusiId(int id)
        {
            string strResult = string.Empty;
            using (var db = new supermanEntities())
            {
                var query = db.business.Where(p => p.Id == id).FirstOrDefault();
                if (query != null)
                {
                    strResult = query.City;
                }
            }
            return strResult;
        }

        public business GetBusinessById(int id)
        {
            using (var db = new supermanEntities())
            {
                var query = db.business.Where(p => p.Id == id).FirstOrDefault();
                if (query != null)
                {
                    return query;
                }
            }
            return null;
        }

        /// <summary>
        /// 根据原平台商户Id和订单来源获取该商户信息
        /// </summary>
        /// <param name="oriBusiId"></param>
        /// <param name="orderFrom"></param>
        /// <returns></returns>
        public business GetBusiByOriIdAndOrderFrom(int oriBusiId, int orderFrom)
        {
            try
            {
                using (var db = new supermanEntities())
                {
                    var query = db.business.Where(p => p.OriginalBusiId == oriBusiId && p.GroupId == orderFrom).FirstOrDefault();
                    if (query != null)
                    {
                        return query;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogWriter(new { oriBusiId = oriBusiId, orderFrom = orderFrom }, ex, "根据原平台订单号和订单来源获取商户信息");
            }
            return null;
        }


        /// <summary>
        /// 更新一个business
        /// </summary>
        /// <param name="business"></param>
        public string UpdateBusi(business business, string picUrl)
        {
            var sResult = string.Empty;
            using (var db = new supermanEntities())
            {
                var query = db.business.Where(p => p.Id == business.Id).FirstOrDefault();
                if (query != null)
                {
                    query.CheckPicUrl = picUrl;
                    //query.Status = ConstValues.BUSINESS_AUDITPASSING;
                    query.Status = ConstValues.BUSINESS_AUDITPASSING;
                    int i = db.SaveChanges();
                    if (i == 1)
                    {
                        sResult = query.Status.ToString();
                    }
                }
            }
            return sResult;
        }

        public business GetBusinessByPhoneNo(string phoneNumber)
        {
            using (var db = new supermanEntities())
            {
                var query = db.business.Where(p => p.PhoneNo == phoneNumber).FirstOrDefault();
                if (query != null)
                {
                    return query;
                }
            }
            return null;
        }

        /// <summary>
        /// B端修改密码
        /// </summary>
        /// <param name="businessid"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool ModifyPwd(int businessid, string password)
        {
            bool result = false;
            using (var db = new supermanEntities())
            {
                db.Configuration.ValidateOnSaveEnabled = false;
                var query = db.business.Where(p => p.Id == businessid).FirstOrDefault();
                if (query != null)
                {
                    query.Password = password;
                    int i = db.SaveChanges();
                    if (i != 0)
                        result = true;
                }
                db.Configuration.ValidateOnSaveEnabled = true;
            }
            return result;
        }
    }
}
