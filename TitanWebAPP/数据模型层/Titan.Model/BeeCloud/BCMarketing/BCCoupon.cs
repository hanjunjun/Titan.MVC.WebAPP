using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Titan.Model.BeeCloud.BCMarketing
{
    public class BCCoupon
    {
        //卡券ID
        public string id { get; set; }
        public BCCouponTemplate template { get; set; }
        public string buyerID { get; set; }
        public string appID { get; set; }
        public int status { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public DateTime useTime { get; set; }

        public BCCoupon() 
        {
            template = new BCCouponTemplate();
        } 
    }
}
