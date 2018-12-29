
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Web.Security;
using Titan.Controllers.ViewModel;
using Titan.Infrastructure.Domain;

namespace Titan.Controllers
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    public class LoginFilter : ActionFilterAttribute
    {
        /// <summary>  
        /// OnActionExecuting是Action执行前的操作  
        /// </summary>  
        /// <param name="filterContext"></param>  
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.RequestContext.HttpContext.Request.IsAuthenticated)
            {
                ContentResult content = new ContentResult();
                content.Content = string.Format("<script type='text/javascript'>window.location.href='{0}';window.location.reload;</script>", FormsAuthentication.LoginUrl);
                filterContext.Result = content;
            }
            else
            {
                //1.登录状态获取用户信息（自定义保存的用户）
                var cookie = filterContext.HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName];

                //2.使用 FormsAuthentication 解密用户凭据
                var ticket = FormsAuthentication.Decrypt(cookie.Value);
                UserInfo userInfo = new UserInfo();

                //3. 直接解析到用户模型里去，有没有很神奇
                userInfo = new JavaScriptSerializer().Deserialize<UserInfo>(ticket.UserData);

                //4. 将要使用的数据放到ViewData 里，方便页面使用
                if (userInfo == null || userInfo.UserId == null || userInfo.UserName == null || userInfo.CompanyName == null ||
                    userInfo.CID == null || userInfo.UserCID == null || userInfo.DeptId == null ||
                    userInfo.PostId == null || userInfo.LoginNo == null || userInfo.CompanyCode == null || userInfo.UserId == Guid.Empty || userInfo.UserName == string.Empty || userInfo.CompanyName == string.Empty ||
                    userInfo.CID == Guid.Empty || userInfo.UserCID == Guid.Empty || userInfo.DeptId == Guid.Empty ||
                    userInfo.PostId == Guid.Empty || userInfo.LoginNo == string.Empty || userInfo.CompanyCode == string.Empty)
                {
                    //少值不允许操作系统,进入登录页面
                    ContentResult content = new ContentResult();
                    content.Content = string.Format("<script type='text/javascript'>window.location.href='{0}';</script>", FormsAuthentication.LoginUrl);
                    filterContext.Result = content;
                }
                else
                {
                    //判断缓存里是否包含某一个userId对应的状态是否为true
                    //如果为true则强制弹到登录页，重新登录，然后在把缓存删除，这个状态用来控制其他地方修改密码，正在使用系统的强制弹出。
                    var flag= CacheHelper.CacheReader(userInfo.UserId.ToString());
                    if (flag!=null && flag.ToString().Trim() == "1")
                    {
                        //如果=1说明 该账号在其他地方修改了密码
                        ContentResult content = new ContentResult();
                        content.Content =
                            string.Format("<script type='text/javascript'>window.location.href='{0}';</script>",
                                FormsAuthentication.LoginUrl);
                        filterContext.Result = content;
                    }
                    else
                    {
                        //验证通过
                        filterContext.Controller.ViewData["CommonDataUserId"] = userInfo.UserId;
                        filterContext.Controller.ViewData["CommonDataUserName"] = userInfo.UserName;
                        filterContext.Controller.ViewData["CommonDataCompanyName"] = userInfo.CompanyName;
                        filterContext.Controller.ViewData["CommonDataCID"] = userInfo.CID;
                        filterContext.Controller.ViewData["CommonDataUserCID"] = userInfo.UserCID;
                        filterContext.Controller.ViewData["CommonDataDeptId"] = userInfo.DeptId;
                        filterContext.Controller.ViewData["CommonDataPostId"] = userInfo.PostId;
                        filterContext.Controller.ViewData["CommonDataLoginNo"] = userInfo.LoginNo;
                        filterContext.Controller.ViewData["CommonDataCompanyCode"] = userInfo.CompanyCode;
                        filterContext.Controller.ViewData["CommonDataSysTitle"] = userInfo.SysTitle;
                        filterContext.Controller.ViewData["CommonDataEmployeePhotoUrl"] = userInfo.EmployeePhotoUrl;
                        filterContext.Controller.ViewData["CommonDataCompanyLogoImgUrl"] = userInfo.CompanyLogoImgUrl;
                    }
                }
            }
            // 别忘了这一句。
            base.OnActionExecuting(filterContext);
        }

    }

    /// <summary>
    /// 控制器执行完成(暂用于记录费用统计流水表)
    /// </summary>
    public class ActionSuccessFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.HttpContext.Response.StatusCode == 200)
            {
                System.Web.HttpRequestBase Requests = filterContext.HttpContext.Request;
                string[] keys = Requests.Form.AllKeys;
                List<string> keylist = new List<string>(keys);
                switch (filterContext.ActionDescriptor.ActionName)
                {
                    //预约缴费
                    case "PayReceptionView":
                        if (keylist.Contains("PayReceptionAddOrUpdateDTOs.ReceptionMakeCostId"))
                        {
                            string ReceptionMakeCostId = Requests.Form["PayReceptionAddOrUpdateDTOs.ReceptionMakeCostId"].ToString();

                            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                            {
                                controller = "CostFlowing",
                                action = "PayReceptionView",
                                id = ReceptionMakeCostId
                            }));
                            return;
                        }
                        break;
                    //预约缴费（退还定金）
                    case "PayReceptionViewByIDBack":
                        {
                            string ReceptionMakeCostId = Requests.Params["id"];
                            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                            {
                                controller = "CostFlowing",
                                action = "PayReceptionBackView",
                                id = ReceptionMakeCostId
                            }));
                            return;
                        }
                    //入院缴费
                    case "PayCheckDataView":
                        if (keylist.Contains("PayCheckDataAddOrUpdateDTOs.CheckElderCostId"))
                        {
                            string CheckElderCostId = Requests.Form["PayCheckDataAddOrUpdateDTOs.CheckElderCostId"].ToString();

                            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                            {
                                controller = "CostFlowing",
                                action = "PayCheckDataView",
                                id = CheckElderCostId
                            }));
                            return;
                        }
                        break;
                    //月度缴费
                    case "PayElderMonthCost0":
                        {
                            //如果返回值不是需要的guid类型，说明报错，不进行修改
                            if (!IsGuid(filterContext))
                            {
                                return;
                            }
                            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                            {
                                controller = "CostFlowing",
                                action = "PayElderMonthCost0",
                                id = ((ContentResult)filterContext.Result).Content
                            }));
                            return;
                        }
                    //批量月度缴费
                    case "PayElderMonthCost":
                        {
                            //如果返回值不是需要的guid类型，说明报错，不进行修改
                            if (!IsString(filterContext))
                            {
                                return;
                            }
                            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                            {
                                controller = "CostFlowing",
                                action = "PayElderMonthCost",
                                idList = ((ContentResult)filterContext.Result).Content
                            }));
                            return;
                        }
                    //月度缴费(当月)
                    case "NSElderMonthCostView":
                        {
                            //如果返回值不是需要的guid类型，说明报错，不进行修改
                            if (!IsGuid(filterContext))
                            {
                                return;
                            }
                            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                            {
                                controller = "CostFlowing",
                                action = "NSElderMonthCostView",
                                id = ((ContentResult)filterContext.Result).Content
                            }));
                            return;
                        }
                    //批量月度缴费(当月)
                    case "PayElderMonthCostMulti":
                        {
                            //如果返回值不是需要的guid类型，说明报错，不进行修改
                            if (!IsGuid(filterContext))
                            {
                                return;
                            }
                            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                            {
                                controller = "CostFlowing",
                                action = "PayElderMonthCostMulti",
                                id = ((ContentResult)filterContext.Result).Content
                            }));
                            return;
                        }
                    //欠费缴费
                    case "OwingMonthCostView":
                        {
                            //如果返回值不是需要的guid类型，说明报错，不进行修改
                            if (!IsGuid(filterContext))
                            {
                                return;
                            }
                            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                            {
                                controller = "CostFlowing",
                                action = "OwingMonthCostView",
                                id = ((ContentResult)filterContext.Result).Content
                            }));
                            return;
                        }
                    //批量欠费缴费
                    case "OwingMonthCostBatchView":
                        {
                            //如果返回值不是需要的guid类型，说明报错，不进行修改
                            if (!IsGuid(filterContext))
                            {
                                return;
                            }
                            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                            {
                                controller = "CostFlowing",
                                action = "OwingMonthCostBatchView",
                                id = ((ContentResult)filterContext.Result).Content
                            }));
                            return;
                        }
                    //预交费
                    case "ElderMonthYCostView":
                        {
                            //如果返回值不是需要的guid类型，说明报错，不进行修改
                            if (!IsGuid(filterContext))
                            {
                                return;
                            }
                            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                            {
                                controller = "CostFlowing",
                                action = "ElderMonthYCostView",
                                id = ((ContentResult)filterContext.Result).Content
                            }));
                            return;
                        }
                    //退住结算（长者）
                    case "PaySettlementByElderListView":
                        {
                            //如果返回值不是需要的guid类型，说明报错，不进行修改
                            if (!IsGuid(filterContext))
                            {
                                return;
                            }
                            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                            {
                                controller = "CostFlowing",
                                action = "PaySettlementByElderListView",
                                id = ((ContentResult)filterContext.Result).Content
                            }));
                            return;
                        }
                    //退住结算（模块）
                    case "PayElderBackLiveCost":
                        {
                            //如果返回值不是需要的guid类型，说明报错，不进行修改
                            if (!IsGuid(filterContext))
                            {
                                return;
                            }
                            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                            {
                                controller = "CostFlowing",
                                action = "PayElderBackLiveCost",
                                id = ((ContentResult)filterContext.Result).Content
                            }));
                            return;
                        }
                    //初始费用充值
                    case "PayCostInitialAddView":
                        {
                            //如果返回值不是需要的guid类型，说明报错，不进行修改
                            if (!IsGuid(filterContext))
                            {
                                return;
                            }
                            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                            {
                                controller = "CostFlowing",
                                action = "PayCostInitialAddView",
                                id = ((ContentResult)filterContext.Result).Content
                            }));
                            return;
                        }
                    //初始费用领回
                    case "PayCostInitialConsumeAddView":
                        {
                            //如果返回值不是需要的guid类型，说明报错，不进行修改
                            if (!IsGuid(filterContext))
                            {
                                return;
                            }
                            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                            {
                                controller = "CostFlowing",
                                action = "PayCostInitialConsumeAddView",
                                id = ((ContentResult)filterContext.Result).Content
                            }));
                            return;
                        }
                    //app端初始费用充值
                    case "PayCostInitialAddViewByAPP":
                        {
                            //如果返回值不是需要的guid类型，说明报错，不进行修改
                            if (!IsGuid(filterContext))
                            {
                                return;
                            }
                            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                            {
                                controller = "CostFlowing",
                                action = "PayCostInitialAddViewByAPP",
                                id = ((ContentResult)filterContext.Result).Content
                            }));
                            return;
                        }
                    //入住意向金缴费
                    case "CheckIntentCostView":
                        {
                            //如果返回值不是需要的guid类型，说明报错，不进行修改
                            if (!IsGuid(filterContext))
                            {
                                return;
                            }
                            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                            {
                                controller = "CostFlowing",
                                action = "CheckIntentCostView",
                                id = ((ContentResult)filterContext.Result).Content
                            }));
                            return;
                        }
                    //入住意向金退费
                    case "CheckIntentCostBackView":
                        {
                            //如果返回值不是需要的guid类型，说明报错，不进行修改
                            if (!IsGuid(filterContext))
                            {
                                return;
                            }
                            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                            {
                                controller = "CostFlowing",
                                action = "CheckIntentCostBackView",
                                id = ((ContentResult)filterContext.Result).Content
                            }));
                            return;
                        }
                    default:
                        break;
                }
            }
            base.OnActionExecuted(filterContext);
        }

        private bool IsGuid(ActionExecutedContext filterContext)
        {
            Guid gReturn = Guid.Empty;
            if (filterContext.Result.GetType() == typeof(EmptyResult))
            {
                return false;
            }
            string id = ((ContentResult)filterContext.Result).Content;
            if (!Guid.TryParse(id, out gReturn))
            {
                return false;
            }
            return true;
        }

        private bool IsString(ActionExecutedContext filterContext)
        {
            if (filterContext.Result.GetType() == typeof(EmptyResult))
            {
                return false;
            }
            return true;
        }
    }

}
