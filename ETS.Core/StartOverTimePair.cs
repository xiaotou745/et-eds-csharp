using System;

namespace ETS
{
	/// <summary>
	/// 开始结束时间对
	/// </summary>
	public class StartOverTimePair
	{
		/// <summary>
		/// 开始时间
		/// </summary>
		public DateTime? StartTime { get; set; }

		/// <summary>
		/// 结束时间
		/// </summary>
		public DateTime? OverTime { get; set; }
	}
}