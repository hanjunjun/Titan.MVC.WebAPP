using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace Titan.Infrastructure.Domain
{
    public static class CommonDataHelper
    {

        /// <summary>
        /// 根据日期计算年龄
        /// </summary>
        /// <param name="strBirthday"></param>
        /// <returns></returns>
        public static string GetAgeByBirthday(string strBirthday)
        {
            if (!string.IsNullOrEmpty(strBirthday))
            {
                DateTime birthday1 = Convert.ToDateTime(strBirthday);
                int age = DateTime.Now.Year - birthday1.Year;
                if (DateTime.Now.Month < birthday1.Month || (DateTime.Now.Month == birthday1.Month && DateTime.Now.Day < birthday1.Day))
                    age--;
                if (age == -1)
                    age = 0;
                return age.ToString();
            }
            else
                return "";
        }

        /// <summary>
        /// 根据日期计算年龄（指定日期）
        /// </summary>
        /// <param name="strBirthday"></param>
        /// <returns></returns>
        public static string GetAgeByBirthday(string strBirthday,DateTime date0)
        {
            if (!string.IsNullOrEmpty(strBirthday))
            {
                DateTime birthday1 = Convert.ToDateTime(strBirthday);
                int age = date0.Year - birthday1.Year;
                if (date0.Month < birthday1.Month || (date0.Month == birthday1.Month && date0.Day < birthday1.Day))
                    age--;
                if (age == -1)
                    age = 0;
                return age.ToString();
            }
            else
                return "";
        }

        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dest"></param>
        public static void CreateImgSize(string src, string dest,int thumbWidth = 140, int thumbHeight = 140)
        {
            System.Drawing.Image image = System.Drawing.Image.FromFile(src); //利用Image对象装载源图像

            //接着创建一个System.Drawing.Bitmap对象，并设置你希望的缩略图的宽度和高度。
            int srcWidth = image.Width;
            int srcHeight = image.Height;
            Bitmap bmp = new Bitmap(thumbWidth, thumbHeight);

            //从Bitmap创建一个System.Drawing.Graphics对象，用来绘制高质量的缩小图。
            System.Drawing.Graphics gr = System.Drawing.Graphics.FromImage(bmp);

            //设置 System.Drawing.Graphics对象的SmoothingMode属性为HighQuality
            gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //下面这个也设成高质量
            gr.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

            //下面这个设成High
            gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //把原始图像绘制成上面所设置宽高的缩小图
            System.Drawing.Rectangle rectDestination = new System.Drawing.Rectangle(0, 0, thumbWidth, thumbHeight);
            gr.DrawImage(image, rectDestination, 0, 0, srcWidth, srcHeight, GraphicsUnit.Pixel);

            //保存图像，大功告成！
            bmp.Save(dest);

            //最后别忘了释放资源
            bmp.Dispose();
            image.Dispose();
        }

        /// <summary>
        /// 根据分隔符，拆解字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 金额大小写转化
        /// </summary>
        /// <param name="Money"></param>
        /// <returns></returns>
        public static string ConvertMoney(decimal Money)
        {
            //金额转换程序  
            string MoneyNum = "";//记录小写金额字符串[输入参数]  
            string MoneyStr = "";//记录大写金额字符串[输出参数]  
            string BNumStr = "零壹贰叁肆伍陆柒捌玖";//模  
            string UnitStr = "万仟佰拾亿仟佰拾万仟佰拾圆角分";//模  

            MoneyNum = ((long)(Money * 100)).ToString();
            for (int i = 0; i < MoneyNum.Length; i++)
            {
                string DVar = "";//记录生成的单个字符(大写)  
                string UnitVar = "";//记录截取的单位  
                for (int n = 0; n < 10; n++)
                {
                    //对比后生成单个字符(大写)  
                    if (Convert.ToInt32(MoneyNum.Substring(i, 1)) == n)
                    {
                        DVar = BNumStr.Substring(n, 1);//取出单个大写字符  
                        //给生成的单个大写字符加单位  
                        UnitVar = UnitStr.Substring(15 - (MoneyNum.Length)).Substring(i, 1);
                        n = 10;//退出循环  
                    }
                }
                //生成大写金额字符串  
                MoneyStr = MoneyStr + DVar + UnitVar;
            }
            //二次处理大写金额字符串  
            MoneyStr = MoneyStr + "整";
            while (MoneyStr.Contains("零分") || MoneyStr.Contains("零角") || MoneyStr.Contains("零佰") || MoneyStr.Contains("零仟")
                || MoneyStr.Contains("零万") || MoneyStr.Contains("零亿") || MoneyStr.Contains("零零") || MoneyStr.Contains("零圆")
                || MoneyStr.Contains("亿万") || MoneyStr.Contains("零整") || MoneyStr.Contains("分整"))
            {
                MoneyStr = MoneyStr.Replace("零分", "零");
                MoneyStr = MoneyStr.Replace("零角", "零");
                MoneyStr = MoneyStr.Replace("零拾", "零");
                MoneyStr = MoneyStr.Replace("零佰", "零");
                MoneyStr = MoneyStr.Replace("零仟", "零");
                MoneyStr = MoneyStr.Replace("零万", "万");
                MoneyStr = MoneyStr.Replace("零亿", "亿");
                MoneyStr = MoneyStr.Replace("亿万", "亿");
                MoneyStr = MoneyStr.Replace("零零", "零");
                MoneyStr = MoneyStr.Replace("零圆", "圆零");
                MoneyStr = MoneyStr.Replace("零整", "整");
                MoneyStr = MoneyStr.Replace("分整", "分");
            }
            if (MoneyStr == "整")
            {
                MoneyStr = "零元整";
            }
            return MoneyStr;
        }
    }
}