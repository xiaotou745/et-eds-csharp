using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
namespace Ets.AccountCheck
{
    static class RepositoryService
    {
        private static readonly string ReadConnectionString = ConfigurationManager.ConnectionStrings["SuperMan_Read"].ConnectionString;
        private static readonly string WriteConnectionString = ConfigurationManager.ConnectionStrings["SuperMan_Write"].ConnectionString;

        /// <summary>
        /// 测试连接数据库
        /// </summary>
        /// <returns></returns>
        public static bool TestReadConnection()
        {
            using (var conn = GetConnection(ReadConnectionString))
            {
                if (conn.State == ConnectionState.Open)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 测试连接数据库
        /// </summary>
        /// <returns></returns>
        public static bool TestWriteConnection()
        {
            using (var conn = GetConnection(WriteConnectionString))
            {
                if (conn.State == ConnectionState.Open)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 获得所有骑士
        /// </summary>
        /// <returns></returns>
        public static IList<int> AllClienters()
        {
            using (var conn = GetConnection(ReadConnectionString))
            {
                return conn.Query<int>("SELECT Id FROM clienter").ToArray();
            }
        }
        /// <summary>
        /// 查询骑士账户余额
        /// </summary>
        /// <param name="clienterId"></param>
        /// <returns></returns>
        public static IDictionary<int, ClienterAccountBalance> GetClienterAccountBalance(int[] clienterId)
        {
            if (clienterId == null || clienterId.Length == 0)
            {
                return new Dictionary<int, ClienterAccountBalance>();
            }

            string sql = "SELECT Id,AccountBalance FROM clienter WHERE Id IN({0})";

            IEnumerable<ClienterAccountBalance> enumerable;
            using (var conn = GetConnection(ReadConnectionString))
            {
                enumerable = conn.Query<ClienterAccountBalance>(string.Format(sql, string.Join(",", clienterId)));
            }
            if (enumerable == null)
            {
                return new Dictionary<int, ClienterAccountBalance>();
            }
            return enumerable.ToDictionary(m => m.Id);
        }
        /// <summary>
        /// 添加对账记录
        /// </summary>
        /// <param name="checking"></param>
        public static void AddAccountChecking(ClienterAccountChecking checking)
        {
            string sql = @"INSERT INTO ClienterAccountChecking(ClienterId, CreateDate, FlowStatMoney, ClienterTotalMoney, StartDate, EndDate,LastTotalMoney)
	VALUES (@ClienterId, @CreateDate, @FlowStatMoney, @ClienterTotalMoney, @StartDate, @EndDate,@LastTotalMoney)";
            using (var conn = GetConnection(WriteConnectionString))
            {
                conn.Execute(sql, checking);
            }
        }
        /// <summary>
        /// 统计骑士指定时间流水金额
        /// </summary>
        /// <param name="clienterId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static IDictionary<int, ClienterFlowStat> FlowMoney(int[] clienterId, DateTime startDate, DateTime endDate)
        {
            string sql = @" SELECT SUM(amount) Amcount,ClienterId FROM ClienterBalanceRecord 
                            WHERE OperateTime>=@Start AND OperateTime<=@End 
                            GROUP BY ClienterId
                            HAVING ClienterId IN({0})";

            if (clienterId == null || clienterId.Length == 0)
            {
                return new Dictionary<int, ClienterFlowStat>();
            }

            IEnumerable<ClienterFlowStat> list;
            using (var conn = GetConnection(ReadConnectionString))
            {
                list = conn.Query<ClienterFlowStat>(string.Format(sql, string.Join(",", clienterId)), new { Start = startDate.Date, End = endDate.Date });
            }

            if (list != null)
            {
                return list.ToDictionary(m => m.ClienterId);
            }
            return new Dictionary<int, ClienterFlowStat>();
        }
        /// <summary>
        /// 获得最后一次对账记录
        /// </summary>
        /// <param name="clienterId"></param>
        /// <returns></returns>
        public static ClienterAccountChecking GetLastStat(int clienterId)
        {
            string sql = "SELECT TOP 1 * FROM ClienterAccountChecking WHERE ClienterId=@ClienterId ORDER BY id DESC";

            using (var conn = GetConnection(ReadConnectionString))
            {
                return conn.Query<ClienterAccountChecking>(sql, new { ClienterId = clienterId }).FirstOrDefault();
            }
        }
        

        private static IDbConnection GetConnection(string connectionstring)
        {
            var connection = new SqlConnection(connectionstring);
            connection.Open();

            return connection;
        }
    }
}
