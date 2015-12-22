using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Expand;

namespace ETS.Enums
{
    public enum AppSource
    {
        // 易代送商户 
        [Description("易代送商户版")]
        EdaiSong = 0,
        // 智能调度版
        [Description("智能调度版")]
        ZhiNengDiaoDu = 1,
        //闪送版
        [Description("闪送版")]
        ShanSong = 2,
    }
}
