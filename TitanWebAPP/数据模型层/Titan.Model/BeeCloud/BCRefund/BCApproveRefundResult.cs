using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Titan.Model.BeeCloud.BCRefund
{
    public class BCApproveRefundResult
    {
        /// <summary>
        /// 每条退款请求的状态, 成功显示OK，失败显示错误信息
        /// </summary>
        public Dictionary<string, string> status { get; set; }

        /// <summary>
        /// 当channel为ALI_APP、ALI_WEB、ALI_QRCODE时，以下字段在result_code为0时有返回
        /// 支付宝退款地址，需用户在支付宝平台上手动输入支付密码处理
        /// </summary>
        public string url { get; set; }
    }
}
