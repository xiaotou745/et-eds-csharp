using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using Common.Logging;

namespace ETS.Util
{
	/// <summary>
	/// 发送邮件工具类
	/// </summary>
	/// <remarks>
	/// 1、收件人仅包含内部地址（以@vancl.cn为后缀），请使用匿名账号连接smtpsrv02.vancloa.cn，可自定义发件人地址；
	/// 2、自定义的发件人邮件地址（MAILFROM）和域名关系的建议：
	///		外发的与Vancl业务直接相关的后缀为@vancl.com，一般是连接SMTPSRV01的；
	///		外发的与Vjia业务直接相关的后缀为@vjia.com，一般是连接SMTPSRV03的；
	///		只是内发的和业务无直接关系的后缀为@vancl.cn，一般是连接SMTPSRV02的；
	/// </remarks>
	public class MailUtils
	{
		/// <summary>
		/// Vancl匿名邮件发送服务器
		/// </summary>
		public const string ANONYMOUS_SMTP = "smtpsrv02.vancloa.cn";

		private static readonly ILog logger = LogManager.GetCurrentClassLogger();

		#region Common MailSend Methods.

		/// <summary>
		/// 发送邮件
		/// </summary>
		/// <param name="mailServer">服务器地址</param>
		/// <param name="credential">NetworkCredential</param>
		/// <param name="mail">MailMessage</param>
		public static void SendMail(string mailServer, NetworkCredential credential, MailMessage mail)
		{
			var sc = new SmtpClient(mailServer) {Credentials = credential, DeliveryMethod = SmtpDeliveryMethod.Network};
			sc.SendAsync(mail, null);
		}

		/// <summary>
		/// 发送邮件
		/// </summary>
		/// <param name="mailServer">服务器地址</param>
		/// <param name="port">端口</param>
		/// <param name="credential">NetworkCredential</param>
		/// <param name="mail">MailMessage</param>
		public static void SendMail(string mailServer, int port, NetworkCredential credential, MailMessage mail)
		{
			var sc = new SmtpClient(mailServer, port)
			         	{
			         		Credentials = credential,
			         		DeliveryMethod = SmtpDeliveryMethod.Network
			         	};
			sc.SendAsync(mail, null);
		}

		/// <summary>
		/// 发送邮件
		/// </summary>
		/// <param name="mailServer">邮件服务器地址</param>
		/// <param name="mailUser">发件人帐号</param>
		/// <param name="mailPwd">发件人密码</param>
		/// <param name="mailAddress">收件人Email地址(多个以分号隔开)</param>
		/// <param name="mailFrom">发件人Email地址</param>
		/// <param name="subject">标题</param>
		/// <param name="body">正文</param>
		/// <param name="isHtml">正文是否HTML格式</param>
		public static void SendMail(string mailServer, string mailUser, string mailPwd, string mailAddress, string mailFrom,
		                            string subject, string body, bool isHtml)
		{
			var mail = new MailMessage();
			var fromAddress = new MailAddress(mailFrom);
			mail.From = fromAddress;
			mail.Subject = subject;
			mail.Body = body;
			string[] arrAddress = mailAddress.Split(';');
			foreach (string address in arrAddress)
			{
				if (!string.IsNullOrEmpty(address))
				{
					mail.To.Add(address);
				}
			}
			mail.IsBodyHtml = isHtml;
			mail.SubjectEncoding = Encoding.GetEncoding("gb2312");
			mail.BodyEncoding = Encoding.GetEncoding("gb2312");
			var credential = new NetworkCredential(mailUser, mailPwd);
			SendMail(mailServer, credential, mail);
		}

		/// <summary>
		/// 发送邮件
		/// </summary>
		/// <param name="mailServer">邮件服务器地址</param>
		/// <param name="mailUser">发件人帐号</param>
		/// <param name="mailPwd">发件人密码</param>
		/// <param name="mailAddress">收件人Email地址(多个以分号隔开)</param>
		/// <param name="mailFrom">发件人Email地址</param>
		/// <param name="subject">标题</param>
		/// <param name="body">正文</param>
		public static void SendMail(string mailServer, string mailUser, string mailPwd, string mailAddress, string mailFrom,
		                            string subject, string body)
		{
			SendMail(mailServer, mailUser, mailPwd, mailAddress, mailFrom, subject, body, false);
		}

		/// <summary>
		/// 发送邮件
		/// </summary>
		/// <param name="mailServer">邮件服务器地址</param>
		/// <param name="mailUser">发件人帐号</param>
		/// <param name="mailPwd">发件人密码</param>
		/// <param name="mailAddress">收件人Email地址(多个以分号隔开)</param>
		/// <param name="mailFrom">发件人Email地址</param>
		/// <param name="cc">抄送</param>
		/// <param name="bcc">密送</param>
		/// <param name="subject">标题</param>
		/// <param name="body">正文</param>
		public static void SendMail(string mailServer, string mailUser, string mailPwd, string mailAddress, string cc,
		                            string bcc,
		                            string mailFrom,
		                            string subject, string body)
		{
			SendMail(mailServer, mailUser, mailPwd, mailAddress, cc, bcc, mailFrom, subject, body, false);
		}

