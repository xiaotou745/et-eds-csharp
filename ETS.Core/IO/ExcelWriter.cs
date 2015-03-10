using System;
using System.IO;
using System.Text;

namespace ETS.IO
{
	/// <summary>
	/// ͨ���ļ��ṹֱ������xls�ļ�
	/// </summary>
	public class ExcelWriter
	{
		FileStream _wirter;
		public ExcelWriter(string strPath)
		{
			_wirter = new FileStream(strPath, FileMode.OpenOrCreate);
		}
		/// <summary>
		/// д��short����
		/// </summary>
		/// <param name="values"></param>
		private void _writeFile(short[] values)
		{
			foreach (short v in values)
			{
				byte[] b = BitConverter.GetBytes(v);
				_wirter.Write(b, 0, b.Length);
			}
		}
		/// <summary>
		/// д�ļ�ͷ
		/// </summary>
		public void BeginWrite()
		{
			_writeFile(new short[] { 0x809, 8, 0, 0x10, 0, 0 });
		}
		/// <summary>
		/// д�ļ�β
		/// </summary>
		public void EndWrite()
		{
			_writeFile(new short[] { 0xa, 0 });
			_wirter.Close();
		}
		/// <summary>
		/// дһ�����ֵ���Ԫ��x,y
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="value"></param>
		public void WriteNumber(short x, short y, double value)
		{
			_writeFile(new short[] { 0x203, 14, x, y, 0 });
			byte[] b = BitConverter.GetBytes(value);
			_wirter.Write(b, 0, b.Length);
		}
		/// <summary>
		/// дһ���ַ�����Ԫ��x,y
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="value"></param>
		public void WriteString(short x, short y, string value)
		{
			byte[] b = Encoding.Default.GetBytes(value);
			_writeFile(new short[] { 0x204, (short)(b.Length + 8), x, y, 0, (short)b.Length });
			_wirter.Write(b, 0, b.Length);
		}
	}
}