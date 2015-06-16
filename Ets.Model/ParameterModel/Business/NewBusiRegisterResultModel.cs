
namespace Ets.Model.ParameterModel.Business
{
    /// <summary>
    /// B端注册成功后返回类-第三方的
    /// </summary>
    public class NewBusiRegisterResultModel
    {
        /// <summary>
        /// 注册成功后的商户Id
        /// </summary>
        public int BusiRegisterId { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

    }
}
