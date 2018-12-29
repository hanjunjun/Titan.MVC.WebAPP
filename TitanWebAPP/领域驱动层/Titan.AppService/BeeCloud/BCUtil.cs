using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Titan.AppService.BeeCloud
{
    public class BCUtil
    {
        public static long GetTimeStamp(DateTime date)
        {
            long unixTimestamp = date.ToUniversalTime().Ticks - new DateTime(1970, 1, 1, 0,0,0, DateTimeKind.Utc).Ticks;
            unixTimestamp /= TimeSpan.TicksPerMillisecond;
            return unixTimestamp;
        }

        public static DateTime GetDateTime(long timestamp)
        {
            DateTime time = DateTime.MinValue;
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            time = startTime.AddMilliseconds(timestamp);
            return time;
        }

        public static string GetUUID()
        {
            Guid g = Guid.NewGuid();
            string uuid = g.ToString().Replace("-", "");
            return uuid;
        }

        /// <summary>
        /// webhook验签
        /// </summary>
        /// <param name="transactionID">交易单号，对应支付请求的bill_no或者退款请求的refund_no,对于秒支付button为传入的out_trade_no</param>
        /// <param name="transactionType">交易类型</param>
        /// <param name="channelType">渠道</param>
        /// <param name="transactionFee">交易金额</param>
        /// <returns></returns>
        public static string GetSign(string transactionID, string transactionType, string channelType, int transactionFee)
        {
            if (BCCache.Instance.appId == null || BCCache.Instance.masterSecret == null)
            {
                throw new BCException("app id或master Secret为空，请调用registerApp方法");
            }
            string input = BCCache.Instance.appId + transactionID + transactionType + channelType + transactionFee + BCCache.Instance.masterSecret;
            string sign = FormsAuthentication.HashPasswordForStoringInConfigFile(input, "MD5").ToLower();
            return sign;
        }

        /// <summary>
        /// webhook 测试模式验签
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static string GetSignByTestMode(string timestamp)
        {
            string testSecret = BCCache.Instance.testSecret;
            string appId = BCCache.Instance.appId;
            if (testSecret == null || appId == null)
            {
                throw new BCException("app id 或 test Secret为空，请调用registerApp方法");
            }
            string input = appId + testSecret + timestamp;
            string sign = FormsAuthentication.HashPasswordForStoringInConfigFile(input, "MD5").ToLower();
            return sign;
        }
    }
}