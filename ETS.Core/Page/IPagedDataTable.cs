using System.Data;

namespace ETS.Page
{
    ///<summary>
    /// ��ҳ���ӿ�(DataTable)
    ///</summary>
    public interface IPagedDataTable
    {
        ///<summary>
        /// ����DataTable
        ///</summary>
        DataTable ContentData { get; }

        /// <summary>
        /// �ܼ�¼��
        /// </summary>
        int RecordCount { get; }

        /// <summary>
        /// ��ҳ��
        /// </summary>
        int PageCount { get; }
		
        /// <summary>
        /// �ܱ仯����
        /// </summary>
        int TotalQuantity { get; }
    }
}