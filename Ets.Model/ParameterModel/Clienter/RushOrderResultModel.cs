 

namespace Ets.Model.ParameterModel.Clienter
{
    public class RushOrderResultModel
    {
        public string userId { get; set; }
    }
    public class FinishOrderResultModel
    {
        public int userId { get; set; }
        public decimal balanceAmount { get; set; }
    }
}
