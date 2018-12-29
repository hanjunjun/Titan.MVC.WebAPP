using System;

namespace Titan.Infrastructure.Domain
{
    public static class MathHelp
    {
        public static int GetMin(int a,int b)
        {
            if (a < b)
            {
                return a;
            }
            return b;
        }

        public static string GetAge(DateTime? birthday)
        {
            if (birthday == null)
            {
                return "";
            }
            var age = (DateTime.Today - birthday.Value).TotalDays / 365.25;
            return Math.Floor(age).ToString();
        }

        /// <summary>
        /// 金额大小写转换
        /// </summary>
        /// <param name="strAmount"></param>
        /// <returns></returns>
        public static string MoneyToUpper(string strAmount)
        {
            string functionReturnValue = null;
            bool IsNegative = false; // 是否是负数
            if (strAmount.Trim().Substring(0, 1) == "-")
            {
                // 是负数则先转为正数
                strAmount = strAmount.Trim().Remove(0, 1);
                IsNegative = true;
            }
            string strLower = null;
            string strUpart = null;
            string strUpper = null;
            int iTemp = 0;
            // 保留两位小数 123.489→123.49　　123.4→123.4
            strAmount = Math.Round(double.Parse(strAmount), 2).ToString();
            if (strAmount.IndexOf(".") > 0)
            {
                if (strAmount.IndexOf(".") == strAmount.Length - 2)
                {
                    strAmount = strAmount + "0";
                }
            }
            else
            {
                strAmount = strAmount + ".00";
            }
            strLower = strAmount;
            iTemp = 1;
            strUpper = "";
            while (iTemp <= strLower.Length)
            {
                switch (strLower.Substring(strLower.Length - iTemp, 1))
                {
                    case ".":
                        strUpart = "圆";
                        break;
                    case "0":
                        strUpart = "零";
                        break;
                    case "1":
                        strUpart = "壹";
                        break;
                    case "2":
                        strUpart = "贰";
                        break;
                    case "3":
                        strUpart = "叁";
                        break;
                    case "4":
                        strUpart = "肆";
                        break;
                    case "5":
                        strUpart = "伍";
                        break;
                    case "6":
                        strUpart = "陆";
                        break;
                    case "7":
                        strUpart = "柒";
                        break;
                    case "8":
                        strUpart = "捌";
                        break;
                    case "9":
                        strUpart = "玖";
                        break;
                }

                switch (iTemp)
                {
                    case 1:
                        strUpart = strUpart + "分";
                    break;
                    case 2:
                        strUpart = strUpart + "角";
                    break;
                    case 3:
                        strUpart = strUpart + "";
                    break;
                    case 4:
                        strUpart = strUpart + "";
                    break;
                    case 5:
                        strUpart = strUpart + "拾";
                    break;
                    case 6:
                        strUpart = strUpart + "佰";
                    break;
                    case 7:
                        strUpart = strUpart + "仟";
                    break;
                    case 8:
                        strUpart = strUpart + "万";
                    break;
                    case 9:
                        strUpart = strUpart + "拾";
                    break;
                    case 10:
                        strUpart = strUpart + "佰";
                    break;
                    case 11:
                        strUpart = strUpart + "仟";
                    break;
                    case 12:
                        strUpart = strUpart + "亿";
                    break;
                    case 13:
                        strUpart = strUpart + "拾";
                    break;
                    case 14:
                        strUpart = strUpart + "佰";
                    break;
                    case 15:
                        strUpart = strUpart + "仟";
                    break;
                    case 16:
                        strUpart = strUpart + "万";
                    break;
                    default:
				        strUpart = strUpart + "";
                    break;
                }
                strUpper = strUpart + strUpper;
                iTemp = iTemp + 1;
            }

            strUpper = strUpper.Replace("零拾", "零");
	        strUpper = strUpper.Replace("零佰", "零");
	        strUpper = strUpper.Replace("零仟", "零");
	        strUpper = strUpper.Replace("零零零", "零");
	        strUpper = strUpper.Replace("零零", "零");
	        strUpper = strUpper.Replace("零角零分", "整");
	        strUpper = strUpper.Replace("零分", "整");
	        strUpper = strUpper.Replace("零角", "零");
	        strUpper = strUpper.Replace("零亿零万零圆", "亿圆");
	        strUpper = strUpper.Replace("亿零万零圆", "亿圆");
	        strUpper = strUpper.Replace("零亿零万", "亿");
	        strUpper = strUpper.Replace("零万零圆", "万圆");
	        strUpper = strUpper.Replace("零亿", "亿");
	        strUpper = strUpper.Replace("零万", "万");
	        strUpper = strUpper.Replace("零圆", "圆");
	        strUpper = strUpper.Replace("零零", "零");

	        // 对壹圆以下的金额的处理
	        if (strUpper.Substring(0, 1) =="圆")
	        {
		        strUpper = strUpper.Substring(1, strUpper.Length -1);
	        }
	        if (strUpper.Substring(0, 1) =="零")
	        {
		        strUpper = strUpper.Substring(1, strUpper.Length -1);
	        }
	        if (strUpper.Substring(0, 1) =="角")
	        {
		        strUpper = strUpper.Substring(1, strUpper.Length -1);
	        }
	        if (strUpper.Substring(0, 1) =="分")
	        {
		        strUpper = strUpper.Substring(1, strUpper.Length -1);
	        }
	        if (strUpper.Substring(0, 1) =="整")
	        {
		        strUpper ="零圆整";
	        }
	        functionReturnValue = strUpper;

	        if (IsNegative ==true)
	        {
		        return"负"+ functionReturnValue;
	        }
	        else
	        {
		        return functionReturnValue;
	        }
        }

        public static string MoneyToUpper(decimal? strAmount)
        {
            return MoneyToUpper(strAmount.GetValueOrDefault().ToString());
        }

        public static string MoneyToUpper2(decimal? strAmount)
        {
            strAmount = Math.Round(strAmount.Value, 2);
            decimal amountReturn = Math.Round(strAmount.Value);
            return MoneyToUpper(amountReturn);
        }

        public static string ToMoneyStr(string strAmount)
        {
            var str = string.Format("{0:C2}", strAmount);
            return str;
        }

        public static string ToMoneyStr(decimal? strAmount)
        {
            var str = string.Format("{0:C2}", strAmount.GetValueOrDefault());
            return str;
        }

        public static string ToMoneyRound(decimal? strAmount)
        {
            strAmount = Math.Round(strAmount.Value, 2);
            decimal amountReturn = Math.Round(strAmount.Value);
            var str = string.Format("{0:C2}", amountReturn);
            return str;
        }
    }
}
