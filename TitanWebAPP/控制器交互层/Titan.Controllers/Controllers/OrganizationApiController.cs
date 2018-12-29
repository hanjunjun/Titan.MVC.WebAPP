/************************************************************************
 * 文件名：OrganizationAPIController
 * 文件功能描述：通用集成接口API
 * 作    者：  
 * 创建日期：2017年6月19日17:00:00
 * 修 改 人：
 * 修改日期：
 * 修改原因：
 * ***********************************************************************/
using Titan.AppService.DomainService;
using Titan.AppService.ModelService;
using Titan.Infrastructure.Attributes;
using Titan.Infrastructure.ServiceOfBY;
using Titan.Model.DataModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Script.Services;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using Titan.AppService.ModelDTO;
using Titan.Model.CommonModel;
using Titan.Model.JWTModel;

namespace Titan.Controllers.Controllers
{
    public class OrganizationApiController : ApiController
    {
        #region 成员和构造函数

        public OrganizationApiController()
        {

        }
        #endregion

        #region 获取Token--废弃
        ///// <summary>
        ///// 根据用户名获取token
        ///// </summary>
        ///// <param name="staffId"></param>
        ///// <returns></returns>
        //[Route("Organization/GetToken")]
        //[OutputCache(Duration = 0)]
        //public HttpResponseMessage GetToken(string staffId)
        //{
        //    ResultMsg resultMsg = null;
        //    int id = 0;

        //    //判断参数是否合法
        //    if (string.IsNullOrEmpty(staffId) || (!int.TryParse(staffId, out id)))
        //    {
        //        resultMsg = new ResultMsg();
        //        resultMsg.StatusCode = (int)StatusCodeEnum.ParameterError;
        //        resultMsg.Info = StatusCodeEnum.ParameterError.GetEnumText();
        //        resultMsg.Data = "";
        //        return HttpResponseExtension.toJson(JsonConvert.SerializeObject(resultMsg));
        //    }

        //    //插入缓存
        //    Token token = (Token)HttpRuntime.Cache.Get(id.ToString());
        //    if (HttpRuntime.Cache.Get(id.ToString()) == null)
        //    {
        //        token = new Token();
        //        token.StaffId = id;
        //        token.SignToken = Guid.NewGuid();
        //        token.ExpireTime = DateTime.Now.AddDays(1);
        //        HttpRuntime.Cache.Insert(token.StaffId.ToString(), token, null, token.ExpireTime, TimeSpan.Zero);
        //    }

        //    //返回token信息
        //    resultMsg = new ResultMsg();
        //    resultMsg.StatusCode = (int)StatusCodeEnum.Success;
        //    resultMsg.Info = "";
        //    resultMsg.Data = token;

        //    return HttpResponseExtension.toJson(JsonConvert.SerializeObject(resultMsg));
        //}
        #endregion

