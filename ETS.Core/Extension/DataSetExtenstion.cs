using System.Data;

namespace ETS.Extension
{
    public static class DataSetExtenstion
    {
        public static bool HasData(this DataSet dataset)
        {
            if (dataset == null || dataset.Tables.Count == 0 || dataset.Tables[0].Rows.Count == 0)
            {
                return true;
            }
            return false;
        }
    }
}