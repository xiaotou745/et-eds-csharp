using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DomainModel.Bussiness
{
    public class BusinessesDistributionModel
    {
        /// <summary>
        /// 发布时间
        /// </summary>
        public string PubDate { get; set; }
        /// <summary>
        /// 骑士数量
        /// </summary>
        public int ClienterCount { get; set; }

        /// <summary>
        /// 商家数量
        /// </summary>
        public int BusinessCount { get; set; }
        /// <summary>
        /// 一次数量
        /// </summary>
        public int OnceCount { get; set; }
        /// <summary>
        /// 两次数量
        /// </summary>
        public int TwiceCount { get; set; }
        /// <summary>
        /// 三次数量
        /// </summary>
        public int ThreeTimesCount { get; set; }
        /// <summary>
        /// 四次数量
        /// </summary>
        public int FourTimesCount { get; set; }
        /// <summary>
        /// 五次数量
        /// </summary>
        public int FiveTimesCount { get; set; }
        /// <summary>
        /// 六次数量
        /// </summary>
        public int SixTimesCount { get; set; }
        /// <summary>
        /// 七次数量
        /// </summary>
        public int SevenTimesCount { get; set; }
        /// <summary>
        /// 八次数量
        /// </summary>
        public int EightTimesCount { get; set; }
        /// <summary>
        /// 九次数量
        /// </summary>
        public int NineTimesCount { get; set; }
        /// <summary>
        /// 十次及以上数量
        /// </summary>
        public int ExceedNineTimesCount { get; set; }


    }
}
