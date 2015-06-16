using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuperManWebApi.Models.Clienter
{
    public class MyBalanceResultModel
    {
        public decimal MyBalance { get; set; }
    }

    public class MyBalanceListResultModel
    {
        /// <summary>
        /// 收入类型
        /// </summary>
        public string MyIncomeName { get; set; }
        /// <summary>
        /// 收入金额
        /// </summary>
        public decimal? MyInComeAmount { get; set; }
        /// <summary>
        /// 收入时间 
        /// </summary>
        public string InsertTime { get; set; }
    }
    //public class MyBalanceListResultModelTranslator : TranslatorBase<myincome, MyBalanceListResultModel>
    //{
    //    public readonly static MyBalanceListResultModelTranslator Instance = new MyBalanceListResultModelTranslator();
    //    public override MyBalanceListResultModel Translate(myincome from)
    //    {
    //        var model = new MyBalanceListResultModel();
    //        if(from.InsertTime!=null)
    //        {
    //            model.InsertTime = from.InsertTime.Value.ToString("yyyy.MM.dd");
    //        }            
    //        model.MyIncomeName = from.MyIncome1;
    //        model.MyInComeAmount = from.MyInComeAmount;
    //        return model;
    //    }

    //    public override myincome Translate(MyBalanceListResultModel from)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}