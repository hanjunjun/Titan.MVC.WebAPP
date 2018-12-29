using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Titan.AppService.ModelDTO;

namespace Titan.Controllers.ViewModel
{
    public class SysTitleViewList:SysTitleDto
    {
        public List<SysTitleViewList> SysTitleViewLists { get; set; }
    }
}
