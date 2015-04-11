using Ets.Model.ParameterModel.Clienter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuperManWebApi.Providers
{
    public class ImageProvider
    {
        public static List<string> ReceiptPicConvert(string receiptPic)
        {
            string[] receiptPicArray = receiptPic.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> listReceiptPic = new List<string>();
            for (int i = 0; i < receiptPicArray.Length; i++)
            {
                string rPic = ETS.Util.ConfigSettings.Instance.PicHost + CustomerIconUploader.Instance.RelativePath + receiptPicArray[i];
                listReceiptPic.Add(rPic);
            }
            return listReceiptPic;
        }
    }
}