using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaskPlatform.PlanEngine
{

    /// <summary>
    /// 触发执行计划属性变更的委托。
    /// </summary>
    /// <param name="sender">执行计划</param>
    /// <param name="e">执行计划属性变更所包含的信息</param>
    public delegate void UpdatePropertyHandler();

}
