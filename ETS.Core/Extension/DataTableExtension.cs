using System.Data;

namespace ETS.Extension
{
    public static class DataTableExtension
    {
        /// <summary>
        /// 是否含有某列名
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static bool HasData(this DataTable dataTable)
        {
            if (dataTable == null || dataTable.Rows.Count == 0)
            {
                return false;
            }
            return true;
        }
    }
}