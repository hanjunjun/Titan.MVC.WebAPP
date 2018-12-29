/************************************************************************
* 文件名：
* 文件功能描述：登录控制器
* 作    者：  韩俊俊
* 创建日期：
* 修 改 人：
* 修改日期：
* 修改原因：
* 备注：直接的return View 不会更改url，Redirect可以重定向url
* Copyright (c) 2018 泰坦. All Rights Reserved.
* ***********************************************************************/

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Titan.AppService.DomainService;
using Titan.AppService.ModelDTO;
using Titan.Controllers.ViewModel;
using Titan.Infrastructure.Communicate;
using Titan.Infrastructure.Domain;
using Titan.Infrastructure.Utility;
using Titan.Infrastructure.Verify;
using Titan.Model;
using Titan.Model.DataModel;

namespace Titan.Controllers.Controllers
{
    public class LoginController : ControllerBase
    {
        #region 成员及构造

        private readonly LoginDomainSvc _loginDomainSvc;
        private readonly SysHandleLogDomainSvc _sysHandleLogDomainSvc;
        public LoginController(LoginDomainSvc loginDomainSvc, SysHandleLogDomainSvc sysHandleLogDomainSvc)
        {
            _loginDomainSvc = loginDomainSvc;
            _sysHandleLogDomainSvc = sysHandleLogDomainSvc;
        }
        #endregion

        #region 登录页面
        /// <summary>
        /// 登录页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [OutputCache(Duration = 0)]
        public ActionResult LoginView()
        {
            LoginView model=new LoginView();
            return View("LoginView",model);
        }
        #endregion

        #region 登录方法、登录失败返回页面、登录成功返回页面
        /// <summary>
        /// 登录方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [OutputCache(Duration = 0)]
        public ActionResult Login(LoginView model)
        {
            return Redirect("~/Login/IndexView");
        }