		/// <summary>
		/// 发送邮件
		/// </summary>
		/// <param name="mailServer">邮件服务器地址</param>
		/// <param name="port">端口</param>
		/// <param name="mailUser">发件人帐号</param>
		/// <param name="mailPwd">发件人密码</param>
		/// <param name="mailAddress">收件人Email地址(多个以分号隔开)</param>
		/// <param name="mailFrom">发件人Email地址</param>
		/// <param name="subject">标题</param>
		/// <param name="body">正文</param>
		/// <param name="isHtml"></param>
		public static void SendMail(string mailServer, int port, string mailUser, string mailPwd, string mailAddress,
		                            string mailFrom,
		                            string subject, string body, bool isHtml)
		{
			var mail = new MailMessage();
			var fromAddress = new MailAddress(mailFrom);
			mail.From = fromAddress;
			mail.Subject = subject;
			mail.Body = body;
			string[] arrAddress = mailAddress.Split(';');
			foreach (string address in arrAddress)
			{
				mail.To.Add(address);
			}
			mail.IsBodyHtml = isHtml;
			mail.SubjectEncoding = Encoding.GetEncoding("gb2312");
			mail.BodyEncoding = Encoding.GetEncoding("gb2312");
			var credential = new NetworkCredential(mailUser, mailPwd);
			SendMail(mailServer, port, credential, mail);
		}


		/// <summary>
		/// 发送带多个附件的邮件
		/// 发送带附件邮件邮件
		/// </summary>
		/// <param name="mailPwd"></param>
		/// <param name="mailAddress"></param>
		/// <param name="mailFrom"></param>
		/// <param name="mailSubject"></param>
		/// <param name="mailBody"></param>
		/// <param name="filePathes">附件地址列表</param>
		/// <param name="mailServer"></param>
		/// <param name="mailUser"></param>
		public static void SendMailByAttachments(string mailServer, string mailUser, string mailPwd,
		                                         string mailAddress, string mailFrom, string mailSubject, string mailBody,
		                                         IList<string> filePathes)
		{
			var mail = new MailMessage();
			var fromAddress = new MailAddress(mailFrom);
			mail.From = fromAddress;
			string[] arrTo = mailAddress.Split(';');
			foreach (string t in arrTo)
			{
				mail.To.Add(new MailAddress(t));
			}
			mail.Subject = mailSubject;
			mail.Body = mailBody;
			mail.IsBodyHtml = true;

			foreach (string path in filePathes)
			{
				mail.Attachments.Add(new Attachment(path));
			}

			mail.SubjectEncoding = Encoding.GetEncoding("gb2312");
			mail.BodyEncoding = Encoding.GetEncoding("gb2312");
			var credential = new NetworkCredential(mailUser, mailPwd);
			SendMail(mailServer, credential, mail);
		}

		/// <summary>
		/// 发送带多个附件的邮件
		/// 发送带附件邮件邮件
		/// </summary>
		/// <param name="mailContent"></param>
		/// <param name="mailAddress"></param>
		/// <param name="mailSubject"></param>
		/// <param name="fileStreams"></param>
		public static void SendMailByAttachments(string mailSubject, string mailContent, string mailAddress,
		                                         IList<Stream> fileStreams)
		{
			var mail = new MailMessage();
			var fromAddress = new MailAddress("crm@vancl.cn");
			mail.From = fromAddress;
			string[] arrTo = mailAddress.Split(';');
			foreach (string t in arrTo)
			{
				mail.To.Add(new MailAddress(t));
			}
			mail.Subject = mailSubject;
			mail.Body = mailContent;
			mail.IsBodyHtml = true;

			foreach (Stream mFile in fileStreams)
			{
				string fileName;
				PropertyInfo fileNmPro = mFile.GetType().GetProperty("FileNm");
				if (fileNmPro != null)
				{
					fileName = fileNmPro.GetValue(mFile, null).ToString();
				}
				else
				{
					fileName = "Attachment.xls";
				}
				var fileAttachment = new Attachment(mFile, fileName) {TransferEncoding = TransferEncoding.Base64};
				mail.Attachments.Add(fileAttachment);
			}

			mail.SubjectEncoding = Encoding.GetEncoding("gb2312");
			mail.BodyEncoding = Encoding.GetEncoding("gb2312");
			var credential = new NetworkCredential("crm@vancloa.cn", ".654sy56kj67dgb577ks");
			var sc = new SmtpClient(ANONYMOUS_SMTP) {Credentials = credential, DeliveryMethod = SmtpDeliveryMethod.Network};
			sc.Send(mail);
		}

