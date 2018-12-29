using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Security;
using System.Collections.Generic;

namespace Titan.AppService.BeeCloud
{
    public class BCPrivateUtil
    {
        private delegate void getBestHostDelegate();
        
        public static List<string> mLocalDefaultHosts = new List<string>(){
            "https://api.beecloud.cn"
            };

        /// <summary>
        /// 获取API地址
        /// </summary>
        public static string getHost()
        {
            Random random = new Random();
            return mLocalDefaultHosts[random.Next(0, mLocalDefaultHosts.Count)];
        }

        /// <summary>
        /// 通过appSerect生成AppSign
        /// </summary>
        public static string getAppSignature(string appId, string appSecret, string timestamp)
        {
            if (appSecret == null || appId == null)
            {
                throw new BCException("app id或app Secret为空，请调用registerApp方法");
            }
            string input = appId + timestamp + appSecret;
            string sign = FormsAuthentication.HashPasswordForStoringInConfigFile(input, "MD5").ToLower();
            return sign;
        }

        /// <summary>
        /// 通过masterSecret生成AppSign
        /// </summary>
        public static string getAppSignatureByMasterSecret(string appId, string masterSecret, string timestamp)
        {
            if (masterSecret == null || appId == null)
            {
                throw new BCException("app id 或 master Secret为空，请调用registerApp方法");
            }
            string input = appId + timestamp + masterSecret;
            string sign = FormsAuthentication.HashPasswordForStoringInConfigFile(input, "MD5").ToLower();
            return sign;
        }

        /// <summary>
        /// 通过testSecret生成AppSign
        /// </summary>
        /// <returns></returns>
        public static string getAppSignatureByTestSecret(string timestamp)
        {
            string testSecret = BCCache.Instance.testSecret;
            string appId = BCCache.Instance.appId;
            if (testSecret == null || appId == null)
            {
                throw new BCException("app id 或 test Secret为空，请调用registerApp方法");
            }
            string input = appId + timestamp + testSecret;
            string sign = FormsAuthentication.HashPasswordForStoringInConfigFile(input, "MD5").ToLower();
            return sign;
        }

        /// <summary>
        /// 创建GET方式的HTTP请求 
        /// </summary>
        public static HttpWebResponse CreateGetHttpResponse(string url, int timeout)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            request.Timeout = timeout;

            return request.GetResponse() as HttpWebResponse;
        }

        /// <summary>
        /// 创建PUT方式的HTTP请求 
        /// </summary>
        public static HttpWebResponse CreatePutHttpResponse(String url, String payload, int timeout)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "PUT";
            request.Timeout = timeout;

            // Encode the data
            byte[] encodedBytes = Encoding.UTF8.GetBytes(payload);
            request.ContentLength = encodedBytes.Length;
            request.ContentType = "application/json";

            // Write encoded data into request stream
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(encodedBytes, 0, encodedBytes.Length);
            requestStream.Close();

            return request.GetResponse() as HttpWebResponse;
        }

        /// <summary>
        /// 创建POST方式的HTTP请求 
        /// </summary>
        public static HttpWebResponse CreatePostHttpResponse(String url, String payload, int timeout)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "POST";
            request.Timeout = timeout;

            // Encode the data
            byte[] encodedBytes = Encoding.UTF8.GetBytes(payload);
            request.ContentLength = encodedBytes.Length;
            request.ContentType = "application/json";

            // Write encoded data into request stream
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(encodedBytes, 0, encodedBytes.Length);
            requestStream.Close();

            return request.GetResponse() as HttpWebResponse;
        }

        /// <summary>
        /// 创建DELETE方式的HTTP请求 
        /// </summary>
        /// <returns></returns>
        public static HttpWebResponse CreateDeleteHttpResponse(String url, int timeout)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "DELETE";
            request.Timeout = timeout;

            return request.GetResponse() as HttpWebResponse;
        }

        /// <summary>
        /// 获取请求的数据
        /// </summary>
        public static string GetResponseString(HttpWebResponse webresponse)
        {
            using (Stream s = webresponse.GetResponseStream())
            {
                StreamReader reader = new StreamReader(s, Encoding.UTF8);
                
                return reader.ReadToEnd();

            }
            
        }
    }
}
