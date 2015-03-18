using ETS.Data.ConnString.Common;
using ETS.Transaction.Common;

namespace ETS.Transaction
{
    public class EdsUtilOfWorkFactory
    {
        public static IUnitOfWork GetUnitOfWorkOfEDS()
        {
            return UnitOfWorkFactory.GetAdoNetUnitOfWork(ConnStringUtil.GetConnectionString("SuperMan_Write"));
        }
    }
}