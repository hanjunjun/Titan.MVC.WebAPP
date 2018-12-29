using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Titan.Model.BeeCloud.BCMarketing
{
    public class BCCouponTemplate
    {
        public string id { get; set; }
        public string name { get; set; }
        public int type { get; set; }
        public string code { get; set; }
        public int limitFee { get; set; }
        public float discount { get; set; }
        public int totalCount { get; set; }
        public int maxCountPerUser { get; set; }
        public int deliverCount { get; set; }
        public int useCount { get; set; }
        public int expiryType { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public int deliveryValidDays { get; set; }
        public int status { get; set; }
        public string mchAccount { get; set; }
        public string appID { get; set; }
    }
}
