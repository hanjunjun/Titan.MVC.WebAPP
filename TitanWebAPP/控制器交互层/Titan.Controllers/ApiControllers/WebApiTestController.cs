/************************************************************************
 * 文件名：WebApiTestController
 * 文件功能描述：xx控制层
 * 作    者：  韩俊俊
 * 创建日期：2018/11/26 13:51:45
 * 修 改 人：
 * 修改日期：
 * 修改原因：
 * Copyright (c) 2017 Titan.Han . All Rights Reserved. 
 * ***********************************************************************/
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Titan.AppService.DomainService;
using Titan.AppService.ModelDTO;
using Titan.AppService.ModelService;
using Titan.Controllers.Attributes;
using Titan.Controllers.ViewModel;
using Titan.Infrastructure.Communicate;
using Titan.Infrastructure.Utility;
using Titan.Infrastructure.Verify;
using Titan.Model.DataModel;
using Titan.Model.JWTModel;

namespace Titan.Controllers.ApiControllers
{
    public class WebApiTestController : ApiController
    {
        #region 构造函数
        private readonly SysHandleLogDomainSvc _sysHandleLogDomainSvc;
        private readonly LoginDomainSvc _loginDomainSvc;

        public WebApiTestController(SysHandleLogDomainSvc sysHandleLogDomainSvc, LoginDomainSvc loginDomainSvc)
        {
            _sysHandleLogDomainSvc = sysHandleLogDomainSvc;
            _loginDomainSvc = loginDomainSvc;
        }
        #endregion

        #region 登录获取token、获取登录验证码
        [Route("api/user/login")]
        [HttpPost]
        public LoginResult Login([FromBody]LoginRequest request)
        {
            var result = new LoginResult();
            try
            {
                var sysEmployye = _loginDomainSvc.GetSysEmployeeByUserIdAndUserPwd(request.UserName, Md5Helper.Md5Hex(request.Password));
                if (sysEmployye == null)
                {
                    //密码或者验证码不正确
                    result.Message = "帐号或密码不正确！";
                    result.Success = false;
                    return result;
                }
                const string secret = "The code to create the world!";
                //payload
                Payload info = new Payload
                {
                    GivenName = sysEmployye.EmployeeName,
                    Role = new List<string> { "Admin", "Manage" },
                    exp = UtilityHelp.GetTimeStamp() + 30//这个时间是utc时间要将本地时间转换成utc时间
                };
                //secret需要加密
                IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
                IJsonSerializer serializer = new JsonNetSerializer();
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
                var token = encoder.Encode(info, secret);
                //登录日志
                LogSysOperation(sysEmployye, $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} {sysEmployye.EmployeeName}登录");
                result.Message = "授权token获取成功！";
                result.Token = token;
                result.Success = true;
                return result;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Success = false;
                return result;
            }
        }

        [HttpGet]
        [Route("api/user/VerifyCode")]
        public void GetValidateCode()
        {
            VerifyCodeHelper vCode = new VerifyCodeHelper();
            var code = vCode.GetRandomString(4);
            var img = vCode.CreateImage(code);
            //HttpContext.Current.Session.SessionID;
            HttpContext.Current.Session["VerifyCode"] = code; //验证码存放在TempData中
            HttpContext.Current.Response.BinaryWrite(img);
            HttpContext.Current.Response.End();
        }
        #endregion

        #region token请求测试
        // GET: User
        [ApiAuthorize]
        [Route("api/user/get")]
        [HttpGet]
        public string Get()
        {
            Payload info = RequestContext.RouteData.Values["auth"] as Payload;
            if (info == null)
            {
                return "获取不到，失败";
            }
            else
            {
                return $"获取到了，Auth的Name是 {info.GivenName}";
            }
        }
         
        [ApiAuthorize]
        [Route("api/user/post")]
        [HttpPost]
        public string Post([FromBody]LoginRequest data)
        {
            Payload info = RequestContext.RouteData.Values["auth"] as Payload;
            if (info == null)
            {
                return "获取不到，失败";
            }
            else
            {
                return $"获取到了，Auth的Name是 {info.GivenName}";
            }
        }

        [ApiAuthorize]
        [Route("api/user/axiosget")]
        [HttpGet]
        public string Axiosget()
        {
            Payload info = RequestContext.RouteData.Values["auth"] as Payload;
            if (info == null)
            {
                return "获取不到，失败";
            }
            else
            {
                return $"获取到了，Auth的Name是 {info.GivenName}";
            }
        }

        [Route("api/user/axiospost")]
        [HttpPost]
        public LoginResult Axiospost([FromBody]LoginRequest request)
        {
            var result = new LoginResult();
            try
            {
                var sysEmployye = _loginDomainSvc.GetSysEmployeeByUserIdAndUserPwd(request.UserName, Md5Helper.Md5Hex(request.Password));
                if (sysEmployye == null)
                {
                    //密码或者验证码不正确
                    result.Message = "帐号或密码不正确！";
                    result.Success = false;
                    return result;
                }
                const string secret = "The code to create the world!";
                //payload
                Payload info = new Payload
                {
                    GivenName = sysEmployye.EmployeeName,
                    Role = new List<string> { "Admin", "Manage" },
                    exp = UtilityHelp.GetTimeStamp() + 30//这个时间是utc时间要将本地时间转换成utc时间
                };
                //secret需要加密
                IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
                IJsonSerializer serializer = new JsonNetSerializer();
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
                var token = encoder.Encode(info, secret);
                //登录日志
                LogSysOperation(sysEmployye, $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} {sysEmployye.EmployeeName}登录");
                result.Message = "授权token获取成功！";
                result.Token = token;
                result.Success = true;
                return result;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Success = false;
                return result;
            }
        }
        #endregion

        #region 写日志
        /// <summary>
        /// 添加系统登录日志
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="handleLogDesc"></param>
        private void LogSysOperation(SysEmployee userInfo, string handleLogDesc)
        {
            SysHandleLogAddorUpdateDto model = new SysHandleLogAddorUpdateDto()
            {
                SysHandleLogId = Guid.NewGuid(),
                SysEmployeeId = userInfo.SysEmployeeId,
                SysTitleId = null,
                HandleTime = DateTime.Now,
                HandleAction = 3,
                HandleDataId = null,
                HandleLogIP = CheckIP.GetNetIP(),
                HandleActionCID = userInfo.SysCompanyId,
                HandleLogDesc = handleLogDesc,
                Isdelete = false
            };
            _sysHandleLogDomainSvc.AddSysHandleLog(model);
        }

        #endregion
    }
}
