using System.Collections.Generic;
using System.Linq;

namespace Titan.Infrastructure.Domain
{
    public static class StringHelp
    {
        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="str">原拼接字符串</param>
        /// <param name="p1">1级分隔符</param>
        /// <param name="p2">2级分隔符</param>
        public static List<List<string>> GetStr(string str, char p1, char p2)
        {
            List<List<string>> data = new List<List<string>>();
            if (!string.IsNullOrEmpty(str))
            {
                var arr = str.Split(p1);
                if (arr.Length > 0)
                {
                    foreach (var item in arr)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            var list = item.Split(p2).ToList();
                            data.Add(list);
                        }
                    }
                }
            }
            return data;
        }

        public static string GetValue(string str)
        {
            return str ?? "";
        }

        public static string TrimStart(string souce,string str)
        {
            var newStr = souce ?? "";
            if (!string.IsNullOrEmpty(str))
            {
                if (souce.Substring(0, str.Length) == str)
                {
                    newStr = souce.Substring(str.Length);
                }
            }
            return newStr;
        }
    }
}
