using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Model.ParameterModel.WtihdrawRecords;
using ETS.Util;

namespace Ets.Model.DomainModel
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

    public class MyBalanceListResultModelTranslator : TranslatorBase<IncomeModel, MyBalanceListResultModel>
    {
        public static readonly MyBalanceListResultModelTranslator Instance = new MyBalanceListResultModelTranslator();

        public override MyBalanceListResultModel Translate(IncomeModel from)
        {
            var model = new MyBalanceListResultModel();
            if (from.InsertTime != null)
            {
                model.InsertTime = from.InsertTime.ToString("yyyy.MM.dd");
            }
            model.MyIncomeName = from.MyIncome1;
            model.MyInComeAmount = from.MyInComeAmount;
            return model;
        }

        public override IncomeModel Translate(MyBalanceListResultModel from)
        {
            throw new NotImplementedException();
        }

    }
}
