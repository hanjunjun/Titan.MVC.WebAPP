/************************************************************************
 * 文件名：ExcelHelper
 * 文件功能描述：Excel帮助类
 * 作    者：hjj
 * 创建日期：2017-06-15
 * 修 改 人：
 * 修改日期：
 * 修改原因：
 * Copyright (c) 2016 Titan.Han . All Rights Reserved. 
 * ***********************************************************************/
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Titan.Infrastructure.Domain;

namespace Titan.Infrastructure.EXCEL
{
    public class ExcelHelper
    {
        /// <summary>
        /// 获取EXCEL对象
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="filePath"></param>
        /// <param name="fileHeader"></param>
        /// <returns></returns>
		public static HSSFWorkbook WriteExcel(DataTable dt, string filePath, string fileHeader = "", List<ExcelColumnStyle> excelColumnStyles = null)
        {
            if (!string.IsNullOrEmpty(filePath) && null != dt && dt.Rows.Count > 0)
            {
                NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
                NPOI.SS.UserModel.ISheet sheet = book.CreateSheet(dt.TableName);
                NPOI.SS.UserModel.IHeader header = sheet.Header;

                int stateNum = 0;

                ICellStyle cellStyle = book.CreateCellStyle();
                cellStyle.Alignment = HorizontalAlignment.Center;
                cellStyle.WrapText = true;

                IFont font1 = book.CreateFont();
                font1.FontHeightInPoints = 12;
                font1.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                cellStyle.SetFont(font1);
                cellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                cellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                cellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                cellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;

                ICellStyle cellStyle2 = book.CreateCellStyle();
                cellStyle2.Alignment = HorizontalAlignment.Center;
                cellStyle2.VerticalAlignment = VerticalAlignment.Center;
                //cellStyle2.WrapText = true;
                IFont font2 = book.CreateFont();
                font2.FontHeightInPoints = 14;
                font2.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                font2.Color = NPOI.HSSF.Util.HSSFColor.Blue.Index;
                cellStyle2.SetFont(font2);

                ICellStyle cellStyle3 = book.CreateCellStyle();
                cellStyle3.Alignment = HorizontalAlignment.Center;
                cellStyle3.VerticalAlignment = VerticalAlignment.Center;
                cellStyle3.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                cellStyle3.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                cellStyle3.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                cellStyle3.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;

                if (fileHeader != "")
                {
                    IRow rowHeader = sheet.CreateRow(stateNum);
                    rowHeader.Height = (short)(500);
                    sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, dt.Columns.Count - 1));
                    rowHeader.CreateCell(0).SetCellValue(fileHeader);
                    stateNum++;
                    rowHeader.GetCell(0).CellStyle = cellStyle2;
                }

