using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Titan.Model.BeeCloud.BCTransfer
{
    public class BCTransferResult
    {
        /// <summary>
        /// 返回码，0为正常
        /// </summary>
        public int resultCode { get; set; }
        /// <summary>
        /// 返回信息， OK为正常
        /// </summary>
        public string resultMsg { get; set; }
        /// <summary>
        /// 具体错误信息
        /// </summary>
        public string errDetail { get; set; }
        /// <summary>
        /// 成功发起打款后返回打款表记录唯一标识
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 需要跳转到支付宝输入密码确认批量打款（支付宝独占字段）
        /// </summary>
        public string url { get; set; }
    }
}
