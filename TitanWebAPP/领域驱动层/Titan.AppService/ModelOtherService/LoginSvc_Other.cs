using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Titan.AppService.ModelService;
using Titan.Model.DataModel;
using Titan.RepositoryCode;

namespace Titan.AppService.ModelOtherService
{
    public static class LoginSvc_Other
    {
        //查用户名是否存在
        public static SysEmployee GetSysEmployeeByUserId(this SysEmployeeSvc sysEmployeeSvc, string userId)
        {
            ModelRespositoryFactory<SysEmployee, Guid> modeSvc = new ModelRespositoryFactory<SysEmployee, Guid>();
            var list = modeSvc.context.Database.SqlQuery<SysEmployee>($@"SELECT
	*
FROM
	SysEmployee
LEFT JOIN SysCompany ON SysEmployee.SysCompanyId = SysCompany.SysCompanyId
WHERE
	EmployeeStatus = 1
AND SysCompany.CompanyLoginCode + SysEmployee.EmployeeId = '{userId}'").ToList().FirstOrDefault();
            return list;
        }

        //查用户名密码是否匹配
        public static SysEmployee GetSysEmployeeByUserIdAndUserPwd(this SysEmployeeSvc sysEmployeeSvc, string userId, string userPwd)
        {
            ModelRespositoryFactory<SysEmployee, Guid> modeSvc = new ModelRespositoryFactory<SysEmployee, Guid>();
            var list = modeSvc.context.Database.SqlQuery<SysEmployee>($@"SELECT
	*
FROM
	SysEmployee
LEFT JOIN SysCompany ON SysEmployee.SysCompanyId = SysCompany.SysCompanyId
WHERE
	EmployeeStatus = 1
AND SysCompany.CompanyLoginCode + SysEmployee.EmployeeId = '{userId}'
AND EmployeePwd = '{userPwd}'").ToList().FirstOrDefault();
            return list;
        }
    }
}
