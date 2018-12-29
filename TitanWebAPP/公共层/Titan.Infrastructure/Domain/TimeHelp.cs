using System;

namespace Titan.Infrastructure.Domain
{
    public class TimeHelp
    {
        #region 时间戳转换

        /// <summary>
        /// 时间戳转为C#格式时间
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static DateTime GetTime(string timeStamp)
        {
            try
            {
                DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                long lTime = long.Parse(timeStamp);
                lTime = lTime * 10000;
                TimeSpan toNow = new TimeSpan(lTime);
                DateTime dateTime = dtStart.Add(toNow);
                return dateTime.AddHours(-8);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static int ConvertDateTimeInt(DateTime time)
        {
            try
            {
                DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                return (int)(time - startTime).TotalSeconds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        /// <summary>
        /// 计算某日起始日期（礼拜一的日期）
        /// </summary>
        /// <param name="someDate">该周中任意一天</param>
        /// <returns>返回礼拜一日期，后面的具体时、分、秒和传入值相等</returns>
        public static DateTime GetMondayDate(DateTime someDate)
        {
            int i = someDate.DayOfWeek - DayOfWeek.Monday;
            if (i == -1) i = 6;// i值 > = 0 ，因为枚举原因，Sunday排在最前，此时Sunday-Monday=-1，必须+7=6。
            TimeSpan ts = new TimeSpan(i, 0, 0, 0);
            return someDate.Subtract(ts);
        }

        /// <summary>
        /// 计算某日结束日期（礼拜日的日期）
        /// </summary>
        /// <param name="someDate">该周中任意一天</param>
        /// <returns>返回礼拜日日期，后面的具体时、分、秒和传入值相等</returns>
        public static DateTime GetSundayDate(DateTime someDate)
        {
            int i = someDate.DayOfWeek - DayOfWeek.Sunday;
            if (i != 0) i = 7 - i;// 因为枚举原因，Sunday排在最前，相减间隔要被7减。
            TimeSpan ts = new TimeSpan(i, 0, 0, 0);
            return someDate.Add(ts);
        }
    }
}
