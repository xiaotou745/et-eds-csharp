using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Task.Common
{
    public class SendEmailCommon
    {
        /// <summary>
        ///     单元格边框样式
        /// </summary>
        private const string tdBorderStyle = "style=\"border:1px solid #B1CDE3;padding:4px;\"";

        /// <summary>
        ///     表格边框样式
        /// </summary>
        private const string tableBorderStyle = "style=\"border-collapse: collapse; border:1px solid #B1CDE3\"";

        /// <summary>
        ///     拼接邮件内容
        /// </summary>
        /// <typeparam name="H"></typeparam>
        /// <typeparam name="B"></typeparam>
        /// <param name="h"></param>
        /// <param name="b"></param>
        /// <param name="Description"></param>
        /// <returns></returns>
        public static StringBuilder CreateHtmlBody<B>(Dictionary<string, string> dicHead, IList<B> b)
        {
            var sbHtmlBody = new StringBuilder();
            try
            {
                int line = 1;
                sbHtmlBody.AppendFormat("<table {0}><tr><th {1}> 序号 </th>", tableBorderStyle, tdBorderStyle);
                foreach (string key in dicHead.Keys)
                {
                    sbHtmlBody.AppendFormat("<th {0}> {1} </th>", tdBorderStyle, dicHead[key]);
                }
                sbHtmlBody.Append("</tr>");
                // 新增只处理500条。
                foreach (B bitem in b.Take(Parameters.MaxCountForEWS))
                {
                    sbHtmlBody.Append("<tr>");
                    sbHtmlBody.AppendFormat("<td {1}>{0}</td>", line++, tdBorderStyle);
                    foreach (string hitem in dicHead.Keys)
                    {
                        sbHtmlBody.AppendFormat("<td {1}>{0}</td>", bitem.GetPropertyValue(hitem), tdBorderStyle);
                    }
                    sbHtmlBody.Append("</tr>");
                }
                sbHtmlBody.Append("</table>");
                return sbHtmlBody;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}