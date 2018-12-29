using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Titan.Model.BeeCloud.BCTransferWithBankCard
{
    public class BankList
    {
        /// <summary>
        /// 支持的银行数量
        /// </summary>
        public int size { get; set; }
        /// <summary>
        /// 银行全称列表
        /// </summary>
        public List<string> bankList { get; set; }
    }
}
