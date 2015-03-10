using System;
using System.Data;
using System.Data.Common;
using System.Security;
using System.Security.Permissions;

namespace ETS.Data.Core
{
	/// <summary>
	/// ��ʾһ�鷽������Щ�������ڴ����ṩ���������Դ���ʵ�ֵ�ʵ����
	/// </summary>
	public interface IDbProvider
	{
		/// <summary>
		/// ��ȡ�����������ַ���
		/// </summary>
		string ConnectionString { get; set; }

		/// <summary>
		/// ָ���ض��� <see cref="DbProviderFactory"/> �Ƿ�֧�� <see cref="DbDataSourceEnumerator"/> �ࡣ
		/// </summary>
		bool CanCreateDataSourceEnumerator { get; }

		/// <summary>
		/// ����ʵ�� <see cref="DbConnection"/> ����ṩ��������һ����ʵ����
		/// </summary>
		/// <param name="connectionString">�����ַ���</param>
		/// <returns></returns>
		IDbConnection CreateConnection(string connectionString);

		/// <summary>
		/// ����ʵ�� <see cref="DbCommand"/> ����ṩ��������һ����ʵ����
		/// </summary>
		/// <returns></returns>
		IDbCommand CreateCommand();

		/// <summary>
		/// ����ʵ�� <see cref="DbCommandBuilder"/> ����ṩ��������һ����ʵ����
		/// </summary>
		/// <returns></returns>
		DbCommandBuilder CreateCommandBuilder();

		/// <summary>
		/// ����ʵ�� <see cref="DbConnection"/> ����ṩ��������һ����ʵ����
		/// </summary>
		/// <returns></returns>
		IDbConnection CreateConnection();

		/// <summary>
		/// ����ʵ�� <see cref="DbConnectionStringBuilder"/> ����ṩ��������һ����ʵ����
		/// </summary>
		/// <returns></returns>
		DbConnectionStringBuilder CreateConnectionStringBuilder();

		/// <summary>
		/// ����ʵ�� <see cref="DbDataAdapter"/> ����ṩ��������һ����ʵ����
		/// </summary>
		/// <returns></returns>
		IDbDataAdapter CreateDataAdapter();

		/// <summary>
		/// ����ʵ�� <see cref="DbParameter"/> ����ṩ��������һ����ʵ����
		/// </summary>
		/// <returns></returns>
		IDataParameter CreateParameter();

		/// <summary>
		/// �����ṩ����������ʵ������ʵ����ʵ���ṩ����� <see cref="CodeAccessPermission"/> ��İ汾��
		/// </summary>
		/// <param name="state"></param>
		/// <returns></returns>
		CodeAccessPermission CreatePermission(PermissionState state);

		/// <summary>
		/// ����ʵ�� <see cref="DbDataSourceEnumerator"/> ����ṩ��������һ����ʵ����
		/// </summary>
		/// <returns></returns>
		DbDataSourceEnumerator CreateDataSourceEnumerator();

		/// <summary>
		/// ����ָ����providerInvariantName������������
		/// </summary>
		/// <param name="paramName"></param>
		/// <returns></returns>
		string CreateParameterNameForCollection(string paramName);

		/// <summary>
		/// �����������Ƿ���ȷ
		/// </summary>
		/// <param name="paramName"></param>
		/// <param name="paramType"></param>
		void CheckedParameterType(string paramName, Enum paramType);
	}
}