using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Titan.AppService.ModelDTO;
using Titan.AppService.ModelOtherService;
using Titan.AppService.ModelService;
using Titan.Infrastructure.Domain;
using Titan.Model.DataModel;

namespace Titan.AppService.DomainService
{
    public class SysHandleLogDomainSvc
    {
        private SysHandleLogSvc _sysHandleLogSvc;
        public SysHandleLogDomainSvc(SysHandleLogSvc sysHandleLogSvc)
        {
            _sysHandleLogSvc = sysHandleLogSvc;
        }

        #region 添加日志
        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="model"></param>
        public void AddSysHandleLog(SysHandleLogAddorUpdateDto model)
        {
            _sysHandleLogSvc.AddSysHandleLog(model);
        }
        #endregion

    }
}
