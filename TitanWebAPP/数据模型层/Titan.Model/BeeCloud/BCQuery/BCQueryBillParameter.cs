using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Titan.Model.BeeCloud.BCQuery
{
    public class BCQueryBillParameter
    {
        public string channel { get; set; }
        public string billNo { get; set; }
        public long? startTime { get; set; }
        public long? endTime { get; set; }
        public bool? result { get; set; }
        public bool? needDetail { get; set; }
        public int? skip { get; set; }
        public int? limit { get; set; }
    }
}
