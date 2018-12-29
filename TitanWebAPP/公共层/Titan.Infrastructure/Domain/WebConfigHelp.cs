/************************************************************************
 * 文件名：WebConfigHelp
 * 文件功能描述：Web.Config配置类
 * 作    者：hjj
 * 创建日期：2017-06-15
 * 修 改 人：
 * 修改日期：
 * 修改原因：
 * Copyright (c) 2016 Titan.Han . All Rights Reserved. 
 * ***********************************************************************/
using System.Configuration;

namespace Titan.Infrastructure.Domain
{
    /// <summary>
    /// Web.Config配置类
    /// </summary>
    public class WebConfigHelp
    {
        // public static readonly string PageDoMain = WebConfigHelp.GetApp("DoMain");

        #region GetApp
        /// <summary>
        /// 取appSettings结点数据
        /// </summary>
        /// <param name="strKey">key</param>
        /// <returns>返回值</returns>
        public static string GetApp(string strKey)
        {
            if (ConfigurationManager.AppSettings[strKey] != null)
                return ConfigurationManager.AppSettings[strKey].ToString();
            return null;
        }
        #endregion

        #region GetConn
        /// <summary>
        /// 取connectionStrings结点数据
        /// </summary>
        /// <param name="strKey">name</param>
        /// <returns>返回值</returns>
        public static string GetConn(string strKey)
        {
            if (ConfigurationManager.ConnectionStrings[strKey] != null)
                return ConfigurationManager.ConnectionStrings[strKey].ToString();
            return null;
        }
        #endregion
    }
}