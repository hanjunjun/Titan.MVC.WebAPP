/************************************************************************
 * 文件名：ConserveIdHelper
 * 文件功能描述：入院编号类
 * 作    者：zhouliangliang
 * 创建日期：2018-01-24
 * 修 改 人：
 * 修改日期：
 * 修改原因：
 * Copyright (c) 2016 Titan.Han . All Rights Reserved. 
 * ***********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Titan.Infrastructure.Domain
{
    public interface ConserveIdHelper
    {
        string GetConserveId(int icount, string CompanyElderCode, string MaxConserveId, List<string> numlist);
    }

    /// <summary>
    /// 前缀-年份-编号
    /// </summary>
    public class PYCConserveId : ConserveIdHelper
    {
        public string GetConserveId(int icount, string CompanyElderCode, string MaxConserveId, List<string> numlist)
        {
            string conserveId = "";
            string NewConserveId = MaxConserveId.Substring(CompanyElderCode.Length + 4, 4).TrimStart('0');
            switch (NewConserveId.Length)
            {
                case 1:
                    if (numlist.IndexOf(NewConserveId) == (numlist.Count - 1))
                        conserveId = CompanyElderCode + DateTime.Now.Year + "00" + numlist[1] + numlist[1];
                    else
                        conserveId = CompanyElderCode + DateTime.Now.Year + "000" + numlist[numlist.IndexOf(NewConserveId) + 1];
                    break;
                case 2:
                    if (numlist.IndexOf(NewConserveId.Substring(1, 1)) == (numlist.Count - 1))
                    {
                        if (numlist.IndexOf(NewConserveId.Substring(0, 1)) == (numlist.Count - 1))
                            conserveId = CompanyElderCode + DateTime.Now.Year + "0" + numlist[1] + numlist[0] + numlist[1];
                        else
                            conserveId = CompanyElderCode + DateTime.Now.Year + "00" + numlist[numlist.IndexOf(NewConserveId.Substring(0, 1)) + 1] + numlist[1];
                    }
                    else
                    {
                        conserveId = CompanyElderCode + DateTime.Now.Year + "00" + NewConserveId.Substring(0, 1) + numlist[numlist.IndexOf(NewConserveId.Substring(1, 1)) + 1];
                    }
                    break;
                case 3:
                    if (numlist.IndexOf(NewConserveId.Substring(2, 1)) == (numlist.Count - 1))
                    {
                        if (numlist.IndexOf(NewConserveId.Substring(1, 1)) == (numlist.Count - 1))
                        {
                            if (numlist.IndexOf(NewConserveId.Substring(0, 1)) == (numlist.Count - 1))
                                conserveId = CompanyElderCode + DateTime.Now.Year + numlist[1] + numlist[0] + numlist[0] + numlist[1];
                            else
                                conserveId = CompanyElderCode + DateTime.Now.Year + "0" + numlist[numlist.IndexOf(NewConserveId.Substring(0, 1)) + 1] + numlist[1] + numlist[1];
                        }
                        else
                            conserveId = CompanyElderCode + DateTime.Now.Year + "0" + NewConserveId.Substring(0, 1) + numlist[numlist.IndexOf(NewConserveId.Substring(1, 1)) + 1] + numlist[1];
                    }
                    else
                    {
                        conserveId = CompanyElderCode + DateTime.Now.Year + "0" + NewConserveId.Substring(0, 2) + numlist[numlist.IndexOf(NewConserveId.Substring(2, 1)) + 1];
                    }
                    break;
                case 4:
                    if (numlist.IndexOf(NewConserveId.Substring(3, 1)) == (numlist.Count - 1))
                    {
                        if (numlist.IndexOf(NewConserveId.Substring(2, 1)) == (numlist.Count - 1))
                        {
                            if (numlist.IndexOf(NewConserveId.Substring(1, 1)) == (numlist.Count - 1))
                            {
                                if (numlist.IndexOf(NewConserveId.Substring(0, 1)) == (numlist.Count - 1))
                                    conserveId = "error";
                                else
                                    conserveId = CompanyElderCode + DateTime.Now.Year + numlist[numlist.IndexOf(NewConserveId.Substring(0, 1)) + 1] + numlist[0] + numlist[0] + numlist[1];
                            }
                            else
                                conserveId = CompanyElderCode + DateTime.Now.Year + NewConserveId.Substring(0, 1) + numlist[numlist.IndexOf(NewConserveId.Substring(1, 1)) + 1] + numlist[0] + numlist[1];
                        }
                        else
                            conserveId = CompanyElderCode + DateTime.Now.Year + NewConserveId.Substring(0, 2) + numlist[numlist.IndexOf(NewConserveId.Substring(2, 1)) + 1] + numlist[1];
                    }
                    else
                        conserveId = CompanyElderCode + DateTime.Now.Year + NewConserveId.Substring(0, 3) + numlist[numlist.IndexOf(NewConserveId.Substring(3, 1)) + 1];
                    break;
            }
            return conserveId;
        }
    }

    /// <summary>
    /// 前缀-编号
    /// </summary>
    public class PCConserveId : ConserveIdHelper
    {
        public string GetConserveId(int icount, string CompanyElderCode, string MaxConserveId, List<string> numlist)
        {
            string conserveId = "";
            string NewConserveId = MaxConserveId.Substring(CompanyElderCode.Length, 4).TrimStart('0');
            switch (NewConserveId.Length)
            {
                case 1:
                    if (numlist.IndexOf(NewConserveId) == (numlist.Count - 1))
                        conserveId = CompanyElderCode + "00" + numlist[1] + numlist[1];
                    else
                        conserveId = CompanyElderCode + "000" + numlist[numlist.IndexOf(NewConserveId) + 1];
                    break;
                case 2:
                    if (numlist.IndexOf(NewConserveId.Substring(1, 1)) == (numlist.Count - 1))
                    {
                        if (numlist.IndexOf(NewConserveId.Substring(0, 1)) == (numlist.Count - 1))
                            conserveId = CompanyElderCode + "0" + numlist[1] + numlist[0] + numlist[1];
                        else
                            conserveId = CompanyElderCode + "00" + numlist[numlist.IndexOf(NewConserveId.Substring(0, 1)) + 1] + numlist[1];
                    }
                    else
                    {
                        conserveId = CompanyElderCode + "00" + NewConserveId.Substring(0, 1) + numlist[numlist.IndexOf(NewConserveId.Substring(1, 1)) + 1];
                    }
                    break;
                case 3:
                    if (numlist.IndexOf(NewConserveId.Substring(2, 1)) == (numlist.Count - 1))
                    {
                        if (numlist.IndexOf(NewConserveId.Substring(1, 1)) == (numlist.Count - 1))
                        {
                            if (numlist.IndexOf(NewConserveId.Substring(0, 1)) == (numlist.Count - 1))
                                conserveId = CompanyElderCode + numlist[1] + numlist[0] + numlist[0] + numlist[1];
                            else
                                conserveId = CompanyElderCode + "0" + numlist[numlist.IndexOf(NewConserveId.Substring(0, 1)) + 1] + numlist[1] + numlist[1];
                        }
                        else
                            conserveId = CompanyElderCode + "0" + NewConserveId.Substring(0, 1) + numlist[numlist.IndexOf(NewConserveId.Substring(1, 1)) + 1] + numlist[1];
                    }
                    else
                    {
                        conserveId = CompanyElderCode + "0" + NewConserveId.Substring(0, 2) + numlist[numlist.IndexOf(NewConserveId.Substring(2, 1)) + 1];
                    }
                    break;
                case 4:
                    if (numlist.IndexOf(NewConserveId.Substring(3, 1)) == (numlist.Count - 1))
                    {
                        if (numlist.IndexOf(NewConserveId.Substring(2, 1)) == (numlist.Count - 1))
                        {
                            if (numlist.IndexOf(NewConserveId.Substring(1, 1)) == (numlist.Count - 1))
                            {
                                if (numlist.IndexOf(NewConserveId.Substring(0, 1)) == (numlist.Count - 1))
                                    conserveId = "error";
                                else
                                    conserveId = CompanyElderCode + numlist[numlist.IndexOf(NewConserveId.Substring(0, 1)) + 1] + numlist[0] + numlist[0] + numlist[1];
                            }
                            else
                                conserveId = CompanyElderCode + NewConserveId.Substring(0, 1) + numlist[numlist.IndexOf(NewConserveId.Substring(1, 1)) + 1] + numlist[0] + numlist[1];
                        }
                        else
                            conserveId = CompanyElderCode + NewConserveId.Substring(0, 2) + numlist[numlist.IndexOf(NewConserveId.Substring(2, 1)) + 1] + numlist[1];
                    }
                    else
                        conserveId = CompanyElderCode + NewConserveId.Substring(0, 3) + numlist[numlist.IndexOf(NewConserveId.Substring(3, 1)) + 1];
                    break;
            }
            return conserveId;
        }
    }

    /// <summary>
    /// 年份-编号
    /// </summary>
    public class YCConserveId : ConserveIdHelper
    {
        public string GetConserveId(int icount, string CompanyElderCode, string MaxConserveId, List<string> numlist)
        {
            string conserveId = "";
            string NewConserveId = MaxConserveId.Substring(DateTime.Now.Year.ToString().Length, 4).TrimStart('0');
            switch (NewConserveId.Length)
            {
                case 1:
                    if (numlist.IndexOf(NewConserveId) == (numlist.Count - 1))
                        conserveId = DateTime.Now.Year + "00" + numlist[1] + numlist[1];
                    else
                        conserveId = DateTime.Now.Year + "000" + numlist[numlist.IndexOf(NewConserveId) + 1];
                    break;
                case 2:
                    if (numlist.IndexOf(NewConserveId.Substring(1, 1)) == (numlist.Count - 1))
                    {
                        if (numlist.IndexOf(NewConserveId.Substring(0, 1)) == (numlist.Count - 1))
                            conserveId = DateTime.Now.Year + "0" + numlist[1] + numlist[0] + numlist[1];
                        else
                            conserveId = DateTime.Now.Year + "00" + numlist[numlist.IndexOf(NewConserveId.Substring(0, 1)) + 1] + numlist[1];
                    }
                    else
                    {
                        conserveId = DateTime.Now.Year + "00" + NewConserveId.Substring(0, 1) + numlist[numlist.IndexOf(NewConserveId.Substring(1, 1)) + 1];
                    }
                    break;
                case 3:
                    if (numlist.IndexOf(NewConserveId.Substring(2, 1)) == (numlist.Count - 1))
                    {
                        if (numlist.IndexOf(NewConserveId.Substring(1, 1)) == (numlist.Count - 1))
                        {
                            if (numlist.IndexOf(NewConserveId.Substring(0, 1)) == (numlist.Count - 1))
                                conserveId = DateTime.Now.Year + numlist[1] + numlist[0] + numlist[0] + numlist[1];
                            else
                                conserveId = DateTime.Now.Year + "0" + numlist[numlist.IndexOf(NewConserveId.Substring(0, 1)) + 1] + numlist[1] + numlist[1];
                        }
                        else
                            conserveId = DateTime.Now.Year + "0" + NewConserveId.Substring(0, 1) + numlist[numlist.IndexOf(NewConserveId.Substring(1, 1)) + 1] + numlist[1];
                    }
                    else
                    {
                        conserveId = DateTime.Now.Year + "0" + NewConserveId.Substring(0, 2) + numlist[numlist.IndexOf(NewConserveId.Substring(2, 1)) + 1];
                    }
                    break;
                case 4:
                    if (numlist.IndexOf(NewConserveId.Substring(3, 1)) == (numlist.Count - 1))
                    {
                        if (numlist.IndexOf(NewConserveId.Substring(2, 1)) == (numlist.Count - 1))
                        {
                            if (numlist.IndexOf(NewConserveId.Substring(1, 1)) == (numlist.Count - 1))
                            {
                                if (numlist.IndexOf(NewConserveId.Substring(0, 1)) == (numlist.Count - 1))
                                    conserveId = "error";
                                else
                                    conserveId = DateTime.Now.Year + numlist[numlist.IndexOf(NewConserveId.Substring(0, 1)) + 1] + numlist[0] + numlist[0] + numlist[1];
                            }
                            else
                                conserveId = DateTime.Now.Year + NewConserveId.Substring(0, 1) + numlist[numlist.IndexOf(NewConserveId.Substring(1, 1)) + 1] + numlist[0] + numlist[1];
                        }
                        else
                            conserveId = DateTime.Now.Year + NewConserveId.Substring(0, 2) + numlist[numlist.IndexOf(NewConserveId.Substring(2, 1)) + 1] + numlist[1];
                    }
                    else
                        conserveId = DateTime.Now.Year + NewConserveId.Substring(0, 3) + numlist[numlist.IndexOf(NewConserveId.Substring(3, 1)) + 1];
                    break;
            }
            return conserveId;
        }
    }

    /// <summary>
    /// 编号
    /// </summary>
    public class CConserveId : ConserveIdHelper
    {
        public string GetConserveId(int icount, string CompanyElderCode, string MaxConserveId, List<string> numlist)
        {
            string conserveId = "";
            string NewConserveId = MaxConserveId.Substring(0, 4).TrimStart('0');
            switch (NewConserveId.Length)
            {
                case 1:
                    if (numlist.IndexOf(NewConserveId) == (numlist.Count - 1))
                        conserveId = "00" + numlist[1] + numlist[1];
                    else
                        conserveId = "000" + numlist[numlist.IndexOf(NewConserveId) + 1];
                    break;
                case 2:
                    if (numlist.IndexOf(NewConserveId.Substring(1, 1)) == (numlist.Count - 1))
                    {
                        if (numlist.IndexOf(NewConserveId.Substring(0, 1)) == (numlist.Count - 1))
                            conserveId = "0" + numlist[1] + numlist[0] + numlist[1];
                        else
                            conserveId = "00" + numlist[numlist.IndexOf(NewConserveId.Substring(0, 1)) + 1] + numlist[1];
                    }
                    else
                    {
                        conserveId = "00" + NewConserveId.Substring(0, 1) + numlist[numlist.IndexOf(NewConserveId.Substring(1, 1)) + 1];
                    }
                    break;
                case 3:
                    if (numlist.IndexOf(NewConserveId.Substring(2, 1)) == (numlist.Count - 1))
                    {
                        if (numlist.IndexOf(NewConserveId.Substring(1, 1)) == (numlist.Count - 1))
                        {
                            if (numlist.IndexOf(NewConserveId.Substring(0, 1)) == (numlist.Count - 1))
                                conserveId = numlist[1] + numlist[0] + numlist[0] + numlist[1];
                            else
                                conserveId = "0" + numlist[numlist.IndexOf(NewConserveId.Substring(0, 1)) + 1] + numlist[1] + numlist[1];
                        }
                        else
                            conserveId = "0" + NewConserveId.Substring(0, 1) + numlist[numlist.IndexOf(NewConserveId.Substring(1, 1)) + 1] + numlist[1];
                    }
                    else
                    {
                        conserveId = "0" + NewConserveId.Substring(0, 2) + numlist[numlist.IndexOf(NewConserveId.Substring(2, 1)) + 1];
                    }
                    break;
                case 4:
                    if (numlist.IndexOf(NewConserveId.Substring(3, 1)) == (numlist.Count - 1))
                    {
                        if (numlist.IndexOf(NewConserveId.Substring(2, 1)) == (numlist.Count - 1))
                        {
                            if (numlist.IndexOf(NewConserveId.Substring(1, 1)) == (numlist.Count - 1))
                            {
                                if (numlist.IndexOf(NewConserveId.Substring(0, 1)) == (numlist.Count - 1))
                                    conserveId = "error";
                                else
                                    conserveId = numlist[numlist.IndexOf(NewConserveId.Substring(0, 1)) + 1] + numlist[0] + numlist[0] + numlist[1];
                            }
                            else
                                conserveId = NewConserveId.Substring(0, 1) + numlist[numlist.IndexOf(NewConserveId.Substring(1, 1)) + 1] + numlist[0] + numlist[1];
                        }
                        else
                            conserveId = NewConserveId.Substring(0, 2) + numlist[numlist.IndexOf(NewConserveId.Substring(2, 1)) + 1] + numlist[1];
                    }
                    else
                        conserveId = DateTime.Now.Year + NewConserveId.Substring(0, 3) + numlist[numlist.IndexOf(NewConserveId.Substring(3, 1)) + 1];
                    break;
            }
            return conserveId;

        }
    }

    public class ConserveIdBase
    {
        public string conserveId;
        private ConserveIdHelper _conserveIdHelper;
        public ConserveIdBase(int icount, string CompanyElderCode, string MaxConserveId, List<string> numlist, string rule)
        {
            switch (rule)
            {
                //默认前缀-年份-编号
                case "":
                    if (icount == 0)
                    {
                        this.conserveId = CompanyElderCode + DateTime.Now.Year + numlist[1].PadLeft(4, '0');
                    }
                    else
                    {
                        this._conserveIdHelper = new PYCConserveId();
                        this.conserveId = this._conserveIdHelper.GetConserveId(icount, CompanyElderCode, MaxConserveId, numlist);
                    }
                    break;
                //默认前缀-年份-编号
                case "p-y-0000":
                    if (icount == 0)
                    {
                        this.conserveId = CompanyElderCode + DateTime.Now.Year + numlist[1].PadLeft(4, '0');
                    }
                    else
                    {
                        this._conserveIdHelper = new PYCConserveId();
                        this.conserveId = this._conserveIdHelper.GetConserveId(icount, CompanyElderCode, MaxConserveId, numlist);
                    }
                    break;
                //前缀-编号
                case "p-0000":
                    if (icount == 0)
                    {
                        this.conserveId = CompanyElderCode + numlist[1].PadLeft(4, '0');
                    }
                    else
                    {
                        this._conserveIdHelper = new PCConserveId();
                        this.conserveId = this._conserveIdHelper.GetConserveId(icount, CompanyElderCode, MaxConserveId, numlist);
                    }
                    break;
                //年份-编号
                case "y-0000":
                    if (icount == 0)
                    {
                        this.conserveId = DateTime.Now.Year + numlist[1].PadLeft(4, '0');
                    }
                    else
                    {
                        this._conserveIdHelper = new YCConserveId();
                        this.conserveId = this._conserveIdHelper.GetConserveId(icount, CompanyElderCode, MaxConserveId, numlist);
                    }
                    break;
                //编号
                case "0000":
                    if (icount == 0)
                    {
                        this.conserveId = numlist[1].PadLeft(4, '0');
                    }
                    else
                    {
                        this._conserveIdHelper = new CConserveId();
                        this.conserveId = this._conserveIdHelper.GetConserveId(icount, CompanyElderCode, MaxConserveId, numlist);
                    }
                    break;
                //默认前缀-年份-编号
                default:
                    if (icount == 0)
                    {
                        this.conserveId = CompanyElderCode + DateTime.Now.Year + numlist[1].PadLeft(4, '0');
                    }
                    else
                    {
                        this._conserveIdHelper = new PYCConserveId();
                        this.conserveId = this._conserveIdHelper.GetConserveId(icount, CompanyElderCode, MaxConserveId, numlist);
                    }
                    break;
            }
        }

    }
}
