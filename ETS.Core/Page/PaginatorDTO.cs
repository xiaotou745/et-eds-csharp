namespace ETS.Page
{
	/// <summary>
	/// ��ҳ������
	/// </summary>
	public class PaginatorDTO
	{
		private int pageSize;

		///<summary>
		///��ҳ�ߴ�
		///</summary>
		public int PageSize
		{
			get { return pageSize; }
			set { pageSize = value; }
		}

		private int pageNo;

		///<summary>
		///��ǰҳ��
		///</summary>
		public int PageNo
		{
			get { return pageNo; }
			set { pageNo = value; }
		}

		public int PageCount { get; set; }
	}
}