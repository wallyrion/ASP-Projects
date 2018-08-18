using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Eshop.Domain.Entities.Helpers
{
    public class GoodQuantity
    {
        [Key]
        public int Id { get; set; }
        public int PurchasedGoodId { get; set; }
        public int GoodCount { get; set; }
        public int TotalPrice { get; set; }
        public string GoodName { get; set; }
        public Order Order { get; set; }

        public GoodQuantity()
        {

        }
    }
}
