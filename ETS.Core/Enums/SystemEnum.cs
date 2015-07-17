using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Expand;

namespace ETS.Enums
{
    #region 状态相关
    /// <summary>
    /// 系统级别接口返回 枚举
    /// </summary>
    public enum SystemState
    {
        [DisplayText("成功")]
        Success = 1,
        [DisplayText("系统错误")]
        SystemError = 0,
        [DisplayText("录入信息有误，请确认后重试。")]
        ParaError = -2,
        [DisplayText("版本号不能为空")]
        VersionError = -3
    }

    public enum VersionStatus
    {
        [DisplayText("获取失败")]
        Failed = 0,
        [DisplayText("获取成功")]
        Success = 1,
        [DisplayText("缺少UserType参数")]
        NoUserType = 2,
        [DisplayText("缺少PlatForm参数")]
        NoPlatForm = 3,
        [DisplayText("暂无数据")]
        NoData = 4
    }

    public enum AuditStatus
    {
        [DisplayText("审核取消")]
        Status0=0,
        [DisplayText("审核通过")]
        Status1 = 1,  
    }
    #endregion

    #region 列表相关
    /// <summary>
    /// 设置显示页数
    /// </summary>
    /// <UpdateBy>hulingbo</UpdateBy>
    /// <UpdateTime>20150706</UpdateTime>
    public enum PageSizeType
    {
        Web_PageSize = 15,
        App_PageSize = 50
    }

    #endregion
  

    public enum AppType
    {
        [DisplayText("B端")]
        FormB=1,
        [DisplayText("BC端")]
        FormC = 0, 
    }

    /// <summary>
    /// 来源
    /// </summary>
    /// <UpdateBy>hulingbo</UpdateBy>
    /// <UpdateTime>20150706</UpdateTime>
    public enum SuperPlatform
    {
        [DisplayText("商家")]
        FromBusiness=0,
        [DisplayText("骑士")]
        FromClienter = 1,
        [DisplayText("服务平台")]
        ServicePlatform=2,
        [DisplayText("管理后台")]
        ManagementBackground= 3,
        [DisplayText("第三方对接平台")]
        ThirdParty = 4,         
    }

    /// <summary>
    /// 客服电话状态
    /// </summary>
    public enum ServicePhoneStatus
    {
        [DisplayText("成功")]
        Success = 0,
        [DisplayText("失败")]
        Failed = 1
    }

    /// <summary>
    /// 省市区信息
    /// </summary>
    public enum CityStatus
    {
        [DisplayText("最新")]
        Newest = 0,
        [DisplayText("非最新")]
        UnNewest = 1
    }

    /// <summary>
    /// 银行省市区信息
    /// </summary>
    public enum BankCityStatus
    {
        [DisplayText("失败")]
        Failed = 0,
        [DisplayText("成功")]
        Success = 1
    } 
}
