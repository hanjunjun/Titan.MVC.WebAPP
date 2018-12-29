using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Titan.Model.CommonModel
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public double Price { get; set; }
        public int Count { get; set; }
    }
}