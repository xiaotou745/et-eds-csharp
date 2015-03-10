using System;
using System.Runtime.InteropServices;
using System.Text;

namespace ETS.IO
{
	/// <summary>
	/// ����.ini �ļ��Ķ�д
	/// </summary>
	public class INIFileManager
	{
		/// <summary>
		/// �ļ�·��
		/// </summary>
		public string Path;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="iniPath"></param>
		public INIFileManager(string iniPath)
		{
			Path = iniPath;
		}

		#region ����

		[DllImport("kernel32")]
		private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

		[DllImport("kernel32")]
		private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal,
		                                                  int size, string filePath);

		[DllImport("kernel32")]
		private static extern int GetPrivateProfileString(string section, string key, string defVal, Byte[] retVal, int size,
		                                                  string filePath);

		#endregion

		#region  дINI

		/// <summary>
		/// дINI�ļ�
		/// </summary>
		/// <param name="section"></param>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void IniWriteValue(string section, string key, string value)
		{
			WritePrivateProfileString(section, key, value, Path);
		}

		#endregion

		#region ɾ��ini����

		/// <summary>
		/// ɾ��ini�ļ������ж���
		/// </summary>
		public void ClearAllSection()
		{
			IniWriteValue(null, null, null);
		}

		/// <summary>
		/// ɾ��ini�ļ���personal�����µ����м�
		/// </summary>
		/// <param name="section"></param>
		public void ClearSection(string section)
		{
			IniWriteValue(section, null, null);
		}

		#endregion

		#region ��ȡINI

		/// <summary>
		/// ��ȡINI�ļ�
		/// </summary>
		/// <param name="section"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public string IniReadValue(string section, string key)
		{
			var temp = new StringBuilder(255);
			int i = GetPrivateProfileString(section, key, "", temp, 255, Path);
			return temp.ToString();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="section"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public byte[] IniReadValues(string section, string key)
		{
			var temp = new byte[255];
			int i = GetPrivateProfileString(section, key, "", temp, 255, Path);
			return temp;
		}

		/// <summary>
		/// ��ȡini�ļ������ж�����
		/// </summary>    
		public string[] IniReadValues()
		{
			byte[] allSection = IniReadValues(null, null);
			return ByteToString(allSection);
		}

		/// <summary>
		/// ת��byte[]����Ϊstring[]�������� 
		/// </summary>
		/// <param name="sectionByte"></param>
		/// <returns></returns>
		private string[] ByteToString(byte[] sectionByte)
		{
			var ascii = new ASCIIEncoding();
			//��������key��string����
			string sections = ascii.GetString(sectionByte);
			//��ȡkey������
			string[] sectionList = sections.Split(new char[1] {'\0'});
			return sectionList;
		}

		/// <summary>
		/// ��ȡini�ļ���ĳ���������м���
		/// </summary>    
		public string[] IniReadValues(string section)
		{
			byte[] sectionByte = IniReadValues(section, null);
			return ByteToString(sectionByte);
		}

		#endregion
	}
}