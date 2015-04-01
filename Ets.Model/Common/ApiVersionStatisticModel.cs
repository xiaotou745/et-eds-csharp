using System;

namespace Ets.Model.Common
{
    /// <summary>
    /// 接口调用统计
    /// </summary>
    public class ApiVersionStatisticModel
    {
        public int id { get; set; }
        public string APIName { get; set; }
        public DateTime CreateTime { get; set; }
        public string Version { get; set; }
    }
}
