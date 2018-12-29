/************************************************************************
 * 文件名：PagingHelper
 * 文件功能描述：分页操作对象方法类
 * 作    者：hjj
 * 创建日期：2017-06-15
 * 修 改 人：
 * 修改日期：
 * 修改原因：
 * Copyright (c) 2016 Titan.Han . All Rights Reserved. 
 * ***********************************************************************/
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Titan.Infrastructure.Domain
{
    public static class PagingHelper
    {
        //HtmlHelper扩展方法，用于分页
        public static MvcHtmlString Pagination(this HtmlHelper html, PagingInfo pageInfo, Func<PagingInfo, string> pageLinks)
        {
            var htmlString = pageLinks(pageInfo);

            return MvcHtmlString.Create(htmlString);
        }

        /// <summary>
        /// List页面分页
        /// </summary>
        /// <param name="html"></param>
        /// <param name="pageInfo"></param>
        /// <param name="paramList"></param>
        /// <returns></returns>
        public static MvcHtmlString NewPagination(this HtmlHelper html, PagingInfo pageInfo, params string[] paramList)
        {
            if (pageInfo.PageCount > 0)
            {
                List<string> newparamList = new List<string>();
                foreach (string item in paramList)
                {
                    if (item == null || item == string.Empty)
                    {
                        newparamList.Add("\'\'");
                    }
                    else
                    {
                        newparamList.Add("'" + item + "'");
                    }
                }
                var pagingString = string.Empty;
                //var pagingString = "<ul class=\"pagination\">";
                //新增内容
                pagingString += "<span class=\"num\">当前";
                pagingString += "<span class=\"record\">" + pageInfo.PageIndex + "</span>";
                pagingString += "<span class=\"num\">页，共" + pageInfo.PageCount + "页，</span>";
                pagingString += "<span class=\"num\">" + pageInfo.TotalItems + "</span>";
                pagingString += "<span class=\"num\">条记录</span>";
                pagingString += " <span class=\"seat\"></span>";
                //end新增内容
                string csum = string.Empty;
                foreach (var csitem in newparamList)
                {
                    csum += csitem + ",";
                }
                pagingString += "<span class=\"record\"> <a id=\"gotoFirstPage\" href=\"#\" onclick=\"pageSumit(" + csum + "" + 1 + "," + pageInfo.PageSize + ") \">" + "首页" + "</a></span>";
                if (pageInfo.PageIndex - 1 > 0)
                {
                    pagingString += "<span class=\"record\"><a href=\"#\" onclick=\"pageSumit2(" + csum + "" + (pageInfo.PageIndex - 1) + "," + pageInfo.PageSize + "," + (pageInfo.PageIndex - 1) + ") \">" + "上一页" + "</a></span>";
                }
                else
                {
                    pagingString += "<span class=\"record\"><a href=\"#\"  href=\"#\">" + "上一页" + "</a></span>";
                }
                for (var i = 1; i <= pageInfo.PageCount; i++)
                {
                    if (i == pageInfo.PageIndex)
                    {
                        pagingString += "<span style=\"cursor:pointer\" class=\"selecthid record\" id=\"s" + i + "\" ><a href=\"#\" style=\"color: red\" >" + i + "</a><a id=\"PageSubmitRefresh\" href=\"#\" style=\"color: red;display:none;\" onclick=\"pageSumit2(" + csum + "" + i + "," + pageInfo.PageSize + "," + i + ") \"></a></span>";
                    }
                    else {
                        pagingString += "<span style=\"cursor:pointer\" onclick=\"pageSumit2(" + csum + "" + i + "," + pageInfo.PageSize + "," + i + ") \" class=\"selecthid record\" id=\"s" + i + "\"  > <a href=\"#\" >" + i + "</a></span>";
                    }
                }

                if (pageInfo.PageIndex - pageInfo.PageCount < 0)
                {
                    pagingString += "<span class=\"record\"><a href=\"#\" onclick=\"pageSumit2(" + csum + "" + (pageInfo.PageIndex + 1) + "," + pageInfo.PageSize + "," + (pageInfo.PageIndex + 1) + ") \">" + "下一页" + "</a></span>";
                }
                else
                {
                    pagingString += "<span class=\"record\"><a href=\"#\"  href=\"#\">" + "下一页" + "</a></span>";
                }
                pagingString += "<span class=\"record\"><a href=\"#\" onclick=\"pageSumit2(" + csum + "" + pageInfo.PageCount + "," + pageInfo.PageSize + "," + pageInfo.PageCount + ") \" >" + "末页" + "</a></span>";

                //新增内容
                //  pagingString += "<li style=\"float:right;height:50px;width:50px;font-size:14px\" > <input id=\"tzpage\" type=\"text\"   class=\"input_1\" style=\"width: 30px;height:30px;text-align:center\" />页</li>";
                //  pagingString += "<li style=\"float:right;height:50px;width:50px; margin-left:10px;font-size:14px\" >  <input type=\"button\" style=\"width: 40px;height:30px;\"  onclick=\"selectPage(" + csum + "" + pageInfo.PageSize + "," + pageInfo.PageCount + ")\" value=\"跳转\"  /></li>";
                //end新增内容

                // pagingString += "</ul>";
                return MvcHtmlString.Create(pagingString);
            }
            else {
                List<string> newparamList = new List<string>();
                foreach (string item in paramList)
                {
                    if (string.IsNullOrEmpty(item))
                    {
                        newparamList.Add("\'\'");
                    }
                    else
                    {
                        newparamList.Add("'" + item + "'");
                    }
                }
                string csum = string.Empty;
                foreach (var csitem in newparamList)
                {
                    csum += csitem + ",";
                }
                return MvcHtmlString.Create("<a id=\"gotoFirstPage\" href=\"#\" style=\"color: red;display:none;\" onclick=\"pageSumit(" + csum + "" + 1 + "," + pageInfo.PageSize + ") \">" + "首页" + "</a>");
            }
        }


        /// <summary>
        /// List页面分页-带跳转
        /// </summary>
        /// <param name="html"></param>
        /// <param name="pageInfo"></param>
        /// <param name="paramList"></param>
        /// <returns></returns>
        public static MvcHtmlString NewPagination_Jump(this HtmlHelper html, PagingInfo pageInfo, params string[] paramList)
        {
            if (pageInfo.PageCount > 0)
            {
                List<string> newparamList = new List<string>();
                foreach (string item in paramList)
                {
                    if (item == null || item == string.Empty)
                    {
                        newparamList.Add("\'\'");
                    }
                    else
                    {
                        newparamList.Add("'" + item + "'");
                    }
                }
                var pagingString = string.Empty;
                //var pagingString = "<ul class=\"pagination\">";
                //新增内容
                pagingString += "<span class=\"num\">当前";
                pagingString += "<input type = \"text\" style=\"color: red;display:none;\" id = \"PageIndexMax\"  value = \"" + pageInfo.PageCount + "\"  />";
                pagingString += "<input type = \"text\" style=\"color: red;display:none;\" id = \"PageIndexNow\"  value = \"" + pageInfo.PageIndex + "\"  />";
                //pagingString += "<span class=\"record\">" + pageInfo.PageIndex + "</span>";
                pagingString += "<input type = \"text\" class=\"small-money4\" style=\"width:35px\" id = \"PageIndexJump\"  value = \"" + pageInfo.PageIndex + "\"  onblur = \"pageChange()\" />";
                pagingString += "<span class=\"num\">页，共" + pageInfo.PageCount + "页，</span>";
                pagingString += "<span class=\"num\">" + pageInfo.TotalItems + "</span>";
                pagingString += "<span class=\"num\">条记录</span>";
                pagingString += " <span class=\"seat\"></span>";
                //end新增内容
                string csum = string.Empty;
                foreach (var csitem in newparamList)
                {
                    csum += csitem + ",";
                }
                pagingString += "<span class=\"record\"> <a id=\"gotoFirstPage\" href=\"#\" onclick=\"pageSumit(" + csum + "" + 1 + "," + pageInfo.PageSize + ") \">" + "首页" + "</a></span>";
                if (pageInfo.PageIndex - 1 > 0)
                {
                    pagingString += "<span class=\"record\"><a href=\"#\" onclick=\"pageSumit2(" + csum + "" + (pageInfo.PageIndex - 1) + "," + pageInfo.PageSize + "," + (pageInfo.PageIndex - 1) + ") \">" + "上一页" + "</a></span>";
                }
                else
                {
                    pagingString += "<span class=\"record\"><a href=\"#\"  href=\"#\">" + "上一页" + "</a></span>";
                }
                for (var i = 1; i <= pageInfo.PageCount; i++)
                {
                    if (i == pageInfo.PageIndex)
                    {
                        pagingString += "<span style=\"cursor:pointer\" class=\"selecthid record\" id=\"s" + i + "\" ><a href=\"#\" style=\"color: red\" >" + i + "</a><a id=\"PageSubmitRefresh\" href=\"#\" style=\"color: red;display:none;\" onclick=\"pageSumit2(" + csum + "" + i + "," + pageInfo.PageSize + "," + i + ") \"></a></span>";
                    }
                    else
                    {
                        pagingString += "<span style=\"cursor:pointer\" onclick=\"pageSumit2(" + csum + "" + i + "," + pageInfo.PageSize + "," + i + ") \" class=\"selecthid record\" id=\"s" + i + "\"  > <a href=\"#\" >" + i + "</a></span>";
                    }
                }

                if (pageInfo.PageIndex - pageInfo.PageCount < 0)
                {
                    pagingString += "<span class=\"record\"><a href=\"#\" onclick=\"pageSumit2(" + csum + "" + (pageInfo.PageIndex + 1) + "," + pageInfo.PageSize + "," + (pageInfo.PageIndex + 1) + ") \">" + "下一页" + "</a></span>";
                }
                else
                {
                    pagingString += "<span class=\"record\"><a href=\"#\"  href=\"#\">" + "下一页" + "</a></span>";
                }
                pagingString += "<span class=\"record\"><a href=\"#\" onclick=\"pageSumit2(" + csum + "" + pageInfo.PageCount + "," + pageInfo.PageSize + "," + pageInfo.PageCount + ") \" >" + "末页" + "</a></span>";

                //新增内容
                //  pagingString += "<li style=\"float:right;height:50px;width:50px;font-size:14px\" > <input id=\"tzpage\" type=\"text\"   class=\"input_1\" style=\"width: 30px;height:30px;text-align:center\" />页</li>";
                //  pagingString += "<li style=\"float:right;height:50px;width:50px; margin-left:10px;font-size:14px\" >  <input type=\"button\" style=\"width: 40px;height:30px;\"  onclick=\"selectPage(" + csum + "" + pageInfo.PageSize + "," + pageInfo.PageCount + ")\" value=\"跳转\"  /></li>";
                //end新增内容

                // pagingString += "</ul>";
                return MvcHtmlString.Create(pagingString);
            }
            else
            {
                List<string> newparamList = new List<string>();
                foreach (string item in paramList)
                {
                    if (string.IsNullOrEmpty(item))
                    {
                        newparamList.Add("\'\'");
                    }
                    else
                    {
                        newparamList.Add("'" + item + "'");
                    }
                }
                string csum = string.Empty;
                foreach (var csitem in newparamList)
                {
                    csum += csitem + ",";
                }
                return MvcHtmlString.Create("<a id=\"gotoFirstPage\" href=\"#\" style=\"color: red;display:none;\" onclick=\"pageSumit(" + csum + "" + 1 + "," + pageInfo.PageSize + ") \">" + "首页" + "</a>");
            }
        }

        /// <summary>
        /// 弹出层分页
        /// </summary>
        /// <param name="html"></param>
        /// <param name="pageInfo"></param>
        /// <param name="paramList"></param>
        /// <returns></returns>
        public static MvcHtmlString NewPopPagination(this HtmlHelper html, PagingInfo pageInfo, params string[] paramList)
        {
            if (pageInfo.PageCount > 0)
            {
                List<string> newparamList = new List<string>();
                foreach (string item in paramList)
                {
                    if (item == null || item == string.Empty)
                    {
                        newparamList.Add("\'\'");
                    }
                    else
                    {
                        newparamList.Add(item);
                    }
                }
                var pagingString = string.Empty;
                // var pagingString = "<ul class=\"pagination\">";
                //新增内容
                pagingString += "<span class=\"num\">当前";
                pagingString += "<span class=\"record\">" + pageInfo.PageIndex + "</span>";
                pagingString += "<span class=\"num\">页，共" + pageInfo.PageCount + "页，</span>";
                pagingString += "<span class=\"num\">" + pageInfo.TotalItems + "</span>";
                pagingString += "<span class=\"num\">条记录</span>";
                pagingString += " <span class=\"seat\"></span>";
                //end新增内容
                string csum = string.Empty;
                foreach (var csitem in newparamList)
                {
                    csum += csitem + ",";
                }
                pagingString += "<span class=\"record\"> <a href=\"#\" onclick=\"popPageSumit(" + csum + "" + 1 + "," + pageInfo.PageSize + ") \">" + "首页" + "</a></span>";
                if (pageInfo.PageIndex - 1 > 0)
                {
                    pagingString += "<span class=\"record\"><a href=\"#\" onclick=\"popPageSumit2(" + csum + "" + (pageInfo.PageIndex - 1) + "," + pageInfo.PageSize + "," + (pageInfo.PageIndex - 1) + ") \">" + "上一页" + "</a></span>";
                }
                else
                {
                    pagingString += "<span class=\"record\"><a href=\"#\"  href=\"#\">" + "上一页" + "</a></span>";
                }
                for (var i = 1; i <= pageInfo.PageCount; i++)
                {
                    if (i == pageInfo.PageIndex)
                    {
                        pagingString += "<span style=\"cursor:pointer\" class=\"selecthid2 record\" id=\"p" + i + "\" ><a href=\"#\" style=\"color: red\" >" + i + "</a></span>";
                    }
                    else
                    {
                        pagingString += "<span style=\"cursor:pointer\" onclick=\"popPageSumit2(" + csum + "" + i + "," + pageInfo.PageSize + "," + i + ") \" class=\"selecthid2 record\" id=\"p" + i + "\"  > <a href=\"#\" >" + i + "</a></span>";
                    }
                }

                if (pageInfo.PageIndex - pageInfo.PageCount < 0)
                {
                    pagingString += "<span class=\"record\"><a href=\"#\" onclick=\"popPageSumit2(" + csum + "" + (pageInfo.PageIndex + 1) + "," + pageInfo.PageSize + "," + (pageInfo.PageIndex + 1) + ") \">" + "下一页" + "</a></span>";
                }
                else
                {
                    pagingString += "<span class=\"record\"><a href=\"#\"  href=\"#\">" + "下一页" + "</a></span>";
                }
                pagingString += "<span class=\"record\"><a href=\"#\" onclick=\"popPageSumit2(" + csum + "" + pageInfo.PageCount + "," + pageInfo.PageSize + "," + pageInfo.PageCount + ") \" >" + "末页" + "</a></span>";

                //新增内容
                //pagingString += "<li style=\"float:right;height:50px;width:50px;font-size:14px\" > <input id=\"tzpage2\" type=\"text\"   class=\"input_1\" style=\"width: 30px;height:30px;text-align:center\" />页</li>";
                // pagingString += "<li style=\"float:right;height:50px;width:50px; margin-left:10px;font-size:14px\" >  <input type=\"button\" style=\"width: 40px;height:30px;\"  onclick=\"selectPopPage(" + csum + "" + pageInfo.PageSize + "," + pageInfo.PageCount + ")\" value=\"跳转\"  /></li>";
                //end新增内容

                //pagingString += "</ul>";
                return MvcHtmlString.Create(pagingString);
            }
            else
            {
                return MvcHtmlString.Create("");
            }
        }

        /// <summary>
        /// 弹出,无页码分页
        /// </summary>
        /// <param name="html"></param>
        /// <param name="pageInfo"></param>
        /// <param name="paramList"></param>
        /// <returns></returns>
        public static MvcHtmlString NewPopPaginationNoIndex(this HtmlHelper html, PagingInfo pageInfo, params string[] paramList)
        {
            if (pageInfo.PageCount > 0)
            {
                List<string> newparamList = new List<string>();
                foreach (string item in paramList)
                {
                    if (item == null || item == string.Empty)
                    {
                        newparamList.Add("\'\'");
                    }
                    else
                    {
                        newparamList.Add(item);
                    }
                }
                var pagingString = string.Empty;
                // var pagingString = "<ul class=\"pagination\">";
                //新增内容
                pagingString += "<span class=\"num\">当前";
                pagingString += "<span class=\"record\">" + pageInfo.PageIndex + "</span>";
                pagingString += "<span class=\"num\">页，共" + pageInfo.PageCount + "页，</span>";
                pagingString += "<span class=\"num\">" + pageInfo.TotalItems + "</span>";
                pagingString += "<span class=\"num\">条记录</span>";
                pagingString += " <span class=\"seat\"></span>";
                //end新增内容
                string csum = string.Empty;
                foreach (var csitem in newparamList)
                {
                    csum += csitem + ",";
                }
                pagingString += "<span class=\"record\"> <a href=\"#\" onclick=\"popPageSumit(" + csum + "" + 1 + "," + pageInfo.PageSize + ") \">" + "首页" + "</a></span>";
                if (pageInfo.PageIndex - 1 > 0)
                {
                    pagingString += "<span class=\"record\"><a href=\"#\" onclick=\"popPageSumit2(" + csum + "" + (pageInfo.PageIndex - 1) + "," + pageInfo.PageSize + "," + (pageInfo.PageIndex - 1) + ") \">" + "上一页" + "</a></span>";
                }
                else
                {
                    pagingString += "<span class=\"record\"><a href=\"#\"  href=\"#\">" + "上一页" + "</a></span>";
                }
                if (pageInfo.PageIndex - pageInfo.PageCount < 0)
                {
                    pagingString += "<span class=\"record\"><a href=\"#\" onclick=\"popPageSumit2(" + csum + "" + (pageInfo.PageIndex + 1) + "," + pageInfo.PageSize + "," + (pageInfo.PageIndex + 1) + ") \">" + "下一页" + "</a></span>";
                }
                else
                {
                    pagingString += "<span class=\"record\"><a href=\"#\"  href=\"#\">" + "下一页" + "</a></span>";
                }
                pagingString += "<span class=\"record\"><a href=\"#\" onclick=\"popPageSumit2(" + csum + "" + pageInfo.PageCount + "," + pageInfo.PageSize + "," + pageInfo.PageCount + ") \" >" + "末页" + "</a></span>";
                return MvcHtmlString.Create(pagingString);
            }
            else
            {
                return MvcHtmlString.Create("");
            }
        }
    }
}