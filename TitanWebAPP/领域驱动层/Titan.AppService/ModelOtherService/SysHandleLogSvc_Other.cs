using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Titan.AppService.ModelDTO;
using Titan.AppService.ModelService;
using Titan.Model.DataModel;
using Titan.RepositoryCode;


namespace Titan.AppService.ModelOtherService
{
    public static class SysHandleLogSvc_Other
    {
        public static void AddSysHandleLog(this SysHandleLogSvc sysHandleLogSvc, SysHandleLogAddorUpdateDto model)
        {
            ModelRespositoryFactory<SysHandleLog, Guid> modeSvc = new ModelRespositoryFactory<SysHandleLog, Guid>();
            modeSvc.context.Database.ExecuteSqlCommand($@"INSERT INTO SysHandleLog (
	SysHandleLogId,
	SysEmployeeId,
	SysTitleId,
	HandleTime,
	HandleAction,
	HandleDataId,
	HandleLogIP,
	HandleActionCID,
	HandleLogDesc,
	Isdelete
)
VALUES
	(
		NEWID(),
		'{model.SysEmployeeId}',
		{(model.SysTitleId == null ? "null" : $"'{model.SysTitleId}'")},
		'{model.HandleTime}',
		'{model.HandleAction}',
		{(model.HandleDataId == null ? "null" : $"'{model.HandleDataId}'")},
		'{model.HandleLogIP}',
		'{model.HandleActionCID}',
		'{model.HandleLogDesc}',
        '{model.Isdelete}'
	)");
        }
        public static void AddSysHandleLog( SysHandleLogAddorUpdateDto model)
        {
            ModelRespositoryFactory<SysHandleLog, Guid> modeSvc = new ModelRespositoryFactory<SysHandleLog, Guid>();
            modeSvc.context.Database.ExecuteSqlCommand($@"INSERT INTO SysHandleLog (
	SysHandleLogId,
	SysEmployeeId,
	SysTitleId,
	HandleTime,
	HandleAction,
	HandleDataId,
	HandleLogIP,
	HandleActionCID,
	HandleLogDesc,
	Isdelete
)
VALUES
	(
		NEWID(),
		'{model.SysEmployeeId}',
		{(model.SysTitleId == null ? "null" : $"'{model.SysTitleId}'")},
		'{model.HandleTime}',
		'{model.HandleAction}',
		{(model.HandleDataId == null ? "null" : $"'{model.HandleDataId}'")},
		'{model.HandleLogIP}',
		'{model.HandleActionCID}',
		'{model.HandleLogDesc}',
        '{model.Isdelete}'
	)");
        }
    }
}
