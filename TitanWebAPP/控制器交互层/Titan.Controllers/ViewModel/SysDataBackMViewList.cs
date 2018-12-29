using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Titan.AppService.ModelDTO;

namespace Titan.Controllers.ViewModel
{
    #region 查询
    public class SysDataBackMViewList : ViewModelListBase
    {
        public List<SysDataBackMDto> SysDataBackMViewLists { get; set; }
    }
    #endregion

    #region 新增/修改

    public class SysDataBackMEditView
    {
        public SysDataBackMAddorUpdateDto SysDataBackMAddorUpdateDtos { get; set; }
    }
    #endregion

    #region 查看

    public class SysDataBackMLookView
    {
        public SysDataBackMLookDto SysDataBackMLookDtos { get; set; }
    }
    #endregion
}
