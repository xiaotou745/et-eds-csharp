using SuperManCommonModel.Models;
using SuperManCore;
using SuperManDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperManBusinessLogic.Subsidy_Logic
{
    public class SubsidyLogic
    {
        //public static SubsidyLogic subsidyLogic()
        //{
        //    return new SubsidyLogic();
        //}
        private volatile static SubsidyLogic _instance = null;
        private static readonly object lockHelper = new object();
        private SubsidyLogic() { }
        public static SubsidyLogic subsidyLogic()
        {
            if (_instance == null)
            {
                lock (lockHelper)
                {
                    if (_instance == null)
                        _instance = new SubsidyLogic();
                }
            }
            return _instance;
        }

        public SubsidyResultModel GetCurrentSubsidy()
        {
            var resultModel = new SubsidyResultModel();
            using (var db = new supermanEntities())
            {
                //start 取补贴信息 
                var subsidyQuery = db.subsidy.AsQueryable();
                subsidyQuery = subsidyQuery.Where(i => i.StartDate <= DateTime.Now && i.EndDate >= DateTime.Now && i.Status.Value == 1).OrderByDescending(i => i.StartDate.Value); //获取当前有效期内的补贴
                var subsidy = subsidyQuery.FirstOrDefault();
                // end 
                if (subsidy != null)
                {
                    resultModel.DistribSubsidy = subsidy.DistribSubsidy;
                    resultModel.OrderCommission = subsidy.OrderCommission;
                    resultModel.WebsiteSubsidy = subsidy.WebsiteSubsidy;
                }
            }
            return resultModel;
        }
        public bool SaveData(SubsidyModel model)
        {
            bool bResult = false;
            using (var db = new supermanEntities())
            {
                var dbModel = new subsidy();
                dbModel.DistribSubsidy = model.DistribSubsidy;
                dbModel.WebsiteSubsidy = model.WebsiteSubsidy;
                if (model.OrderCommission != null)
                {
                    dbModel.OrderCommission = model.OrderCommission.Value / 100;
                }
                else
                {
                    dbModel.OrderCommission = 0.1m;
                }
                dbModel.StartDate = model.StartDate;
                dbModel.EndDate = model.EndDate;
                dbModel.Status = 1;
                db.subsidy.Add(dbModel);
                int i = db.SaveChanges();
                if (i == 1)
                {
                    bResult = true;
                }
            }
            return bResult;
        }

        public SubsidyManageList GetSubsidyList(SuperManCommonModel.Entities.HomeCountCriteria criteria)
        {
            IQueryable<subsidy> items = null;
            using (var db = new supermanEntities())
            {
                items = db.subsidy.OrderByDescending(p=>p.StartDate).AsQueryable();
                items = items.OrderByDescending(p => p.EndDate);
                var resultModel = new PagedList<subsidy>(items.ToList(), criteria.PagingRequest.PageIndex, criteria.PagingRequest.PageSize);
                var businesslists = new SubsidyManageList(resultModel.ToList(), resultModel.PagingResult);
                var pagedQuery = businesslists;
                return pagedQuery;
            }
        }
    }
}
