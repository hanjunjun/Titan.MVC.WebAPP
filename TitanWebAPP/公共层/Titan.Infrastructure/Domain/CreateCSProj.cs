/************************************************************************
 * 文件名：CreateCSProj
 * 文件功能描述：代码生成器生成处理类
 * 作    者：hjj
 * 创建日期：2017-06-15
 * 修 改 人：
 * 修改日期：
 * 修改原因：
 * Copyright (c) 2016 Titan.Han . All Rights Reserved. 
 * ***********************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace Titan.Infrastructure.Domain
{
    public static class CreateCSProj
    {
        public static void GenCsproj(string strCsprojPath, string strFilePath, string strFileName, string strFiletype)
        {
            //ProtocolModel目前的cs文件列表  
            var files = Directory.GetFiles(strFilePath, strFiletype);//"*.cs"
            List<String> currFiles = new List<String>();

            foreach (var file in files)
            {
                String path = file.ToString();
                StringBuilder sb = new StringBuilder();
                sb.Append(strFileName).Append(path.Substring(path.LastIndexOf(@"\")));
                currFiles.Add(sb.ToString());
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(strCsprojPath);
            //Project节点  
            XmlNodeList xnl = doc.ChildNodes[0].ChildNodes;
            if (doc.ChildNodes[0].Name.ToLower() != "project")
            {
                xnl = doc.ChildNodes[1].ChildNodes;
            }
            foreach (XmlNode xn in xnl)
            {
                //找到包含compile的节点  
                if (xn.ChildNodes.Count > 0 && xn.ChildNodes[0].Name.ToLower() == "compile")
                {
                    foreach (XmlNode cxn in xn.ChildNodes)
                    {
                        if (cxn.Name.ToLower() == "compile")
                        {
                            XmlElement xe = (XmlElement)cxn;
                            String includeFile = xe.GetAttribute("Include");
                            //项目中已包含的ProtocolModel  
                            if (includeFile.StartsWith(strFileName + "\\"))
                            {
                                Console.WriteLine(includeFile);
                                foreach (String item in currFiles)
                                {
                                    //将已经包含在项目中的cs文件在所有文件的列表中剔除  
                                    //操作完之后currFiles中剩下的就是接下来需要包含到项目中的文件  
                                    if (item.Equals(includeFile))
                                    {
                                        currFiles.Remove(item);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    //将剩下的文件加入csproj中  
                    foreach (String item in currFiles)
                    {
                        XmlElement xelKey = doc.CreateElement("Compile", doc.DocumentElement.NamespaceURI);
                        XmlAttribute xelType = doc.CreateAttribute("Include");
                        xelType.InnerText = item;
                        xelKey.SetAttributeNode(xelType);
                        xn.AppendChild(xelKey);
                    }
                }
            }
            doc.Save(strCsprojPath);
        }

        public static void GenCsprojForView(string strCsprojPath, string strFilePath, string strFileName, string strFiletype)
        {
            //ProtocolModel目前的cs文件列表  
            var files = Directory.GetFiles(strFilePath, strFiletype);//"*.cs"
            List<String> currFiles = new List<String>();

            foreach (var file in files)
            {
                String path = file.ToString();
                StringBuilder sb = new StringBuilder();
                sb.Append(strFileName).Append(path.Substring(path.LastIndexOf(@"\")));
                currFiles.Add(sb.ToString());
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(strCsprojPath);
            //Project节点  
            XmlNodeList xnl = doc.ChildNodes[0].ChildNodes;
            if (doc.ChildNodes[0].Name.ToLower() != "project")
            {
                xnl = doc.ChildNodes[1].ChildNodes;
            }
            foreach (XmlNode xn in xnl)
            {
                //找到包含compile的节点  
                if (xn.ChildNodes.Count > 0 && xn.ChildNodes[0].Name.ToLower() == "content")
                {
                    foreach (XmlNode cxn in xn.ChildNodes)
                    {
                        if (cxn.Name.ToLower() == "content")
                        {
                            XmlElement xe = (XmlElement)cxn;
                            String includeFile = xe.GetAttribute("Include");
                            //项目中已包含的ProtocolModel  
                            if (includeFile.StartsWith(strFileName + "\\"))
                            {
                                Console.WriteLine(includeFile);
                                foreach (String item in currFiles)
                                {
                                    //将已经包含在项目中的cs文件在所有文件的列表中剔除  
                                    //操作完之后currFiles中剩下的就是接下来需要包含到项目中的文件  
                                    if (item.Equals(includeFile))
                                    {
                                        currFiles.Remove(item);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    //将剩下的文件加入csproj中  
                    foreach (String item in currFiles)
                    {
                        XmlElement xelKey = doc.CreateElement("Content", doc.DocumentElement.NamespaceURI);
                        XmlAttribute xelType = doc.CreateAttribute("Include");
                        xelType.InnerText = item;
                        xelKey.SetAttributeNode(xelType);
                        xn.AppendChild(xelKey);

                    }
                    break;
                }
            }
            doc.Save(strCsprojPath);
        }

        public static void GenFolder(string strCsprojPath, List<string> fileLines)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(strCsprojPath);
            //Project节点  
            XmlNodeList xnl = doc.ChildNodes[0].ChildNodes;
            if (doc.ChildNodes[0].Name.ToLower() != "project")
            {
                xnl = doc.ChildNodes[1].ChildNodes;
            }
            foreach (XmlNode xn in xnl)
            {
                //找到包含compile的节点  
                if (xn.ChildNodes.Count > 0 && xn.ChildNodes[0].Name.ToLower() == "folder")
                {
                    foreach (string item in fileLines)
                    {
                        XmlElement xelKey = doc.CreateElement("Folder", doc.DocumentElement.NamespaceURI);
                        XmlAttribute xelType = doc.CreateAttribute("Include");
                        xelType.InnerText = @"Views\" + item + "\\";
                        xelKey.SetAttributeNode(xelType);

                        bool isHas = false;

                        foreach (XmlNode cxn in xn.ChildNodes)
                        {
                            if (cxn.Name.ToLower() == "folder")
                            {
                                XmlElement xe = (XmlElement)cxn;
                                String includeFile = xe.GetAttribute("Include");

                                if (includeFile.StartsWith(@"Views\" + item + "\\"))
                                {
                                    isHas = true;
                                }
                            }
                        }
                        if (!isHas)
                            xn.AppendChild(xelKey);

                    }

                }
            }
            doc.Save(strCsprojPath);
        }
    }
}