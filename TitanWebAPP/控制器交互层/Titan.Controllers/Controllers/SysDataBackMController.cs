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
using Titan.Controllers.ViewModel.CommonViewModel;
using Titan.Infrastructure.Domain;
using Titan.Model;

namespace Titan.Controllers.Controllers
{
    [LoginFilter]
    public class SysDataBackMController : ControllerBase
    {
        #region 成员及构造

        private readonly SysDataBackMDomainSvc _sysDataBackMDomainSvc;
        public SysDataBackMController(SysDataBackMDomainSvc sysDataBackMDomainSvc)
        {
            _sysDataBackMDomainSvc = sysDataBackMDomainSvc;
        }
        #endregion

        #region 查询、分页
        [LoginFilter]
        [HttpGet]
        [OutputCache(Duration = 0)]
        public ActionResult SysDataBackMListView()
        {
            var model = new SysDataBackMViewList
            {
                //SysDataBackMViewLists = new List<SysDataBackMDto>(),
                ViewQuery = new ViewQuery
                {
                    Param1 = "",
                    StartDateTime = null,
                    EndDateTime = null
                },
                MyPowerModel = NewMethod("")
            };
            return View("SysDataBackMListView", model);
        }

        //分页查询
        [HttpGet]
        [LoginFilter]
        [OutputCache(Duration = 0)]
        public ActionResult MemberShipMOtherListViewBy(DateTime? startTime, DateTime? endTime,string key
            , int pageIndex = pageIndexs, int pageSize = pageSizes)
        {
            endTime = endTime?.AddDays(1).AddSeconds(-1);
            int rowCount;
            GetPageSizeByScreen(ref pageSize);
            var model = new SysDataBackMViewList
            {
                SysDataBackMViewLists = _sysDataBackMDomainSvc.GetSysDataBackMDtos(startTime,endTime,key,CID,pageIndex,pageSize,out rowCount),
                ViewQuery = new ViewQuery
                {
                    Param1 = "",
                    StartDateTime = null,
                    EndDateTime = null
                },
                PagingInfo = new PagingInfo
                {
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    TotalItems = rowCount
                },
                MyPowerModel = NewMethod("")
            };
            return View("SysDataBackMOtherListView", model);
        }
        #endregion
    }
}
