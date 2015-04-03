using System;
using System.Data;

namespace TaskPlatform.TaskInterface
{
    /// <summary>
    /// 提供对数据库的统一访问接口
    /// </summary>
    public interface IDBHelper
    {
        string ConnectionString { get; set; }
        int ExecuteSql(string SQLString);
        int ExecuteSql(string SQLString, string content);
        int ExecuteSqlByTime(string SQLString, int Times);
        object ExecuteSqlGet(string SQLString, string content);
        int ExecuteSqlTran(System.Collections.Generic.List<string> SQLStringList);
        void ExecuteSqlTran(System.Collections.Hashtable SQLStringList);
        bool Exists(string strSql);
        int GetMaxID(string FieldName, string TableName);
        object GetSingle(string SQLString);
        object GetSingle(string SQLString, int Times);
        DataSet Query(string SQLString);
        DataSet Query(string SQLString, int Times);
    }
}
