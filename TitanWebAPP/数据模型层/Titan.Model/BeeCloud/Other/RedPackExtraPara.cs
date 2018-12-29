
namespace Titan.Model.BeeCloud.Other
{
    public class RedPackExtraPara
    {
        public string appId { get; set; }
        public string appSign { get; set; }
        public string mch_billno { get; set; }
        public string re_openid { get; set; }
        private int? _total_amount = null;
        public int? total_amount 
        {
            get 
            { 
                return _total_amount; 
            }
            set 
            {
                _total_amount = value;
            } 
        }
        public string nick_name { get; set; }
        public string send_name { get; set; }
        public string wishing { get; set; }
        public string remark { get; set; }
        public string act_name { get; set; }
        private int? _count_per_user = null;
        public int? count_per_user 
        {
            get
            {
                return _count_per_user;
            }
            set
            {
                _count_per_user = value;
            }
        }
        private int? _min = null;
        public int? min 
        {
            get
            {
                return _min; 
            }
            set
            {
                _min = value;
            }
        }
        private int? _max = null;
        public int? max 
        {
            get
            {
                return _max;
            }
            set
            {
                _max = value;
            }
        }
        private double? _probability = null;
        public double? probability 
        {
            get
            {
                return _probability;
            }
            set
            {
                _probability = value;
            }
        }
        private long? _period = null;
        public long? period 
        {
            get
            {
                return _period;
            }
            set
            {
                _period = value;
            }
        }
    }
}
