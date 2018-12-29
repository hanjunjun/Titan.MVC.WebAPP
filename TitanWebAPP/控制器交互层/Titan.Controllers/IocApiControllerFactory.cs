/************************************************************************
 * 文件名：IoCApiControllerFactory
 * 文件功能描述：StructureMapHttpControllerActivator
 * 作    者：Weidaicheng
 * 创建日期：2017-11-10
 * 修 改 人：
 * 修改日期：
 * 修改原因：
 * Copyright (c) 2016 Titan.Han . All Rights Reserved. 
 * ***********************************************************************/

using StructureMap;
using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace Titan.Controllers
{
	public class StructureMapHttpControllerActivator : IHttpControllerActivator
	{
		private readonly IContainer container;

		public StructureMapHttpControllerActivator(IContainer container)
		{
			this.container = container;
		}

		public IHttpController Create(
				HttpRequestMessage request,
				HttpControllerDescriptor controllerDescriptor,
				Type controllerType)
		{
			return (IHttpController)this.container.GetInstance(controllerType);
		}
	}
}
