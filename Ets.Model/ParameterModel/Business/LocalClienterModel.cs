using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Business
{
    public class LocalClienterModel
    {
        /// <summary>
        /// 骑士姓名
        /// </summary>
        public string ClienterName { get; set; }
        /// <summary>
        /// 骑士电话
        /// </summary>
        public string PhoneNo { get; set; }
        /// <summary>
        /// 骑士距商户距离
        /// </summary>
        public string Radius { get; set; }
        /// <summary>
        /// 上班状态（0：上班 1：下班）
        /// </summary>
        public int WorkStatus { get; set; }
        /// <summary>
        /// 已抢单任务数量
        /// </summary>
        public int ReceiveQty { get; set; }
        /// <summary>
        /// 配送中任务数量
        /// </summary>
        public int TransferQty { get; set; }
        /// <summary>
        /// 已完成任务数量
        /// </summary>
        public int FinishQty { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public double Longitude { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public double Latitude { get; set; }
    }
}
