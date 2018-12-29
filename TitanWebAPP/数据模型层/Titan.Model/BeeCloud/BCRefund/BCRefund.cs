using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Titan.Model.BeeCloud.BCRefund
{
    public class BCRefund
    {
        public BCRefund() { }

        public BCRefund(String billNo, String refundNo, int refundFee)
        {

            this.billNo = billNo;
            this.refundNo = refundNo;
            this.refundFee = refundFee;
        }

        /// <summary>
        /// 退款记录的唯一标识，可用于查询单笔记录
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string billNo { get; set; }
        /// <summary>
        /// 退款号
        /// </summary>
        public string refundNo { get; set; }
        /// <summary>
        /// 订单标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 订单金额，单位为分
        /// </summary>
        public int totalFee { get; set; }
        /// <summary>
        /// 退款金额，单位为分
        /// </summary>
        public int refundFee { get; set; }
        /// <summary>
        /// 渠道类型 WX、ALI、UN、JD、YEE、KUAIQIAN、BD
        /// </summary>
        public string channel { get; set; }
        /// <summary>
        /// 退款是否完成
        /// </summary>
        public bool finish { get; set; }
        /// <summary>
        /// 退款是否成功
        /// </summary>
        public bool result { get; set; }
        /// <summary>
        /// 附加数据,用户自定义的参数，
        /// 将会在webhook通知中原样返回，
        /// 该字段是dic "{"key1":"value1","key2":"value2",...}"
        /// </summary>
        public Dictionary<string, string> optional { get; set; }
        /// <summary>
        /// 渠道详细信息， 当need_detail传入true时返回
        /// </summary>
        public string messageDetail { get; set; }
        /// <summary>
        /// 退款创建时间
        /// </summary>
        public DateTime createdTime { get; set; }
        /// <summary>
        /// 当channel为ALI_APP、ALI_WEB、ALI_QRCODE时，以下字段在成功时有返回
        /// 支付宝退款地址，需用户在支付宝平台上手动输入支付密码处理
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// 标识该笔是预退款还是直接退款
        /// </summary>
        public bool needApproval { get; set; }
        /// <summary>
        /// 微信渠道退款资金来源
        /// 1:可用余额退款 
        /// 0:未结算资金退款（默认使用未结算资金退款）
        /// </summary>
        public int refundAccount { get; set; }

    }
}
