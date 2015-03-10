using ETS.Transaction.Common;

namespace ETS.Transaction.WMS
{
	/// <summary>
	/// WMS ������ӿ�
	/// ��ͨ�ýӿ��ϼ��������ݿ��ָ��
	/// </summary>
	public interface IWMSUnitOfWorkDefinition : IUnitOfWorkDefinition
	{
		/// <summary>
		/// ���ĸ����ݿ�ִ������
		/// </summary>
		WMSDatabase? Database { get; set; }

		/// <summary>
		/// �ֿ�Id
		/// </summary>
		string WarehouseId { get; set; }
	}
}