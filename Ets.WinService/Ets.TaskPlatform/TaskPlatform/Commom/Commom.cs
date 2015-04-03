using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace TaskPlatform.Commom
{
    public class Commom
    {
        /// <summary>
        /// 得到红色字体
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string GetRedFont(object content)
        {
            return string.Format("<span style=\"color: Red;\">{0}</span>", content);
        }

        /// <summary>
        /// 得到蓝色字体
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string GetBlueFont(object content)
        {
            return string.Format("<span style=\"color: Blue;\">{0}</span>", content);
        }

        /// <summary>
        /// 得到绿色字体
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string GetGreenFont(object content)
        {
            return string.Format("<span style=\"color: Green;\">{0}</span>", content);
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="body"></param>
        public static void SendEmail(string body, string emailAddress)
        {
            try
            {
                string address = "";
                string displayName = "";
                string[] mailNames = emailAddress.Split(';');
                MailAddress from = new MailAddress("wang.xudan@etaostars.com", "e代送Worker检查服务");
                SmtpClient client = new SmtpClient();
                client.Host = "smtp.exmail.qq.com";
                client.Credentials = new System.Net.NetworkCredential("wang.xudan@etaostars.com", "asd123");
                client.Port = 25;
                MailMessage mail = new MailMessage();
                mail.From = from;
                foreach (string name in mailNames)
                {
                    if (name != string.Empty)
                    {
                        if (name.IndexOf('<') > 0)
                        {
                            displayName = name.Substring(0, name.IndexOf('<'));
                            address = name.Substring(name.IndexOf('<') + 1).Replace('>', ' ');
                        }
                        else
                        {
                            displayName = string.Empty;
                            address = name.Substring(name.IndexOf('<') + 1).Replace('>', ' ');
                        }
                        mail.To.Add(new MailAddress(address, displayName));
                    }
                }
                mail.Subject = "e代送Worker检查服务报警(来自计划任务平台：" + Environment.MachineName + ")";
                mail.Body = body;
                mail.BodyEncoding = System.Text.Encoding.UTF8;
                mail.IsBodyHtml = false;
                // 回复至
                mail.ReplyToList.Add("wang.xudan@etaostars.com");
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Send(mail);
            }
            catch { }
        }

        /// <summary> 
        /// 将文件大小(字节)转换为最适合的显示方式 
        /// </summary> 
        /// <param name="size"></param> 
        /// <returns></returns> 
        public static string ConvertFileSize(long size)
        {
            string result = "0KB";
            int filelength = size.ToString().Length;
            if (filelength < 4)
                result = size + "byte";
            else if (filelength < 7)
                result = Math.Round(Convert.ToDouble(size / 1024d), 2) + "KB";
            else if (filelength < 10)
                result = Math.Round(Convert.ToDouble(size / 1024d / 1024), 2) + "MB";
            else if (filelength < 13)
                result = Math.Round(Convert.ToDouble(size / 1024d / 1024 / 1024), 2) + "GB";
            else
                result = Math.Round(Convert.ToDouble(size / 1024d / 1024 / 1024 / 1024), 2) + "TB";
            return result;
        }
    }
}
