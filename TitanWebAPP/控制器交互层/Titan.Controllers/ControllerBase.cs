/************************************************************************
 * 文件名：ControllerBase
 * 文件功能描述：控制层母版类
 * 作    者：hjj
 * 创建日期：2017-06-15
 * 修 改 人：
 * 修改日期：
 * 修改原因：
 * Copyright (c) 2016 Titan.Han . All Rights Reserved. 
 * ***********************************************************************/
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Web.Security;
using Titan.AppService.DomainService;
using Titan.AppService.ModelDTO;
using Titan.AppService.ModelOtherService;
using Titan.AppService.ModelService;
using Titan.Controllers.ViewModel;
using Titan.Controllers.ViewModel.CommonViewModel;
using Titan.Infrastructure.Communicate;
using Titan.Infrastructure.Domain;
using Titan.Model.DataModel;

namespace Titan.Controllers
{
    public class ControllerBase : Controller
    {
        protected readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 日志记录
        /// </summary>
        /// <param name="filterContext"></param>
		protected override void OnException(ExceptionContext filterContext)
        {
            Log(filterContext);
            base.OnException(filterContext);
        }

        /// <summary>
        /// 日志记录
        /// </summary>
        /// <param name="filterContext"></param>
		private void Log(ExceptionContext filterContext)
        {
            Logger.Error(filterContext.Exception);
        }

        /// <summary>
        /// 登录人主键Id
        /// </summary>
        protected Guid UserId
        {
            get
            {
                Guid g = Guid.Empty;
                if (ViewData["CommonDataUserId"] != null)
                {
                    g = new Guid(ViewData["CommonDataUserId"].ToString());
                }
                return g;
            }
        }

        /// <summary>
        /// 登录人姓名
        /// </summary>
        protected string UserName
        {
            get
            {
                string g = string.Empty;
                if (ViewData["CommonDataUserName"] != null)
                {
                    g = ViewData["CommonDataUserName"].ToString();
                }
                return g;
            }
        }

        /// <summary>
        /// 登录人登入机构名
        /// </summary>
        protected string CompanyName
        {
            get
            {
                string g = string.Empty;
                if (ViewData["CommonDataCompanyName"] != null)
                {
                    g = ViewData["CommonDataCompanyName"].ToString();
                }
                return g;
            }
        }

        /// <summary>
        /// 登录人当前登入机构
        /// </summary>
        protected Guid CID
        {
            get
            {
                Guid g = Guid.Empty;
                if (ViewData["CommonDataCID"] != null)
                {
                    g = new Guid(ViewData["CommonDataCID"].ToString());
                }
                return g;
            }
        }

        /// <summary>
        /// 登陆人所属机构
        /// </summary>
        protected Guid UserCID
        {
            get
            {
                Guid g = Guid.Empty;
                if (ViewData["CommonDataUserCID"] != null)
                {
                    g = new Guid(ViewData["CommonDataUserCID"].ToString());
                }
                return g;
            }
        }

        /// <summary>
        /// 登录人部门Id
        /// </summary>
        protected Guid DeptId
        {
            get
            {
                Guid g = Guid.Empty;
                if (ViewData["CommonDataDeptId"] != null)
                {
                    g = new Guid(ViewData["CommonDataDeptId"].ToString());
                }
                return g;
            }
        }

        /// <summary>
        /// 登录人岗位Id
        /// </summary>
        protected Guid PostId
        {
            get
            {
                Guid g = Guid.Empty;
                if (ViewData["CommonDataPostId"] != null)
                {
                    g = new Guid(ViewData["CommonDataPostId"].ToString());
                }
                return g;
            }
        }

        /// <summary>
        /// 登录人登入帐号
        /// </summary>
        protected string LoginNo
        {
            get
            {
                string g = string.Empty;
                if (ViewData["CommonDataLoginNo"] != null)
                {
                    g = ViewData["CommonDataLoginNo"].ToString();
                }
                return g;
            }
        }

