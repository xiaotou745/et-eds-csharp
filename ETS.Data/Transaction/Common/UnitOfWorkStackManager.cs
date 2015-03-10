using System.Collections.Generic;
using Common.Logging;
using ETS.Data;
using ETS.Threading;

namespace ETS.Transaction.Common
{
	/// <summary>
	/// UnitOfWork的线程栈管理类
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
		/// 线程中<see cref="IUnitOfWork"/>栈的resource key
		/// </summary>
		public const string UnitOfWorkStack = "wyc:unitOfWorkStackResource";

		#region ThreadBoundedStack of UnitOfWork

		/// <summary>
		/// UnitOfWork是否已存在
		/// 如果已存在，则当前要创建的UnitOfWork为嵌套事务
		/// 如果线程中不存在UnitOfWork栈，则证明当前要创建的UnitOfWork为最外层事务
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
		/// 返回当前线程中的Stack中UnitOfWork的数量
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
		/// 获取当前线程中存放IUnitOfWork的Stack
		/// </summary>
		private static Stack<IUnitOfWork> ThreadBoundUnitOfWorkStack
		{
			get { return LogicalThreadContext.GetData(UnitOfWorkStack) as Stack<IUnitOfWork>; }
		}

		/// <summary>
		/// 把当前UnitOfWork压入当前线程的事务线中
		/// 如果当前线程中不存在事务栈，则创建事务栈，并把事务栈放入当前线程
		/// 否则从当前线程中取出栈，并把当前UnitOfWork压入栈中
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
		/// 从当前线程的UnitOfWork栈中移除顶部的对象
		/// 如果当前线程中不存在事务栈，则返回<see langword="null"></see>
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