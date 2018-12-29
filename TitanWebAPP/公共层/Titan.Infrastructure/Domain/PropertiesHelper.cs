/************************************************************************
 * 文件名：PropertiesHelper
 * 文件功能描述：对象属性帮助类
 * 作    者：hjj
 * 创建日期：2017-06-15
 * 修 改 人：
 * 修改日期：
 * 修改原因：
 * Copyright (c) 2016 Titan.Han . All Rights Reserved. 
 * ***********************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Titan.Infrastructure.Domain
{
    public class PropertiesHelper
    {
        /// <summary>
        /// C#反射遍历对象属性
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="model">对象</param>
        public static PropertyModelGe ForeachClassProperties(string strModelName)
        {
            var asm = Assembly.LoadFile(@"D:\机构版本3.0\WebMVC\Controllers\bin\Debug\Controllers.dll");
            Type t = asm.GetType(strModelName, true);
            //PropertyInfo[] PropertyList = t.GetProperties();     
            PropertyModelGe pg = new PropertyModelGe();

            var asmTow = Assembly.LoadFile(@"D:\机构版本3.0\WebMVC\AppService\bin\Debug\AppService.dll");
            string mName = strModelName.Substring(strModelName.LastIndexOf('.') + 1) + "s";
            var p = t.GetProperty(mName);

            pg.PropertyName = p.Name;
            pg.TableName = mName.Substring(0, mName.IndexOf("View"));

            foreach (PropertyInfo items in p.PropertyType.GetProperties())
            {
                Type tt = asmTow.GetType(items.PropertyType.FullName);
                if (tt != null)
                {
                    PropertyInfo[] PropertyListL = tt.GetProperties();
                    List<PropertyMChildrenodelGe> cList = new List<PropertyMChildrenodelGe>();
                    foreach (PropertyInfo itemL in PropertyListL)
                    {
                        PropertyMChildrenodelGe ch = new PropertyMChildrenodelGe();
                        bool isChildren;
                        ch.ChildrenENName = itemL.Name;
                        GetAttribute(pg, itemL, ch, asmTow, out isChildren);
                        if (ch.ChildrenCHName == string.Empty || ch.ChildrenCHName == null)
                        {
                            ch.ChildrenCHName = ch.ChildrenENName;
                        }
                        cList.Add(ch);

                    }
                    pg.ChildrenodelGList = cList;
                }
            }

            return pg;
        }

        /// <summary>
        /// 得到特性值
        /// </summary>
        /// <param name="pg"></param>
        /// <param name="itemL"></param>
        /// <param name="ch"></param>
        private static void GetAttribute(PropertyModelGe pg, PropertyInfo itemL, PropertyMChildrenodelGe ch, Assembly asmTow, out bool isChildren)
        {
            object[] objAttrs = itemL.GetCustomAttributes(typeof(DescriptionAttribute), true);
            isChildren = false;
            if (objAttrs.Length > 0)
            {
                DescriptionAttribute attr = objAttrs[0] as DescriptionAttribute;
                if (attr != null)
                {
                    if (attr.Description == "主键")
                    {
                        pg.PKName = itemL.Name;
                    }
                    else
                    {
                        if (attr.Description.Contains("-子-"))
                        {
                            isChildren = GetChildrens(asmTow, itemL, ch);
                        }
                    }
                    ch.ChildrenCHName = attr.Description;
                }
            }

            object[] objAttrsT = itemL.GetCustomAttributes(typeof(ControllersTypeAttribute), true);
            if (objAttrsT.Length > 0)
            {
                ControllersTypeAttribute attr = objAttrsT[0] as ControllersTypeAttribute;
                if (attr != null)
                {
                    ch.ControllerTypeName = attr.ControllerTypeName;
                    ch.AtaterValue = attr.AtaterValue;
                }
            }

        }

        /// <summary>
        /// 生成查询对象
        /// </summary>
        /// <param name="strModelName"></param>
        /// <returns></returns>
        public static PropertyModelGe LookProperties(string strModelName)
        {
            string callingDomainName = AppDomain.CurrentDomain.FriendlyName;//Thread.GetDomain
            var asm = Assembly.LoadFile(@"D:\机构版本3.0\WebMVC\Controllers\bin\Debug\Controllers.dll");
            Type t = asm.GetType(strModelName, true);
            PropertyModelGe pg = new PropertyModelGe();
            var asmTow = Assembly.LoadFile(@"D:\机构版本3.0\WebMVC\AppService\bin\Debug\AppService.dll");
            string mName = strModelName.Substring(strModelName.LastIndexOf('.') + 1) + "s";
            var p = t.GetProperty(mName);

            pg.PropertyName = p.Name;
            pg.TableName = mName.Substring(0, mName.IndexOf("View"));

            Type tt = asmTow.GetType(p.PropertyType.FullName);
            if (tt != null)
            {
                PropertyInfo[] PropertyListL = tt.GetProperties();

                List<PropertyMChildrenodelGe> cList = new List<PropertyMChildrenodelGe>();
                foreach (PropertyInfo itemL in PropertyListL)
                {
                    PropertyMChildrenodelGe ch = new PropertyMChildrenodelGe();
                    ch.ChildrenENName = itemL.Name;
                    bool isChildren;
                    GetAttribute(pg, itemL, ch, asmTow, out isChildren);
                    ch.IsChildrenList = isChildren;
                    if (ch.ChildrenCHName == string.Empty || ch.ChildrenCHName == null)
                    {
                        ch.ChildrenCHName = ch.ChildrenENName;
                    }
                    cList.Add(ch);
                }
                pg.ChildrenodelGList = cList;
            }
            asm = null; asmTow = null;
            return pg;
        }

        /// <summary>
        /// 获取对象的子对象（1：n）
        /// </summary>
        /// <param name="asmTow"></param>
        /// <param name="itemL"></param>
        /// <param name="ch"></param>
        /// <returns></returns>

        private static bool GetChildrens(Assembly asmTow, PropertyInfo itemL, PropertyMChildrenodelGe ch)
        {
            bool isChildren = true;
            //获取子集信息
            List<PropertyMChildrensList> pList = new List<PropertyMChildrensList>();
            foreach (PropertyInfo items in itemL.PropertyType.GetProperties())
            {
                Type ttc = asmTow.GetType(items.PropertyType.FullName);
                if (ttc != null)
                {
                    PropertyInfo[] PropertyListLc = ttc.GetProperties();

                    foreach (PropertyInfo itemLcc in PropertyListLc)
                    {
                        PropertyMChildrensList cLists = new PropertyMChildrensList();
                        cLists.PChildrenENName = itemLcc.Name;
                        object[] objAttrsc = itemLcc.GetCustomAttributes(typeof(DescriptionAttribute), true);
                        if (objAttrsc.Length > 0)
                        {
                            DescriptionAttribute attrc = objAttrsc[0] as DescriptionAttribute;
                            if (attrc != null)
                            {
                                cLists.PChildrenCHName = attrc.Description;
                            }
                        }
                        else
                        {
                            cLists.PChildrenCHName = itemLcc.Name;
                        }
                        pList.Add(cLists);

                    }

                }
            }
            ch.PChildrenList = pList;
            return isChildren;
        }

        /// <summary>
        /// 获取对象属性的注释值
        /// </summary>
        /// <param name="strModelName"></param>
        /// <returns></returns>
        public static string GetDescriptionProperties(string strModelName, string strProName)
        {
            string str = string.Empty;
            var asm = Assembly.LoadFile(@"D:\机构版本3.0\WebMVC\Model\bin\Debug\Model.dll");
            Type t = asm.GetType(strModelName, true);
            var p = t.GetProperty(strProName);


            object[] objAttrs = p.GetCustomAttributes(typeof(DescriptionAttribute), true);
            if (objAttrs.Length > 0)
            {
                DescriptionAttribute attr = objAttrs[0] as DescriptionAttribute;
                if (attr != null)
                {
                    str = attr.Description;
                }
            }
            if (str == string.Empty)
            {
                str = p.Name;
            }
            return str;
        }

        /// <summary>
        /// 新增或修改属性
        /// </summary>
        /// <param name="strModelName"></param>
        /// <returns></returns>
        public static PropertyModelGe AddOrUpdateProperties(string strModelName)
        {
            var asm = Assembly.LoadFile(@"D:\机构版本3.0\WebMVC\Controllers\bin\Debug\Controllers.dll");
            Type t = asm.GetType(strModelName, true);
            PropertyModelGe pg = new PropertyModelGe();
            var asmTow = Assembly.LoadFile(@"D:\机构版本3.0\WebMVC\AppService\bin\Debug\AppService.dll");
            string mName = strModelName.Substring(strModelName.LastIndexOf('.') + 1) + "s";
            var p = t.GetProperty(mName);

            pg.PropertyName = p.Name;
            pg.TableName = mName.Substring(0, mName.IndexOf("View"));

            Type tt = asmTow.GetType(p.PropertyType.FullName);
            if (tt != null)
            {
                PropertyInfo[] PropertyListL = tt.GetProperties();

                List<PropertyMChildrenodelGe> cList = new List<PropertyMChildrenodelGe>();
                foreach (PropertyInfo itemL in PropertyListL)
                {
                    PropertyMChildrenodelGe ch = new PropertyMChildrenodelGe();
                    ch.ChildrenENName = itemL.Name;
                    bool isChildren;
                    GetAttribute(pg, itemL, ch, asmTow, out isChildren);
                    ch.IsChildrenList = isChildren;
                    if (ch.ChildrenCHName == string.Empty || ch.ChildrenCHName == null)
                    {
                        ch.ChildrenCHName = ch.ChildrenENName;
                    }
                    cList.Add(ch);
                }
                pg.ChildrenodelGList = cList;
            }
            asm = null; asmTow = null;
            return pg;
        }

    }

    /// <summary>
    /// 页面属性Model
    /// </summary>
    public class PropertyModelGe
    {
        /// <summary>
        /// 页面对象名
        /// </summary>
        public string PropertyName { get; set; }
        /// <summary>
        /// 主键名
        /// </summary>
        public string PKName { get; set; }
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }
        public List<PropertyMChildrenodelGe> ChildrenodelGList { get; set; }
    }

    /// <summary>
    /// Model里的属性
    /// </summary>
    public class PropertyMChildrenodelGe
    {
        /// <summary>
        /// 英文名称
        /// </summary>
        public string ChildrenENName { get; set; }
        /// <summary>
        /// 中文名称
        /// </summary>
        public string ChildrenCHName { get; set; }
        /// <summary>
        /// 是否有子集合
        /// </summary>
        public bool IsChildrenList { get; set; }

        /// <summary>
        /// 属性类型
        /// </summary>
        public string ControllerTypeName { get; set; }

        /// <summary>
        /// 属性初始值
        /// </summary>
        public string AtaterValue { get; set; }

        public List<PropertyMChildrensList> PChildrenList { get; set; }
    }

    /// <summary>
    /// 属性中的集合
    /// </summary>
    public class PropertyMChildrensList
    {
        public string PChildrenENName { get; set; }

        public string PChildrenCHName { get; set; }
    }
}