using System;

namespace Ets.Model.DataModel.Authority
{
    /// <summary>
    /// 用户账户实体
    /// </summary>
    public class AccountModel
    {
        public int Id { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string LoginName { get; set; }
        public int Status { get; set; }
        public int AccountType { get; set; }
        public DateTime FADateTime { get; set; }
        public string FAUser { get; set; }
        public DateTime LCDateTime { get; set; }
        public string LCUser { get; set; }
        public int GroupId { get; set; }
        public int RoleId { get; set; }
    }
}
