﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuperManWebApi.Models.Business
{
    public class BusiForgetPwdInfoModel
    {
        public string phoneNumber { get; set; }
        public string password { get; set; }  
        public string checkCode { get; set; }
    }
}