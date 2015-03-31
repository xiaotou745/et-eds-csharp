using SuperManCommonModel.Models;
using SuperManCore; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuperManWebApi.Models.Business
{
    public class BusiOrderResultModel
    {
        public int userId { get; set; }
    }

    //public class BusiOrderResultModelTranslator : TranslatorBase<order, OrderModel>
    //{
    //    public static readonly BusiOrderResultModelTranslator Instance = new BusiOrderResultModelTranslator();

    //    public override OrderModel Translate(order from)
    //    {
    //        var to = new OrderModel();
    //        to.Id = from.Id;
    //        to.IsPay = from.IsPay;
    //        to.ActualDoneDate = from.ActualDoneDate;
    //        to.Amount = from.Amount;
    //        to.DistribSubsidy = from.DistribSubsidy;
    //        to.OrderCommission = from.OrderCommission;
    //        to.PickUpAddress = from.PickUpAddress;
    //        to.PubDate = from.PubDate.ToString();
    //        to.ReceviceAddress = from.ReceviceAddress;
    //        to.ReceviceName = from.ReceviceName;
    //        return to;
    //    }

    //    public override order Translate(OrderModel from)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}