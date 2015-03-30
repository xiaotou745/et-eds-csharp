using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS.Page
{
    public class AjaxPageHelper
    {
        //<div data-ajax="true" data-ajax-currentpage="2" data-ajax-dataformid="#searchForm" data-ajax-method="Post" data-ajax-update="#dataList" data-firstpage="/Order/PostOrder" data-invalidpageerrmsg="页索引无效" data-maxpages="14" data-mvcpager="true" data-outrangeerrmsg="页索引超出范围" data-pageparameter="pageindex" data-urlformat="/Order/PostOrder?pageindex=__pageindex__" id="badoopager" style="float:right">
        //<a data-pageindex="1" href="/Order/PostOrder">首页</a>&nbsp;&nbsp;
        //<a data-pageindex="1" href="/Order/PostOrder">上一页</a>&nbsp;&nbsp;
        //<a data-pageindex="1" href="/Order/PostOrder">1</a>&nbsp;&nbsp;
        //<span class="current">2</span>&nbsp;&nbsp;
        // <a data-pageindex="11" href="/Order/PostOrder?pageindex=11">...</a>&nbsp;&nbsp;
        //<a data-pageindex="3" href="/Order/PostOrder?pageindex=3">下一页</a>&nbsp;&nbsp;
        //<a data-pageindex="14" href="/Order/PostOrder?pageindex=14">尾页</a>&nbsp;&nbsp;
        //<input type="text" value="2" data-pageindexbox="true">
        //<input type="button" data-submitbutton="true" value="跳转"></div>

        /// <summary>
        /// 评论ajax分页
        /// </summary>
        /// <param name="totleNum">总记录数</param>
        /// <param name="numPerPage">每页条数</param>
        /// <param name="thisPage">当前页数</param>
        /// <returns>分页HTML代码</returns>
        public static string ShowPageAjax(int totleNum, int numPerPage, int thisPage,string fromName,string updateName,string url)
        {
            if (totleNum <= numPerPage) { return ""; }
            int AllPage = (int)Math.Ceiling((float)totleNum / numPerPage); //求总页数
            string thisurl = url + "?pageindex=" + "{0}";//获取当前页URL地址
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(
                @"<div data-ajax='true' data-ajax-currentpage='{0}' data-ajax-dataformid='#{1}' data-ajax-method='Post' data-ajax-update='#{2}' data-firstpage='{3}' data-invalidpageerrmsg='页索引无效' data-maxpages='{4}' data-mvcpager='true' data-outrangeerrmsg='页索引超出范围' data-pageparameter='pageindex' data-urlformat='{3}?pageindex=__pageindex__' id='badoopager' style='float:right'>",
                thisPage, fromName, updateName, url, AllPage);  
            string PageHtml1 = "";
            if (thisPage > 1)
            {
                sb.AppendFormat("<a href=\"{0}\" data-pageindex='{1}'>上一页</a>&nbsp;&nbsp;", string.Format(thisurl, thisPage - 1),thisPage-1);
            }
            else
            {
                sb.AppendFormat("<a href=\"{0}\" data-pageindex='1' >上一页</a>&nbsp;&nbsp;", url);
            }
            int shownum = 8;     //显示相邻页数量
            int haveshownum = 1; //已经显示相邻页数量 

            int prenum = shownum / 2; //默认前面显示4页
            if (AllPage - thisPage < prenum) //如果后面多余
            {
                prenum = shownum - (AllPage - thisPage); //重新计算前面显示页数
            }
            if ((thisPage - prenum) > 1)
            {
                sb.AppendFormat("<a  href=\"{0}\" data-pageindex='{1}'>1</a>&nbsp;&nbsp;", string.Format(thisurl, thisPage), thisPage);
            }
            if ((thisPage - prenum) > 2)
            {
                sb.AppendFormat("<a href=\"{0}\" data-pageindex='{1}'>...</a>&nbsp;&nbsp;", string.Format(thisurl, thisPage), thisPage);
            }
            int realPrePage = 0;
            //显示前相邻页
            for (int loop = thisPage - 1; loop >= 1 && haveshownum <= prenum; loop--)
            {
                realPrePage++;
                haveshownum++;
                PageHtml1 = " <a href=\"" + string.Format(thisurl, loop) + "\" data-pageindex=\"" + thisPage + "\">" + loop + "</a>&nbsp;&nbsp;" + PageHtml1;
            }
            sb.Append(PageHtml1 + " <<span class=\"current\">"+thisPage+"</span>&nbsp;&nbsp;");//本页 
            //显示后相邻页
            for (int loop = thisPage + 1; loop <= AllPage && haveshownum <= shownum; loop++)
            {
                haveshownum++;
                sb.Append(" <a href=\"" + string.Format(thisurl, loop) + "\" data-pageindex=\"" + thisPage + "\">" + loop + "</a>&nbsp;&nbsp;");
            }

            if (thisPage + (shownum - realPrePage) < AllPage)
            {
                sb.AppendFormat("<a href=\"{0}\" data-pageindex='{1}'>...</a>&nbsp;&nbsp;", string.Format(thisurl, thisPage), thisPage);
            }

            if (thisPage < AllPage)
            {
                //<a data-pageindex="3" href="/Order/PostOrder?pageindex=3">下一页</a>&nbsp;&nbsp;
                //<a data-pageindex="14" href="/Order/PostOrder?pageindex=14">尾页</a>&nbsp;&nbsp;
              

                sb.AppendFormat("<a  href=\"{0}\" data-pageindex='{1}'>下一页</a>&nbsp;&nbsp;", string.Format(thisurl, thisPage + 1),thisPage+1);
                sb.AppendFormat("<a  href=\"{0}\" data-pageindex='{1}'>尾页</a>&nbsp;&nbsp;", string.Format(thisurl, AllPage),thisPage);
            }
            else
            {
                sb.Append("<a disabled=\"disabled\">下一页</a>");
                sb.Append("<a disabled=\"disabled\">尾页</a>");
            }
            sb.AppendFormat("<input type=\"text\" value=\"{0}\" data-pageindexbox=\"true\">",thisPage);
            sb.Append("<input type=\"button\" data-submitbutton=\"true\" value=\"跳转\"></div>");
            return sb.ToString(); ;
        } 
    }
}
