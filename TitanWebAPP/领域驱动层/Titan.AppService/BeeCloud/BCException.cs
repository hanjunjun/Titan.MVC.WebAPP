using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Titan.AppService.BeeCloud
{
    public class BCException: ApplicationException
    {
        public BCException() { }
        public BCException(string message)  
            : base(message) { }  
    }
}
