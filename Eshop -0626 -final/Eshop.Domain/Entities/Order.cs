using System;
using System.Collections.Generic;
using System.Text;
using Eshop.Domain.Entities.Goods;
using Eshop.Domain.Entities.Helpers;

namespace Eshop.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime Date {get; set; }
        public virtual ICollection<GoodQuantity> GoodsQuantities { get; set; }
        public User User { get; set; }
        public int Status { get; set; }
        public Order()
        {
            Status = 1;
            Date = DateTime.UtcNow;
            GoodsQuantities = new List<GoodQuantity>();
        }
    }
}