        #region 统一请求入口
        [System.Web.Http.Route("Organization/Integration")]
        [System.Web.Http.HttpPost]
        [OutputCache(Duration = 0)]
        public JsonResultDto Integration(JsonPara tmpPara)
        {
            JsonResultDto result = new JsonResultDto();
            try
            {
                List<object> paraList = new List<object>();

                #region 反射方法
                string typeName = string.Format("Titan.Controllers.Controllers.{0}Controller", tmpPara.ServiceType);
                Type conType = Type.GetType(typeName);
                //创建方法实例
                var o = ObjectFactory.GetInstance(conType);
                //反射方法
                MethodInfo methodInfo;

                //控制器下的所有方法
                var allMethods = conType.GetMethods();
                //所有标记ApiVersion特性的方法
                var attritedMethods = allMethods.Select(m =>
                {
                    var apiVersionAttr = m.GetCustomAttribute(typeof(ApiAttribute)) as ApiAttribute;
                    if (apiVersionAttr != null && apiVersionAttr.MethodName == tmpPara.Method)
                        return m;
                    else
                        return null;
                }).Where(m => m != null);

                //检查是否有标记相同的版本号
                if (attritedMethods
                    .GroupBy(m => (m.GetCustomAttribute(typeof(ApiAttribute)) as ApiAttribute).Version)
                    .Count() != attritedMethods.Count())
                    throw new ApiVersionException();

                //有标记版本号的方法
                if (attritedMethods.Count() != 0)
                {
                    //根据版本号查找方法
                    var thisVMethod = attritedMethods
                        .FirstOrDefault(m => (m.GetCustomAttribute(typeof(ApiAttribute)) as ApiAttribute).Version == tmpPara.Version);

                    //有该版本的方法
                    if (thisVMethod != null)
                    {
                        methodInfo = thisVMethod;
                        result.Version = tmpPara.Version;
                    }
                    else
                    {
                        //没有该版本号的方法，查找小于该版本号的方法

                        //小于该版本的所有方法
                        var attritedMethodsLowV = attritedMethods
                            .Where(m => (m.GetCustomAttribute(typeof(ApiAttribute)) as ApiAttribute).Version < tmpPara.Version);

                        if (attritedMethodsLowV.Count() > 0)
                        {
                            //有小于该版本的方法
                            //使用小于该版本的最大版本的方法
                            methodInfo = attritedMethodsLowV
                                .OrderByDescending(m => (m.GetCustomAttribute(typeof(ApiAttribute)) as ApiAttribute).Version)
                                .First();
                            result.Version = (methodInfo.GetCustomAttribute(typeof(ApiAttribute)) as ApiAttribute).Version;
                        }
                        else
                        {
                            //没有小于该版本的方法，使用原始方法
                            methodInfo = conType.GetMethod(tmpPara.Method);
                            result.Version = 0;
                        }
                    }
                }
                else
                {
                    //没有标记版本号的方法，使用原始方法
                    methodInfo = conType.GetMethod(tmpPara.Method);
                    result.Version = 0;
                }
                #endregion

                #region 分页
                //反射分页特性
                var attributeInfo = methodInfo.GetCustomAttribute(typeof(PagedAttribute)) as PagedAttribute;
                if (attributeInfo != null)
                {
                    if (tmpPara.PageNumber == null)
                        throw new Exception("未包含页码");

                    if (tmpPara.PageNumber <= 0)
                        throw new Exception("页码必须大于1");

                    if (!methodInfo.ReturnType.CanBeCastTo(typeof(IEnumerable<object>)))
                        throw new Exception($"方法{methodInfo.Name}不返回枚举类型");
                }
                #endregion

                #region 处理参数
                ParameterInfo[] args = methodInfo.GetParameters();//方法参数列表
                if (args != null && args.Length > 0)
                {
                    foreach (ParameterInfo paraInfo in args)
                    {
                        #region 遍历参数列表，并对参数赋值
                        if (tmpPara.Paras != null && tmpPara.Paras.Length > 0)
                        {
                            foreach (MethodParameter item in tmpPara.Paras)
                            {
                                if (paraInfo.Name.Equals(item.ParaName))//匹配到参数
                                {
                                    #region 加载参数
                                    if (paraInfo.ParameterType.IsValueType || paraInfo.ParameterType.Name.Equals("String"))
                                    {
                                        var typename = paraInfo.ParameterType.Name;
                                        if (paraInfo.ParameterType.IsGenericType && paraInfo.ParameterType.GetGenericTypeDefinition() == typeof(Nullable<>))
                                        {
                                            typename = paraInfo.ParameterType.GetGenericArguments()[0].Name; ;
                                        }
                                        #region 值类型或string类型直接加载
                                        switch (typename.ToUpper())
                                        {
                                            case "INT32":
                                                int tempInt = 0;
                                                int.TryParse(item.ParaValue.ToString(), out tempInt);
                                                paraList.Add(tempInt);
                                                break;
                                            case "DATETIME":
                                                DateTime tempDateTime = DateTime.MinValue;
                                                if (DateTime.TryParse(item.ParaValue.ToString(), out tempDateTime))
                                                    paraList.Add(tempDateTime);
                                                else
                                                    paraList.Add(null);
                                                break;
                                            case "GUID":
                                                if (!string.IsNullOrEmpty(item.ParaValue?.ToString()))
                                                {
                                                    paraList.Add(new Guid(item.ParaValue.ToString()));
                                                }
                                                else
                                                {
                                                    paraList.Add(Guid.Empty);
                                                }
                                                break;
                                            case "STRING":
                                                if (!string.IsNullOrEmpty(item.ParaValue?.ToString()))
                                                {
                                                    paraList.Add(item.ParaValue?.ToString());
                                                }
                                                else
                                                {
                                                    paraList.Add(string.Empty);
                                                }
                                                break;
                                            default:
                                                if (Nullable.GetUnderlyingType(paraInfo.ParameterType) != null)
                                                {
                                                    paraList.Add(ChangeType(item.ParaValue, Nullable.GetUnderlyingType(paraInfo.ParameterType)));
                                                }
                                                else
                                                {
                                                    paraList.Add(ChangeType(item.ParaValue, paraInfo.ParameterType));
                                                }
                                                break;
                                        }
                                        #endregion
                                    }
                                    else if (paraInfo.ParameterType.IsGenericType)
                                    {
                                        #region 集合(目前只支持List<string>,其它没想好)
                                        //创建参数实例
                                        var o1 = Activator.CreateInstance(paraInfo.ParameterType);
                                        //o1 = JsonConvert.DeserializeObject<List<string>>(item.ParaValue.ToString());
                                        o1 = JsonConvert.DeserializeObject<List<string>>(item.ParaValue.ToString());
                                        paraList.Add(o1);
                                        #endregion
                                    }
                                    else
                                    {
                                        #region 类对象
                                        //创建参数实例
                                        var o1 = Activator.CreateInstance(paraInfo.ParameterType);
                                        //反射需要传入参数属性
                                        PropertyInfo[] pArgs = o1.GetType().GetProperties();

                                        #region 为参数赋值
                                        foreach (var pA in pArgs)
                                        {
                                            if (item.ParaValue != null)
                                            {
                                                var jObject = JObject.Parse(item.ParaValue.ToString());
                                                JToken value;
                                                if (jObject.TryGetValue(pA.Name, out value))
                                                {
                                                    string v = value.Value<string>();
                                                    Type pp = pA.PropertyType;
                                                    var typename = pA.PropertyType.Name;
                                                    if (pA.PropertyType.IsGenericType && pA.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                                                    {
                                                        typename = pA.PropertyType.GetGenericArguments()[0].Name; ;
                                                    }
                                                    switch (typename)
                                                    {
                                                        case "Guid":
                                                            if (!string.IsNullOrEmpty(v))
                                                            {
                                                                pA.SetValue(o1, new Guid(v));
                                                            }
                                                            else
                                                            {
                                                                pA.SetValue(o1, Guid.Empty);
                                                            }
                                                            break;
                                                        case "Int32":
                                                            int tempInt = 0;
                                                            int.TryParse(v, out tempInt);
                                                            pA.SetValue(o1, tempInt);
                                                            break;
                                                        case "DateTime":
                                                            DateTime tempDateTime = DateTime.MinValue;
                                                            if (DateTime.TryParse(v, out tempDateTime))
                                                            {
                                                                pA.SetValue(o1, tempDateTime);
                                                            }
                                                            else
                                                            {
                                                                pA.SetValue(o1, null);
                                                            }
                                                            break;
                                                        default:

                                                            Type targetType = pA.PropertyType;
                                                            if (Nullable.GetUnderlyingType(pA.PropertyType) != null)
                                                                targetType = Nullable.GetUnderlyingType(pA.PropertyType);
                                                            //prop.SetValue(instance, ChangeType(value, targetType), null);
                                                            pA.SetValue(o1, ChangeType(v, targetType));
                                                            break;
                                                    }
                                                }
                                            }
                                        }
                                        #endregion

                                        paraList.Add(o1);
                                        #endregion
                                    }
                                    break;
                                    #endregion
                                }
                            }
                        }
                        else
                        {
                            break;
                        }
                        #endregion
                    }
                }
                #endregion

                if (attributeInfo != null)
                {
                    var objs = methodInfo.Invoke(o, paraList.ToArray()) as IEnumerable<object>;
                    var totalRecords = objs.Count();
                    var totalPages = totalRecords % attributeInfo.PageSize == 0 ? totalRecords / attributeInfo.PageSize : (totalRecords / attributeInfo.PageSize + 1);

                    if (tmpPara.PageNumber > totalPages)
                        throw new Exception($"页码超过最大页数 {totalPages}");

                    #region 数据
                    result.Data = objs.Skip(tmpPara.PageNumber.Value - 1).Take(attributeInfo.PageSize);
                    #endregion

                    #region 分页
                    if (attributeInfo.ReturnPageInfo)
                    {
                        result.PageInfo = new PageInfo();

                        #region 页码
                        result.PageInfo.PageNumber = tmpPara.PageNumber.Value;
                        result.PageInfo.PageSize = attributeInfo.PageSize;
                        result.PageInfo.TotalRecords = totalRecords;
                        result.PageInfo.TotalPages = totalPages;
                        #endregion

                        #region Url
                        if (attributeInfo.ReturnUrl)
                        {
                            result.PageInfo.Url = Request.RequestUri.ToString();
                        }
                        #endregion

                        #region 参数
                        if (attributeInfo.ReturnParas)
                        {
                            result.PageInfo.PageParas = new PagePara
                            {
                                Self = tmpPara,
                                First = new JsonPara()
                                {
                                    Token = tmpPara.Token,
                                    ServiceType = tmpPara.ServiceType,
                                    Method = tmpPara.Method,
                                    Paras = tmpPara.Paras,
                                    PageNumber = 1
                                },
                                Last = new JsonPara()
                                {
                                    Token = tmpPara.Token,
                                    ServiceType = tmpPara.ServiceType,
                                    Method = tmpPara.Method,
                                    Paras = tmpPara.Paras,
                                    PageNumber = totalPages
                                },
                                Privious = new Func<JsonPara>(() =>
                                {
                                    if (tmpPara.PageNumber == 1)
                                        return null;

                                    return new JsonPara()
                                    {
                                        Token = tmpPara.Token,
                                        ServiceType = tmpPara.ServiceType,
                                        Method = tmpPara.Method,
                                        Paras = tmpPara.Paras,
                                        PageNumber = tmpPara.PageNumber - 1
                                    };
                                }).Invoke(),
                                Next = new Func<JsonPara>(() =>
                                {
                                    if (tmpPara.PageNumber == totalPages)
                                        return null;

                                    return new JsonPara()
                                    {
                                        Token = tmpPara.Token,
                                        ServiceType = tmpPara.ServiceType,
                                        Method = tmpPara.Method,
                                        Paras = tmpPara.Paras,
                                        PageNumber = tmpPara.PageNumber + 1
                                    };
                                }).Invoke()
                            };
                        }
                        #endregion 
                    }
                    #endregion
                }
                else
                {
                    result.Data = methodInfo.Invoke(o, paraList.ToArray());//执行方法 
                    result.PageInfo = null;
                }
            }
            catch (Exception ex)
            {
                result.StatusCode = "8010";
                if (ex.InnerException != null && !string.IsNullOrEmpty(ex.InnerException.Message))
                {
                    result.StatusMsg = ex.InnerException.Message + ":" + ex.InnerException.StackTrace;
                }
                else
                {
                    result.StatusMsg = ex.Message + ":" + ex.StackTrace;
                }
            }

            return result;
        }

        private Object ChangeType(object value, Type targetType)
        {
            Type convertType = targetType;
            if (targetType.IsGenericType && targetType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                //NullableConverter nullableConverter = new NullableConverter(targetType);
                //convertType = nullableConverter.UnderlyingType;
                convertType = targetType.GetGenericArguments()[0];
            }


            return Convert.ChangeType(value, convertType);
        }
        #endregion

        #region 测试接口
        //[Route("Organization/GetProduct")]
        //[HttpGet]
        //public HttpResponseMessage GetProduct(string id)
        //{
        //    var product = new Product() { Id = 1, Name = "哇哈哈", Count = 10, Price = 38.8 };
        //    ResultMsg resultMsg = null;
        //    resultMsg = new ResultMsg();
        //    resultMsg.StatusCode = (int)StatusCodeEnum.Success;
        //    resultMsg.Info = StatusCodeEnum.Success.GetEnumText();
        //    resultMsg.Data = product;
        //    return HttpResponseExtension.toJson(JsonConvert.SerializeObject(resultMsg));
        //}

        //[Route("Organization/AddProudct")]
        //[HttpPost]
        //public HttpResponseMessage AddProudct(Product product)
        //{
        //    ResultMsg resultMsg = null;
        //    resultMsg = new ResultMsg();
        //    resultMsg.StatusCode = (int)StatusCodeEnum.Success;
        //    resultMsg.Info = StatusCodeEnum.Success.GetEnumText();
        //    resultMsg.Data = product;
        //    return HttpResponseExtension.toJson(JsonConvert.SerializeObject(resultMsg));
        //}
        #endregion
    }
}
