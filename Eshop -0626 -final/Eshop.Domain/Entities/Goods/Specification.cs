using System;
using System.Collections.Generic;
using System.Text;

namespace Eshop.Domain.Entities.Goods
{
    public class Specification
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Property Property { get; set; }
        public virtual ICollection<Good> Goods{get; set;}
        public Specification()
        {
            Goods = new List<Good>();
        }


    }
}
