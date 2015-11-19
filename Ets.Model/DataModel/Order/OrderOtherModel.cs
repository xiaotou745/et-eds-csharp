using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DataModel.Order
{
    public class OrderOtherModel
    {
        /// <summary>
        /// 自增主键
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// 份数：需要上传的小票图片张数
        /// </summary>
        public int NeedUploadCount { get; set; }
        /// <summary>
        /// 小票图片用竖线分隔（|）
        /// </summary>
        public string ReceiptPic { get; set; }
        /// <summary>
        /// 已上传小票数量
        /// </summary>
        public int HadUploadCount { get; set; }
        /// <summary>
        /// 是否加入已提现
        /// </summary>
        public int IsJoinWithdraw { get; set; }
        /// <summary>
        /// 商户发单经度
        /// </summary>
        public decimal? PubLongitude { get; set; }
        /// <summary>
        /// 商户发单纬度
        /// </summary>
        public decimal? PubLatitude { get; set; }
        /// <summary>
        /// 骑士抢单时间
        /// </summary>
        public DateTime? GrabTime { get; set; }
        /// <summary>
        /// 骑士抢单经度
        /// </summary>
        public decimal? GrabLongitude { get; set; }
        /// <summary>
        /// 骑士抢单纬度
        /// </summary>
        public decimal? GrabLatitude { get; set; }
        /// <summary>
        /// 骑士完成订单经度
        /// </summary>
        public decimal? CompleteLongitude { get; set; }
        /// <summary>
        /// 骑士完成订单纬度
        /// </summary>
        public decimal? CompleteLatitude { get; set; }
        /// <summary>
        /// 取货时间
        /// </summary>
        public DateTime? TakeTime { get; set; }
        /// <summary>
        ///  取货经度
        /// </summary>
        public decimal? TakeLongitude { get; set; }
        /// <summary>
        /// 取货纬度 
        /// </summary>
        public decimal? TakeLatitude { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal? PubToGrabDistance { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal? GrabToCompleteDistance { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal? PubToCompleteDistance { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int OneKeyPubOrder { get; set; }
        /// <summary>
        /// 是否无效订单
        /// </summary>
        public int IsNotRealOrder { get; set; }
        /// <summary>
        /// 扣除补贴原因
        /// </summary>
        public string DeductCommissionReason { get; set; }
        /// <summary>
        /// 扣除补贴类型: 1 自动扣除    2 人工扣除
        /// </summary>
        public int? DeductCommissionType { get; set; }
        /// <summary>
        /// 审核状态：0待审核1审核通过2审核拒绝
        /// </summary>
        public int AuditStatus { get; set; }
        /// <summary>
        /// 订单是否需要审核(快照) 0不需要 1 需要
        /// </summary>
        public int? IsOrderChecked { get; set; }
        /// <summary>
        /// 取消订单时间
        /// </summary>
        public DateTime? CancelTime { get; set; }
        /// <summary>
        /// 是否允许现金支付1允许0不允许，默认0
        /// </summary>
        public int IsAllowCashPay { get; set; }
        /// <summary>
        /// 发单是否实时上传坐标
        /// </summary>
        public int IsPubDateTimely { get; set; }
        /// <summary>
        /// 抢单是否实时上传坐标
        /// </summary>
        public int IsGrabTimely { get; set; }
        /// <summary>
        /// 取货是否实时上传坐标
        /// </summary>
        public int IsTakeTimely { get; set; }
        /// <summary>
        /// 完成订单是否实时上传坐标
        /// </summary>
        public int IsCompleteTimely { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? AuditDate { get; set; }
        /// <summary>
        /// 审核操作人
        /// </summary>
        public string AuditOptName { get; set; }


    }
}
