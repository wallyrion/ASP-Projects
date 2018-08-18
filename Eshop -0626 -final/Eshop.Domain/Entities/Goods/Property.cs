using System;
using System.Collections.Generic;
using System.Text;

namespace Eshop.Domain.Entities.Goods
{
    public class Property
    {
        public int Id { get; set; }
        public Category Category { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Specification> Specifications { get; set; }

        public Property()
        {
            Specifications = new List<Specification>();
        }
    }
}
