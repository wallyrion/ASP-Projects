using System;
using System.Collections.Generic;
using System.Text;

namespace Eshop.Domain.Entities.Goods
{
    public class Cart
    {
        public int Id { get; set; }
        public User User {get; set; }
        public virtual ICollection<Good> Goods { get; set; }

        public Cart()
        {
            Goods = new List<Good>();
        }
    }
}
