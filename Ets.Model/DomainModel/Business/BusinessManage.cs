﻿using Ets.Model.Common;
using Ets.Model.DataModel.Business;
using Ets.Model.DataModel.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DomainModel.Business
{
    public class BusinessManage
    {
        public BusinessManageList businessManageList { get; set; }
    }
    public class BusinessCountManage
    {
        public BusinessCountManageList businessCountManageList { get; set; }
    }
    public class BusinessManageList
    {
        public BusinessManageList(List<BusListResultModel> _businessModel, NewPagingResult pagingResult)
        {
            businessModel = _businessModel;
            PagingResult = pagingResult;
        }
        public List<BusListResultModel> businessModel { get; set; }
        public NewPagingResult PagingResult { get; set; }
        //public PagingResult PagingResult { get; set; }
    }

    public class BusinessCountManageList
    {
        public BusinessCountManageList(List<BusinessViewModel> _businessModel, PagingResult pagingResult)
        {
            businessModel = _businessModel;
            PagingResult = pagingResult;
        }
        public List<BusinessViewModel> businessModel { get; set; }
        public PagingResult PagingResult { get; set; }
    }
}
