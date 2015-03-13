using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SuperManCommonModel;
namespace SuperMan
{
	public sealed class AuthorityItem
    {
        public AuthorityItem(string name, AuthorityType authorityType)
        {
            this.Name = name;
            this.AuthorityType = authorityType;
        }

        public string Name { get; private set; }
        public AuthorityType AuthorityType { get; private set; }
    }

	public sealed class AuthorityNames
	{
		
		public const string OrderView = "订单管理-查看";
		
		public const string SupperView = "超人管理-查看";
		
		public const string SuperAudit = "超人管理-审核";
		
		public const string SuperClear = "超人管理-清除余额";
		
		public const string BusiView = "商户设置-查看";
		
		public const string BusiAudit = "商户设置-审核";

        public const string AccountView = "账号权限管理-查看";

        public const string AccountAdd = "账号权限管理-新增";

        public const string AccountEditPassword = "账号权限管理-修改密码";

        public const string AccountEdit = "账号权限管理-设置权限";

        public const string AccountDelete = "账号权限管理-删除";

        public const string BusSet = "补贴设置-设置";

        public const string AdminTools = "管理员工具-设置";
		
	}

	public sealed class Authorities
	{
		
		public AuthorityItem OrderView = new AuthorityItem( AuthorityNames.OrderView,(AuthorityType)1);		
		
		public AuthorityItem SupperView = new AuthorityItem( AuthorityNames.SupperView,(AuthorityType)1);		
		
		public AuthorityItem SuperAudit = new AuthorityItem( AuthorityNames.SuperAudit,(AuthorityType)1);		
		
		public AuthorityItem SuperClear = new AuthorityItem( AuthorityNames.SuperClear,(AuthorityType)1);		
		
		public AuthorityItem BusiView = new AuthorityItem( AuthorityNames.BusiView,(AuthorityType)1);		
		
		public AuthorityItem BusiAudit = new AuthorityItem( AuthorityNames.BusiAudit,(AuthorityType)1);		
		
		public AuthorityItem BusSet = new AuthorityItem( AuthorityNames.BusSet,(AuthorityType)1);

        public AuthorityItem AdminTools = new AuthorityItem(AuthorityNames.AdminTools, (AuthorityType)1);
		
	}

	public static class DefaultAuthoritiesForGallery
    {
        private static List<string> defaultAuthorities = new List<string>();
        static DefaultAuthoritiesForGallery()
        {
						defaultAuthorities.Add(AuthorityNames.OrderView);
								defaultAuthorities.Add(AuthorityNames.SupperView);
								defaultAuthorities.Add(AuthorityNames.SuperAudit);
								defaultAuthorities.Add(AuthorityNames.SuperClear);
								defaultAuthorities.Add(AuthorityNames.BusiView);
								defaultAuthorities.Add(AuthorityNames.BusiAudit);
								defaultAuthorities.Add(AuthorityNames.BusSet);
                                defaultAuthorities.Add(AuthorityNames.AdminTools);
				        }
        public static List<string> DefaultAuthorities
        {
            get
            {
                return defaultAuthorities;
            }
        }
    }
}