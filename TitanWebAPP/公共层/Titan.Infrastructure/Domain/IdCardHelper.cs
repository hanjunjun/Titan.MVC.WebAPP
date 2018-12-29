
using System;
using System.Text.RegularExpressions;

namespace Titan.Infrastructure.Domain
{
    public static class IdCardHelper
    {
        /// <summary>
        /// 身份证验证
        /// </summary>
        /// <param name="id">身份证号</param>
        /// <returns></returns>
        public static bool CheckIdCard(string id)
        {
            if (id.Length == 18)
            {
                bool check = CheckIdCard18(id);
                return check;
            }
            if (id.Length != 15) return false;
            {
                bool check = CheckIdCard15(id);
                return check;
            }
        }

        /// <summary>
        /// 18位身份证验证
        /// </summary>
        /// <param name="id">身份证号</param>
        /// <returns></returns>
        private static bool CheckIdCard18(string id)
        {
            long n;
            if (long.TryParse(id.Remove(17), out n) == false || n < Math.Pow(10, 16) || long.TryParse(id.Replace('x', '0').Replace('X', '0'), out n) == false)
            {
                return false;//数字验证
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(id.Remove(2), StringComparison.Ordinal) == -1)
            {
                return false;//省份验证
            }
            string birth = id.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            DateTime time;
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证
            }
            string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
            string[] wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
            char[] ai = id.Remove(17).ToCharArray();
            int sum = 0;
            for (int i = 0; i < 17; i++)
            {
                sum += int.Parse(wi[i]) * int.Parse(ai[i].ToString());
            }
            int y;
            Math.DivRem(sum, 11, out y);
            if (arrVarifyCode[y] != id.Substring(17, 1).ToLower())
            {
                return false;//校验码验证
            }
            return true;//符合GB11643-1999标准
        }

        /// <summary>
        /// 15位身份证验证
        /// </summary>
        /// <param name="id">身份证号</param>
        /// <returns></returns>
        private static bool CheckIdCard15(string id)
        {
            long n;
            if (long.TryParse(id, out n) == false || n < Math.Pow(10, 14))
            {
                return false;//数字验证
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(id.Remove(2), StringComparison.Ordinal) == -1)
            {
                return false;//省份验证
            }
            string birth = id.Substring(6, 6).Insert(4, "-").Insert(2, "-");
            DateTime time;
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证
            }
            return true;//符合15位身份证标准
        }

        /// <summary>
        /// 根据身份证获取身份证信息
        /// 18位身份证
        /// 0地区代码(1~6位,其中1、2位数为各省级政府的代码，3、4位数为地、市级政府的代码，5、6位数为县、区级政府代码)
        /// 1出生年月日(7~14位)
        /// 2顺序号(15~17位单数为男性分配码，双数为女性分配码)
        /// 3性别
        /// 
        /// 15位身份证
        /// 0地区代码 
        /// 1出生年份(7~8位年,9~10位为出生月份，11~12位为出生日期 
        /// 2顺序号(13~15位)，并能够判断性别，奇数为男，偶数为女
        /// 3性别
        /// </summary>
        /// <param name="cardId"></param>
        /// <returns></returns>
        public static string[] GetCardIdInfo(string cardId)
        {
            string[] info = new string[4];
            Regex regex;
            if (cardId.Length == 18)
            {
                regex = new Regex(@"^\d{17}(\d|x)$");
                if (regex.IsMatch(cardId))
                {
                    info.SetValue(cardId.Substring(0, 6), 0);
                    info.SetValue(cardId.Substring(6, 8), 1);
                    info.SetValue(cardId.Substring(14, 3), 2);
                    info.SetValue(Convert.ToInt32(info[2]) % 2 != 0 ? "男" : "女", 3);
                }
            }
            else if (cardId.Length == 15)
            {
                regex = new Regex(@"^\d{15}");
                if (regex.IsMatch(cardId))
                {
                    info.SetValue(cardId.Substring(0, 6), 0);
                    info.SetValue(cardId.Substring(6, 6), 1);
                    info.SetValue(cardId.Substring(12, 3), 2);
                    info.SetValue(Convert.ToInt32(info[2]) % 2 != 0 ? "男" : "女", 3);
                }
            }
            return info;
        }
    }
}
