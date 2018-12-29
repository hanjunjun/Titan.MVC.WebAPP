using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Titan.Model.BeeCloud.BCSubscription
{
    public class BCPlan
    {
        public string ID { get; set; }
        //每次收费的金额，以分为单位 (0表示免费计划)
        public int fee { get; set; }
        //定义收费周期单位，只能是day、week、month、year
        public string interval { get; set; }
        //计划名
        public string name { get; set; }
        //ISO货币名, 默认为CNY，表示人民币，目前仅支持CNY
        public string currency { get; set; }
        //和interval共同定义收费周期，例如interval=month interval_count=3，那么每3个月收费一次，最大的收费间隔为1年(1 year, 12 months, or 52 weeks).
        public int? intervalCount { get; set; }
        //试用天数
        public int? trialDays { get; set; }
        //计划是否生效
        public bool valid { get; set; }
        //自定义字段
        public Dictionary<string, string> optional { get; set; }

        public BCPlan() { }

        /// <summary>
        /// 初始化订阅计划
        /// </summary>
        /// <param name="_fee">每次收费的金额，以分为单位 (0表示免费计划)</param>
        /// <param name="_interval">定义收费周期单位，只能是day、week、month、year</param>
        /// <param name="_name">计划名</param>
        public BCPlan(int _fee, string _interval, string _name)
        {
            fee = _fee;
            interval = _interval;
            name = _name;
        }
    }
}
