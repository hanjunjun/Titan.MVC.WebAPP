using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Titan.Infrastructure.HTMLBase
{
    public static class HtmlHelper
    {
        public static string GetPostHttpRequest(string strMethod, string strUrl, Encoding encoding, string args = null)
        {
            var mHtml = string.Empty;
            try
            {
                var uurl = Uri.UnescapeDataString(strUrl);
            }
            catch
            {
                return mHtml;
            }
            var re = new Regex("(?<h>[^\x00-\xff]+)");
            var mc = re.Match(strUrl);
            if (mc.Success)
            {
                var han = mc.Groups["h"].Value;
                strUrl = strUrl.Replace(han, HttpUtility.UrlEncode(han, encoding));
            }
            try
            {
                var _mUri = new Uri(strUrl);
                if (strUrl.EndsWith(".rar") || strUrl.EndsWith(".dat") || strUrl.EndsWith(".msi"))
                {
                    return mHtml;
                }
                var request = (HttpWebRequest)WebRequest.Create(_mUri);
                request.AllowAutoRedirect = true;
                request.MaximumAutomaticRedirections = 50;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; .NET4.0C; .NET4.0E; .NET CLR 2.0.50727; .NET CLR 3.0.30729; .NET CLR 3.5.30729; InfoPath.3; BRI/2; rv:11.0)";
                request.KeepAlive = true;
                request.Timeout = 30000;
                if (!string.IsNullOrEmpty(args))
                {
                    request.Method = "POST";
                    request.ContentType = "application/x-www-form-urlencoded";
                    byte[] byteArray = encoding.GetBytes(args); //转化
                    Stream newStream = request.GetRequestStream();
                    newStream.Write(byteArray, 0, byteArray.Length);//写入参数
                    newStream.Close();
                }
                var rsps = (HttpWebResponse)request.GetResponse();
                var cookie = new Cookie()
                {
                    Name = "N",
                    Domain = _mUri.Authority
                };
                rsps.Cookies.Add(cookie);
                var cc = new CookieContainer();
                request.CookieContainer = cc;
                var sm = rsps.GetResponseStream();
                if (!request.HaveResponse)
                {
                    rsps.Close();
                    return null;
                }
                if (!rsps.ContentType.ToLower().StartsWith("text/") || rsps.ContentLength > 1 << 22)
                {
                    rsps.Close();
                    return mHtml;
                }
                if (sm != null) mHtml = new StreamReader(sm, encoding).ReadToEnd();
                rsps.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return mHtml;
        }

        public static string HttpGet(string url)
        {
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "application/json";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }

        
        private static string servieUrl = (ConfigurationManager.AppSettings["CostInterfaceUrl"]);

        public static string RefreshElderCost_DateRange(int strOptType, string strOtherCostID, string strCheckDataID, string strStartDate, string strEndDate)
        {
            return HttpGet(servieUrl + $@"ElderCost/RefreshElderCost_DateRange?strOptType={strOptType}&strOtherCostID={strOtherCostID}&strCheckDataID={strCheckDataID}&strStartDate={strStartDate}&strEndDate={strEndDate}");
        }

        public static string RefreshElderCost_BackLive(string strCheckDataID, string strBackLiveDate)
        {
            return HttpGet(servieUrl + $@"ElderCost/RefreshElderCost_BackLive?strBackLiveDate={strBackLiveDate}&strCheckDataID={strCheckDataID}");
        }

        public static string StopOtherMonthCost(string strCheckDataID, string strSDate, string strOtherMonthCostID)
        {
            var url = servieUrl + $"ElderCost/StopOtherMonthCost?strCheckDataID={strCheckDataID}&strSDate={strSDate}&strOtherMonthCostID={strOtherMonthCostID}";
            return HttpGet(url);
        }

        public static string PostHttpRequest(string postUrl, string paramData, Encoding dataEncode, CookieCollection cookies = null)
        {
            string strJson;
            try
            {
                byte[] byteArray = dataEncode.GetBytes(paramData); //转化
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(postUrl));
                webReq.Method = "POST";
                webReq.ContentType = "application/x-www-form-urlencoded";
                //webReq.ContentType = "multipart/form-data";
                if (cookies != null)
                {
                    webReq.CookieContainer = new CookieContainer();
                    webReq.CookieContainer.Add(cookies);
                }
                webReq.UserAgent = "CURL";
                webReq.ContentLength = byteArray.Length;
                Stream newStream = webReq.GetRequestStream();
                newStream.Write(byteArray, 0, byteArray.Length);//写入参数
                HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
                var result = response.GetResponseStream();
                if (result == null) return null;
                StreamReader reader = new StreamReader(result, Encoding.UTF8);
                strJson = reader.ReadToEnd();
                reader.Close();
                response.Close();
                newStream.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return strJson;
        }

    }
}