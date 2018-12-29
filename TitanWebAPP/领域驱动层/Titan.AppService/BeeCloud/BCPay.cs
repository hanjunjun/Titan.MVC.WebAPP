using System.Net;
using System.Text;
using System.Web;
using System.Collections.Generic;
using LitJson;
using System;
using Titan.Model.BeeCloud.BCBill;
using Titan.Model.BeeCloud.BCMarketing;
using Titan.Model.BeeCloud.BCQuery;
using Titan.Model.BeeCloud.BCRefund;
using Titan.Model.BeeCloud.BCSubscription;
using Titan.Model.BeeCloud.BCTransfer;
using Titan.Model.BeeCloud.BCTransferWithBankCard;

namespace Titan.AppService.BeeCloud
{
    public class BCPay
    {   
        public enum PayChannel
        {
            WX_NATIVE,
            WX_JSAPI,
            ALI_WEB,
            ALI_QRCODE,
            ALI_WAP,
            UN_WEB,
            UN_WAP,
            JD_WAP,
            JD_WEB,
            YEE_WAP,
            YEE_WEB,
            KUAIQIAN_WAP,
            KUAIQIAN_WEB,
            BD_WEB,
            BD_WAP,
            BC_GATEWAY,
            BC_EXPRESS,
            BC_NATIVE,
            BC_WX_WAP,
            BC_WX_JSAPI,
        };

        public enum OfflinePayChannel
        {
            BC_WX_SCAN,
            BC_ALI_QRCODE,
            BC_ALI_SCAN
        };

        public enum InternationalPay
        {
            PAYPAL_PAYPAL,
            PAYPAL_CREDITCARD,
            PAYPAL_SAVED_CREDITCARD
        };

        public enum QueryChannel
        {
            WX,
            ALI,
            UN,
            WX_APP,
            WX_NATIVE,
            WX_JSAPI,
            ALI_APP,
            ALI_WEB,
            ALI_QRCODE,
            ALI_WAP,
            UN_APP,
            UN_WEB,
            JD_WAP,
            JD_WEB,
            YEE_WAP,
            YEE_WEB,
            KUAIQIAN_WAP,
            KUAIQIAN_WEB,
            BD_WEB,
            BD_WAP,
            PAYPAL
        };

        public enum RefundChannel
        {
            WX,
            ALI,
            UN,
            JD,
            YEE,
            KUAIQIAN,
            BD,
            BC
        };

        public enum RefundStatusChannel
        {
            WX,
            YEE,
            KUAIQIAN,
            BD
        };

        public enum TransferChannel
        {
            ALI,
            WX_REDPACK, 
            WX_TRANSFER, 
            ALI_TRANSFER
        };

        public enum Banks
        {
            CMB,    //招商银行
            ICBC,   //工商银行
            BOC,    //中国银行
            ABC,    //农业银行
            BOCM,   //交通银行
            SPDB,   //浦发银行
            GDB,    //广发银行
            CITIC,  //中信银行
            CEB,    //光大银行
            CIB,    //兴业银行
            SDB,    //平安银行
            CMBC    //民生银行
        };

        #region 支付
        //准备支付数据
        public static string preparePayParameters(BCBill bill)
        {
            long timestamp = BCUtil.GetTimeStamp(DateTime.Now);

            JsonData data = new JsonData();
            data["app_id"] = BCCache.Instance.appId;
            if (!BCCache.Instance.testMode)
            {
                data["app_sign"] = BCPrivateUtil.getAppSignature(BCCache.Instance.appId, BCCache.Instance.appSecret, timestamp.ToString());
            }
            else
            {
                data["app_sign"] = BCPrivateUtil.getAppSignatureByTestSecret(timestamp.ToString());
            }   
            data["timestamp"] = timestamp;
            data["channel"] = bill.channel;
            data["total_fee"] = bill.totalFee;
            data["bill_no"] = bill.billNo;
            data["title"] = bill.title;
            data["return_url"] = bill.returnUrl;

            data["bill_timeout"] = bill.billTimeout;

            data["openid"] = bill.openId;
            data["show_url"] = bill.showURL;
            data["qr_pay_mode"] = bill.qrPayMode;

            data["identity_id"] = bill.yeeID;

            if (bill.useApp.HasValue)
            {
                data["use_app"] = bill.useApp.Value;
            }

            if (bill.bank != null) 
            {
                data["bank"] = bill.bank;
            }

            if (bill.cardNo != null)
            {
                data["card_no"] = bill.cardNo;
            }
            if (bill.notifyURL != null)
            {
                data["notify_url"] = bill.notifyURL;
            }
            if (bill.optional != null && bill.optional.Count > 0)
            {
                data["optional"] = new JsonData();
                foreach (string key in bill.optional.Keys)
                {
                    data["optional"][key] = bill.optional[key];
                }
            }
            if (bill.analysis != null)
            {
                data["analysis"] = JsonMapper.ToObject(JsonMapper.ToJson(bill.analysis));
            }
            if (bill.buyerID != null)
            {
                data["buyer_id"] = bill.buyerID;
            }
            if (bill.couponID != null)
            {
                data["coupon_id"] = bill.couponID;
            }

            data["bc_analysis"] = new JsonData();
            data["bc_analysis"]["sdk_version"] = BCConstants.SDKVersion;

            string paraString = data.ToJson();
            return paraString;
        }

        //处理支付回调
        public static BCBill handlePayResult(string respString, BCBill bill)
        {
            JsonData responseData = JsonMapper.ToObject(respString);

            if (bill.channel == "WX_NATIVE")
            {
                if (responseData["result_code"].ToString() == "0")
                {
                    bill.id = responseData["id"].ToString();
                    if (BCCache.Instance.testMode)
                    {
                        bill.codeURL = responseData["url"].ToString();
                    }
                    else
                    {
                        bill.codeURL = responseData["code_url"].ToString();
                    } 
                    return bill;
                }
                else
                {
                    var ex = new BCException(responseData["err_detail"].ToString());
                    throw ex;
                }
                
            }
            if (bill.channel == "WX_JSAPI")
            {
                if (BCCache.Instance.testMode)
                {
                    throw new BCException("微信公众号内支付不支持测试模式");
                }
                if (responseData["result_code"].ToString() == "0")
                {
                    bill.id = responseData["id"].ToString();
                    bill.appId = responseData["app_id"].ToString();
                    bill.package = responseData["package"].ToString();
                    bill.noncestr = responseData["nonce_str"].ToString();
                    bill.timestamp = responseData["timestamp"].ToString();
                    bill.paySign = responseData["pay_sign"].ToString();
                    bill.signType = responseData["sign_type"].ToString();

                    return bill;
                }
                else
                {
                    var ex = new BCException(responseData["err_detail"].ToString());
                    throw ex;
                }
            }
            if (bill.channel == "ALI_WEB" || bill.channel == "ALI_WAP")
            {
                if (responseData["result_code"].ToString() == "0")
                {
                    bill.id = responseData["id"].ToString();
                    if (BCCache.Instance.testMode)
                    {
                        bill.html = string.Format("<html><head></head><body><script>location.href='{0}'</script></body></html>", responseData["url"].ToString());
                    }
                    else
                    {
                        bill.html = responseData["html"].ToString();
                    }
                    bill.url = responseData["url"].ToString();

                    return bill;
                }
                else
                {
                    var ex = new BCException(responseData["err_detail"].ToString());
                    throw ex;
                }
            }
            if (bill.channel == "ALI_QRCODE")
            {
                if (responseData["result_code"].ToString() == "0")
                {
                    bill.id = responseData["id"].ToString();
                    bill.url = responseData["url"].ToString();
                    if (BCCache.Instance.testMode)
                    {
                        bill.html = string.Format("<html><head></head><body><script>location.href='{0}'</script></body></html>", responseData["url"].ToString());
                    }
                    else
                    {
                        bill.html = responseData["html"].ToString();
                    }
                    return bill;
                }
                else
                {
                    var ex = new BCException(responseData["err_detail"].ToString());
                    throw ex;
                }
            }
            if (bill.channel == "JD_WAP" || bill.channel == "JD_WEB" || bill.channel == "KUAIQIAN_WAP" || bill.channel == "KUAIQIAN_WEB" || bill.channel == "UN_WEB" || bill.channel == "UN_WAP")
            {
                if (responseData["result_code"].ToString() == "0")
                {
                    bill.id = responseData["id"].ToString();
                    if (BCCache.Instance.testMode)
                    {
                        bill.html = string.Format("<html><head></head><body><script>location.href='{0}'</script></body></html>", responseData["url"].ToString());
                    }
                    else
                    {
                        bill.html = responseData["html"].ToString();
                    }
                    return bill;
                }
                else
                {
                    var ex = new BCException(responseData["err_detail"].ToString());
                    throw ex;
                }
            }
            if (bill.channel == "BD_WEB" || bill.channel == "BD_WAP" || bill.channel == "YEE_WEB" || bill.channel == "YEE_WAP")
            {
                if (responseData["result_code"].ToString() == "0")
                {
                    bill.id = responseData["id"].ToString();
                    if (BCCache.Instance.testMode)
                    {
                        bill.html = string.Format("<html><head></head><body><script>location.href='{0}'</script></body></html>", responseData["url"].ToString());
                    }
                    else
                    {
                        bill.url = responseData["url"].ToString();
                    }
                   
                    return bill;
                }
                else
                {
                    var ex = new BCException(responseData["err_detail"].ToString());
                    throw ex;
                }
            }
            if (bill.channel == "BC_GATEWAY")
            {
                if (responseData["result_code"].ToString() == "0")
                {
                    bill.id = responseData["id"].ToString();
                    if (BCCache.Instance.testMode)
                    {
                        bill.html = string.Format("<html><head></head><body><script>location.href='{0}'</script></body></html>", responseData["url"].ToString());
                    }
                    else
                    {
                        bill.html = responseData["html"].ToString();
                    }
                    return bill;
                }
                else
                {
                    var ex = new BCException(responseData["err_detail"].ToString());
                    throw ex;
                }
            }
            if (bill.channel == "BC_EXPRESS")
            {
                if (responseData["result_code"].ToString() == "0")
                {
                    bill.id = responseData["id"].ToString();
                    if (BCCache.Instance.testMode)
                    {
                        bill.html = string.Format("<html><head></head><body><script>location.href='{0}'</script></body></html>", responseData["url"].ToString());
                    }
                    else
                    {
                        bill.html = responseData["html"].ToString();
                        bill.url = responseData["url"].ToString();
                    }
                    return bill;
                }
                else
                {
                    var ex = new BCException(responseData["err_detail"].ToString());
                    throw ex;
                }
            }
            if (bill.channel == "BC_NATIVE")
            {
                if (responseData["result_code"].ToString() == "0")
                {
                    bill.id = responseData["id"].ToString();
                    if (BCCache.Instance.testMode)
                    {
                        //bill.codeURL = responseData["url"].ToString();
                    }
                    else
                    {
                        bill.codeURL = responseData["code_url"].ToString();
                    }
                    return bill;
                }
                else
                {
                    var ex = new BCException(responseData["err_detail"].ToString());
                    throw ex;
                }

            }
            if (bill.channel == "BC_WX_WAP")
            {
                if (responseData["result_code"].ToString() == "0")
                {
                    bill.id = responseData["id"].ToString();
                    if (BCCache.Instance.testMode)
                    {
                        //bill.codeURL = responseData["url"].ToString();
                    }
                    else
                    {
                        bill.url = responseData["url"].ToString();
                    }
                    return bill;
                }
                else
                {
                    var ex = new BCException(responseData["err_detail"].ToString());
                    throw ex;
                }

            }
            if (bill.channel == "BC_WX_JSAPI")
            {
                if (BCCache.Instance.testMode)
                {
                    throw new BCException("微信公众号内支付不支持测试模式");
                }
                if (responseData["result_code"].ToString() == "0")
                {
                    bill.id = responseData["id"].ToString();
                    bill.appId = responseData["app_id"].ToString();
                    bill.package = responseData["package"].ToString();
                    bill.noncestr = responseData["nonce_str"].ToString();
                    bill.timestamp = responseData["timestamp"].ToString();
                    bill.paySign = responseData["pay_sign"].ToString();
                    bill.signType = responseData["sign_type"].ToString();

                    return bill;
                }
                else
                {
                    var ex = new BCException(responseData["err_detail"].ToString());
                    throw ex;
                }

            }
            return bill;
        }

