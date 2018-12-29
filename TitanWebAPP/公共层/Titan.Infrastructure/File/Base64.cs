using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Titan.Infrastructure.File
{
    public class Base64
    {
        //图片转为base64编码的字符串  
        public static string ImgToBase64String(string Imagefilename)
        {
            try
            {
                Bitmap bmp = new Bitmap(Imagefilename);

                MemoryStream ms = new MemoryStream();
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                return Convert.ToBase64String(arr);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //threeebase64编码的字符串转为图片  
        protected Bitmap Base64StringToImage(string strbase64)
        {
            try
            {
                byte[] arr = Convert.FromBase64String(strbase64);
                MemoryStream ms = new MemoryStream(arr);
                Bitmap bmp = new Bitmap(ms);

                bmp.Save(@"d:\test.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                //bmp.Save(@"d:\"test.bmp", ImageFormat.Bmp);  
                //bmp.Save(@"d:\"test.gif", ImageFormat.Gif);  
                //bmp.Save(@"d:\"test.png", ImageFormat.Png);  
                ms.Close();
                return bmp;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 保存base64图片到服务器--单图
        /// </summary>
        /// <param name="strBase64"></param>
        /// <param name="savefilePath"></param>
        public static void SaveBase64Image(string strBase64,string savefilePath)
        {
            if (!Directory.Exists(System.IO.Path.GetDirectoryName(savefilePath)))
            {
                Directory.CreateDirectory(System.IO.Path.GetDirectoryName(savefilePath));
            }
            byte[] bytes = System.Convert.FromBase64String(strBase64);
            System.IO.File.WriteAllBytes(savefilePath, bytes);
        }

        public static string GetIcoTypePath(string fileUrl)
        {
            if (!string.IsNullOrEmpty(fileUrl))
            {
                var iExtension = Path.GetExtension(fileUrl)?.ToLower();

                var excelIco = new[] { ".xls", ".xlsx" };
                var pdfIco = new[] { ".pdf" };
                var wordIco = new[] { ".doc", ".docx" };
                var rarIco = new[] { ".rar", ".zip", ".7z" };
                var jpgIco = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                if (excelIco.Contains(iExtension))
                {
                    return "1^" + "../../img/EXCEL.png";
                }
                if (pdfIco.Contains(iExtension))
                {
                    return "2^" + "../../img/PDF.png";
                }
                if (wordIco.Contains(iExtension))
                {
                    return "3^" + "../../img/WORD.png";
                }
                if (rarIco.Contains(iExtension))
                {
                    return "4^" + "../../img/RAR.png";
                }
                return jpgIco.Contains(iExtension) ? "0^" : null;
            }
            return null;
        }
    }
}
