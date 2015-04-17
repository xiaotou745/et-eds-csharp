using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.Common
{
    public class ImageCommon
    {
        public static List<string> ReceiptPicConvert(string receiptPic)
        {
            string[] receiptPicArray = receiptPic.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> listReceiptPic = new List<string>();
            for (int i = 0; i < receiptPicArray.Length; i++)
            {
                string rPic = ETS.Util.ConfigSettings.Instance.PicHost + Ets.Model.ParameterModel.Clienter.CustomerIconUploader.Instance.RelativePath + receiptPicArray[i];
                listReceiptPic.Add(rPic);
            }
            return listReceiptPic;
        }

        public static List<string> GetListImgString(string receiptPic)
        {
            string[] receiptPicArray = receiptPic.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> listReceiptPic = new List<string>();
            for (int i = 0; i < receiptPicArray.Length; i++)
            {
                //string rPic = ETS.Util.ConfigSettings.Instance.PicHost + Ets.Model.ParameterModel.Clienter.CustomerIconUploader.Instance.RelativePath + receiptPicArray[i];
                listReceiptPic.Add(receiptPicArray[i]);
            }
            return listReceiptPic;
        } 

        //public static List<string>  GetPicDir(string )
    }
}
