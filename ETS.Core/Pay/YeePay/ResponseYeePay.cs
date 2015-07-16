using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace ETS.Pay.YeePay
{
    /// <summary>
    /// 易宝返回值解析帮助类  add by caoheyang 20150714
    /// </summary>
   public class ResponseYeePay
    {
        /// <summary>
        /// 解析返回值
        /// </summary>
        /// <param name="result">返回值</param>
        /// <param name="key">商户密钥</param>
        /// <returns></returns>
        private  static string OutRes(string result, string key)
        {
            var js = new JavaScriptSerializer();
            var resModle = js.Deserialize<ResponseModel>(result);
            if (string.IsNullOrWhiteSpace(resModle.code))
            {
                //返回{"data":"0158F0F5088AC62C07F935BD1E21E3A2CCBA3C72A7E2926C9E4631E7D9BAFF25D7B6FB4715DC98373CACBE603040E7D48FBAAF11CB37600A486960C75C48679BEE80333B01AB07B0A40A2A009D4A0AD5E6000245091C0189F8E75B4A7E01825C644C72A01674E9E0BA28ED8D06F32A05D139E7F225F4FE028BAF0A9FC735BC109DA5484D5611DDFFC3E38C8E5814B97A"} 
                var data = AESUtil.Decrypt(resModle.data, key);
                return data;
            }
            else
            {
                return result;
            }
        }
        /// <summary>
        /// 解析返回值
        /// </summary>
        /// <param name="result">返回值</param>
        /// <returns></returns>
        public static string OutRes(string result)
        {
            return OutRes(result, KeyConfig.YeepayHmac);
        }
       
       /// <summary>
       /// 
       /// </summary>
       /// <param name="result"></param>
       /// <param name="forCallBack"></param>
       /// <returns></returns>
        public static string OutRes(string result, bool forCallBack)
        {
            return AESUtil.Decrypt(result, KeyConfig.YeepayHmac);
        }

    }
}
