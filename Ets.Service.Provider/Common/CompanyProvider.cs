using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Dao.Common;
using Ets.Model.DomainModel.DeliveryCompany;

namespace Ets.Service.Provider.Common
{
    public class CompanyProvider
    {
        /// <summary>
        /// 获取物流公司ID 和名称
        /// </summary>
        /// <returns></returns>
        public IList<CompanyModel> GetCompanyList()
        {
            CompanyDao companyDao=new CompanyDao();
            return  companyDao.GetCompanyList();
        }
        /// <summary>
        /// 根据用户获取物流公司权限
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public IList<CompanyModel> GetCompanyListByAccountID(int accountId)
        {
            CompanyDao companyDao = new CompanyDao();
            return companyDao.GetCompanyListByAccountID(accountId);
        }
    }
}
