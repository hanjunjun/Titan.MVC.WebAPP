
namespace Titan.Model.BeeCloud.Other
{
    public class MchPayPara
    {
        public string appId            { get; set; }
        public string appSign          { get; set; }
        public string partner_trade_no { get; set; }
        public string openid           { get; set; }
        public int    amount           { get; set; }
        public string check_name       { get; set; }
        public string re_user_name     { get; set; }
        public string desc             { get; set; }
    }
}
