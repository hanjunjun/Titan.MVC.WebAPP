using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

namespace Titan.Infrastructure.Reflection
{
    public class ObjectReflection
    {
        /// <summary>
        /// 创建对象
        /// </summary>
        /// <param name="typeInfo">类型名称</param>
        /// <returns></returns>
        public static object CreateObject(string typeInfo)
        {
            object obj = null;
            if (!string.IsNullOrEmpty(typeInfo))
            {
                string assemblyInfo = typeInfo;
                if (!string.IsNullOrEmpty(assemblyInfo.Trim()))
                {
                    Type objType = LoadSmartPartType(assemblyInfo.Trim());
                    if (objType != null)
                        obj = Activator.CreateInstance(objType);
                }
            }
            return obj;
        }

        public static Type LoadSmartPartType(string FilePath)
        {
            string asseblyName = string.Empty;
            Assembly targetAssembly = null;
            string _className = null;
            asseblyName = FilePath.Substring(FilePath.LastIndexOf("\\") + 1);
            asseblyName += asseblyName.Substring(0, asseblyName.IndexOf('.'));
            targetAssembly = Assembly.LoadFrom(FilePath);
            //循环医保组件类  一般为Cls_
            foreach (Type clsName in targetAssembly.GetTypes())
            {
                _className = asseblyName + "." + clsName.Name;
                break;
            }
            Type type = null;
            if (targetAssembly == null)
            {
                type = System.Type.GetType(_className);
            }
            else
            {
                type = targetAssembly.GetType(_className);
            }
            return type;
        }
        public static List<string> GetClassName(string FilePath)
        {
            if(!System.IO.File.Exists(FilePath))
            {
                return new List<string>();
            }
            string asseblyName = string.Empty;
            Assembly targetAssembly = null;
            List<string> _className = new List<string>();
            asseblyName = FilePath.Substring(FilePath.LastIndexOf("/") + 1);
            asseblyName =asseblyName.Substring(0, asseblyName.LastIndexOf('.'));
            byte[] b = System.IO.File.ReadAllBytes(FilePath);
            targetAssembly = Assembly.Load(b);
            //循环医保组件类  一般为Cls_
            foreach (Type clsName in targetAssembly.GetTypes())
            {
                if(clsName.FullName.Contains("TimingTask")&& !clsName.FullName.Contains("<>"))
                {
                    _className.Add(asseblyName + "." + clsName.Name);
                }
            }
            return _className;
        }
    }
}
