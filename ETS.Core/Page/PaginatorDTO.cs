namespace ETS.Page
{
	/// <summary>
	/// 分页数据类
	/// </summary>
	public class PaginatorDTO
	{
		private int pageSize;

		///<summary>
		///分页尺寸
		///</summary>
		public int PageSize
		{
			get { return pageSize; }
			set { pageSize = value; }
		}

		private int pageNo;

		///<summary>
		///当前页码
		///</summary>
		public int PageNo
		{
			get { return pageNo; }
			set { pageNo = value; }
		}

		public int PageCount { get; set; }
	}
}