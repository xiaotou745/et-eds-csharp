using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Authority
{
    public class LoginModel
    {
        [Required]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "验证码")]
        public string Captcha { get; set; }

        public bool RememberMe { get; set; }
    }
}
