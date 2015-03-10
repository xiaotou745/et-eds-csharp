using System.Collections.Generic;

namespace ETS.Page
{
	/// <summary>
	/// ��ҳ���ӿ�
	/// </summary>
	/// <typeparam name="T">IPagedList<T></typeparam>
	public interface IPagedList<T>
	{
		///<summary>
		/// ��¼�б�
		///</summary>
		List<T> ContentList { get; }

		/// <summary>
		/// �ܼ�¼��
		/// </summary>
		int RecordCount { get; }

		/// <summary>
		/// ��ҳ��
		/// </summary>
		int PageCount { get; }
        /// <summary>
        /// ҳ��С��ÿҳ20����
        /// </summary>
        int PageSize { get; set; }
        /// <summary>
        /// ��ǰҳ
        /// </summary>
        int CurrentPage { get; set; }
         
	}
    public interface PricIPageList<T> : IPagedList<T>
        
    {
        decimal PriceCount { get; set; }
        /// <summary>
        /// ����Ϊ1�ĸ���
        /// </summary>
        int DefCount { get; set; }
       
       
        
    }
}