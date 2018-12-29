/************************************************************************
 * 文件名：IoCControllerFactory
 * 文件功能描述：控制层IOC容器工厂
 * 作    者：hjj
 * 创建日期：2017-06-15
 * 修 改 人：
 * 修改日期：
 * 修改原因：
 * Copyright (c) 2016 Titan.Han . All Rights Reserved. 
 * ***********************************************************************/
using StructureMap;
using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace Titan.Controllers
{
    /// <summary>
    /// IOC初始化工厂
    /// </summary>
    public class IoCControllerFactory : DefaultControllerFactory
    {
        /// <summary>
        /// IOC初始化
        /// </summary>
        /// <param name="requestContext"></param>
        /// <param name="controllerType"></param>
        /// <returns></returns>
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType != null)
            {
                Controller c = ObjectFactory.GetInstance(controllerType) as Controller;
                c.ActionInvoker = new ErrorHandlingActionInvoker(new HandleErrorAttribute());
                return c;
            }
            else
                return null;
            // return ObjectFactory.GetInstance(controllerType) as IController;
        }
    }

    /// <summary>
    /// IOC错误提示
    /// </summary>
    public class ErrorHandlingActionInvoker : ControllerActionInvoker
    {
        private readonly IExceptionFilter filter;
        /// <summary>
        /// 控制器错误处理
        /// </summary>
        /// <param name="filter"></param>
        public ErrorHandlingActionInvoker(IExceptionFilter filter)
        {
            if (filter == null)
                throw new ArgumentNullException("Exception filter is missing");
            this.filter = filter;
        }
        /// <summary>
        /// 控制器上下文文件处理
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="actionDescriptor"></param>
        /// <returns></returns>
        protected override FilterInfo GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            var filterInfo = base.GetFilters(controllerContext, actionDescriptor);
            filterInfo.ExceptionFilters.Add(this.filter);
            return filterInfo;
        }
    }
}