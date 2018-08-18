using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Eshop.Domain.Entities.Goods;

namespace Eshop.Models
{
    public class IndexViewModel
    {
        public List<Property> Properties { get; set; }
        public string CategoryName { get; set; }
        public int PageSize { get; set; }

        public IndexViewModel()
        {
        }
    }
}