		public static void SendMailByPictures(string to, string mailSubject, string mailBody, List<string> filePathes)
		{
			var mail = new MailMessage();
			var fromAddress = new MailAddress("crm@vancl.cn");
			mail.From = fromAddress;
			string[] arrTo = to.Split(';');
			foreach (string t in arrTo)
			{
				mail.To.Add(new MailAddress(t));
			}
			mail.Subject = mailSubject;

			//mail.AlternateViews.Add(AlternateView.CreateAlternateViewFromString("图片", null, "text/plain"));

			AlternateView htmlBody = AlternateView.CreateAlternateViewFromString(mailBody + "<img src=\"cid:report\">", null,
			                                                                     "text/html");
			var lrImage = new LinkedResource(filePathes[0], "image/gif");
			lrImage.ContentId = "report";
			htmlBody.LinkedResources.Add(lrImage);
			mail.AlternateViews.Add(htmlBody);

			mail.Body = mailBody;
			mail.IsBodyHtml = true;

			mail.SubjectEncoding = Encoding.GetEncoding("gb2312");
			mail.BodyEncoding = Encoding.GetEncoding("gb2312");
			var credential = new NetworkCredential("crm@vancloa.cn", ".654sy56kj67dgb577ks");
			var sc = new SmtpClient(ANONYMOUS_SMTP) {Credentials = credential, DeliveryMethod = SmtpDeliveryMethod.Network};
			sc.Send(mail);
		}

		/// <summary>
		/// 发送邮件
		/// </summary>
		/// <param name="mailServer">邮件服务器地址</param>
		/// <param name="mailUser">发件人帐号</param>
		/// <param name="mailPwd">发件人密码</param>
		/// <param name="mailAddress">收件人Email地址(多个以分号隔开)</param>
		/// <param name="mailCc">抄送</param>
		/// <param name="mailBcc">密送</param>
		/// <param name="mailFrom">发件人Email地址</param>
		/// <param name="subject">标题</param>
		/// <param name="body">正文</param>
		/// <param name="isHtml">正文是否HTML格式</param>
		public static void SendMail(string mailServer, string mailUser, string mailPwd, string mailAddress, string mailCc,
		                            string mailBcc, string mailFrom,
		                            string subject, string body, bool isHtml)
		{
			var mail = new MailMessage();
			var fromAddress = new MailAddress(mailFrom);
			mail.From = fromAddress;
			mail.Subject = subject;
			mail.Body = body;
			string[] arrAddress = mailAddress.Split(';');
			foreach (string address in arrAddress)
			{
				if (!string.IsNullOrEmpty(address))
				{
					mail.To.Add(address);
				}
			}
			string[] arrCc = mailCc.Split(';');
			foreach (string cc in arrCc)
			{
				if (!string.IsNullOrEmpty(cc))
				{
					mail.CC.Add(cc);
				}
			}
			string[] arrBcc = mailBcc.Split(';');
			foreach (string bcc in arrBcc)
			{
				if (!string.IsNullOrEmpty(bcc))
				{
					mail.Bcc.Add(bcc);
				}
			}

			mail.IsBodyHtml = isHtml;
			mail.SubjectEncoding = Encoding.GetEncoding("gb2312");
			mail.BodyEncoding = Encoding.GetEncoding("gb2312");
			var credential = new NetworkCredential(mailUser, mailPwd);
			SendMail(mailServer, credential, mail);
		}

		#endregion

		public static bool SendMailOfSCMWithDns(string mailSubject, string mailContent, string mailAddress, string mailFrom)
		{
			mailSubject = mailSubject + String.Format("(ServerHost Ip/Name:{0}/{1})", DnsUtils.HostIp, DnsUtils.HostName);
			mailContent = String.Format("(ServerHost Ip/Name:{0}/{1})\r\n", DnsUtils.HostIp, DnsUtils.HostName) + mailContent;

			return SendMailOfSCM(mailSubject, mailContent, mailAddress, mailFrom, false);
		}

		public static bool SendMailOfSCMWithDns(string mailSubject, string mailContent, string mailAddress)
		{
			return SendMailOfSCMWithDns(mailSubject, mailContent, mailAddress, "scm@vancl.cn");
		}

		public static bool SendMailOfSCM(String mailSubject, string mailContent, string mailAddress, string mailFrom, bool isHtml)
		{
			try
			{
				SendMail(ANONYMOUS_SMTP, "scm", "scm", mailAddress, mailFrom, mailSubject, mailContent, isHtml);
			}
			catch (Exception e)
			{
				logger.Error(m => m("SendMailToUser faild!", e));
				return false;
			}
			return true;
		}

		public static bool SendMailOfSCM(String mailSubject, string mailContent, string mailAddress)
		{
			return SendMailOfSCM(mailSubject, mailContent, mailAddress, "scm@vancl.cn", false);
		}
	}
}