/************************************************************************
 * 文件名：LoginDomainSvc
 * 文件功能描述：登陆页领域层
 * 作    者：  
 * 创建日期：2017-06-08 15:09:34
 * 修 改 人：
 * 修改日期：
 * 修改原因：
 * Copyright (c) 2017 Titan.Han . All Rights Reserved. 
 * ***********************************************************************/
using AutoMapper;
using Titan.Infrastructure.Domain;
using Titan.Model.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Titan.AppService.ModelDTO;
using Titan.AppService.ModelOtherService;
using Titan.AppService.ModelService;
using Titan.RepositoryCode;

namespace Titan.AppService.DomainService
{
    public class LoginDomainSvc
    {
        #region 成员及构造

        private readonly SysCompanySvc _sysCompanySvc;
        private readonly SysEmployeeSvc _sysEmployeeSvc;
        private readonly SysTitleSvc _sysTitleSvc;
        private readonly ModelRespositoryFactory<SysEmployee, Guid> _sysEmployeeModeSvc;
        private readonly ModelRespositoryFactory<SysCompany, Guid> _sysCompanyModeSvc;
        private readonly ModelRespositoryFactory<SysTitle, Guid> _sysTitleModeSvc;
        public LoginDomainSvc(SysCompanySvc sysCompanySvc, SysEmployeeSvc sysEmployeeSvc, SysTitleSvc sysTitleSvc
            , ModelRespositoryFactory<SysEmployee, Guid> sysEmployeeModeSvc, ModelRespositoryFactory<SysCompany, Guid> sysCompanyModeSvc
            , ModelRespositoryFactory<SysTitle, Guid> sysTitleModeSvc)
        {
            _sysCompanySvc = sysCompanySvc;_sysEmployeeSvc = sysEmployeeSvc;
            _sysTitleSvc = sysTitleSvc;
            _sysEmployeeModeSvc = sysEmployeeModeSvc;
            _sysCompanyModeSvc = sysCompanyModeSvc;
            _sysTitleModeSvc = sysTitleModeSvc;
        }
        #endregion

        /// <summary>
        /// 根据用户名获取用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public SysEmployee GetSysEmployeeByUserId(string userId)
        {
            return _sysEmployeeModeSvc.GetDatasNoTracking(x => x.EmployeeStatus.Value.Equals(1)
                                                         && (x.SysCompany.CompanyLoginCode + x.EmployeeId) == userId)
                .FirstOrDefault();
            //return _sysEmployeeSvc.GetSysEmployeeByUserId(userId);
        }

        public SysEmployee GetSysEmployeeByUserIdAndUserPwd(string userId,string userPwd)
        {
            return _sysEmployeeModeSvc.GetDatasNoTracking(x => x.EmployeeStatus.Value.Equals(1)
                                                         && (x.SysCompany.CompanyLoginCode + x.EmployeeId) == userId
                                                         && x.EmployeePwd == userPwd).FirstOrDefault();
            //return _sysEmployeeSvc.GetSysEmployeeByUserIdAndUserPwd(userId,userPwd);
        }

        public void UpdateSysEmployeeDto(SysEmployee model)
        {
            _sysEmployeeSvc.UpdateModel(model);
        }

        public List<SysCompany> GetSysCompanyList()
        {
            return _sysCompanySvc.GetSysCompanyList();
        }

        public SysCompany GetSysCompany(Guid syscompanyId)
        {
            return _sysCompanySvc.GetSysCompany(syscompanyId);
        }

        /// <summary>
        /// 查找大模块
        /// </summary>
        /// <returns></returns>
        public List<SysTitleDto> GetSysTitleByMain()
        {
            //Expression<Func<SysTitle, bool>> selector = x => x.TitleFatherId == null && x.Isdelete != true && x.IsDisplay==1;
            //Expression<Func<SysTitle, int>> orderby = x => x.TitleOrderIndex.Value;
            //var bl = _sysTitleSvc.GetModelListOrderBy(selector, orderby, null, true);
            //return bl.MapToList<SysTitle, SysTitleDto>();
            var bl = _sysTitleModeSvc.GetDatasNoTracking(x =>
                x.TitleFatherId == null && x.Isdelete != true && x.IsDisplay == 1).ToList();
            return bl.MapToList<SysTitle, SysTitleDto>().OrderBy(x=>x.TitleOrderIndex).ToList();
        }

        /// <summary>
        /// 查找子级模块
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<SysTitleDto> GetSysTitleByFatherId(Guid id)
        {
            //Expression<Func<SysTitle, bool>> selector = x => x.TitleFatherId == id && x.Isdelete != true && x.IsDisplay==1;
            //var bl = _sysTitleSvc.GetModelList(selector, null, null, false);
            //return bl.MapToList<SysTitle, SysTitleDto>().OrderBy(x => x.TitleOrderIndex).ToList();
            var bl = _sysTitleModeSvc.GetDatasNoTracking(x =>
                x.TitleFatherId == id && x.Isdelete != true && x.IsDisplay == 1).ToList();
            return bl.MapToList<SysTitle, SysTitleDto>().OrderBy(x => x.TitleOrderIndex).ToList();
        }
    }
}
