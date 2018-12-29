/************************************************************************
 * 文件名：CacheHelper
 * 文件功能描述：缓存帮助类
 * 作    者：hjj
 * 创建日期：2017-06-15
 * 修 改 人：
 * 修改日期：
 * 修改原因：
 * Copyright (c) 2016 Titan.Han . All Rights Reserved. 
 * ***********************************************************************/
using System;
using System.Web;
using System.Web.Caching;

namespace Titan.Infrastructure.Domain
{
    public static class CacheHelper
    {
        static int cacheTime = 1;
        /// <summary>
        /// 读取缓存项
        /// </summary>
        /// <returns></returns>
        public static object CacheReader(string cacheKey)
        {
            return HttpRuntime.Cache[cacheKey];
        }


        /// <summary>
        /// 写入缓存项
        /// </summary>
        public static void CacheWriter(string cacheKey, object cacheValue, int cache_time = 0)
        {
            HttpRuntime.Cache.Insert(cacheKey, cacheValue, null,
                DateTime.Now.AddMinutes(cache_time <= 0 ? cacheTime : cache_time),
                Cache.NoSlidingExpiration);
        }

        /// <summary>
        /// 写入缓存项-平滑过期
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="cacheValue"></param>
        /// <param name="cache_time">超时时间（分）</param>
        public static void CacheWriterSliding(string cacheKey, object cacheValue, int cache_time = 0)
        {
            HttpRuntime.Cache.Insert(
                cacheKey,
                cacheValue,
                null,
                Cache.NoAbsoluteExpiration,
                TimeSpan.FromMinutes(cache_time <= 0 ? cacheTime : cache_time));
        }

        /// <summary>
        /// 移除指定缓存项
        /// </summary>
        public static void CacheRemove(string cacheName)
        {
            HttpRuntime.Cache.Remove(cacheName);
        }
    }
}