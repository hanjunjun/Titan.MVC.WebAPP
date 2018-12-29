using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Titan.Model.BeeCloud.BCSubscription
{
    public class BCSubscription
    {
        public string ID { get; set; }
        //订阅者ID，可以是用户email，也可以是商户系统中的用户ID
        public string buyerID { get; set; }
        //订阅计划ID
        public string planID { get; set; }
        //用户卡ID
        public string cardID { get; set; }
        //订阅用户银行名称
        public string bankName { get; set; }
        //订阅用户银行卡号
        public string cardNo { get; set; }
        //订阅用户身份证姓名
        public string IDName { get; set; }
        //订阅用户身份证号
        public string IDNo { get; set; }
        //订阅用户银行预留手机号
        public string mobile { get; set; }
        //订阅数量
        public double amount { get; set; }
        //优惠券id
        public string couponID { get; set; }
        //试用截止时间点
        public long trialEnd { get; set; }
        //自定义字段
        public Dictionary<string, string> optional { get; set; }

        //订阅是否生效
        public bool valid { get; set; }
        //订阅状态
        public string status { get; set; }
        //银行卡号后4位
        public string last4 { get; set; }
        public bool cancelAtPeriodEnd { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        public BCSubscription() { }

        /// <summary>
        /// 已有cardID时的初始化方法
        /// </summary>
        /// <param name="_buyerID">购买者ID</param>
        /// <param name="_planID">订阅计划ID</param>
        /// <param name="_cardID">用户卡ID</param>
        public BCSubscription(string _buyerID, string _planID, string _cardID)
        {
            buyerID = _buyerID;
            planID = _planID;
            cardID = _cardID;
        }

        /// <summary>
        /// 只知道用户卡号等信息时的初始化方法
        /// </summary>
        /// <param name="_buyerID">购买者ID</param>
        /// <param name="_planID">订阅计划ID</param>
        /// <param name="_bankName">银行名称，可以通过getCommonBanks/getBanks方法获取</param>
        /// <param name="_cardNo">卡号</param>
        /// <param name="_IDName">卡持有人的姓名</param>
        /// <param name="_IDNo">卡持有人的身份证号</param>
        /// <param name="_mobile">持卡人预留银行卡的手机号</param>
        public BCSubscription(string _buyerID, string _planID, string _bankName, string _cardNo, string _IDName, string _IDNo, string _mobile)
        {
            buyerID = _buyerID;
            planID = _planID;
            bankName = _bankName;
            cardNo = _cardNo;
            IDName = _IDName;
            IDNo = _IDNo;
            mobile = _mobile;
        }
    }
}
