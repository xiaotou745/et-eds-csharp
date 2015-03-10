using System;
using System.Runtime.InteropServices;
using System.Text;

namespace ETS.IO
{
	/// <summary>
	/// 操作.ini 文件的读写
	/// </summary>
	public class INIFileManager
	{
		/// <summary>
		/// 文件路径
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

		#region 声明

		[DllImport("kernel32")]
		private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

		[DllImport("kernel32")]
		private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal,
		                                                  int size, string filePath);

		[DllImport("kernel32")]
		private static extern int GetPrivateProfileString(string section, string key, string defVal, Byte[] retVal, int size,
		                                                  string filePath);

		#endregion

		#region  写INI

		/// <summary>
		/// 写INI文件
		/// </summary>
		/// <param name="section"></param>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void IniWriteValue(string section, string key, string value)
		{
			WritePrivateProfileString(section, key, value, Path);
		}

		#endregion

		#region 删除ini配置

		/// <summary>
		/// 删除ini文件下所有段落
		/// </summary>
		public void ClearAllSection()
		{
			IniWriteValue(null, null, null);
		}

		/// <summary>
		/// 删除ini文件下personal段落下的所有键
		/// </summary>
		/// <param name="section"></param>
		public void ClearSection(string section)
		{
			IniWriteValue(section, null, null);
		}

		#endregion

		#region 读取INI

		/// <summary>
		/// 读取INI文件
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
		/// 读取ini文件的所有段落名
		/// </summary>    
		public string[] IniReadValues()
		{
			byte[] allSection = IniReadValues(null, null);
			return ByteToString(allSection);
		}

		/// <summary>
		/// 转换byte[]类型为string[]数组类型 
		/// </summary>
		/// <param name="sectionByte"></param>
		/// <returns></returns>
		private string[] ByteToString(byte[] sectionByte)
		{
			var ascii = new ASCIIEncoding();
			//编码所有key的string类型
			string sections = ascii.GetString(sectionByte);
			//获取key的数组
			string[] sectionList = sections.Split(new char[1] {'\0'});
			return sectionList;
		}

		/// <summary>
		/// 读取ini文件的某段落下所有键名
		/// </summary>    
		public string[] IniReadValues(string section)
		{
			byte[] sectionByte = IniReadValues(section, null);
			return ByteToString(sectionByte);
		}

		#endregion
	}
}