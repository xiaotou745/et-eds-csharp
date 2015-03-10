using System.IO;
using System.Text;

namespace ETS.IO
{
	/// <summary>
	/// 文件I/O操作
	/// </summary>
	public  class FileIO
	{
		/// <summary>
		/// 保存二进制数据流到文件
		/// </summary>
		/// <param name="bitArray">binary array</param>
		/// <param name="savePath">file directory</param>
		public static void SaveBytes(byte[] bitArray, string savePath)
		{
			FileStream fs = new FileStream(savePath, FileMode.Create);
			BinaryWriter bw = new BinaryWriter(fs);
			bw.Write(bitArray,0,bitArray.Length);
			fs.Close();
			fs.Dispose();
			bw.Close();
		}

        /// <summary>
        /// 保存二进制数据流到文件
        /// </summary>
        /// <param name="bitArray">binary array</param>
        /// <param name="savePath">file directory</param>
        public static void SaveBytesByStep(byte[] dataArray, string savePath, int Step)
        {
            using (FileStream fileStream = new FileStream(savePath, FileMode.Create))
            {
                // Write the data to the file, byte by byte.
                for (int i = 0; i < dataArray.Length; i++)
                {
                    fileStream.WriteByte(dataArray[i]);
                }
                // Set the stream position to the beginning of the file.
                //fileStream.Seek(0, SeekOrigin.Begin);
            }
        }

		/// <summary>
		/// 保存文本文件
		/// </summary>
		/// <param name="fileName">file directory</param>
		/// <param name="txt">file content</param>
		/// <param name="encoding">encoding</param>
		public static void SaveTextFile(string fileName, string txt,Encoding encoding)
		{
			using (StreamWriter sw = new StreamWriter(fileName, false, encoding))
			{
				sw.Write(txt);
			}
		}
	}
}