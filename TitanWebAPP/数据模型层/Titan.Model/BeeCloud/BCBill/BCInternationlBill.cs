using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Titan.Model.BeeCloud.BCBill
{
    public class BCInternationlBill
    {
        public BCInternationlBill() { }
        public BCInternationlBill(string _channel, int _totalFee, string _billNo, string _title, string _currency)
        {
            channel = _channel;
            totalFee = _totalFee;
            billNo = _billNo;
            title = _title;
            currency = _currency;
        }

        public string channel { get; set; }
        public int totalFee { get; set; }
        public string billNo { get; set; }
        public string title { get; set; }
        public string currency { get; set; }
        public BCCreditCardInfo info { get; set; }
        public string creditCardId { get; set; }
        public string returnUrl { get; set; }
        public string url { get; set; }
    }
}
