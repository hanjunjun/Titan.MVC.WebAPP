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
using Titan.Infrastructure.Domain;
using Titan.Model;

namespace Titan.Controllers.Controllers
{
    [LoginFilter]
    public class HomeController : ControllerBase
    {
        #region 成员及构造
        public HomeController()
        {
           
        }
        #endregion

        #region 超级管理员工作台首页
        [LoginFilter]
        [HttpGet]
        [OutputCache(Duration = 0)]
        public ActionResult HomePageView()
        {
            //throw new Exception("测试错误页");
            return View("HomePageView");
        }
        #endregion
    }
}
