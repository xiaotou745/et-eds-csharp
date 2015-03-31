
namespace Ets.Model.ParameterModel.Clienter
{
    /// <summary>
    /// C端用户当前状态
    /// </summary>
    public class ClienterStatusModel
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string phoneNo { get; set; }
        /// <summary>
        /// 余额
        /// </summary>
        public decimal Amount { get; set; }
    }
}