        /// <summary>
        /// 登录人所属机构编号
        /// </summary>
        protected string CompanyCode
        {
            get
            {
                string g = string.Empty;
                if (ViewData["CommonDataCompanyCode"] != null)
                {
                    g = ViewData["CommonDataCompanyCode"].ToString();
                }
                return g;
            }
        }

        protected string SysTitle
        {
            get
            {
                string g = string.Empty;
                if (ViewData["CommonDataSysTitle"] != null)
                {
                    g = ViewData["CommonDataSysTitle"].ToString();
                }
                return g;
            }
        }

        protected string EmployeePhotoUrl
        {
            get
            {
                string g = string.Empty;
                if (ViewData["CommonDataEmployeePhotoUrl"] != null)
                {
                    g = ViewData["CommonDataEmployeePhotoUrl"].ToString();
                }
                return g;
            }
        }

        protected string CompanyLogoImgUrl
        {
            get
            {
                string g = string.Empty;
                if (ViewData["CommonDataCompanyLogoImgUrl"] != null)
                {
                    g = ViewData["CommonDataCompanyLogoImgUrl"].ToString();
                }
                return g;
            }
        }

        /// <summary>
        /// 根据当前分辨率设置table行数
        /// </summary>
        protected int? PageSizeScreen
        {
            get
            {
                if (Session != null && Session["pageSize"] != null)
                {
                    int pageSize;
                    int.TryParse(Session["pageSize"].ToString(), out pageSize);
                    return pageSize;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 分页显示数据个数
        /// </summary>
        protected const int pageSizes = 10;

        /// <summary>
        /// 默认显示分页页数
        /// </summary>
		protected const int pageIndexs = 1;

        /// <summary>
        /// 获取分页对象
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
		protected PagingInfo GetPaging(int pageIndex, int pageSize)
        {
            return new PagingInfo
            {
                TotalItems = 0,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
        }

        /// <summary>
        /// 获取分页对象
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="Rcount"></param>
        /// <returns></returns>
		protected PagingInfo GetPaging(int pageIndex, int pageSize, ref int Rcount)
        {
            return new PagingInfo
            {
                TotalItems = Rcount,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
        }

        private static int maxFileSize = int.Parse(ConfigurationManager.AppSettings["MaxFileSize"] ?? "4");
        private static int maxImageFileSize = int.Parse(ConfigurationManager.AppSettings["MaxImageFileSize"] ?? "2");

        /// <summary>
        /// 最大文件上传大小（单位：b）
        /// </summary>
        protected int MaxFileSize_B
        {
            get
            {
                return 1024 * 1024 * maxFileSize;
            }
        }

        /// <summary>
        /// 最大文件上传大小（单位：M）
        /// </summary>
        protected int MaxFileSize_M
        {
            get
            {
                return maxFileSize;
            }
        }

        /// <summary>
        /// 最大图片上传大小（单位：b）
        /// </summary>
        protected int MaxImageFileSize_B
        {
            get { return 1024 * 1024 * maxImageFileSize; }
        }

        /// <summary>
        /// 最大图片上传大小（单位：M）
        /// </summary>
        protected int MaxImageFileSize_M
        {
            get { return maxImageFileSize; }
        }

        /// <summary>
        /// 获取按钮权限
        /// </summary>
        /// <param name="_SID">模块id</param>
		protected MyPowerModel NewMethod(string _SID)
		{
            MyPowerModel myp = new MyPowerModel();
            myp.Sid = _SID;
      //      List < SysPower > sysPowers = new List<SysPower>();
		    //if (Session["lookMode"] != null) //档案管理采用查看模式，除查看外权限屏蔽
		    //{
		    //    myp.Look = true;
      //          return myp;
		    //}
      //      if (System.Web.HttpContext.Current.Session["RoleId"] != null)
      //      {
      //          sysPowers = _roleDomainSvc.GetPower(new Guid(System.Web.HttpContext.Current.Session["RoleId"].ToString()));
      //      }
		    //sysPowers = sysPowers.FindAll(x => String.Equals(x.SysTitleId.GetValueOrDefault().ToString(), _SID, StringComparison.CurrentCultureIgnoreCase));
		    //foreach (var item in sysPowers)
		    //{
		    //    var powers = item.FunStr;
		    //    if (!string.IsNullOrEmpty(powers))
		    //    {
		    //        string[] pwlst = powers.TrimEnd(';').Split(';');
      //              if (pwlst.Length<=0)
      //              {
      //                  continue;
      //              }
		    //        foreach (var items in pwlst)
		    //        {
		    //            if (items != string.Empty)
		    //            {
		    //                if (items.Contains("pw1_"))
		    //                {
		    //                    myp.Add = true;
		    //                }
		    //                if (items.Contains("pw2_"))
		    //                {
		    //                    myp.Update = true;
		    //                }
		    //                if (items.Contains("pw3_"))
		    //                {
		    //                    myp.Delete = true;
		    //                }
		    //                if (items.Contains("pw4_"))
		    //                {
		    //                    myp.Look = true;
		    //                }
		    //                if (items.Contains("pw5_"))
		    //                {
		    //                    myp.Export = true;
		    //                }
		    //                if (items.Contains("pw6_"))
		    //                {
		    //                    myp.StartStop = true;
		    //                }
		    //                if (items.Contains("pw7_"))
		    //                {
		    //                    myp.ReSetPassWord = true;
		    //                }
		    //                if (items.Contains("pw8_"))
		    //                {
		    //                    myp.PowerAllot = true;
		    //                }
		    //                if (items.Contains("pw9_"))
		    //                {
		    //                    myp.TJLook = true;
		    //                }
		    //                if (items.Contains("pw10_"))
		    //                {
		    //                    myp.HealthLook = true;
		    //                }
		    //                if (items.Contains("pw11_"))
		    //                {
		    //                    myp.Diagnose = true;
		    //                }
		    //                if (items.Contains("pw12_"))
		    //                {
		    //                    myp.Handing = true;
		    //                }
		    //                if (items.Contains("pw13_"))
		    //                {
		    //                    myp.Unbind = true;
		    //                }
		    //                if (items.Contains("pw14_"))
		    //                {
		    //                    myp.JWInfo = true;
		    //                }
		    //                if (items.Contains("pw15_"))
		    //                {
		    //                    myp.Intervention = true;
		    //                }
		    //                if (items.Contains("pw16_"))
		    //                {
		    //                    myp.PostTJInfo = true;
		    //                }
		    //                if (items.Contains("pw17_"))
		    //                {
		    //                    myp.Print = true;
		    //                }
		    //                if (items.Contains("pw18_"))
		    //                {
		    //                    myp.Back = true;
		    //                }
		    //                if (items.Contains("pw19_"))
		    //                {
		    //                    myp.Logout = true;
		    //                }
		    //                if (items.Contains("pw20_"))
		    //                {
		    //                    myp.OutAlready = true;
		    //                }
		    //                if (items.Contains("pw21_"))
		    //                {
		    //                    myp.SetShiftWorker = true;
		    //                }
		    //                if (items.Contains("pw22_"))
		    //                {
		    //                    myp.SetNursingType = true;
		    //                }
		    //                if (items.Contains("pw23_"))
		    //                {
		    //                    myp.SetRoleCompanyPower = true;
		    //                }
		    //                if (items.Contains("pw24_"))
		    //                {
		    //                    myp.TransferredToLive = true;
		    //                }
		    //                if (items.Contains("pw25_"))
		    //                {
		    //                    myp.CheckInAssessment = true;
		    //                }
		    //                if (items.Contains("pw26_"))
		    //                {
		    //                    myp.CheckInInformation = true;
		    //                }
		    //                if (items.Contains("pw27_"))
		    //                {
		    //                    myp.Active = true;
		    //                }
		    //                if (items.Contains("pw28_"))
		    //                {
		    //                    myp.ReportOfLoss = true;
		    //                }
		    //                if (items.Contains("pw29_"))
		    //                {
		    //                    myp.ChangeCard = true;
		    //                }
		    //                if (items.Contains("pw30_"))
		    //                {
		    //                    myp.EjectCard = true;
		    //                }
		    //                if (items.Contains("pw31_"))
		    //                {
		    //                    myp.AddTheLowerSection = true;
		    //                }
		    //                if (items.Contains("pw32_"))
		    //                {
		    //                    myp.StopService = true;
		    //                }
		    //                if (items.Contains("pw33_"))
		    //                {
		    //                    myp.ElderlyRecord = true;
		    //                }
		    //                if (items.Contains("pw34_"))
		    //                {
		    //                    myp.DoctorList = true;
		    //                }
		    //                if (items.Contains("pw35_"))
		    //                {
		    //                    myp.EntrustedDrug = true;
		    //                }
		    //                if (items.Contains("pw36_"))
		    //                {
		    //                    myp.SignRecord = true;
		    //                }
		    //                if (items.Contains("pw37_"))
		    //                {
		    //                    myp.AdmissionRegistration = true;
		    //                }
		    //                if (items.Contains("pw38_"))
		    //                {
		    //                    myp.RecordEdit = true;
		    //                }
		    //                if (items.Contains("pw39_"))
		    //                {
		    //                    myp.AddDoctor = true;
		    //                }
		    //                if (items.Contains("pw40_"))
		    //                {
		    //                    myp.AddFixedCostDesc = true;
		    //                }
		    //                if (items.Contains("pw41_"))
		    //                {
		    //                    myp.OtherMonthIsSo = true;
		    //                }
		    //                if (items.Contains("pw42_"))
		    //                {
		    //                    myp.EntrustGrantFill = true;
		    //                }
		    //                if (items.Contains("pw43_"))
		    //                {
		    //                    myp.CheckEntrustGrant = true;
		    //                }
		    //                if (items.Contains("pw44_"))
		    //                {
		    //                    myp.CheckEntrustGrantRecord = true;
		    //                }
		    //                if (items.Contains("pw45_"))
		    //                {
		    //                    myp.BackUp = true;
		    //                }
      //                      if (items.Contains("pw46_"))
      //                      {
      //                          myp.Execute = true;
      //                      }
      //                      if (items.Contains("pw47_"))
      //                      {
      //                          myp.Payment = true;
      //                      }
      //                      if (items.Contains("pw48_"))
      //                      {
      //                          myp.Settlement = true;
      //                      }
      //                      if (items.Contains("pw49_"))
      //                      {
      //                          myp.Recharge = true;
      //                      }
      //                      if (items.Contains("pw50_"))
      //                      {
      //                          myp.Reclaim = true;
      //                      }
      //                      if (items.Contains("pw51_"))
      //                      {
      //                          myp.Maintain = true;
      //                      }
						//	if(items.Contains("pw52_"))
						//	{
						//		myp.AppPowerAllot = true;
						//	}
      //                      if (items.Contains("pw53_"))
      //                      {
      //                          myp.Run = true;
      //                      }
      //                      if (items.Contains("pw54_"))
      //                      {
      //                          myp.Stop = true;
      //                      }
						//	if(items.Contains("pw55_"))
						//	{
						//		myp.ModuleMTAdd = true;
						//	}
						//	if(items.Contains("pw56_"))
						//	{
						//		myp.ModuleMTEdit = true;
						//	}
						//	if (items.Contains("pw57_"))
						//	{
						//		myp.ModuleMTDelete = true;
						//	}
						//	if (items.Contains("pw58_"))
						//	{
						//		myp.ModuleMCAdd = true;
						//	}
						//	if (items.Contains("pw59_"))
						//	{
						//		myp.ModuleMCEdit = true;
						//	}
						//	if (items.Contains("pw60_"))
						//	{
						//		myp.ModuleMCDelete = true;
						//	}
						//	if (items.Contains("pw61_"))
						//	{
						//		myp.ModuleMCLook = true;
						//	}
      //                      if (items.Contains("pw62_"))
      //                      {
      //                          myp.QuartzLookLog = true;
      //                      }
      //                      if (items.Contains("pw63_"))
      //                      {
      //                          myp.QuartzRunOnce = true;
      //                      }
      //                      if (items.Contains("pw64_"))
      //                      {
      //                          myp.NextShift = true;
      //                      }
      //                      if (items.Contains("pw65_"))
      //                      {
      //                          myp.Record = true;
      //                      }
      //                      if (items.Contains("pw66_"))
      //                      {
      //                          myp.Audit = true;
      //                      }
      //                      if (items.Contains("pw67_"))
      //                      {
      //                          myp.Submit = true;
      //                      }
      //                      if (items.Contains("pw68_"))
      //                      {
      //                          myp.Confirm = true;
      //                      }
      //                      if (items.Contains("pw69_"))
      //                      {
      //                          myp.Upload = true;
      //                      }
      //                      if (items.Contains("pw70_"))
      //                      {
      //                          myp.Grant = true;
      //                      }
      //                      if (items.Contains("pw71_"))
      //                      {
      //                          myp.BaoF = true;
      //                      }
		    //                if (items.Contains("pw72_"))
		    //                {
		    //                    myp.Elder = true;
		    //                }
		    //                if (items.Contains("pw73_"))
		    //                {
		    //                    myp.Live = true;
		    //                }
		    //                if (items.Contains("pw74_"))
		    //                {
		    //                    myp.Assessment = true;
		    //                }
		    //                if (items.Contains("pw75_"))
		    //                {
		    //                    myp.Medical = true;
		    //                }
		    //                if (items.Contains("pw76_"))
		    //                {
		    //                    myp.Finance = true;
		    //                }
		    //                if (items.Contains("pw77_"))
		    //                {
		    //                    myp.Service = true;
		    //                }
		    //                if (items.Contains("pw78_"))
		    //                {
		    //                    myp.EntrustImg = true;
		    //                }
		    //                if (items.Contains("pw79_"))
		    //                {
		    //                    myp.EntrustGrantHandle = true;
		    //                }
		    //                if (items.Contains("pw80_"))
		    //                {
		    //                    myp.OutRecord = true;
		    //                }
		    //                if (items.Contains("pw81_"))
		    //                {
		    //                    myp.TakeMedical = true;
		    //                }
		    //                if (items.Contains("pw82_"))
		    //                {
		    //                    myp.Infusion = true;
		    //                }
		    //                if (items.Contains("pw83_"))
		    //                {
		    //                    myp.Cure = true;
		    //                }
		    //                if (items.Contains("pw84_"))
		    //                {
		    //                    myp.SubsidiesMaterials = true;
		    //                }
		    //                if (items.Contains("pw85_"))
		    //                {
		    //                    myp.NoExecute = true;
		    //                }
      //                      if (items.Contains("pw86_"))
      //                      {
      //                          myp.DownLoad = true;
      //                      }
      //                      if (items.Contains("pw87_"))
      //                      {
      //                          myp.Import = true;
      //                      }
		    //                if (items.Contains("pw88_"))
		    //                {
		    //                    myp.Examine = true;
		    //                }
      //                      if (items.Contains("pw89_"))
      //                      {
      //                          myp.ElderIndex = true;
      //                      }
      //                      if (items.Contains("pw90_"))
      //                      {
      //                          myp.Subsidy = true;
      //                      }
      //                      if (items.Contains("pw91_"))
      //                      {
      //                          myp.InitialFee = true;
      //                      }
      //                      if (items.Contains("pw92_"))
      //                      {
      //                          myp.PrePayment = true;
      //                      }
      //                      if (items.Contains("pw93_"))
      //                      {
      //                          myp.MonthCost = true;
      //                      }
      //                      if (items.Contains("pw94_"))
      //                      {
      //                          myp.OwingCost = true;
      //                      }
      //                      if (items.Contains("pw95_"))
      //                      {
      //                          myp.RetreatCost = true;
      //                      }
      //                      if (items.Contains("pw96_"))
      //                      {
      //                          myp.Review = true;
      //                      }
      //                      if (items.Contains("pw97_"))
      //                      {
      //                          myp.Assess = true;
      //                      }
      //                      if (items.Contains("pw98_"))
      //                      {
      //                          myp.PaymentNotice = true;
      //                      }
      //                      if (items.Contains("pw99_"))
      //                      {
      //                          myp.AgreementSign = true;
      //                      }
		    //                if (items.Contains("pw100_"))
		    //                {
		    //                    myp.YPEntrustGrand = true;
		    //                }
		    //                if (items.Contains("pw101_"))
		    //                {
		    //                    myp.YPEntrustDrugStock = true;
		    //                }
		    //                if (items.Contains("pw102_"))
		    //                {
		    //                    myp.YPEntrustAdd = true;
		    //                }
		    //                if (items.Contains("pw103_"))
		    //                {
		    //                    myp.YPEntrustStartStop = true;
		    //                }
		    //                if (items.Contains("pw104_"))
		    //                {
		    //                    myp.YPEntrustEdit = true;
		    //                }
		    //                if (items.Contains("pw105_"))
		    //                {
		    //                    myp.YPEntrustDrugStockAdd = true;
		    //                }
      //                      if(items.Contains("pw106_"))
      //                      {
      //                          myp.AdditionRecord = true;
      //                      }
      //                      if(items.Contains("pw107_"))
      //                      {
      //                          myp.AdditionRecordAdd = true;
      //                      }
      //                      if(items.Contains("pw108_"))
      //                      {
      //                          myp.AdditionRecordEdit = true;
      //                      }
      //                      if(items.Contains("pw109_"))
      //                      {
      //                          myp.AdditionRecordDelete = true;
      //                      }
      //                      if(items.Contains("pw110_"))
      //                      {
      //                          myp.AdditionRecordLook = true;
      //                      }
		    //                if (items.Contains("pw111_"))
		    //                {
		    //                    myp.LookReply = true;
		    //                }
		    //                if (items.Contains("pw112_"))
		    //                {
		    //                    myp.Release = true;
		    //                }
		    //                if (items.Contains("pw113_"))
		    //                {
		    //                    myp.Reply = true;
		    //                }
      //                  }
		    //        }
		    //        break;
		    //    }
		    //}
		    return myp;
		}


        #region 操作动作
        protected enum HandleAction
		{
			/// <summary>
			/// 新增
			/// </summary>
			Add = 0,

			/// <summary>
			/// 编辑
			/// </summary>
			Update = 1,

			/// <summary>
			/// 删除
			/// </summary>
			Delete = 2,

            /// <summary>
            /// 登录
            /// </summary>
            Login = 3
        }
		#endregion

        /// <summary>
        /// 获取客户端IP地址
        /// </summary>
        /// <returns>若失败则返回回送地址</returns>
        protected string GetWebClientIp()
        {
            //如果客户端使用了代理服务器，则利用HTTP_X_FORWARDED_FOR找到客户端IP地址
            string userHostAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"]?.Split(',')[0].Trim() ?? "";
            //否则直接读取REMOTE_ADDR获取客户端IP地址
            if (string.IsNullOrEmpty(userHostAddress))
            {
                userHostAddress = Request.ServerVariables["REMOTE_ADDR"];
            }
            //前两者均失败，则利用Request.UserHostAddress属性获取IP地址，但此时无法确定该IP是客户端IP还是代理IP
            if (string.IsNullOrEmpty(userHostAddress))
            {
                userHostAddress = Request.UserHostAddress;
            }
            //最后判断获取是否成功，并检查IP地址的格式（检查其格式非常重要）
            if (!string.IsNullOrEmpty(userHostAddress) && IPHelper.IsIp(userHostAddress))
            {
                return userHostAddress;
            }
            return "127.0.0.1";
        }

        //private SysHandleLogDomainSvc SysHandleLogDomainSvc { get; } = new SysHandleLogDomainSvc(new SysHandleLogSvc(new ModelRespositoryFactory<SysHandleLog, Guid>()));
        /// <summary>
        /// 操作日志记录
        /// </summary>
        /// <param name="sysTitleId">模块Id</param>
        /// <param name="handleAction">操作动作</param>
        /// <param name="handleDataId">操作数据主键Id</param>
        /// <param name="handleLogDesc">操作备注</param>
        protected void LogSysOperation(Guid? sysTitleId, HandleAction handleAction, Guid? handleDataId,string handleLogDesc)
        {

            SysHandleLogAddorUpdateDto model = new SysHandleLogAddorUpdateDto()
            {
                SysHandleLogId = Guid.NewGuid(),
                SysEmployeeId = UserId,
                SysTitleId = sysTitleId,
                HandleTime = DateTime.Now,
                HandleAction = (int)handleAction,
                HandleDataId = handleDataId,
                HandleLogIP = GetWebClientIp(),
                HandleActionCID = CID,
                HandleLogDesc = handleLogDesc,

            };
            SysHandleLogSvc_Other.AddSysHandleLog(model);
            //SysHandleLogDomainSvc.AddSysHandleLog(model);
        }

        /// <summary>
        /// 根据分辨率大小获取
        /// </summary>
        /// <param name="pageSize"></param>
	    protected void GetPageSizeByScreen(ref int pageSize,int s=0)
	    {
	        if (PageSizeScreen.HasValue)
	        {
	            pageSize = PageSizeScreen.Value+s;
	        }
	    }

        #region 文件上传
        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="groupName">文件分类名称</param>
        /// <returns></returns>
        protected ActionResult UploadFiles(string groupName)
	    {
	        var files = System.Web.HttpContext.Current.Request.Files;
	        var error = string.Empty;
	        if (files.Count <= 0) return Content("#");
	        var fileName = Path.GetFileName(files[0].FileName);

	        var extension = Path.GetExtension(fileName);//扩展名
	        if (string.IsNullOrEmpty(GetIcoTypePath(fileName)))
	        {
	            return Content("ERROR^不支持该类型文件!");
	        }
	        var iContentLength = files[0].ContentLength / 1024 / 1024;
	        if (iContentLength > 10)
	        {
	            return Content("ERROR^文件超过限定大小(10M)!");
	        }
	        var fileNameWithoutExtension = Guid.NewGuid().ToString();
	        var sfileName = fileNameWithoutExtension + extension;
	        var filesPath = $"/Files/{groupName}/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";
	        if (!Directory.Exists(filesPath))
	        {
	            Directory.CreateDirectory(Server.MapPath("/") + filesPath);
	        }
	        var path = filesPath + sfileName;
	        files[0].SaveAs(Server.MapPath("/") + path);
	        var msg = " 成功! 文件大小为:" + files[0].ContentLength;
	        var res = "{ error:'" + error + "', msg:'" + msg + "',imgurl:'" + path + "'}";
	        ViewData["phoneUrl"] = fileName + "|" + path;

	        return Content(fileName + "^" + path);
        }

        /// <summary>
        /// 获取文件图标路径
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GetIcoTypePath(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                var iExtension = Path.GetExtension(fileName)?.ToLower();

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

        #endregion

        ///// <summary>
        ///// 未找到action时直接跳到以该action名称的视图
        ///// </summary>
        ///// <param name="actionName"></param>
        //protected override void HandleUnknownAction(string actionName)
        //{
        //    try
        //    {
        //        this.View(actionName).ExecuteResult(this.ControllerContext);
        //    }
        //    catch
        //    {
        //        ViewBag.ErroMessage = "你输入的页面不存在";
        //        this.View("/Web404.aspx").ExecuteResult(this.ControllerContext);
        //    }
        //}

    }
}