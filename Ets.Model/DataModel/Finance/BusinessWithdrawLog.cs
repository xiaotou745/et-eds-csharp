using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DataModel.Finance
{
    /// <summary>
    /// �̻�������־��ʵ����BusinessBalanceRecordDTO ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
    /// Generate By: tools.etaoshi.com  caoheyang
    /// Generate Time: 2015-05-11 16:37:45
    /// </summary>
    public class BusinessWithdrawLog
    {/// <summary>
        /// ����ID(PK)
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// ���ֵ�ID
        /// </summary>
        public long WithwardId { get; set; }
        /// <summary>
        /// ������״̬
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// ��ע
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        public string Operator { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime OperatTime { get; set; }
        /// <summary>
        /// ֮ǰ״̬
        /// </summary>
        public int OldStatus { get; set; }
        /// <summary>
        /// ����״̬��0�������� 1���Ѵ���
        /// </summary>
        public int DealStatus { get; set; }

        /// <summary>
        /// �Ƿ�Ϊ�ص���0���� 1���ǣ�
        /// </summary>
        public int IsCallBack { get; set; }
        /// <summary>
        /// �ص�����Id
        /// </summary>
        public string CallBackRequestId { get; set; }


        


    }

}
