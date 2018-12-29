using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Titan.Model.BeeCloud.BCTransferWithBankCard
{
    public class BCCJTransferWithBackCard
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="_accountName">收款帐户名称(收款方的姓名或者单位名)</param>
        /// <param name="_bankName">
        /// 银行全名
        ///     中国工商银行
        ///     中国农业银行
        ///     中国银行
        ///     中国建设银行
        ///     交通银行
        ///     招商银行
        ///     中国民生银行 
        ///     中国光大银行 
        ///     兴业银行
        ///     中国邮政储蓄银行
        ///     广发银行
        ///     上海浦东发展银行
        ///     浦发银行
        ///     华夏银行
        ///  </param>
        /// <param name="_bankAccountNo">收款帐户类型(帐户类型，C代表私户，B代表公户，其他值为非法)</param>
        /// <param name="_bankBranch">支行名称</param>
        /// <param name="_province">省</param>
        /// <param name="_city">市</param>
        /// <param name="_cardType">银行卡类型(DEBIT代表借记卡，CREDIT代表信用卡，其他值为非法)</param>
        /// <param name="_cardAttribute">收款帐户类型(帐户类型，C代表私户，B代表公户，其他值为非法)</param>
        public BCCJTransferWithBackCard(string _billNo, string _title, int _totalFee, string _accountName, string _bankName, string _bankAccountNo, string _bankBranch, string _province, string _city, string _cardType, string _cardAttribute)
        {
            title = _title;
            billNo = _billNo;
            totalFee = _totalFee;
            accountName = _accountName;
            bankName = _bankName;
            bankAccountNo = _bankAccountNo;
            bankBranch = _bankBranch;
            province = _province;
            city = _city;
            cardType = _cardType;
            cardAttribute = _cardAttribute;
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
        /// 银行全名(中国银行，而不能写成"中行",因为“中行”也是中信银行和中兴银行的缩写)
        ///  中国工商银行
        ///  中国农业银行
        ///  中国银行
        ///  中国建设银行
        ///  交通银行
        ///  招商银行
        ///  中国民生银行 
        ///  中国光大银行 
        ///  兴业银行
        ///  中国邮政储蓄银行
        ///  广发银行
        ///  上海浦东发展银行
        ///  浦发银行
        ///  华夏银行
        /// </summary>
        public string bankName { get; set; }
        /// <summary>
        /// 支行名称
        /// </summary>
        public string bankBranch { get; set; }
        /// <summary>
        /// 省
        /// </summary>
        public string province { get; set; }
        /// <summary>
        /// 市
        /// </summary>
        public string city { get; set; }
        /// <summary>
        /// 银行卡类型(DEBIT代表借记卡，CREDIT代表信用卡，其他值为非法)
        /// </summary>
        public string cardType { get; set; }
        /// <summary>
        /// 收款帐户类型(帐户类型，C代表私户，B代表公户，其他值为非法)
        /// </summary>
        public string cardAttribute { get; set; }
        /// <summary>
        /// 收款帐户号(收款方的银行卡号)
        /// </summary>
        public string bankAccountNo { get; set; }
        /// <summary>
        /// 收款帐户名称(收款方的姓名或者单位名)
        /// </summary>
        public string accountName { get; set; }
        /// <summary>
        /// 附加数据(用户自定义的参数，将会在Webhook通知中原样返回，该字段主要用于商户携带的自定义数据)
        /// </summary>
        public Dictionary<string, string> optional { get; set; }

        public DateTime createTime { get; set; }
    }
}