                if (dt.Rows.Count != 0)
                {
                    IRow row = sheet.CreateRow(stateNum);
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        row.CreateCell(i).SetCellValue(dt.Columns[i].ColumnName);
                        row.GetCell(i).CellStyle = cellStyle;
                    }

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        IRow row2 = sheet.CreateRow(i + 1 + stateNum);
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            row2.CreateCell(j).SetCellValue(Convert.ToString(dt.Rows[i][j]));
                            if (excelColumnStyles != null && excelColumnStyles.Any(x => x.ColumnName == dt.Columns[j].ColumnName))
                            {
                                var cstyle = book.CreateCellStyle();
                                cellStyle3.MapTo(cstyle);
                                cstyle.Alignment = excelColumnStyles.Find(x => x.ColumnName == dt.Columns[j].ColumnName).CellStyleAlign;
                                row2.GetCell(j).CellStyle = cstyle;
                            }
                            else
                            {
                                row2.GetCell(j).CellStyle = cellStyle3;
                            }
                        }
                    }
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        sheet.AutoSizeColumn(i, true);
                    }
                }
                return book;
            }
            return null;
        }

        /// <summary>
        /// 写xlsx文件
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static XSSFWorkbook WriteExcelXlsx(DataTable dt, string filePath, string fileHeader = "",List<ExcelColumnStyle> excelColumnStyles = null)
        {
            if (string.IsNullOrEmpty(filePath) || dt == null)
            {
                return null;
            }

            XSSFWorkbook book = new XSSFWorkbook();
            ISheet sheet = book.CreateSheet(dt.TableName);
            IHeader header = sheet.Header;
            int stateNum = 0;

            ICellStyle cellStyle = book.CreateCellStyle();
            cellStyle.Alignment = HorizontalAlignment.Center;
            cellStyle.WrapText = true;

            IFont font1 = book.CreateFont();
            font1.FontHeightInPoints = 12;
            font1.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
            cellStyle.SetFont(font1);
            cellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;

            ICellStyle cellStyle2 = book.CreateCellStyle();
            cellStyle2.Alignment = HorizontalAlignment.Center;
            cellStyle2.VerticalAlignment = VerticalAlignment.Center;
            //cellStyle2.WrapText = true;
            IFont font2 = book.CreateFont();
            font2.FontHeightInPoints = 14;
            font2.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
            font2.Color = NPOI.HSSF.Util.HSSFColor.Blue.Index;
            cellStyle2.SetFont(font2);

            ICellStyle cellStyle3 = book.CreateCellStyle();
            cellStyle3.Alignment = HorizontalAlignment.Center;
            cellStyle3.VerticalAlignment = VerticalAlignment.Center;
            cellStyle3.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyle3.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyle3.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyle3.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;

            if (fileHeader != "")
            {
                IRow rowHeader = sheet.CreateRow(stateNum);
                rowHeader.Height = (short)(500);
                sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, dt.Columns.Count - 1));
                rowHeader.CreateCell(0).SetCellValue(fileHeader);
                stateNum++;
                rowHeader.GetCell(0).CellStyle = cellStyle2;
            }

            if (dt.Rows.Count != 0)
            {
                IRow row = sheet.CreateRow(stateNum);
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    row.CreateCell(i).SetCellValue(dt.Columns[i].ColumnName);
                    row.GetCell(i).CellStyle = cellStyle;
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    IRow row2 = sheet.CreateRow(i + 1 + stateNum);
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        row2.CreateCell(j).SetCellValue(Convert.ToString(dt.Rows[i][j]));
                        if (excelColumnStyles !=null && excelColumnStyles.Any(x => x.ColumnName == dt.Columns[j].ColumnName))
                        {
                            var cstyle = book.CreateCellStyle();
                            cellStyle3.MapTo(cstyle);
                            cstyle.Alignment = excelColumnStyles.Find(x => x.ColumnName == dt.Columns[j].ColumnName).CellStyleAlign;
                            row2.GetCell(j).CellStyle = cstyle;
                        }
                        else
                        {
                            row2.GetCell(j).CellStyle = cellStyle3;
                        }
                    }
                }
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    sheet.AutoSizeColumn(i, true);
                }

            }
            return book;
        }



        /// <summary>
        /// 获取当前某个版本Office的安装路径
        /// </summary>
        /// <param name="Path">返回当前系统Office安装路径</param>
        /// <param name="Version">返回当前系统Office版本信息</param>
        public static void GetOfficePath(out string Path, out string Version)
        {
            string strPathResult = "";
            string strVersionResult = "";
            string strKeyName = "Path";
            object objResult = null;
            Microsoft.Win32.RegistryValueKind regValueKind;
            Microsoft.Win32.RegistryKey regKey = null;
            Microsoft.Win32.RegistryKey regSubKey = null;

            try
            {
                regKey = Microsoft.Win32.Registry.LocalMachine;

                if (regSubKey == null)
                {//Office2016
                    regSubKey = regKey.OpenSubKey(@"SOFTWARE\Microsoft\Office\16.0\Common\InstallRoot", false);
                    strVersionResult = "office2007";
                    strKeyName = "Path";
                }

                if (regSubKey == null)
                {//Office2013
                    regSubKey = regKey.OpenSubKey(@"SOFTWARE\Microsoft\Office\15.0\Common\InstallRoot", false);
                    strVersionResult = "office2007";
                    strKeyName = "Path";
                }

                if (regSubKey == null)
                {//Office2010
                    regSubKey = regKey.OpenSubKey(@"SOFTWARE\Microsoft\Office\14.0\Common\InstallRoot", false);
                    strVersionResult = "office2007";
                    strKeyName = "Path";
                }

                if (regSubKey == null)
                {//office2007 
                    regSubKey = regKey.OpenSubKey(@"SOFTWARE\Microsoft\Office\12.0\Common\InstallRoot", false);
                    strVersionResult = "office2007";
                    strKeyName = "Path";
                }

                if (regSubKey == null)
                {//Office2003
                    regSubKey = regKey.OpenSubKey(@"SOFTWARE\Microsoft\Office\11.0\Common\InstallRoot", false);
                    strVersionResult = "office2003";
                    strKeyName = "Path";
                }

                if (regSubKey == null)
                {//officeXp
                    regSubKey = regKey.OpenSubKey(@"SOFTWARE\Microsoft\Office\10.0\Common\InstallRoot", false);
                    strVersionResult = "officeXP";
                    strKeyName = "Path";
                }

                if (regSubKey == null)
                {//Office2000
                    regSubKey = regKey.OpenSubKey(@"SOFTWARE\Microsoft\Office\9.0\Common\InstallRoot", false);
                    strVersionResult = "office2000";
                    strKeyName = "Path";
                }

                if (regSubKey == null)
                {//office97
                    regSubKey = regKey.OpenSubKey(@"SOFTWARE\Microsoft\Office\8.0\Common\InstallRoot", false);
                    strVersionResult = "office97";
                    strKeyName = "OfficeBin";
                }
                //objResult = regSubKey.GetValue(strKeyName);
                //regValueKind = regSubKey.GetValueKind(strKeyName);
                //if (regValueKind == Microsoft.Win32.RegistryValueKind.String)
                //{
                //    strPathResult = objResult.ToString();
                //}
            }
            catch (System.Security.SecurityException ex)
            {
                throw new System.Security.SecurityException("您没有读取注册表的权限", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("读取注册表出错!", ex);
            }
            finally
            {

                if (regKey != null)
                {
                    regKey.Close();
                    regKey = null;
                }

                if (regSubKey != null)
                {
                    regSubKey.Close();
                    regSubKey = null;
                }
            }

            Path = strPathResult;
            Version = strVersionResult;
        }

        /// <summary>
        /// 获取EXCEL-Npoi对象
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="excelName"></param>
        /// <param name="titleName"></param>
        /// <returns></returns>
        public static NpoiMemoryStream GetExcel(DataTable dt, string excelName, string titleName, List<ExcelColumnStyle> excelColumnStyles = null)
        {

            string path = string.Empty; string version = string.Empty;
            ExcelHelper.GetOfficePath(out path, out version);
            if (version == "office2007")
            {
                XSSFWorkbook book = ExcelHelper.WriteExcelXlsx(dt, @excelName, titleName, excelColumnStyles);
                NpoiMemoryStream ms = new NpoiMemoryStream();
                if (book != null)
                {
                    ms.AllowClose = false;
                    book.Write(ms);
                    ms.Flush();
                    ms.Seek(0, SeekOrigin.Begin);
                    ms.AllowClose = true;
                    return ms;
                }
                else
                {
                    return ms;
                }
            }
            else
            {
                HSSFWorkbook book = ExcelHelper.WriteExcel(dt, @excelName, titleName, excelColumnStyles);
                NpoiMemoryStream ms = new NpoiMemoryStream();
                if (book != null)
                {
                    ms.AllowClose = false;
                    book.Write(ms);
                    ms.Flush();
                    ms.Seek(0, SeekOrigin.Begin);
                    ms.AllowClose = true;
                    return ms;
                }
                else
                {
                    return ms;
                }
            }

        }
    }

    /// <summary>
    /// 获取文件流对象
    /// </summary>
	public class NpoiMemoryStream : MemoryStream
    {
        public NpoiMemoryStream()
        {
            AllowClose = true;
        }

        public bool AllowClose { get; set; }

        public override void Close()
        {
            if (AllowClose)
            {
                base.Close();
            }
        }
    }

    public class ExcelColumnStyle
    {
        public string ColumnName { get; set; }

        public HorizontalAlignment CellStyleAlign { get; set; }
    }
}