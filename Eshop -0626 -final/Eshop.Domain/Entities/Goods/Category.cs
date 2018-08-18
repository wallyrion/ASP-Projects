using System;
using System.Collections.Generic;
using System.Text;

namespace Eshop.Domain.Entities.Goods
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get;set; }
        public virtual ICollection<Good> Goods { get; set; }
        public virtual ICollection<Property> Properties { get; set; }

        public Category()
        {
            Goods = new List<Good>();
            Properties = new List<Property>();
        }
    }
}
