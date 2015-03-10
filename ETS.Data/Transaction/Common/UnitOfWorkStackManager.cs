using System.Collections.Generic;
using Common.Logging;
using ETS.Data;
using ETS.Threading;

namespace ETS.Transaction.Common
{
	/// <summary>
	/// UnitOfWork���߳�ջ������
	/// </summary>
	public sealed class UnitOfWorkStackManager
	{
		#region Logging Definition

		/// <summary>
		/// get the <see cref="ILog"/> with a WYC.Data.txt file appender
		/// </summary>
		private static readonly ILog Logger = DbLogManager.GetConnectionStringLog();

		#endregion

		/// <summary>
		/// �߳���<see cref="IUnitOfWork"/>ջ��resource key
		/// </summary>
		public const string UnitOfWorkStack = "wyc:unitOfWorkStackResource";

		#region ThreadBoundedStack of UnitOfWork

		/// <summary>
		/// UnitOfWork�Ƿ��Ѵ���
		/// ����Ѵ��ڣ���ǰҪ������UnitOfWorkΪǶ������
		/// ����߳��в�����UnitOfWorkջ����֤����ǰҪ������UnitOfWorkΪ���������
		/// </summary>
		public static bool ThreadBoundUnitOfWorkStackExists
		{
			get
			{
				var unitOfWorkStack = LogicalThreadContext.GetData(UnitOfWorkStack) as Stack<IUnitOfWork>;
				if (unitOfWorkStack == null)
				{
					return false;
				}
				return true;
			}
		}

		/// <summary>
		/// ���ص�ǰ�߳��е�Stack��UnitOfWork������
		/// </summary>
		public static int Count
		{
			get
			{
				if (ThreadBoundUnitOfWorkStack == null)
				{
					return 0;
				}
				return ThreadBoundUnitOfWorkStack.Count;
			}
		}

		/// <summary>
		/// ��ȡ��ǰ�߳��д��IUnitOfWork��Stack
		/// </summary>
		private static Stack<IUnitOfWork> ThreadBoundUnitOfWorkStack
		{
			get { return LogicalThreadContext.GetData(UnitOfWorkStack) as Stack<IUnitOfWork>; }
		}

		/// <summary>
		/// �ѵ�ǰUnitOfWorkѹ�뵱ǰ�̵߳���������
		/// �����ǰ�߳��в���������ջ���򴴽�����ջ����������ջ���뵱ǰ�߳�
		/// ����ӵ�ǰ�߳���ȡ��ջ�����ѵ�ǰUnitOfWorkѹ��ջ��
		/// </summary>
		public static void Push(IUnitOfWork unitOfWork)
		{
			if (ThreadBoundUnitOfWorkStack == null)
			{
				var stack = new Stack<IUnitOfWork>();
				stack.Push(unitOfWork);
				LogicalThreadContext.SetData(UnitOfWorkStack, stack);
			}
			else
			{
				ThreadBoundUnitOfWorkStack.Push(unitOfWork);
			}
		}

		/// <summary>
		/// �ӵ�ǰ�̵߳�UnitOfWorkջ���Ƴ������Ķ���
		/// �����ǰ�߳��в���������ջ���򷵻�<see langword="null"></see>
		/// </summary>
		/// <returns>The UnitOfWork which at the top of Stack in the current thread.</returns>
		public static IUnitOfWork Pop()
		{
			if (ThreadBoundUnitOfWorkStack == null)
			{
				return null;
			}
			IUnitOfWork unitOfWork = ThreadBoundUnitOfWorkStack.Pop();
			if (ThreadBoundUnitOfWorkStack.Count == 0)
			{
				LogicalThreadContext.FreeNamedDataSlot(UnitOfWorkStack);
			}
			return unitOfWork;
		}

		#endregion
	}
}