        /// <summary>
        /// 支付
        /// </summary>
        /// <param name="channel">渠道类型
        ///     根据不同场景选择不同的支付方式
        ///     必填
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
        ///     UN_WAP:       银联wap支付
        ///     JD_WAP:       京东wap支付
        ///     JD_WEB:       京东web支付
        ///     YEE_WAP:      易宝wap支付 
        ///     YEE_WEB:      易宝web支付
        ///     KUAIQIAN_WAP: 快钱wap支付
        ///     KUAIQIAN_WEB: 快钱web支付
        ///     BD_WEB:       百度web支付
        ///     BD_WAP:       百度wap支付
        ///     BC_GATEWAY    比可网关支付
        ///     BC_EXPRESS    比可快捷支付（收款最低金额1元）
        /// </param>
        /// <param name="totalFee">订单总金额
        ///     只能为整数，单位为分
        ///     必填
        /// </param>
        /// <param name="billNo">商户订单号
        ///     32个字符内，数字和/或字母组合，确保在商户系统中唯一（即所有渠道所有订单号不同）
        ///     必填
        /// </param>
        /// <param name="title">订单标题
        ///     32个字节内，最长支持16个汉字
        ///     必填
        /// </param>
        /// <param name="optional">附加数据
        ///     用户自定义的参数，将会在webhook通知中原样返回，该字段主要用于商户携带订单的自定义数据
        ///     {"key1":"value1","key2":"value2",...}
        ///     可空
        /// </param>
        /// <param name="returnUrl">同步返回页面
        ///     支付渠道处理完请求后,当前页面自动跳转到商户网站里指定页面的http路径。
        ///     当channel 参数为 ALI_WEB 或 ALI_QRCODE 或 UN_WEB时为必填
        /// </param>
        /// <param name="billTimeout">订单失效时间
        ///     必须为非零正整数，单位为秒，建议最短失效时间间隔必须大于300秒
        ///     可空
        ///     京东系列支付不支持该参数，填空
        /// </param>
        /// <param name="openId">用户相对于微信公众号的唯一id
        ///     例如'0950c062-5e41-44e3-8f52-f89d8cf2b6eb'
        ///     微信公众号支付(WX_JSAPI)的必填参数
        /// </param>
        /// <param name="showURL">商品展示地址
        ///     以http://开头,例如'http://beecloud.cn'
        ///     支付宝网页支付(ALI_WEB)的选填参数
        /// </param>
        /// <param name="qrPayMode">二维码类型
        ///     支付宝内嵌二维码支付(ALI_QRCODE)的选填参数
        ///     二维码类型含义
        ///     0： 订单码-简约前置模式,对应 iframe 宽度不能小于 600px, 高度不能小于 300px
        ///     1： 订单码-前置模式,对应 iframe 宽度不能小于 300px, 高度不能小于 600px
        ///     3： 订单码-迷你前置模式,对应 iframe 宽度不能小于 75px, 高度不能小于 75px
        /// </param>
        /// <returns>
        /// </returns>
        public static BCBill BCPayByChannel(BCBill bill)
        {
            string payUrl = "";
            if (!BCCache.Instance.testMode)
            {
                payUrl = BCPrivateUtil.getHost() + BCConstants.version + BCConstants.billURL;
            }
            else
            {
                payUrl = BCPrivateUtil.getHost() + BCConstants.version + BCConstants.billTestURL;
            }
            

            string paraString = preparePayParameters(bill);

            try
            {
                HttpWebResponse response = BCPrivateUtil.CreatePostHttpResponse(payUrl, paraString, BCCache.Instance.networkTimeout);

                string respString = BCPrivateUtil.GetResponseString(response);

                return handlePayResult(respString, bill);
            }
            catch (Exception e)
            {
                var ex = new BCException(e.Message);
                throw ex;
            }
        }
        #endregion

