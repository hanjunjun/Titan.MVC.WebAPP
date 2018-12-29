using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Titan.Model.BeeCloud.BCBill
{
    public enum CardType
    {
        visa,
        mastercard,
        discover,
        amex
    };

    public class BCCreditCardInfo
    {
        //由于json序列化后服务端需要蛇形命名，所以这里用蛇形命名
        /// <summary>
        /// 卡号
        /// </summary>
        public string card_number { get; set; }
        /// <summary>
        /// 过期时间中的月
        /// </summary>
        public int expire_month { get; set; }
        /// <summary>
        /// 过期时间中的年
        /// </summary>
        public int expire_year { get; set; }
        /// <summary>
        /// 信用卡的三位cvv码
        /// </summary>
        public int cvv { get; set; }
        /// <summary>
        /// 用户名字
        /// </summary>
        public string first_name { get; set; }
        /// <summary>
        /// 用户的姓
        /// </summary>
        public string last_name { get; set; }
        /// <summary>
        /// 卡类别 visa/mastercard/discover/amex
        /// </summary>
        public string card_type { get; set; }
    }
}
