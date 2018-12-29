using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Titan.Controllers;
using Titan.WebMVC;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using StructureMap;

namespace Titan.WebMVC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);//WebApi路由
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);//Mvc全局过滤器
            RouteConfig.RegisterRoutes(RouteTable.Routes);//Mvc路由
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ControllerBuilder.Current.SetControllerFactory(new IoCControllerFactory());//mvc的ioc容器
            GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerActivator), new StructureMapHttpControllerActivator(new Container()));//webApi的ioc容器
            log4net.Config.XmlConfigurator.Configure(new FileInfo(Server.MapPath("~/Web.config")));//log4配置
        }

        protected void Application_Error(Object sender, EventArgs e)
        {
            //当路径出错，无法找到控制器时，不会执行FilterConfig中的OnException，而会在这里捕获。
            //当发生404错误时，执行完OnException后，还会执行到这里。
            //当发生其他错误，会执行OnException，但在base.OnException中已经处理完错误，不会再到这里执行。

            var lastError = Server.GetLastError();
            if (lastError != null)
            {
                //var httpError = lastError as HttpException;
                //if (httpError != null)
                //{
                //    switch (httpError.GetHttpCode())
                //    {
                //        case 404:
                //            Server.Transfer("~/Web404.aspx");
                //            break;
                //    }
                //}

                StringBuilder strExceptionMessage = new StringBuilder();
                strExceptionMessage.AppendFormat("错误信息：{1}；<br> 详细信息：{2}", lastError.Source, lastError.Message, lastError.StackTrace);
                Server.ClearError();
                Application["error"] = strExceptionMessage.ToString();
                Server.Transfer("~/ErrorAll.aspx");
            }
        }

        protected void Application_PostAuthenticateRequest(object sender, System.EventArgs e)
        {
            //var formsIdentity = HttpContext.Current.User.Identity as FormsIdentity;
            //if (formsIdentity != null && formsIdentity.IsAuthenticated && formsIdentity.AuthenticationType == "Forms")
            //{
            //    HttpContext.Current.User =
            //        MyFormsAuthentication<MyUserDataPrincipal>.TryParsePrincipal(HttpContext.Current.Request);
            //}
        }
    }
}
