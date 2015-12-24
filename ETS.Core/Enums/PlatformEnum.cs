using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETS.Enums
{
    public enum PlatformEnum
    {
        // E代送商户版      (旧后台)
        OldModel = 1,
        // E代送智能调度    (新后台)
        NewModel = 2,
        //E代送             (闪送模式)
        FlashToSendModel = 3
    }

    public class PlatformClass
    {
        public static string GetPlatformStr(int platformId)
        {
            if (platformId == PlatformEnum.OldModel.GetHashCode())
            {
                return "E代送商户版";
            }
            if (platformId == PlatformEnum.NewModel.GetHashCode())
            {
                return "E代送智能调度";
            }
            if (platformId == PlatformEnum.FlashToSendModel.GetHashCode())
            {
                return "E代送";
            }
            return string.Empty;
        }
    }
}

