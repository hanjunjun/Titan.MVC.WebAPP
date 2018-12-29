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
    public static class SysTitleSvc_Other
    {
        public static List<SysTitle> GetSysTitleList(this SysTitleSvc sysTitleSvc)
        {
            var titleList = new List<SysTitle>();
            ModelRespositoryFactory<SysTitle, string> modeSvc = new ModelRespositoryFactory<SysTitle, string>();
            var list = modeSvc.context.Database.SqlQuery<SysTitle>("select * from [SysTitle] where TitleFatherId is null order by TitleOrderIndex").ToList();
            var allTitleList= modeSvc.context.Database.SqlQuery<SysTitle>("select * from [SysTitle] where Isdelete!=1 and IsDisplay=1 order by TitleOrderIndex").ToList();
            GetSysTitleChildList(list, titleList, allTitleList);
            return titleList;
        }

        private static void GetSysTitleChildList(List<SysTitle> dv, List<SysTitle> titleList, List<SysTitle> allTitleList)
        {
            foreach (var item in dv)
            {
                titleList.Add(item);
                var list = allTitleList.Where(x =>
                    x.TitleFatherId ==
                    item.SysTitleId).ToList(); 
                GetSysTitleChildList(list, titleList,allTitleList);
            }
        }
    }
}
