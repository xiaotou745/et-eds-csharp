using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Expand;

namespace ETS.Enums
{ 
    public enum VehicleEnum
    { 
        [Description("自行车/电动车")]
        ZiXingChe = 1,
        [Description("摩托车")] 
        MoTuoChe = 2,
        [Description("公共交通")] 
        GongGongJiaoTong = 3,
        [Description("汽车")] 
        QiChe = 4
    }  
}
