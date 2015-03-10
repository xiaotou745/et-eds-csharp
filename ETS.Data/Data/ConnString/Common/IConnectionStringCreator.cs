using System.Collections.Generic;

namespace ETS.Data.ConnString.Common
{
	/// <summary>
	/// ����ConnString�ӿ�
	/// </summary>
	public interface IConnectionStringCreator
	{
		/// <summary>
		/// �������е�ConnString.
		/// </summary>
		/// <returns></returns>
		IList<IConnectionString> CreateConnStrings();
	}
}