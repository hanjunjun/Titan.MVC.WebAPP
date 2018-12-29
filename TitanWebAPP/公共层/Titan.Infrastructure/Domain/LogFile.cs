using System;
using System.IO;

namespace Titan.Infrastructure.Domain
{
    /// <summary>
    /// 写入日志文件
    /// </summary>
    public class LogFile
    {
        /// <summary>
        /// 日志文件名
        /// </summary>
        private readonly string _fileName;

        public LogFile(string fileName = "Log.txt")
        {
            _fileName = fileName;
        }
        /// <summary>
        /// 写入日志文件
        /// </summary>
        public void WriteLogFile(string input)
        {
            //指定日志文件的目录
            var strPath = AppDomain.CurrentDomain.BaseDirectory;  //当前路径
            strPath = strPath + "\\logs\\ActionLog\\" + $"{ DateTime.Now:yyyyMMdd}";
            var fname = strPath + $"\\{_fileName}";
            //定义文件信息对象
            var finfo = new FileInfo(fname);
            if (Directory.Exists(finfo.DirectoryName) == false)//如果不存在就创建file文件夹
            {
                Directory.CreateDirectory(finfo.DirectoryName);
            }
            if (!finfo.Exists)
            {
                //Directory.CreateDirectory(strPath);
                var fs = System.IO.File.Create(fname);
                fs.Close();
                finfo = new FileInfo(fname);
            }
            //判断文件是否存在以及是否大于2K
            if (finfo.Length > 1024 * 10 * 1024)
            {
                //var fname2 = strPath + $"\\{DateTime.Now.ToString("yyyyMMddhhmmss") + _fileName}";
                //File.Move(fname, fname2);//文件超过10MB则重命名
                //finfo.Delete();       //删除该文件


                try
                {
                    var fileMove = strPath + "\\" + $"{DateTime.Now:yyyyMMddHHmmssffff}" + _fileName;

                    //var fileMoveinfo = new FileInfo(fileMove);
                    /**/
                    //文件超过10MB则重命名
                    System.IO.File.Move(fname, fileMove);
                    /**/
                    //删除该文件
                    //finfo.Delete();

                    //if (!finfo.Exists)
                    //{
                    //    var fs = File.Create(fname);
                    //    fs.Close();
                    //    finfo = new FileInfo(fname);
                    //}
                    //else
                    //{
                    //    if (Directory.Exists(finfo.DirectoryName) == false)//如果不存在就创建file文件夹
                    //    {
                    //        Directory.CreateDirectory(finfo.DirectoryName);
                    //    }
                    //    if (!finfo.Exists)
                    //    {
                    //        //Directory.CreateDirectory(strPath);
                    //        var fs = File.Create(fname);
                    //        fs.Close();
                    //        finfo = new FileInfo(fname);
                    //    }
                    //}
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

            }
            //finfo.AppendText();
            //创建只写文件流
            using (FileStream fs = finfo.OpenWrite())
            {
                StreamWriter w = new StreamWriter(fs);      //根据上面创建的文件流创建写数据流
                w.BaseStream.Seek(0, SeekOrigin.End);       //设置写数据流的起始位置为文件流的末尾
                w.Write("\r\nLog Entry : ");                //写入“Log Entry : ”
                w.Write("{0} {1} \r\n", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString());     //写入当前系统时间并换行
                w.Write(input + "\r\n");                    //写入日志内容并换行
                //w.Write("------------------------------------\n");//写入------------------------------------“并换行
                w.Flush();                                  //清空缓冲区内容，并把缓冲区内容写入基础流
                w.Close();                                  //关闭写数据流
            }
        }

        public void WriteLogFile(string strTitle, string strContent)
        {
            WriteLogFile(strTitle + ":" + strContent);
        }
    }
}