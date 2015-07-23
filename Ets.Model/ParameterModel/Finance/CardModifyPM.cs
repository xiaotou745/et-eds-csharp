﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS;

namespace Ets.Model.ParameterModel.Finance
{
    /// <summary>
    ///  商户/骑士 修改绑定银行卡功能 参数实体 add by caoheyang 20150511
    /// 父类
    /// </summary>
    public class CardModifyPM
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 户名
        /// </summary>
        [Required(ErrorMessage = "户名不能为空")]
        public string TrueName { get; set; }

        /// <summary>
        /// 卡号(需要DES加密)
        /// </summary>
        [Required(ErrorMessage = "卡号不能为空")]
        [Compare("AccountNo2", ErrorMessage = "两次录入卡号必须一致")]
        public string AccountNo { get; set; }

        /// <summary>
        /// 第二次录入卡号(需要DES加密)
        /// </summary>
        [Required(ErrorMessage = "第二次录入卡号不能为空")]
        public string AccountNo2 { get; set; }

        /// <summary>
        /// 账号类别  0 个人账户 1 公司账户  
        /// </summary>
        public int BelongType { get; set; }

        /// <summary>
        /// 开户行
        /// </summary>
        [Required(ErrorMessage = "开户行不能为空")]
        public string OpenBank { get; set; }

        /// <summary>
        /// 开户支行
        /// </summary>
        [MinLength(3, ErrorMessage = "开户支行输入有误，请重新输入")]
        public string OpenSubBank { get; set; }

        /// <summary>
        /// 最后更新人
        /// </summary>
        [Required(ErrorMessage = "最后更新人不能为空")]
        public string UpdateBy { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        [Required(ErrorMessage = "版本号不能为空")]
        public string Version { get; set; }

        /// <summary>
        /// 开户省
        /// </summary>
        [Required(ErrorMessage = "请选择开户省份")]
        public string OpenProvince { get; set; }
        /// <summary>
        /// 省编码
        /// </summary>
        [Required(ErrorMessage = "开户省编码不能为空")]
        public int OpenProvinceCode { get; set; }
        /// <summary>
        /// 开户市
        /// </summary>
        [Required(ErrorMessage = "请选择开户城市")]
        public string OpenCity { get; set; }
        /// <summary>
        /// 市编码
        /// </summary>
        [Required(ErrorMessage = "开户市编码不能为空")]
        public int OpenCityCode { get; set; }

    }
}
