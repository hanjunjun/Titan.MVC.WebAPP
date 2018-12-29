/************************************************************************
 * 文件名：ApiAuthorizeAttribute
 * 文件功能描述：xx控制层
 * 作    者：  韩俊俊
 * 创建日期：2018/11/26 13:45:54
 * 修 改 人：
 * 修改日期：
 * 修改原因：
 * Copyright (c) 2017 Titan.Han . All Rights Reserved. 
 * ***********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using JWT;
using JWT.Serializers;
using Titan.Model.JWTModel;

namespace Titan.Controllers.Attributes
{
    public class ApiAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var authHeader = from t in actionContext.Request.Headers where t.Key == "auth" select t.Value.FirstOrDefault();
            if (authHeader != null)
            {
                string token = authHeader.FirstOrDefault();
                if (!string.IsNullOrEmpty(token))
                {
                    const string secret = "The code to create the world!";
                    //secret需要加密
                    IJsonSerializer serializer = new JsonNetSerializer();
                    IDateTimeProvider provider = new UtcDateTimeProvider();
                    IJwtValidator validator = new JwtValidator(serializer, provider);
                    IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                    IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);
                    //var json = decoder.Decode(token, secret, verify: true);//token为之前生成的字符串
                    var json = decoder.DecodeToObject<Payload>(token, secret, verify: true);
                    if (json != null)
                    {
                        actionContext.RequestContext.RouteData.Values.Add("auth", json);
                        return true;
                    }
                    return false;
                }
            }
            return false;
        }
    }
}
