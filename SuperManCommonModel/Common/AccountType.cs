using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperManCommonModel
{
    public enum AccountType
    {
        // 管理员 
        AdminUser = 1,
        // 普通帐号
        User = 2,
    }

    [Flags]
    public enum AuthorityType
    {
        [CandidateAccounts(AccountType.AdminUser)]
        Administration = 1,

        [CandidateAccounts(AccountType.User)]
        ChannelBussiness = 2,

        [CandidateAccounts(AccountType.AdminUser, AccountType.User)]
        AdministrationAndBussiness = Administration | ChannelBussiness
    }

    public class CandidateAccountsAttribute : Attribute
    {
        public CandidateAccountsAttribute(params AccountType[] types)
        {
            AccountTypes = types;
        }

        public AccountType[] AccountTypes { get; private set; }
    }
}
