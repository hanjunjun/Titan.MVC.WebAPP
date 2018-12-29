using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Titan.Infrastructure.Utility
{
    public static class StringExtensions
    {
        /// <summary>
        /// 字符串转化为guid,如果转化失败或者字符串为空,则返回Guid.Empty
        /// </summary>
        /// <param name="txt">The text.</param>
        /// <returns></returns>
        public static Guid ToGuid(this string txt)
        {
            if (string.IsNullOrEmpty(txt))
            {
                return Guid.Empty;
            }

            var guid_tmp = Guid.Empty;
            if (Guid.TryParse(txt, out guid_tmp))
            {
                return guid_tmp;
            }
            else
            {
                return Guid.Empty;
            }
        }
    }
}
