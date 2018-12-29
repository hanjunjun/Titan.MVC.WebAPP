namespace Titan.AppService.BeeCloud
{
    internal static class BCConstants
    {
        public const string SDKVersion = "2.8.0";

        public const string version = "/2/";

        //pay & transfer
        public const string billURL = "rest/bill";
        public const string billsURL = "rest/bills";
        public const string billsCountURL = "rest/bills/count";
        public const string refundURL = "rest/refund";
        public const string refundsURL = "rest/refunds";
        public const string refundsCountURL = "rest/refunds/count";
        public const string refundStatusURL = "rest/refund/status";
        public const string transferURL = "rest/transfer";
        public const string transfersURL = "rest/transfers";
        public const string internationalURL = "rest/international/bill";
        public const string bctransferURL = "rest/bc_transfer";
        public const string cjtransferURL = "rest/cj_transfer";
        public const string bctransferBanks = "rest/bc_transfer/banks";

        //subscription API
        public const string bcsendSMSURL = "sms";
        public const string bcplanURL = "plan";
        public const string bcsubscriptionURL ="subscription";
        public const string bcsubscriptionbanksURL = "subscription_banks";

        //auth API
        public const string authULR = "auth";

        //test mode API
        public const string billTestURL = "rest/sandbox/bill";
        public const string billsTestURL = "rest/sandbox/bills";

        //offline API
        public const string offlineBillURL = "rest/offline/bill";
        public const string offlineStatusURL = "rest/offline/bill/status";

        //user system API
        public const string userURL = "rest/user";
        public const string usersURL = "rest/users";
        public const string historyBillsURL = "rest/history_bills";

        //marketing system API
        public const string couponURL = "rest/coupon";
        public const string couponTemplateURL = "rest/coupon/template";
    }
}
