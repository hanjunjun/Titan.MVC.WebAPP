/************************************************************************
 * 文件名：FileHelper
 * 文件功能描述：附件帮助类
 * 作    者：hjj
 * 创建日期：2017-06-15
 * 修 改 人：
 * 修改日期：
 * 修改原因：
 * Copyright (c) 2016 Titan.Han . All Rights Reserved. 
 * ***********************************************************************/
using System.Collections.Generic;
using System.IO;

namespace Titan.Infrastructure.Domain
{
    public class FileHelper
    {
        /// <summary>
        /// 获取附件集合
        /// </summary>
        /// <param name="strPath"></param>
        /// <returns></returns>
        public static List<string> GetFiles(string strPath)
        {
            List<string> result = new List<string>();
            DirectoryInfo directory = new DirectoryInfo(strPath);
            if (directory.Exists)
            {
                //foreach (FileInfo info in directory.GetFiles(pattern))
                //{
                //    result = result + info.FullName.ToString() + ";";
                //    //result = result + info.Name.ToString() + ";";
                //}

                foreach (DirectoryInfo info in directory.GetDirectories())
                {
                    if (info.Name != "Shared")
                        result.Add(info.Name);
                }
            }
            return result;

        }
        /// <summary>
        /// 创建附件
        /// </summary>
        /// <param name="fileLines"></param>

        public static void CreateFiles(List<string> fileLines)
        {

            // String[] fileLines = System.IO.File.ReadAllLines(@"D:\机构版本3.0\WebMVC\ViewListName.txt");

            List<string> result = new List<string>() { "Menber_Base", "Dept_001_BaseInfo" };
            foreach (string item in fileLines)
            {
                string pathStr = @"~/View/" + item;
                // string paths = System.Web.HttpContext.Current.Server.MapPath(pathStr);
                string paths = @"D:\机构版本3.0\WebMVC\WebMVC\Views\" + item;
                if (!Directory.Exists(paths))//如果不存在就创建文件夹
                {
                    Directory.CreateDirectory(paths);
                }
            }
        }

        /// <summary>
        /// 文件是否存在
        /// </summary>
        /// <param name="fileurl"></param>
        /// <returns></returns>
        public static bool ExistFile(string fileurl)
        {
            string xmlpath = System.Web.HttpContext.Current.Server.MapPath("~");
            string strXmlFile = xmlpath + fileurl;
            if (System.IO.File.Exists(strXmlFile))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}