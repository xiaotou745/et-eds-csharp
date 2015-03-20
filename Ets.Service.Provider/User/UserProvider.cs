using Ets.Dao.Clienter;
using Ets.Dao.User;
using Ets.Model.ParameterModel.Clienter;
using Ets.Service.IProvider;
using Ets.Service.IProvider.User;
using ETS.Transaction;
using ETS.Transaction.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Service.Provider.User
{
    public class UserProvider : IUser
    {
        ClienterDao dao = new ClienterDao();
        public virtual List<int> Register(Model.UserModel user)
        {
            //如果有查询就放到外层
            //...
            using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
            {
                //实现调用DAO层方法
                //...
                ChangeWorkStatusPM pmModel = new ChangeWorkStatusPM();
                pmModel.Id = 1;
                pmModel.WorkStatus = 1;
                dao.ChangeWorkStatusToSql(pmModel);
                tran.Complete();
            }
            //new UserDao().RegisterToSql(new Model.UserModel());
            return new List<int>() { 
                1,2
            };
        }
    }
}