        #region 微信支付宝扫码/被扫支付
        /// <summary>
        ///  微信支付宝扫码/被扫支付
        /// </summary>
        /// <param name="bill"></param>
        /// <returns></returns>
        public static BCBill BCOfflinePayByChannel(BCBill bill)
        {
            string payUrl = "";

            if (bill.channel.ToString().Contains("SCAN"))
            {
                payUrl = BCPrivateUtil.getHost() + BCConstants.version + BCConstants.offlineBillURL;
            }
            else
            {
                payUrl = BCPrivateUtil.getHost() + BCConstants.version + BCConstants.billURL;
            }
            

            long timestamp = BCUtil.GetTimeStamp(DateTime.Now);

            JsonData data = new JsonData();

            data["app_id"] = BCCache.Instance.appId;
            data["app_sign"] = BCPrivateUtil.getAppSignature(BCCache.Instance.appId, BCCache.Instance.appSecret, timestamp.ToString());
            data["timestamp"] = timestamp;
            data["channel"] = bill.channel;
            data["total_fee"] = bill.totalFee;
            data["bill_no"] = bill.billNo;
            data["title"] = bill.title;

            if (bill.authCode != null)
            {
                data["auth_code"] = bill.authCode;
            }
   
            if (bill.optional != null && bill.optional.Count > 0)
            {
                data["optional"] = new JsonData();
                foreach (string key in bill.optional.Keys)
                {
                    data["optional"][key] = bill.optional[key];
                }
            }

            string paraString = data.ToJson();

            try
            {
                HttpWebResponse response = BCPrivateUtil.CreatePostHttpResponse(payUrl, paraString, BCCache.Instance.networkTimeout);

                string respString = BCPrivateUtil.GetResponseString(response);

                JsonData responseData = JsonMapper.ToObject(respString);

                if (bill.channel == "BC_ALI_QRCODE")
                {
                    if (responseData["result_code"].ToString() == "0")
                    {
                        bill.id = responseData["id"].ToString();
                        bill.codeURL = responseData["code_url"].ToString();
                        return bill;
                    }
                    else
                    {
                        var ex = new BCException(responseData["err_detail"].ToString());
                        throw ex;
                    }
                }
                if (bill.channel == "BC_ALI_SCAN" || bill.channel == "BC_WX_SCAN")
                {
                    if (responseData["result_code"].ToString() == "0")
                    {
                        bill.id = responseData["id"].ToString();
                        bill.result = (bool)responseData["pay_result"];
                        return bill;
                    }
                    else
                    {
                        var ex = new BCException(responseData["err_detail"].ToString());
                        throw ex;
                    }
                }
                return bill;
            }
            catch (Exception e)
            {
                var ex = new BCException(e.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 支付宝微信扫码/被扫订单状态查询
        /// </summary>
        /// <param name="billNo"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        public static bool BCOfflineBillStatus(string billNo, string channel)
        {
            string statusUrl = BCPrivateUtil.getHost() + BCConstants.version + BCConstants.offlineStatusURL;

            long timestamp = BCUtil.GetTimeStamp(DateTime.Now);

            JsonData data = new JsonData();
            data["app_id"] = BCCache.Instance.appId;

            data["app_sign"] = BCPrivateUtil.getAppSignature(BCCache.Instance.appId, BCCache.Instance.appSecret, timestamp.ToString());
            data["timestamp"] = timestamp;
            data["bill_no"] = billNo;
            data["channel"] = channel;
            
            string paraString = data.ToJson();

            try
            {
                HttpWebResponse response = BCPrivateUtil.CreatePostHttpResponse(statusUrl, paraString, BCCache.Instance.networkTimeout);

                string respString = BCPrivateUtil.GetResponseString(response);

                JsonData responseData = JsonMapper.ToObject(respString);

                if (responseData["result_code"].ToString() == "0")
                {
                    return (bool)responseData["pay_result"];
                }
                else
                {
                    var ex = new BCException(responseData["err_detail"].ToString());
                    throw ex;
                }
            }
            catch (Exception e)
            {
                var ex = new BCException(e.Message);
                throw ex;
            }
        }
        #endregion

        #region （预）退款
        //准备退款参数
        public static string prepareRefundParameters(BCRefund refund)
        {
            long timestamp = BCUtil.GetTimeStamp(DateTime.Now);

            JsonData data = new JsonData();
            data["app_id"] = BCCache.Instance.appId;
            data["app_sign"] = BCPrivateUtil.getAppSignatureByMasterSecret(BCCache.Instance.appId, BCCache.Instance.masterSecret, timestamp.ToString());
            data["timestamp"] = timestamp;
            data["channel"] = refund.channel;
            data["refund_no"] = refund.refundNo;
            data["bill_no"] = refund.billNo;
            data["refund_fee"] = refund.refundFee;
            data["refund_account"] = refund.refundAccount;
            if (refund.optional != null && refund.optional.Count > 0)
            {
                data["optional"] = new JsonData();
                foreach (string key in refund.optional.Keys)
                {
                    data["optional"][key] = refund.optional[key];
                }
            }
            data["need_approval"] = refund.needApproval;
            string paraString = data.ToJson();
            return paraString;
        }

        //处理退款回调
        public static BCRefund handleRefundResult(string respString, BCRefund refund)
        {
            JsonData responseData = JsonMapper.ToObject(respString);
            if (responseData["result_code"].ToString() == "0")
            {
                refund.id = responseData["id"].ToString();
                try
                {
                    refund.url = responseData["url"].ToString();
                }
                catch
                {
                    //
                }
            }
            else
            {
                var ex = new BCException(responseData["err_detail"].ToString());
                throw ex;
            }
            return refund;
        }

        /// <summary>
        /// (预)退款
        /// </summary>
        /// <param name="channel">渠道类型   
        ///     选填
        ///     可以通过enum BCPay.RefundChannel获取
        ///     ALI:      支付宝
        ///     WX:       微信
        ///     UN:       银联
        ///     JD:       京东
        ///     YEE:      易宝
        ///     KUAIQIAN: 快钱
        ///     BD:       百度
        ///     注意：不传channel也能退款的前提是保证所有渠道所有订单号不同，如果出现两个订单号重复，会报错提示传入channel进行区分
        /// </param>
        /// <param name="refundNo">商户退款单号
        ///     格式为:退款日期(8位) + 流水号(3~24 位)。不可重复，且退款日期必须是当天日期。流水号可以接受数字或英文字符，建议使用数字，但不可接受“000”。
        ///     必填
        ///     例如：201506101035040000001
        /// </param>
        /// <param name="billNo">商户订单号
        ///     32个字符内，数字和/或字母组合，确保在商户系统中唯一
        ///     DIRECT_REFUND和PRE_REFUND时必填
        /// </param>
        /// <param name="refundFee">退款金额
        ///     只能为整数，单位为分
        ///     DIRECT_REFUND和PRE_REFUND时必填
        /// </param>
        /// <param name="refundAccount">
        ///     微信渠道退款资金来源
        ///     1:可用余额退款 
        ///     0:未结算资金退款（默认使用未结算资金退款）
        /// </param>
        /// <param name="optional">附加数据
        ///     用户自定义的参数，将会在webhook通知中原样返回，该字段主要用于商户携带订单的自定义数据
        ///     选填
        ///     {"key1":"value1","key2":"value2",...}
        /// </param>
        /// <param name="needApproval">是否为预退款
        ///     预退款needApproval值传true,直接退款传false
        ///     如果needApproval值传true，开发者需要调用审核退款接口或者直接去BeeCloud控制台的预退款界面审核退款方能最终退款
        /// </param>
        /// <returns>
        /// </returns>
        public static BCRefund BCRefundByChannel(BCRefund refund)
        {
            Random random = new Random();
            string refundUrl = BCPrivateUtil.getHost() + BCConstants.version + BCConstants.refundURL;
            string paraString = prepareRefundParameters(refund);

            try
            {
                HttpWebResponse response = BCPrivateUtil.CreatePostHttpResponse(refundUrl, paraString, BCCache.Instance.networkTimeout);
                string respString = BCPrivateUtil.GetResponseString(response);
                return handleRefundResult(respString, refund);
                
            }
            catch(Exception e)
            {
                var ex = new BCException(e.Message);
                throw ex;
            }            
        }
        #endregion

        #region 退款审核
        //准备退款审核参数
        public static string prepareApproveRefundParameters(string channel, List<string> ids, bool agree, string denyReason)
        {
            long timestamp = BCUtil.GetTimeStamp(DateTime.Now);

            JsonData data = new JsonData();
            data["app_id"] = BCCache.Instance.appId;
            data["app_sign"] = BCPrivateUtil.getAppSignatureByMasterSecret(BCCache.Instance.appId, BCCache.Instance.masterSecret, timestamp.ToString());
            data["timestamp"] = timestamp;
            data["channel"] = channel;
            data["ids"] = JsonMapper.ToObject(JsonMapper.ToJson(ids));
            data["agree"] = agree;
            data["denyReason"] = denyReason;

            string paraString = data.ToJson();
            return paraString;
        }

        //处理退款审核回调
        public static BCApproveRefundResult handleApproveRefundResult(string respString, string channel)
        {
            JsonData responseData = JsonMapper.ToObject(respString);
            BCApproveRefundResult result = new BCApproveRefundResult();
            if (responseData["result_code"].ToString() == "0")
            {
                try
                {
                    result.url = responseData["url"].ToString();
                }
                catch
                {
                    //
                }
                result.status = JsonMapper.ToObject<Dictionary<string, string>>(responseData["result_map"].ToJson().ToString());
            }
            else
            {
                var ex = new BCException(responseData["err_detail"].ToString());
                throw ex;
            }
            return result;
        }

        /// <summary>
        ///  预退款(批量)审核
        /// </summary>
        /// <param name="channel">渠道类型
        ///     根据不同渠道选不同的值
        ///     必填
        ///     可以通过enum BCPay.RefundChannel获取
        ///     ALI:      支付宝
        ///     WX:       微信
        ///     UN:       银联
        ///     JD:       京东
        ///     YEE:      易宝
        ///     KUAIQIAN: 快钱
        ///     BD:       百度
        /// </param>
        /// <param name="ids">退款记录id列表
        ///     批量审核的退款记录的唯一标识符集合
        ///     必填
        /// </param>
        /// <param name="agree">同意或者驳回
        ///     批量驳回传false，批量同意传true
        ///     必填
        /// </param>
        /// <param name="denyReason">驳回理由
        ///     可空
        /// </param>
        /// <returns>
        ///     参考BCApproveRefundResult
        /// </returns>
        public static BCApproveRefundResult BCApproveRefund(string channel, List<string> ids, bool agree, string denyReason)
        {
            Random random = new Random();
            string approveRefundUrl = BCPrivateUtil.getHost() + BCConstants.version + BCConstants.refundURL;

            string paraString = prepareApproveRefundParameters(channel, ids, agree, denyReason);

            try
            {
                HttpWebResponse response = BCPrivateUtil.CreatePutHttpResponse(approveRefundUrl, paraString, BCCache.Instance.networkTimeout);
                string respString = BCPrivateUtil.GetResponseString(response);
                return handleApproveRefundResult(respString, channel);
            }
            catch (Exception e)
            {
                var ex = new BCException(e.Message);
                throw ex;
            } 
        }
        #endregion

        #region 查询
        ///准备订单查询参数
        public static string preparePayQueryByConditionParameters(BCQueryBillParameter para)
        {
            long timestamp = BCUtil.GetTimeStamp(DateTime.Now);

            JsonData data = new JsonData();
            data["app_id"] = BCCache.Instance.appId;
            if (!BCCache.Instance.testMode)
            {
                data["app_sign"] = BCPrivateUtil.getAppSignature(BCCache.Instance.appId, BCCache.Instance.appSecret, timestamp.ToString());
            }
            else
            {
                data["app_sign"] = BCPrivateUtil.getAppSignatureByTestSecret(timestamp.ToString());
            }
            data["timestamp"] = timestamp;
            data["channel"] = para.channel;
            data["bill_no"] = para.billNo;
            data["start_time"] = para.startTime;
            data["end_time"] = para.endTime;
            data["skip"] = para.skip;
            data["spay_result"] = para.result;
            data["need_detail"] = para.needDetail;
            data["limit"] = para.limit;

            string paraString = data.ToJson();
            return paraString;
        }

        //处理订单条件查询回调
        public static List<BCBill> handlePayQueryByConditionResult(string respString, bool? needDetail)
        {
            JsonData responseData = JsonMapper.ToObject(respString);
            List<BCBill> bills = new List<BCBill>();
            if (responseData["result_code"].ToString() == "0")
            {
                if (responseData["bills"].IsArray)
                {
                    foreach (JsonData billData in responseData["bills"])
                    {
                        BCBill bill = new BCBill();
                        bill.id = billData["id"].ToString();
                        bill.title = billData["title"].ToString();
                        bill.totalFee = int.Parse(billData["total_fee"].ToString());
                        bill.createdTime = BCUtil.GetDateTime((long)billData["create_time"]);
                        bill.billNo = billData["bill_no"].ToString();
                        bill.result = (bool)billData["spay_result"];
                        bill.channel = billData["sub_channel"].ToString();
                        bill.tradeNo = billData["trade_no"].ToString();
                        bill.optional = JsonMapper.ToObject<Dictionary<string, string>>(billData["optional"].ToString());
                        if (needDetail == true)
                        {
                            bill.messageDetail = billData["message_detail"].ToString();
                        }
                        bill.revertResult = (bool)billData["revert_result"];
                        bill.refundResult = (bool)billData["refund_result"];

                        bill.billFee = int.Parse(billData["bill_fee"].ToString());
                        bill.discount = int.Parse(billData["discount"].ToString());
                        if (billData["coupon_id"] != null)
                        {
                            bill.couponID = billData["coupon_id"].ToString();
                        }

                        bills.Add(bill);
                    }
                }
            }
            else
            {
                var ex = new BCException(responseData["err_detail"].ToString());
                throw ex;
            }

            return bills;
        }

        //处理订单/退款单数量
        public static int handleQueryCountResult(string respString)
        {
            JsonData responseData = JsonMapper.ToObject(respString);
            if (responseData["result_code"].ToString() == "0")
            {
                if (responseData["count"].IsInt)
                {
                    return int.Parse(responseData["count"].ToString());
                }
                else
                {
                    var ex = new BCException("服务出错啦:-(");
                    throw ex;
                }
            }
            else
            {
                var ex = new BCException(responseData["err_detail"].ToString());
                throw ex;
            }
        }

        //处理订单Id查询回调
        public static BCBill handlePayQueryByIdResult(string respString)
        {
            JsonData responseData = JsonMapper.ToObject(respString);
            BCBill bill = new BCBill();
            if (responseData["result_code"].ToString() == "0")
            {
                JsonData billData = responseData["pay"];
                bill.id = billData["id"].ToString();
                bill.title = billData["title"].ToString();
                bill.totalFee = int.Parse(billData["total_fee"].ToString());
                bill.createdTime = BCUtil.GetDateTime((long)billData["create_time"]);
                bill.billNo = billData["bill_no"].ToString();
                bill.result = (bool)billData["spay_result"];
                bill.channel = billData["sub_channel"].ToString();
                bill.tradeNo = billData["trade_no"].ToString();
                bill.optional = JsonMapper.ToObject<Dictionary<string, string>>(billData["optional"].ToString());
                bill.messageDetail = billData["message_detail"].ToString();
                bill.revertResult = (bool)billData["revert_result"];
                bill.refundResult = (bool)billData["refund_result"];

                bill.billFee = int.Parse(billData["bill_fee"].ToString());
                bill.discount = int.Parse(billData["discount"].ToString());
                if (billData["coupon_id"] != null)
                {
                    bill.couponID = billData["coupon_id"].ToString();
                }
            }
            else
            {
                var ex = new BCException(responseData["err_detail"].ToString());
                throw ex;
            }

            return bill;
        }

        //准备订单/退款id查询参数
        public static string prepareQueryByIdParameters(string id)
        {
            long timestamp = BCUtil.GetTimeStamp(DateTime.Now);

            JsonData data = new JsonData();
            data["app_id"] = BCCache.Instance.appId;
            if (!BCCache.Instance.testMode)
            {
                data["app_sign"] = BCPrivateUtil.getAppSignature(BCCache.Instance.appId, BCCache.Instance.appSecret, timestamp.ToString());
            }
            else
            {
                data["app_sign"] = BCPrivateUtil.getAppSignatureByTestSecret(timestamp.ToString());
            }
            data["timestamp"] = timestamp;

            string paraString = data.ToJson();
            return paraString;
        }

        //准备退款查询参数
        public static string prepareRefundQueryByConditionParameters(BCQueryRefundParameter para)
        {
            long timestamp = BCUtil.GetTimeStamp(DateTime.Now);

            JsonData data = new JsonData();
            data["app_id"] = BCCache.Instance.appId;
            data["app_sign"] = BCPrivateUtil.getAppSignature(BCCache.Instance.appId, BCCache.Instance.appSecret, timestamp.ToString());
            data["timestamp"] = timestamp;
            data["channel"] = para.channel;
            data["bill_no"] = para.billNo;
            data["refund_no"] = para.refundNo;
            data["start_time"] = para.startTime;
            data["end_time"] = para.endTime;
            data["need_approval"] = para.needApproval;
            data["need_detail"] = para.needDetail;
            data["skip"] = para.skip;
            data["limit"] = para.limit;

            string paraString = data.ToJson();
            return paraString;
        }

        //处理退款条件查询回调
        public static List<BCRefund> handleRefundQueryByConditionResult(string respString, bool? needDetail)
        {
            JsonData responseData = JsonMapper.ToObject(respString);
            List<BCRefund> refunds = new List<BCRefund>();
            if (responseData["result_code"].ToString() == "0")
            {
                if (responseData["refunds"].IsArray)
                {
                    foreach (JsonData refundData in responseData["refunds"])
                    {
                        BCRefund refund = new BCRefund();
                        refund.id = refundData["id"].ToString();
                        refund.title = refundData["title"].ToString();
                        refund.billNo = refundData["bill_no"].ToString();
                        refund.refundNo = refundData["refund_no"].ToString();
                        refund.totalFee = int.Parse(refundData["total_fee"].ToString());
                        refund.refundFee = int.Parse(refundData["refund_fee"].ToString());
                        refund.channel = refundData["channel"].ToString();
                        refund.finish = (bool)refundData["finish"];
                        refund.result = (bool)refundData["result"];
                        refund.optional = JsonMapper.ToObject<Dictionary<string, string>>(refundData["optional"].ToString());
                        if (needDetail == true)
                        {
                            refund.messageDetail = refundData["message_detail"].ToString();
                        }
                        refund.createdTime = BCUtil.GetDateTime((long)refundData["create_time"]);
                        refunds.Add(refund);
                    }
                }
            }
            else
            {
                var ex = new BCException(responseData["err_detail"].ToString());
                throw ex;
            }

            return refunds;
        }

        //处理退款Id查询回调
        public static BCRefund handleRefundQueryByIdResult(string respString)
        {
            JsonData responseData = JsonMapper.ToObject(respString);
            BCRefund refund = new BCRefund();
            if (responseData["result_code"].ToString() == "0")
            {
                JsonData refundData = responseData["refund"];
                refund.id = refundData["id"].ToString();
                refund.title = refundData["title"].ToString();
                refund.billNo = refundData["bill_no"].ToString();
                refund.refundNo = refundData["refund_no"].ToString();
                refund.totalFee = int.Parse(refundData["total_fee"].ToString());
                refund.refundFee = int.Parse(refundData["refund_fee"].ToString());
                refund.channel = refundData["channel"].ToString();
                refund.finish = (bool)refundData["finish"];
                refund.result = (bool)refundData["result"];
                refund.optional = JsonMapper.ToObject<Dictionary<string, string>>(refundData["optional"].ToString());
                refund.messageDetail = refundData["message_detail"].ToString();
                refund.createdTime = BCUtil.GetDateTime((long)refundData["create_time"]);
            }
            else
            {
                var ex = new BCException(responseData["err_detail"].ToString());
                throw ex;
            }

            return refund;
        }

        //准备退款状态查询参数
        public static string prepareRefundStatusQueryParameters(string channel, string refundNo)
        {
            long timestamp = BCUtil.GetTimeStamp(DateTime.Now);

            JsonData data = new JsonData();
            data["app_id"] = BCCache.Instance.appId;
            data["app_sign"] = BCPrivateUtil.getAppSignature(BCCache.Instance.appId, BCCache.Instance.appSecret, timestamp.ToString());
            data["timestamp"] = timestamp;
            data["channel"] = channel;
            data["refund_no"] = refundNo;

            string paraString = data.ToJson();
            return paraString;
        }

        //处理退款状态查询回调
        public static string handleRefundStatusQueryResult(string respString)
        {
            JsonData responseData = JsonMapper.ToObject(respString);
            string refundStatus = "";
            if (responseData["result_code"].ToString() == "0")
            {
                refundStatus = responseData["refund_status"].ToString();
            }
            else
            {
                var ex = new BCException(responseData["err_detail"].ToString());
                throw ex;
            }
            return refundStatus;
        }

        /// <summary>
        /// 支付订单查询
        /// </summary>
        /// <param name="channel">渠道类型
        ///     选填
        ///     可以通过enum BCPay.QueryChannel获取
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
        ///     注意：不传channel也能查询的前提是保证所有渠道所有订单号不同，如果出现两个订单号重复，会报错提示传入channel进行区分
        /// </param>
        /// <param name="billNo">商户订单号
        /// </param>
        /// <param name="startTime">起始时间
        ///     毫秒时间戳, 13位, 可以使用BCUtil.GetTimeStamp()方法获取
        ///     选填
        /// </param>
        /// <param name="endTime">结束时间
        ///     毫秒时间戳, 13位, 可以使用BCUtil.GetTimeStamp()方法获取
        ///     选填
        /// </param>
        /// <param name="spayResult">订单状态
        ///     订单是否成功，null为全部返回，true只返回成功订单，false只返回失败订单
        ///     选填
        /// </param>
        /// <param name="needDetail">是否需要返回渠道详细信息
        ///     决定是否需要返回渠道的回调信息，true为需要
        ///     选填
        /// </param>
        /// <param name="skip">查询起始位置
        ///     默认为0。设置为10表示忽略满足条件的前10条数据
        ///     选填
        /// </param>
        /// <param name="limit">查询的条数
        ///     默认为10，最大为50。设置为10表示只返回满足条件的10条数据
        ///     选填
        /// </param>
        /// <returns></returns>
        public static List<BCBill> BCPayQueryByCondition(BCQueryBillParameter para)
        {
            Random random = new Random();
            string payQueryUrl = "";
            if (!BCCache.Instance.testMode)
            {
                payQueryUrl = BCPrivateUtil.getHost() + BCConstants.version + BCConstants.billsURL;
            }
            else
            {
                payQueryUrl = BCPrivateUtil.getHost() + BCConstants.version + BCConstants.billsTestURL;
            }
            

            string paraString = preparePayQueryByConditionParameters(para);

            try
            {
                string url = payQueryUrl + "?para=" + HttpUtility.UrlEncode(paraString, Encoding.UTF8);
                HttpWebResponse response = BCPrivateUtil.CreateGetHttpResponse(url, BCCache.Instance.networkTimeout);
                string respString = BCPrivateUtil.GetResponseString(response);
                return handlePayQueryByConditionResult(respString, para.needDetail);
            }
            catch(Exception e)
            {
                var ex = new BCException(e.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 获得订单笔数，配合BCPayQueryByCondition使用，使用查询订单时一样的参数
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public static int BCPayQueryCount(BCQueryBillParameter para)
        {
            string payQueryUrl = BCPrivateUtil.getHost() + BCConstants.version + BCConstants.billsCountURL;

            string paraString = preparePayQueryByConditionParameters(para);

            try
            {
                string url = payQueryUrl + "?para=" + HttpUtility.UrlEncode(paraString, Encoding.UTF8);
                HttpWebResponse response = BCPrivateUtil.CreateGetHttpResponse(url, BCCache.Instance.networkTimeout);
                string respString = BCPrivateUtil.GetResponseString(response);
                return handleQueryCountResult(respString);
            }
            catch (Exception e)
            {
                var ex = new BCException(e.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 支付订单查询(指定ID)
        /// </summary>
        /// <param name="id">订单id</param>
        /// <returns></returns>
        public static BCBill BCPayQueryById(string id)
        {
            Random random = new Random();
            string payQueryUrl = "";
            if (!BCCache.Instance.testMode)
            {
                payQueryUrl = BCPrivateUtil.getHost() + BCConstants.version + BCConstants.billURL + "/" + id;
            }
            else
            {
                payQueryUrl = BCPrivateUtil.getHost() + BCConstants.version + BCConstants.billTestURL + "/" + id;
            }
            

            string paraString = prepareQueryByIdParameters(id);

            try
            {
                string url = payQueryUrl + "?para=" + HttpUtility.UrlEncode(paraString, Encoding.UTF8);
                HttpWebResponse response = BCPrivateUtil.CreateGetHttpResponse(url, BCCache.Instance.networkTimeout);
                string respString = BCPrivateUtil.GetResponseString(response);
                return handlePayQueryByIdResult(respString);
            }
            catch (Exception e)
            {
                var ex = new BCException(e.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 退款订单查询
        /// </summary>
        /// <param name="channel">渠道类型
        ///     根据不同场景选择不同的支付方式
        ///     必填
        ///     可以通过enum BCPay.QueryChannel获取
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
        ///     注意：不传channel也能查询的前提是保证所有渠道所有订单号不同，如果出现两个订单号重复，会报错提示传入channel进行区分
        /// </param>
        /// <param name="billNo">商户订单号
        /// </param>
        /// <param name="refundNo">商户退款单号
        /// </param>
        /// <param name="startTime">起始时间
        ///     毫秒时间戳, 13位, 可以使用BCUtil.GetTimeStamp()方法获取
        ///     选填</param>
        /// <param name="endTime">结束时间
        ///     毫秒时间戳, 13位, 可以使用BCUtil.GetTimeStamp()方法获取
        ///     选填
        /// </param>
        /// <param name="needApproval">需要审核     
        ///     标识退款记录是否为预退款
        ///     选填
        /// </param>
        /// <param name="needDetail">是否需要返回渠道详细信息
        ///     决定是否需要返回渠道的回调信息，true为需要
        ///     选填
        /// </param>
        /// <param name="skip">查询起始位置
        ///     默认为0。设置为10表示忽略满足条件的前10条数据
        ///     选填
        /// </param>
        /// <param name="limit">查询的条数
        ///     默认为10，最大为50。设置为10表示只返回满足条件的10条数据
        ///     选填
        /// </param>
        /// <returns>
        /// </returns>
        public static List<BCRefund> BCRefundQueryByCondition(BCQueryRefundParameter para)
        {
            Random random = new Random();
            string payQueryUrl = BCPrivateUtil.getHost() + BCConstants.version + BCConstants.refundsURL;

            string paraString = prepareRefundQueryByConditionParameters(para);

            try
            {
                string url = payQueryUrl + "?para=" + HttpUtility.UrlEncode(paraString, Encoding.UTF8);
                HttpWebResponse response = BCPrivateUtil.CreateGetHttpResponse(url, BCCache.Instance.networkTimeout);
                string respString = BCPrivateUtil.GetResponseString(response);
                return handleRefundQueryByConditionResult(respString, para.needDetail);
            }
            catch (Exception e)
            {
                var ex = new BCException(e.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 获得退款笔数，配合BCRefundQueryByCondition使用，使用查询退款时一样的参数
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public static int BCRefundQueryCount(BCQueryRefundParameter para)
        {
            string payQueryUrl = BCPrivateUtil.getHost() + BCConstants.version + BCConstants.refundsCountURL;

            string paraString = prepareRefundQueryByConditionParameters(para);

            try
            {
                string url = payQueryUrl + "?para=" + HttpUtility.UrlEncode(paraString, Encoding.UTF8);
                HttpWebResponse response = BCPrivateUtil.CreateGetHttpResponse(url, BCCache.Instance.networkTimeout);
                string respString = BCPrivateUtil.GetResponseString(response);
                return handleQueryCountResult(respString);
            }
            catch (Exception e)
            {
                var ex = new BCException(e.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 退款订单查询(指定ID)
        /// </summary>
        /// <param name="id">退款记录的唯一标识，可用于查询单笔记录</param>
        /// <returns></returns>
        public static BCRefund BCRefundQueryById(string id)
        {
            Random random = new Random();
            string payQueryUrl = BCPrivateUtil.getHost() + BCConstants.version + BCConstants.refundURL + "/" + id;

            string paraString = prepareQueryByIdParameters(id);

            try
            {
                string url = payQueryUrl + "?para=" + HttpUtility.UrlEncode(paraString, Encoding.UTF8);
                HttpWebResponse response = BCPrivateUtil.CreateGetHttpResponse(url, BCCache.Instance.networkTimeout);
                string respString = BCPrivateUtil.GetResponseString(response);
                return handleRefundQueryByIdResult(respString);
            }
            catch (Exception e)
            {
                var ex = new BCException(e.Message);
                throw ex;
            }
        }

        /// <summary>
        ///退款状态查询"
        /// </summary>
        /// <param name="channel">渠道类型
        ///     只有WX、YEE、KUAIQIAN、BD需要
        /// </param>
        /// <param name="refundNo">商户退款单号
        /// </param>
        /// <returns>
        /// </returns>
        public static string BCRefundStatusQuery(string channel, string refundNo)
        {
            Random random = new Random();
            string refundStatusUrl = BCPrivateUtil.getHost() + BCConstants.version + BCConstants.refundStatusURL;

            string paraString = prepareRefundStatusQueryParameters(channel, refundNo);
            
            string url = refundStatusUrl + "?para=" + HttpUtility.UrlEncode(paraString, Encoding.UTF8);
            try
            {
                HttpWebResponse response = BCPrivateUtil.CreateGetHttpResponse(url, BCCache.Instance.networkTimeout);
                string respString = BCPrivateUtil.GetResponseString(response);
                return handleRefundStatusQueryResult(respString);
            }
            catch (Exception e)
            {
                var ex = new BCException(e.Message);
                throw ex;
            }
        }
        #endregion

        #region 微信/支付宝打款
        //准备单笔单款参数
        public static string prepareTransferParameters(BCTransferParameter para)
        {
            if (BCCache.Instance.masterSecret == null)
            {
                var ex = new BCException("masterSecret未注册, 请查看registerApp方法");
                throw ex;
            }

            long timestamp = BCUtil.GetTimeStamp(DateTime.Now);

            JsonData data = new JsonData();
            data["app_id"] = BCCache.Instance.appId;
            data["app_sign"] = BCPrivateUtil.getAppSignatureByMasterSecret(BCCache.Instance.appId, BCCache.Instance.masterSecret, timestamp.ToString());
            data["timestamp"] = timestamp;
            data["channel"] = para.channel;
            data["transfer_no"] = para.transferNo;
            data["total_fee"] = para.totalFee;
            data["desc"] = para.desc;
            data["channel_user_id"] = para.channelUserId;
            data["channel_user_name"] = para.channelUserName;
            data["account_name"] = para.accountName;
            if (para.info != null)
            {
                data["redpack_info"] = new JsonData();
                data["redpack_info"]["send_name"] = para.info.sendName;
                data["redpack_info"]["wishing"] = para.info.wishing;
                data["redpack_info"]["act_name"] = para.info.actName;
            }

            string paraString = data.ToJson();
            return paraString;
        }

        //准备批量打款参数
        public static string prepareTransfersParameters(BCTransfersParameter para)
        {
            if (BCCache.Instance.masterSecret == null)
            {
                var ex = new BCException("masterSecret未注册, 请查看registerApp方法");
                throw ex;
            }

            long timestamp = BCUtil.GetTimeStamp(DateTime.Now);

            JsonData data = new JsonData();
            data["app_id"] = BCCache.Instance.appId;
            data["app_sign"] = BCPrivateUtil.getAppSignatureByMasterSecret(BCCache.Instance.appId, BCCache.Instance.masterSecret, timestamp.ToString());
            data["timestamp"] = timestamp;
            data["channel"] = para.channel;
            data["batch_no"] = para.batchNo;
            data["account_name"] = para.accountName;
            JsonData list = new JsonData();
            foreach (var transfer in para.transfersData)
            {
                JsonData d = new JsonData();
                d["transfer_id"] = transfer.transferId;
                d["receiver_account"] = transfer.receiverAccount;
                d["receiver_name"] = transfer.receiverName;
                d["transfer_fee"] = transfer.transferFee;
                d["transfer_note"] = transfer.transferNote;
                list.Add(d);
            }
            data["transfer_data"] = list;
            string paraString = data.ToJson();
            return paraString;
        }

        //处理(批量)打款回调
        public static string handleTransfersResult(string respString, string channel)
        {
            JsonData responseData = JsonMapper.ToObject(respString);
            string result = "";
            if (responseData["result_code"].ToString() == "0")
            {
                if (channel.Contains("ALI"))
                {
                    result = responseData["url"].ToString();
                }
            }
            else
            {
                var ex = new BCException(responseData["err_detail"].ToString());
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// 批量打款
        /// </summary>
        /// <param name="channel">渠道
        ///     必填
        ///     现在只支持支付宝（TransferChannel.ALI_TRANSFER）</param>
        /// <param name="batchNo">批量付款批号
        ///     必填
        ///     此次批量付款的唯一标示，11-32位数字字母组合
        /// </param>
        /// <param name="accountName">付款方的支付宝账户名
        ///     必填
        /// </param>
        /// <param name="transferData">付款的详细数据
        ///     必填
        ///     每一个Map对应一笔付款的详细数据, list size 小于等于 1000。
        ///     具体参BCTransferData类
        /// </param>
        /// <returns>
        ///     如果channel类型是TRANSFER_CHANNEL.ALI_TRANSFER, 返回需要跳转支付的url, 否则返回空字符串
        /// </returns>
        public static string BCTransfers(BCTransfersParameter para)
        {
            string transferUrl = BCPrivateUtil.getHost() + BCConstants.version + BCConstants.transfersURL;

            string paraString = prepareTransfersParameters(para);

            try
            {
                HttpWebResponse response = BCPrivateUtil.CreatePostHttpResponse(transferUrl, paraString, BCCache.Instance.networkTimeout);
                string respString = BCPrivateUtil.GetResponseString(response);
                return handleTransfersResult(respString, para.channel);
            }
            catch (Exception e)
            {
                var ex = new BCException(e.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 打款
        /// </summary>
        /// <param name="channel">渠道类型
        ///     WX_REDPACK 微信红包, 
        ///     WX_TRANSFER 微信企业打款, 
        ///     ALI_TRANSFER 支付宝企业打款
        /// </param>
        /// <param name="transferNo">打款单号
        ///     支付宝为11-32位数字字母组合， 微信为10位数字
        /// </param>
        /// <param name="totalFee">打款金额
        ///     此次打款的金额,单位分,正整数(微信红包1.00-200元，微信打款>=1元)
        /// </param>
        /// <param name="desc">打款说明
        ///     此次打款的说明
        /// </param>
        /// <param name="channelUserId">用户id
        ///     支付渠道方内收款人的标示, 微信为openid, 支付宝为支付宝账户
        /// </param>
        /// <param name="channelUserName">用户名
        ///     支付渠道内收款人账户名， 支付宝必填
        /// </param>
        /// <param name="info">红包信息
        ///     查看BCRedPackInfo
        /// </param>
        /// <param name="account_name">打款方账号名称
        ///     打款方账号名全称，支付宝必填
        /// </param>
        /// <returns>
        ///     批量打款跳转支付url
        /// </returns>
        public static string BCTransfer(BCTransferParameter para)
        {
            Random random = new Random();
            string transferUrl = BCPrivateUtil.getHost() + BCConstants.version + BCConstants.transferURL;

            string paraString = prepareTransferParameters(para);

            try
            {
                HttpWebResponse response = BCPrivateUtil.CreatePostHttpResponse(transferUrl, paraString, BCCache.Instance.networkTimeout);
                string respString = BCPrivateUtil.GetResponseString(response);
                return handleTransfersResult(respString, para.channel);
            }
            catch (Exception e)
            {
                var ex = new BCException(e.Message);
                throw ex;
            }
        }
        #endregion

        #region 境外支付
        //准备境外支付参数
        public static string prepareInternationalPayParameters(BCInternationlBill bill)
        {
            long timestamp = BCUtil.GetTimeStamp(DateTime.Now);

            JsonData data = new JsonData();
            data["app_id"] = BCCache.Instance.appId;
            data["app_sign"] = BCPrivateUtil.getAppSignature(BCCache.Instance.appId, BCCache.Instance.appSecret, timestamp.ToString());
            data["timestamp"] = timestamp;
            data["channel"] = bill.channel;
            data["total_fee"] = bill.totalFee;
            data["bill_no"] = bill.billNo;
            data["title"] = bill.title;
            data["currency"] = bill.currency;
            if (bill.info != null)
            {
                data["credit_card_info"] = JsonMapper.ToObject(JsonMapper.ToJson(bill.info));
            }
            if (bill.creditCardId != null)
            {
                data["credit_card_id"] = bill.creditCardId;
            }
            if (bill.returnUrl != null)
            {
                data["return_url"] = bill.returnUrl;
            }

            string paraString = data.ToJson();
            return paraString;
        }

        //处理境外支付回调
        public static BCInternationlBill handleInternationalPayResult(string respString, BCInternationlBill bill)
        {
            JsonData responseData = JsonMapper.ToObject(respString);
            if (responseData["result_code"].ToString() == "0")
            {
                if (bill.channel == "PAYPAL_PAYPAL")
                {
                    bill.url = responseData["url"].ToString();
                }
                if (bill.channel == "PAYPAL_CREDITCARD")
                {
                    bill.creditCardId = responseData["credit_card_id"].ToString();
                }
            }
            else
            {
                var ex = new BCException(responseData["err_detail"].ToString());
                throw ex;
            }
            return bill;
        }

        /// <summary>
        /// 境外支付
        /// </summary>
        /// <param name="channel">渠道类型
        ///     enum InternationalPay提供了三个境外支付渠道类型，分别是：
        ///     PAYPAL_PAYPAL ： 跳转到paypal使用paypal内支付
        ///     PAYPAL_CREDITCARD ： 直接使用信用卡支付（paypal渠道）
        ///     PAYPAL_SAVED_CREDITCARD ： 使用存储的行用卡id支付（信用卡信息存储在PAYPAL)
        /// </param>
        /// <param name="totalFee">订单总金额
        ///     只能为整数，单位为分
        ///     必填
        /// </param>
        /// <param name="billNo">商户订单号
        ///     32个字符内，数字和/或字母组合，确保在商户系统中唯一（即所有渠道所有订单号不同）
        ///     必填
        /// </param>
        /// <param name="title">订单标题
        ///     32个字节内，最长支持16个汉字
        ///     必填
        /// </param>
        /// <param name="currency">三位货币种类代码
        ///     必填
        ///     类型如下：
        ///         Australian dollar	AUD
        ///         Brazilian real**	BRL
        ///         Canadian dollar	    CAD
        ///         Czech koruna	    CZK
        ///         Danish krone	    DKK
        ///         Euro	            EUR
        ///         Hong Kong dollar	HKD
        ///         Hungarian forint	HUF
        ///         Israeli new shekel	ILS
        ///         Japanese yen	    JPY
        ///         Malaysian ringgit	MYR
        ///         Mexican peso	    MXN
        ///         New Taiwan dollar	TWD
        ///         New Zealand dollar	NZD
        ///         Norwegian krone	    NOK
        ///         Philippine peso	    PHP
        ///         Polish złoty	    PLN
        ///         Pound sterling	    GBP
        ///         Singapore dollar	SGD
        ///         Swedish krona	    SEK
        ///         Swiss franc	        CHF
        ///         Thai baht	        THB
        ///         Turkish lira	    TRY
        ///         United States dollar	USD
        /// </param>
        /// <param name="info">信用卡信息
        ///     具体查看BCCreditCardInfo类
        ///     当channel 为PAYPAL_CREDITCARD必填
        /// </param>
        /// <param name="creditCardId">
        ///     当使用PAYPAL_CREDITCARD支付完成后会返回一个credit_card_id，商家可以存储这个id方便下次通过这个id发起支付无需再输入卡面信息
        /// </param>
        /// <param name="returnUrl">同步返回页面
        ///     支付渠道处理完请求后,当前页面自动跳转到商户网站里指定页面的http路径。
        ///     当channel参数为PAYPAL_PAYPAL时为必填
        /// </param>
        /// <returns></returns>
        public static BCInternationlBill BCInternationalPay(BCInternationlBill bill)
        {
            Random random = new Random();
            string payUrl = BCPrivateUtil.getHost() + BCConstants.version + BCConstants.internationalURL;

            string paraString = prepareInternationalPayParameters(bill);

            try
            {
                HttpWebResponse response = BCPrivateUtil.CreatePostHttpResponse(payUrl, paraString, BCCache.Instance.networkTimeout);
                string respString = BCPrivateUtil.GetResponseString(response);
                return handleInternationalPayResult(respString, bill);                
            }
            catch (Exception e)
            {
                var ex = new BCException(e.Message);
                throw ex;
            }
        }
        #endregion

        #region BeeCloud企业打款
        //准备代付参数
        public static string prepareBCTransferWithBankCard(BCTransferWithBackCard transfer)
        {
            long timestamp = BCUtil.GetTimeStamp(DateTime.Now);

            JsonData data = new JsonData();
            data["app_id"] = BCCache.Instance.appId;
            data["app_sign"] = BCPrivateUtil.getAppSignature(BCCache.Instance.appId, BCCache.Instance.appSecret, timestamp.ToString());
            data["timestamp"] = timestamp;

            data["total_fee"] = transfer.totalFee;
            data["bill_no"] = transfer.billNo;
            data["title"] = transfer.title;
            data["trade_source"] = transfer.tradeSource;
            data["bank_fullname"] = transfer.bankFullName;
            data["card_type"] = transfer.cardType;
            data["account_type"] = transfer.accountType;
            data["account_no"] = transfer.accountNo;
            data["account_name"] = transfer.accountName;
            data["mobile"] = transfer.mobile;

            if (transfer.optional != null && transfer.optional.Count > 0)
            {
                data["optional"] = new JsonData();
                foreach (string key in transfer.optional.Keys)
                {
                    data["optional"][key] = transfer.optional[key];
                }
            }

            string paraString = data.ToJson();
            return paraString;
        }

        //处理代付回调
        public static BCTransferWithBackCard handleBCTransferWithBankCardResult(string respString, BCTransferWithBackCard transfer)
        {
            JsonData responseData = JsonMapper.ToObject(respString);

            if (responseData["result_code"].ToString() == "0")
            {
                transfer.id = responseData["id"].ToString();
                return transfer;
            }
            else
            {
                var ex = new BCException(responseData["err_detail"].ToString());
                throw ex;
            }
        }

        /// <summary>
        /// 获取BeeCloud企业打款支持的银行全称列表
        /// </summary>
        /// <param name="type">业务类型：
        ///     P_DE:对私借记卡,
        ///     P_CR:对私信用卡,
        ///     C:对公账户
        /// </param>
        /// <returns></returns>
        public static BankList getBankFullNames(string type)
        {
            string transferUrl = BCPrivateUtil.getHost() + BCConstants.version + BCConstants.bctransferBanks;
            JsonData data = new JsonData();
            data["type"] = type;
            string paraString = data.ToJson();

            try
            {
                string url = transferUrl + "?para=" + HttpUtility.UrlEncode(paraString, Encoding.UTF8);
                HttpWebResponse response = BCPrivateUtil.CreateGetHttpResponse(url, BCCache.Instance.networkTimeout);
                string respString = BCPrivateUtil.GetResponseString(response);
                JsonData responseData = JsonMapper.ToObject(respString);

                if (responseData["result_code"].ToString() == "0")
                {
                    BankList backlist = new BankList();
                    backlist.size = int.Parse(responseData["size"].ToString());
                    backlist.bankList = JsonMapper.ToObject<List<string>>(responseData["bank_list"].ToJson());
                    return backlist;
                }
                else
                {
                    var ex = new BCException(responseData["err_detail"].ToString());
                    throw ex;
                }
            }
            catch (Exception e)
            {
                var ex = new BCException(e.Message);
                throw ex;
            }
        }

        /// <summary>
        /// BC银行卡代付
        /// </summary>
        /// <param name="transfer">具体参考初始化BCTransferWithBackCard</param>
        /// <returns></returns>
        public static BCTransferWithBackCard BCBankCardTransfer(BCTransferWithBackCard transfer)
        {
            string transferUrl = BCPrivateUtil.getHost() + BCConstants.version + BCConstants.bctransferURL;

            string paraString = prepareBCTransferWithBankCard(transfer);

            try
            {
                HttpWebResponse response = BCPrivateUtil.CreatePostHttpResponse(transferUrl, paraString, BCCache.Instance.networkTimeout);
                string respString = BCPrivateUtil.GetResponseString(response);
                return handleBCTransferWithBankCardResult(respString, transfer);
            }
            catch (Exception e)
            {
                var ex = new BCException(e.Message);
                throw ex;
            }
        }
        #endregion

        #region BeeCloud_CJ企业打款

        public static BCCJTransferWithBackCard BCCJBankCardTransfer(BCCJTransferWithBackCard transfer)
        {
            string transferUrl = BCPrivateUtil.getHost() + BCConstants.version + BCConstants.cjtransferURL;
            long timestamp = BCUtil.GetTimeStamp(DateTime.Now);

            JsonData data = new JsonData();
            data["app_id"] = BCCache.Instance.appId;
            data["app_sign"] = BCPrivateUtil.getAppSignatureByMasterSecret(BCCache.Instance.appId, BCCache.Instance.masterSecret, timestamp.ToString());
            data["timestamp"] = timestamp;

            data["total_fee"] = transfer.totalFee;
            data["bill_no"] = transfer.billNo;
            data["title"] = transfer.title;
            data["bank_branch"] = transfer.bankBranch;
            data["bank_name"] = transfer.bankName;
            data["province"] = transfer.province;
            data["city"] = transfer.city;
            data["card_type"] = transfer.cardType;
            data["card_attribute"] = transfer.cardAttribute;
            data["bank_account_no"] = transfer.bankAccountNo;
            data["account_name"] = transfer.accountName;

            if (transfer.optional != null && transfer.optional.Count > 0)
            {
                data["optional"] = new JsonData();
                foreach (string key in transfer.optional.Keys)
                {
                    data["optional"][key] = transfer.optional[key];
                }
            }

            string paraString = data.ToJson();
            try
            {
                HttpWebResponse response = BCPrivateUtil.CreatePostHttpResponse(transferUrl, paraString, BCCache.Instance.networkTimeout);
                string respString = BCPrivateUtil.GetResponseString(response);
                JsonData responseData = JsonMapper.ToObject(respString);

                if (responseData["result_code"].ToString() == "0")
                {
                    transfer.id = responseData["id"].ToString();
                    return transfer;
                }
                else
                {
                    var ex = new BCException(responseData["err_detail"].ToString());
                    throw ex;
                }
            }
            catch (Exception e)
            {
                var ex = new BCException(e.Message);
                throw ex;
            }
        }
        #endregion

        #region BeeCloud订阅
        /// <summary>
        /// 给用户发送验证码
        /// </summary>
        /// <param name="phone">用户电话</param>
        /// <returns></returns>
        public static string sendSMS(string phone)
        {
            long timestamp = BCUtil.GetTimeStamp(DateTime.Now);

            JsonData data = new JsonData();
            data["app_id"] = BCCache.Instance.appId;
            data["app_sign"] = BCPrivateUtil.getAppSignature(BCCache.Instance.appId, BCCache.Instance.appSecret, timestamp.ToString());
            data["timestamp"] = timestamp;
            data["phone"] = phone;
            string paraString = data.ToJson();

            string smsURL = BCPrivateUtil.getHost() + BCConstants.version + BCConstants.bcsendSMSURL;

            try
            {
                HttpWebResponse response = BCPrivateUtil.CreatePostHttpResponse(smsURL, paraString, BCCache.Instance.networkTimeout);

                string respString = BCPrivateUtil.GetResponseString(response);

                JsonData responseData = JsonMapper.ToObject(respString);

                if (responseData["result_code"].ToString() == "0")
                {
                    return responseData["sms_id"].ToString();
                }
                else
                {
                    var ex = new BCException(responseData["err_detail"].ToString());
                    throw ex;
                }
            }
            catch (Exception e)
            {
                var ex = new BCException(e.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 获取所有支持订阅的银行列表
        /// </summary>
        /// <returns></returns>
        public static List<string> getBanks()
        {
            long timestamp = BCUtil.GetTimeStamp(DateTime.Now);

            //JsonData data = new JsonData();
            //data["app_id"] = BCCache.Instance.appId;
            //data["app_sign"] = BCPrivateUtil.getAppSignature(BCCache.Instance.appId, BCCache.Instance.appSecret, timestamp.ToString());
            //data["timestamp"] = timestamp;
            //string paraString = data.ToJson();

            string banksURL = BCPrivateUtil.getHost() + BCConstants.version + BCConstants.bcsubscriptionbanksURL;
            banksURL = banksURL + "?app_id=" + BCCache.Instance.appId + "&app_sign=" + BCPrivateUtil.getAppSignature(BCCache.Instance.appId, BCCache.Instance.appSecret, timestamp.ToString()) + "&timestamp=" + timestamp;

            try
            {
                HttpWebResponse response = BCPrivateUtil.CreateGetHttpResponse(banksURL, BCCache.Instance.networkTimeout);

                string respString = BCPrivateUtil.GetResponseString(response);

                JsonData responseData = JsonMapper.ToObject(respString);

                if (responseData["result_code"].ToString() == "0")
                {
                    List<string> banks = new List<string>();
                    if (responseData["banks"].IsArray)
                    {
                        foreach (JsonData bankData in responseData["banks"])
                        {
                            banks.Add(bankData.ToString());
                        }
                        return banks;
                    }
                    else
                    {
                        return banks;
                    }
                }
                else
                {
                    var ex = new BCException(responseData["err_detail"].ToString());
                    throw ex;
                }
            }
            catch (Exception e)
            {
                var ex = new BCException(e.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 获取常用的支持订阅的银行列表
        /// </summary>
        /// <returns></returns>
        public static List<string> getCommonBanks()
        {
            long timestamp = BCUtil.GetTimeStamp(DateTime.Now);

            //JsonData data = new JsonData();
            //data["app_id"] = BCCache.Instance.appId;
            //data["app_sign"] = BCPrivateUtil.getAppSignature(BCCache.Instance.appId, BCCache.Instance.appSecret, timestamp.ToString());
            //data["timestamp"] = timestamp;
            //string paraString = data.ToJson();

            string banksURL = BCPrivateUtil.getHost() + BCConstants.version + BCConstants.bcsubscriptionbanksURL;
            banksURL = banksURL + "?app_id=" + BCCache.Instance.appId + "&app_sign=" + BCPrivateUtil.getAppSignature(BCCache.Instance.appId, BCCache.Instance.appSecret, timestamp.ToString()) + "&timestamp=" + timestamp;

            try
            {
                HttpWebResponse response = BCPrivateUtil.CreateGetHttpResponse(banksURL, BCCache.Instance.networkTimeout);

                string respString = BCPrivateUtil.GetResponseString(response);

                JsonData responseData = JsonMapper.ToObject(respString);

                if (responseData["result_code"].ToString() == "0")
                {
                    List<string> banks = new List<string>();
                    if (responseData["common_banks"].IsArray)
                    {
                        foreach (JsonData bankData in responseData["common_banks"])
                        {
                            banks.Add(bankData.ToString());
                        }
                        return banks;
                    }
                    else
                    {
                        return banks;
                    }
                }
                else
                {
                    var ex = new BCException(responseData["err_detail"].ToString());
                    throw ex;
                }
            }
            catch (Exception e)
            {
                var ex = new BCException(e.Message);
                throw ex;
            }
        }

        

        /// <summary>
        /// 创建订阅计划
        /// </summary>
        /// <param name="plan">设置计划参数</param>
        /// <returns></returns>
        public static BCPlan createPlan(BCPlan plan)
        {
            long timestamp = BCUtil.GetTimeStamp(DateTime.Now);

            JsonData data = new JsonData();
            data["app_id"] = BCCache.Instance.appId;

            data["app_sign"] = BCPrivateUtil.getAppSignature(BCCache.Instance.appId, BCCache.Instance.appSecret, timestamp.ToString());

            data["timestamp"] = timestamp;
            data["fee"] = plan.fee;
            data["interval"] = plan.interval;
            data["name"] = plan.name;
            data["currency"] = plan.currency;
            if (plan.intervalCount != null)
            {
                data["interval_count"] = plan.intervalCount;
            }
            if (plan.trialDays != null)
            {
                data["trial_days"] = plan.trialDays;
            }
            if (plan.valid != null)
            {
                data["valid"] = plan.valid;
            }
            if (plan.optional != null && plan.optional.Count > 0)
            {
                data["optional"] = new JsonData();
                foreach (string key in plan.optional.Keys)
                {
                    data["optional"][key] = plan.optional[key];
                }
            }

            string paraString = data.ToJson();

            string planURL = BCPrivateUtil.getHost() + BCConstants.version + BCConstants.bcplanURL;

            try
            {
                HttpWebResponse response = BCPrivateUtil.CreatePostHttpResponse(planURL, paraString, BCCache.Instance.networkTimeout);

                string respString = BCPrivateUtil.GetResponseString(response);

                JsonData responseData = JsonMapper.ToObject(respString);

                if (responseData["result_code"].ToString() == "0")
                {
                    plan.ID = responseData["plan"]["id"].ToString();
                    plan.currency = responseData["plan"]["currency"].ToString();
                    plan.intervalCount = (int)responseData["plan"]["interval_count"];
                    plan.trialDays = (int)responseData["plan"]["trial_days"];
                    plan.valid = (bool)responseData["plan"]["valid"];
                    return plan;
                }
                else
                {
                    var ex = new BCException(responseData["err_detail"].ToString());
                    throw ex;
                }
            }
            catch (Exception e)
            {
                var ex = new BCException(e.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 按条件查询订阅计划
        /// </summary>
        /// <param name="nameWithSubstring">订阅计划名</param>
        /// <param name="interval">订阅周期</param>
        /// <param name="intervalCount">周期长度</param>
        /// <param name="trialDays">订阅发生时间与实际扣款时间之间的时长</param>
        /// <param name="createdBefore">创建时间前</param>
        /// <param name="createdAfter">创建时间后</param>
        /// <param name="skip">跳过数量</param>
        /// <param name="limit">查询限量</param>
        /// <param name="countOnly">设置为true时只返回数量，设置为false时只返回plan记录</param>
        /// <returns></returns>
        /// 
        public static List<BCPlan> queryPlansByCondition(string nameWithSubstring, string interval, int? intervalCount, int? trialDays, long? createdBefore, long? createdAfter, int? skip, int? limit, bool countOnly)
        {
            long timestamp = BCUtil.GetTimeStamp(DateTime.Now);

            string planURL = BCPrivateUtil.getHost() + BCConstants.version + BCConstants.bcplanURL;
            planURL += "?app_id=" + BCCache.Instance.appId + "&app_sign=" + BCPrivateUtil.getAppSignature(BCCache.Instance.appId, BCCache.Instance.appSecret, timestamp.ToString()) + "&timestamp=" + timestamp;
            if (nameWithSubstring != null)
            {
                planURL += "&name_with_substring=" + nameWithSubstring;
            }
            if (interval != null)
            {
                planURL += "&interval=" + interval;
            }
            if (intervalCount.HasValue)
            {
                planURL += "&interval_count=" + intervalCount.Value;
            }
            if (trialDays.HasValue)
            {
                planURL += "&trial_days=" + trialDays.Value;
            }
            if (createdBefore.HasValue)
            {
                planURL += "&created_before=" + createdBefore.Value;
            }
            if (createdAfter.HasValue)
            {
                planURL += "&created_after=" + createdAfter.Value;
            }
            if (skip.HasValue)
            {
                planURL += "&skip=" + skip.Value;
            }
            if (limit.HasValue)
            {
                planURL += "&limit=" + limit.Value;
            }
            planURL += "&count_only=" + countOnly;

            try
            {
                HttpWebResponse response = BCPrivateUtil.CreateGetHttpResponse(planURL, BCCache.Instance.networkTimeout);

                string respString = BCPrivateUtil.GetResponseString(response);

                JsonData responseData = JsonMapper.ToObject(respString);

                if (responseData["result_code"].ToString() == "0")
                {
                    List<BCPlan> plans = new List<BCPlan>();
                    if (responseData["plans"].IsArray)
                    {
                        foreach (JsonData planData in responseData["plans"])
                        {
                            BCPlan plan = new BCPlan();
                            plan.ID = planData["id"].ToString();
                            plan.fee = (int)planData["fee"];
                            plan.interval = planData["interval"].ToString();
                            plan.name = planData["name"].ToString();
                            plan.currency = planData["currency"].ToString();
                            plan.intervalCount = (int)planData["interval_count"];
                            plan.trialDays = (int)planData["trial_days"];
                            plan.valid = (bool)planData["valid"];
                            plan.optional = JsonMapper.ToObject<Dictionary<string, string>>(planData["optional"].ToJson().ToString());

                            plans.Add(plan);
                        }
                    }
                    return plans;
                }
                else
                {
                    var ex = new BCException(responseData["err_detail"].ToString());
                    throw ex;
                }
            }
            catch (Exception e)
            {
                var ex = new BCException(e.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 根据ID查询订阅计划
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static BCPlan queryPlanByID(string id)
        {
            long timestamp = BCUtil.GetTimeStamp(DateTime.Now);

            string planURL = BCPrivateUtil.getHost() + BCConstants.version + BCConstants.bcplanURL + "/" + id;
            planURL += "?app_id=" + BCCache.Instance.appId + "&app_sign=" + BCPrivateUtil.getAppSignature(BCCache.Instance.appId, BCCache.Instance.appSecret, timestamp.ToString()) + "&timestamp=" + timestamp;

            try
            {
                HttpWebResponse response = BCPrivateUtil.CreateGetHttpResponse(planURL, BCCache.Instance.networkTimeout);

                string respString = BCPrivateUtil.GetResponseString(response);

                JsonData responseData = JsonMapper.ToObject(respString);

                if (responseData["result_code"].ToString() == "0")
                {
                    BCPlan plan = new BCPlan();
                    plan.ID = responseData["plan"]["id"].ToString();
                    plan.fee = (int)responseData["plan"]["fee"];
                    plan.interval = responseData["plan"]["interval"].ToString();
                    plan.name = responseData["plan"]["name"].ToString();
                    plan.currency = responseData["plan"]["currency"].ToString();
                    plan.intervalCount = (int)responseData["plan"]["interval_count"];
                    plan.trialDays = (int)responseData["plan"]["trial_days"];
                    plan.valid = (bool)responseData["plan"]["valid"];
                    plan.optional = JsonMapper.ToObject<Dictionary<string, string>>(responseData["plan"]["optional"].ToJson().ToString());
                    return plan;
                }
                else
                {
                    var ex = new BCException(responseData["err_detail"].ToString());
                    throw ex;
                }
            }
            catch (Exception e)
            {
                var ex = new BCException(e.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 跟新订阅计划
        /// </summary>
        /// <param name="id">计划ID</param>
        /// <param name="name">计划名</param>
        /// <param name="optional">自定义字段</param>
        /// <returns></returns>
        public static string updatePlan(string id, string name, bool? valid, Dictionary<string, string> optional)
        {
            long timestamp = BCUtil.GetTimeStamp(DateTime.Now);

            JsonData data = new JsonData();
            data["app_id"] = BCCache.Instance.appId;

            data["app_sign"] = BCPrivateUtil.getAppSignature(BCCache.Instance.appId, BCCache.Instance.appSecret, timestamp.ToString());

            data["timestamp"] = timestamp;
            if (name != null)
            {
                data["name"] = name;
            }
            if (valid.HasValue)
            {
                data["valid"] = valid.Value;
            }

            if (optional != null && optional.Count > 0)
            {
                data["optional"] = new JsonData();
                foreach (string key in optional.Keys)
                {
                    data["optional"][key] = optional[key];
                }
            }

            string paraString = data.ToJson();

            string planURL = BCPrivateUtil.getHost() + BCConstants.version + BCConstants.bcplanURL + "/" + id;

            try
            {
                HttpWebResponse response = BCPrivateUtil.CreatePutHttpResponse(planURL, paraString, BCCache.Instance.networkTimeout);

                string respString = BCPrivateUtil.GetResponseString(response);

                JsonData responseData = JsonMapper.ToObject(respString);

                if (responseData["result_code"].ToString() == "0")
                {
                    //BCPlan plan = new BCPlan();
                    //plan.ID = responseData["plan"]["id"].ToString();
                    //plan.fee = (int)responseData["plan"]["fee"];
                    //plan.interval = responseData["plan"]["interval"].ToString();
                    //plan.name = responseData["plan"]["name"].ToString();
                    //plan.currency = responseData["plan"]["currency"].ToString();
                    //plan.intervalCount = (int)responseData["plan"]["interval_count"];
                    //plan.trialDays = (int)responseData["plan"]["trial_days"];
                    //plan.optional = JsonMapper.ToObject<Dictionary<string, string>>(responseData["optional"].ToString());
                    return responseData["id"].ToString();
                }
                else
                {
                    var ex = new BCException(responseData["err_detail"].ToString());
                    throw ex;
                }
            }
            catch (Exception e)
            {
                var ex = new BCException(e.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 删除订阅计划
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string deletePlan(string id)
        {
            long timestamp = BCUtil.GetTimeStamp(DateTime.Now);

            string planURL = BCPrivateUtil.getHost() + BCConstants.version + BCConstants.bcplanURL + "/" + id;
            planURL += "?app_id=" + BCCache.Instance.appId + "&app_sign=" + BCPrivateUtil.getAppSignature(BCCache.Instance.appId, BCCache.Instance.appSecret, timestamp.ToString()) + "&timestamp=" + timestamp;

            try
            {
                HttpWebResponse response = BCPrivateUtil.CreateDeleteHttpResponse(planURL, BCCache.Instance.networkTimeout);

                string respString = BCPrivateUtil.GetResponseString(response);

                JsonData responseData = JsonMapper.ToObject(respString);

                if (responseData["result_code"].ToString() == "0")
                {
                    return id;
                }
                else
                {
                    var ex = new BCException(responseData["err_detail"].ToString());
                    throw ex;
                }
            }
            catch (Exception e)
            {
                var ex = new BCException(e.Message);
                throw ex;
            }
        }



        /// <summary>
        /// 创建订阅记录
        /// </summary>
        /// <param name="smsID">用户短信ID，通过sendSMS方法发送给用户手机时获得</param>
        /// <param name="smsCode">用户短信验证码</param>
        /// <param name="subscription">设置订阅参数</param>
        /// <returns></returns>
        public static BCSubscription createSubscription(string smsID, string smsCode, BCSubscription subscription)
        {
            long timestamp = BCUtil.GetTimeStamp(DateTime.Now);

            JsonData data = new JsonData();
            data["app_id"] = BCCache.Instance.appId;

            data["app_sign"] = BCPrivateUtil.getAppSignature(BCCache.Instance.appId, BCCache.Instance.appSecret, timestamp.ToString());

            data["timestamp"] = timestamp;
            data["sms_id"] = smsID;
            data["sms_code"] = smsCode;
            data["buyer_id"] = subscription.buyerID;
            data["plan_id"] = subscription.planID;
            data["card_id"] = subscription.cardID;
            data["bank_name"] = subscription.bankName;
            data["card_no"] = subscription.cardNo;
            data["id_name"] = subscription.IDName;
            data["id_no"] = subscription.IDNo;
            data["amount"] = subscription.amount;
            data["coupon_id"] = subscription.couponID;
            data["trial_end"] = subscription.trialEnd;
            data["mobile"] = subscription.mobile;

            if (subscription.optional != null && subscription.optional.Count > 0)
            {
                data["optional"] = new JsonData();
                foreach (string key in subscription.optional.Keys)
                {
                    data["optional"][key] = subscription.optional[key];
                }
            }

            string paraString = data.ToJson();

            string subscriptionURL = BCPrivateUtil.getHost() + BCConstants.version + BCConstants.bcsubscriptionURL;

            try
            {
                HttpWebResponse response = BCPrivateUtil.CreatePostHttpResponse(subscriptionURL, paraString, BCCache.Instance.networkTimeout);

                string respString = BCPrivateUtil.GetResponseString(response);

                JsonData responseData = JsonMapper.ToObject(respString);

                if (responseData["result_code"].ToString() == "0")
                {
                    subscription.ID = responseData["subscription"]["id"].ToString();
                    subscription.buyerID = responseData["subscription"]["buyer_id"].ToString();
                    subscription.planID = responseData["subscription"]["plan_id"].ToString();
                    subscription.bankName = responseData["subscription"]["bank_name"].ToString();
                    subscription.IDName = responseData["subscription"]["id_name"].ToString();
                    subscription.IDNo = responseData["subscription"]["id_no"].ToString();
                    subscription.mobile = responseData["subscription"]["mobile"].ToString();
                    subscription.amount = (double)responseData["subscription"]["amount"];
                    subscription.couponID = responseData["subscription"]["coupon_id"].ToString();
                    subscription.trialEnd = (long)responseData["subscription"]["trial_end"];
                    subscription.optional = JsonMapper.ToObject<Dictionary<string, string>>(responseData["subscription"]["optional"].ToJson().ToString());
                    subscription.last4 = responseData["subscription"]["last4"].ToString();
                    subscription.status = responseData["subscription"]["status"].ToString();
                    subscription.valid = (bool)responseData["subscription"]["valid"];
                    subscription.cancelAtPeriodEnd = (bool)responseData["subscription"]["cancel_at_period_end"];
                    return subscription;
                }
                else
                {
                    var ex = new BCException(responseData["err_detail"].ToString());
                    throw ex;
                }
            }
            catch (Exception e)
            {
                var ex = new BCException(e.Message);
                throw ex;
            }
        }


        /// <summary>
        /// 按条件查询用户订阅
        /// </summary>
        /// <param name="buyerID">用户ID</param>
        /// <param name="planID">订阅计划ID</param>
        /// <param name="cardID">用户卡ID</param>
        /// <param name="createdBefore">创建时间前</param>
        /// <param name="createdAfter">创建时间后</param>
        /// <param name="skip">跳过数量</param>
        /// <param name="limit">查询限量</param>
        /// <param name="countOnly">设置为true时只返回数量，设置为false时只返回plan记录</param>
        /// <returns></returns>
        public static List<BCSubscription> querySubscriptionsByCondition(string buyerID, string planID, string cardID, long? createdBefore, long? createdAfter, int? skip, int? limit, bool countOnly)
        {
            long timestamp = BCUtil.GetTimeStamp(DateTime.Now);

            string subscriptionURL = BCPrivateUtil.getHost() + BCConstants.version + BCConstants.bcsubscriptionURL;
            subscriptionURL += "?app_id=" + BCCache.Instance.appId + "&app_sign=" + BCPrivateUtil.getAppSignature(BCCache.Instance.appId, BCCache.Instance.appSecret, timestamp.ToString()) + "&timestamp=" + timestamp;
            if (buyerID != null)
            {
                subscriptionURL += "&buyer_id=" +　buyerID;
            }
            if (planID != null)
            {
                subscriptionURL += "&plan_id=" + planID;
            }
            if (cardID != null)
            {
                subscriptionURL += "&card_id=" + cardID;
            }
            if (createdBefore.HasValue)
            {
                subscriptionURL += "&created_before=" + createdBefore.Value;
            }
            if (createdAfter.HasValue)
            {
                subscriptionURL += "&created_after=" + createdAfter.Value;
            }
            if (skip.HasValue)
            {
                subscriptionURL += "&skip=" + skip.Value;
            }
            if (limit.HasValue)
            {
                subscriptionURL += "&limit=" + limit.Value;
            }
            subscriptionURL += "&count_only=" + countOnly;

            try
            {
                HttpWebResponse response = BCPrivateUtil.CreateGetHttpResponse(subscriptionURL, BCCache.Instance.networkTimeout);

                string respString = BCPrivateUtil.GetResponseString(response);

                JsonData responseData = JsonMapper.ToObject(respString);

                if (responseData["result_code"].ToString() == "0")
                {
                    List<BCSubscription> subscriptions = new List<BCSubscription>();
                    if (responseData["subscriptions"].IsArray)
                    {
                        foreach (JsonData subData in responseData["subscriptions"])
                        {
                            BCSubscription sub = new BCSubscription();
                            sub.ID = subData["id"].ToString();
                            sub.buyerID = subData["buyer_id"].ToString();
                            sub.planID = subData["plan_id"].ToString();
                            sub.cardID = subData["card_id"].ToString();
                            sub.bankName = subData["bank_name"].ToString();
                            sub.IDName = subData["id_name"].ToString();
                            sub.IDNo = subData["id_no"].ToString();
                            sub.mobile = subData["mobile"].ToString();
                            sub.amount = (double)subData["amount"];
                            sub.couponID = subData["coupon_id"].ToString();
                            sub.trialEnd = (long)subData["trial_end"];
                            sub.optional = JsonMapper.ToObject<Dictionary<string, string>>(subData["optional"].ToJson().ToString());

                            sub.last4 = subData["last4"].ToString();
                            sub.status = subData["status"].ToString();
                            sub.valid = (bool)subData["valid"];
                            sub.cancelAtPeriodEnd = (bool)subData["cancel_at_period_end"];

                            subscriptions.Add(sub);
                        }
                    }
                    return subscriptions;
                }
                else
                {
                    var ex = new BCException(responseData["err_detail"].ToString());
                    throw ex;
                }
            }
            catch (Exception e)
            {
                var ex = new BCException(e.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 按ID查询订阅记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static BCSubscription querySubscriptionByID(string id)
        {
            long timestamp = BCUtil.GetTimeStamp(DateTime.Now);

            string subscriptionURL = BCPrivateUtil.getHost() + BCConstants.version + BCConstants.bcsubscriptionURL + "/" + id;
            subscriptionURL += "?app_id=" + BCCache.Instance.appId + "&app_sign=" + BCPrivateUtil.getAppSignature(BCCache.Instance.appId, BCCache.Instance.appSecret, timestamp.ToString()) + "&timestamp=" + timestamp;

            try
            {
                HttpWebResponse response = BCPrivateUtil.CreateGetHttpResponse(subscriptionURL, BCCache.Instance.networkTimeout);

                string respString = BCPrivateUtil.GetResponseString(response);

                JsonData responseData = JsonMapper.ToObject(respString);

                if (responseData["result_code"].ToString() == "0")
                {
                    BCSubscription sub = new BCSubscription();
                    sub.ID = responseData["subscription"]["id"].ToString();
                    sub.buyerID = responseData["subscription"]["buyer_id"].ToString();
                    sub.planID = responseData["subscription"]["plan_id"].ToString();
                    sub.cardID = responseData["subscription"]["card_id"].ToString();
                    sub.bankName = responseData["subscription"]["bank_name"].ToString();
                    sub.IDName = responseData["subscription"]["id_name"].ToString();
                    sub.IDNo = responseData["subscription"]["id_no"].ToString();
                    sub.mobile = responseData["subscription"]["mobile"].ToString();
                    sub.amount = (double)responseData["subscription"]["amount"];
                    sub.couponID = responseData["subscription"]["coupon_id"].ToString();
                    sub.trialEnd = (long)responseData["subscription"]["trial_end"];
                    sub.optional = JsonMapper.ToObject<Dictionary<string, string>>(responseData["subscription"]["optional"].ToJson().ToString());

                    sub.last4 = responseData["subscription"]["last4"].ToString();
                    sub.status = responseData["subscription"]["status"].ToString();
                    sub.valid = (bool)responseData["subscription"]["valid"];
                    sub.cancelAtPeriodEnd = (bool)responseData["subscription"]["cancel_at_period_end"];
                    return sub;
                }
                else
                {
                    var ex = new BCException(responseData["err_detail"].ToString());
                    throw ex;
                }
            }
            catch (Exception e)
            {
                var ex = new BCException(e.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 跟新订阅记录
        /// </summary>
        /// <param name="id"></param>
        /// <param name="buyerID"></param>
        /// <param name="planID"></param>
        /// <param name="cardID"></param>
        /// <param name="amount"></param>
        /// <param name="coupon"></param>
        /// <param name="trialEnd"></param>
        /// <param name="optional"></param>
        /// <returns></returns>
        public static string updateSubscription(string id, string buyerID, string planID, string cardID, double? amount, string coupon, long? trialEnd, Dictionary<string, string> optional)
        {
            long timestamp = BCUtil.GetTimeStamp(DateTime.Now);

            JsonData data = new JsonData();
            data["app_id"] = BCCache.Instance.appId;

            data["app_sign"] = BCPrivateUtil.getAppSignature(BCCache.Instance.appId, BCCache.Instance.appSecret, timestamp.ToString());

            data["timestamp"] = timestamp;
            if (buyerID != null)
            {
                data["buyer_id"] = buyerID;
            }
            if (cardID != null)
            {
                data["card_id"] = cardID;
            }
            if (planID != null)
            {
                data["plan_id"] = planID;
            }
            if (coupon != null)
            {
                data["coupon"] = coupon;
            }
            if (amount.HasValue)
            {
                data["amount"] = amount.Value;
            }
            if (trialEnd.HasValue)
            {
                data["trial_end"] = trialEnd.Value;
            }

            if (optional != null && optional.Count > 0)
            {
                data["optional"] = new JsonData();
                foreach (string key in optional.Keys)
                {
                    data["optional"][key] = optional[key];
                }
            }

            string paraString = data.ToJson();

            string subscriptionURL = BCPrivateUtil.getHost() + BCConstants.version + BCConstants.bcsubscriptionURL + "/" + id;

            try
            {
                HttpWebResponse response = BCPrivateUtil.CreatePutHttpResponse(subscriptionURL, paraString, BCCache.Instance.networkTimeout);

                string respString = BCPrivateUtil.GetResponseString(response);

                JsonData responseData = JsonMapper.ToObject(respString);

                if (responseData["result_code"].ToString() == "0")
                {
                    return responseData["id"].ToString();
                }
                else
                {
                    var ex = new BCException(responseData["err_detail"].ToString());
                    throw ex;
                }
            }
            catch (Exception e)
            {
                var ex = new BCException(e.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 删除订阅记录
        /// </summary>
        /// <param name="id"></param>
        /// <param name="atPeriodEnd"></param>
        /// <returns></returns>
        public static string deleteSubscription(string id, bool atPeriodEnd)
        {
            long timestamp = BCUtil.GetTimeStamp(DateTime.Now);

            string subscriptionURL = BCPrivateUtil.getHost() + BCConstants.version + BCConstants.bcsubscriptionURL + "/" + id;
            subscriptionURL += "?app_id=" + BCCache.Instance.appId + "&app_sign=" + BCPrivateUtil.getAppSignature(BCCache.Instance.appId, BCCache.Instance.appSecret, timestamp.ToString()) + "&timestamp=" + timestamp + "&at_period_end=" + atPeriodEnd;

            try
            {
                HttpWebResponse response = BCPrivateUtil.CreateDeleteHttpResponse(subscriptionURL, BCCache.Instance.networkTimeout);

                string respString = BCPrivateUtil.GetResponseString(response);

                JsonData responseData = JsonMapper.ToObject(respString);

                if (responseData["result_code"].ToString() == "0")
                {
                    return id;
                }
                else
                {
                    var ex = new BCException(responseData["err_detail"].ToString());
                    throw ex;
                }
            }
            catch (Exception e)
            {
                var ex = new BCException(e.Message);
                throw ex;
            }
        }

        #endregion

        #region 身份实名验证
        /// <summary>
        /// 身份实名验证，支持二要素，三要素，四要素验证
        /// </summary>
        /// <param name="name">身份证姓名</param>
        /// <param name="IDNo">身份证号</param>
        /// <param name="cardNo">用户银行卡号(选填)</param>
        /// <param name="mobile">用户银行卡预留手机号（选填）</param>
        /// <returns></returns>
        public static bool BCAuthentication(string name, string IDNo, string cardNo, string mobile)
        {
            long timestamp = BCUtil.GetTimeStamp(DateTime.Now);

            JsonData data = new JsonData();
            data["app_id"] = BCCache.Instance.appId;
            data["app_sign"] = BCPrivateUtil.getAppSignature(BCCache.Instance.appId, BCCache.Instance.appSecret, timestamp.ToString());
            data["timestamp"] = timestamp;
            data["name"] = name;
            data["id_no"] = IDNo;
            data["card_no"] = cardNo;
            data["mobile"] = mobile;
            string paraString = data.ToJson();

            string authURL = BCPrivateUtil.getHost() + BCConstants.version + BCConstants.authULR;

            try
            {
                HttpWebResponse response = BCPrivateUtil.CreatePostHttpResponse(authURL, paraString, BCCache.Instance.networkTimeout);

                string respString = BCPrivateUtil.GetResponseString(response);

                JsonData responseData = JsonMapper.ToObject(respString);

                if (responseData["result_code"].ToString() == "0")
                {
                    return (bool)responseData["auth_result"];
                }
                else
                {
                    var ex = new BCException(responseData["errMsg"].ToString());
                    throw ex;
                }
            }
            catch (Exception e)
            {
                var ex = new BCException(e.Message);
                throw ex;
            }
        }

        #endregion

        #region 用户系统
        /// <summary>
        /// 在BeeCloud平台注册用户信息，推荐在用户注册的时候同时调用。
        /// </summary>
        /// <param name="buyerID">商户为自己的用户分配的ID。可以是email、手机号、随机字符串等。最长32位。在商户自己系统内必须保证唯一。</param>
        /// <returns></returns>
        public static bool BCUserRegister(string buyerID)
        {
            long timestamp = BCUtil.GetTimeStamp(DateTime.Now);

            JsonData data = new JsonData();
            data["app_id"] = BCCache.Instance.appId;
            data["app_sign"] = BCPrivateUtil.getAppSignature(BCCache.Instance.appId, BCCache.Instance.appSecret, timestamp.ToString());
            data["timestamp"] = timestamp;
            data["buyer_id"] = buyerID;
            string paraString = data.ToJson();

            string userRegisterURL = BCPrivateUtil.getHost() + BCConstants.version + BCConstants.userURL;

            try
            {
                HttpWebResponse response = BCPrivateUtil.CreatePostHttpResponse(userRegisterURL, paraString, BCCache.Instance.networkTimeout);

                string respString = BCPrivateUtil.GetResponseString(response);

                JsonData responseData = JsonMapper.ToObject(respString);

                if (responseData["result_code"].ToString() == "0")
                {
                    return true;
                }
                else
                {
                    var ex = new BCException(responseData["errMsg"].ToString());
                    throw ex;
                }
            }
            catch (Exception e)
            {
                var ex = new BCException(e.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 批量在BeeCloud平台注册用户信息，可以定时导入用户数据时使用。
        /// </summary>
        /// <param name="buyeIDs">用户列表</param>
        /// <returns></returns>
        public static bool BCUsersRegister(List<string> buyeIDs)
        {
            long timestamp = BCUtil.GetTimeStamp(DateTime.Now);

            JsonData data = new JsonData();
            data["app_id"] = BCCache.Instance.appId;
            data["app_sign"] = BCPrivateUtil.getAppSignature(BCCache.Instance.appId, BCCache.Instance.appSecret, timestamp.ToString());
            data["timestamp"] = timestamp;
            data["buyer_ids"] = JsonMapper.ToObject(JsonMapper.ToJson(buyeIDs));
            string paraString = data.ToJson();

            string usersRegisterURL = BCPrivateUtil.getHost() + BCConstants.version + BCConstants.usersURL;

            try
            {
                HttpWebResponse response = BCPrivateUtil.CreatePostHttpResponse(usersRegisterURL, paraString, BCCache.Instance.networkTimeout);

                string respString = BCPrivateUtil.GetResponseString(response);

                JsonData responseData = JsonMapper.ToObject(respString);

                if (responseData["result_code"].ToString() == "0")
                {
                    return true;
                }
                else
                {
                    var ex = new BCException(responseData["errMsg"].ToString());
                    throw ex;
                }
            }
            catch (Exception e)
            {
                var ex = new BCException(e.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 商户用户批量查询接口
        /// </summary>
        /// <param name="email">企业注册邮箱</param>
        /// <param name="startTime">查询时间范围开始时间</param>
        /// <param name="endTime">查询时间范围结束时间</param>
        /// <returns></returns>
        public static List<string> BCUserInfoQuery(string email, DateTime? startTime, DateTime? endTime)
        {
            long timestamp = BCUtil.GetTimeStamp(DateTime.Now);

            JsonData data = new JsonData();
            data["app_id"] = BCCache.Instance.appId;
            data["app_sign"] = BCPrivateUtil.getAppSignature(BCCache.Instance.appId, BCCache.Instance.appSecret, timestamp.ToString());
            data["timestamp"] = timestamp;
            if (email != null)
            {
                data["email"] = email;
            }
            if (startTime != null)
            {
                DateTime time = startTime.Value;
                data["start_time"] = BCUtil.GetTimeStamp(startTime.Value);
            }
            if (endTime != null)
            {
                data["end_time"] = BCUtil.GetTimeStamp(endTime.Value);
            }
            string paraString = data.ToJson();

            string usersQueryURL = BCPrivateUtil.getHost() + BCConstants.version + BCConstants.usersURL + "?para=" + HttpUtility.UrlEncode(paraString, Encoding.UTF8);

            try
            {
                HttpWebResponse response = BCPrivateUtil.CreateGetHttpResponse(usersQueryURL, BCCache.Instance.networkTimeout);

                string respString = BCPrivateUtil.GetResponseString(response);

                JsonData responseData = JsonMapper.ToObject(respString);

                if (responseData["result_code"].ToString() == "0")
                {
                    List<string> users = new List<string>();
                    if (responseData["users"].IsArray)
                    {
                        foreach (JsonData userData in responseData["users"])
                        {
                            users.Add(userData.ToString());
                        }
                        return users;
                    }
                    else
                    {
                        return users;
                    }
                }
                else
                {
                    var ex = new BCException(responseData["err_detail"].ToString());
                    throw ex;
                }
            }
            catch (Exception e)
            {
                var ex = new BCException(e.Message);
                throw ex;
            }
        }
        #endregion

        #region 营销卡券
        /// <summary>
        /// 向客户发放卡券
        /// </summary>
        /// 卡券的创建与支付应用绑定，发放时也需要用和支付应用对应的appid来调用
        /// <param name="templateID">卡券模板ID</param>
        /// <param name="buyerID">用户ID（下单时的buyer_id），用户需要先注册才能下发</param>
        /// <returns>成功发放优惠券的详情</returns>
        public static BCCoupon BCMarketingDeliverCoupons(string templateID, string buyerID)
        {
            long timestamp = BCUtil.GetTimeStamp(DateTime.Now);

            JsonData data = new JsonData();
            data["app_id"] = BCCache.Instance.appId;
            data["app_sign"] = BCPrivateUtil.getAppSignature(BCCache.Instance.appId, BCCache.Instance.appSecret, timestamp.ToString());
            data["timestamp"] = timestamp;
            data["template_id"] = templateID;
            data["user_id"] = buyerID;
            string paraString = data.ToJson();

            string couponURL = BCPrivateUtil.getHost() + BCConstants.version + BCConstants.couponURL;

            try
            {
                HttpWebResponse response = BCPrivateUtil.CreatePostHttpResponse(couponURL, paraString, BCCache.Instance.networkTimeout);

                string respString = BCPrivateUtil.GetResponseString(response);

                JsonData responseData = JsonMapper.ToObject(respString);

                if (responseData["result_code"].ToString() == "0")
                {
                    BCCoupon coupon = new BCCoupon();
                    coupon.id = responseData["coupon"]["id"].ToString();
                    coupon.template.name = responseData["coupon"]["template"]["name"].ToString();
                    coupon.template.limitFee = int.Parse(responseData["coupon"]["template"]["limit_fee"].ToString());
                    if (responseData["coupon"]["template"]["delivery_valid_days"] != null)
                    {
                        coupon.template.deliveryValidDays = int.Parse(responseData["coupon"]["template"]["delivery_valid_days"].ToString());
                    }
                    coupon.template.maxCountPerUser = int.Parse(responseData["coupon"]["template"]["max_count_per_user"].ToString());
                    coupon.template.id = responseData["coupon"]["template"]["id"].ToString();
                    coupon.template.type = int.Parse(responseData["coupon"]["template"]["type"].ToString());
                    coupon.template.useCount = int.Parse(responseData["coupon"]["template"]["use_count"].ToString());
                    coupon.template.expiryType = int.Parse(responseData["coupon"]["template"]["expiry_type"].ToString());
                    coupon.template.deliverCount = int.Parse(responseData["coupon"]["template"]["deliver_count"].ToString());
                    coupon.template.appID = responseData["coupon"]["template"]["app_id"].ToString();
                    coupon.template.discount = int.Parse(responseData["coupon"]["template"]["discount"].ToString());
                    coupon.template.totalCount = int.Parse(responseData["coupon"]["template"]["deliver_count"].ToString());
                    coupon.template.status = int.Parse(responseData["coupon"]["template"]["status"].ToString());
                    coupon.template.mchAccount = responseData["coupon"]["template"]["mch_account"].ToString();
                    if (responseData["coupon"]["template"]["start_time"] != null)
                    {
                        coupon.template.startTime = BCUtil.GetDateTime(long.Parse(responseData["coupon"]["template"]["start_time"].ToString()));
                    }
                    if (responseData["coupon"]["template"]["end_time"] != null)
                    {
                        coupon.template.endTime = BCUtil.GetDateTime(long.Parse(responseData["coupon"]["template"]["end_time"].ToString()));
                    }
                    coupon.buyerID = responseData["coupon"]["user_id"].ToString();
                    coupon.appID = responseData["coupon"]["app_id"].ToString();
                    coupon.status = int.Parse(responseData["coupon"]["status"].ToString());
                    coupon.createdAt = BCUtil.GetDateTime(long.Parse(responseData["coupon"]["created_at"].ToString()));
                    coupon.updatedAt = BCUtil.GetDateTime(long.Parse(responseData["coupon"]["updated_at"].ToString()));
                    if (responseData["coupon"]["start_time"] != null)
                    {
                        coupon.startTime = BCUtil.GetDateTime(long.Parse(responseData["coupon"]["start_time"].ToString()));
                    }
                    if (responseData["coupon"]["end_time"] != null)
                    {
                        coupon.startTime = BCUtil.GetDateTime(long.Parse(responseData["coupon"]["end_time"].ToString()));
                    }
                    if (responseData["coupon"]["use_time"] != null)
                    {
                        coupon.startTime = BCUtil.GetDateTime(long.Parse(responseData["coupon"]["use_time"].ToString()));
                    }
                    coupon.id = responseData["coupon"]["id"].ToString();

                    return coupon;
                }
                else
                {
                    var ex = new BCException(responseData["errMsg"].ToString());
                    throw ex;
                }
            }
            catch (Exception e)
            {
                var ex = new BCException(e.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 查询用户可用优惠券
        /// </summary>
        /// <param name="buyerID">用户ID</param>
        /// <param name="limitFee">返回满足limitFee才能使用的优惠券，直接传订单金额即可</param>
        /// <param name="status">卡券状态，0表示未使用，1表示已使用</param>
        /// <param name="limit">返回卡券数量，默认为10，最大50</param>
        /// <param name="skip">需要跳过的数量，和limit配合选择超过50的数量</param>
        /// <returns></returns>
        public static List<BCCoupon> BCMarketingQueryCoupons(string buyerID, int? limitFee, int? status, int? limit, int? skip)
        {
            long timestamp = BCUtil.GetTimeStamp(DateTime.Now);

            JsonData data = new JsonData();
            data["app_id"] = BCCache.Instance.appId;
            data["app_sign"] = BCPrivateUtil.getAppSignature(BCCache.Instance.appId, BCCache.Instance.appSecret, timestamp.ToString());
            data["timestamp"] = timestamp;
            if (buyerID != null)
            {
                data["user_id"] = buyerID;
            }
            if (limitFee != null)
            {
                data["limit_fee"] = limitFee.Value;
            }
            if (status != null)
            {
                data["status"] = status.Value;
            }
            if (status != null)
            {
                data["limit"] = limit.Value;
            }
            if (status != null)
            {
                data["skip"] = skip.Value;
            }
            string paraString = data.ToJson();

            string conponsQueryURL = BCPrivateUtil.getHost() + BCConstants.version + BCConstants.couponURL + "?para=" + HttpUtility.UrlEncode(paraString, Encoding.UTF8);

            try
            {
                HttpWebResponse response = BCPrivateUtil.CreateGetHttpResponse(conponsQueryURL, BCCache.Instance.networkTimeout);

                string respString = BCPrivateUtil.GetResponseString(response);

                JsonData responseData = JsonMapper.ToObject(respString);

                if (responseData["result_code"].ToString() == "0")
                {
                    List<BCCoupon> coupons = new List<BCCoupon>();
                    if (responseData["coupons"].IsArray)
                    {
                        foreach (JsonData userData in responseData["coupons"])
                        {
                            BCCoupon coupon = new BCCoupon();
                            coupon.id = userData["id"].ToString();
                            coupon.template.name = userData["template"]["name"].ToString();
                            coupon.template.limitFee = int.Parse(userData["template"]["limit_fee"].ToString());
                            if (userData["template"]["delivery_valid_days"] != null)
                            {
                                coupon.template.deliveryValidDays = int.Parse(userData["template"]["delivery_valid_days"].ToString());
                            }
                            coupon.template.maxCountPerUser = int.Parse(userData["template"]["max_count_per_user"].ToString());
                            coupon.template.id = userData["template"]["id"].ToString();
                            coupon.template.type = int.Parse(userData["template"]["type"].ToString());
                            coupon.template.useCount = int.Parse(userData["template"]["use_count"].ToString());
                            coupon.template.expiryType = int.Parse(userData["template"]["expiry_type"].ToString());
                            coupon.template.deliverCount = int.Parse(userData["template"]["deliver_count"].ToString());
                            coupon.template.appID = userData["template"]["app_id"].ToString();
                            coupon.template.discount = int.Parse(userData["template"]["discount"].ToString());
                            coupon.template.totalCount = int.Parse(userData["template"]["deliver_count"].ToString());
                            coupon.template.status = int.Parse(userData["template"]["status"].ToString());
                            coupon.template.mchAccount = userData["template"]["mch_account"].ToString();
                            if (userData["template"]["start_time"] != null)
                            {
                                coupon.template.startTime = BCUtil.GetDateTime(long.Parse(userData["template"]["start_time"].ToString()));
                            }
                            if (userData["template"]["end_time"] != null)
                            {
                                coupon.template.endTime = BCUtil.GetDateTime(long.Parse(userData["template"]["end_time"].ToString()));
                            }
                            coupon.buyerID = userData["user_id"].ToString();
                            coupon.appID = userData["app_id"].ToString();
                            coupon.status = int.Parse(userData["status"].ToString());
                            coupon.createdAt = BCUtil.GetDateTime(long.Parse(userData["created_at"].ToString()));
                            coupon.updatedAt = BCUtil.GetDateTime(long.Parse(userData["updated_at"].ToString()));
                            if (userData["start_time"] != null)
                            {
                                coupon.startTime = BCUtil.GetDateTime(long.Parse(userData["start_time"].ToString()));
                            }
                            if (userData["end_time"] != null)
                            {
                                coupon.startTime = BCUtil.GetDateTime(long.Parse(userData["end_time"].ToString()));
                            }
                            if (userData["use_time"] != null)
                            {
                                coupon.startTime = BCUtil.GetDateTime(long.Parse(userData["use_time"].ToString()));
                            }
                            coupon.id = userData["id"].ToString();

                            coupons.Add(coupon);
                        }
                        return coupons;
                    }
                    else
                    {
                        return coupons;
                    }
                }
                else
                {
                    var ex = new BCException(responseData["err_detail"].ToString());
                    throw ex;
                }
            }
            catch (Exception e)
            {
                var ex = new BCException(e.Message);
                throw ex;
            }
        }
        #endregion
    }
}
