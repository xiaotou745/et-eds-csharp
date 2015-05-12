using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Model.Common;


namespace Ets.Model.DataModel.Clienter
{
    public class CrossShopStatisticLogModel
    {
        public int Id { get; set; }
        public decimal TotalAmount { get; set; }
        public int OnceCount { get; set; }
        public int TwiceCount { get; set; }
        public int ThreeTimesCount { get; set; }
        public int FourTimesCount { get; set; }
        public int FiveTimesCount { get; set; }
        public int SixTimesCount { get; set; }
        public int SevenTimesCount { get; set; }
        public int EightTimesCount { get; set; }
        public int NineTimesCount { get; set; }
        public int ExceedNineTimesCount { get; set; }

        public decimal OncePrice { get; set; }
        public decimal TwicePrice { get; set; }
        public decimal ThreeTimesPrice { get; set; }
        public decimal FourTimesPrice { get; set; }
        public decimal FiveTimesPrice { get; set; }
        public decimal SixTimesPrice { get; set; }
        public decimal SevenTimesPrice { get; set; }
        public decimal EightTimesPrice { get; set; }
        public decimal NineTimesPrice { get; set; }
        public decimal ExceedNineTimesPrice { get; set; }

        public DateTime CreateTime { get; set; }
        public DateTime StatisticalTime { get; set; }
    }
}
