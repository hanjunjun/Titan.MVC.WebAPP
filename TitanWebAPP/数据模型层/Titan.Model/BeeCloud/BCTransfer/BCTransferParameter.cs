using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Titan.Model.BeeCloud.BCTransfer
{
    public class BCTransferParameter
    {
        public string channel { get; set; }
        public string transferNo { get; set; }
        public int totalFee { get; set; }
        public string desc { get; set; }
        public string channelUserId { get; set; }
        public string channelUserName { get; set; }
        public BCRedPackInfo info { get; set; }
        public string accountName { get; set; }
    }
}
