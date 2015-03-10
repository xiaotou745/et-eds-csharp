namespace ETS.Data.ConnString.Common
{
	/// <summary>
	/// ��ʾӦ�ó��������ļ��е�ConnectionString���ýڶ�����ַ���
	/// </summary>
	/// <example>
	/// <code>
	/// <connectionStrings>
	///		<add name="dbConfigConnStringTest" connectionString="" providerName="System.Data.SqlClient"/>
	/// </connectionStrings>
	/// </code>
	/// </example>
	public class ConfigConnectionString : IConnectionString
	{
		#region IConnectionString Members

		/// <summary>
		/// ��ȡ������Ψһ����һ�������ַ������ַ�������
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// ��ȡ�����������ַ�������
		/// </summary>
		public string ConnectionString { get; set; }

		/// <summary>
		/// ��ȡ������Provider
		/// </summary>
		public string ProviderName { get; set; }

		#endregion

		public override string ToString()
		{
			return string.Format("ConfigConnString:[Name:{0}]-[ConnectionString:{1}]-[ProviderName:{2}]", Name, ConnectionString,
			                     ProviderName);
		}
	}
}