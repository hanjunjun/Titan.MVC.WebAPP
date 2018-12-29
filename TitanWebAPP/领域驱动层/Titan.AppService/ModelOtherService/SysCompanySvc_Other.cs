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
    public static class SysCompanySvc_Other
    {
        public static List<SysCompany> GetSysCompanyList(this SysCompanySvc sysTitleSvc)
        {
            var i = 0;
            var titleList = new List<SysCompany>();
            ModelRespositoryFactory<SysCompany, string> modeSvc = new ModelRespositoryFactory<SysCompany, string>();
            var list = modeSvc.context.Database.SqlQuery<SysCompany>("select * from [SysCompany] where SysCompanyFatherId is null order by CompanyOrderIndex").ToList();
            var allTitleList = modeSvc.context.Database.SqlQuery<SysCompany>("select * from [SysCompany] where Isdelete!=1 and CompanyStatus=1 order by CompanyOrderIndex").ToList();
            GetSysCompanyChildList(list, titleList, allTitleList, i);
            return titleList;
        }

        private static void GetSysCompanyChildList(List<SysCompany> dv, List<SysCompany> titleList, List<SysCompany> allTitleList, int i)
        {
            foreach (var item in dv)
            {
                for (int j = 0; j < i; j++)
                {
                    item.CompanyDesc += "--";
                }
                titleList.Add(item);
                var list = allTitleList.Where(x =>
                    x.SysCompanyFatherId ==
                    item.SysCompanyId).ToList();
                GetSysCompanyChildList(list, titleList, allTitleList,  i+1);
            }
        }

        public static SysCompany GetSysCompany(this SysCompanySvc sysTitleSvc,Guid syscompanyId)
        {
            ModelRespositoryFactory<SysCompany, Guid> modeSvc = new ModelRespositoryFactory<SysCompany, Guid>();
            var syscompany = modeSvc.context.Database.SqlQuery<SysCompany>($"select * from [SysCompany] where SysCompanyId='{syscompanyId}'").ToList().FirstOrDefault();
            return syscompany;
        }
    }
}
