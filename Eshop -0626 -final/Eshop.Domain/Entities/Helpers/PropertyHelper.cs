using System;
using System.Collections.Generic;
using System.Text;
using Eshop.Domain.Entities.Goods;

namespace Eshop.Domain.Entities.Helpers
{
    class PropertyHelper
    {
        public string Name { get; set; }
        public List<Specification> Specifications { get; set; }
    }
}
