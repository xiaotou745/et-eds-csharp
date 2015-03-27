using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Ets.Model.Common
{
    /// <summary>
    /// 带sign的参数实体 add by caoheyang 20150318
    /// </summary>
    public class ParaModel<T>
    {
        /// <summary>
        /// AppKey,由e代送统一分配
        /// </summary>
        [Required]
        public string app_key { get; set; }
        /// <summary>
        /// 时间戳，格式为yyyy-mm-ddHH:mm:ss，例如：2015-03-11 13:52:03。
        /// </summary>
        [Required]
        public string timestamp { get; set; }
        /// <summary>
        /// api版本号，可选1.0
        /// </summary>
        [Required]
        public string v { get; set; }
        /// <summary>
        /// 对API调用参数(除sign外)进行md5加密获得。详细参考如下所示。
        /// </summary>
        [Required]
        public string sign { get; set; }

        /// <summary>
        /// 具体参数信息
        /// </summary>
        [Required]
        public T fields { get; set; }

        /// <summary>
        /// 集团：3:万达    查询得到。赋值
        /// </summary>
        public int group { get; set; }

    }

     /// <summary>
    /// 带sign的参数实体 add by caoheyang 20150318
    /// </summary>
    public class ParaModel {

        /// <summary>
        /// AppKey,由e代送统一分配
        /// </summary>
        [Required]
        public string app_key { get; set; }
        /// <summary>
        /// 时间戳，格式为yyyy-mm-ddHH:mm:ss，例如：2015-03-11 13:52:03。
        /// </summary>
        [Required]
        public string timestamp { get; set; }
        /// <summary>
        /// api版本号，可选1.0
        /// </summary>
        [Required]
        public string v { get; set; }
        /// <summary>
        /// 对API调用参数(除sign外)进行md5加密获得。详细参考如下所示。
        /// </summary>
        [Required]
        public string sign { get; set; }

        /// <summary>
        /// 集团：3:万达    查询得到。赋值
        /// </summary>
        public int group { get; set; }
    }
}
