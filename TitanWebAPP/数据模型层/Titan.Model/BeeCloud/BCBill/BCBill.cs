using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Titan.Model.BeeCloud.BCBill
{
    public class BCBill
    {
        public BCBill() { }

        public BCBill(string _channel, int _totalFee, string _billNo, string _title) 
        {
            channel = _channel;
            totalFee = _totalFee;
            billNo = _billNo;
            title = _title;
        }

        /// <summary>
        /// 订单记录的唯一标识，可用于查询单笔记录
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string billNo { get; set; }
        /// <summary>
        /// 实付金额，单位为分
        /// </summary>
        public int totalFee { get; set; }
        /// <summary>
        /// 渠道交易号， 当支付成功时有值
        /// </summary>
        public string tradeNo { get; set; }
        /// <summary>
        ///     可以通过enum BCPay.PayChannel获取
        ///     channel的参数值含义：
        ///     WX_APP:       微信手机APP支付
        ///     WX_NATIVE:    微信公众号二维码支付
        ///     WX_JSAPI:     微信公众号支付
        ///     ALI_APP:      支付宝APP支付
        ///     ALI_WEB:      支付宝网页支付 
        ///     ALI_QRCODE:   支付宝内嵌二维码支付
        ///     UN_APP:       银联APP支付
        ///     UN_WEB:       银联网页支付
        ///     JD_WAP:       京东wap支付
        ///     JD_WEB:       京东web支付
        ///     YEE_WAP:      易宝wap支付 
        ///     YEE_WEB:      易宝web支付
        ///     KUAIQIAN_WAP: 快钱wap支付
        ///     KUAIQIAN_WEB: 快钱web支付
        ///     BD_WEB:       百度web支付
        ///     BD_WAP:       百度wap支付
        /// </summary>
        public string channel { get; set; }
        /// <summary>
        /// 订单标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 订单是否成功
        /// </summary>
        public bool result { get; set; }
        /// <summary>
        /// 附加数据,用户自定义的参数，将会在webhook通知中原样返回，该字段是Dic {"key1":"value1","key2":"value2",...}
        /// </summary>
        public Dictionary<string, string> optional { get; set; }
        /// <summary>
        /// 	用于统计分析的数据，将会在控制台的统计分析报表中展示，用户自愿上传
        /// </summary>
        public Analysis analysis { get; set; }
        /// <summary>
        /// 可自定义webhook的发送地址，选填
        /// </summary>
        public string notifyURL { get; set; }
        /// <summary>
        /// 渠道详细信息， 当need_detail传入true时返回
        /// </summary>
        public string messageDetail { get; set; }
        /// <summary>
        /// 订单是否撤销
        /// </summary>
        public bool revertResult { get; set; }
        /// <summary>
        /// 订单是否已经退款
        /// </summary>
        public bool refundResult { get; set; }
        /// <summary>
        /// 订单创建时间
        /// </summary>
        public DateTime createdTime { get; set; }
        /// <summary>
        /// 用户相对于微信公众号的唯一id
        ///     例如'0950c062-5e41-44e3-8f52-f89d8cf2b6eb'
        ///     微信公众号支付(WX_JSAPI)的必填参数
        /// </summary>
        public string openId { get; set; }
        /// <summary>
        /// 商品展示地址
        ///     以http://开头,例如'http://beecloud.cn'
        ///     支付宝网页支付(ALI_WEB)的选填参数
        /// </summary>
        public string showURL { get; set; }
        /// <summary>
        /// 二维码类型
        /// 支付宝内嵌二维码支付(ALI_QRCODE)的选填参数
        /// 二维码类型含义
        ///     0： 订单码-简约前置模式,对应 iframe 宽度不能小于 600px, 高度不能小于 300px
        ///     1： 订单码-前置模式,对应 iframe 宽度不能小于 300px, 高度不能小于 600px
        ///     3： 订单码-迷你前置模式,对应 iframe 宽度不能小于 75px, 高度不能小于 75px
        /// </summary>
        public string qrPayMode { get; set; }
        /// <summary>
        /// 订单失效时间
        ///     必须为非零正整数，单位为秒，建议最短失效时间间隔必须大于300秒
        ///     可空
        ///     京东系列支付不支持该参数，填空
        /// </summary>
        public int? billTimeout { get; set; }
        /// <summary>
        /// 同步返回页面
        ///     支付渠道处理完请求后,当前页面自动跳转到商户网站里指定页面的http路径。
        ///     当channel 参数为 ALI_WEB 或 ALI_QRCODE 或 UN_WEB时为必填
        /// </summary>
        public string returnUrl { get; set; }
        /// <summary>
        /// 发起支付的URL
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// 发起支付的脚本
        /// </summary>
        public string html { get; set; }
        /// <summary>
        /// 支付二维码
        /// </summary>
        public string codeURL { get; set; }
        /// <summary>
        /// 50位以内数字和/或字母组合，易宝移动网页（一键）支付用户唯一标识符，用于绑定用户一键支付的银行卡信息
        /// </summary>
        public string yeeID { get; set; }

        /// <summary>
        /// 支付宝移动网页支付(ALI_WAP)的选填参数,是否尝试掉起支付宝APP原生支付，默认为true
        /// </summary>
        public bool? useApp { get; set; }

        #region 微信JSAPI使用
        /// <summary>
        /// 微信应用APPID
        /// </summary>
        public string appId { get; set; }
        /// <summary>
        /// 微信支付打包参数
        /// </summary>
        public string package { get; set; }
        /// <summary>
        /// 随机字符串
        /// </summary>
        public string noncestr { get; set; }
        /// <summary>
        /// 当前毫秒时间戳，13位
        /// </summary>
        public string timestamp { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        public string paySign { get; set; }
        /// <summary>
        /// 签名类型，固定为MD5
        /// </summary>
        public string signType { get; set; }
        #endregion

        /// <summary>BC_GATEWAY渠道必填参数
        /// bank    取值	含义
        /// CMB	    招商银行
        /// ICBC	工商银行
        /// BOC	    中国银行
        /// ABC	    农业银行
        /// BOCM	交通银行
        /// SPDB	浦发银行
        /// GDB	    广发银行
        /// CITIC	中信银行
        /// CEB	    光大银行
        /// CIB	    兴业银行
        /// SDB	    平安银行
        /// CMBC	民生银行
        /// </summary>
        public string bank { get; set; }
        /// <summary>BC_EXPRESS渠道选填参数
        ///  设置后在wap页面可以实现固定卡号的支付，即不可以从页面输入卡号
        /// </summary>
        public string cardNo { get; set; }
        /// <summary>BC_WX_SCAN或BC_ALI_SCAN 时为必填
        /// 当商户用扫码枪扫用户的条形码时得到的字符串 
        /// </summary>
        public string authCode { get; set; }

        /// <summary>
        /// 用户ID，用来标示下单客户，需要与用户系统注册过的用户ID一致
        /// </summary>
        public string buyerID { get; set; }
        /// <summary>
        /// 卡券id, 传入卡券id，下单时会自动扣除优惠金额再发起支付
        /// </summary>
        public string couponID { get; set; }
        /// <summary>
        /// 订单金额,单位为分
        /// </summary>
        public int billFee { get; set; }
        /// <summary>
        /// 优惠金额
        /// </summary>
        public int discount { get; set; }
    }
}
