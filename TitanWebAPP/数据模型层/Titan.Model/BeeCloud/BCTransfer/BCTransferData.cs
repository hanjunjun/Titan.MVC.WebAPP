using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Titan.Model.BeeCloud.BCTransfer
{
    /// <summary>
    /// 以下所有字段在创建时都是必填的
    /// </summary>
    public class BCTransferData
    {
        /// <summary>
        /// 付款流水号，32位以内数字字母
        /// </summary>
        public string transferId { get; set; }
        /// <summary>
        /// 收款方支付宝账号
        /// </summary>
        public string receiverAccount { get; set; }
        /// <summary>
        /// 收款方支付宝账户名
        /// </summary>
        public string receiverName { get; set; }
        /// <summary>
        /// 付款金额，单位为分
        /// </summary>
        public int transferFee { get; set; }
        /// <summary>
        /// 付款备注
        /// </summary>
        public string transferNote { get; set; }
    }
}
