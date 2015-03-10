using Common.Logging;
using ETS.Data.ConnString.Common;

namespace ETS.Data
{
    /// <summary>
    /// DaoBase基类
    /// </summary>
    public abstract class AbstractDaoBase
    {
        protected readonly ILog Logger;

        private DbHelper dbHelper;

        protected AbstractDaoBase()
        {
            Logger = LogManager.GetLogger(typeof (AbstractDaoBase));
        }

        protected DbHelper DbHelper
        {
            get { return dbHelper ?? (dbHelper = new DbHelper()); }
        }

        protected string GetConnString(string connName)
        {
            return ConnStringUtil.GetConnectionString(connName);
        }
    }
}