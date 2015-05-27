using System;

namespace Ets.Model.DataModel.Authority
{
    public class AccountCityRelation
    {
		/// <summary>
		/// 主键Id
		/// </summary>
		public int Id { get; set; }
		/// <summary>
		/// 用户Id
		/// </summary>
		public int AccountId { get; set; }
		/// <summary>
		/// 城市Id
		/// </summary>
		public int CityId { get; set; }
		/// <summary>
		/// 是否有效(0:否 1:是)
		/// </summary>
		public int IsEnable { get; set; }
		/// <summary>
		/// 创建人
		/// </summary>
		public string CreateBy { get; set; }
		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime CreateTime { get; set; }
		/// <summary>
		/// 最后一次更新人
		/// </summary>
		public string UpdateBy { get; set; }
		/// <summary>
		/// 最后一次更改时间
		/// </summary>
		public DateTime? UpdateTime { get; set; }

    }
}
