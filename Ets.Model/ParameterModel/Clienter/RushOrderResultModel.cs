 

namespace Ets.Model.ParameterModel.Clienter
{
    public class RushOrderResultModel
    {
        public string userId { get; set; }
    }
    public class FinishOrderResultModel
    {
        /// <summary>
        /// 返回信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 是否允许修改小票
        /// </summary>
        public bool IsModifyTicket { get; set; }
    }
}
