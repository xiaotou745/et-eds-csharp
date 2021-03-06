﻿using SuperManCommonModel.Models;
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

        public SubsidyResultModel GetCurrentSubsidy(int groupId = 0, int orderType = 0)
        {
            SubsidyResultModel resultModel = new SubsidyResultModel();
            using (var db = new supermanEntities())
            {
                //start 取补贴信息 
                var subsidyQuery = db.subsidy.AsQueryable();
                if (ConfigSettings.Instance.IsGroupPush)
                {
                    if (groupId > 0)  // 集团
                    {
                        if (orderType > 0)  //订单类型 1送餐订单  2取餐盒订单
                        {
                            subsidyQuery = subsidyQuery.Where(i => i.StartDate <= DateTime.Now && i.EndDate >= DateTime.Now && i.Status.Value == 1 && i.GroupId == groupId && i.OrderType == orderType)
                                .OrderByDescending(i => i.StartDate.Value); //获取当前有效期内的补贴
                        }
                        else
                        {
                            subsidyQuery = subsidyQuery.Where(i => i.StartDate <= DateTime.Now && i.EndDate >= DateTime.Now && i.Status.Value == 1 && i.GroupId == groupId)
                                .OrderByDescending(i => i.StartDate.Value); //获取当前有效期内的补贴
                        }
                    }
                    else
                    {
                        subsidyQuery = subsidyQuery.Where(i => i.StartDate <= DateTime.Now && i.EndDate >= DateTime.Now && i.Status.Value == 1 && (i.GroupId == null || i.GroupId == 0))
                                .OrderByDescending(i => i.StartDate.Value); //获取当前有效期内的补贴 
                    }
                }
                else
                {
                        subsidyQuery = subsidyQuery.Where(i => i.StartDate <= DateTime.Now && i.EndDate >= DateTime.Now && i.Status.Value == 1 && (i.GroupId == null || i.GroupId == 0))
                                .OrderByDescending(i => i.StartDate.Value); //获取当前有效期内的补贴
                }
                var subsidy = subsidyQuery.FirstOrDefault();
                // end 
                if (subsidy != null)
                {
                    resultModel.DistribSubsidy = subsidy.DistribSubsidy;
                    resultModel.OrderCommission = subsidy.OrderCommission;
                    resultModel.WebsiteSubsidy = subsidy.WebsiteSubsidy;
                    if (subsidy.OrderType != null)
                    {
                        resultModel.OrderType = subsidy.OrderType.Value;
                    }
                    if (subsidy.PKMCost != null)
                    {
                        resultModel.PKMCost = subsidy.PKMCost.Value;
                    }
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
                dbModel.GroupId = model.GroupId;
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
                items = db.subsidy.OrderByDescending(p => p.StartDate).AsQueryable();
                items = items.OrderByDescending(p => p.EndDate);
                if (criteria.GroupId != null && criteria.GroupId!=0)
                {
                    items = items.Where(p => p.GroupId == criteria.GroupId);
                }
                var resultModel = new PagedList<subsidy>(items.ToList(), criteria.PagingRequest.PageIndex, criteria.PagingRequest.PageSize);
                var businesslists = new SubsidyManageList(resultModel.ToList(), resultModel.PagingResult);
                var pagedQuery = businesslists;
                return pagedQuery;
            }
        }
    }
}
