using System.Data;

namespace ETS.Transaction.Common
{
	/// <summary>
	/// Much based on TransactionDefinition of Spring.NET. But add <see cref="Exclude"/> support.
	/// </summary>
	public interface IUnitOfWorkDefinition
	{
		/// <summary>
		/// ������뼶��
		/// </summary>
		IsolationLevel IsolationLevel { get; set; }

		/// <summary>
		/// ����ʱʱ��
		/// </summary>
		int Timeout { get; set; }

		/// <summary>
		/// ��֪Nhibernate��sessionΪReadOnly
		/// </summary>
		bool ReadOnly { get; set; }

		/// <summary>
		/// ��������
		/// </summary>
		string Name { get; set; }

		/// <summary>
		/// ��������Ƕ����ʽ�����Ƿ��ų��������Զ���ֱ��Using��������
		/// </summary>
		bool Exclude { get; set; }
	}
}