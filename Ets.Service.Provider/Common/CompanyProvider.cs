using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Dao.Common;
using Ets.Model.DomainModel.Company;

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
    }
}
