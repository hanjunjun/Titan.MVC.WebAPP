using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Titan.Model.BeeCloud.BCTransferWithBankCard
{
    public class BCTransferWithBackCard
    {
        public BCTransferWithBackCard() { }

        /// <summary>
        /// 初始化下发参数
        /// </summary>
        /// <param name="_totalFee">代付订单总金额</param>
        /// <param name="_billNo">商户订单号</param>
        /// <param name="_title">下发订单标题</param>
        /// <param name="_tradeSource">交易源(UTF8编码格式，目前只能填写OUT_PC)</param>
        /// <param name="_bankFullName"> 银行全名(中国银行，而不能写成"中行")</param>
        /// <param name="_cardType">银行卡类型(DE代表借记卡，CR代表信用卡，其他值为非法)</param>
        /// <param name="_accountType"> 收款帐户类型(帐户类型，P代表私户，C代表公户，其他值为非法)</param>
        /// <param name="_accountNo">收款帐户号(收款方的银行卡号)</param>
        /// <param name="_accountName">收款帐户名称(收款方的姓名或者单位名)</param>
        public BCTransferWithBackCard(int _totalFee, string _billNo, string _title, string _tradeSource, string _bankFullName, string _cardType, string _accountType, string _accountNo, string _accountName)
        {
            totalFee = _totalFee;
            billNo = _billNo;
            title = _title;
            tradeSource = _tradeSource;
            bankFullName = _bankFullName;
            cardType = _cardType;
            accountType = _accountType;
            accountNo = _accountNo;
            accountName = _accountName;
        }

        public string id { get; set; }
        /// <summary>
        /// 代付订单总金额
        /// </summary>
        public int totalFee { get; set; }
        /// <summary>
        /// 商户订单号
        /// </summary>
        public string billNo { get; set; }
        /// <summary>
        /// 下发订单标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 交易源(UTF8编码格式，目前只能填写OUT_PC)
        /// </summary>
        public string tradeSource { get; set; }
        /// <summary>
        /// 银行全名(中国银行，而不能写成"中行",因为“中行”也是中信银行和中兴银行的缩写)，可以通过方法getBankFullNames()获得
        /// </summary>
        public string bankFullName { get; set; }
        /// <summary>
        /// 银行卡类型(DE代表借记卡，CR代表信用卡，其他值为非法)
        /// </summary>
        public string cardType { get; set; }
        /// <summary>
        /// 收款帐户类型(帐户类型，P代表私户，C代表公户，其他值为非法)
        /// </summary>
        public string accountType { get; set; }
        /// <summary>
        /// 收款帐户号(收款方的银行卡号)
        /// </summary>
        public string accountNo { get; set; }
        /// <summary>
        /// 收款帐户名称(收款方的姓名或者单位名)
        /// </summary>
        public string accountName { get; set; }
        /// <summary>
        /// 银行绑定的手机号(银行绑定的手机号，当需要手机收到银行入账信息时，填写该值，前提是该手机在银行有短信通知业务，否则收不到银行信息)
        /// </summary>
        public string mobile { get; set; }
        /// <summary>
        /// 附加数据(用户自定义的参数，将会在Webhook通知中原样返回，该字段主要用于商户携带的自定义数据)
        /// </summary>
        public Dictionary<string, string> optional { get; set; }

        public DateTime createTime { get; set; }
    }
}