        /// <summary>
        /// 验证用户名是否存在
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult ValidateUserId(string userId)
        {
            var sysEmployye = _loginDomainSvc.GetSysEmployeeByUserId(userId);
            if (sysEmployye == null)
            {
                //用户不存在
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 验证用户名和密码和验证码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="passWord"></param>
        /// <param name="verifyCode"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ValidateUserIdPassWrod(string userId,string passWord,string verifyCode)
        {
            try
            {
                var sysEmployye = _loginDomainSvc.GetSysEmployeeByUserIdAndUserPwd(userId, Md5Helper.Md5Hex(passWord));
                if (sysEmployye == null || Session["ValidateCode"].ToString() != Md5Helper.Md5Hex(verifyCode))
                {
                    //密码或者验证码不正确
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
                //cookie
                //用户描述用户基本信息
                UserInfo userInfo = new UserInfo
                {
                    UserId = sysEmployye.SysEmployeeId,
                    UserName = sysEmployye.EmployeeName,
                    CompanyName = sysEmployye.SysCompany.SysCompanyName,
                    CID = sysEmployye.SysCompanyId,
                    UserCID = sysEmployye.SysCompanyId,
                    DeptId = sysEmployye.SysPost.SysDeptId,
                    PostId = sysEmployye.SysPostId,
                    LoginNo = userId,
                    CompanyCode = sysEmployye.SysCompany.CompanyCode,
                    SysTitle = sysEmployye.SysCompany.CompanyTitle,
                    EmployeePhotoUrl = sysEmployye.EmployeePhotoUrl,
                    CompanyLogoImgUrl = sysEmployye.SysCompany.CompanyLogoImgUrl
                };
                //生成初始化凭据
                FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                    1,
                    userId,
                    DateTime.Now,
                    DateTime.Now.AddDays(7),
                    true,
                    JsonHelper.ToJsonStr(userInfo)
                );
                //加密
                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

                //响应到客户端
                System.Web.HttpCookie authCookie = new System.Web.HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                System.Web.HttpContext.Current.Response.Cookies.Add(authCookie);
                //登录日志
                LogSysOperation(Guid.Parse("C364BCDE-35B7-4987-8860-C7568645995A"), HandleAction.Add, Guid.NewGuid(), "仓储层测试");
                LogSysOperation(userInfo, $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} {userId}登录");
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 登录成功返回页面
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        [HttpGet]
        [OutputCache(Duration = 0)]
        public ActionResult IndexView()
        {
            //模块列表
            ViewBag.LeftTitleModel = GetSysTitleList();

            //机构列表
            ViewBag.CompanyModel = GetSysCompanyList();
            
            return View("IndexView", "_Layout");
        }
        #endregion

        #region 获取权限模块列表、所属机构以及下级机构列表

        private List<SysTitleViewList> GetSysTitleList()
        {
            var list = new List<SysTitleViewList>();
            var mainTitleList = _loginDomainSvc.GetSysTitleByMain();
            foreach (var item in mainTitleList)
            {
                var iList = new SysTitleViewList()
                {
                    SysTitleId = item.SysTitleId,
                    TitleName = item.TitleName,
                    TitleUrl = item.TitleUrl,
                    TitleFatherId = item.TitleFatherId,
                    TitleImgUrl = item.TitleImgUrl,
                    SysTitleViewLists = AddSysTitleViewList(item.SysTitleId),
                };
                list.Add(iList);
            }

            return list;
        }

        public List<SysTitleViewList> AddSysTitleViewList(Guid id)
        {
            var listDto = _loginDomainSvc.GetSysTitleByFatherId(id);
            return listDto.Select(item => new SysTitleViewList
            {
                SysTitleId = item.SysTitleId,
                TitleName = item.TitleName,
                TitleUrl = item.TitleUrl,
                TitleFatherId = item.TitleFatherId,
                TitleImgUrl = item.TitleImgUrl,
                SysTitleViewLists = AddSysTitleViewList(item.SysTitleId),
            }).ToList();
        }

        private List<SysCompany> GetSysCompanyList()
        {
            return _loginDomainSvc.GetSysCompanyList();
        }
        #endregion

        #region 修改密码页面、密码验证、修改密码保存
        /// <summary>
        /// 修改密码页面
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        [HttpGet]
        [OutputCache(Duration = 0)]
        public ActionResult ChangePwdView()
        {
            ChangePwdView model = new ChangePwdView();
            return View(model);
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="employeePwd">旧密码</param>
        /// <param name="employeeNewPwd">新密码</param>
        /// <param name="employeeNewPwdSecond">确认密码</param>
        /// <returns></returns>
        [LoginFilter]
        [HttpPost]
        [OutputCache(Duration = 0)]
        public string ChangePwd(string employeePwd,string employeeNewPwd,string employeeNewPwdSecond)
        {
            try
            {
                var sysemployee=_loginDomainSvc.GetSysEmployeeByUserIdAndUserPwd(LoginNo, Md5Helper.Md5Hex(employeePwd));
                sysemployee.EmployeePwd = Md5Helper.Md5Hex(employeeNewPwdSecond);
                _loginDomainSvc.UpdateSysEmployeeDto(sysemployee);
                FormsAuthentication.SignOut();
                return "success";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        /// <summary>
        /// 验证当前登录用户密码是否正确
        /// </summary>
        /// <param name="employeePwd"></param>
        /// <returns></returns>
        [LoginFilter]
        [HttpGet]
        public string ValidatePwd(string employeePwd)
        {
            try
            {
                var sysemployee = _loginDomainSvc.GetSysEmployeeByUserIdAndUserPwd(LoginNo, Md5Helper.Md5Hex(employeePwd));
                if (sysemployee!=null)
                    return "1";
                return "0";
            }
            catch(Exception ex)
            {
                return "0";
            }
            
        }
        #endregion

        #region 生成验证码、
        public ActionResult GetValidateCode()
        {
            VerifyCodeHelper vCode = new VerifyCodeHelper();
            var code = vCode.GetRandomString(4);
            var img = vCode.CreateImage(code);
            Session["ValidateCode"] = Md5Helper.Md5Hex(code);
            return File(img, @"image/jpeg");
        }
        #endregion

        #region 登出方法
        [HttpGet]
        [OutputCache(Duration = 0)]
        public ActionResult LogOut()
        {
            //清除cookie
            FormsAuthentication.SignOut();
            return Redirect("~/Login/LoginView");
        }
        #endregion

        #region 添加系统登录日志、获取客户端IP
        /// <summary>
        /// 添加系统登录日志
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="handleLogDesc"></param>
        private void LogSysOperation(UserInfo userInfo, string handleLogDesc)
        {
            SysHandleLogAddorUpdateDto model = new SysHandleLogAddorUpdateDto()
            {
                SysHandleLogId = Guid.NewGuid(),
                SysEmployeeId = userInfo.UserId,
                SysTitleId = null,
                HandleTime = DateTime.Now,
                HandleAction = 3,
                HandleDataId = null,
                HandleLogIP = GetWebClientIp(),
                HandleActionCID = userInfo.CID,
                HandleLogDesc = handleLogDesc,
                Isdelete = false
            };
            _sysHandleLogDomainSvc.AddSysHandleLog(model);
        }

        /// <summary>
        /// 获取客户端IP地址
        /// </summary>
        /// <returns>若失败则返回回送地址</returns>
        private new string GetWebClientIp()
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
        #endregion

        #region 切换机构

        [LoginFilter]
        [HttpGet]
        [OutputCache(Duration = 0)]
        public ActionResult ChangeSysCompany(Guid sysCompanyId)
        {
            //修改cookie
            var sysEmployye = _loginDomainSvc.GetSysEmployeeByUserId(LoginNo);//用户信息
            var companyInfo = _loginDomainSvc.GetSysCompany(sysCompanyId);//机构信息
            //用户描述用户基本信息
            UserInfo userInfo = new UserInfo
            {
                UserId = sysEmployye.SysEmployeeId,
                UserName = sysEmployye.EmployeeName,
                CompanyName = sysEmployye.SysCompany.SysCompanyName,
                CID = sysCompanyId,//当前机构
                UserCID = sysEmployye.SysCompanyId,
                DeptId = sysEmployye.SysPost.SysDeptId,
                PostId = sysEmployye.SysPostId,
                LoginNo = LoginNo,
                CompanyCode = sysEmployye.SysCompany.CompanyCode,
                SysTitle = companyInfo.CompanyTitle,
                EmployeePhotoUrl = sysEmployye.EmployeePhotoUrl,
                CompanyLogoImgUrl = companyInfo.CompanyLogoImgUrl //机构图标
            };
            //生成初始化凭据
            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                1,
                LoginNo,
                DateTime.Now,
                DateTime.Now.AddDays(7),
                true,
                JsonHelper.ToJsonStr(userInfo)
            );
            //加密
            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

            //响应到客户端
            System.Web.HttpCookie authCookie = new System.Web.HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            System.Web.HttpContext.Current.Response.Cookies.Add(authCookie);

            
            return Json(companyInfo, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region APP接口

        public LoginDto LoginByApp(string userId,string passWord,string userName)
        {
            var loginInfo = new LoginDto()
            {
                UserId = userId,
                UserName = userName,
                UserPassWord = passWord
            };
            return loginInfo;
        }
        #endregion
    }
}
