using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Eshop.Domain.Entities.Goods;

namespace Eshop.Models
{
 
    public class GoodView
    {
        public int Id {get; set; }
        public string Name { get; set; }
        public int Price { get; set; }

        public GoodView()
        {
        }

        public GoodView(Good good)
        {
            Id = good.Id;
            Name = good.Name;
            Price = good.Price;
        }
    }
}