using System.IO;
using System.Text;

namespace ETS.IO
{
    /// <summary>
    /// �ļ�I/O����
    /// </summary>
    public class FileIO
    {
        /// <summary>
        /// ������������������ļ�
        /// </summary>
        /// <param name="bitArray">binary array</param>
        /// <param name="savePath">file directory</param>
        public static void SaveBytes(byte[] bitArray, string savePath)
        {
            FileStream fs = new FileStream(savePath, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(bitArray, 0, bitArray.Length);
            fs.Close();
            fs.Dispose();
            bw.Close();
        }

        /// <summary>
        /// ������������������ļ�
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
        /// �����ı��ļ�
        /// </summary>
        /// <param name="fileName">file directory</param>
        /// <param name="txt">file content</param>
        /// <param name="encoding">encoding</param>
        public static void SaveTextFile(string fileName, string txt, Encoding encoding)
        {
            using (StreamWriter sw = new StreamWriter(fileName, false, encoding))
            {
                sw.Write(txt);
            }
        }

        #region �ļ�����
        /// <summary>
        /// ��ȡһ���ļ����ı�����
        /// </summary>
        /// <param name="FileFullPath">�ļ�����·��</param>
        /// <returns></returns>
        public static string GetFileContent(string FileFullPath, Encoding en)
        {
            string Content = "";
            Encoding enCode = Encoding.UTF8;
            if (en != null)
            {
                enCode = en;
            }
            if (System.IO.File.Exists(FileFullPath))
            {
                StreamReader sr = new StreamReader(FileFullPath, enCode);
                Content = sr.ReadToEnd();
                sr.Close();
            }
            return Content;
        }
        #endregion
    }
}