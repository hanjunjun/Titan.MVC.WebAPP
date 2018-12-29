using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Titan.Model.BeeCloud.BCTransfer
{
    public class BCRedPackInfo
    {
        /// <summary>
        /// 红包发送者名称 32位字符
        /// </summary>
        public string sendName { get; set; }
        /// <summary>
        /// 红包祝福语 128 位字符
        /// </summary>
        public string wishing { get; set; }
        /// <summary>
        /// 红包活动名称 32位字符
        /// </summary>
        public string actName { get; set; }
    }
}
