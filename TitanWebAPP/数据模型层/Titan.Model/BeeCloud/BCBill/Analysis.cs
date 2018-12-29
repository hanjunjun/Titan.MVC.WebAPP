using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Titan.Model.BeeCloud.BCBill
{
    public class Analysis
    {
        //用户购买的产品列表
        public List<Product> product { get; set; }
        //获取的客户端IP，用来分析客户地域分布
        public string ip { get; set; }

    }

    public class Product
    {
        //产品名
        public string name { get; set; }
        //购买数量
        public int count { get; set; }
        //产品单价
        public int price { get; set; }
    }

}
