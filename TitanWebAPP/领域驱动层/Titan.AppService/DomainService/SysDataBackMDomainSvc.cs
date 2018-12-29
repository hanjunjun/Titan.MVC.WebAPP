using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Titan.AppService.ModelDTO;
using Titan.AppService.ModelOtherService;
using Titan.AppService.ModelService;
using Titan.Infrastructure.Domain;
using Titan.Model.DataModel;

namespace Titan.AppService.DomainService
{
    public class SysDataBackMDomainSvc
    {
        #region 成员及构造

        private readonly SysDataBackMSvc _sysDataBackMSvc;
        public SysDataBackMDomainSvc(SysDataBackMSvc sysDataBackMSvc)
        {
            _sysDataBackMSvc = sysDataBackMSvc;
        }
        #endregion


        public List<SysDataBackMDto> GetSysDataBackMDtos(DateTime? startTime, DateTime? endTime,string key, Guid cid,
            int pageIndex, int pageSize, out int rowCount)
        {
            Expression<Func<SysDataBackM, bool>> selector = x =>
                ((startTime == null
                     ? true
                     : x.DataBackMStartTime >= startTime && (endTime == null ? true : x.DataBackMEndTime <= endTime)) &&
                 x.DataBackMFileName.Contains(key));
            Expression<Func<SysDataBackM, DateTime>> orderBy = x => (x.DataBackMStartTime.Value);
            Expression<Func<SysDataBackM, string>> orderBy1 = x => x.DataBackMFileName;
            var sysDataBackMs = _sysDataBackMSvc.GetModelListT(pageIndex, pageSize, out rowCount
                , selector, orderBy, orderBy1,false);
            return sysDataBackMs.MapToList<SysDataBackM, SysDataBackMDto>();
        }
    }
}
